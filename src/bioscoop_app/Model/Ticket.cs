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

		public Ticket(double price, string name, Seat seat, ScreenTime screenTime, int visitorAge)
		{
			base(price, name);
			this.seat = seat;
			this.screenTime = screenTime;
			this.visitorAge = visitorAge;
		}

		[JsonConstructor]
		public Ticket(int id, double price, string name, Seat seat, ScreenTime screenTime, int visitorAge)
		{
			base(id, price, name);
			this.seat = seat;
			this.screenTime = screenTime;
			this.visitorAge = visitorAge;
		}

		public override bool Equals(object other)
		{
			if (other == null) return false;
			if (!other.GetType().Equals(typeof(Ticket))) return false;
			Ticket that = (Ticket)other;
			if (!base.Equals(that)) return false;
			if (!screenTime.Equals(that.screenTime)) return false;
			if (!seat.Equals(that.seat)) return false;
			if (visitorAge != that.visitorAge) return false;
			return true;
		}
	}

}

