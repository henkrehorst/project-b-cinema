using System.ComponentModel.DataAnnotations.Schema;

namespace bioscoop_app.Model
{
	public class Ticket
	{
		//public Seat seat { get; set; }

		//private ScreenTime screenTime { get; set; }


		public int id { get; set; }

		public int visitor_age { get; set; }

		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		public Product Product { get; set; }
	}

}

