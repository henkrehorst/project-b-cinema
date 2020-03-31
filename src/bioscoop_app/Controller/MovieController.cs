using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Repository;
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
            var movieRepository = new MovieRepository();

            var movies = movieRepository.Data;

            return new ChromelyResponse(request.Id)
            {
                Data = JsonConvert.SerializeObject(movies)
            };
        }


        [HttpPost(Route = "/movies/add")]
        public ChromelyResponse AddMovie(ChromelyRequest request)
        {
            var data = (JObject) JsonConvert.DeserializeObject(request.PostData.ToJson());
            var movieRepository = new MovieRepository();

            movieRepository.AddMovie(new Movie(
                data["title"].Value<string>(),
                data["genre"].Value<string>(),
                data["rating"].Value<double>(),
                data["duration"].Value<int>()
            ));

            return new ChromelyResponse(request.Id)
            {
                Data = "Movie added"
            };
        }
    }
}