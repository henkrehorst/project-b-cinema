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
            ChromelyResponse res = new ChromelyResponse(req.Id)
            {
                Data = JsonConvert.SerializeObject(new ProductRepository().Data)
            };
            return res;
        }

        [HttpPost(Route = "/products#add")]
        public ChromelyResponse AddProduct(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            //Console.WriteLine(data);
            new ProductRepository().AddAndWrite(ToProduct(data));
            return new ChromelyResponse(req.Id)
            {
                Data = "Product added"
            };
        }

        [HttpPost(Route = "/products#update")]
        public ChromelyResponse UpdateProduct(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            int? id = data.Value<int>("id");
            if (id is null) throw new ArgumentNullException();
            int checkedId = (int) id;
            Repository<Product> repository = new ProductRepository();
            repository.Update(checkedId, ToProduct(data));
            repository.SaveChangesAndDiscard();
            ChromelyResponse res = new ChromelyResponse(req.Id)
            {
                Data = "Updated succesfully"
            };
            return res;
        }

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