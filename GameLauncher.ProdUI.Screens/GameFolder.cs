using GameLauncher.ProdUI.Controls;
using GameLauncher.ProdUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GameLauncher.ProdUI.Screens
{
	public class GameFolder : BaseScreen
	{
		private Form parentForm;

		private string selectedFolder;

		private IContainer components;

		private MainButton wMainButtonContinue;

		private Label wLabelDescription;

		private TextBox wTextFolder;

		private FolderButton wFolderButton;

		private Label wLabelFolderHeader;

		public string SelectedFolder => selectedFolder;

		public GameFolder(Form launcherForm)
			: base(launcherForm)
		{
			parentForm = launcherForm;
			InitializeComponent();
			InitializeSettings();
			ApplyEmbeddedFonts();
			LocalizeFE();
		}

		protected override void InitializeSettings()
		{
			backgrounds = new Dictionary<double, Image>();
			backgrounds.Add(96.0, Resources.nfsw_lp_96dpi_bg_s0_w_1bit_alpha);
			backgrounds.Add(120.0, Resources.nfsw_lp_120dpi_bg_s0_w_1bit_alpha);
			backgrounds.Add(144.0, Resources.nfsw_lp_144dpi_bg_s0_w_1bit_alpha);
			BackgroundImage = SelectBackgroundImage();
			wFolderButton.wLabelFolder.Click += wFolderButton_Click;
			wMainButtonContinue.LabelButtonEnabled = false;
			wMainButtonContinue.wLabelButton.Click += wContinueButton_Click;
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			folderPath = Path.Combine(folderPath, "Electronic Arts\\Need for Speed World");
			SetSelectedFolder(folderPath);
		}

		protected override void ApplyEmbeddedFonts()
		{
			FontFamily fontFamily = FontWrapper.Instance.GetFontFamily("MyriadProSemiCondBold.ttf");
			FontFamily fontFamily2 = FontWrapper.Instance.GetFontFamily("Reg-DB-I.ttf");
			FontFamily fontFamily3 = FontWrapper.Instance.GetFontFamily("Reg-B-I.ttf");
			wLabelFolderHeader.Font = new Font(fontFamily2, 12.75f, FontStyle.Italic);
			wLabelDescription.Font = new Font(fontFamily, 9.749999f, FontStyle.Bold);
			wMainButtonContinue.wLabelButton.Font = new Font(fontFamily3, 15f, FontStyle.Bold | FontStyle.Italic);
		}

		protected override void LocalizeFE()
		{
			wMainButtonContinue.wLabelButton.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00078");
			wLabelFolderHeader.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00079");
			wLabelDescription.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "GAMELAUNCHER00080");
		}

		private void wFolderButton_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = SelectedFolder;
			DialogResult dialogResult = folderBrowserDialog.ShowDialog();
			_ = string.Empty;
			if (dialogResult == DialogResult.OK)
			{
				SetSelectedFolder(folderBrowserDialog.SelectedPath);
			}
		}

		private void wContinueButton_Click(object sender, EventArgs e)
		{
			parentForm.Close();
		}

		public void SetSelectedFolder(string folder)
		{
			selectedFolder = folder;
			wTextFolder.Text = selectedFolder;
			wMainButtonContinue.LabelButtonEnabled = !string.IsNullOrEmpty(selectedFolder);
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
			wMainButtonContinue = new GameLauncher.ProdUI.MainButton();
			wLabelDescription = new System.Windows.Forms.Label();
			wTextFolder = new System.Windows.Forms.TextBox();
			wFolderButton = new GameLauncher.ProdUI.Controls.FolderButton();
			wLabelFolderHeader = new System.Windows.Forms.Label();
			SuspendLayout();
			wMainButtonContinue.AutoSize = true;
			wMainButtonContinue.BackColor = System.Drawing.Color.Transparent;
			wMainButtonContinue.LabelButtonEnabled = false;
			wMainButtonContinue.Location = new System.Drawing.Point(613, 420);
			wMainButtonContinue.Name = "wMainButtonContinue";
			wMainButtonContinue.Size = new System.Drawing.Size(149, 37);
			wMainButtonContinue.TabIndex = 19;
			wLabelDescription.BackColor = System.Drawing.Color.Transparent;
			wLabelDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold);
			wLabelDescription.ForeColor = System.Drawing.Color.White;
			wLabelDescription.Location = new System.Drawing.Point(62, 213);
			wLabelDescription.Name = "wLabelDescription";
			wLabelDescription.Size = new System.Drawing.Size(663, 37);
			wLabelDescription.TabIndex = 25;
			wLabelDescription.Text = "Select a folder where the game will be downloaded, this folder needs to have write access for anyone playing the game.";
			wTextFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			wTextFolder.Location = new System.Drawing.Point(65, 253);
			wTextFolder.Name = "wTextFolder";
			wTextFolder.ReadOnly = true;
			wTextFolder.Size = new System.Drawing.Size(600, 20);
			wTextFolder.TabIndex = 26;
			wFolderButton.AutoSize = true;
			wFolderButton.BackColor = System.Drawing.Color.Transparent;
			wFolderButton.Location = new System.Drawing.Point(671, 249);
			wFolderButton.MinimumSize = new System.Drawing.Size(31, 29);
			wFolderButton.Name = "wFolderButton";
			wFolderButton.Size = new System.Drawing.Size(35, 29);
			wFolderButton.TabIndex = 27;
			wLabelFolderHeader.BackColor = System.Drawing.Color.Transparent;
			wLabelFolderHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold);
			wLabelFolderHeader.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLabelFolderHeader.Location = new System.Drawing.Point(74, 122);
			wLabelFolderHeader.Name = "wLabelFolderHeader";
			wLabelFolderHeader.Size = new System.Drawing.Size(314, 25);
			wLabelFolderHeader.TabIndex = 28;
			wLabelFolderHeader.Text = "PLEASE SELECT YOUR GAME FOLDER";
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			BackgroundImage = GameLauncher.ProdUI.Properties.Resources.nfsw_lp_96dpi_bg_s0_w_1bit_alpha;
			base.Controls.Add(wLabelFolderHeader);
			base.Controls.Add(wFolderButton);
			base.Controls.Add(wTextFolder);
			base.Controls.Add(wLabelDescription);
			base.Controls.Add(wMainButtonContinue);
			base.Name = "GameFolder";
			base.Controls.SetChildIndex(wMainButtonContinue, 0);
			base.Controls.SetChildIndex(wLabelDescription, 0);
			base.Controls.SetChildIndex(wTextFolder, 0);
			base.Controls.SetChildIndex(wFolderButton, 0);
			base.Controls.SetChildIndex(wLabelFolderHeader, 0);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
