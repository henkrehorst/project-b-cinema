﻿using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace bioscoop_app.Controller
{
    /// <summary>
    /// Controller for the requests related to orders.
    /// </summary>
    class OrderController : ChromelyController
    {
        private Func<List<Product>, List<Ticket>> filterTickets = sequence =>
        {
            return (from product in sequence
                    where product.GetType() == typeof(Ticket)
                    select (Ticket)product).ToList();
        };
        /// <summary>
        /// Searches the order with the specified code in the data, and returns it.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>HttpStatus 200 and the order data if found, 204 if not found</returns>
        [HttpPost(Route = "/order#fetch")]
        public ChromelyResponse FetchOrder(ChromelyRequest req)
        {
            var data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());

            try
            {
                string code = data["code"].Value<string>();
                var orders = new Repository<Order>().Data.Values.AsQueryable<Order>();
                IEnumerable<Order> queryResult = from order in orders
                              where order.code == code
                              select order;
                Order result = queryResult.First();
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(result)
                }.ChromelyWrapper(req.Id);
            } catch (InvalidOperationException)
            {
                return new Response
                {
                    status = 204,
                    statusText = "No order was found for the given code."
                }.ChromelyWrapper(req.Id);
            }
        }

        /// <summary>
        /// Creates an order with the provided products and customer data.
        /// Writes the order to the data file then returns it to the front-end.
        /// </summary>
        /// <param name="req">HttpPOST request containing product and customer data.</param>
        /// <returns>The created order.</returns>
        [HttpPost(Route = "/order#create")]
        public ChromelyResponse CreateOrder(ChromelyRequest req)
        {
            var data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            List<Product> products = ParseItems(data["items"].Value<JArray>());
            Order order = new Order(
                    products,
                    data["cust_name"].Value<string>(),
                    data["cust_email"].Value<string>()
                );
            SetSeatsAvailability(filterTickets(order.items), true);
            new Repository<Order>().AddThenWrite(order);
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(order.code)
            }.ChromelyWrapper(req.Id);
        }

        /// <summary>
        /// Updates the order associated with the specified id, with the specified data.
        /// </summary>
        /// <param name="req">http POST request containing the id and data</param>
        /// <returns>Status code indicating success or failure.</returns>
        [HttpPost(Route = "/reserveringen#update")]
        public ChromelyResponse UpdateOrder(ChromelyRequest req)
        {
            //throw new NotImplementedException();
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            Repository<Order> repository = new Repository<Order>();
            try
            {
                int orderId = data["id"].Value<int>();
                Order input = new Order(
                        orderId,
                        ParseItems(data["items"].Value<JArray>()),
                        data["code"].Value<string>(),
                        data["cust_name"].Value<string>(),
                        data["cust_email"].Value<string>()
                    );
                if (!input.items.OrderBy(p => p.Id).SequenceEqual(repository.Data[orderId].items.OrderBy(p => p.Id)))
                {
                    //Backend magic to change availability of items
                    List<Ticket> inputTickets = filterTickets(input.items);
                    List<Ticket> existingTickets = filterTickets(repository.Data[orderId].items);
                    if (inputTickets.Any() && !existingTickets.Any())
                    {
                        //Reserve tickets
                        SetSeatsAvailability(inputTickets, false);
                    }
                    if (!inputTickets.Any() && existingTickets.Any())
                    {
                        // Cancel ticket reservation
                        SetSeatsAvailability(existingTickets, true);
                    }
                    if (inputTickets.Any() && existingTickets.Any() &&
                        !inputTickets.OrderBy(t => t.Id).SequenceEqual(existingTickets.OrderBy(t => t.Id)))
                    {
                        //fix ticket difference in data
                        List<Ticket> reserve = inputTickets.Except(existingTickets).ToList(); //A - B
                        List<Ticket> cancel = existingTickets.Except(inputTickets).ToList(); // B - A
                        SetSeatsAvailability(reserve, true);
                        SetSeatsAvailability(cancel, false);
                    }
                }
                repository.Update(
                        orderId,
                        input
                    );
            }
            catch (InvalidOperationException except)
            {
                return new Response
                {
                    status = 400,
                    statusText = except.Message
                }.ChromelyWrapper(req.Id);
            }
            repository.SaveChangesThenDiscard();
            return new Response
            {
                status = 200,
            }.ChromelyWrapper(req.Id);
        }

        /// <summary>
        /// Route to cancel an order.
        /// </summary>
        /// <param name="req">HttpPost request containing the order id.</param>
        /// <returns>Status 200 if the order was cancelled, 400 if the order was not found.</returns>
        [HttpPost(Route = "/order#cancel")]
        public ChromelyResponse CancelOrder(ChromelyRequest req)
        {
            //throw new NotImplementedException();
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            Repository<Order> repository = new Repository<Order>();
            int orderId = data["id"].Value<int>();
            try
            {
                SetSeatsAvailability(filterTickets(repository.Data[orderId].items), true);
            } catch (KeyNotFoundException)
            {
                return new Response
                {
                    status = 400,
                    statusText = "Order was not cancelled because it was not found."
                }.ChromelyWrapper(req.Id);
            }
            repository.Data.Remove(orderId);
            repository.SaveChangesThenDiscard();
            return new Response
            {
                status = 200
            }.ChromelyWrapper(req.Id);
        }

        /// <summary>
        /// Sets the availability of the seat specified on the ticket to the supplied value.
        /// </summary>
        /// <param name="tickets">Ticket containing the seat and screentime id</param>
        /// <param name="value">The value to set the availability to.</param>
        private void SetSeatsAvailability(List<Ticket> tickets, bool value)
        {
            Repository<ScreenTime> repo = new Repository<ScreenTime>();
            foreach (Ticket ticket in tickets)
            {
                repo.Data[ticket.screenTime].SetSeatAvailability(ticket, value);
            }
            repo.SaveChangesThenDiscard();
        }

        /// <summary>
        /// Parses the items in the order to a list of products.
        /// </summary>
        /// <param name="dataSeq">Data as a JArray.</param>
        /// <returns>Data as a List of Product Objects.</returns>
        private List<Product> ParseItems(JArray dataSeq)
        {
            List<Product> products = new List<Product>();
            foreach (JObject product in dataSeq)
            {
                products.Add(ProductController.ToProduct(product));
            }
            return products;
        }
    }
}
