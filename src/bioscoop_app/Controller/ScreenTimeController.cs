using System;
using System.Collections.Generic;
using System.Linq;
using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using FluentValidation;
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
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new Repository<ScreenTime>().Data)
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
            try
            {
                new Repository<ScreenTime>().AddThenWrite(new ScreenTime(
                    data["movie_id"].Value<int>(),
                    data["start_time"].Value<DateTime>(),
                    data["end_time"].Value<DateTime>(),
                    data["room_name"].Value<string>()
                ));
            } catch (FormatException)
            {
                return Response.ParseError(request.Id);
            } catch (ValidationException exception)
            {
                return Response.ValidationError(request.Id, exception);
            }
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
            int id;
            try
            {
                id = ((JObject)JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<int>("id");
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            try
            {
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(new Repository<ScreenTime>().Data[id])
                }.ChromelyWrapper(req.Id);
            } catch (KeyNotFoundException)
            {
                return new Response
                {
                    status = 400,
                    statusText = "No ScreenTime found for the given id."
                }.ChromelyWrapper(req.Id);
            }
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
                var screenTimeRepository = new Repository<ScreenTime>();
                screenTimeRepository.Update(data["id"].Value<int>(), new ScreenTime(
                    data["movie_id"].Value<int>(),
                    data["start_time"].Value<DateTime>(),
                    data["end_time"].Value<DateTime>(),
                    data["room_name"].Value<string>()
                ));
                screenTimeRepository.SaveChangesThenDiscard();
            } catch (InvalidOperationException exception)
            {
                if (exception.Message is object && exception.Message != "")
                {
                    return Response.IllegalUpdate(req.Id, exception.Message);
                } else
                {
                    return Response.TransactionProtocolViolation(req.Id);
                }
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            } catch (ValidationException exception)
            {
                return Response.ValidationError(req.Id, exception);
            }
            return new Response
            {
                status = 204
            }.ChromelyWrapper(req.Id);
        }
        
        /// <param name="req">http POST request that contains the id of the movie</param>
        /// <returns>The Screentimes associated with the movie.</returns>
        [HttpPost(Route = "/screentime#movie")]
        public ChromelyResponse GetScreenTimesByMovieId(ChromelyRequest req)
        {
            JObject data = (JObject) JsonConvert.DeserializeObject(req.PostData.ToJson());
            int id;
            try
            {
                id = data["id"].Value<int>();
            }
            catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(GetScreenTimeByMovieId(id))
            }.ChromelyWrapper(req.Id);
        }
        /// <param name="movieId">The id of the movie.</param>
        /// <returns>A dictionary of ScreenTimes associated with the movie.</returns>
        private Dictionary<int, ScreenTime> GetScreenTimeByMovieId(int movieId)
        {
            return new Repository<ScreenTime>().Data.Where(item =>
                    item.Value.movie.Equals(movieId))
                .ToDictionary(item => item.Key, item => item.Value);
        }
    }
}