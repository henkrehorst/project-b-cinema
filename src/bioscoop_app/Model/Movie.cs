using System;

namespace bioscoop_app.Model
{
    public class Movie
    {
        public int id { get; set; }
        public string title { get; set; }
        //TODO: make properties later available
        // private Kijkwijzer[] kijkwijzers { get; set; }
        // public Enum dimension { get; set; }
        public string genre { get; set; }
        public float rating { get; set; }
        public int duration { get; set; }

        public Movie(string title, string genre, string rating, string duration) {
            this.title = title;
            this.genre = genre;
            this.rating = rating;
            this.duration = duration;
        }
    }
}