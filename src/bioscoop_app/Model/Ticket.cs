using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public sealed class Ticket : Product
	{
		public Seat seat;
		public ScreenTime screenTime;
		public int visitorAge;

		public Ticket(double price, string name, Seat seat, ScreenTime screenTime, int visitorAge) : base(price, name)
		{
			this.seat = seat;
			this.screenTime = screenTime;
			this.visitorAge = visitorAge;
		}

		[JsonConstructor]
		public Ticket(int id, double price, string name, Seat seat, ScreenTime screenTime, int visitorAge): base(id, price, name)
		{
			this.seat = seat;
			this.screenTime = screenTime;
			this.visitorAge = visitorAge;
		}
	}

}

