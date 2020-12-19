using GameLauncher.ProdUI.Properties;
using System;
using System.IO;
using System.Xml;

namespace GameLauncher.ProdUI
{
	public class UserSettings
	{
		public static XmlDocument GetSettingsFileName()
		{
			XmlDocument xmlDocument = new XmlDocument();
			string settingsPath = GetSettingsPath();
			if (File.Exists(settingsPath))
			{
				try
				{
					xmlDocument.Load(settingsPath);
					return xmlDocument;
				}
				catch (Exception ex)
				{
					GameLauncherUI.Logger.Warn("GetSettingsFile Exception: " + ex.ToString());
					GameLauncherUI.Logger.Warn("Deleting corrupted Settings file: " + settingsPath);
					File.SetAttributes(settingsPath, FileAttributes.Normal);
					File.Delete(settingsPath);
				}
			}
			XmlNode newChild = xmlDocument.CreateElement("Settings");
			xmlDocument.AppendChild(newChild);
			Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));
			xmlDocument.Save(settingsPath);
			return xmlDocument;
		}

		public static string GetSettingsPath()
		{
			string[] array = Settings.Default.SettingFile.Split('/', '\\');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].StartsWith("$(") && array[i].EndsWith(")"))
				{
					array[i] = Environment.GetEnvironmentVariable(array[i].Trim('$', '(', ')', ' '));
				}
			}
			string text = array[0];
			for (int j = 1; j < array.Length; j++)
			{
				text = text + "\\" + array[j];
			}
			return text;
		}

		public static bool UpdateUserSettingsXml()
		{
			try
			{
				XmlDocument settingsFileName = GetSettingsFileName();
				string[] array = Settings.Default.LanguagePath.Split('/', '\\');
				XmlNode xmlNode = settingsFileName["Settings"];
				string[] array2 = array;
				foreach (string name in array2)
				{
					XmlNode xmlNode2 = xmlNode[name];
					if (xmlNode2 == null)
					{
						xmlNode2 = settingsFileName.CreateElement(name);
						xmlNode.AppendChild(xmlNode2);
					}
					xmlNode = xmlNode2;
				}
				string text = Settings.Default.LanguageValues[Settings.Default.Language].Trim().Split(',')[0];
				InsertValue(settingsFileName, xmlNode, Settings.Default.LanguageName, text.ToUpper(), "string");
				InsertValue(settingsFileName, xmlNode, Settings.Default.TracksName, Settings.Default.Tracks.ToString(), "int");
				bool flag = false;
				int num = 10;
				string settingsPath = GetSettingsPath();
				XmlDocument xmlDocument = new XmlDocument();
				while (!flag && --num >= 0)
				{
					try
					{
						settingsFileName.Save(settingsPath);
						xmlDocument.Load(settingsPath);
						flag = true;
					}
					catch (XmlException)
					{
						try
						{
							if (File.Exists(settingsPath))
							{
								File.SetAttributes(settingsPath, FileAttributes.Normal);
								File.Delete(settingsPath);
							}
						}
						catch (Exception ex)
						{
							GameLauncherUI.Logger.Error("Exception when trying to delete corrupted config file - retries: " + num);
							GameLauncherUI.Logger.Error("Main Exception: " + ex.ToString());
						}
					}
					catch (Exception arg)
					{
						GameLauncherUI.Logger.Error("UpdateUserSettingsXml Exception " + arg);
					}
				}
				return true;
			}
			catch (Exception ex3)
			{
				GameLauncherUI.Logger.Error("UpdateUserSettingsXml Exception: " + ex3.ToString());
				return false;
			}
		}

		public static void InsertValue(XmlDocument settingsFile, XmlNode locationNode, string key, string value, string typeName)
		{
			XmlNode xmlNode = locationNode[key];
			if (xmlNode == null)
			{
				xmlNode = settingsFile.CreateElement(key);
				locationNode.AppendChild(xmlNode);
				XmlAttribute xmlAttribute = settingsFile.CreateAttribute("Type");
				xmlAttribute.Value = typeName;
				xmlNode.Attributes.Append(xmlAttribute);
			}
			xmlNode.InnerText = value;
		}
	}
}
