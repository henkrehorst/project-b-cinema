using Chromely.Core.Network;

namespace bioscoop_app.Controller
{
    public class DemoController : ChromelyController
    {
        [HttpGet(Route = "/demo")]
        public ChromelyResponse GetDemo(ChromelyRequest request)
        {
            return new ChromelyResponse(request.Id)
            {
                Data = "Dit is watch test!"
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
    }
}