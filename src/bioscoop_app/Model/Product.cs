using System.Collections.Generic;

namespace bioscoop_app.Model
{
	public class Product : DataType
	{
		public double price;

		public string name;

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

