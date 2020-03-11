namespace bioscoop_app.Model
{
	public class Order
	{
		public int id { get; set; }
		private Product[] items { get; set; }

		private string code { get; set; }

		private string cust_name { get; set; }

		private string cust_email { get; set; }

	}

}

