using System.Collections.Generic;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public class Product : DataType
	{
		public double price;
		public string name;
		public string type;

		public Product(double price, string name, string type = "product")
		{
			this.price = price;
			this.name = name;
			this.type = type;
		}

		[JsonConstructor]
		public Product(int id, double price, string name, string type = "product")
		{
			Id = id;
			this.price = price;
			this.name = name;
			this.type = type;
		}
	}

}

