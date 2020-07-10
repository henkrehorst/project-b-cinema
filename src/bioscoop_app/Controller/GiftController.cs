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
            Gift MyGift = new Gift();

            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(new Repository<Gift>().Data)
            }.ChromelyWrapper(request.Id);
        }
    }
}
