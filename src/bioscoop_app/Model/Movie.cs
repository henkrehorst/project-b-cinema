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
        public string coverImage;

        public Movie(string title, string genre, double rating, int duration, string coverImage) {
            this.title = title;
            this.genre = genre;
            this.rating = rating;
            this.duration = duration;
            this.coverImage = coverImage;
        }
        
        [JsonConstructor]
        public Movie(int id, string title, string genre, double rating, int duration, string coverImage) {
            this.id = id;
            this.title = title;
            this.genre = genre;
            this.rating = rating;
            this.duration = duration;
            this.coverImage = coverImage;
        }
    }
}