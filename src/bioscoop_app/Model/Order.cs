using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public sealed class Order : DataType
	{
		public List<Product> items;
		public string code;
		public string cust_name;
		public string cust_email;

		public Order(List<Product> items, string cust_name, string cust_email)
		{
			this.items = items;
			code = null;
			this.cust_email = cust_email;
			this.cust_name = cust_name;
		}

		[JsonConstructor]
		public Order(int id, List<Product> items, string cust_name, string cust_email)
		{
			Id = id;
			this.items = items;
			code = null;
			this.cust_email = cust_email;
			this.cust_name = cust_name;
		}
	}

}

