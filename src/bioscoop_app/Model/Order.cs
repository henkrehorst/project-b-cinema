using Model;

namespace Model
{
	public class Order
	{
		private int id { get; }

		private Product[] items { get; }

		private string code { get; }

		private string cust_name { get; set; }

		private string cust_email { get; set; }

	}

}

