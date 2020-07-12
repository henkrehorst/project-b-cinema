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
    /// Controller that handles the routes related to Review.
    /// </summary>
    public class ReviewController : ChromelyController
    {
        /// <param name="request">http GET request</param>
        /// <returns>All Review data.</returns>
        [HttpGet(Route = "/review")]
        public ChromelyResponse GetReviews(ChromelyRequest request)
        {
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new Repository<Review>().Data)
            }.ChromelyWrapper(request.Id);
        }

        /// <summary>
        /// Adds posted ScreenTime to the data file.
        /// </summary>
        /// <param name="request">http POST request that contains a ScreenTime as postData.</param>
        /// <returns>An http response stating the addition was succesfull.</returns>
        [HttpPost(Route = "/review/add")]
        public ChromelyResponse AddReview(ChromelyRequest request)
        {
            Console.WriteLine("voorvoor");
            var data = (JObject)JsonConvert.DeserializeObject(request.PostData.ToJson());
            try
            {
                Console.WriteLine("voor");
                new Repository<Review>().AddThenWrite(new Review(
                    data["movie_id"].Value<int>(),
                    data["rating"].Value<double>(),
                    data["mening"].Value<string>()
                ));
                Console.WriteLine("na");
            }
            catch (FormatException)
            {
                return Response.ParseError(request.Id);
            }
            catch (ValidationException exception)
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
        [HttpPost(Route = "/review#id")]
        public ChromelyResponse GetReviewById(ChromelyRequest req)
        {
            int id;
            try
            {
                id = ((JObject)JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<int>("id");
            }
            catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            try
            {
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(new Repository<Review>().Data[id])
                }.ChromelyWrapper(req.Id);
            }
            catch (KeyNotFoundException)
            {
                return new Response
                {
                    status = 400,
                    statusText = "No Review found for the given id."
                }.ChromelyWrapper(req.Id);
            }
        }

        /// <summary>
        /// Updates posted ScreenTime in the data file.
        /// </summary>
        /// <param name="request">http POST request that contains a ScreenTime with and id as postData.</param>
        /// <returns>An http response stating the update was succesfull.</returns>
        [HttpPost(Route = "/review#update")]
        public ChromelyResponse UpdateReview(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());

            if (data["id"] is null) return new Response
            {
                status = 400,
                statusText = "no id provided"
            }.ChromelyWrapper(req.Id);

            try
            {
                var reviewRepository = new Repository<Review>();
                reviewRepository.Update(data["id"].Value<int>(), new Review(
                    data["movie_id"].Value<int>(),
                    data["rating"].Value<double>(),
                    data["mening"].Value<string>()
                ));
                reviewRepository.SaveChangesThenDiscard();
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message is object && exception.Message != "")
                {
                    return Response.IllegalUpdate(req.Id, exception.Message);
                }
                else
                {
                    return Response.TransactionProtocolViolation(req.Id);
                }
            }
            catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            catch (ValidationException exception)
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
        [HttpPost(Route = "/review#movie")]
        public ChromelyResponse GetReviewsByMovieId(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
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
                data = JsonConvert.SerializeObject(GetReviewByMovieId(id))
            }.ChromelyWrapper(req.Id);
        }
        /// <param name="movieId">The id of the movie.</param>
        /// <returns>A dictionary of ScreenTimes associated with the movie.</returns>
        private Dictionary<int, Review> GetReviewByMovieId(int movieId)
        {
            return new Repository<Review>().Data.Where(item =>
                    item.Value.movie.Equals(movieId))
                .ToDictionary(item => item.Key, item => item.Value);
        }
    }
}