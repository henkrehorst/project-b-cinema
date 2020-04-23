using System.Collections.Generic;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public class Product : DataType
	{
		public double price;
		public string name;

		public Product(double price, string name)
		{
			this.price = price;
			this.name = name;
		}

		[JsonConstructor]
		public Product(int id, double price, string name)
		{
			Id = id;
			this.price = price;
			this.name = name;
		}
	}

}

