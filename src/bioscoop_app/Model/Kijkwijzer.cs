using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public sealed class Kijkwijzer : DataType
	{
		public string symbool;
		public string name;

		public Kijkwijzer(string symbool, string name)
		{
			this.symbool = symbool;
			this.name = name;
		}

		[JsonConstructor]
		public Kijkwijzer(int id, string symbool, string name)
		{
			this.id = id;
			this.symbool = symbool;
			this.name = name;
		}

		public override bool Equals(object other)
		{
			if (other == null) return false;
			if (!other.GetType().Equals(typeof(Kijkwijzer))) return false;
			Kijkwijzer that = (Kijkwijzer)other;
			if (!symbool.Equals(that.symbool)) return false;
			if (!name.Equals(that.name)) return false;
			return true;
		}
	}

}

