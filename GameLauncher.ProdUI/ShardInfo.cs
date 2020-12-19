namespace GameLauncher.ProdUI
{
	public class ShardInfo
	{
		public string ShardKey => $"{ShardName} - {RegionName}";

		public int RegionId
		{
			get;
			set;
		}

		public string ShardName
		{
			get;
			set;
		}

		public string RegionName
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}
	}
}
