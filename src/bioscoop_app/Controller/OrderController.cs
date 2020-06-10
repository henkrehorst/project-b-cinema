using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Chromely.Windows;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
                select (Ticket) product).ToList();
        };

        /// <summary>
        /// Performs a linq query on the order data, searching for the order with the code in the request postdata.
        /// </summary>
        /// <param name="req">Request containing the code as postdata.</param>
        /// <returns>The order matching the code, null if no match.</returns>
        /// <exception cref="InvalidOperationException">If the repository is closed.</exception>
        /// <exception cref="ArgumentNullException">If repository.Data.Values is null.</exception>
        private Order QueryByCode(Repository<Order> repository, string code)
        {
            var orders = repository.Data.Values.AsQueryable();
            IEnumerable<Order> queryResult = from order in orders
                where order.code == code
                select order;
            try
            {
                return queryResult.First();
            } catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Searches the order with the specified code in the data, and returns it.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>HttpStatus 200 and the order data if found, 204 if not found</returns>
        [HttpPost(Route = "/order#fetch")]
        public ChromelyResponse FetchOrder(ChromelyRequest req)
        {
            var data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            string code;
            try
            {
                code = data["code"].Value<string>();
            }
            catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            if (code is null || code.Length != 8)
            {
                return Response.ParseError(req.Id);
            }
            Order order;
            try
            {
                order = QueryByCode(new Repository<Order>(), code);
            } catch (InvalidOperationException)
            {
                return Response.TransactionProtocolViolation(req.Id);
            } catch (ArgumentNullException)
            {
                return new Response
                {
                    status = 500,
                    statusText = "The server does not possess the data for this request."
                }.ChromelyWrapper(req.Id);
            }
            if(order is null)
            {
                return new Response
                {
                    status = 400,
                    statusText = "No order was found for the given code."
                }.ChromelyWrapper(req.Id);
            }

            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(order)
            }.ChromelyWrapper(req.Id);
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
            var data = (JObject) JsonConvert.DeserializeObject(req.PostData.ToJson());
            string email;
            string name;
            try
            {
                email = data["cust_email"].Value<string>();
                name = data["cust_name"].Value<string>();
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            //List<Product> products = ParseItems(data["items"].Value<JArray>());
            if (Validator.IsEmail(email) &&
                Validator.IsName(name))
            {
                Order order;
                try
                {
                    order = new Order(
                        ParseItems(data["items"].Value<JArray>()),
                        ParseTickets(data["tickets"].Value<JArray>()),
                        data["cust_name"].Value<string>(),
                        data["cust_email"].Value<string>()
                    );
                } catch (FormatException)
                {
                    return Response.ParseError(req.Id);
                }
                Repository<ScreenTime> subtransaction = new Repository<ScreenTime>();
                try
                {
                    SetSeatsAvailability(order.tickets, false, ref subtransaction);
                }
                catch (InvalidOperationException err)
                {
                    if (err.Source.Contains("ScreenTime"))
                    {
                        return new Response
                        {
                            status = 409,
                            statusText = err.Message
                        }.ChromelyWrapper(req.Id);
                    } else
                    {
                        return Response.TransactionProtocolViolation(req.Id);
                    }
                }
                try
                {
                    subtransaction.SaveChangesThenDiscard();
                    new Repository<Order>().AddThenWrite(order);
                } catch (InvalidOperationException)
                {
                    return Response.TransactionProtocolViolation(req.Id);
                } catch (ValidationException exception)
                {
                    return Response.ValidationError(req.Id, exception);
                }
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(order.code)
                }.ChromelyWrapper(req.Id);
            }
            else
            {
                //create error object
                Dictionary<string, string> errorMessage = new Dictionary<string, string>();
                //set error message
                if (!Validator.IsName(data["cust_name"].Value<string>()))
                    errorMessage.Add("name-field"
                        , "De opgegeven naam is leeg of bevat verkeerde tekens, voer een geldige naam van minimaal 2 tekens in!");
                if (!Validator.IsEmail(data["cust_email"].Value<string>()))
                    errorMessage.Add("email-field"
                        , "Voer een geldig email adres in!");

                return new Response
                {
                    status = 422,
                    data = JsonConvert.SerializeObject(errorMessage)
                }.ChromelyWrapper(req.Id);
            }
        }

        ///<summary>      
        /// Post route voor het voltooien van een order.        
        /// Bedoeld voor de medewerker.         
        /// </summary>         
        /// <param name="req">Post request with the order code.</param>         
        /// <returns>The items in the order.</returns>         
        [HttpPost(Route = "/order#collect")]
        public ChromelyResponse CollectOrder(ChromelyRequest req)
        {
            var data = (JObject) JsonConvert.DeserializeObject(req.PostData.ToJson());
            string code;
            try
            {
                code = data["code"].Value<string>();
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            if (code is null)
            {
                return new Response {status = 400, statusText = "Bad input de code was null"}
                    .ChromelyWrapper(req.Id);
            }

            var repos = new Repository<Order>();
            IQueryable<Order> orders;
            try
            {
                orders = repos.Data.Values.AsQueryable();
            }
            catch (ArgumentNullException)
            {
                return new Response
                {
                    status = 500,
                    statusText = "The server does not possess the data for this request."
                }.ChromelyWrapper(req.Id);
            }
            var result = from order in orders where order.code == code select order;
            Order matchedOrder = null;
            try
            {
                matchedOrder = result.First();
            }
            catch (InvalidOperationException)
            {
                return new Response
                {
                    status = 400,
                    statusText = "No order matching the input code was found."
                }.ChromelyWrapper(req.Id);
            }

            if (!matchedOrder.redeemable)
            {
                return new Response
                {
                    status = 409,
                    statusText = "Order has been redeemed before."
                }.ChromelyWrapper(req.Id);
            }
            else
            {
                matchedOrder.redeemable = false;
            }
            try
            {
                repos.Update(matchedOrder.Id, matchedOrder);
                repos.SaveChangesThenDiscard();
            } catch (InvalidOperationException err)
            {
                if(err.Message is object && err.Message != "")
                {
                    return Response.IllegalUpdate(req.Id, err.Message);
                }
                return Response.TransactionProtocolViolation(req.Id);
            }
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new {matchedOrder.tickets, matchedOrder.items})
            }.ChromelyWrapper(req.Id);
        }


        /// <summary>
        /// Updates the order associated with the specified id, with the specified data.
        /// </summary>
        /// <param name="req">http POST request containing the id and data</param>
        /// <returns>Status code indicating success or failure.</returns>
        [HttpPost(Route = "/order#update")]
        public ChromelyResponse UpdateOrder(ChromelyRequest req)
        {
            //throw new NotImplementedException();
            JObject data = (JObject) JsonConvert.DeserializeObject(req.PostData.ToJson());
            string email;
            string name;
            try
            {
                email = data["cust_email"].Value<string>();
                name = data["cust_name"].Value<string>();
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            if (Validator.IsEmail(email) &&
                Validator.IsName(name))
            {
                int orderId;
                try
                {
                    orderId = data["id"].Value<int>();
                }
                catch (FormatException)
                {
                    return Response.ParseError(req.Id);
                }
                if (orderId == 0)
                {
                    return new Response
                    {
                        status = 400,
                        statusText = "id was null or zero"
                    }.ChromelyWrapper(req.Id);
                }

                Repository<Order> orderRepository = new Repository<Order>();
                string orderCode = null;
                try
                {
                    orderCode = orderRepository.Data[orderId].code;
                }
                catch (KeyNotFoundException)
                {
                    return new Response
                    {
                        status = 400,
                        statusText = "No order found for the id provided."
                    }.ChromelyWrapper(req.Id);
                }
                bool negRedeem;
                try
                {
                    negRedeem = !orderRepository.Data[orderId].redeemable;
                } catch (InvalidOperationException)
                {
                    return Response.TransactionProtocolViolation(req.Id);
                }
                if (negRedeem)
                {
                    return new Response
                    {
                        status = 400,
                        statusText = "Can't modify an order that has already been collected."
                    }.ChromelyWrapper(req.Id);
                }
                Order input;
                try
                {
                    input = new Order(
                        orderId,
                        ParseItems(data["items"].Value<JArray>()),
                        ParseTickets(data["tickets"].Value<JArray>()),
                        orderCode,
                        data["cust_name"].Value<string>(),
                        data["cust_email"].Value<string>(),
                        true
                    );
                } catch (FormatException)
                {
                    return Response.ParseError(req.Id);
                }
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

                try
                {
                    orderRepository.Update(
                        orderId,
                        input
                    );
                    Repository<ScreenTime> screenTimeRepository = new Repository<ScreenTime>();
                    try
                    {
                        if (input.tickets.Any() && orderRepository.Data[orderId].items.OrderBy(p => p.Id).Any() &&
                            !input.tickets.OrderBy(t => t.Id)
                                .SequenceEqual(orderRepository.Data[orderId].tickets.OrderBy(p => p.Id)))
                        {
                            //fix ticket difference in data
                            List<Ticket> reserve = input.tickets.Except(orderRepository.Data[orderId].tickets).ToList(); //A - B
                            List<Ticket> cancel = orderRepository.Data[orderId].tickets.Except(input.tickets).ToList(); // B - A
                            SetSeatsAvailability(reserve, false, ref screenTimeRepository);
                            SetSeatsAvailability(cancel, true, ref screenTimeRepository);
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        return Response.TransactionProtocolViolation(req.Id);
                    }
                    screenTimeRepository.SaveChangesThenDiscard();
                    orderRepository.SaveChangesThenDiscard();
                }
                catch (InvalidOperationException)
                {
                    return Response.TransactionProtocolViolation(req.Id);
                }
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(orderCode)
                }.ChromelyWrapper(req.Id);
            }
            else
            {
                //create error object
                Dictionary<string, string> errorMessage = new Dictionary<string, string>();
                //set error message
                if (!Validator.IsName(data["cust_name"].Value<string>()))
                    errorMessage.Add("name-field"
                        , "De opgegeven naam is leeg of bevat verkeerde tekens, voer een geldige naam van minimaal 2 tekens in!");
                if (!Validator.IsEmail(data["cust_email"].Value<string>()))
                    errorMessage.Add("email-field"
                        , "Voer een geldig email adres in!");

                return new Response
                {
                    status = 422,
                    data = JsonConvert.SerializeObject(errorMessage)
                }.ChromelyWrapper(req.Id);
            }
        }

        /// <summary>
        /// Route to cancel an order.
        /// </summary>
        /// <param name="req">HttpPost request containing the order id.</param>
        /// <returns>Status 200 if the order was cancelled, 400 if the order was not found.</returns>
        [HttpPost(Route = "/order#cancel")]
        public ChromelyResponse CancelOrder(ChromelyRequest req)
        {
            var data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            string code;
            try
            {
                code = data["code"].Value<string>();
            }
            catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            if(code is null || code.Length != 8)
            {
                return Response.ParseError(req.Id);
            }
            Order order;
            Repository<Order> orderRepository = new Repository<Order>();
            try
            {
                order = QueryByCode(orderRepository, code);
            }
            catch (InvalidOperationException)
            {
                return Response.TransactionProtocolViolation(req.Id);
            }
            catch (ArgumentNullException)
            {
                return new Response
                {
                    status = 500,
                    statusText = "The server does not possess the data for this request."
                }.ChromelyWrapper(req.Id);
            }
            if (order is null)
            {
                return new Response
                {
                    status = 400,
                    statusText = "Order was not cancelled because it was not found."
                }.ChromelyWrapper(req.Id);
            }
            if (!order.redeemable)
            {
                return new Response
                {
                    status = 400,
                    statusText = "Order can't be cancelled because it has already been collected."
                }.ChromelyWrapper(req.Id);
            }
            Repository<ScreenTime> screenTimeRepository = new Repository<ScreenTime>();
            try
            {
                SetSeatsAvailability(orderRepository.Data[order.Id].tickets, true, ref screenTimeRepository);
                orderRepository.Data.Remove(order.Id);
                screenTimeRepository.SaveChangesThenDiscard();
                orderRepository.SaveChangesThenDiscard();
            } catch (InvalidOperationException)
            {
                return Response.TransactionProtocolViolation(req.Id);
            }
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
        /// <exception cref="InvalidOperationException">If the repository is closed (Source Repository) or the seat availability can't be set (Source ScreenTime)</exception>
        private void SetSeatsAvailability(List<Ticket> tickets, bool value, ref Repository<ScreenTime> repo)
        {
            foreach (Ticket ticket in tickets)
            {
                repo.Data[ticket.screenTime].SetSeatAvailability(ticket, value);
            }
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
            foreach (Product item in order.items)
            {
                if (item.type == "ticket")
                {
                    jItems.Add((JObject) JsonConvert.DeserializeObject(JsonConvert.SerializeObject((Ticket) item)));
                }
                else
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