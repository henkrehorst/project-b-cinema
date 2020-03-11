using System;
using Logic;

namespace Logic
{
	public class Movie
	{
		private int id { get; }

		private string title { get; set; }

		private Array<Kijkwijzer> kijkwijzers { get; }

		private Enum dimension { get; set; }

		private string genre { get; set; }

		private float rating { get; }

		private int duration { get; set; }
	}

}

