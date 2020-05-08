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
    /// <summary>
    /// Repository that acts on ScreenTime.
    /// </summary>
    public class ScreenTimeRepository : Repository<ScreenTime>
    {
        /// <param name="movieId">The id of the movie.</param>
        /// <returns>A dictionary of ScreenTimes associated with the movie.</returns>
        public Dictionary<int, ScreenTime> GetScreenTimeByMovieId(int movieId)
        {
            return Data.Where(item =>
                    item.Value.movie.Equals(movieId))
                .ToDictionary(item => item.Key, item => item.Value);
        }
    }
}