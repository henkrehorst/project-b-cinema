using System.Collections.Generic;
using System.Linq;

namespace bioscoop_app.Model
{
	public sealed class Order : DataType
	{
		public List<Product> items;
		public string code;
		public string cust_name;
		public string cust_email;

		public override bool Equals(object other)
		{
			if (other == null) return false;
			if (!other.GetType().Equals(typeof(Order))) return false;
			Order that = (Order)other;
			if (!items.SequenceEqual(that.items)) return false;
			if (!code.ToUpper().Equals(that.code.ToUpper())) return false;
			if (!cust_name.Equals(that.cust_name)) return false;
			if (!cust_email.ToLower().Equals(that.cust_email.ToLower())) return false;
			return true;
		}
	}

}

