using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Chromely.Windows;
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
        [Obsolete("Order.items no longer contains tickets, to retrieve the Tickets address Order.tickets.")]
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
                var orders = new Repository<Order>().Data.Values.AsQueryable();
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
            //List<Product> products = ParseItems(data["items"].Value<JArray>());
            Order order = new Order(
                    ParseItems(data["items"].Value<JArray>()),
                    ParseTickets(data["tickets"].Value<JArray>()),
                    data["cust_name"].Value<string>(),
                    data["cust_email"].Value<string>()
                );
            try
            {
                SetSeatsAvailability(order.tickets, false);
            } catch (InvalidOperationException err)
            {
                return new Response
                {
                    status = 409,
                    statusText = err.Message
                }.ChromelyWrapper(req.Id);
            }
            new Repository<Order>().AddThenWrite(order);
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(order.code)
            }.ChromelyWrapper(req.Id);
        }

         ///<summary>      
        /// Post route voor het voltooien van een order.        
        /// Bedoeld voor de medewerker.         
        /// </summary>         
        /// <param name="req">Post request with the order code.</param>         
        /// <returns>The items in the order.</returns>         
        [HttpPost (Route = "/order#collect")]         
        public ChromelyResponse CollectOrder(ChromelyRequest req)        
        {
            var data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            string code = data["code"].Value<string>();
            if (code is null)
            {
                return new Response { status = 400, statusText = "Bad input de code was null" }
                .ChromelyWrapper(req.Id);
            }
            var repos = new Repository<Order>();
            var orders = repos.Data.Values.AsQueryable();
            var result = from order in orders where order.code == code select order;
            Order matchedOrder = null;
            try
            {
                matchedOrder = result.First();
            } catch (InvalidOperationException)
            {
                return new Response
                {
                    status = 204,
                    statusText = "No order matching the input code was found."
                }.ChromelyWrapper(req.Id);
            }
            return new Response { 
                status = 200, 
                data = JsonConvert.SerializeObject(new { matchedOrder.tickets, matchedOrder.items})
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
                        ParseTickets(data["tickets"].Value<JArray>()),
                        data["code"].Value<string>(),
                        data["cust_name"].Value<string>(),
                        data["cust_email"].Value<string>()
                    );
                /*if (!input.items.OrderBy(p => p.Id).SequenceEqual(repository.Data[orderId].items.OrderBy(p => p.Id)))
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
                }*/
                if (input.tickets.Any() && repository.Data[orderId].items.OrderBy(p => p.Id).Any() &&
                        !input.tickets.OrderBy(t => t.Id).SequenceEqual(repository.Data[orderId].tickets.OrderBy(p => p.Id)))
                {
                    //fix ticket difference in data
                    List<Ticket> reserve = input.tickets.Except(repository.Data[orderId].tickets).ToList(); //A - B
                    List<Ticket> cancel = repository.Data[orderId].tickets.Except(input.tickets).ToList(); // B - A
                    SetSeatsAvailability(reserve, false);
                    SetSeatsAvailability(cancel, true);
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
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            Repository<Order> repository = new Repository<Order>();
            int orderId = data["id"].Value<int>();
            try
            {
                SetSeatsAvailability(repository.Data[orderId].tickets, true);
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

        /// <summary>
        /// Parses the tickets in the order to a list of tickets.
        /// </summary>
        /// <param name="dataSeq">Data as a JArray.</param>
        /// <returns>Data as a List of Product Objects.</returns>
        private List<Ticket> ParseTickets(JArray dataSeq)
        {
            List<Ticket> tickets = new List<Ticket>();
            foreach (JObject ticket in dataSeq)
            {
                tickets.Add(new Ticket(
                    ticket["Id"].Value<int>(),
                    ticket["price"].Value<double>(),
                    ticket["name"].Value<string>(),
                    ticket["row"].Value<int>(),
                    ticket["seatnr"].Value<int>(),
                    ticket["screenTime"].Value<int>(),
                    ticket["visitorAge"].Value<int>()
                    ));
            }
            return tickets;
        }

        /// <summary>
        /// Converts a single order to json in a way that preserves all information.
        /// </summary>
        /// <param name="order">The order to convert.</param>
        /// <returns></returns>
        [Obsolete("Ticket < Product is deprecated, please use JsonConvert instead.", true)]
        private string SingleOrderToJson(Order order)
        {
            List<JObject> jItems = new List<JObject>();
            foreach (Product item in order.items) {
                if (item.type == "ticket")
                {
                    jItems.Add((JObject) JsonConvert.DeserializeObject(JsonConvert.SerializeObject((Ticket) item)));
                } else
                {
                    jItems.Add((JObject) JsonConvert.DeserializeObject(JsonConvert.SerializeObject(item)));
                }
            }
            return JsonConvert.SerializeObject(new
            {
                code = order.code,
                cust_name = order.cust_name,
                cust_email = order.cust_email,
                items = jItems
            });
        }
    }
}
