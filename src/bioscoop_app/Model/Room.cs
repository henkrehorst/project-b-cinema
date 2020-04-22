using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public class Room : DataType
	{
		public List<List<Seat>> layout;
		public int seats;
		public bool auro;
		public bool imax;

		public Room(List<List<Seat>> layout, int seats, bool auro, bool imax)
		{
			this.layout = layout;
			this.seats = seats;
			this.auro = auro;
			this.imax = imax;
		}

		[JsonConstructor]
		public Room(int id, List<List<Seat>> layout, int seats, bool auro, bool imax)
		{
			this.Id = id;
			this.layout = layout;
			this.seats = seats;
			this.auro = auro;
			this.imax = imax;
		}
	}

}

