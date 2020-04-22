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
			this.Id = id;
			this.symbool = symbool;
			this.name = name;
		}
	}

}

