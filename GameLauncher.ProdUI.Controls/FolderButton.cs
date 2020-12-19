using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GameLauncher.ProdUI.Controls
{
	public class FolderButton : UserControl
	{
		private bool mLabelFolderHover;

		private IContainer components;

		public Label wLabelFolder;

		private ImageList wImageList96;

		private ImageList wImageList144;

		private ImageList wImageList120;

		public FolderButton()
		{
			InitializeComponent();
			SetImageLists();
		}

		private void SetImageLists()
		{
			float dpiX;
			using (Graphics graphics = CreateGraphics())
			{
				dpiX = graphics.DpiX;
			}
			if ((double)dpiX <= 96.0)
			{
				wLabelFolder.ImageList = wImageList96;
				return;
			}
			if ((double)dpiX <= 120.0)
			{
				wLabelFolder.ImageList = wImageList120;
				return;
			}
			if ((double)dpiX <= 144.0)
			{
				wLabelFolder.ImageList = wImageList144;
				return;
			}
			LauncherImage launcherImage = new LauncherImage(dpiX);
			wLabelFolder.ImageList = launcherImage.ResizeImageList(wImageList144, 144f);
		}

		public void ButtonPressed(bool isPressed)
		{
			if (isPressed)
			{
				wLabelFolder.ImageIndex = 3;
			}
			else
			{
				wLabelFolder.ImageIndex = 2;
			}
		}

		private void OptionsButton_GotFocus(object sender, EventArgs e)
		{
			wLabelFolder_MouseEnter(null, null);
		}

		private void OptionsButton_LostFocus(object sender, EventArgs e)
		{
			wLabelFolder_MouseLeave(null, null);
		}

		private void wLabelFolder_MouseDown(object sender, MouseEventArgs e)
		{
			ButtonPressed(isPressed: true);
		}

		private void wLabelFolder_MouseEnter(object sender, EventArgs e)
		{
			if (!mLabelFolderHover)
			{
				wLabelFolder.ImageIndex = 2;
				mLabelFolderHover = true;
			}
		}

		private void wLabelFolder_MouseLeave(object sender, EventArgs e)
		{
			if (mLabelFolderHover)
			{
				wLabelFolder.ImageIndex = 1;
				mLabelFolderHover = false;
			}
		}

		private void wLabelFolder_MouseUp(object sender, MouseEventArgs e)
		{
			ButtonPressed(isPressed: false);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameLauncher.ProdUI.Controls.FolderButton));
			wLabelFolder = new System.Windows.Forms.Label();
			wImageList96 = new System.Windows.Forms.ImageList(components);
			wImageList144 = new System.Windows.Forms.ImageList(components);
			wImageList120 = new System.Windows.Forms.ImageList(components);
			SuspendLayout();
			wLabelFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 15f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			wLabelFolder.ForeColor = System.Drawing.Color.White;
			wLabelFolder.ImageIndex = 0;
			wLabelFolder.ImageList = wImageList96;
			wLabelFolder.Location = new System.Drawing.Point(1, 0);
			wLabelFolder.Name = "wLabelFolder";
			wLabelFolder.Size = new System.Drawing.Size(31, 29);
			wLabelFolder.TabIndex = 0;
			wLabelFolder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			wLabelFolder.MouseLeave += new System.EventHandler(wLabelFolder_MouseLeave);
			wLabelFolder.MouseDown += new System.Windows.Forms.MouseEventHandler(wLabelFolder_MouseDown);
			wLabelFolder.MouseUp += new System.Windows.Forms.MouseEventHandler(wLabelFolder_MouseUp);
			wLabelFolder.MouseEnter += new System.EventHandler(wLabelFolder_MouseEnter);
			wImageList96.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageList96.ImageStream");
			wImageList96.TransparentColor = System.Drawing.Color.Red;
			wImageList96.Images.SetKeyName(0, "nfsw_lp_120dpi_bt_folder_enabled.png");
			wImageList96.Images.SetKeyName(1, "nfsw_lp_120dpi_bt_folder_rollover.png");
			wImageList96.Images.SetKeyName(2, "nfsw_lp_120dpi_bt_folder_down.png");
			wImageList144.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageList144.ImageStream");
			wImageList144.TransparentColor = System.Drawing.Color.Red;
			wImageList144.Images.SetKeyName(0, "nfsw_lp_144dpi_bt_folder_enabled.png");
			wImageList144.Images.SetKeyName(1, "nfsw_lp_144dpi_bt_folder_rollover.png");
			wImageList144.Images.SetKeyName(2, "nfsw_lp_144dpi_bt_folder_down.png");
			wImageList120.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageList120.ImageStream");
			wImageList120.TransparentColor = System.Drawing.Color.Red;
			wImageList120.Images.SetKeyName(0, "nfsw_lp_120dpi_bt_folder_enabled.png");
			wImageList120.Images.SetKeyName(1, "nfsw_lp_120dpi_bt_folder_rollover.png");
			wImageList120.Images.SetKeyName(2, "nfsw_lp_120dpi_bt_folder_down.png");
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			AutoSize = true;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(wLabelFolder);
			MinimumSize = new System.Drawing.Size(31, 29);
			base.Name = "FolderButton";
			base.Size = new System.Drawing.Size(35, 29);
			base.GotFocus += new System.EventHandler(OptionsButton_GotFocus);
			base.LostFocus += new System.EventHandler(OptionsButton_LostFocus);
			ResumeLayout(false);
		}
	}
}
