using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public class ScreenTime : DataType
	{
		public int movie;
		public DateTime startTime;
		public DateTime endTime;
		//public Room room;
		//public List<List<bool>> availability;

		public ScreenTime(int movie, DateTime startTime, DateTime endTime/*, Room room, List<List<bool>> availability*/)
		{
			this.movie = movie;
			this.startTime = startTime;
			this.endTime = endTime;
			//this.room = room;
			//this.availability = (availability is null) ? AvailabilityMap(room.layout) : availability;
		}

		[JsonConstructor]
		public ScreenTime(int id, int movie, DateTime startTime, DateTime endTime /*,Room room, List<List<bool>> availability*/)
		{
			this.id = id;
			this.movie = movie;
			this.startTime = startTime;
			this.endTime = endTime;
			//this.room = room;
			//this.availability = (availability is null) ? AvailabilityMap(room.layout) : availability;
		}

		// private List<List<bool>> AvailabilityMap(List<List<Seat>> layout)
		// {
		// 	List<List<bool>> availability = new List<List<bool>>();
		// 	foreach (List<Seat> row in layout)
		// 	{
		// 		List<bool> boolrow = new List<bool>();
		// 		foreach (Seat seat in row)
		// 		{
		// 			boolrow.Add(seat is null);
		// 		}
		// 		availability.Add(boolrow);
		// 	}
		// 	return availability;
		// }
	}

}

