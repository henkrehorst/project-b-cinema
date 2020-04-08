using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Service;
using Newtonsoft.Json;

namespace bioscoop_app.Repository
{
    public class ScreenTimeRepository : Repository<ScreenTime>
    {
        public Dictionary<int, ScreenTime> GetScreenTimeByMovieId(int movieId)
        {
            return Data.Where(item =>
                    item.Value.movie.Equals(movieId))
                .ToDictionary(item => item.Key, item => item.Value);
        }
    }
}