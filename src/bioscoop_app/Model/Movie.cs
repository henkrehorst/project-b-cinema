using System;

namespace bioscoop_app.Model
{
	public class Movie
	{
		public int id { get; set; }

		private string title { get; set; }

		private Kijkwijzer[] kijkwijzers { get; set; }

		private Enum dimension { get; set; }

		private string genre { get; set; }

		private float rating { get; set; }

		private int duration { get; set; }
	}

}

