using Newtonsoft.Json;

namespace bioscoop_app.Model
{
	public sealed class Kijkwijzer : DataType
	{
		public string symbool;

		public Kijkwijzer(string symbool)
		{
			this.symbool = symbool;
		}

		[JsonConstructor]
		public Kijkwijzer(int id, string symbool)
		{
			this.Id = id;
			this.symbool = symbool;
		}
	}

}

