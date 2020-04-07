using System;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Chromely.Core.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bioscoop_app.Controller
{
    public class ScreenTimeController : ChromelyController
    {
        public void AddScreenTime(Movie movie, DateTime startTime, DateTime endTime /*, Room room*/)
        {
            var screenTimeRepository = new ScreenTimeRepository();
            screenTimeRepository.Add(new ScreenTime(movie, startTime, endTime /*,Room room*/));
        }
    }
}
