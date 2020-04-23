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

        [HttpPost(Route = "/movies#id")]
        public ChromelyResponse GetMovieById(ChromelyRequest req)
        {
            int id = ((JObject)JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<int>("id");
            return new ChromelyResponse(req.Id)
            {
                Data = JsonConvert.SerializeObject(new MovieRepository().Data[id])
            };
        }

        [HttpPost(Route = "/movies/add")]
        public ChromelyResponse AddMovie(ChromelyRequest request)
        {
            var data = (JObject) JsonConvert.DeserializeObject(request.PostData.ToJson());
            var movieRepository = new MovieRepository();
            string fileName = "";
            
            //get base64 image string
            string coverImage = data["cover_image"].Value<string>();
            if (coverImage.Length != 0)
            {
                var uploadService = new UploadService(coverImage);
                if (uploadService.CheckIsImage())
                {
                    uploadService.CreateFileInUploadFolder();
                    fileName = uploadService.GetFileName();
                }
            }


            movieRepository.Add(new Movie(
                data["title"].Value<string>(),
                data["genre"].Value<string>(),
                data["rating"].Value<double>(),
                data["duration"].Value<int>(),
                fileName
            ));
            
            movieRepository.SaveChangesThenDiscard();

            return new ChromelyResponse(request.Id)
            {
                Data = "Movie added"
            };
        }

        [HttpPost(Route = "/movies#update")]
        public ChromelyResponse UpdateMovie(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            string filename = data["filename"].Value<string>();
            string filestring = data["filestring"].Value<string>();
            if (filestring is object)
            {
                var uploadService = new UploadService(filestring);
                if (uploadService.CheckIsImage())
                {
                    uploadService.UpdateFile(filename);
                }
            }
            Repository<Movie> repository = new MovieRepository();
            repository.Update(data.Value<int>("id"), new Movie(
                    data["title"].Value<string>(),
                    data["genre"].Value<string>(),
                    data["rating"].Value<double>(),
                    data["duration"].Value<int>(),
                    filename
                ));
            repository.SaveChangesThenDiscard();
            return new ChromelyResponse(req.Id)
            {
                Data = req.PostData.ToJson()
            };
        }
    }
}