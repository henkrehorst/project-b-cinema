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

            Order order = new Order(
                    data["items"].Value<List<Product>>(),
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
            List<Ticket> tickets = (List<Ticket>) items.Where(p => p.GetType() == typeof(Ticket));
            foreach(Ticket ticket in tickets)
            {
                var repo = new Repository<ScreenTime>();
                repo.Data[ticket.screenTime.Id].ReserveTicket(ticket);
                repo.SaveChangesThenDiscard();
            }
        }
    }
}
