using System;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
    public sealed class Movie : DataType
    {
        public string title { get; set; }
        //TODO: make properties later available
        // private Kijkwijzer[] kijkwijzers { get; set; }
        // public Enum dimension { get; set; }
        public string genre { get; set; }
        public double rating { get; set; }
        public int duration { get; set; }

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

        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (!other.GetType().Equals(typeof(Movie))) return false;
            Movie that = (Movie) other;
            if (!title.Equals(that.title)) return false;
            if (!genre.Equals(that.genre)) return false;
            if (rating != that.rating) return false;
            if (duration != that.duration) return false;
            return true;
        }
    }
}