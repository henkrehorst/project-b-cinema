using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace bioscoop_app.Model
{
	public class Ticket : Product
	{
		public Seat seat;

		public ScreenTime screenTime;

		public int visitorAge;
	}

}

