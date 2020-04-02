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
			this.id = id;
			this.price = price;
			this.name = name;
		}

		public override bool Equals(object other)
		{
			if (other == null) return false;
			if (!other.GetType().Equals(typeof(Product))) return false;
			Product that = (Product)other;
			if (!that.name.Equals(name)) return false;
			if (that.price != price) return false;
			if (that.id != id) return false;
			return true;
		}
	}

}

