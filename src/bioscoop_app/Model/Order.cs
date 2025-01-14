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
		public List<Ticket> tickets;
		public string code;
		public string cust_name;
		public string cust_email;
		public bool redeemable;

		public Order(List<Product> items, List<Ticket> tickets, string cust_name, string cust_email)
		{
			this.items = items;
			this.tickets = tickets;
			code = null;
			this.cust_email = cust_email;
			this.cust_name = cust_name;
			redeemable = true;
			code = GenerateCode();
		}

		[JsonConstructor]
		public Order(int id, List<Product> items, List<Ticket> tickets, string code, string cust_name, string cust_email, bool redeemable)
		{
			Id = id;
			this.items = items;
			this.tickets = tickets;
			this.code = code;
			this.cust_email = cust_email;
			this.cust_name = cust_name;
			this.redeemable = redeemable;
		}

		/// <summary>
		/// Generates a random and unique code for this Order.
		/// </summary>
		/// <returns>An 8 character string representing the code.</returns>
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
			byte[] bytes = new byte[8];
			byte byteindex = 0;
			while (seq.MoveNext())
			{
				string a = seq.Current.ToString();
				a += seq.MoveNext() ? seq.Current.ToString() : 0.ToString();
				bytes[byteindex++] = Convert.ToByte(a, 16);
			}

			//Convert the byte array to a char array with the size of a long,
			//then return the char array as a string.
			char[] chars = new char[8];
			byte charindex = 0;
			Array.ForEach(bytes, by => {
				//Randomizes non-displayable chars.
				by = (by >= Convert.ToByte("30", 16) && by <= Convert.ToByte("7A", 16)) ? by : Convert.ToByte(new Random().Next(48, 122));
				chars[charindex++] = Convert.ToChar(by);
				});
			return new string(chars);
		}
	}

}

