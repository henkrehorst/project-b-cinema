using System;
using System.Collections.Generic;
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
<<<<<<< Updated upstream
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

=======

<<<<<<< Updated upstream
=======
            movieRepository.Add(new Movie(
                data["title"].Value<string>(),
                data["genre"].Value<string>(),
                data["rating"].Value<double>(),
                data["samenvatting"].Value<string>(),
                data["duration"].Value<int>(),
                fileName
            ));
            
            movieRepository.SaveChangesThenDiscard();
>>>>>>> Stashed changes

            Dictionary<string, string> errorMessage = new Dictionary<string, string>();

            if (String.IsNullOrWhiteSpace(data["title"].Value<string>()))
            {

                errorMessage.Add("title", "Er zit niks in title");
            }
            if (String.IsNullOrWhiteSpace(data["genre"].Value<string>()))
            {
                errorMessage.Add("genre", "Er zit niks in de genre");
            }

            if (String.IsNullOrWhiteSpace(data["rating"].Value<string>()))
            {
<<<<<<< Updated upstream
                errorMessage.Add("rating", "Er zit niks in de rating");
            }

            if (String.IsNullOrWhiteSpace(data["duration"].Value<string>()))
=======
                repository.Update(data.Value<int>("id"), new Movie(
                    data["title"].Value<string>(),
                    data["genre"].Value<string>(),
                    data["rating"].Value<double>(),
                    data["samenvatting"].Value<string>(),
                    data["duration"].Value<int>(),
                    filename
                ));
            } catch(InvalidOperationException except)
>>>>>>> Stashed changes
            {
                errorMessage.Add("duration", "Er zit niks in de duration");
            }

            var data = (JObject) JsonConvert.DeserializeObject(request.PostData.ToJson());
            var movieRepository = new MovieRepository();
            //get base64 image string
            

            movieRepository.Add(new Movie(
                data["title"].Value<string>(),
                data["genre"].Value<string>(),
                data["rating"].Value<double>(),
                data["duration"].Value<int>()
            ));
            
            movieRepository.SaveChanges();

>>>>>>> Stashed changes
            return new ChromelyResponse(request.Id)
            {
                Data = response
            };
            
        }
    }
}