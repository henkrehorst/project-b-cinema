using Newtonsoft.Json;
using System;

namespace bioscoop_app.Model
{
	[Obsolete]
	public class Seat : DataType
	{
		public double price_modifier;
		public int row;
		public int seatnr;

		public Seat(double price_modifier, int row, int seatnr)
		{
			this.price_modifier = price_modifier;
			this.row = row;
			this.seatnr = seatnr;
		}

		[JsonConstructor]
		public Seat(int id, double price_modifier, int row, int seatnr)
		{
			this.Id = id;
			this.price_modifier = price_modifier;
			this.row = row;
			this.seatnr = seatnr;
		}
	}

}

