using System;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public class ScreenTime : DataType
	{
		public int id { get; set; }
		public Movie movie { get; set; }
		public DateTime startTime { get; set; }
		public DateTime endTime { get; set; }
		public Room room { get; set; }

        public ScreenTime(Movie movie, DateTime startTime, DateTime endTime, Room room)
        {
            this.movie = movie;
            this.startTime = startTime;
            this.endTime = endTime;
            this.room = room;
        }

        [JsonConstructor]
        public ScreenTime(int id, Movie movie, DateTime startTime, DateTime endTime, Room room)
        {
            this.id = id;
            this.movie = movie;
            this.startTime = startTime;
            this.endTime = endTime;
            this.room = room;
        }
    }

}

