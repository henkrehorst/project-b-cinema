using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public class Room : DataType
	{
		public List<Seat> layout;
		public int seats;
		public bool auro;
		public bool imax;

		public Room(List<Seat> layout, int seats, bool auro, bool imax)
		{
			this.layout = layout;
			this.seats = seats;
			this.auro = auro;
			this.imax = imax;
		}

		[JsonConstructor]
		public Room(int id, List<Seat> layout, int seats, bool auro, bool imax)
		{
			this.id = id;
			this.layout = layout;
			this.seats = seats;
			this.auro = auro;
			this.imax = imax;
		}

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

