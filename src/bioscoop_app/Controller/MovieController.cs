using System;
using System.Linq;
using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using bioscoop_app.Service;
using Chromely.Core.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bioscoop_app.Controller
{
    /// <summary>
    /// Class that controls the routes related to Movie.
    /// </summary>
    public class MovieController : ChromelyController
    {
        /// <param name="request">http GET request</param>
        /// <returns>All Movies in the data file.</returns>
        [HttpGet(Route = "/movies")]
        public ChromelyResponse GetMovies(ChromelyRequest request)
        {
            var movieRepository = new MovieRepository();

            var movies = movieRepository.Data;

            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(movies)
            }.ChromelyWrapper(request.Id);
        }

        /// <param name="req">http POST request containing the desired Movie's id</param>
        /// <returns>The Movie associated with the id.</returns>
        [HttpPost(Route = "/movies#id")]
        public ChromelyResponse GetMovieById(ChromelyRequest req)
        {
            int id = ((JObject)JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<int>("id");
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new MovieRepository().Data[id])
            }.ChromelyWrapper(req.Id);
        }

        /// <summary>
        /// Adds a movie to the data file.
        /// </summary>
        /// <param name="request">http POST request containing the movie</param>
        /// <returns>Status 204 indicating the movie was added successfully</returns>
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

            return new Response
            {
                status = 204
            }.ChromelyWrapper(request.Id);
        }

        /// <summary>
        /// Updates the movie associated with the specified id, with the specified data.
        /// </summary>
        /// <param name="req">http POST request containing the id and data</param>
        /// <returns>Status code indicating success or failure.</returns>
        [HttpPost(Route = "/movies#update")]
        public ChromelyResponse UpdateMovie(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            Repository<Movie> repository = new MovieRepository();
            
            string filestring = data["cover_image"].Value<string>();
            string filename = repository.Data[data.Value<int>("id")].coverImage;
            
            if (filestring.Length > 0)
            {
                var uploadService = new UploadService(filestring);
                if (uploadService.CheckIsImage())
                {
                    uploadService.DeleteFile(filename);
                    uploadService.CreateFileInUploadFolder();
                    filename = uploadService.GetFileName();
                }
            }
            
            try
            {
                repository.Update(data.Value<int>("id"), new Movie(
                    data["title"].Value<string>(),
                    data["genre"].Value<string>(),
                    data["rating"].Value<double>(),
                    data["duration"].Value<int>(),
                    filename
                ));
            } catch(InvalidOperationException except)
            {
                return new Response
                {
                    status = 400,
                    statusText = except.Message
                }.ChromelyWrapper(req.Id);
            }
            repository.SaveChangesThenDiscard();
            return new Response
            {
                status = 200,
                data = req.PostData.ToJson()
            }.ChromelyWrapper(req.Id);
        }
    }
}