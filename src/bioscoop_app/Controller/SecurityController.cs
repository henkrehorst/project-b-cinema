using System;
using System.Collections.Generic;
using bioscoop_app.Helper;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bioscoop_app.Controller
{
    public class SecurityController : ChromelyController
    {
        private string _adminPassword = "beheerder";
        private string _cashierPassword = "medewerker";

        /// <param name="req">http POST request for authentication admin and cashier</param>
        /// <returns>The users roll if the password is correct</returns>
        [HttpPost(Route = "/security/login")]
        public ChromelyResponse Login(ChromelyRequest req)
        {
            string password;
            try
            {
                password = ((JObject)JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<string>("password");
            }
            catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }

            if (password == _adminPassword)
            {
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(new Dictionary<string, string>
                    {
                        {"role", "admin"}
                    })
                }.ChromelyWrapper(req.Id);
            }
            else if (password == _cashierPassword)
            {
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(new Dictionary<string, string>
                    {
                        {"role", "cashier"}
                    })
                }.ChromelyWrapper(req.Id);
            }
            else
            {
                return new Response
                {
                    status = 401,
                    data = JsonConvert.SerializeObject(new Dictionary<string, string>
                    {
                        {"error", "Incorrect password!"}
                    })
                }.ChromelyWrapper(req.Id);
            }
        }
    }
}