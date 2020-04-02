using System;

namespace bioscoop_app.Model
{
	public class ScreenTime : DataType
	{
		public Movie movie;
		public DateTime startTime;
		public DateTime endTime;
		public Room room;

		public override bool Equals(object other)
		{
			if (other == null) return false;
			if (!other.GetType().Equals(typeof(ScreenTime))) return false;
			ScreenTime that = (ScreenTime)other;
			if (!movie.Equals(that.movie)) return false;
			if (!startTime.Equals(that.startTime)) return false;
			if (!endTime.Equals(that.endTime)) return false;
			if (!room.Equals(that.room)) return false;
			return true;
		}
	}

}

