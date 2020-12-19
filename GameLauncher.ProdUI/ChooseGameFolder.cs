using GameLauncher.ProdUI.Screens;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class ChooseGameFolder : Form
	{
		private IContainer components;

		private GameFolder wGameFolderScreen;

		public string SelectedFolder => wGameFolderScreen.SelectedFolder;

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
			new System.ComponentModel.ComponentResourceManager(typeof(GameLauncher.ProdUI.ChooseGameFolder));
			SuspendLayout();
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(790, 490);
			ForeColor = System.Drawing.Color.FromArgb(51, 153, 255);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.MaximizeBox = false;
			MinimumSize = new System.Drawing.Size(790, 490);
			base.Name = "ChooseGameFolderUI";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Need for Speed™ World";
			base.TransparencyKey = System.Drawing.Color.Red;
			ResumeLayout(false);
		}

		public ChooseGameFolder()
		{
			InitializeComponent();
			wGameFolderScreen = new GameFolder(this);
			base.Controls.Add(wGameFolderScreen);
		}
	}
}
