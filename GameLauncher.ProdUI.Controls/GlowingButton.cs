using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GameLauncher.ProdUI.Controls
{
	public class GlowingButton : UserControl
	{
		private IContainer components;

		public Label wLabelButton;

		private Timer wTimerGlowingButton;

		private ImageList wImageListButton96;

		private ImageList wImageListButton120;

		private ImageList wImageListButton144;

		private Image mLabelButtonImage;

		private float mCurrentRange = -8f;

		private float mMinRange = -8f;

		private float mMaxRange = 8f;

		private bool mLabelButtonHover;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameLauncher.ProdUI.Controls.GlowingButton));
			wLabelButton = new System.Windows.Forms.Label();
			wImageListButton96 = new System.Windows.Forms.ImageList(components);
			wTimerGlowingButton = new System.Windows.Forms.Timer(components);
			wImageListButton120 = new System.Windows.Forms.ImageList(components);
			wImageListButton144 = new System.Windows.Forms.ImageList(components);
			SuspendLayout();
			wLabelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			wLabelButton.ForeColor = System.Drawing.Color.White;
			wLabelButton.ImageIndex = 0;
			wLabelButton.ImageList = wImageListButton96;
			wLabelButton.Location = new System.Drawing.Point(0, 0);
			wLabelButton.Name = "wLabelButton";
			wLabelButton.Size = new System.Drawing.Size(165, 58);
			wLabelButton.TabIndex = 31;
			wLabelButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			wLabelButton.MouseEnter += new System.EventHandler(wLabelButton_MouseEnter);
			wLabelButton.MouseLeave += new System.EventHandler(wLabelButton_MouseLeave);
			wLabelButton.MouseDown += new System.Windows.Forms.MouseEventHandler(wLabelButton_MouseDown);
			wLabelButton.MouseUp += new System.Windows.Forms.MouseEventHandler(wLabelButton_MouseUp);
			wImageListButton96.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButton96.ImageStream");
			wImageListButton96.TransparentColor = System.Drawing.Color.Red;
			wImageListButton96.Images.SetKeyName(0, "nfsw_lp_96dpi_bg_s2_bt_play_disabled.bmp");
			wImageListButton96.Images.SetKeyName(1, "nfsw_lp_96dpi_bg_s2_bt_play_enabled.bmp");
			wImageListButton96.Images.SetKeyName(2, "nfsw_lp_96dpi_bg_s2_bt_play_rollover.bmp");
			wImageListButton96.Images.SetKeyName(3, "nfsw_lp_96dpi_bg_s2_bt_play_down.bmp");
			wTimerGlowingButton.Tick += new System.EventHandler(wTimerGlowingButton_Tick);
			wImageListButton120.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButton120.ImageStream");
			wImageListButton120.TransparentColor = System.Drawing.Color.Red;
			wImageListButton120.Images.SetKeyName(0, "nfsw_lp_120dpi_bg_s2_bt_play_disabled.bmp");
			wImageListButton120.Images.SetKeyName(1, "nfsw_lp_120dpi_bg_s2_bt_play_enabled.bmp");
			wImageListButton120.Images.SetKeyName(2, "nfsw_lp_120dpi_bg_s2_bt_play_rollover.bmp");
			wImageListButton120.Images.SetKeyName(3, "nfsw_lp_120dpi_bg_s2_bt_play_down.bmp");
			wImageListButton144.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButton144.ImageStream");
			wImageListButton144.TransparentColor = System.Drawing.Color.Red;
			wImageListButton144.Images.SetKeyName(0, "nfsw_lp_144dpi_bg_s2_bt_play_disabled.bmp");
			wImageListButton144.Images.SetKeyName(1, "nfsw_lp_144dpi_bg_s2_bt_play_enabled.bmp");
			wImageListButton144.Images.SetKeyName(2, "nfsw_lp_144dpi_bg_s2_bt_play_rollover.bmp");
			wImageListButton144.Images.SetKeyName(3, "nfsw_lp_144dpi_bg_s2_bt_play_down.bmp");
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			AutoSize = true;
			base.Controls.Add(wLabelButton);
			base.Name = "GlowingButton";
			base.Size = new System.Drawing.Size(168, 58);
			ResumeLayout(false);
			base.GotFocus += new System.EventHandler(GlowingButton_GotFocus);
			base.LostFocus += new System.EventHandler(GlowingButton_LostFocus);
		}

		public GlowingButton()
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
				wLabelButton.ImageList = wImageListButton96;
				return;
			}
			if ((double)dpiX <= 120.0)
			{
				wLabelButton.ImageList = wImageListButton120;
				return;
			}
			if ((double)dpiX <= 144.0)
			{
				wLabelButton.ImageList = wImageListButton144;
				return;
			}
			LauncherImage launcherImage = new LauncherImage(dpiX);
			wLabelButton.ImageList = launcherImage.ResizeImageList(wImageListButton144, 144f);
		}

		public void ButtonPressed(bool isPressed)
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

		private void GlowingButton_GotFocus(object sender, EventArgs e)
		{
			wLabelButton_MouseEnter(null, null);
		}

		private void GlowingButton_LostFocus(object sender, EventArgs e)
		{
			wLabelButton_MouseLeave(null, null);
		}

		private void wLabelButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (wLabelButton.ImageIndex != 0)
			{
				ButtonPressed(isPressed: true);
			}
		}

		private void wLabelButton_MouseEnter(object sender, EventArgs e)
		{
			if (wLabelButton.ImageIndex != 0 && !mLabelButtonHover)
			{
				wLabelButton.ImageIndex = 2;
				mLabelButtonHover = true;
			}
		}

		private void wLabelButton_MouseLeave(object sender, EventArgs e)
		{
			if (wLabelButton.ImageIndex != 0 && mLabelButtonHover)
			{
				wLabelButton.ImageIndex = 1;
				mLabelButtonHover = false;
			}
		}

		private void wLabelButton_MouseUp(object sender, MouseEventArgs e)
		{
			if (wLabelButton.ImageIndex != 0)
			{
				ButtonPressed(isPressed: false);
			}
		}

		private void wTimerGlowingButton_Tick(object sender, EventArgs e)
		{
			try
			{
				if (wLabelButton.ImageIndex == -1)
				{
					goto IL_004d;
				}
				SetImageLists();
				if (wLabelButton.ImageIndex != 1)
				{
					return;
				}
				mLabelButtonImage = wLabelButton.ImageList.Images[wLabelButton.ImageIndex];
				goto IL_004d;
				IL_004d:
				float imgOpac = Utils.Gauss(mCurrentRange, 1f, 0f, 0.4f, 0f, inverted: false);
				wLabelButton.Image = LauncherImage.SetImgOpacity(mLabelButtonImage, imgOpac);
				if (mCurrentRange < mMaxRange)
				{
					mCurrentRange += 0.8f;
				}
				else
				{
					mCurrentRange = mMinRange;
				}
			}
			catch (Exception ex)
			{
				wTimerGlowingButton.Enabled = false;
				GameLauncherUI.Logger.Error("Exception in the glowing timer, turning glowing off");
				GameLauncherUI.Logger.Error("wTimerGlowingButton_Tick Exception: " + ex.ToString());
				SetImageLists();
				wLabelButton.ImageIndex = 1;
			}
		}

		public void DisableButton(string text)
		{
			if (wLabelButton.ImageList == null)
			{
				SetImageLists();
			}
			wLabelButton.ImageIndex = 0;
			wLabelButton.ForeColor = Color.FromArgb(153, 153, 153);
			wLabelButton.Text = text;
			mCurrentRange = mMinRange;
			wTimerGlowingButton.Enabled = false;
		}

		public void EnableButton(string text)
		{
			if (wLabelButton.ImageList == null)
			{
				SetImageLists();
			}
			wLabelButton.ImageIndex = 1;
			wLabelButton.ForeColor = Color.FromArgb(255, 255, 255);
			wLabelButton.Text = text;
			mCurrentRange = mMinRange;
			wTimerGlowingButton.Enabled = true;
		}
	}
}
