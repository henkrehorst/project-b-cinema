using System.Collections.Generic;

namespace bioscoop_app.Model
{
	public class Product
	{
		public int id { get; set; }

		public double price { get; set; }

		public string name { get; set; }

		public List<Ticket> Tickets { get; set; }
	}

}

