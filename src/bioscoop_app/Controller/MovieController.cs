using System;
using System.Collections.Generic;
using System.Linq;
using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using bioscoop_app.Service;
using Chromely.Core.Network;
using FluentValidation;
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
            var movieRepository = new Repository<Movie>();

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
            JObject jParse = ((JObject)JsonConvert.DeserializeObject(req.PostData.ToJson()));
            int id;
            try
            {
                id = jParse.Value<int>("id");
            }
            catch(FormatException) {
                return Response.ParseError(req.Id);
            }
            Movie data;  
            try
            {
                data = new Repository<Movie>().Data[id];
            }
            catch(KeyNotFoundException)
            {
                return new Response
                {
                    status = 204,
                    statusText = $"No movie found for {id}"
                }.ChromelyWrapper(req.Id);
            }
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(data)
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
            string fileName = "";
            string thumbnailName = "";

            //convert kijkwijzer collection to int array
            int[] kijkWijzers;
            //get base64 image string
            string coverImage;
            //get base64 string of thumbnail image
            string thumbnail;
            try
            {
                try
                {
                    kijkWijzers = data["kijkwijzers"].Select(x => (int)x).ToArray();
                } catch (ArgumentNullException)
                {
                    return Response.ParseError(request.Id);
                }
                coverImage = data["cover_image"].Value<string>();
                thumbnail = data["thumbnail_image"].Value<string>();
            } catch (FormatException)
            {
                return Response.ParseError(request.Id);
            }

            if (coverImage.Length != 0)
            {
                var uploadService = new UploadService(coverImage);
                if (uploadService.CheckIsImage())
                {
                    uploadService.CreateFileInUploadFolder();
                    fileName = uploadService.GetFileName();
                }
            }
            
            if (thumbnail.Length != 0)
            {
                var uploadService = new UploadService(thumbnail);
                if (uploadService.CheckIsImage())
                {
                    uploadService.CreateFileInUploadFolder();
                    thumbnailName = uploadService.GetFileName();
                }
            }

            var movieRepository = new Repository<Movie>();
            try
            {
                movieRepository.Add(new Movie(
                    data["title"].Value<string>(),
                    data["genre"].Value<string>(),
                    data["rating"].Value<double>(),
                    data["samenvatting"].Value<string>(),
                    data["duration"].Value<int>(),
                    fileName,
                    kijkWijzers,
                    thumbnailName
                ));
            } catch (FormatException)
            {
                return Response.ParseError(request.Id);
            } catch (InvalidOperationException)
            {
                return Response.TransactionProtocolViolation(request.Id);
            } catch (ValidationException exception)
            {
                return Response.ValidationError(request.Id, exception);
            }

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
            Repository<Movie> repository = new Repository<Movie>();
            
            string filestring = data["cover_image"].Value<string>();
            string filename = repository.Data[data.Value<int>("id")].coverImage;
            string thumbnailName = repository.Data[data.Value<int>("id")].thumbnailImage;
            
            //convert kijkwijzer collection to int array
            int[] kijkWijzers = data["kijkwijzers"].Select(x => (int) x).ToArray();

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
            
            //get base64 string of thumbnail image
            string thumbnail = data["thumbnail_image"].Value<string>();
            
            if (thumbnail.Length != 0)
            {
                var uploadService = new UploadService(thumbnail);
                if (uploadService.CheckIsImage())
                {
                    uploadService.DeleteFile(thumbnailName);
                    uploadService.CreateFileInUploadFolder();
                    thumbnailName = uploadService.GetFileName();
                }
            }
            
            try
            {
                repository.Update(data.Value<int>("id"), new Movie(
                    data["title"].Value<string>(),
                    data["genre"].Value<string>(),
                    data["rating"].Value<double>(),
                    data["samenvatting"].Value<string>(),
                    data["duration"].Value<int>(),
                    filename,
                    kijkWijzers,
                    thumbnailName
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