using System.Collections.Generic;
using System.Linq;

namespace bioscoop_app.Model
{
	public class Room : DataType
	{
		public List<Seat> layout;
		public int seats;
		public bool auro;
		public bool imax;

		public override bool Equals(object other)
		{
			if (other == null) return false;
			if (!other.GetType().Equals(typeof(Room))) return false;
			Room that = (Room)other;
			if (!layout.SequenceEqual(that.layout)) return false;
			if (seats != that.seats) return false;
			if (auro != that.auro) return false;
			if (imax != that.imax) return false;
			return true;
		}
	}

}

