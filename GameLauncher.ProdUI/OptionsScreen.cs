using GameLauncher.ProdUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class OptionsScreen : BaseScreen
	{
		private IContainer components;

		private ComboBox wComboBoxLanguage;

		private Label wLabelLanguage;

		private Label wLabelTracks;

		private ComboBox wComboBoxTracks;

		private Label wLabelOptionsHeader;

		private MainButton wMainButtonContinue;

		private Label wLabelHavingTrouble;

		private LinkLabel wLinkLabelCustomerService;

		private Label wLabelLanguageDesc;

		private Label wLabelDownloadDesc;

		private GameLauncherUI parentForm;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameLauncher.ProdUI.OptionsScreen));
			wComboBoxLanguage = new System.Windows.Forms.ComboBox();
			wLabelLanguage = new System.Windows.Forms.Label();
			wLabelTracks = new System.Windows.Forms.Label();
			wComboBoxTracks = new System.Windows.Forms.ComboBox();
			wLabelOptionsHeader = new System.Windows.Forms.Label();
			wMainButtonContinue = new GameLauncher.ProdUI.MainButton();
			wLabelHavingTrouble = new System.Windows.Forms.Label();
			wLinkLabelCustomerService = new System.Windows.Forms.LinkLabel();
			wLabelLanguageDesc = new System.Windows.Forms.Label();
			wLabelDownloadDesc = new System.Windows.Forms.Label();
			SuspendLayout();
			wComboBoxLanguage.BackColor = System.Drawing.Color.White;
			wComboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			wComboBoxLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wComboBoxLanguage.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
			wComboBoxLanguage.FormattingEnabled = true;
			wComboBoxLanguage.Location = new System.Drawing.Point(77, 198);
			wComboBoxLanguage.Name = "wComboBoxLanguage";
			wComboBoxLanguage.Size = new System.Drawing.Size(185, 24);
			wComboBoxLanguage.TabIndex = 1;
			wLabelLanguage.BackColor = System.Drawing.Color.Transparent;
			wLabelLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold);
			wLabelLanguage.ForeColor = System.Drawing.Color.White;
			wLabelLanguage.Location = new System.Drawing.Point(74, 180);
			wLabelLanguage.Name = "wLabelLanguage";
			wLabelLanguage.Size = new System.Drawing.Size(171, 15);
			wLabelLanguage.TabStop = false;
			wLabelLanguage.Text = "SELECT LANGUAGE:";
			wLabelTracks.BackColor = System.Drawing.Color.Transparent;
			wLabelTracks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold);
			wLabelTracks.ForeColor = System.Drawing.Color.White;
			wLabelTracks.Location = new System.Drawing.Point(74, 251);
			wLabelTracks.Name = "wLabelTracks";
			wLabelTracks.Size = new System.Drawing.Size(171, 15);
			wLabelLanguage.TabStop = false;
			wLabelTracks.Text = "SELECT DOWNLOAD SIZE:";
			wComboBoxTracks.BackColor = System.Drawing.Color.White;
			wComboBoxTracks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			wComboBoxTracks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wComboBoxTracks.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
			wComboBoxTracks.FormattingEnabled = true;
			wComboBoxTracks.Location = new System.Drawing.Point(77, 269);
			wComboBoxTracks.Name = "wComboBoxTracks";
			wComboBoxTracks.Size = new System.Drawing.Size(189, 24);
			wComboBoxTracks.TabIndex = 2;
			wComboBoxTracks.SelectedIndexChanged += new System.EventHandler(wComboBoxTracks_SelectedIndexChanged);
			wLabelOptionsHeader.BackColor = System.Drawing.Color.Transparent;
			wLabelOptionsHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold);
			wLabelOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLabelOptionsHeader.Location = new System.Drawing.Point(74, 117);
			wLabelOptionsHeader.Name = "wLabelOptionsHeader";
			wLabelOptionsHeader.Size = new System.Drawing.Size(314, 30);
			wLabelLanguage.TabStop = false;
			wLabelOptionsHeader.Text = "PLEASE SELECT YOUR GAME SETTINGS:";
			wLabelOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			wMainButtonContinue.AutoSize = true;
			wMainButtonContinue.BackColor = System.Drawing.Color.Transparent;
			wMainButtonContinue.LabelButtonEnabled = false;
			wMainButtonContinue.Location = new System.Drawing.Point(613, 420);
			wMainButtonContinue.Name = "wMainButtonContinue";
			wMainButtonContinue.Size = new System.Drawing.Size(149, 37);
			wMainButtonContinue.TabIndex = 4;
			wMainButtonContinue.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(OptionsScreen_PreviewKeyDown);
			wLabelHavingTrouble.AutoSize = true;
			wLabelHavingTrouble.BackColor = System.Drawing.Color.Transparent;
			wLabelHavingTrouble.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wLabelHavingTrouble.ForeColor = System.Drawing.Color.White;
			wLabelHavingTrouble.Location = new System.Drawing.Point(32, 415);
			wLabelHavingTrouble.Name = "wLabelHavingTrouble";
			wLabelHavingTrouble.Size = new System.Drawing.Size(148, 16);
			wLabelLanguage.TabStop = false;
			wLabelHavingTrouble.Text = "HAVING TROUBLE?";
			wLinkLabelCustomerService.ActiveLinkColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLinkLabelCustomerService.AutoSize = true;
			wLinkLabelCustomerService.BackColor = System.Drawing.Color.Transparent;
			wLinkLabelCustomerService.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wLinkLabelCustomerService.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLinkLabelCustomerService.LinkColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLinkLabelCustomerService.Location = new System.Drawing.Point(32, 433);
			wLinkLabelCustomerService.Name = "wLinkLabelCustomerService";
			wLinkLabelCustomerService.Size = new System.Drawing.Size(226, 16);
			wLinkLabelCustomerService.TabIndex = 3;
			wLinkLabelCustomerService.TabStop = true;
			wLinkLabelCustomerService.Text = "Visit our customer service page";
			wLinkLabelCustomerService.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(wLinkLabelCustomerService_LinkClicked);
			wLabelLanguageDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f);
			wLabelLanguageDesc.ForeColor = System.Drawing.Color.White;
			wLabelLanguageDesc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			wLabelLanguageDesc.Location = new System.Drawing.Point(314, 179);
			wLabelLanguageDesc.Name = "wLabelLanguageDesc";
			wLabelLanguageDesc.Size = new System.Drawing.Size(427, 56);
			wLabelLanguage.TabStop = false;
			wLabelLanguageDesc.Text = "Select the language that the game text and audio should be displayed in. This will not affect your chat or server options.";
			wLabelLanguageDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			wLabelDownloadDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f);
			wLabelDownloadDesc.ForeColor = System.Drawing.Color.White;
			wLabelDownloadDesc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			wLabelDownloadDesc.Location = new System.Drawing.Point(313, 242);
			wLabelDownloadDesc.Name = "wLabelDownloadDesc";
			wLabelDownloadDesc.Size = new System.Drawing.Size(405, 85);
			wLabelLanguage.TabStop = false;
			wLabelDownloadDesc.Text = resources.GetString("wLabelDownloadDesc.Text");
			wLabelDownloadDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			BackColor = System.Drawing.Color.Transparent;
			BackgroundImage = GameLauncher.ProdUI.Properties.Resources.nfsw_lp_96dpi_bg_s0_w_1bit_alpha;
			base.Controls.Add(wLabelDownloadDesc);
			base.Controls.Add(wLabelLanguageDesc);
			base.Controls.Add(wLinkLabelCustomerService);
			base.Controls.Add(wLabelHavingTrouble);
			base.Controls.Add(wMainButtonContinue);
			base.Controls.Add(wLabelOptionsHeader);
			base.Controls.Add(wComboBoxTracks);
			base.Controls.Add(wLabelTracks);
			base.Controls.Add(wLabelLanguage);
			base.Controls.Add(wComboBoxLanguage);
			base.Name = "OptionsScreen";
			base.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(OptionsScreen_PreviewKeyDown);
			base.Controls.SetChildIndex(wComboBoxLanguage, 0);
			base.Controls.SetChildIndex(wLabelLanguage, 0);
			base.Controls.SetChildIndex(wLabelTracks, 0);
			base.Controls.SetChildIndex(wComboBoxTracks, 0);
			base.Controls.SetChildIndex(wLabelOptionsHeader, 0);
			base.Controls.SetChildIndex(wMainButtonContinue, 0);
			base.Controls.SetChildIndex(wLabelHavingTrouble, 0);
			base.Controls.SetChildIndex(wLinkLabelCustomerService, 0);
			base.Controls.SetChildIndex(wLabelLanguageDesc, 0);
			base.Controls.SetChildIndex(wLabelDownloadDesc, 0);
			ResumeLayout(false);
			PerformLayout();
		}

		public OptionsScreen(GameLauncherUI launcherForm)
			: base(launcherForm)
		{
			parentForm = launcherForm;
			InitializeComponent();
			InitializeSettings();
			ApplyEmbeddedFonts();
			LocalizeFE();
		}

		public override void LoadScreen()
		{
			base.LoadScreen();
			base.ActiveControl = wComboBoxLanguage;
		}

		private void FillLanguageCombo()
		{
			for (int i = 0; i < Settings.Default.Languages.Count; i += 2)
			{
				wComboBoxLanguage.Items.Add(Settings.Default.Languages[i]);
			}
			bool flag = false;
			if (!string.IsNullOrEmpty(parentForm.CommandArgLanguage) && wComboBoxLanguage.Items.Contains(parentForm.CommandArgLanguage))
			{
				wComboBoxLanguage.SelectedItem = parentForm.CommandArgLanguage;
				flag = true;
			}
			if (!flag && Settings.Default.Language < wComboBoxLanguage.Items.Count)
			{
				wComboBoxLanguage.SelectedIndex = Settings.Default.Language;
			}
		}

		private void FillTracksCombo()
		{
			for (int i = 0; i < Settings.Default.TracksFolders.Count; i++)
			{
				wComboBoxTracks.Items.Add(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER0005" + i));
			}
			bool flag = false;
			if (!string.IsNullOrEmpty(parentForm.CommandArgTracks) && wComboBoxTracks.Items.Contains(parentForm.CommandArgTracks))
			{
				wComboBoxTracks.SelectedItem = parentForm.CommandArgTracks;
				flag = true;
			}
			if (Settings.Default.Tracks == -1)
			{
				if (Directory.Exists("Data\\Tracks"))
				{
					Settings.Default.Tracks = 1;
				}
				else
				{
					Settings.Default.Tracks = 0;
				}
				Settings.Default.Save();
			}
			if (!flag && Settings.Default.Tracks < wComboBoxTracks.Items.Count)
			{
				wComboBoxTracks.SelectedIndex = Settings.Default.Tracks;
			}
		}

		private void SetSelectedLanguage()
		{
			if (wComboBoxLanguage.SelectedIndex != Settings.Default.Language)
			{
				GameLauncherUI.Logger.Info("ComboBox Language changed to " + wComboBoxLanguage.SelectedItem.ToString());
				Cursor = Cursors.WaitCursor;
				parentForm.StopDownload();
				Cursor = Cursors.Default;
				Settings.Default.Language = wComboBoxLanguage.SelectedIndex;
			}
		}

		private void SetSelectedTracks()
		{
			if (wComboBoxTracks.SelectedIndex != Settings.Default.Tracks)
			{
				Cursor = Cursors.WaitCursor;
				parentForm.StopDownload();
				Cursor = Cursors.Default;
				Settings.Default.Tracks = wComboBoxTracks.SelectedIndex;
			}
		}

		private void OptionsScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				wMainButtonContinue.ButtonPressed(isPressed: true);
				wMainButtonContinue.Update();
				wMainButtonContinue_Click(null, null);
				wMainButtonContinue.ButtonPressed(isPressed: false);
				wMainButtonContinue.Update();
			}
		}

		private void wMainButtonContinue_Click(object sender, EventArgs e)
		{
			if (wMainButtonContinue.LabelButtonEnabled)
			{
				SetSelectedLanguage();
				SetSelectedTracks();
				parentForm.SwitchToPreviousScreen();
			}
		}

		private void wComboBoxTracks_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (wComboBoxTracks.SelectedIndex == 1 && Settings.Default.Tracks != 1 && MessageBox.Show(ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00069"), ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00070"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				wComboBoxTracks.SelectedIndex = 0;
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

		protected override void InitializeSettings()
		{
			backgrounds = new Dictionary<double, Image>();
			backgrounds.Add(96.0, Resources.nfsw_lp_96dpi_bg_s0_w_1bit_alpha);
			backgrounds.Add(120.0, Resources.nfsw_lp_120dpi_bg_s0_w_1bit_alpha);
			backgrounds.Add(144.0, Resources.nfsw_lp_144dpi_bg_s0_w_1bit_alpha);
			BackgroundImage = SelectBackgroundImage();
			FillTracksCombo();
			FillLanguageCombo();
			wMainButtonContinue.LabelButtonEnabled = true;
			wMainButtonContinue.wLabelButton.Click += wMainButtonContinue_Click;
		}

		protected override void ApplyEmbeddedFonts()
		{
			FontFamily fontFamily = FontWrapper.Instance.GetFontFamily("MyriadProSemiCondBold.ttf");
			FontFamily fontFamily2 = FontWrapper.Instance.GetFontFamily("Reg-DB-I.ttf");
			FontFamily fontFamily3 = FontWrapper.Instance.GetFontFamily("Reg-B-I.ttf");
			float num = 1f;
			float num2 = 1f;
			if (string.Compare(Thread.CurrentThread.CurrentUICulture.Name, "th-TH", ignoreCase: true) == 0)
			{
				num = 1.4f;
				num2 = 1.3f;
			}
			wLabelOptionsHeader.Font = new Font(fontFamily2, 12.75f * num2, FontStyle.Italic);
			wLabelLanguage.Font = new Font(fontFamily, 9.749999f * num2, FontStyle.Bold);
			wComboBoxLanguage.Font = new Font(fontFamily, 9.749999f, FontStyle.Bold);
			wLabelLanguageDesc.Font = new Font(fontFamily, 9.749999f * num, FontStyle.Bold);
			wLabelTracks.Font = new Font(fontFamily, 9.749999f * num2, FontStyle.Bold);
			wComboBoxTracks.Font = new Font(fontFamily, 9.749999f * num2, FontStyle.Bold);
			wLabelDownloadDesc.Font = new Font(fontFamily, 9.749999f * num, FontStyle.Bold);
			wLabelHavingTrouble.Font = new Font(fontFamily, 9.749999f * num2, FontStyle.Bold);
			wLinkLabelCustomerService.Font = new Font(fontFamily, 9.749999f * num2, FontStyle.Bold);
			wMainButtonContinue.wLabelButton.Font = new Font(fontFamily3, 15f, FontStyle.Bold | FontStyle.Italic);
		}

		protected override void LocalizeFE()
		{
			wLabelOptionsHeader.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00073");
			wLabelLanguage.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00074");
			wLabelLanguageDesc.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00075");
			wLabelTracks.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00076");
			wLabelDownloadDesc.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00077");
			wLabelHavingTrouble.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00036");
			wLinkLabelCustomerService.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHERDESIGNER00037");
			wMainButtonContinue.wLabelButton.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00078");
		}
	}
}
