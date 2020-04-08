using System;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bioscoop_app.Controller
{
    public class ScreenTimeController : ChromelyController
    {
        [HttpGet(Route = "/screentime")]
        public ChromelyResponse GetScreenTimes(ChromelyRequest request)
        {
            var screenTimeRepository = new ScreenTimeRepository();

            return new ChromelyResponse(request.Id)
            {
                Data = JsonConvert.SerializeObject(screenTimeRepository.Data)
            };
        }

        [HttpPost(Route = "/screentime/add")]
        public ChromelyResponse AddScreenTime(ChromelyRequest request)
        {
            var data = (JObject) JsonConvert.DeserializeObject(request.PostData.ToJson());

            var screenTimeRepository = new ScreenTimeRepository();
            screenTimeRepository.Add(new ScreenTime(
                data["movie_id"].Value<int>(),
                data["start_time"].Value<DateTime>(),
                data["end_time"].Value<DateTime>())
            );
            screenTimeRepository.SaveChanges();

            return new ChromelyResponse(request.Id)
            {
                Data = "Screentime added"
            };
        }

        [HttpPost(Route = "/screentime#id")]
        public ChromelyResponse GetScreenTimeById(ChromelyRequest req)
        {
            int id = ((JObject) JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<int>("id");
            return new ChromelyResponse(req.Id)
            {
                Data = JsonConvert.SerializeObject(new ScreenTimeRepository().Data[id])
            };
        }


        [HttpPost(Route = "/screentime#update")]
        public ChromelyResponse UpdateScreenTime(ChromelyRequest req)
        {
            JObject data = (JObject) JsonConvert.DeserializeObject(req.PostData.ToJson());

            var screenTimeRepository = new ScreenTimeRepository();
            screenTimeRepository.Update(data["id"].Value<int>(), new ScreenTime(
                data["movie_id"].Value<int>(),
                data["start_time"].Value<DateTime>(),
                data["end_time"].Value<DateTime>())
            );
            screenTimeRepository.SaveChanges();

            return new ChromelyResponse(req.Id)
            {
                Data = "screenTime updated"
            };
        }
        
        [HttpPost(Route = "/screentime#movie")]
        public ChromelyResponse GetScreenTimesByMovieId(ChromelyRequest req)
        {
            JObject data = (JObject) JsonConvert.DeserializeObject(req.PostData.ToJson());
            
            return new ChromelyResponse(req.Id)
            {
                Data = JsonConvert.SerializeObject(new ScreenTimeRepository()
                    .GetScreenTimeByMovieId(data["id"].Value<int>()))
            };
        }
    }
}