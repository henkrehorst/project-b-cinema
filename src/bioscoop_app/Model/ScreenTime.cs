using System;

namespace bioscoop_app.Model
{
	public class ScreenTime
	{
		public int id { get; set; }

		private Movie movie { get; set; }

		private DateTime startTime1 { get; set; }

		private DateTime endTime { get; set; }

		private Room room { get; set; }

	}

}

