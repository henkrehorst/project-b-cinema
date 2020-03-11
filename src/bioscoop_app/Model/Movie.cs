using System;

namespace bioscoop_app.Model
{
	public class Movie
	{
		private int id { get; }

		private string title { get; set; }

		private Kijkwijzer[] kijkwijzers { get; }

		private Enum dimension { get; set; }

		private string genre { get; set; }

		private float rating { get; }

		private int duration { get; set; }
	}

}

