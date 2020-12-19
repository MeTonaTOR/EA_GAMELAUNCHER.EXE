using GameLauncher.ProdUI.Controls;
using GameLauncher.ProdUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class LoginScreen : BaseScreen
	{
		private string mPictureBoxHorizontalLink = string.Empty;

		private string mPictureBoxVerticalLink = string.Empty;

		private GameLauncherUI parentForm;

		private IContainer components;

		private TextBox wTextBoxEmail;

		private TextBox wTextBoxPassword;

		private CheckBox wCheckBoxRememberMe;

		private LinkLabel wLinkLabelForgetPassword;

		private Label wLabelEmail;

		private Label wLabelPassword;

		private PictureBox wPictureBoxServerStatusOff;

		private Label wLabelServerStatus;

		private PictureBox wPictureBoxServerStatusOn;

		private PictureBox wPictureBoxHorizontal;

		private Label wLabelAdvertisement;

		private Label wLabelHavingTrouble;

		private LinkLabel wLinkLabelCustomerService;

		private PictureBox wPictureBoxVertical;

		private LinkLabel wLinkLabelNewAccount;

		private MainButton wLoginButton;

		private Label wLabelLoginInfo;

		private ShardComboBox wShardComboBox;

		private OptionsButton wOptionsButton;

		private Label wLabelShard;

		private System.Windows.Forms.Timer wTimerServerStatusCheck;

		public LoginScreen(GameLauncherUI launcherForm)
			: base(launcherForm)
		{
			parentForm = launcherForm;
			InitializeComponent();
			LocalizeFE();
			ApplyEmbeddedFonts();
			InitializeSettings();
		}

		public override void LoadScreen()
		{
			base.LoadScreen();
			wShardComboBox.SetSelectedValue();
			wShardComboBox.ShardUrlChanged = OnShardUrlChanged;
			parentForm.LoadServerDataFinished = OnLoadServerDataFinished;
			parentForm.LoadServerStatusFinished = LoadServerStatusFinished;
			parentForm.LoadServerAdsFinished = LoadServerAdsFinished;
			if (string.IsNullOrEmpty(wTextBoxEmail.Text))
			{
				base.ActiveControl = wTextBoxEmail;
			}
			else
			{
				base.ActiveControl = wTextBoxPassword;
			}
		}

		public override void UnloadScreen()
		{
			base.UnloadScreen();
			wTimerServerStatusCheck.Enabled = false;
			wShardComboBox.ShardUrlChanged = null;
			parentForm.LoadServerDataFinished = null;
		}

		private void PerformLogin()
		{
			if (wCheckBoxRememberMe.Checked)
			{
				Settings.Default.Email = wTextBoxEmail.Text;
				Settings.Default.RememberEmail = wCheckBoxRememberMe.Checked;
				Settings.Default.Save();
			}
			parentForm.PerformLogin(wTextBoxEmail.Text, wTextBoxPassword.Text);
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

		private void wLogin_TextChanged(object sender, EventArgs e)
		{
			wLoginButton.LabelButtonEnabled = (wTextBoxEmail.Text.Length != 0 && wTextBoxPassword.Text.Length != 0 && parentForm.EngineUp);
		}

		private void wLoginButton_Click(object sender, EventArgs e)
		{
			if (wLoginButton.LabelButtonEnabled)
			{
				Cursor = Cursors.WaitCursor;
				PerformLogin();
				Cursor = Cursors.Default;
			}
		}

		private void wLoginButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Return && wLoginButton.LabelButtonEnabled)
			{
				wLoginButton.ButtonPressed(isPressed: true);
				wLoginButton.Update();
				wLoginButton_Click(null, null);
				wLoginButton.ButtonPressed(isPressed: false);
				wLoginButton.Update();
			}
		}

		private void wLinkLabelForgetPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (!string.IsNullOrEmpty(parentForm.WebApplicationData[0]))
			{
				string text = string.Empty;
				try
				{
					text = parentForm.WebApplicationData[1].Replace("%1", string.Format("{0}?lang={1}", "https://www.origin.com/account/reset-password", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower()));
					Process.Start(parentForm.WebApplicationData[0], text);
				}
				catch (Exception ex)
				{
					GameLauncherUI.Logger.ErrorFormat("Problem running {0} {1}", parentForm.WebApplicationData[0], text);
					GameLauncherUI.Logger.Error("wLinkLabelForgetPassword_LinkClicked Exception: " + ex.ToString());
				}
			}
		}

		private void wLinkLabelCustomerService_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (!string.IsNullOrEmpty(parentForm.WebApplicationData[0]))
			{
				string text = string.Empty;
				try
				{
					text = parentForm.WebApplicationData[1].Replace("%1", string.Format(Settings.Default.UrlCustomerService, CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower()));
					Process.Start(parentForm.WebApplicationData[0], text);
				}
				catch (Exception ex)
				{
					GameLauncherUI.Logger.ErrorFormat("Problem running {0} {1}", parentForm.WebApplicationData[0], text);
					GameLauncherUI.Logger.Error("wLinkLabelCustomerService_LinkClicked Exception: " + ex.ToString());
				}
			}
		}

		private void wLinkLabelNewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (!string.IsNullOrEmpty(parentForm.WebApplicationData[0]))
			{
				string text = string.Empty;
				try
				{
					text = parentForm.WebApplicationData[1].Replace("%1", $"{Settings.Default.UrlCreateNewAccount}?lang={CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower()}");
					Process.Start(parentForm.WebApplicationData[0], text);
				}
				catch (Exception ex)
				{
					GameLauncherUI.Logger.ErrorFormat("Problem running {0} {1}", parentForm.WebApplicationData[0], text);
					GameLauncherUI.Logger.Error("wLinkLabelNewAccount_LinkClicked Exception: " + ex.ToString());
				}
			}
		}

		private void wPictureBoxVertical_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(mPictureBoxVerticalLink) && !string.IsNullOrEmpty(parentForm.WebApplicationData[0]))
			{
				string text = string.Empty;
				try
				{
					text = parentForm.WebApplicationData[1].Replace("%1", mPictureBoxVerticalLink);
					Process.Start(parentForm.WebApplicationData[0], text);
				}
				catch (Exception ex)
				{
					GameLauncherUI.Logger.ErrorFormat("Problem running {0} {1}", parentForm.WebApplicationData[0], text);
					GameLauncherUI.Logger.Error("wPictureBoxVertical_Click Exception: " + ex.ToString());
				}
			}
		}

		private void wPictureBoxHorizontal_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(mPictureBoxHorizontalLink) && !string.IsNullOrEmpty(parentForm.WebApplicationData[0]))
			{
				string text = string.Empty;
				try
				{
					text = parentForm.WebApplicationData[1].Replace("%1", mPictureBoxHorizontalLink);
					Process.Start(parentForm.WebApplicationData[0], text);
				}
				catch (Exception ex)
				{
					GameLauncherUI.Logger.ErrorFormat("Problem running {0} {1}", parentForm.WebApplicationData[0], text);
					GameLauncherUI.Logger.Error("wPictureBoxHorizontal_Click Exception: " + ex.ToString());
				}
			}
		}

		private void wTimerServerStatusCheck_Tick(object sender, EventArgs e)
		{
		}

		protected override void InitializeSettings()
		{
			if (!string.IsNullOrEmpty(parentForm.CommandArgEmail))
			{
				wTextBoxEmail.Text = parentForm.CommandArgEmail;
				base.ActiveControl = wTextBoxPassword;
			}
			else
			{
				wCheckBoxRememberMe.Checked = Settings.Default.RememberEmail;
				if (Settings.Default.RememberEmail)
				{
					wTextBoxEmail.Text = Settings.Default.Email;
				}
			}
			if (!string.IsNullOrEmpty(parentForm.CommandArgPassword))
			{
				wTextBoxPassword.Text = parentForm.CommandArgPassword;
			}
			if (!string.IsNullOrEmpty(parentForm.CommandArgShard))
			{
				wShardComboBox.SetSelectedValue(parentForm.CommandArgShard);
			}
			backgrounds = new Dictionary<double, Image>();
			backgrounds.Add(96.0, Resources.nfsw_lp_96dpi_bg_s1_w_1bit_alpha);
			backgrounds.Add(120.0, Resources.nfsw_lp_120dpi_bg_s1_w_1bit_alpha);
			backgrounds.Add(144.0, Resources.nfsw_lp_144dpi_bg_s1_w_1bit_alpha);
			BackgroundImage = SelectBackgroundImage();
			wLoginButton.wLabelButton.Click += wLoginButton_Click;
			wOptionsButton.wLabelOptions.Click += wOptionsButton_Click;
		}

		protected override void ApplyEmbeddedFonts()
		{
			FontFamily fontFamily = FontWrapper.Instance.GetFontFamily("MyriadProSemiCondBold.ttf");
			FontWrapper.Instance.GetFontFamily("MyriadPro-SemiCn.ttf");
			FontFamily fontFamily2 = FontWrapper.Instance.GetFontFamily("Reg-DB-I.ttf");
			FontFamily fontFamily3 = FontWrapper.Instance.GetFontFamily("Reg-B-I.ttf");
			FontFamily fontFamily4 = FontWrapper.Instance.GetFontFamily("REGIDB__.TTF");
			float num = 1f;
			if (string.Compare(Thread.CurrentThread.CurrentUICulture.Name, "th-TH", ignoreCase: true) == 0)
			{
				num = 1.2f;
			}
			if (string.Compare(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, "zh", ignoreCase: true) == 0)
			{
				wCheckBoxRememberMe.Font = new Font(fontFamily4, 9f);
				wLinkLabelForgetPassword.Font = new Font(fontFamily4, 9f);
			}
			else
			{
				wCheckBoxRememberMe.Font = new Font(fontFamily, 9f, FontStyle.Bold);
				wLinkLabelForgetPassword.Font = new Font(fontFamily, 9f, FontStyle.Bold);
			}
			wLoginButton.wLabelButton.Font = new Font(fontFamily3, 15f, FontStyle.Bold | FontStyle.Italic);
			wLabelServerStatus.Font = new Font(fontFamily, 9.749999f * num, FontStyle.Bold);
			wLabelAdvertisement.Font = new Font(fontFamily, 8f, FontStyle.Bold);
			wLabelLoginInfo.Font = new Font(fontFamily2, 12.75f, FontStyle.Italic);
			wLabelEmail.Font = new Font(fontFamily4, 11f);
			wLabelPassword.Font = new Font(fontFamily4, 11f);
			wLabelHavingTrouble.Font = new Font(fontFamily, 9.749999f * num, FontStyle.Bold);
			wLinkLabelCustomerService.Font = new Font(fontFamily, 9.749999f * num, FontStyle.Bold);
			wLinkLabelNewAccount.Font = new Font(fontFamily, 9.749999f * num, FontStyle.Bold);
			wLabelShard.Font = new Font(fontFamily, 9.749999f * num, FontStyle.Bold);
		}

		protected override void LocalizeFE()
		{
			wCheckBoxRememberMe.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00015");
			wLabelEmail.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00016");
			wLoginButton.wLabelButton.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00017");
			wLabelPassword.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00018");
			wLinkLabelForgetPassword.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00028");
			wLabelLoginInfo.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00032");
			wLinkLabelNewAccount.Text = string.Format("{0}\n{1}", ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00034"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00035"));
			wLabelHavingTrouble.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00036");
			wLinkLabelCustomerService.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00037");
			wLabelAdvertisement.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00038");
			wLabelServerStatus.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00039");
			wLabelShard.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00072");
		}

		private void OnShardUrlChanged()
		{
			if (base.IsActive)
			{
				wLoginButton.LabelButtonEnabled = false;
				wLabelServerStatus.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00039");
				wLabelServerStatus.ForeColor = Color.White;
				wLabelServerStatus.Update();
				wPictureBoxServerStatusOn.Visible = false;
				wPictureBoxServerStatusOff.Visible = false;
				parentForm.LoadServerData();
			}
		}

		private void OnLoadServerDataFinished()
		{
		}

		private void LoadServerAdsFinished(string horizontalImage, string horizontalLink, string verticalImage, string verticalLink)
		{
			wPictureBoxHorizontal.Image = launcherImage.ResizeImage(LauncherImage.DownloadImage(horizontalImage));
			if (wPictureBoxHorizontal.InvokeRequired)
			{
				wPictureBoxHorizontal.Invoke((MethodInvoker)delegate
				{
					wPictureBoxHorizontal.Visible = true;
				});
			}
			else
			{
				wPictureBoxHorizontal.Visible = true;
			}
			mPictureBoxHorizontalLink = horizontalLink;
			if (wPictureBoxHorizontal.InvokeRequired)
			{
				wPictureBoxHorizontal.Invoke((MethodInvoker)delegate
				{
					wPictureBoxHorizontal.Cursor = Cursors.Hand;
				});
			}
			else
			{
				wPictureBoxHorizontal.Cursor = Cursors.Hand;
			}
			wPictureBoxVertical.Image = launcherImage.ResizeImage(LauncherImage.DownloadImage(verticalImage));
			if (wPictureBoxVertical.InvokeRequired)
			{
				wPictureBoxVertical.Invoke((MethodInvoker)delegate
				{
					wPictureBoxVertical.Visible = true;
				});
			}
			else
			{
				wPictureBoxVertical.Visible = true;
			}
			mPictureBoxVerticalLink = verticalLink;
			if (wPictureBoxVertical.InvokeRequired)
			{
				wPictureBoxVertical.Invoke((MethodInvoker)delegate
				{
					wPictureBoxVertical.Cursor = Cursors.Hand;
				});
			}
			else
			{
				wPictureBoxVertical.Cursor = Cursors.Hand;
			}
		}

		private void LoadServerStatusFinished(bool serversUp, string serverStatusText)
		{
			parentForm.LoadServerAdsAsync();
			if (serversUp)
			{
				wPictureBoxServerStatusOn.Image = launcherImage.ResizeImage(Resources.launcher_greenlight);
				if (wPictureBoxServerStatusOn.InvokeRequired)
				{
					wPictureBoxServerStatusOn.Invoke((MethodInvoker)delegate
					{
						wPictureBoxServerStatusOn.Visible = true;
						wPictureBoxServerStatusOff.Visible = false;
					});
				}
				else
				{
					wPictureBoxServerStatusOn.Visible = true;
					wPictureBoxServerStatusOff.Visible = false;
				}
				wLabelServerStatus.ForeColor = Color.FromArgb(181, 255, 33);
			}
			else
			{
				wPictureBoxServerStatusOff.Image = launcherImage.ResizeImage(Resources.launcher_redlight);
				if (wPictureBoxServerStatusOn.InvokeRequired)
				{
					wPictureBoxServerStatusOn.Invoke((MethodInvoker)delegate
					{
						wPictureBoxServerStatusOn.Visible = false;
						wPictureBoxServerStatusOff.Visible = true;
					});
				}
				else
				{
					wPictureBoxServerStatusOn.Visible = false;
					wPictureBoxServerStatusOff.Visible = true;
				}
				wLabelServerStatus.ForeColor = Color.FromArgb(227, 88, 50);
			}
			if (wLabelServerStatus.InvokeRequired)
			{
				wLabelServerStatus.Invoke((MethodInvoker)delegate
				{
					wLabelServerStatus.Text = serverStatusText;
				});
			}
			else
			{
				wLabelServerStatus.Text = serverStatusText;
			}
			wLabelServerStatus.Cursor = Cursors.Default;
			wLogin_TextChanged(null, null);
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
			wTextBoxEmail = new System.Windows.Forms.TextBox();
			wTextBoxPassword = new System.Windows.Forms.TextBox();
			wCheckBoxRememberMe = new System.Windows.Forms.CheckBox();
			wLinkLabelForgetPassword = new System.Windows.Forms.LinkLabel();
			wLabelEmail = new System.Windows.Forms.Label();
			wLabelPassword = new System.Windows.Forms.Label();
			wPictureBoxServerStatusOff = new System.Windows.Forms.PictureBox();
			wLabelServerStatus = new System.Windows.Forms.Label();
			wPictureBoxServerStatusOn = new System.Windows.Forms.PictureBox();
			wPictureBoxHorizontal = new System.Windows.Forms.PictureBox();
			wLabelAdvertisement = new System.Windows.Forms.Label();
			wLabelHavingTrouble = new System.Windows.Forms.Label();
			wLinkLabelCustomerService = new System.Windows.Forms.LinkLabel();
			wPictureBoxVertical = new System.Windows.Forms.PictureBox();
			wLinkLabelNewAccount = new System.Windows.Forms.LinkLabel();
			wLoginButton = new GameLauncher.ProdUI.MainButton();
			wLabelLoginInfo = new System.Windows.Forms.Label();
			wShardComboBox = new GameLauncher.ProdUI.ShardComboBox();
			wOptionsButton = new GameLauncher.ProdUI.Controls.OptionsButton();
			wLabelShard = new System.Windows.Forms.Label();
			wTimerServerStatusCheck = new System.Windows.Forms.Timer(components);
			((System.ComponentModel.ISupportInitialize)wPictureBoxServerStatusOff).BeginInit();
			((System.ComponentModel.ISupportInitialize)wPictureBoxServerStatusOn).BeginInit();
			((System.ComponentModel.ISupportInitialize)wPictureBoxHorizontal).BeginInit();
			((System.ComponentModel.ISupportInitialize)wPictureBoxVertical).BeginInit();
			SuspendLayout();
			wTextBoxEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			wTextBoxEmail.Location = new System.Drawing.Point(33, 183);
			wTextBoxEmail.Name = "wTextBoxEmail";
			wTextBoxEmail.Size = new System.Drawing.Size(210, 20);
			wTextBoxEmail.TabIndex = 1;
			wTextBoxEmail.TextChanged += new System.EventHandler(wLogin_TextChanged);
			wTextBoxPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			wTextBoxPassword.Location = new System.Drawing.Point(276, 183);
			wTextBoxPassword.MaxLength = 16;
			wTextBoxPassword.Name = "wTextBoxPassword";
			wTextBoxPassword.Size = new System.Drawing.Size(210, 20);
			wTextBoxPassword.TabIndex = 2;
			wTextBoxPassword.UseSystemPasswordChar = true;
			wTextBoxPassword.TextChanged += new System.EventHandler(wLogin_TextChanged);
			wTextBoxPassword.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(wLoginButton_PreviewKeyDown);
			wCheckBoxRememberMe.AutoSize = true;
			wCheckBoxRememberMe.BackColor = System.Drawing.Color.Transparent;
			wCheckBoxRememberMe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999f, System.Drawing.FontStyle.Bold);
			wCheckBoxRememberMe.ForeColor = System.Drawing.Color.Transparent;
			wCheckBoxRememberMe.Location = new System.Drawing.Point(33, 209);
			wCheckBoxRememberMe.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			wCheckBoxRememberMe.Name = "wCheckBoxRememberMe";
			wCheckBoxRememberMe.Size = new System.Drawing.Size(244, 19);
			wCheckBoxRememberMe.TabIndex = 3;
			wCheckBoxRememberMe.Text = "REMEMBER MY EMAIL ADDRESS";
			wCheckBoxRememberMe.UseVisualStyleBackColor = false;
			wLinkLabelForgetPassword.ActiveLinkColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLinkLabelForgetPassword.BackColor = System.Drawing.Color.Transparent;
			wLinkLabelForgetPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wLinkLabelForgetPassword.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLinkLabelForgetPassword.LinkArea = new System.Windows.Forms.LinkArea(0, 100);
			wLinkLabelForgetPassword.LinkColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLinkLabelForgetPassword.Location = new System.Drawing.Point(276, 206);
			wLinkLabelForgetPassword.Name = "wLinkLabelForgetPassword";
			wLinkLabelForgetPassword.Size = new System.Drawing.Size(210, 21);
			wLinkLabelForgetPassword.TabIndex = 4;
			wLinkLabelForgetPassword.TabStop = true;
			wLinkLabelForgetPassword.Text = "I forgot my password.";
			wLinkLabelForgetPassword.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			wLinkLabelForgetPassword.UseCompatibleTextRendering = true;
			wLinkLabelForgetPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(wLinkLabelForgetPassword_LinkClicked);
			wLabelEmail.BackColor = System.Drawing.Color.Transparent;
			wLabelEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			wLabelEmail.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLabelEmail.Location = new System.Drawing.Point(30, 161);
			wLabelEmail.Name = "wLabelEmail";
			wLabelEmail.Size = new System.Drawing.Size(212, 19);
			wLabelShard.TabStop = false;
			wLabelEmail.Text = "EMAIL ADDRESS:";
			wLabelEmail.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			wLabelPassword.BackColor = System.Drawing.Color.Transparent;
			wLabelPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f);
			wLabelPassword.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLabelPassword.Location = new System.Drawing.Point(276, 161);
			wLabelPassword.Name = "wLabelPassword";
			wLabelPassword.Size = new System.Drawing.Size(210, 19);
			wLabelShard.TabStop = false;
			wLabelPassword.Text = "PASSWORD:";
			wLabelPassword.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			wPictureBoxServerStatusOff.BackColor = System.Drawing.Color.Transparent;
			wPictureBoxServerStatusOff.Image = GameLauncher.ProdUI.Properties.Resources.launcher_redlight;
			wPictureBoxServerStatusOff.Location = new System.Drawing.Point(20, 335);
			wPictureBoxServerStatusOff.Name = "wPictureBoxServerStatusOff";
			wPictureBoxServerStatusOff.Size = new System.Drawing.Size(16, 16);
			wLabelShard.TabStop = false;
			wPictureBoxServerStatusOff.TabStop = false;
			wPictureBoxServerStatusOff.Visible = false;
			wLabelServerStatus.BackColor = System.Drawing.Color.Transparent;
			wLabelServerStatus.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			wLabelServerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wLabelServerStatus.ForeColor = System.Drawing.Color.White;
			wLabelServerStatus.Location = new System.Drawing.Point(41, 312);
			wLabelServerStatus.Name = "wLabelServerStatus";
			wLabelServerStatus.Size = new System.Drawing.Size(740, 56);
			wLabelShard.TabStop = false;
			wLabelServerStatus.Text = "Retrieving server status...";
			wLabelServerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			wPictureBoxServerStatusOn.BackColor = System.Drawing.Color.Transparent;
			wPictureBoxServerStatusOn.Image = GameLauncher.ProdUI.Properties.Resources.launcher_greenlight;
			wPictureBoxServerStatusOn.Location = new System.Drawing.Point(20, 323);
			wPictureBoxServerStatusOn.Name = "wPictureBoxServerStatusOn";
			wPictureBoxServerStatusOn.Size = new System.Drawing.Size(16, 16);
			wLabelShard.TabStop = false;
			wPictureBoxServerStatusOn.TabStop = false;
			wPictureBoxServerStatusOn.Visible = false;
			wPictureBoxHorizontal.BackColor = System.Drawing.Color.Transparent;
			wPictureBoxHorizontal.Location = new System.Drawing.Point(34, 389);
			wPictureBoxHorizontal.Name = "wPictureBoxHorizontal";
			wPictureBoxHorizontal.Size = new System.Drawing.Size(720, 88);
			wLabelShard.TabStop = false;
			wPictureBoxHorizontal.TabStop = false;
			wPictureBoxHorizontal.Visible = false;
			wPictureBoxHorizontal.Click += new System.EventHandler(wPictureBoxHorizontal_Click);
			wLabelAdvertisement.BackColor = System.Drawing.Color.Transparent;
			wLabelAdvertisement.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wLabelAdvertisement.ForeColor = System.Drawing.Color.FromArgb(69, 133, 193);
			wLabelAdvertisement.Location = new System.Drawing.Point(308, 375);
			wLabelAdvertisement.Name = "wLabelAdvertisement";
			wLabelAdvertisement.Size = new System.Drawing.Size(200, 13);
			wLabelShard.TabStop = false;
			wLabelAdvertisement.Text = "ADVERTISEMENT";
			wLabelAdvertisement.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			wLabelHavingTrouble.AutoSize = true;
			wLabelHavingTrouble.BackColor = System.Drawing.Color.Transparent;
			wLabelHavingTrouble.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wLabelHavingTrouble.ForeColor = System.Drawing.Color.White;
			wLabelHavingTrouble.Location = new System.Drawing.Point(30, 255);
			wLabelHavingTrouble.Name = "wLabelHavingTrouble";
			wLabelHavingTrouble.Size = new System.Drawing.Size(148, 16);
			wLabelShard.TabStop = false;
			wLabelHavingTrouble.Text = "HAVING TROUBLE?";
			wLinkLabelCustomerService.ActiveLinkColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLinkLabelCustomerService.AutoSize = true;
			wLinkLabelCustomerService.BackColor = System.Drawing.Color.Transparent;
			wLinkLabelCustomerService.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wLinkLabelCustomerService.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLinkLabelCustomerService.LinkColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLinkLabelCustomerService.Location = new System.Drawing.Point(30, 271);
			wLinkLabelCustomerService.Name = "wLinkLabelCustomerService";
			wLinkLabelCustomerService.Size = new System.Drawing.Size(226, 16);
			wLinkLabelCustomerService.TabIndex = 5;
			wLinkLabelCustomerService.TabStop = true;
			wLinkLabelCustomerService.Text = "Visit our customer service page";
			wLinkLabelCustomerService.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(wLinkLabelCustomerService_LinkClicked);
			wPictureBoxVertical.BackColor = System.Drawing.Color.Transparent;
			wPictureBoxVertical.Location = new System.Drawing.Point(526, 107);
			wPictureBoxVertical.Name = "wPictureBoxVertical";
			wPictureBoxVertical.Size = new System.Drawing.Size(249, 118);
			wPictureBoxVertical.TabStop = false;
			wPictureBoxVertical.Visible = false;
			wPictureBoxVertical.Click += new System.EventHandler(wPictureBoxVertical_Click);
			wLinkLabelNewAccount.ActiveLinkColor = System.Drawing.Color.FromArgb(181, 255, 33);
			wLinkLabelNewAccount.AutoSize = true;
			wLinkLabelNewAccount.BackColor = System.Drawing.Color.Transparent;
			wLinkLabelNewAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			wLinkLabelNewAccount.ForeColor = System.Drawing.Color.FromArgb(181, 255, 33);
			wLinkLabelNewAccount.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
			wLinkLabelNewAccount.LinkColor = System.Drawing.Color.FromArgb(181, 255, 33);
			wLinkLabelNewAccount.Location = new System.Drawing.Point(527, 260);
			wLinkLabelNewAccount.Name = "wLinkLabelNewAccount";
			wLinkLabelNewAccount.Size = new System.Drawing.Size(248, 30);
			wLinkLabelNewAccount.TabIndex = 7;
			wLinkLabelNewAccount.TabStop = true;
			wLinkLabelNewAccount.Text = "DON'T HAVE AN ACCOUNT?\nCLICK HERE TO CREATE ONE NOW...";
			wLinkLabelNewAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(wLinkLabelNewAccount_LinkClicked);
			wLoginButton.AutoSize = true;
			wLoginButton.BackColor = System.Drawing.Color.Transparent;
			wLoginButton.LabelButtonEnabled = false;
			wLoginButton.Location = new System.Drawing.Point(340, 250);
			wLoginButton.Name = "wLoginButton";
			wLoginButton.Size = new System.Drawing.Size(149, 37);
			wLoginButton.TabIndex = 6;
			wLoginButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(wLoginButton_PreviewKeyDown);
			wLabelLoginInfo.BackColor = System.Drawing.Color.Transparent;
			wLabelLoginInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75f, System.Drawing.FontStyle.Italic);
			wLabelLoginInfo.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLabelLoginInfo.Location = new System.Drawing.Point(68, 121);
			wLabelLoginInfo.Name = "wLabelLoginInfo";
			wLabelLoginInfo.Size = new System.Drawing.Size(430, 21);
			wLabelShard.TabStop = false;
			wLabelLoginInfo.Text = "ENTER YOUR ACCOUNT INFORMATION TO LOG IN:";
			wLabelLoginInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			wShardComboBox.AutoSize = true;
			wShardComboBox.Location = new System.Drawing.Point(548, 47);
			wShardComboBox.Name = "wShardComboBox";
			wShardComboBox.Size = new System.Drawing.Size(188, 26);
			wShardComboBox.TabIndex = 8;
			wOptionsButton.AutoSize = true;
			wOptionsButton.BackColor = System.Drawing.Color.Transparent;
			wOptionsButton.Location = new System.Drawing.Point(741, 44);
			wOptionsButton.MinimumSize = new System.Drawing.Size(31, 29);
			wOptionsButton.Name = "wOptionsButton";
			wOptionsButton.Size = new System.Drawing.Size(40, 29);
			wOptionsButton.TabIndex = 9;
			wOptionsButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(wOptionsButton_PreviewKeyDown);
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
			wTimerServerStatusCheck.Interval = 300000;
			wTimerServerStatusCheck.Tick += new System.EventHandler(wTimerServerStatusCheck_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			BackColor = System.Drawing.Color.Transparent;
			BackgroundImage = GameLauncher.ProdUI.Properties.Resources.nfsw_lp_96dpi_bg_s1_w_1bit_alpha;
			base.Controls.Add(wLabelShard);
			base.Controls.Add(wOptionsButton);
			base.Controls.Add(wShardComboBox);
			base.Controls.Add(wLabelLoginInfo);
			base.Controls.Add(wLoginButton);
			base.Controls.Add(wLinkLabelNewAccount);
			base.Controls.Add(wPictureBoxVertical);
			base.Controls.Add(wLinkLabelCustomerService);
			base.Controls.Add(wLabelHavingTrouble);
			base.Controls.Add(wLabelAdvertisement);
			base.Controls.Add(wPictureBoxHorizontal);
			base.Controls.Add(wPictureBoxServerStatusOff);
			base.Controls.Add(wLabelServerStatus);
			base.Controls.Add(wPictureBoxServerStatusOn);
			base.Controls.Add(wLabelPassword);
			base.Controls.Add(wLabelEmail);
			base.Controls.Add(wLinkLabelForgetPassword);
			base.Controls.Add(wCheckBoxRememberMe);
			base.Controls.Add(wTextBoxPassword);
			base.Controls.Add(wTextBoxEmail);
			base.Name = "LoginScreen";
			base.Controls.SetChildIndex(wTextBoxEmail, 0);
			base.Controls.SetChildIndex(wTextBoxPassword, 0);
			base.Controls.SetChildIndex(wCheckBoxRememberMe, 0);
			base.Controls.SetChildIndex(wLinkLabelForgetPassword, 0);
			base.Controls.SetChildIndex(wLabelEmail, 0);
			base.Controls.SetChildIndex(wLabelPassword, 0);
			base.Controls.SetChildIndex(wPictureBoxServerStatusOn, 0);
			base.Controls.SetChildIndex(wLabelServerStatus, 0);
			base.Controls.SetChildIndex(wPictureBoxServerStatusOff, 0);
			base.Controls.SetChildIndex(wPictureBoxHorizontal, 0);
			base.Controls.SetChildIndex(wLabelAdvertisement, 0);
			base.Controls.SetChildIndex(wLabelHavingTrouble, 0);
			base.Controls.SetChildIndex(wLinkLabelCustomerService, 0);
			base.Controls.SetChildIndex(wPictureBoxVertical, 0);
			base.Controls.SetChildIndex(wLinkLabelNewAccount, 0);
			base.Controls.SetChildIndex(wLoginButton, 0);
			base.Controls.SetChildIndex(wLabelLoginInfo, 0);
			base.Controls.SetChildIndex(wShardComboBox, 0);
			base.Controls.SetChildIndex(wOptionsButton, 0);
			base.Controls.SetChildIndex(wLabelShard, 0);
			((System.ComponentModel.ISupportInitialize)wPictureBoxServerStatusOff).EndInit();
			((System.ComponentModel.ISupportInitialize)wPictureBoxServerStatusOn).EndInit();
			((System.ComponentModel.ISupportInitialize)wPictureBoxHorizontal).EndInit();
			((System.ComponentModel.ISupportInitialize)wPictureBoxVertical).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
