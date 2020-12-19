using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GameLauncher.ProdUI.Controls
{
	public class OptionsButton : UserControl
	{
		private bool mLabelOptionsHover;

		private IContainer components;

		private ImageList wImageList96;

		private ImageList wImageList120;

		private ImageList wImageList144;

		public Label wLabelOptions;

		public OptionsButton()
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
				wLabelOptions.ImageList = wImageList96;
				return;
			}
			if ((double)dpiX <= 120.0)
			{
				wLabelOptions.ImageList = wImageList120;
				return;
			}
			if ((double)dpiX <= 144.0)
			{
				wLabelOptions.ImageList = wImageList144;
				return;
			}
			LauncherImage launcherImage = new LauncherImage(dpiX);
			wLabelOptions.ImageList = launcherImage.ResizeImageList(wImageList144, 144f);
		}

		public void ButtonPressed(bool isPressed)
		{
			if (isPressed)
			{
				wLabelOptions.ImageIndex = 3;
			}
			else
			{
				wLabelOptions.ImageIndex = 2;
			}
		}

		private void OptionsButton_GotFocus(object sender, EventArgs e)
		{
			wLabelOptions_MouseEnter(null, null);
		}

		private void OptionsButton_LostFocus(object sender, EventArgs e)
		{
			wLabelOptions_MouseLeave(null, null);
		}

		private void wLabelOptions_MouseDown(object sender, MouseEventArgs e)
		{
			ButtonPressed(isPressed: true);
		}

		private void wLabelOptions_MouseEnter(object sender, EventArgs e)
		{
			if (!mLabelOptionsHover)
			{
				wLabelOptions.ImageIndex = 2;
				mLabelOptionsHover = true;
			}
		}

		private void wLabelOptions_MouseLeave(object sender, EventArgs e)
		{
			if (mLabelOptionsHover)
			{
				wLabelOptions.ImageIndex = 1;
				mLabelOptionsHover = false;
			}
		}

		private void wLabelOptions_MouseUp(object sender, MouseEventArgs e)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameLauncher.ProdUI.Controls.OptionsButton));
			wImageList96 = new System.Windows.Forms.ImageList(components);
			wImageList120 = new System.Windows.Forms.ImageList(components);
			wImageList144 = new System.Windows.Forms.ImageList(components);
			wLabelOptions = new System.Windows.Forms.Label();
			SuspendLayout();
			wImageList96.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageList96.ImageStream");
			wImageList96.TransparentColor = System.Drawing.Color.Red;
			wImageList96.Images.SetKeyName(0, "nfsw_lp_96dpi_bt_options_enabled.png");
			wImageList96.Images.SetKeyName(1, "nfsw_lp_96dpi_bt_options_rollover.png");
			wImageList96.Images.SetKeyName(2, "nfsw_lp_96dpi_bt_options_down.png");
			wImageList120.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageList120.ImageStream");
			wImageList120.TransparentColor = System.Drawing.Color.Red;
			wImageList120.Images.SetKeyName(0, "nfsw_lp_120dpi_bt_options_enabled.png");
			wImageList120.Images.SetKeyName(1, "nfsw_lp_120dpi_bt_options_rollover.png");
			wImageList120.Images.SetKeyName(2, "nfsw_lp_120dpi_bt_options_down.png");
			wImageList144.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageList144.ImageStream");
			wImageList144.TransparentColor = System.Drawing.Color.Red;
			wImageList144.Images.SetKeyName(0, "nfsw_lp_144dpi_bt_options_enabled.png");
			wImageList144.Images.SetKeyName(1, "nfsw_lp_144dpi_bt_options_rollover.png");
			wImageList144.Images.SetKeyName(2, "nfsw_lp_144dpi_bt_options_down.png");
			wLabelOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 15f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			wLabelOptions.ForeColor = System.Drawing.Color.White;
			wLabelOptions.ImageIndex = 0;
			wLabelOptions.ImageList = wImageList96;
			wLabelOptions.Location = new System.Drawing.Point(1, 0);
			wLabelOptions.Name = "wLabelOptions";
			wLabelOptions.Size = new System.Drawing.Size(31, 29);
			wLabelOptions.TabStop = false;
			wLabelOptions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			wLabelOptions.MouseLeave += new System.EventHandler(wLabelOptions_MouseLeave);
			wLabelOptions.MouseDown += new System.Windows.Forms.MouseEventHandler(wLabelOptions_MouseDown);
			wLabelOptions.MouseUp += new System.Windows.Forms.MouseEventHandler(wLabelOptions_MouseUp);
			wLabelOptions.MouseEnter += new System.EventHandler(wLabelOptions_MouseEnter);
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			AutoSize = true;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(wLabelOptions);
			MinimumSize = new System.Drawing.Size(31, 29);
			base.Name = "OptionsButton";
			base.Size = new System.Drawing.Size(35, 29);
			ResumeLayout(false);
			base.GotFocus += new System.EventHandler(OptionsButton_GotFocus);
			base.LostFocus += new System.EventHandler(OptionsButton_LostFocus);
		}
	}
}
