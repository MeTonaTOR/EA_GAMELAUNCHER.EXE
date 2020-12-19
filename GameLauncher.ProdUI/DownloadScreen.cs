using GameLauncher.ProdUI.Controls;
using GameLauncher.ProdUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class DownloadScreen : BaseScreen
	{
		private bool isPlayButton;

		private bool isPlayButtonEnabled;

		private GameLauncherUI parentForm;

		private string remoteUserId = string.Empty;

		private string webAuthKey = string.Empty;

		private IContainer components;

		private Label wLabelDownloadProgress;

		private ProgressBar wProgressBar;

		private ExtendedWebBrowser wWebBrowser;

		private GlowingButton wPlayButton;

		private OptionsButton wOptionsButton;

		private ShardComboBox wShardComboBox;

		private Label wLabelShard;

		public DownloadScreen()
		{
			InitializeComponent();
		}

		public DownloadScreen(GameLauncherUI launcherForm)
			: base(launcherForm)
		{
			parentForm = launcherForm;
			InitializeComponent();
			LocalizeFE();
			InitializeSettings();
			ApplyEmbeddedFonts();
		}

		public override void LoadScreen()
		{
			base.LoadScreen();
			wShardComboBox.SetSelectedValue();
			wShardComboBox.ShardUrlChanged = OnShardUrlChanged;
			wShardComboBox.ShardRegionChanged = OnShardRegionChanged;
			parentForm.LoadServerStatusFinished = LoadServerStatusFinished;
			parentForm.StartDownload();
			base.ActiveControl = wShardComboBox;
			Cursor = Cursors.Default;
		}

		private void LoadServerStatusFinished(bool serverUp, string statusMessage)
		{
			if (!string.IsNullOrEmpty(remoteUserId) && !string.IsNullOrEmpty(webAuthKey))
			{
				LoadWebStore(remoteUserId, webAuthKey);
			}
		}

		private void LoadWebStore(string remoteUserId, string webAuthKey)
		{
			this.remoteUserId = remoteUserId;
			this.webAuthKey = webAuthKey;
			if (parentForm.PortalUp)
			{
				string text = ShardManager.ShardUrl;
				string[] array = text.Split('/');
				if (array.Length > 3)
				{
					text = array[2] + "/" + array[3];
				}
				wWebBrowser.DocumentCompleted += HandleWebBrowserDocumentLoaded;
				wWebBrowser.Navigate(new Uri(parentForm.PortalDomain + "/webkit/lp/store?locale=" + CultureInfo.CurrentCulture.Name.Replace('-', '_')), "", null, string.Format("userId: {0}{4}token: {1}{4}shard: {2}{4}worldserverurl: {3}{4}User-Agent: {5}", remoteUserId, webAuthKey, ShardManager.ShardName, text, Environment.NewLine, "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; MS-RTC LM 8; .NET4.0C; .NET4.0E)"));
				wWebBrowser.Visible = true;
				remoteUserId = string.Empty;
				webAuthKey = string.Empty;
			}
		}

		private void HandleWebBrowserDocumentLoaded(object sender, WebBrowserDocumentCompletedEventArgs args)
		{
			if ((double)mDpi > 96.0)
			{
				wWebBrowser.Document.Body.Style = "zoom:" + (int)((double)mDpi / 96.0 * 100.0) + "%";
			}
		}

		private void wLabelButton_Click(object sender, EventArgs e)
		{
			if (isPlayButtonEnabled)
			{
				Cursor = Cursors.WaitCursor;
				if (isPlayButton)
				{
					parentForm.PerformPlay();
				}
				else
				{
					parentForm.PerformUpdate();
				}
				Cursor = Cursors.Default;
			}
		}

		private void wPlayButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Return && isPlayButtonEnabled)
			{
				wPlayButton.ButtonPressed(isPressed: true);
				wPlayButton.Update();
				wLabelButton_Click(null, null);
				wPlayButton.ButtonPressed(isPressed: false);
				wPlayButton.Update();
			}
		}

		private void wWebBrowser_NewWindow2(object sender, NewWindow2EventArgs e)
		{
			e.Cancel = true;
			if (!string.IsNullOrEmpty(parentForm.WebApplicationData[0]))
			{
				string text = string.Empty;
				try
				{
					text = parentForm.WebApplicationData[1].Replace("%1", wWebBrowser.StatusText);
					Process.Start(parentForm.WebApplicationData[0], text);
				}
				catch (Exception ex)
				{
					GameLauncherUI.Logger.ErrorFormat("Problem running {0} {1}", parentForm.WebApplicationData[0], text);
					GameLauncherUI.Logger.Error("wWebBrowser_NewWindow2 Exception: " + ex.ToString());
				}
			}
		}

		private void SwitchPlayButtonToUpdate()
		{
			string @string = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00027");
			wPlayButton.EnableButton(@string);
			isPlayButton = false;
			isPlayButtonEnabled = true;
			wLabelDownloadProgress.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00001");
			wLabelDownloadProgress.Visible = true;
			wProgressBar.Value = 0;
			wProgressBar.Visible = true;
		}

		private void SwitchPlayButtonToDownloading()
		{
			string @string = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00041");
			wPlayButton.DisableButton(@string);
			isPlayButton = true;
			isPlayButtonEnabled = false;
			wLabelDownloadProgress.Visible = true;
			wProgressBar.Value = 0;
			wProgressBar.Visible = true;
		}

		private void SwitchPlayButtonToPlay()
		{
			string @string = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00041");
			wPlayButton.EnableButton(@string);
			isPlayButton = true;
			isPlayButtonEnabled = true;
			wProgressBar.Value = 0;
			wProgressBar.Visible = true;
		}

		private void wOptionsButton_Click(object sender, EventArgs e)
		{
			parentForm.SwitchScreen(ScreenType.Options);
		}

		private void wOptionsButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				wOptionsButton.ButtonPressed(isPressed: true);
				wOptionsButton.Update();
				wOptionsButton_Click(null, null);
				wOptionsButton.ButtonPressed(isPressed: false);
				wOptionsButton.Update();
			}
		}

		private void OnShardUrlChanged()
		{
			if (base.IsActive)
			{
				parentForm.PerformLogout();
				parentForm.LoadServerData();
				parentForm.SwitchScreen(ScreenType.Login);
			}
		}

		private void OnShardRegionChanged()
		{
			if (base.IsActive)
			{
				Cursor = Cursors.WaitCursor;
				parentForm.SetRegion();
				Cursor = Cursors.Default;
			}
		}

		private void OnDownloadStarted()
		{
			SwitchPlayButtonToDownloading();
			OnDownloadProgressUpdated(progressVisible: true, 0, ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00017"));
		}

		private void OnDownloadProgressUpdated(bool progressVisible, int progressValue, string progressText)
		{
			ProgressBar progressBar = wProgressBar;
			bool visible = wLabelDownloadProgress.Visible = progressVisible;
			progressBar.Visible = visible;
			wProgressBar.Value = progressValue;
			if (!string.IsNullOrEmpty(progressText))
			{
				wLabelDownloadProgress.Text = progressText;
			}
		}

		private void OnDownloadFinished()
		{
			SwitchPlayButtonToPlay();
		}

		private void OnLoginFinished(string userId, string webAuthKey)
		{
			LoadWebStore(userId, webAuthKey);
		}

		private void OnUpdateStarted()
		{
			SwitchPlayButtonToUpdate();
		}

		protected override void InitializeSettings()
		{
			backgrounds = new Dictionary<double, Image>();
			backgrounds.Add(96.0, Resources.nfsw_lp_96dpi_bg_s2_w_1bit_alpha);
			backgrounds.Add(120.0, Resources.nfsw_lp_120dpi_bg_s2_w_1bit_alpha);
			backgrounds.Add(144.0, Resources.nfsw_lp_144dpi_bg_s2_w_1bit_alpha);
			BackgroundImage = SelectBackgroundImage();
			wLabelDownloadProgress.Visible = false;
			wProgressBar.Visible = false;
			if (!string.IsNullOrEmpty(parentForm.CommandArgShard))
			{
				wShardComboBox.SetSelectedValue(parentForm.CommandArgShard);
			}
			wPlayButton.wLabelButton.Click += wLabelButton_Click;
			wOptionsButton.wLabelOptions.Click += wOptionsButton_Click;
			parentForm.DownloadStarted = OnDownloadStarted;
			parentForm.DownloadProgressUpdated = OnDownloadProgressUpdated;
			parentForm.DownloadFinished = OnDownloadFinished;
			parentForm.LoginFinished = OnLoginFinished;
			parentForm.UpdateStarted = OnUpdateStarted;
		}

		protected override void ApplyEmbeddedFonts()
		{
			FontFamily fontFamily = FontWrapper.Instance.GetFontFamily("MyriadProSemiCondBold.ttf");
			FontFamily fontFamily2 = FontWrapper.Instance.GetFontFamily("Reg-B-I.ttf");
			wPlayButton.wLabelButton.Font = new Font(fontFamily2, 18.75f, FontStyle.Bold | FontStyle.Italic);
			wLabelDownloadProgress.Font = new Font(fontFamily, 9.749999f, FontStyle.Bold);
			wLabelShard.Font = new Font(fontFamily, 9.749999f, FontStyle.Bold);
		}

		protected override void LocalizeFE()
		{
			wLabelShard.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00072");
			wLabelDownloadProgress.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00017");
			wPlayButton.wLabelButton.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00041");
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
			wLabelDownloadProgress = new System.Windows.Forms.Label();
			wProgressBar = new System.Windows.Forms.ProgressBar();
			wWebBrowser = new GameLauncher.ProdUI.ExtendedWebBrowser();
			wPlayButton = new GameLauncher.ProdUI.Controls.GlowingButton();
			wOptionsButton = new GameLauncher.ProdUI.Controls.OptionsButton();
			wShardComboBox = new GameLauncher.ProdUI.ShardComboBox();
			wLabelShard = new System.Windows.Forms.Label();
			SuspendLayout();
			wLabelDownloadProgress.BackColor = System.Drawing.Color.Transparent;
			wLabelDownloadProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wLabelDownloadProgress.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLabelDownloadProgress.Location = new System.Drawing.Point(17, 457);
			wLabelDownloadProgress.Name = "wLabelDownloadProgress";
			wLabelDownloadProgress.Size = new System.Drawing.Size(480, 15);
			wLabelShard.TabStop = false;
			wLabelDownloadProgress.Text = "DOWNLOAD WAITING...";
			wProgressBar.BackColor = System.Drawing.Color.FromArgb(175, 197, 220);
			wProgressBar.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wProgressBar.Location = new System.Drawing.Point(20, 436);
			wProgressBar.Name = "wProgressBar";
			wProgressBar.Size = new System.Drawing.Size(580, 18);
			wProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			wLabelShard.TabStop = false;
			wWebBrowser.AllowWebBrowserDrop = false;
			wWebBrowser.IsWebBrowserContextMenuEnabled = false;
			wWebBrowser.Location = new System.Drawing.Point(10, 95);
			wWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			wWebBrowser.Name = "wWebBrowser";
			wWebBrowser.ScriptErrorsSuppressed = true;
			wWebBrowser.ScrollBarsEnabled = false;
			wWebBrowser.Size = new System.Drawing.Size(771, 310);
			wWebBrowser.TabStop = false;
			wWebBrowser.Url = new System.Uri("", System.UriKind.Relative);
			wWebBrowser.Visible = false;
			wWebBrowser.WebBrowserShortcutsEnabled = false;
			wWebBrowser.NewWindow2 += new System.EventHandler<GameLauncher.ProdUI.NewWindow2EventArgs>(wWebBrowser_NewWindow2);
			wPlayButton.AutoSize = true;
			wPlayButton.BackColor = System.Drawing.Color.Transparent;
			wPlayButton.Location = new System.Drawing.Point(618, 424);
			wPlayButton.Name = "wPlayButton";
			wPlayButton.Size = new System.Drawing.Size(168, 58);
			wPlayButton.TabIndex = 1;
			wPlayButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(wPlayButton_PreviewKeyDown);
			wOptionsButton.AutoSize = true;
			wOptionsButton.BackColor = System.Drawing.Color.Transparent;
			wOptionsButton.Location = new System.Drawing.Point(741, 44);
			wOptionsButton.MinimumSize = new System.Drawing.Size(31, 29);
			wOptionsButton.Name = "wOptionsButton";
			wOptionsButton.Size = new System.Drawing.Size(40, 29);
			wOptionsButton.TabIndex = 3;
			wOptionsButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(wOptionsButton_PreviewKeyDown);
			wShardComboBox.AutoSize = true;
			wShardComboBox.BackColor = System.Drawing.Color.Transparent;
			wShardComboBox.Location = new System.Drawing.Point(548, 47);
			wShardComboBox.Name = "wShardComboBox";
			wShardComboBox.Size = new System.Drawing.Size(188, 26);
			wShardComboBox.TabIndex = 2;
			wLabelShard.BackColor = System.Drawing.Color.Transparent;
			wLabelShard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold);
			wLabelShard.ForeColor = System.Drawing.Color.White;
			wLabelShard.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			wLabelShard.Location = new System.Drawing.Point(376, 51);
			wLabelShard.Name = "wLabelShard";
			wLabelShard.Size = new System.Drawing.Size(171, 15);
			wLabelShard.TabStop = false;
			wLabelShard.Text = "SELECT SHARD:";
			wLabelShard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			BackgroundImage = GameLauncher.ProdUI.Properties.Resources.nfsw_lp_96dpi_bg_s2_w_1bit_alpha;
			base.Controls.Add(wLabelShard);
			base.Controls.Add(wShardComboBox);
			base.Controls.Add(wOptionsButton);
			base.Controls.Add(wPlayButton);
			base.Controls.Add(wWebBrowser);
			base.Controls.Add(wProgressBar);
			base.Controls.Add(wLabelDownloadProgress);
			base.Name = "DownloadScreen";
			base.Controls.SetChildIndex(wLabelDownloadProgress, 0);
			base.Controls.SetChildIndex(wProgressBar, 0);
			base.Controls.SetChildIndex(wWebBrowser, 0);
			base.Controls.SetChildIndex(wPlayButton, 0);
			base.Controls.SetChildIndex(wOptionsButton, 0);
			base.Controls.SetChildIndex(wShardComboBox, 0);
			base.Controls.SetChildIndex(wLabelShard, 0);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
