using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public sealed class Ticket : Product
	{
		public int row;
		public int seatnr;
		public int screenTime;
		public int visitorAge;

		public Ticket(double price, string name, int row, int seatnr, int screenTime, int visitorAge)
			: base(price, name, "ticket")
		{
			this.row = row;
			this.seatnr = seatnr;
			this.screenTime = screenTime;
			this.visitorAge = visitorAge;
		}

		[JsonConstructor]
		public Ticket(int id, double price, string name, int row, int seatnr, int screenTime, int visitorAge)
			: base(id, price, name, "ticket")
		{
			this.row = row;
			this.seatnr = seatnr;
			this.screenTime = screenTime;
			this.visitorAge = visitorAge;
		}
	}

}

