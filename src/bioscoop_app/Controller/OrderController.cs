using Chromely.Core.Network;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
    }
}
