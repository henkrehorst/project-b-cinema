using Chromely.Core.Network;

namespace bioscoop_app.Controller
{
    public class DemoController
    {
        [HttpGet(Route = "/demo")]
        public ChromelyResponse GetDemo(ChromelyRequest request)
        {
            return new ChromelyResponse(request.Id)
            {
                Data = "Dit is een response van de DemoController!"
            };
        }
    }
}