using Newtonsoft.Json;

namespace bioscoop_app.Model
{
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
			this.id = id;
			this.price_modifier = price_modifier;
			this.row = row;
			this.seatnr = seatnr;
		}

		public override bool Equals(object other)
		{
			if (other == null) return false;
			if (!other.GetType().Equals(typeof(Seat));
			Seat that = (Seat)other;
			if (price_modifier != that.price_modifier) return false;
			if (row != that.row) return false;
			if (seatnr != that.seatnr) return false;
			return true;
		}
	}

}

