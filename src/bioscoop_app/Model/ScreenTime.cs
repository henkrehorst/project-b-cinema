using System;
using System.Reflection;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public class ScreenTime : DataType
	{
		public Movie movie;
		public DateTime startTime;
		public DateTime endTime;
		public Room room;

		public ScreenTime(Movie movie, DateTime startTime, DateTime endTime /*, Room room*/)
		{
			this.movie = movie;
			this.startTime = startTime;
			this.endTime = endTime;
			//this.room = room;
		}

		[JsonConstructor]
		public ScreenTime(int id, Movie movie, DateTime startTime, DateTime endTime/*, Room room*/)
		{
			this.id = id;
			this.movie = movie;
			this.startTime = startTime;
			this.endTime = endTime;
			//this.room = room;
		}
	}

}
