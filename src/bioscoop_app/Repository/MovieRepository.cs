﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Service;
using Newtonsoft.Json;

namespace bioscoop_app.Repository
{
    public class MovieRepository : Repository<Movie>
    {
        public MovieRepository() : base() { }

        public void AddMovie(Movie movie)
        {
            if (!IsOpen)
            {
                throw new System.InvalidOperationException();
            }
            if(!Data.Any())
            {
                movie.id = 0;
            } else
            {
                movie.id = Data.Keys.Max() + 1;
            }
            this.Data.Add(movie.id, movie);
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

        

        public List<Movie> Query(string title, string genre, double rating, int duration, int limit)
        {
            List<Movie> resultSet = new List<Movie>();
            foreach (Movie movie in Data.Values)
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

        public List<Movie> QueryFirst(string title, string genre, double rating, int duration)
        {
            return Query(title, genre, rating, duration, 1);
        }

        public List<Movie> UnlimitedQuery(string title, string genre, double rating, int duration)
        {
            return Query(title, genre, rating, duration, -1);
        }
    }
}