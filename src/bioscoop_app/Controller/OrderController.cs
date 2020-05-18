using bioscoop_app.Helper;
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
    class OrderController : ChromelyController
    {
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

        /// <summary>
        /// Updates the order associated with the specified id, with the specified data.
        /// </summary>
        /// <param name="req">http POST request containing the id and data</param>
        /// <returns>Status code indicating success or failure.</returns>
        [HttpPost(Route = "/reserveringen#update")]
        public ChromelyResponse UpdateOrder(ChromelyRequest req)
        {
            throw new NotImplementedException();
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            Repository<Order> repository = new Repository<Order>();
            try
            {
                /*repository.Update(
                    data["id"].Value<int>(),
                    new Order(
                        )
                    );*/
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
                data = req.PostData.ToJson()
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
