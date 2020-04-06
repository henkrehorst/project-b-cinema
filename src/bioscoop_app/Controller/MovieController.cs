using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using bioscoop_app.Service;
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
            //get base64 image string
            string coverImage = data["cover_image"].Value<string>();
            if (coverImage.Length != 0)
            {
                var uploadService = new UploadService(coverImage);
                if (uploadService.CheckIsImage())
                {
                    uploadService.CreateFileInUploadFolder();
                }
            }


            movieRepository.Add(new Movie(
                data["title"].Value<string>(),
                data["genre"].Value<string>(),
                data["rating"].Value<double>(),
                data["duration"].Value<int>()
            ));
            
            movieRepository.SaveChanges();

            return new ChromelyResponse(request.Id)
            {
                Data = "Movie added"
            };
        }
    }
}