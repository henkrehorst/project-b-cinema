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

            Gift MyGift = new Gift(email, param["gift-type"].Value<string>());
            Repository<Gift> repos = new Repository<Gift>();
            repos.Add(MyGift);
            repos.SaveChangesThenDiscard();

            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(MyGift.Code)
            }.ChromelyWrapper(request.Id);
        }

        [HttpPost(Route = "/gift#fetch")]
        public ChromelyResponse FetchGift(ChromelyRequest request, string code)
        {
            Repository<Gift> repository = new Repository<Gift>();
            Dictionary<int, Gift> reposData = repository.Data;

            IQueryable<Gift> allGifts = reposData.Values.AsQueryable();
            IEnumerable<Gift> queryResult = from gift in allGifts where gift.Code == code select gift;

            if(queryResult.Any())
            {
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(queryResult.First().Type)
                }.ChromelyWrapper(request.Id);
            }
            else {
                return new Response
                {
                    status = 409,
                    statusText = "Er is geen gift gevonden van deze code!"
                }.ChromelyWrapper(request.Id);
            }
        }
    }
}
