using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Newtonsoft.Json;

namespace bioscoop_app.Controller
{
    public class KijkwijzerController: ChromelyController
    {
        /// <summary>
        /// Retrieves all kijkwijzers from the Data file.
        /// </summary>
        /// <param name="req">http GET request</param>
        /// <returns>Kijkwijzers from the Data file</returns>
        [HttpGet(Route = "/kijkwijzer")]
        public ChromelyResponse GetProducts(ChromelyRequest req)
        {
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new Repository<Kijkwijzer>().Data)
            }.ChromelyWrapper(req.Id);
        }
    }
}