using System;
using System.Linq;
using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bioscoop_app.Controller
{
    /// <summary>
    /// Controller that handles the routes related to ScreenTime.
    /// </summary>
    public class ScreenTimeController : ChromelyController
    {
        /// <param name="request">http GET request</param>
        /// <returns>All ScreenTime data.</returns>
        [HttpGet(Route = "/screentime")]
        public ChromelyResponse GetScreenTimes(ChromelyRequest request)
        {
            var screenTimeRepository = new ScreenTimeRepository();

            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(screenTimeRepository.Data)
            }.ChromelyWrapper(request.Id);
        }

        /// <summary>
        /// Adds posted ScreenTime to the data file.
        /// </summary>
        /// <param name="request">http POST request that contains a ScreenTime as postData.</param>
        /// <returns>An http response stating the addition was succesfull.</returns>
        [HttpPost(Route = "/screentime/add")]
        public ChromelyResponse AddScreenTime(ChromelyRequest request)
        {
            var data = (JObject) JsonConvert.DeserializeObject(request.PostData.ToJson());

            var screenTimeRepository = new ScreenTimeRepository();
            screenTimeRepository.AddThenWrite(new ScreenTime(
                data["movie_id"].Value<int>(),
                data["start_time"].Value<DateTime>(),
                data["end_time"].Value<DateTime>())
            );

            return new Response
            {
                status = 204
            }.ChromelyWrapper(request.Id);
        }

        /// <param name="req">http POST re</param>
        /// <returns>the ScreenTime associated with the provided id</returns>
        [HttpPost(Route = "/screentime#id")]
        public ChromelyResponse GetScreenTimeById(ChromelyRequest req)
        {
            int id = ((JObject) JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<int>("id");
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new ScreenTimeRepository().Data[id])
            }.ChromelyWrapper(req.Id);
        }

        /// <summary>
        /// Updates posted ScreenTime in the data file.
        /// </summary>
        /// <param name="request">http POST request that contains a ScreenTime with and id as postData.</param>
        /// <returns>An http response stating the update was succesfull.</returns>
        [HttpPost(Route = "/screentime#update")]
        public ChromelyResponse UpdateScreenTime(ChromelyRequest req)
        {
            JObject data = (JObject) JsonConvert.DeserializeObject(req.PostData.ToJson());

            if (data["id"] is null) return new Response
                {
                    status = 400,
                    statusText = "no id provided"
                }.ChromelyWrapper(req.Id);

            try
            {
                var screenTimeRepository = new ScreenTimeRepository();
                screenTimeRepository.Update(data["id"].Value<int>(), new ScreenTime(
                    data["movie_id"].Value<int>(),
                    data["start_time"].Value<DateTime>(),
                    data["end_time"].Value<DateTime>())
                );
                screenTimeRepository.SaveChanges();

                return new Response
                {
                    status = 204
                }.ChromelyWrapper(req.Id);
            } catch (InvalidOperationException exception)
            {
                return new Response
                {
                    status = 400,
                    statusText = exception.Message
                }.ChromelyWrapper(req.Id);
            }
        }
        
        /// <param name="req">http POST request that contains the id of the movie</param>
        /// <returns>The Screentimes associated with the movie.</returns>
        [HttpPost(Route = "/screentime#movie")]
        public ChromelyResponse GetScreenTimesByMovieId(ChromelyRequest req)
        {
            JObject data = (JObject) JsonConvert.DeserializeObject(req.PostData.ToJson());
            
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new ScreenTimeRepository()
                    .GetScreenTimeByMovieId(data["id"].Value<int>()))
            }.ChromelyWrapper(req.Id);
        }
    }
}