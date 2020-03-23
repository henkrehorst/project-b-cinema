using Chromely.Core.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bioscoop_app.Controller
{
    public class DemoController : ChromelyController
    {
        [HttpGet(Route = "/demo")]
        public ChromelyResponse GetDemo(ChromelyRequest request)
        {
            return new ChromelyResponse(request.Id)
            {
                Data = "Dit is een bericht van de backend!"
            };
        }

        [HttpPost(Route = "/file/upload")]
        public ChromelyResponse GetUpload(ChromelyRequest request)
        {

            return new ChromelyResponse(request.Id)
            {
                Data = "Dit is een test met upload"
            };
        }
        
        [HttpPost(Route = "/demo/post")]
        public ChromelyResponse GetDemoPost(ChromelyRequest request)
        {
            var data = (JObject)JsonConvert.DeserializeObject(request.PostData.ToJson());
            string message = data["message"].Value<string>();

            return new ChromelyResponse(request.Id)
            {
                Data = message
            };
        }
    }
}