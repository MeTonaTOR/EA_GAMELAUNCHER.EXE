using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class MainButton : UserControl
	{
		private IContainer components;

		private ImageList wImageList96;

		private ImageList wImageList120;

		private ImageList wImageList144;

		public Label wLabelButton;

		private bool mLabelButtonEnabled;

		private bool mLabelButtonHover;

		public bool LabelButtonEnabled
		{
			get
			{
				return mLabelButtonEnabled;
			}
			set
			{
				mLabelButtonEnabled = value;
				UpdateButton();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameLauncher.ProdUI.MainButton));
			wImageList96 = new System.Windows.Forms.ImageList(components);
			wImageList120 = new System.Windows.Forms.ImageList(components);
			wImageList144 = new System.Windows.Forms.ImageList(components);
			wLabelButton = new System.Windows.Forms.Label();
			SuspendLayout();
			wImageList96.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageList96.ImageStream");
			wImageList96.TransparentColor = System.Drawing.Color.Red;
			wImageList96.Images.SetKeyName(0, "nfsw_lp_96dpi_bg_s1_bt_login_disabled.bmp");
			wImageList96.Images.SetKeyName(1, "nfsw_lp_96dpi_bg_s1_bt_login_enabled.bmp");
			wImageList96.Images.SetKeyName(2, "nfsw_lp_96dpi_bg_s1_bt_login_rollover.bmp");
			wImageList96.Images.SetKeyName(3, "nfsw_lp_96dpi_bg_s1_bt_login_down.bmp");
			wImageList120.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageList120.ImageStream");
			wImageList120.TransparentColor = System.Drawing.Color.Red;
			wImageList120.Images.SetKeyName(0, "nfsw_lp_120dpi_bg_s1_bt_login_disabled.bmp");
			wImageList120.Images.SetKeyName(1, "nfsw_lp_120dpi_bg_s1_bt_login_enabled.bmp");
			wImageList120.Images.SetKeyName(2, "nfsw_lp_120dpi_bg_s1_bt_login_rollover.bmp");
			wImageList120.Images.SetKeyName(3, "nfsw_lp_120dpi_bg_s1_bt_login_down.bmp");
			wImageList144.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageList144.ImageStream");
			wImageList144.TransparentColor = System.Drawing.Color.Red;
			wImageList144.Images.SetKeyName(0, "nfsw_lp_144dpi_bg_s1_bt_login_disabled.bmp");
			wImageList144.Images.SetKeyName(1, "nfsw_lp_144dpi_bg_s1_bt_login_enabled.bmp");
			wImageList144.Images.SetKeyName(2, "nfsw_lp_144dpi_bg_s1_bt_login_rollover.bmp");
			wImageList144.Images.SetKeyName(3, "nfsw_lp_144dpi_bg_s1_bt_login_down.bmp");
			wLabelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			wLabelButton.ForeColor = System.Drawing.Color.White;
			wLabelButton.ImageIndex = 0;
			wLabelButton.ImageList = wImageList96;
			wLabelButton.Location = new System.Drawing.Point(0, 0);
			wLabelButton.Name = "wLabelButton";
			wLabelButton.Size = new System.Drawing.Size(146, 37);
			wLabelButton.TabIndex = 0;
			wLabelButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			wLabelButton.MouseLeave += new System.EventHandler(wLabelButton_MouseLeave);
			wLabelButton.MouseDown += new System.Windows.Forms.MouseEventHandler(wLabelButton_MouseDown);
			wLabelButton.MouseUp += new System.Windows.Forms.MouseEventHandler(wLabelButton_MouseUp);
			wLabelButton.MouseEnter += new System.EventHandler(wLabelButton_MouseEnter);
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			AutoSize = true;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(wLabelButton);
			base.Name = "MainButton";
			base.Size = new System.Drawing.Size(149, 37);
			base.GotFocus += new System.EventHandler(MainButton_GotFocus);
			base.LostFocus += new System.EventHandler(MainButton_LostFocus);
			ResumeLayout(false);
		}

		public MainButton()
		{
			InitializeComponent();
			SetImageLists();
			UpdateButton();
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
				wLabelButton.ImageList = wImageList96;
				return;
			}
			if ((double)dpiX <= 120.0)
			{
				wLabelButton.ImageList = wImageList120;
				return;
			}
			if ((double)dpiX <= 144.0)
			{
				wLabelButton.ImageList = wImageList144;
				return;
			}
			LauncherImage launcherImage = new LauncherImage(dpiX);
			wLabelButton.ImageList = launcherImage.ResizeImageList(wImageList144, 144f);
		}

		public void UpdateButton()
		{
			if (mLabelButtonEnabled)
			{
				wLabelButton.ImageIndex = 1;
				wLabelButton.ForeColor = Color.FromArgb(255, 255, 255);
			}
			else
			{
				wLabelButton.ImageIndex = 0;
				wLabelButton.ForeColor = Color.FromArgb(153, 153, 153);
			}
			wLabelButton.Update();
		}

		public void ButtonPressed(bool isPressed)
		{
			if (mLabelButtonEnabled)
			{
				if (isPressed)
				{
					wLabelButton.ImageIndex = 3;
				}
				else
				{
					wLabelButton.ImageIndex = 2;
				}
			}
		}

		private void MainButton_GotFocus(object sender, EventArgs e)
		{
			wLabelButton_MouseEnter(null, null);
		}

		private void MainButton_LostFocus(object sender, EventArgs e)
		{
			wLabelButton_MouseLeave(null, null);
		}

		private void wLabelButton_MouseDown(object sender, MouseEventArgs e)
		{
			ButtonPressed(isPressed: true);
		}

		private void wLabelButton_MouseEnter(object sender, EventArgs e)
		{
			if (mLabelButtonEnabled && !mLabelButtonHover)
			{
				wLabelButton.ImageIndex = 2;
				mLabelButtonHover = true;
			}
		}

		private void wLabelButton_MouseLeave(object sender, EventArgs e)
		{
			if (mLabelButtonEnabled && mLabelButtonHover)
			{
				wLabelButton.ImageIndex = 1;
				mLabelButtonHover = false;
			}
		}

		private void wLabelButton_MouseUp(object sender, MouseEventArgs e)
		{
			ButtonPressed(isPressed: false);
		}
	}
}
