using GameLauncher.ProdUI.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;

namespace GameLauncher.ProdUI
{
	public class ShardManager
	{
		private static Dictionary<string, ShardInfo> shards;

		public static Dictionary<string, ShardInfo> Shards => shards;

		public static string ShardKey
		{
			get
			{
				return Settings.Default.Shard;
			}
			set
			{
				if (shards.ContainsKey(value))
				{
					Settings.Default.Shard = value;
					Settings.Default.Save();
				}
			}
		}

		public static string ShardName => shards[Settings.Default.Shard].ShardName;

		public static string ShardUrl => shards[Settings.Default.Shard].Url;

		public static int ShardRegionId => shards[Settings.Default.Shard].RegionId;

		public static string ShardRegion => shards[Settings.Default.Shard].RegionName;

		static ShardManager()
		{
			shards = new Dictionary<string, ShardInfo>();
			GetShardData();
		}

		private static void GetShardData()
		{
			string text = string.Empty;
			shards = new Dictionary<string, ShardInfo>();
			StringEnumerator enumerator = Settings.Default.MasterShardUrls.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					try
					{
						WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
						string url = current;
						string xml = webServicesWrapper.DoCall(url, "/getshardinfo", null, null, RequestMethod.GET);
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.LoadXml(xml);
						foreach (XmlNode childNode in xmlDocument.FirstChild.ChildNodes)
						{
							if (childNode.Name == "ShardInfo")
							{
								int.TryParse(childNode["RegionId"].InnerText, out int result);
								ShardInfo shardInfo = new ShardInfo();
								shardInfo.RegionId = result;
								shardInfo.RegionName = childNode["RegionName"].InnerText;
								shardInfo.ShardName = childNode["ShardName"].InnerText;
								shardInfo.Url = childNode["Url"].InnerText;
								shards.Add(shardInfo.ShardKey, shardInfo);
								if (string.IsNullOrEmpty(text))
								{
									text = shardInfo.ShardKey;
								}
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
			finally
			{
				(enumerator as IDisposable)?.Dispose();
			}
			if (shards.Count == 0)
			{
				GameLauncherUI.Logger.Error("GetShardData: no master shards are up");
				throw new WebServicesWrapperHttpException();
			}
			if (!shards.ContainsKey(Settings.Default.Shard))
			{
				Settings.Default.Shard = text;
			}
		}
	}
}
