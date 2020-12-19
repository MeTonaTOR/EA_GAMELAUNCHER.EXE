using GameLauncher.ProdUI.Properties;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace GameLauncher.ProdUI
{
	public class GameLauncherUI : Form
	{
		private LoadServerDataFinished mLoadServerDataFinished;

		private LoadServerStatusFinished mLoadServerStatusFinished;

		private LoadServerAdsFinished mLoadServerAdsFinished;

		private DownloadProgressUpdated mDownloadProgressUpdated;

		private DownloadStarted mDownloadStarted;

		private DownloadFinished mDownloadFinished;

		private LoginFinished mLoginFinished;

		private UpdateStarted mUpdateStarted;

		private string mCommandArgLanguage = string.Empty;

		private string mCommandArgTracks = string.Empty;

		private string mCommandArgShard = string.Empty;

		private string mCommandArgEmail = string.Empty;

		private string mCommandArgPassword = string.Empty;

		private bool mCommandBypassServerCheck;

		private string mUserId = string.Empty;

		private string mUserName = string.Empty;

		private string mRemoteUserId = string.Empty;

		private string mAuthKey = string.Empty;

		private string mWebAuthKey = string.Empty;

		private System.Threading.Timer mHeartbeatTimer;

		private bool mEngineUp;

		private bool mPortalUp;

		private string mPortalDomain = string.Empty;

		private string[] mWebApplicationData = new string[2];

		private Downloader mDownloader;

		private CommandManager mCommandManager;

		private bool mDownloadStopped = true;

		private bool mDownloadCompleted;

		private DateTime mDownloadStartTime;

		private DateTime mDownloadEndTime;

		private TimeSpan mAccumulatedDownloadTime;

		private string[] mDataUnitNames;

		private ulong mFreeBytesAvailable;

		private string mOnVerifyProgressUpdatedText = string.Empty;

		private string mOnDownloadProgressUpdatedText = string.Empty;

		private bool mPlaySuccessful;

		private int mGameLauncherSelfUpdateClosing;

		private bool mClosePending;

		private bool mFormClosing;

		private object mFormClosingLock = new object();

		public Dictionary<ScreenType, BaseScreen> screens;

		private ScreenType previousScreen;

		private ScreenType currentScreen;

		private Dictionary<string, bool> mTelemetrySentActions = new Dictionary<string, bool>();

		private static readonly ILog mLogger = LogManager.GetLogger(typeof(GameLauncherUI));

		private IContainer components;

		private System.Windows.Forms.Timer wTimerServerCheck;

		public LoadServerDataFinished LoadServerDataFinished
		{
			get
			{
				return mLoadServerDataFinished;
			}
			set
			{
				mLoadServerDataFinished = value;
			}
		}

		public LoadServerStatusFinished LoadServerStatusFinished
		{
			get
			{
				return mLoadServerStatusFinished;
			}
			set
			{
				mLoadServerStatusFinished = value;
			}
		}

		public LoadServerAdsFinished LoadServerAdsFinished
		{
			get
			{
				return mLoadServerAdsFinished;
			}
			set
			{
				mLoadServerAdsFinished = value;
			}
		}

		public DownloadProgressUpdated DownloadProgressUpdated
		{
			get
			{
				return mDownloadProgressUpdated;
			}
			set
			{
				mDownloadProgressUpdated = value;
			}
		}

		public DownloadStarted DownloadStarted
		{
			get
			{
				return mDownloadStarted;
			}
			set
			{
				mDownloadStarted = value;
			}
		}

		public DownloadFinished DownloadFinished
		{
			get
			{
				return mDownloadFinished;
			}
			set
			{
				mDownloadFinished = value;
			}
		}

		public LoginFinished LoginFinished
		{
			get
			{
				return mLoginFinished;
			}
			set
			{
				mLoginFinished = value;
			}
		}

		public UpdateStarted UpdateStarted
		{
			get
			{
				return mUpdateStarted;
			}
			set
			{
				mUpdateStarted = value;
			}
		}

		public string CommandArgLanguage => mCommandArgLanguage;

		public string CommandArgTracks => mCommandArgTracks;

		public string CommandArgShard => mCommandArgShard;

		public string CommandArgEmail => mCommandArgEmail;

		public string CommandArgPassword => mCommandArgPassword;

		public bool CommandBypassServerCheck => mCommandBypassServerCheck;

		public bool EngineUp => mEngineUp;

		public bool PortalUp
		{
			get
			{
				return mPortalUp;
			}
			set
			{
				mPortalUp = value;
			}
		}

		public string PortalDomain => mPortalDomain;

		public string[] WebApplicationData => mWebApplicationData;

		public static ILog Logger => mLogger;

		public GameLauncherUI(string[] args)
		{
			ReadCommandLine(args);
			InitializeComponent();
		}

		private void GameLauncherUI_Load(object sender, EventArgs e)
		{
			BasicConfigurator.Configure();
			mCommandManager = new CommandManager();
			mAccumulatedDownloadTime = new TimeSpan(0L);
			mWebApplicationData = Utils.RetrieveWebApplicationData();
			InitializeDataUnitNames();
			InitializeScreens();
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += loadServerDataWorker_DoWork;
			backgroundWorker.RunWorkerAsync();
			mOnVerifyProgressUpdatedText = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00046");
			mOnDownloadProgressUpdatedText = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00019");
		}

		private void InitializeDataUnitNames()
		{
			mDataUnitNames = new string[4]
			{
				ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00065"),
				ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00066"),
				ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00067"),
				ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00068")
			};
		}

		private void ReadCommandLine(string[] args)
		{
			foreach (string text in args)
			{
				if (text.ToLower().Contains("language="))
				{
					mCommandArgLanguage = text.Replace("language=", "");
				}
				else if (text.ToLower().Contains("tracks="))
				{
					mCommandArgTracks = text.Replace("tracks=", "");
				}
				else if (text.ToLower().Contains("email="))
				{
					mCommandArgEmail = text.Replace("email=", "");
				}
				else if (text.ToLower().Contains("password="))
				{
					mCommandArgPassword = text.Replace("password=", "");
				}
				else if (text.ToLower().Contains("bypassservercheck="))
				{
					mCommandBypassServerCheck = true;
				}
			}
		}

		private void InitializeScreens()
		{
			screens = new Dictionary<ScreenType, BaseScreen>();
			screens.Add(ScreenType.Options, new OptionsScreen(this));
			screens.Add(ScreenType.Login, new SSOLoginScreen(this));
			screens.Add(ScreenType.Download, new DownloadScreen(this));
			SwitchScreen(ScreenType.Login);
		}

		public void ReplaceScreen(ScreenType screenType, BaseScreen screen)
		{
			if (currentScreen == screenType)
			{
				base.Controls.Remove(screens[screenType]);
				screens[screenType] = screen;
				base.Controls.Add(screens[screenType]);
				screens[screenType].LoadScreen();
			}
		}

		public void SwitchToPreviousScreen()
		{
			if (previousScreen != 0)
			{
				screens[currentScreen].UnloadScreen();
				base.Controls.Remove(screens[currentScreen]);
				screens[previousScreen].LoadScreen();
				base.Controls.Add(screens[previousScreen]);
				ScreenType screenType = previousScreen;
				previousScreen = currentScreen;
				currentScreen = screenType;
			}
		}

		public void SwitchScreen(ScreenType newScreen)
		{
			if (screens.ContainsKey(newScreen))
			{
				previousScreen = currentScreen;
				if (currentScreen != 0)
				{
					screens[currentScreen].UnloadScreen();
					base.Controls.Remove(screens[currentScreen]);
				}
				currentScreen = newScreen;
				base.Controls.Add(screens[newScreen]);
				screens[newScreen].LoadScreen();
			}
		}

		private void loadServerDataWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			LoadServerData();
		}

		public void LoadServerData()
		{
			mEngineUp = false;
			string shard = Settings.Default.Shard;
			if (string.IsNullOrEmpty(shard))
			{
				return;
			}
			string shardUrl = ShardManager.ShardUrl;
			try
			{
				WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
				string xml = webServicesWrapper.DoCall(shardUrl, "/systeminfo", null, null, RequestMethod.GET);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
				foreach (XmlNode childNode in xmlDocument.FirstChild.ChildNodes)
				{
					if (childNode.Name == "PortalDomain")
					{
						mPortalDomain = childNode.InnerText;
						if (!mPortalDomain.ToLower().StartsWith("http://"))
						{
							mPortalDomain = "http://" + mPortalDomain;
						}
						break;
					}
					mEngineUp = true;
				}
			}
			catch (Exception ex)
			{
				mLogger.Error("LoadServerData Exception: " + ex.ToString());
				mPortalDomain = "http://webkit.world.needforspeed.com";
				mEngineUp = false;
			}
			if (mLoadServerDataFinished != null)
			{
				BeginInvoke(mLoadServerDataFinished);
			}
			LoadServerStatusAsync();
		}

		private void SendHeartbeat(object stateInfo)
		{
			try
			{
				mLogger.Debug("Sending heartbeat");
				string shardUrl = ShardManager.ShardUrl;
				WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
				string[][] extraHeaders = new string[4][]
				{
					new string[2]
					{
						"Content-Type",
						"text/xml;charset=utf-8"
					},
					new string[2]
					{
						"Content-Length",
						"0"
					},
					new string[2]
					{
						"userId",
						mUserId
					},
					new string[2]
					{
						"securityToken",
						mAuthKey
					}
				};
				webServicesWrapper.DoCall(shardUrl, "/heartbeatLauncher", null, extraHeaders, null, RequestMethod.POST);
			}
			catch (Exception ex)
			{
				mLogger.Error("SendHeartbeat Exception: " + ex.ToString());
			}
		}

		public void PerformLogin(string username, string password)
		{
			mLogger.Info("Login button pressed");
			Login(username, password);
		}

		public void PerformUpdate()
		{
			if (!mDownloadCompleted)
			{
				mLogger.Info("Update button pressed");
				SendTelemetry("update_button_start");
				StartDownload();
			}
		}

		public void PerformPlay()
		{
			if (!mDownloadCompleted)
			{
				return;
			}
			try
			{
				mLogger.Info("Play button pressed");
				SendTelemetry("play_button_start");
				if (UserSettings.UpdateUserSettingsXml())
				{
					string text = ShardManager.ShardUrl.Trim();
					string shardRegion = ShardManager.ShardRegion;
					Process process = new Process();
					string path = Path.Combine(Directory.GetCurrentDirectory(), Settings.Default.BuildFolder);
					process.StartInfo.FileName = Path.Combine(path, Settings.Default.GameExecutable);
					if (!File.Exists(process.StartInfo.FileName))
					{
						SendTelemetry("file_exists_failed");
						MessageBox.Show(string.Format(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00030"), process.StartInfo.FileName), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					try
					{
						X509Certificate certificate = X509Certificate.CreateFromSignedFile(process.StartInfo.FileName);
						X509Certificate2 x509Certificate = new X509Certificate2(certificate);
						if (!x509Certificate.SubjectName.Name.StartsWith("CN=Electronic Arts"))
						{
							SendTelemetry("certificate_wrong");
							mLogger.Error("The game executable is signed with the wrong certificate: " + x509Certificate.SubjectName.Name);
							MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00054"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
					}
					catch (Exception ex)
					{
						SendTelemetry("certificate_failed");
						mLogger.Error("wLabelButtonPlay_Click Exception: " + ex.ToString());
						MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00054"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					SendBinaryHash(process.StartInfo.FileName, "Client");
					process.StartInfo.Arguments = $"{shardRegion} {text} {mAuthKey} {mUserId}";
					if (!SendStartTelemetry(text))
					{
						SendTelemetry("engine_telemetry_failed");
						MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00059"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						PerformLogout();
						return;
					}
					mPlaySuccessful = true;
					SendClosingTelemetry(CloseReason.UserClosing);
					SendTelemetry("game_start");
					process.Start();
					SendTelemetry("game_started");
					Close();
				}
				else
				{
					SendTelemetry("save_settings_failed");
				}
			}
			catch (Exception ex2)
			{
				SendTelemetry("play_button_failed");
				mLogger.Error("PerformPlay Exception: " + ex2.ToString());
				MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00031") + Environment.NewLine + Environment.NewLine + ex2.Message, ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		public bool PerformLogout()
		{
			Cursor = Cursors.WaitCursor;
			StopDownload();
			SendTelemetry("logout");
			mAuthKey = null;
			wTimerServerCheck.Enabled = true;
			Cursor.Current = Cursors.Default;
			mHeartbeatTimer.Dispose();
			mHeartbeatTimer = null;
			return true;
		}

		public bool SingleSignOnLogin(string authToken)
		{
			string text = "";
			try
			{
				Dictionary<string, string> dictionary = null;
				string shardKey = ShardManager.ShardKey;
				string shardUrl = ShardManager.ShardUrl;
				mLogger.InfoFormat("Trying to log into shard'{0}:{1}' and region '{2}' with auth token '{3}'", shardKey, shardUrl, ShardManager.ShardRegion, authToken);
				SendTelemetry("login_try");
				WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
				bool flag = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
				SendTelemetry(flag ? "login_admin" : "login_no_admin");
				SendTelemetry($"lang_{CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower()}");
				try
				{
					dictionary = Engine.Instance.SSOLogin(shardUrl, authToken, ShardManager.ShardRegion);
				}
				catch (WebServicesWrapperServerException ex)
				{
					if (ex.ErrorCode != -1730 && ex.ErrorCode != -1612)
					{
						mLogger.ErrorFormat("SSO Login exception WebServicesWrapperServerException: {0} - {1}", ex.ErrorCode, ex.ToString());
						SendTelemetry($"sso_login_error_{ex.ErrorCode.ToString()}");
						ShowInternalErrorMessage(ex.ErrorCode, ex.Message);
						return false;
					}
					if (!ShowTos(force: true))
					{
						return false;
					}
					dictionary = Engine.Instance.SSOLogin(shardUrl, authToken, ShardManager.ShardRegion, eualaAccepted: true);
				}
				text = dictionary["username"];
				mUserId = dictionary["userId"];
				mAuthKey = dictionary["securityToken"];
				mRemoteUserId = dictionary["remoteUserId"];
				if (!ShowTos(force: false))
				{
					return false;
				}
				mWebAuthKey = GetWebSecurityToken();
				SendBinaryHash(Assembly.GetExecutingAssembly().Location, "Downloader");
				if (mLoginFinished != null)
				{
					BeginInvoke(mLoginFinished, mRemoteUserId, mWebAuthKey);
				}
				mHeartbeatTimer = new System.Threading.Timer(SendHeartbeat, this, 900000, 900000);
			}
			catch (WebServicesWrapperServerException ex2)
			{
				mLogger.ErrorFormat("SSO Login exception WebServicesWrapperServerException: {0} - {1}", ex2.ErrorCode, ex2.ToString());
				SendTelemetry($"sso_login_error_{ex2.ErrorCode.ToString()}");
				ShowInternalErrorMessage(ex2.ErrorCode, ex2.Message);
				return false;
			}
			catch
			{
				mLogger.Error("SSO Login exception");
				return false;
			}
			mLogger.Info("SSO Login successful");
			LoginSuccessful(text);
			return true;
		}

		private void Login(string username, string password)
		{
			try
			{
				Dictionary<string, string> dictionary = null;
				string shard = Settings.Default.Shard;
				string shardUrl = ShardManager.ShardUrl;
				mLogger.InfoFormat("Trying to log into shard'{0}:{1}' and region '{2}' with user '{3}'", shard, shardUrl, ShardManager.ShardRegion, username);
				SendTelemetry("login_try");
				WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
				bool flag = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
				SendTelemetry(flag ? "login_admin" : "login_no_admin");
				SendTelemetry($"lang_{CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower()}");
				try
				{
					dictionary = Engine.Instance.Login(shardUrl, username, password, ShardManager.ShardRegion);
				}
				catch (WebServicesWrapperServerException ex)
				{
					if (ex.ErrorCode != -1730 && ex.ErrorCode != -1612)
					{
						throw;
					}
					if (!ShowTos(force: true))
					{
						return;
					}
					WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
					webServicesWrapper.DoCall(mPortalDomain.Replace("http:", "https:"), $"/SpeedAPI/ws/game/nfsw/euala/accept?email={username}&password={password}", null, null, RequestMethod.POST);
					dictionary = Engine.Instance.Login(shardUrl, username, password, ShardManager.ShardRegion);
				}
				mUserId = dictionary["userId"];
				mAuthKey = dictionary["securityToken"];
				mRemoteUserId = dictionary["remoteUserId"];
				if (ShowTos(force: false))
				{
					mWebAuthKey = GetWebSecurityToken();
					SendBinaryHash(Assembly.GetExecutingAssembly().Location, "Downloader");
					if (mLoginFinished != null)
					{
						BeginInvoke(mLoginFinished, mRemoteUserId, mWebAuthKey);
					}
					mHeartbeatTimer = new System.Threading.Timer(SendHeartbeat, this, 900000, 900000);
					mLogger.Info("Logging successful");
					LoginSuccessful(username);
				}
			}
			catch (WebServicesWrapperServerException ex2)
			{
				mLogger.ErrorFormat("Login WebServicesWrapperServerException: {0} - {1}", ex2.ErrorCode, ex2.ToString());
				SendTelemetry($"login_error_{ex2.ErrorCode.ToString()}");
				ShowInternalErrorMessage(ex2.ErrorCode, ex2.Message);
			}
			catch (WebServicesWrapperHttpException ex3)
			{
				mLogger.Error("Login WebServicesWrapperHTTPException: " + ex3.ToString());
				SendTelemetry("login_error_http");
				ShowErrorMessage(ex3.Message);
			}
			catch (Exception ex4)
			{
				mLogger.Error("Login Exception: " + ex4.ToString());
				SendTelemetry("login_error_unknown");
				ShowErrorMessage(ex4.Message);
			}
		}

		public bool SetRegion()
		{
			try
			{
				Engine.Instance.SetRegion(ShardManager.ShardUrl, mUserId, mAuthKey, ShardManager.ShardRegionId);
			}
			catch (Exception ex)
			{
				mLogger.Error("Set Region Exception: " + ex.ToString());
				return false;
			}
			return true;
		}

		private string GetWebSecurityToken()
		{
			string[][] extraHeaders = new string[4][]
			{
				new string[2]
				{
					"Content-Type",
					"text/xml;charset=utf-8"
				},
				new string[2]
				{
					"Content-Length",
					"0"
				},
				new string[2]
				{
					"userId",
					mUserId
				},
				new string[2]
				{
					"securityToken",
					mAuthKey
				}
			};
			WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
			string xml = webServicesWrapper.DoCall(ShardManager.ShardUrl, "/security/generatewebtoken", null, extraHeaders, null, RequestMethod.POST);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			return xmlDocument.InnerText;
		}

		private bool ShowTos(bool force)
		{
			try
			{
				using (WebClient webClient = new WebClient())
				{
					byte[] array = webClient.DownloadData(Program.TermsOfService);
					using (MD5 mD = MD5.Create())
					{
						string text = Convert.ToBase64String(mD.ComputeHash(array));
						if (force || Settings.Default.TOSHash != text)
						{
							SendTelemetry("tos_show");
							using (Tos tos = new Tos(Encoding.UTF8.GetString(array)))
							{
								if (tos.ShowDialog() != DialogResult.OK)
								{
									mLogger.Info("Terms of Service not accepted");
									mUserId = string.Empty;
									mAuthKey = string.Empty;
									MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00052"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00053"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									SendTelemetry("tos_declined");
									return false;
								}
								Settings.Default.TOSHash = text;
								Settings.Default.Save();
								SendTelemetry("tos_accepted");
								mLogger.Info("Terms of Service accepted");
							}
						}
					}
				}
			}
			catch (Exception)
			{
				SendTelemetry("tos_exception");
				mLogger.Info("Terms of Service exception - skipping");
			}
			return true;
		}

		private void LoginSuccessful(string username)
		{
			SendTelemetry("login_success");
			mUserName = username;
			wTimerServerCheck.Enabled = false;
			SwitchScreen(ScreenType.Download);
		}

		private void ScheduleDownloadingProcess()
		{
			mDownloader = new Downloader(this);
			mDownloader.DeleteFinished = OnDeleteFinished;
			mDownloader.DeleteFailed = OnDeleteFailed;
			DeleteCommand command = new DeleteCommand(mDownloader);
			mCommandManager.AddBack(command, new DeleteCommandArgument(Program.CdnUrl, string.Empty, Settings.Default.BuildFolder));
			mDownloader = new Downloader(this, 3, 2, 64);
			mDownloader.VerifyProgressUpdated = OnVerifyProgressUpdated;
			mDownloader.VerifyFinished = OnVerifyFinished;
			mDownloader.VerifyFailed = OnVerifyFailed;
			mDownloader.DownloadProgressUpdated = OnDownloadProgressUpdated;
			mDownloader.DownloadFinished = OnDownloadFinished;
			mDownloader.DownloadFailed = OnDownloadFailed;
			mDownloader.ShowMessage = OnShowMessage;
			mDownloader.SendTelemetry = SendTelemetry;
			VerifyCommand command2 = new VerifyCommand(mDownloader);
			mCommandManager.AddBack(command2, new VerifyCommandArgument(Program.CdnUrl, string.Empty, Settings.Default.BuildFolder, stopOnFail: false, clearHashes: false, writeHashes: false, download: true));
			string text = string.Empty;
			try
			{
				text = Settings.Default.TracksFolders[Settings.Default.Tracks];
				mLogger.DebugFormat("Tracks package for '{0}' found, scheduling verification", text);
				mCommandManager.AddBack(command, new DeleteCommandArgument(Program.CdnUrl, text, Settings.Default.BuildFolder));
				mCommandManager.AddBack(command2, new VerifyCommandArgument(Program.CdnUrl, text, Settings.Default.BuildFolder, stopOnFail: false, clearHashes: false, writeHashes: false, download: true));
			}
			catch (Exception)
			{
				mLogger.ErrorFormat("The tracks package '{0}' does not exit", text);
			}
			string text2 = string.Empty;
			try
			{
				text2 = Settings.Default.LanguageValues[Settings.Default.Language].Trim().Split(',')[1];
				mLogger.DebugFormat("Language package for '{0}' found, scheduling verification", text2);
				mCommandManager.AddBack(command, new DeleteCommandArgument(Program.CdnUrl, text2, Settings.Default.BuildFolder));
				mCommandManager.AddBack(command2, new VerifyCommandArgument(Program.CdnUrl, text2, Settings.Default.BuildFolder, stopOnFail: false, clearHashes: false, writeHashes: false, download: true));
			}
			catch (Exception)
			{
				mLogger.ErrorFormat("The language package '{0}' does not exit", text2);
			}
		}

		public void StartDownload()
		{
			if (mDownloadStopped)
			{
				mDownloadStopped = false;
				ScheduleDownloadingProcess();
				string lpDirectoryName = Path.GetFullPath(Environment.CurrentDirectory).Split('\\')[0];
				UnsafeNativeMethods.GetDiskFreeSpaceEx(lpDirectoryName, out mFreeBytesAvailable, out ulong _, out ulong _);
				mDownloadStartTime = DateTime.Now;
				if (mCommandManager.Count > 0)
				{
					mLogger.Debug("Executing next in command queue");
					mCommandManager.ExecuteNext();
				}
				if (mDownloadStarted != null)
				{
					BeginInvoke(mDownloadStarted);
				}
			}
		}

		public void StopDownload()
		{
			if (!mDownloadStopped)
			{
				if (mDownloader != null)
				{
					mDownloader.VerifyProgressUpdated = null;
					mDownloader.VerifyFinished = null;
					mDownloader.VerifyFailed = null;
					mDownloader.DownloadProgressUpdated = null;
					mDownloader.DownloadFinished = null;
					mDownloader.DownloadFailed = null;
				}
				if (mCommandManager.CurrentCommand != null && (mCommandManager.CurrentCommand.Downloader.Downloading || mCommandManager.CurrentCommand.Downloader.Verifying))
				{
					mCommandManager.CurrentCommand.Downloader.Stop();
					mCommandManager.Clear();
				}
				mDownloadCompleted = false;
				mDownloadStopped = true;
				if (mDownloadProgressUpdated != null)
				{
					BeginInvoke(mDownloadProgressUpdated, false, 0, " ");
				}
			}
		}

		private string FormatFileSize(long byteCount)
		{
			double[] array = new double[4]
			{
				1073741824.0,
				1048576.0,
				1024.0,
				1.0
			};
			for (int i = 0; i < array.Length; i++)
			{
				if ((double)byteCount >= array[i])
				{
					return $"{(double)byteCount / array[i]:0.00}" + mDataUnitNames[i];
				}
			}
			return "0 " + mDataUnitNames[3];
		}

		private string EstimateFinishTime(long current, long total, DateTime downloadPartStart)
		{
			double num = (double)current / (double)total;
			if (num < 0.05000000074505806)
			{
				return ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00018");
			}
			TimeSpan t = DateTime.Now - downloadPartStart;
			TimeSpan timeSpan = TimeSpan.FromTicks((long)((double)t.Ticks / num)) - t;
			return string.Format("{0}:{1}:{2}", timeSpan.Hours, timeSpan.Minutes.ToString("D02"), timeSpan.Seconds.ToString("D02"));
		}

		private void OnDownloadProgressUpdated(long downloadLength, long downloadCurrent, long compressedLength, string filename, DateTime downloadPartStart)
		{
			string text = null;
			if (downloadCurrent < compressedLength)
			{
				text = string.Format(mOnDownloadProgressUpdatedText, FormatFileSize(downloadCurrent), FormatFileSize(compressedLength), EstimateFinishTime(downloadCurrent, compressedLength, downloadPartStart));
			}
			if (mDownloadProgressUpdated != null)
			{
				BeginInvoke(mDownloadProgressUpdated, true, (int)(100 * downloadCurrent / compressedLength), text);
			}
		}

		private void OnDownloadFinished()
		{
			mLogger.Info("Download Finished");
			if (mDownloadProgressUpdated != null)
			{
				BeginInvoke(mDownloadProgressUpdated, true, 100, ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00020"));
			}
			mDownloadEndTime = DateTime.Now;
			TimeSpan t = mDownloadEndTime - mDownloadStartTime;
			mLogger.InfoFormat("Download successful time: {0} s", t.TotalSeconds);
			if (t > mAccumulatedDownloadTime)
			{
				mAccumulatedDownloadTime = t;
			}
			if (mClosePending)
			{
				Close();
			}
			else
			{
				if (mDownloadStopped)
				{
					return;
				}
				if (mCommandManager.Count > 0)
				{
					if (mDownloadProgressUpdated != null)
					{
						BeginInvoke(mDownloadProgressUpdated, true, 0, ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00017"));
					}
					mLogger.Debug("Executing next in command queue");
					mCommandManager.ExecuteNext();
				}
				else
				{
					mDownloadCompleted = true;
					if (mDownloadFinished != null)
					{
						BeginInvoke(mDownloadFinished);
					}
				}
			}
		}

		private void OnDownloadFailed(Exception ex)
		{
			mLogger.Info("Download Failed");
			DownloadFailed();
			mDownloadEndTime = DateTime.Now;
			mLogger.InfoFormat("Download failed time: {0} s", new TimeSpan(mDownloadEndTime.Ticks - mDownloadStartTime.Ticks).TotalSeconds);
			TimeSpan t = new TimeSpan(mDownloadEndTime.Ticks - mDownloadStartTime.Ticks);
			if (t > mAccumulatedDownloadTime)
			{
				mAccumulatedDownloadTime = t;
			}
			if (mClosePending)
			{
				Close();
			}
			else
			{
				if (mDownloadStopped || ex == null)
				{
					return;
				}
				mLogger.Error("OnDownloadFailed Exception: " + ex.ToString());
				if (ex is UncompressionException)
				{
					int errorCode = ((UncompressionException)ex).ErrorCode;
					if (errorCode == 9)
					{
						MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00055"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					else
					{
						MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00051"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else if (ex is DownloaderException)
				{
					using (CustomMessageBox customMessageBox = new CustomMessageBox(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00062"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), CustomMessageBox.CustomMessageBoxButtons.OK, CustomMessageBox.CustomMessageBoxIcon.Error))
					{
						customMessageBox.ShowDialog(this);
					}
				}
				else
				{
					MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00051"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private void OnVerifyProgressUpdated(long downloadLength, long downloadCurrent, long compressedLength, string filename, DateTime downloadPartStart)
		{
			string text = (downloadCurrent >= downloadLength) ? ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00047") : string.Format(mOnVerifyProgressUpdatedText, FormatFileSize(downloadCurrent), FormatFileSize(downloadLength), EstimateFinishTime(downloadCurrent, downloadLength, downloadPartStart));
			if (mDownloadProgressUpdated != null)
			{
				BeginInvoke(mDownloadProgressUpdated, true, (int)(100 * downloadCurrent / downloadLength), text);
			}
		}

		private void OnVerifyFinished()
		{
			mLogger.Info("Verify Finished");
			Cursor.Current = Cursors.Default;
			if (mDownloadProgressUpdated != null)
			{
				BeginInvoke(mDownloadProgressUpdated, true, 100, ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00047"));
			}
		}

		private void OnVerifyFailed(Exception ex)
		{
			mLogger.Info("Verify Failed");
			SendTelemetry("verify_failed");
			Cursor.Current = Cursors.Default;
			if (ex != null)
			{
				if (ex is VerificationException)
				{
					ulong sizeRequired = ((VerificationException)ex).SizeRequired;
					if ((double)sizeRequired * 1.05 > (double)mFreeBytesAvailable)
					{
						SendTelemetry("verify_failed_disk_full");
						string arg = Path.GetFullPath(Environment.CurrentDirectory).Split('\\')[0];
						MessageBox.Show(string.Format(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00056"), arg, (double)sizeRequired * 1.05 / 1024.0 / 1024.0, mFreeBytesAvailable / 1024uL / 1024uL), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
						mDownloadCompleted = false;
						if (mDownloadProgressUpdated != null)
						{
							BeginInvoke(mDownloadProgressUpdated, true, 0, ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00025"));
						}
						if (mClosePending)
						{
							Close();
						}
					}
					else
					{
						SendTelemetry("verify_failed_exception");
					}
				}
				else if (ex is DownloaderException)
				{
					SendTelemetry("verify_failed_download");
					mCommandManager.Clear();
					MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00044"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					DownloadFailed();
				}
				else
				{
					SendTelemetry("verify_failed_unknown");
					mCommandManager.Clear();
					MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00045"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					DownloadFailed();
				}
			}
			else
			{
				DownloadFailed();
			}
		}

		private void DownloadFailed()
		{
			SendTelemetry("download_failed");
			if (mUpdateStarted != null)
			{
				BeginInvoke(mUpdateStarted);
			}
			if (mClosePending)
			{
				Close();
			}
		}

		private void OnDeleteFinished()
		{
			mLogger.Info("Delete Finished");
			Cursor.Current = Cursors.Default;
			if (mClosePending)
			{
				Close();
			}
			else
			{
				if (mDownloadStopped)
				{
					return;
				}
				if (mCommandManager.Count > 0)
				{
					mLogger.Debug("Executing next in command queue");
					mCommandManager.ExecuteNext();
					return;
				}
				mLogger.Error("No command scheduled after delete");
				mDownloadCompleted = false;
				if (mUpdateStarted != null)
				{
					BeginInvoke(mUpdateStarted);
				}
			}
		}

		private void OnDeleteFailed(Exception ex)
		{
			mLogger.Info("Delete Failed");
			SendTelemetry("delete_failed");
			Cursor.Current = Cursors.Default;
			if (ex != null)
			{
				return;
			}
			if (mClosePending)
			{
				Close();
			}
			else
			{
				if (mDownloadStopped)
				{
					return;
				}
				if (mCommandManager.Count > 0)
				{
					mLogger.Debug("Executing next in command queue");
					mCommandManager.ExecuteNext();
					return;
				}
				mLogger.Error("No command scheduled after delete");
				mDownloadCompleted = false;
				if (mUpdateStarted != null)
				{
					BeginInvoke(mUpdateStarted);
				}
			}
		}

		private void OnShowMessage(string message, string header)
		{
			MessageBox.Show(message, header);
		}

		private void ShowErrorMessage(string message)
		{
			using (CustomMessageBox customMessageBox = new CustomMessageBox(message, ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), CustomMessageBox.CustomMessageBoxButtons.OK, CustomMessageBox.CustomMessageBoxIcon.Error))
			{
				customMessageBox.ShowDialog(this);
			}
		}

		private void ShowInternalErrorMessage(int errorCode, string message)
		{
			string empty = string.Empty;
			try
			{
				switch (errorCode)
				{
				case 6:
					empty = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00045");
					break;
				case -1807:
				case -778:
				case -777:
				case -775:
				case -774:
				case -773:
				case -747:
				case -746:
				case -745:
				case -714:
				case -713:
				case -712:
				case -710:
				case -708:
				case -706:
					empty = string.Format(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00057"), errorCode);
					break;
				case -521:
					empty = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00071");
					break;
				case -523:
				case -522:
					empty = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00058");
					break;
				case -1730:
				case -1612:
					empty = string.Format(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00060"), "?lang=" + CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower());
					break;
				case -1613:
				case -748:
					empty = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00061");
					break;
				case -750:
					empty = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00081");
					break;
				case -520:
					empty = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00082");
					break;
				default:
					mLogger.DebugFormat("Server error '{0}' not found", errorCode);
					empty = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00045");
					break;
				}
			}
			catch (Exception ex)
			{
				mLogger.Debug("ShowInternalErrorMessage Exception: " + ex.Message);
				empty = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00045");
			}
			using (CustomMessageBox customMessageBox = new CustomMessageBox(empty, ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00011"), CustomMessageBox.CustomMessageBoxButtons.OK, CustomMessageBox.CustomMessageBoxIcon.Error))
			{
				customMessageBox.ShowDialog(this);
			}
		}

		private void SendTelemetry(string action)
		{
			if (mPortalUp && !mTelemetrySentActions.ContainsKey(action))
			{
				BackgroundWorker backgroundWorker = new BackgroundWorker();
				backgroundWorker.DoWork += SendTelemetry_DoWork;
				backgroundWorker.RunWorkerAsync(action);
				mTelemetrySentActions.Add(action, value: true);
			}
		}

		private void SendTelemetry_DoWork(object sender, DoWorkEventArgs args)
		{
			string text = args.Argument as string;
			try
			{
				string arg = "http";
				mLogger.Info($"Sending telemetry with action={text}");
				string arg2 = $"gl_{arg}_{Program.AssemblyVersion}";
				WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
				webServicesWrapper.DoCall(mPortalDomain, $"/SpeedAPI/ws/game/nfsw/telemetry?category={arg2}&action={text}", null, null, RequestMethod.POST);
			}
			catch (Exception ex)
			{
				mLogger.Error("SendTelemetry Exception: " + ex.ToString());
			}
		}

		private bool SendStartTelemetry(string serverUrl)
		{
			try
			{
				mLogger.Info("Sending start client telemetry");
				WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
				StringBuilder stringBuilder = new StringBuilder();
				XmlWriter xmlWriter = new XmlTextWriter(new StringWriter(stringBuilder));
				xmlWriter.WriteStartElement("LauncherStartTrans", "http://schemas.datacontract.org/2004/07/Victory.DataLayer.Serialization");
				xmlWriter.WriteStartElement("autoLogin");
				xmlWriter.WriteString(Settings.Default.RememberEmail ? "1" : "0");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("autoStart");
				xmlWriter.WriteString("0");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("language");
				xmlWriter.WriteString($"{Settings.Default.Language + 1}");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("region");
				xmlWriter.WriteString($"{ShardManager.ShardRegionId}");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("sku");
				xmlWriter.WriteString("");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("userID");
				xmlWriter.WriteString(mUserId);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
				xmlWriter.Close();
				string[][] extraHeaders = new string[4][]
				{
					new string[2]
					{
						"Content-Type",
						"text/xml;charset=utf-8"
					},
					new string[2]
					{
						"Content-Length",
						stringBuilder.Length.ToString(CultureInfo.InvariantCulture)
					},
					new string[2]
					{
						"userId",
						mUserId
					},
					new string[2]
					{
						"securityToken",
						mAuthKey
					}
				};
				webServicesWrapper.DoCall(serverUrl, "/Reporting/LauncherPatcherStart/", null, extraHeaders, stringBuilder.ToString(), RequestMethod.POST);
				mLogger.Info("start client telemetry sent successfully");
			}
			catch (WebServicesWrapperServerException ex)
			{
				mLogger.Error("SendStartTelemetry WebServicesWrapperServerException: " + ex.ToString());
				if (ex.ErrorCode == 1503)
				{
					return false;
				}
			}
			catch (WebServicesWrapperHttpException ex2)
			{
				mLogger.Error("SendStartTelemetry WebServicesWrapperHTTPException: " + ex2.ToString());
				return false;
			}
			catch (Exception ex3)
			{
				mLogger.Error("SendStartTelemetry Exception: " + ex3.ToString());
			}
			return true;
		}

		private void SendClosingTelemetry(CloseReason reason)
		{
			string text = "0";
			if (mDownloadCompleted)
			{
				text = ((long)mAccumulatedDownloadTime.TotalSeconds).ToString(CultureInfo.InvariantCulture);
			}
			double totalMinutes = mAccumulatedDownloadTime.TotalMinutes;
			string arg = (totalMinutes < 15.0) ? "15" : ((totalMinutes < 30.0) ? "30" : ((totalMinutes < 45.0) ? "45" : ((totalMinutes < 60.0) ? "60" : ((totalMinutes < 90.0) ? "90" : ((totalMinutes < 120.0) ? "120" : ((totalMinutes < 180.0) ? "180" : ((!(totalMinutes < 240.0)) ? "240plus" : "240")))))));
			SendTelemetry(string.Format("patch_time_{0}_{1}", arg, mDownloadCompleted ? "completed" : "partial"));
			try
			{
				mLogger.Info("Sending closing gamelauncher telemetry");
				string shardUrl = ShardManager.ShardUrl;
				WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
				StringBuilder stringBuilder = new StringBuilder();
				XmlWriter xmlWriter = new XmlTextWriter(new StringWriter(stringBuilder));
				xmlWriter.WriteStartElement("LauncherEndTrans", "http://schemas.datacontract.org/2004/07/Victory.DataLayer.Serialization");
				string text2 = "3";
				switch (reason)
				{
				case CloseReason.UserClosing:
					text2 = ((mGameLauncherSelfUpdateClosing != 1) ? ((mGameLauncherSelfUpdateClosing != 2) ? ((mGameLauncherSelfUpdateClosing != 3) ? (mPlaySuccessful ? "1" : "2") : "8") : "7") : "6");
					break;
				case CloseReason.WindowsShutDown:
					text2 = "4";
					break;
				case CloseReason.TaskManagerClosing:
					text2 = "5";
					break;
				}
				xmlWriter.WriteStartElement("leaveReasonID");
				xmlWriter.WriteString(text2);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("patchTime");
				xmlWriter.WriteString(text);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("userID");
				xmlWriter.WriteString(mUserId);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
				xmlWriter.Close();
				string[][] extraHeaders = new string[4][]
				{
					new string[2]
					{
						"Content-Type",
						"text/xml;charset=utf-8"
					},
					new string[2]
					{
						"Content-Length",
						stringBuilder.Length.ToString(CultureInfo.InvariantCulture)
					},
					new string[2]
					{
						"userId",
						mUserId
					},
					new string[2]
					{
						"securityToken",
						mAuthKey
					}
				};
				webServicesWrapper.DoCall(shardUrl, "/Reporting/LauncherPatcherEnd/", null, extraHeaders, stringBuilder.ToString(), RequestMethod.POST);
				mLogger.Info($"Closing telemetry sent successfully : leave reason : {text2}");
				SendTelemetry($"leave_reason_{text2}");
			}
			catch (Exception ex)
			{
				mLogger.Error("SendClosingTelemetry Exception: " + ex.ToString());
			}
		}

		private void SendBinaryHash(string fileName, string hashId)
		{
			mLogger.DebugFormat("Calculate the hash for {0} {1}", hashId, fileName);
			try
			{
				SendHash(CalculateHash(fileName), hashId);
			}
			catch (Exception ex)
			{
				mLogger.Error("SendBinaryHash Exception: " + ex.ToString());
			}
		}

		private string CalculateHash(string fileName)
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

		private void SendHash(string hash, string hashId)
		{
			try
			{
				mLogger.Debug("Sending Hash for " + hashId);
				WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
				StringBuilder stringBuilder = new StringBuilder();
				XmlWriter xmlWriter = new XmlTextWriter(new StringWriter(stringBuilder));
				xmlWriter.WriteStartElement("FraudDetectionCollection", "http://schemas.datacontract.org/2004/07/Victory.DataLayer.Serialization");
				xmlWriter.WriteStartElement("FraudDetectionLogs");
				xmlWriter.WriteStartElement("FraudDetection");
				xmlWriter.WriteStartElement("IsEncrypted");
				xmlWriter.WriteString("false");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("ModuleName");
				xmlWriter.WriteString(EncodeBase64(hashId));
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("ModulePath");
				xmlWriter.WriteString(EncodeBase64("GameLauncher"));
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("ModuleValue");
				xmlWriter.WriteString(hash);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
				xmlWriter.Close();
				string[][] extraHeaders = new string[4][]
				{
					new string[2]
					{
						"Content-Type",
						"text/xml;charset=utf-8"
					},
					new string[2]
					{
						"Content-Length",
						stringBuilder.Length.ToString(CultureInfo.InvariantCulture)
					},
					new string[2]
					{
						"userId",
						mUserId
					},
					new string[2]
					{
						"securityToken",
						mAuthKey
					}
				};
				webServicesWrapper.DoCall(ShardManager.ShardUrl, "/security/logFraudDetectionEvents", null, extraHeaders, stringBuilder.ToString(), RequestMethod.POST);
			}
			catch (Exception ex)
			{
				mLogger.Error("SendHash Exception: " + ex.ToString());
			}
		}

		private string EncodeBase64(string text)
		{
			byte[] array = new byte[text.Length];
			array = Encoding.UTF8.GetBytes(text);
			return Convert.ToBase64String(array);
		}

		private void GameLauncher_FormClosing(object sender, FormClosingEventArgs e)
		{
			lock (mFormClosingLock)
			{
				if (!mFormClosing)
				{
					mFormClosing = true;
					if (!string.IsNullOrEmpty(mAuthKey) && !mPlaySuccessful)
					{
						SendClosingTelemetry(e.CloseReason);
					}
					if (mHeartbeatTimer != null)
					{
						mHeartbeatTimer.Dispose();
						mHeartbeatTimer = null;
					}
					if (!mClosePending && mCommandManager.CurrentCommand != null && (mCommandManager.CurrentCommand.Downloader.Verifying || mCommandManager.CurrentCommand.Downloader.Downloading))
					{
						mCommandManager.CurrentCommand.Downloader.Stop();
						e.Cancel = true;
						mClosePending = true;
						while (mCommandManager.CurrentCommand.Downloader.Verifying || mCommandManager.CurrentCommand.Downloader.Downloading)
						{
							Thread.Sleep(100);
						}
					}
					else
					{
						mLogger.Info("Application closing");
					}
					bool flag = false;
					int num = 0;
					while (!flag && ++num < 10)
					{
						try
						{
							Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
							configuration.Save(ConfigurationSaveMode.Full, forceSaveAll: true);
							flag = true;
						}
						catch (ConfigurationErrorsException ex)
						{
							try
							{
								string filename = ex.Filename;
								if (File.Exists(filename))
								{
									File.SetAttributes(filename, FileAttributes.Normal);
									File.Delete(filename);
								}
							}
							catch (Exception ex2)
							{
								mLogger.Error("Exception when trying to delete corrupted config file - retries: " + num);
								mLogger.Error("Main Exception: " + ex2.ToString());
							}
						}
						catch (Exception arg)
						{
							mLogger.Error("GameLauncher_FormClosing Exception " + arg);
						}
					}
					mLogger.Debug("Closing: " + !e.Cancel);
				}
				else
				{
					mLogger.Debug("Form already closing");
				}
			}
		}

		private void wTimerServerCheck_Tick(object sender, EventArgs e)
		{
			if (!mEngineUp)
			{
				LoadServerData();
			}
		}

		private void LoadServerStatusAsync()
		{
			if (CommandBypassServerCheck)
			{
				BeginInvoke(LoadServerStatusFinished, true, "Server check bypassed, have a nice day!");
				return;
			}
			LoadServerStatusArgs loadServerStatusArgs = new LoadServerStatusArgs();
			loadServerStatusArgs.shardName = ShardManager.ShardName;
			loadServerStatusArgs.portalDomain = PortalDomain;
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += LoadServerStatus_BackgroundWork;
			backgroundWorker.RunWorkerCompleted += LoadServerStatus_BackgroundWorkComplete;
			backgroundWorker.RunWorkerAsync(loadServerStatusArgs);
		}

		private void LoadServerStatus_BackgroundWork(object sender, DoWorkEventArgs args)
		{
			LoadServerStatusArgs loadServerStatusArgs = args.Argument as LoadServerStatusArgs;
			LoadServerStatusResult loadServerStatusResult = new LoadServerStatusResult();
			try
			{
				string a = string.Empty;
				WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
				string xml = webServicesWrapper.DoCall(loadServerStatusArgs.portalDomain, $"/SpeedAPI/ws/game/nfsw/server/status?locale={CultureInfo.CurrentCulture.Name.Replace('-', '_')}&shard={loadServerStatusArgs.shardName}", null, null, RequestMethod.GET);
				loadServerStatusResult.portalUp = true;
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				foreach (XmlNode item in xmlDocument.SelectNodes("/worldServerStatus/*"))
				{
					switch (item.Name)
					{
					case "status":
						a = item.InnerText;
						break;
					case "localizedMessage":
						loadServerStatusResult.statusMessage = item.InnerText;
						break;
					}
				}
				loadServerStatusResult.serversUp = (a == "UP");
			}
			catch (Exception ex)
			{
				Logger.Error("LoadServerStatus Exception: " + ex.ToString());
				loadServerStatusResult.statusMessage = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00064");
			}
			args.Result = loadServerStatusResult;
		}

		private void LoadServerStatus_BackgroundWorkComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			LoadServerStatusResult loadServerStatusResult = e.Result as LoadServerStatusResult;
			PortalUp = loadServerStatusResult.portalUp;
			if (LoadServerStatusFinished != null)
			{
				BeginInvoke(LoadServerStatusFinished, loadServerStatusResult.serversUp, loadServerStatusResult.statusMessage);
			}
		}

		public void LoadServerAdsAsync()
		{
			if (EngineUp && PortalUp)
			{
				LoadServerAdsArgs loadServerAdsArgs = new LoadServerAdsArgs();
				loadServerAdsArgs.shardName = ShardManager.ShardName;
				loadServerAdsArgs.portalDomain = PortalDomain;
				BackgroundWorker backgroundWorker = new BackgroundWorker();
				backgroundWorker.DoWork += LoadServerAds_BackgroundWork;
				backgroundWorker.RunWorkerCompleted += LoadServerAds_BackgroundWorkComplete;
				backgroundWorker.RunWorkerAsync(loadServerAdsArgs);
			}
		}

		private void LoadServerAds_BackgroundWork(object sender, DoWorkEventArgs args)
		{
			LoadServerAdsArgs loadServerAdsArgs = args.Argument as LoadServerAdsArgs;
			LoadServerAdsResult loadServerAdsResult = new LoadServerAdsResult();
			try
			{
				WebServicesWrapper webServicesWrapper = new WebServicesWrapper();
				string xml = webServicesWrapper.DoCall(loadServerAdsArgs.portalDomain, $"/SpeedAPI/ws/game/nfsw/launcher/content?locale={CultureInfo.CurrentCulture.Name.Replace('-', '_')}", null, null, RequestMethod.GET);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				foreach (XmlNode item in xmlDocument.SelectNodes("/launcherContent/*"))
				{
					switch (item.Name)
					{
					case "horizontalPromo":
						foreach (XmlNode childNode in item.ChildNodes)
						{
							switch (childNode.Name)
							{
							case "image":
								loadServerAdsResult.horizontalImage = childNode.InnerText;
								break;
							case "link":
								loadServerAdsResult.horizontalLink = childNode.InnerText;
								break;
							}
						}
						break;
					case "verticalPromo":
						foreach (XmlNode childNode2 in item.ChildNodes)
						{
							switch (childNode2.Name)
							{
							case "image":
								loadServerAdsResult.verticalImage = childNode2.InnerText;
								break;
							case "link":
								loadServerAdsResult.verticalLink = childNode2.InnerText;
								break;
							}
						}
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error("LoadAdData Exception: " + ex.ToString());
			}
			args.Result = loadServerAdsResult;
		}

		private void LoadServerAds_BackgroundWorkComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			LoadServerAdsResult loadServerAdsResult = e.Result as LoadServerAdsResult;
			if (LoadServerAdsFinished != null)
			{
				BeginInvoke(LoadServerAdsFinished, loadServerAdsResult.horizontalImage, loadServerAdsResult.horizontalLink, loadServerAdsResult.verticalImage, loadServerAdsResult.verticalLink);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameLauncher.ProdUI.GameLauncherUI));
			wTimerServerCheck = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			wTimerServerCheck.Interval = 30000;
			wTimerServerCheck.Enabled = true;
			wTimerServerCheck.Tick += new System.EventHandler(wTimerServerCheck_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(790, 490);
			ForeColor = System.Drawing.Color.FromArgb(51, 153, 255);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			MinimumSize = new System.Drawing.Size(790, 490);
			base.Name = "GameLauncherUI";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Need for Speed™ World";
			base.TransparencyKey = System.Drawing.Color.Red;
			base.Load += new System.EventHandler(GameLauncherUI_Load);
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GameLauncher_FormClosing);
			ResumeLayout(false);
		}
	}
}
