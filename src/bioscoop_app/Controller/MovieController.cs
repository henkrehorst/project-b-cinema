using System.Linq;
using bioscoop_app.Model;
using Chromely.Core.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bioscoop_app.Controller
{
    public class MovieController : ChromelyController
    {
        [HttpGet(Route = "/movies")]
        public ChromelyResponse GetMovies(ChromelyRequest request)
        {
            using var context = new CinemaContext();

            var movies = context.Movies;

            return new ChromelyResponse(request.Id)
            {
                Data = movies.ToJson()
            };
        }


        [HttpPost(Route = "/movies/add")]
        public ChromelyResponse AddMovie(ChromelyRequest request)
        {
            var response = new Helper.Response { Status = 204, Data = "Hoi", StatusText = "OKOK" };
            var data = (JObject)JsonConvert.DeserializeObject(request.PostData.ToJson());

            using (var context = new CinemaContext())
            {
                context.Add(new Movie {
                    title = data["title"].Value<string>(),
                    duration = data["duration"].Value<int>(),
                    genre = data["genre"].Value<string>(), 
                    rating = data["rating"].Value<float>(), 
                });
                context.SaveChanges();
            }

            return new ChromelyResponse(request.Id)
            {
                Data = response
            };
            
        }
    }
}