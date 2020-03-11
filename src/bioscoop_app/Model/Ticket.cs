namespace bioscoop_app.Model
{
	public class Ticket : Product
	{
		private Seat seat { get; set; }

		private ScreenTime screenTime { get; }

		private int visitor_age { get; set; }

	}

}

