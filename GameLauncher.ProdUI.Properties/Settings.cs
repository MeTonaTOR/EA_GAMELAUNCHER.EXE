using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GameLauncher.ProdUI.Properties
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default => defaultInstance;

		[UserScopedSetting]
		[DefaultSettingValue("")]
		[DebuggerNonUserCode]
		public string Email
		{
			get
			{
				return (string)this["Email"];
			}
			set
			{
				this["Email"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("False")]
		public bool RememberEmail
		{
			get
			{
				return (bool)this["RememberEmail"];
			}
			set
			{
				this["RememberEmail"] = value;
			}
		}

		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("0")]
		public int Language
		{
			get
			{
				return (int)this["Language"];
			}
			set
			{
				this["Language"] = value;
			}
		}

		[DefaultSettingValue("Language =")]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public string LanguageIdentifier => (string)this["LanguageIdentifier"];

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>[System</string>\r\n  <string>{0}</string>\r\n  <string>]</string>\r\n</ArrayOfString>")]
		public StringCollection LanguageTemplate => (StringCollection)this["LanguageTemplate"];

		[DefaultSettingValue("$(APPDATA)/Need for Speed World/Settings/UserSettings.xml")]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public string SettingFile => (string)this["SettingFile"];

		[DefaultSettingValue("UI")]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public string LanguagePath => (string)this["LanguagePath"];

		[ApplicationScopedSetting]
		[DefaultSettingValue("Language")]
		[DebuggerNonUserCode]
		public string LanguageName => (string)this["LanguageName"];

		[DefaultSettingValue("Data")]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public string BuildFolder => (string)this["BuildFolder"];

		[DefaultSettingValue("False")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public bool AutoPlay
		{
			get
			{
				return (bool)this["AutoPlay"];
			}
			set
			{
				this["AutoPlay"] = value;
			}
		}

		[DefaultSettingValue("nfsw.exe")]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public string GameExecutable => (string)this["GameExecutable"];

		[ApplicationScopedSetting]
		[DefaultSettingValue("")]
		[DebuggerNonUserCode]
		public string BackGroundImage => (string)this["BackGroundImage"];

		[DefaultSettingValue("")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string TOSHash
		{
			get
			{
				return (string)this["TOSHash"];
			}
			set
			{
				this["TOSHash"] = value;
			}
		}

		[ApplicationScopedSetting]
		[DefaultSettingValue("http://world.needforspeed.com/register")]
		[DebuggerNonUserCode]
		public string UrlCreateNewAccount => (string)this["UrlCreateNewAccount"];

		[ApplicationScopedSetting]
		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>https://94.236.124.241/nfsw/Engine.svc</string>\r\n</ArrayOfString>")]
		[DebuggerNonUserCode]
		public StringCollection MasterShardUrls => (StringCollection)this["MasterShardUrls"];

		[DefaultSettingValue("https://help.ea.com/{0}/need-for-speed/need-for-speed-world")]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public string UrlCustomerService => (string)this["UrlCustomerService"];

		[ApplicationScopedSetting]
		[DefaultSettingValue("")]
		[DebuggerNonUserCode]
		public string ForceLocale => (string)this["ForceLocale"];

		[DefaultSettingValue("-1")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public int Tracks
		{
			get
			{
				return (int)this["Tracks"];
			}
			set
			{
				this["Tracks"] = value;
			}
		}

		[ApplicationScopedSetting]
		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>TracksHigh</string>\r\n  <string>Tracks</string>\r\n</ArrayOfString>")]
		[DebuggerNonUserCode]
		public StringCollection TracksFolders => (StringCollection)this["TracksFolders"];

		[DefaultSettingValue("Tracks")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string TracksName
		{
			get
			{
				return (string)this["TracksName"];
			}
			set
			{
				this["TracksName"] = value;
			}
		}

		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>English</string>\r\n  <string>Language = EN</string>\r\n  <string>Deutsch</string>\r\n  <string>Language = DE</string>\r\n  <string>Español</string>\r\n  <string>Language = ES</string>\r\n  <string>Français</string>\r\n  <string>Language = FR</string>\r\n  <string>Polski</string>\r\n  <string>Language = PL</string>\r\n  <string>Русский</string>\r\n  <string>Language = RU</string>\r\n  <string>Português (Brasil)</string>\r\n  <string>Language = PT</string>\r\n  <string>繁體中文</string>\r\n  <string>Language = TC</string>\r\n  <string>简体中文</string>\r\n  <string>Language = SC</string>\r\n  <string>ภาษาไทย</string>\r\n  <string>Language = TH</string>\r\n  <string>Türkçe</string>\r\n  <string>Language = TR</string>\r\n</ArrayOfString>")]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public StringCollection Languages => (StringCollection)this["Languages"];

		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>en,en</string>\r\n  <string>de,de</string>\r\n  <string>es,es</string>\r\n  <string>fr,fr</string>\r\n  <string>pl,en</string>\r\n  <string>ru,ru</string>\r\n  <string>pt,en</string>\r\n  <string>tc,tw</string>\r\n  <string>sc,tw</string>\r\n  <string>th,en</string>\r\n  <string>tr,en</string>\r\n</ArrayOfString>")]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public StringCollection LanguageValues => (StringCollection)this["LanguageValues"];

		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Shard
		{
			get
			{
				return (string)this["Shard"];
			}
			set
			{
				this["Shard"] = value;
			}
		}
	}
}
