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
    public class ReserveringController : ChromelyController
    {
        /// <param name="request">http GET request</param>
        /// <returns>All Movies in the data file.</returns>
        [HttpGet(Route = "/reserveringen")]
        public ChromelyResponse Getreserveringen(ChromelyRequest request)
        {
            var reserveringRepository = new ReserveringRepository();

            var reserveringen = reserveringRepository.Data;

            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(reserveringen)
            }.ChromelyWrapper(request.Id);
        }

        /// <param name="req">http POST request containing the desired Movie's id</param>
        /// <returns>The Movie associated with the id.</returns>
        [HttpPost(Route = "/reserveringen#id")]
        public ChromelyResponse GetReserveringenById(ChromelyRequest req)
        {
            int id = ((JObject)JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<int>("id");
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new ReserveringRepository().Data[id])
            }.ChromelyWrapper(req.Id);
        }

        /// <summary>
        /// Adds a movie to the data file.
        /// </summary>
        /// <param name="request">http POST request containing the movie</param>
        /// <returns>Status 204 indicating the movie was added successfully</returns>
        [HttpPost(Route = "/reserveringen/add")]
        public ChromelyResponse AddReservering(ChromelyRequest request)
        {
            var data = (JObject) JsonConvert.DeserializeObject(request.PostData.ToJson());
            var reserveringRepository = new ReserveringRepository();


            //get base64 image string



            reserveringRepository.Add(new Reservering(
                data["naam"].Value<string>(),
                data["email"].Value<string>(),
                data["producten"].Value<string>()
               
            ));

            reserveringRepository.SaveChangesThenDiscard();

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
        [HttpPost(Route = "/reserveringen#update")]
        public ChromelyResponse UpdateReservering(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            Repository<Reservering> repository = new ReserveringRepository();
            
           
            
            
           
            
            try
            {
                repository.Update(new Reservering(
                   data["naam"].Value<string>(),
                   data["email"].Value<string>(),
                   data["producten"].Value<string>()
                  
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