using bioscoop_app.Helper;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Chromely.Windows;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace bioscoop_app.Controller
{
    public class GiftController : ChromelyController
    {
        [HttpPost(Route = "/gift#create")]
        public ChromelyResponse CreateGift(ChromelyRequest request)
        {
            JObject param = (JObject)JsonConvert.DeserializeObject(request.PostData.ToJson());
            int validate = 0;
            string email = param["gift-email"].Value<string>();

            for (int i = 0; i < email.Length; i++)
            {
                if ((char)email[i] == '@' && validate == 0)
                {
                    validate++;
                }
                else if ((char)email[i] == '.')
                {
                    validate++;
                }
            }

            if (validate != 2)
            {
                return new Response
                {
                    status = 409,
                    statusText = "Dit is geen geldige email!"
                }.ChromelyWrapper(request.Id);
            }

            Gift MyGift = new Gift(email);
            Repository<Gift> repos = new Repository<Gift>();
            repos.Add(MyGift);
            repos.SaveChangesThenDiscard();

            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(MyGift.Code)
            }.ChromelyWrapper(request.Id);
        }
    }
}
