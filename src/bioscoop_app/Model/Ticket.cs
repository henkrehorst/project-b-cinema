using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public sealed class Ticket : Product
	{
		public static double basePrice = 10.00;
		public static Func<double, double> GetModifier = price => price / basePrice;
		public Seat seat;
		public ScreenTime screenTime;
		public int visitorAge;

		public Ticket(double priceModifier, string name, Seat seat, ScreenTime screenTime, int visitorAge)
			: base(basePrice*priceModifier, name, "ticket")
		{
			this.seat = seat;
			this.screenTime = screenTime;
			this.visitorAge = visitorAge;
		}

		[JsonConstructor]
		public Ticket(int id, double priceModifier, string name, Seat seat, ScreenTime screenTime, int visitorAge)
			: base(id, basePrice*priceModifier, name, "ticket")
		{
			this.seat = seat;
			this.screenTime = screenTime;
			this.visitorAge = visitorAge;
		}
	}

}

