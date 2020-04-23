using System;
using Chromely.Core.Network;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace bioscoop_app.Controller
{
    /// <summary>
    /// Controller that handles the routes related to Product.
    /// </summary>
    public class ProductController : ChromelyController
    {
        /// <summary>
        /// Retrieves all products from the Data file. Excludes Tickets.
        /// </summary>
        /// <param name="req">http GET request</param>
        /// <returns>Products from the Data file, excluding Tickets.</returns>
        [HttpGet(Route = "/products")]
        public ChromelyResponse GetProducts(ChromelyRequest req)
        {
            Dictionary<int, Product> rawData = new ProductRepository().Data;
            List<Product> data = new List<Product>();
            foreach (Product prod in rawData.Values)
            {
                if (prod.GetType() == typeof(Product)) data.Add(prod);
            }
            return new ChromelyResponse(req.Id)
            {
                Data = JsonConvert.SerializeObject(data)
            };
        }

        /// <param name="req">http GET request</param>
        /// <returns>Current ticket price.</returns>
        [HttpGet(Route = "/product#ticketprice")]
        public ChromelyResponse GetTicketPrice(ChromelyRequest req)
        {
            return new ChromelyResponse(req.Id)
            {
                Data = Ticket.basePrice
            };
        }

        /// <summary>
        /// Updates the basePrice of a Ticket to the specified value.
        /// </summary>
        /// <param name="req">http POST request containing the new basePrice</param>
        /// <returns>Status 204</returns>
        [HttpPost(Route = "/product#ticketprice")]
        public ChromelyResponse SetTicketPrice(ChromelyRequest req)
        {
            Ticket.basePrice = ((JObject)JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<double>("price");
            return new ChromelyResponse(req.Id)
            {
                Data = "Succesfully updated ticket price"
            };
        }

        /// <summary>
        /// Adds the specified Product to the data file.
        /// </summary>
        /// <param name="req">http POST request containing the Product data</param>
        /// <returns>Status 204</returns>
        [HttpPost(Route = "/products#add")]
        public ChromelyResponse AddProduct(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            //Console.WriteLine(data);
            new ProductRepository().AddThenWrite(ToProduct(data));
            return new ChromelyResponse(req.Id)
            {
                Data = "Product added"
            };
        }

        /// <summary>
        /// Updates Product associated with the specified id in the data file.
        /// </summary>
        /// <param name="req">http POST request containing the key and value data.</param>
        /// <returns>Status 204 on succes, Status 400 on failure</returns>
        [HttpPost(Route = "/products#update")]
        public ChromelyResponse UpdateProduct(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            int? id = data.Value<int>("id");
            if (id is null) throw new ArgumentNullException();
            int checkedId = (int) id;
            Repository<Product> repository = new ProductRepository();
            repository.Update(checkedId, ToProduct(data));
            repository.SaveChangesThenDiscard();
            ChromelyResponse res = new ChromelyResponse(req.Id)
            {
                Data = "Updated succesfully"
            };
            return res;
        }

        /// <param name="data">Product as a JObject</param>
        /// <returns>Product as a Product or Ticket</returns>
        private Product ToProduct(JObject data)
        {
            if (data.ContainsKey("seat") && data.ContainsKey("screenTime") && data.ContainsKey("visitorAge"))
            {
                return new Ticket(
                    Ticket.GetModifier(data["price"].Value<double>()),
                    data["name"].Value<string>(),
                    data["seat"].Value<Seat>(),
                    data["screenTime"].Value<ScreenTime>(),
                    data["visitorAge"].Value<int>()
                    );
            }
            else
            {
                return new Product(
                    data["price"].Value<double>(),
                    data["name"].Value<string>()
                    );
            }
        }
    }
}