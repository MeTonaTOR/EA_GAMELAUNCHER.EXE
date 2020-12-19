using GameLauncher.ProdUI.Properties;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace GameLauncher.ProdUI
{
	internal static class Program
	{
		private static readonly ILog mLogger = LogManager.GetLogger(typeof(Program));

		private static string GameDirRegistryKeyPath = "HKEY_LOCAL_MACHINE\\software\\Electronic Arts\\Need For Speed World";

		private static string GameDirRegistryKeyName = "GameInstallDir";

		private static string mCdnUrl;

		private static string mTermsOfService = "http://cdn.world.needforspeed.com/static/world/euala.txt";

		private static string mUpdaterFile = "GameLauncher.Updater.exe";

		private static string mApplicationDir = string.Empty;

		public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

		public static string CdnUrl => mCdnUrl;

		public static string TermsOfService => mTermsOfService;

		private static string GetGameDirFromRegistry()
		{
			try
			{
				return (string)Registry.GetValue(GameDirRegistryKeyPath, GameDirRegistryKeyName, "");
			}
			catch (Exception ex)
			{
				mLogger.Error("GetGameDirFromRegistryLocalMachine Exception: " + ex.ToString());
			}
			return "";
		}

		private static void SetGameDirFromRegistry(string gameInstallDir)
		{
			try
			{
				Registry.SetValue(GameDirRegistryKeyPath, GameDirRegistryKeyName, gameInstallDir);
			}
			catch (Exception ex)
			{
				mLogger.Error("SetGameDirFromRegistry Exception: " + ex.ToString());
			}
		}

		private static bool HasAdministrativeRight()
		{
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
			return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		private static void LaunchInAdmin(string[] args)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.Verb = "runas";
			processStartInfo.FileName = Application.ExecutablePath;
			if (args.Length > 0)
			{
				processStartInfo.Arguments = args[0];
			}
			try
			{
				Process.Start(processStartInfo);
				mLogger.Info("Executing the game launcher with admin rights");
			}
			catch (Exception ex)
			{
				mLogger.Fatal("Main Exception " + ex.ToString());
			}
		}

		private static bool HasEveryoneRight(string dirPath)
		{
			SecurityIdentifier right = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
			DirectorySecurity accessControl = Directory.GetAccessControl(dirPath);
			AuthorizationRuleCollection accessRules = accessControl.GetAccessRules(includeExplicit: true, includeInherited: true, typeof(SecurityIdentifier));
			foreach (AuthorizationRule item in accessRules)
			{
				if (item.IdentityReference == right)
				{
					return true;
				}
			}
			return false;
		}

		private static void SetEveryoneRight(string dirPath)
		{
			SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
			DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
			DirectorySecurity accessControl = directoryInfo.GetAccessControl();
			accessControl.AddAccessRule(new FileSystemAccessRule(identity, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
			directoryInfo.SetAccessControl(accessControl);
		}

		[STAThread]
		private static void Main(string[] args)
		{
			bool flag = false;
			try
			{
				bool createdNew = true;
				using (new Mutex(initiallyOwned: true, "GameLauncher", out createdNew))
				{
					if (!createdNew)
					{
						return;
					}
					if (string.IsNullOrEmpty(Settings.Default.ForceLocale))
					{
						Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;
						Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
					}
					else
					{
						try
						{
							Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.ForceLocale);
							Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Settings.Default.ForceLocale);
						}
						catch (Exception)
						{
							Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;
							Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
						}
					}
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(defaultValue: false);
					string text = GetGameDirFromRegistry();
					string currentDirectory = Environment.CurrentDirectory;
					if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
					{
						goto IL_0113;
					}
					if (!HasAdministrativeRight())
					{
						LaunchInAdmin(args);
						return;
					}
					ChooseGameFolder chooseGameFolder = new ChooseGameFolder();
					Application.Run(chooseGameFolder);
					text = chooseGameFolder.SelectedFolder;
					Directory.CreateDirectory(text);
					if (string.IsNullOrEmpty(text) || !Directory.Exists(text))
					{
						Environment.Exit(1);
					}
					SetGameDirFromRegistry(text);
					goto IL_0113;
					IL_0133:
					Environment.CurrentDirectory = text;
					XmlConfigurator.Configure(Assembly.GetExecutingAssembly().GetManifestResourceStream("GameLauncher.ProdUI.Log4NetConfig.xml"));
					Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
					IAppender[] appenders = hierarchy.GetAppenders();
					IAppender[] array = appenders;
					foreach (IAppender appender in array)
					{
						FileAppender fileAppender = appender as FileAppender;
						if (fileAppender != null)
						{
							fileAppender.File = Path.Combine(text, "trace.log");
							fileAppender.ActivateOptions();
						}
					}
					flag = true;
					List<string> list = new List<string>();
					string[] array2 = args;
					foreach (string text2 in array2)
					{
						if (text2.ToLower() == "/debug")
						{
							hierarchy.Root.Level = Level.Debug;
						}
						else
						{
							list.Add(text2);
						}
					}
					if (args.Length != list.Count)
					{
						args = list.ToArray();
					}
					try
					{
						if (File.Exists(Path.Combine(currentDirectory, "trace.log")))
						{
							File.SetAttributes(Path.Combine(currentDirectory, "trace.log"), FileAttributes.Normal);
							File.Delete(Path.Combine(currentDirectory, "trace.log"));
						}
					}
					catch (Exception)
					{
						mLogger.Debug("Cannot delete trace.log file in the local folder");
					}
					Configuration configuration = null;
					try
					{
						configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
					}
					catch (ConfigurationErrorsException ex3)
					{
						try
						{
							mLogger.Error(ex3.ToString());
							string filename = ex3.Filename;
							if (filename != null && File.Exists(filename))
							{
								File.SetAttributes(filename, FileAttributes.Normal);
								File.Delete(filename);
							}
						}
						catch (Exception ex4)
						{
							mLogger.Error("Exception when trying to delete corrupted config file");
							mLogger.Error("Main Exception: " + ex4.ToString());
						}
					}
					if (configuration == null)
					{
						configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
					}
					if (!File.Exists(configuration.FilePath))
					{
						try
						{
							Settings.Default.Upgrade();
							Settings.Default.Save();
						}
						catch (Exception ex5)
						{
							mLogger.Warn("Failed to upgrade the settings from a previous version");
							mLogger.Warn("Exception Program " + ex5.ToString());
						}
					}
					if (Process.GetProcessesByName(Settings.Default.GameExecutable.Split('.')[0]).Length > 0)
					{
						mLogger.Error("The game is running, exiting");
						MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00007"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00004"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					try
					{
						X509Certificate certificate = X509Certificate.CreateFromSignedFile(Assembly.GetExecutingAssembly().Location);
						X509Certificate2 x509Certificate = new X509Certificate2(certificate);
						if (!x509Certificate.SubjectName.Name.StartsWith("CN=Electronic Arts"))
						{
							mLogger.Error("The application is signed with the wrong certificate: " + x509Certificate.SubjectName.Name);
							MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00005"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00004"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
							Environment.Exit(1);
						}
					}
					catch (Exception ex6)
					{
						mLogger.Error("Main Exception: " + ex6.ToString());
						MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00005"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00004"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
						Environment.Exit(1);
					}
					try
					{
						mLogger.Info("--------------------------------------------------------------------------------");
						mLogger.Info("");
						mLogger.Info("Starting application: " + Assembly.GetExecutingAssembly().Location);
						mLogger.Info("OS Version: " + Environment.OSVersion);
						mLogger.Info("Locale: " + CultureInfo.CurrentCulture.Name);
						TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
						DateTime now = DateTime.Now;
						mLogger.InfoFormat("Time Zone: UTC{0} ({1})", currentTimeZone.GetUtcOffset(now), currentTimeZone.IsDaylightSavingTime(now) ? currentTimeZone.DaylightName : currentTimeZone.StandardName);
						mLogger.Info("Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
						mLogger.Info("Logging level: " + hierarchy.Root.Level);
						InitializeUpdater();
						string text3 = null;
						StringEnumerator enumerator = Settings.Default.MasterShardUrls.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								string current = enumerator.Current;
								try
								{
									text3 = GetLauncherInfo(current);
								}
								catch
								{
									continue;
								}
								break;
							}
						}
						finally
						{
							(enumerator as IDisposable)?.Dispose();
						}
						if (text3 == null)
						{
							throw new WebServicesWrapperHttpException();
						}
						if (PatchGameLauncher(text3))
						{
							Application.Run(new GameLauncherUI(args));
							mLogger.Info("Closing application");
						}
					}
					catch (WebServicesWrapperHttpException ex7)
					{
						mLogger.Error("Probably the server is down and we cannot patch");
						mLogger.Error("Main Exception: " + ex7.ToString());
						MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00063"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00004"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					catch (Exception ex8)
					{
						mLogger.Error("Main Exception: " + ex8.ToString());
						MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00008"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00004"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					goto end_IL_0012;
					IL_0113:
					if (HasEveryoneRight(text))
					{
						goto IL_0133;
					}
					if (!HasAdministrativeRight())
					{
						LaunchInAdmin(args);
						return;
					}
					SetEveryoneRight(text);
					goto IL_0133;
					end_IL_0012:;
				}
			}
			catch (ConfigurationErrorsException ex9)
			{
				if (flag)
				{
					mLogger.Error("Main ConfigurationErrorsException: " + ex9.ToString());
				}
				try
				{
					string empty = string.Empty;
					empty = (((ConfigurationErrorsException)ex9.InnerException == null) ? ex9.Filename : ((ConfigurationErrorsException)ex9.InnerException).Filename);
					if (empty != null && File.Exists(empty))
					{
						File.SetAttributes(empty, FileAttributes.Normal);
						File.Delete(empty);
					}
				}
				catch (Exception ex10)
				{
					if (flag)
					{
						mLogger.Error("Exception when trying to delete corrupted config file");
						mLogger.Error("Main Exception: " + ex10.ToString());
					}
				}
				MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00008"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00004"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			catch (Exception ex11)
			{
				if (flag)
				{
					mLogger.Error("Main Exception: " + ex11.ToString());
				}
				MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00008"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "PROGRAM00004"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private static void InitializeUpdater()
		{
			mApplicationDir = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\software\\Electronic Arts\\Need for Speed World", "GameInstallDir", "");
			mUpdaterFile = Path.Combine(mApplicationDir, mUpdaterFile);
			int num = 10;
			while (File.Exists(mUpdaterFile) && num-- > 0)
			{
				try
				{
					mLogger.Debug("Deleting updater: " + mUpdaterFile);
					File.SetAttributes(mUpdaterFile, FileAttributes.Normal);
					File.Delete(mUpdaterFile);
				}
				catch (Exception)
				{
					mLogger.Debug("Exception trying to delete " + mUpdaterFile);
					Thread.Sleep(500);
				}
			}
		}

		private static string GetLauncherInfo(string shard)
		{
			WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
			string xml = webServicesWrapper.DoCall(shard, "/launcherinfo", null, null, RequestMethod.GET);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			_ = string.Empty;
			string result = string.Empty;
			foreach (XmlNode childNode in xmlDocument.FirstChild.ChildNodes)
			{
				switch (childNode.Name)
				{
				case "gameserver":
					_ = childNode.InnerText;
					break;
				case "cdn":
					foreach (XmlNode childNode2 in childNode.ChildNodes)
					{
						if (childNode2.Name == "game")
						{
							mCdnUrl = childNode2.InnerText;
						}
						if (childNode2.Name == "launcher")
						{
							result = childNode2.InnerText;
						}
					}
					break;
				case "termsofservice":
					foreach (XmlNode childNode3 in childNode.ChildNodes)
					{
						if (childNode3.Name.ToLower() == CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower())
						{
							mTermsOfService = childNode3.InnerText;
						}
					}
					break;
				}
			}
			if (string.IsNullOrEmpty(mCdnUrl))
			{
				mLogger.Error("CDN Url not found");
				throw new Exception(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00062"));
			}
			return result;
		}

		private static string RemoveBuildFromVersion(string version)
		{
			string[] array = version.Split('.');
			if (array.Length >= 3)
			{
				return array[0] + array[1] + array[2];
			}
			return version;
		}

		private static bool PatchGameLauncher(string gameLauncherCndUrl)
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (string.IsNullOrEmpty(gameLauncherCndUrl))
			{
				mLogger.Error("GameLauncher Url not found");
				throw new Exception(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00062"));
			}
			try
			{
				try
				{
					xmlDocument.Load(gameLauncherCndUrl + "/DownloadInfo.xml");
				}
				catch
				{
				}
				XmlNode xmlNode = xmlDocument.SelectSingleNode("/update/application/version");
				XmlNode xmlNode2 = xmlDocument.SelectSingleNode("/update/application/fileset");
				string a = RemoveBuildFromVersion(AssemblyVersion);
				string b = RemoveBuildFromVersion(xmlNode.InnerText);
				bool flag = a != b;
				if (!flag)
				{
					string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					try
					{
						foreach (XmlNode childNode in xmlNode2.ChildNodes)
						{
							string value = childNode.Attributes["name"].Value;
							string text = Path.Combine(directoryName, value);
							if (!File.Exists(text))
							{
								flag = true;
								mLogger.InfoFormat("File {0} does not exists", value);
								break;
							}
							string value2 = childNode.Attributes["hash"].Value;
							string text2 = CalculateHash(text);
							if (text2 != value2)
							{
								flag = true;
								mLogger.InfoFormat("Hashes do not match for '{0}', L-{1} != R-{2}", value, text2, value2);
								break;
							}
						}
					}
					catch (Exception ex)
					{
						mLogger.Error("a problem occurred while checking for the files integrity : assuming that self update is required");
						mLogger.Error("Exception: " + ex.ToString());
						flag = true;
					}
				}
				else
				{
					mLogger.InfoFormat("Versions do not match L-{0} != R-{1}", AssemblyVersion, xmlNode.InnerText);
				}
				if (flag)
				{
					try
					{
						mLogger.Info("Patching GameLauncher");
						if (File.Exists(mUpdaterFile))
						{
							mLogger.Debug("Deleting the updater " + mUpdaterFile);
							File.SetAttributes(mUpdaterFile, FileAttributes.Normal);
							File.Delete(mUpdaterFile);
						}
						using (WebClient webClient = new WebClient())
						{
							mLogger.DebugFormat("Downloading new updater from {0} to {1}", gameLauncherCndUrl + "/GameLauncher.Updater.exe", mUpdaterFile);
							webClient.DownloadFile(gameLauncherCndUrl + "/GameLauncher.Updater.exe", mUpdaterFile);
						}
						File.SetAttributes(mUpdaterFile, FileAttributes.Normal);
						mLogger.DebugFormat("Executing updater: {0} {1} \"{2}\"", mUpdaterFile, gameLauncherCndUrl, Application.ExecutablePath);
						ProcessStartInfo processStartInfo = new ProcessStartInfo();
						processStartInfo.FileName = mUpdaterFile;
						processStartInfo.Arguments = $"{gameLauncherCndUrl} \"{Application.ExecutablePath}\"";
						processStartInfo.CreateNoWindow = true;
						processStartInfo.UseShellExecute = true;
						Process.Start(processStartInfo);
					}
					catch (Exception ex2)
					{
						MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00044"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
						mLogger.Fatal("PatchGameLauncher Problem downloading or executing the updater");
						mLogger.Fatal("PatchGameLauncher Exception: " + ex2.ToString());
					}
					return false;
				}
			}
			catch (Exception ex3)
			{
				mLogger.Fatal("Error loading/accessing file " + gameLauncherCndUrl + "/DownloadInfo.xml");
				mLogger.Error("PatchGameLauncher Exception: " + ex3.ToString());
				return false;
			}
			return true;
		}

		private static string CalculateHash(string fileName)
		{
			string empty = string.Empty;
			using (FileStream inputStream = File.OpenRead(fileName))
			{
				using (SHA512 sHA = new SHA512Managed())
				{
					empty = Convert.ToBase64String(sHA.ComputeHash(inputStream));
					mLogger.DebugFormat("Hash = '{0}'", empty);
					return empty;
				}
			}
		}
	}
}
