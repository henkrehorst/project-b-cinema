namespace bioscoop_app.Model
{
	public class Ticket : Product
	{
		public Seat seat { get; set; }

		private ScreenTime screenTime { get; set; }

		private int visitor_age { get; set; }

	}

}

