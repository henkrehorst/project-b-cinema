﻿using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bioscoop_app.Controller
{
    /// <summary>
    /// Controller for the requests related to orders.
    /// </summary>
    class OrderController : ChromelyController
    {
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
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(new Repository<Order>().Data[data["code"].Value<int>()])
                }.ChromelyWrapper(req.Id);
            } catch (KeyNotFoundException)
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

            //parse items
            var prods = data["items"].Value<JArray>();
            List<Product> products = new List<Product>();
            foreach(JObject product in prods)
            {
                products.Append(ProductController.ToProduct(product));
            }

            Order order = new Order(
                    products,
                    data["cust_name"].Value<string>(),
                    data["cust_email"].Value<string>()
                );
            ReserveTickets(order.items);
            new Repository<Order>().AddThenWrite(order);
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(order.code)
            }.ChromelyWrapper(req.Id);
        }

        private void ReserveTickets(List<Product> items)
        {
            var tickets = items.Where(p => p.GetType() == typeof(Ticket))
                .Select(t => t);
            foreach(Ticket ticket in tickets)
            {
                var repo = new Repository<ScreenTime>();
                repo.Data[ticket.screenTime].ReserveSeat(ticket);
                repo.SaveChangesThenDiscard();
            }
        }
    }
}
