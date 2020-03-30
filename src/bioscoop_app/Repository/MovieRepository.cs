using System;
using System.Collections.Generic;
using System.Collections;
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
        private Dictionary<int, Movie> Movies { get; }
        private bool IsOpen { get; } = false;

        public MovieRepository()
        {
            Movies = JsonConvert.DeserializeObject<Dictionary<int, Movie>>(
                File.ReadAllText(StorageService.GetDataSourcePath() + FileName)
            );
            IsOpen = true;
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
            if (!IsOpen)
            {
                throw new System.InvalidOperationException();
            }
            if(Movies.Count() == 0)
            {
                Console.WriteLine(e);
                movie.id = 0;
            } else
            {
                movie.id = Movies.Keys.Max() + 1;
            }
            this.Movies.Add(movie.id, movie);
        }
        
        public void Discard()
        {
            IsOpen = false;
        }

        public void SaveChangesAndDiscard()
        {
            SaveChanges();
            Discard();
        }

        public void AddMovieAndWrite(Movie movie)
        {
            AddMovie(movie);
            SaveChanges();
        }

        public void SaveChanges()
        {
            File.WriteAllText(
                StorageService.GetDataSourcePath() + FileName,
                JsonConvert.SerializeObject(Movies)
            );
        }

        public List<Movie> Query(string title, string genre, double rating, int duration, int limit)
        {
            List<Movie> resultSet = new ArrayList<Movie>();
            foreach(Movie movie in Movies)
            {
                if (title == null || movie.title.Equals(title))
                {
                    if (genre == null || movie.genre.Equals(genre))
                    {
                        if (rating == null || movie.rating == rating)
                        {
                            if (duration == null || movie.duration == duration)
                            {
                                resultSet.Add(movie);
                                if (resultSet.Count() == limit)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return resultSet;
        }

        public Dictionary<int, Movie> GetMovies()
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException();
            }
            return Movies;
        }
        
    }
}