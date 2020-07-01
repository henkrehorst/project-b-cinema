using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Newtonsoft.Json;

namespace bioscoop_app.Controller
{
    public class GiftController : ChromelyController
    {
        [HttpPost(Route = "/gift#create")]
        public ChromelyResponse CreateGift(ChromelyRequest request)
        {
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new Repository<Gift>().Data)
            }.ChromelyWrapper(request.Id);
        }

        [HttpGet(Route = "/gift#get")]
        public ChromelyResponse GetGift(ChromelyRequest request)
        {
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new Repository<Gift>().Data)
            }.ChromelyWrapper(request.Id);
        }
    }
}
