using System;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
    public sealed class Review : DataType
    {
        public string title;

        //TODO: make properties later available
        // private Kijkwijzer[] kijkwijzers { get; set; }
        // public Enum dimension { get; set; }
        public int movie;
        public double rating;
        public string mening;

        public Review(int movie, double rating, string mening)
        {
            this.movie = movie;
            this.rating = rating;
            this.mening = mening;
        }

        [JsonConstructor]
        public Review(int movie, int id, double rating, string mening)
        {
            this.Id = id;
            this.movie = movie;
            this.rating = rating;
            this.mening = mening;

        }
    }
}