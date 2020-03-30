using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Service;
using Newtonsoft.Json;

namespace bioscoop_app.Repository
{
    public class MovieRepository
    {
        private const string FileName = "movies.json";
        private Dictionary<int, Movie> Movies;

        public MovieRepository()
        {
            Movies = JsonConvert.DeserializeObject<Dictionary<int, Movie>>(
                File.ReadAllText(StorageService.GetDataSourcePath() + FileName)
            );
        }

        public static void SetupDataSource()
        {
            if (!File.Exists(StorageService.GetDataSourcePath() + FileName))
            {
                //File.Create(StorageService.GetDataSourcePath() + FileName);
                File.WriteAllText(
                    StorageService.GetDataSourcePath() + FileName,
                    JsonConvert.SerializeObject(new Dictionary<int, Movie>())
                );
            }
        }

        public void AddMovie(Movie movie)
        {
            try
            {
                movie.id = Movies.Keys.Max() + 1;
            }
            catch (Exception e)
            {
                movie.id = 0;
            }
            
            this.Movies.Add(movie.id, movie);
            
            File.WriteAllText(
                StorageService.GetDataSourcePath() + FileName,
                JsonConvert.SerializeObject(Movies)
            );
            
        }
        
    }
}