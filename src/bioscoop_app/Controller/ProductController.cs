using System;
using Chromely.Core.Network;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bioscoop_app.Controller
{
    public class ProductController : ChromelyController
    {
        [HttpGet(Route = "/products")]
        public ChromelyResponse GetProducts(ChromelyRequest req)
        {
            Repository<Product> repository = new ProductRepository();
            ChromelyResponse res = new ChromelyResponse(req.Id)
            {
                Data = JsonConvert.SerializeObject(repository.Data)
            };
            repository.Discard();
            return res;
        }

        [HttpPost(Route = "/products/add")]
        public ChromelyResponse AddProduct(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            Product prod;
            if (data.ContainsKey("seat") && data.ContainsKey("screenTime") && data.ContainsKey("visitorAge"))
            {
                prod = new Ticket(
                    data["price"].Value<double>(),
                    data["name"].Value<string>(),
                    data["seat"].Value<Seat>(),
                    data["screenTime"].Value<ScreenTime>(),
                    data["visitorAge"].Value<int>()
                    );
            }
            else
            {
                prod = new Product(
                    data["price"].Value<double>(),
                    data["name"].Value<string>()
                    );
            }
            new ProductRepository().AddAndWrite(prod);
            ChromelyResponse res = new ChromelyResponse(req.Id)
            {
                Data = "Product added"
            };
            return res;
        }
    }
}