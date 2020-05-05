using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
			code = GenerateCode();
		}

		[JsonConstructor]
		public Order(int id, List<Product> items, string code, string cust_name, string cust_email)
		{
			Id = id;
			this.items = items;
			this.code = code;
			this.cust_email = cust_email;
			this.cust_name = cust_name;
		}

		/// <summary>
		/// Generates a random and unique code for this Order.
		/// </summary>
		/// <returns>A 4 character string representing the code.</returns>
		private string GenerateCode()
		{
			//Create a random and unique unique long and converts to hexadecimal string
			int hash = GetHashCode();
			string binHash = Convert.ToString(hash, 2);
			long numerical = Convert.ToInt64(binHash + binHash, 2);
			unchecked
			{
				numerical += new Random().Next(int.MaxValue - 10 ^ 4, int.MaxValue);
			}
			string hex = Convert.ToString(numerical, 16);

			//Convert the hexadecimal string to a byte array with the size of a long
			IEnumerator<char> seq = hex.GetEnumerator();
			byte[] bytes = new byte[4];
			byte byteindex = 0;
			while (seq.MoveNext())
			{
				string a = seq.Current.ToString();
				seq.MoveNext();
				a += seq.Current;
				bytes[byteindex++] = Convert.ToByte(a, 16);
			}

			//Convert the byte array to a char array with the size of a long,
			//then return the char array as a string.
			char[] chars = new char[4];
			byte charindex = 0;
			Array.ForEach(bytes, by => {
				chars[charindex++] = Convert.ToChar(by);
				});
			return new string(chars);
		}
	}

}

