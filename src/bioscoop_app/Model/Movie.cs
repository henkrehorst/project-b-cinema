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
        public double rating { get; set; }
        public int duration { get; set; }

        public Movie(string title, string genre, double rating, int duration) {
            this.title = title;
            this.genre = genre;
            this.rating = rating;
            this.duration = duration;
        }
        
        public Movie(int id, string title, string genre, double rating, int duration) {
            this.id = id;
            this.title = title;
            this.genre = genre;
            this.rating = rating;
            this.duration = duration;
        }
    }
}