using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace bioscoop_app.Model
{
	public class Ticket : DataType
	{
		//public Seat seat { get; set; }

		//private ScreenTime screenTime { get; set; }

		public int visitorAge { get; set; }

		public int productId { get; set; }
		
		public Product product { get; set; }
	}

}

