using System;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
    public sealed class Movie : DataType
    {
        public string title;
        //TODO: make properties later available
        // private Kijkwijzer[] kijkwijzers { get; set; }
        // public Enum dimension { get; set; }
        public string genre;
        public double rating;
        public int duration;

        public Movie(string title, string genre, double rating, int duration) {
            this.title = title;
            this.genre = genre;
            this.rating = rating;
            this.duration = duration;
        }
        
        [JsonConstructor]
        public Movie(int id, string title, string genre, double rating, int duration) {
            this.id = id;
            this.title = title;
            this.genre = genre;
            this.rating = rating;
            this.duration = duration;
        }
    }
}