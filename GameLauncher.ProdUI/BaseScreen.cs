using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class BaseScreen : UserControl
	{
		protected float mDpi;

		protected LauncherImage launcherImage;

		private Form launcherForm;

		private bool isActive;

		protected Dictionary<double, Image> backgrounds;

		private bool mLabelButtonMinimizeHover;

		private bool mLabelButtonCloseHover;

		private bool mDragging;

		private Point mDraggingStart;

		private Point mDraggingCursorStart;

		private IContainer components;

		private Label wLabelButtonMinimize;

		private Label wLabelButtonClose;

		private ImageList wImageListButtonMinimize96;

		private ImageList wImageListButtonMinimize120;

		private ImageList wImageListButtonMinimize144;

		private ImageList wImageListButtonClose96;

		private ImageList wImageListButtonClose120;

		private ImageList wImageListButtonClose144;

		public bool IsActive => isActive;

		public BaseScreen()
		{
			InitializeComponent();
		}

		public BaseScreen(Form launcherForm)
		{
			InitializeComponent();
			this.launcherForm = launcherForm;
			using (Graphics graphics = CreateGraphics())
			{
				mDpi = graphics.DpiX;
			}
			launcherImage = new LauncherImage(mDpi);
			SelectImageList();
		}

		protected void SelectImageList()
		{
			if ((double)mDpi <= 96.0)
			{
				wLabelButtonMinimize.ImageList = wImageListButtonMinimize96;
				wLabelButtonClose.ImageList = wImageListButtonClose96;
			}
			else if ((double)mDpi <= 120.0)
			{
				wLabelButtonMinimize.ImageList = wImageListButtonMinimize120;
				wLabelButtonClose.ImageList = wImageListButtonClose120;
			}
			else if ((double)mDpi <= 144.0)
			{
				wLabelButtonMinimize.ImageList = wImageListButtonMinimize144;
				wLabelButtonClose.ImageList = wImageListButtonClose144;
			}
			else
			{
				wLabelButtonMinimize.ImageList = launcherImage.ResizeImageList(wImageListButtonMinimize144, 144f);
				wLabelButtonClose.ImageList = launcherImage.ResizeImageList(wImageListButtonClose144, 144f);
			}
		}

		public virtual void LoadScreen()
		{
			isActive = true;
		}

		public virtual void UnloadScreen()
		{
			isActive = false;
		}

		protected Image SelectBackgroundImage()
		{
			Image image = null;
			image = (((double)mDpi <= 96.0) ? backgrounds[96.0] : (((double)mDpi <= 120.0) ? backgrounds[120.0] : ((!((double)mDpi <= 144.0)) ? launcherImage.ResizeImage(backgrounds[144.0], 1440f) : backgrounds[144.0])));
			Bitmap bitmap = new Bitmap(image);
			Color pixel = bitmap.GetPixel(1, 1);
			bitmap.MakeTransparent(pixel);
			launcherForm.BackColor = pixel;
			launcherForm.TransparencyKey = pixel;
			return bitmap;
		}

		protected virtual void InitializeSettings()
		{
		}

		protected virtual void ApplyEmbeddedFonts()
		{
		}

		protected virtual void LocalizeFE()
		{
		}

		private void wLabelButtonMinimize_Click(object sender, EventArgs e)
		{
			launcherForm.WindowState = FormWindowState.Minimized;
		}

		private void wLabelButtonMinimize_MouseDown(object sender, MouseEventArgs e)
		{
			wLabelButtonMinimize.ImageIndex = 2;
		}

		private void wLabelButtonMinimize_MouseEnter(object sender, EventArgs e)
		{
			if (!mLabelButtonMinimizeHover)
			{
				wLabelButtonMinimize.ImageIndex = 1;
				mLabelButtonMinimizeHover = true;
			}
		}

		private void wLabelButtonMinimize_MouseLeave(object sender, EventArgs e)
		{
			if (mLabelButtonMinimizeHover)
			{
				wLabelButtonMinimize.ImageIndex = 0;
				mLabelButtonMinimizeHover = false;
			}
		}

		private void wLabelButtonMinimize_MouseUp(object sender, MouseEventArgs e)
		{
			wLabelButtonMinimize.ImageIndex = 0;
		}

		private void wLabelButtonClose_Click(object sender, EventArgs e)
		{
			launcherForm.Close();
		}

		private void wLabelButtonClose_MouseDown(object sender, MouseEventArgs e)
		{
			wLabelButtonClose.ImageIndex = 2;
		}

		private void wLabelButtonClose_MouseEnter(object sender, EventArgs e)
		{
			if (!mLabelButtonCloseHover)
			{
				wLabelButtonClose.ImageIndex = 1;
				mLabelButtonCloseHover = true;
			}
		}

		private void wLabelButtonClose_MouseLeave(object sender, EventArgs e)
		{
			if (mLabelButtonCloseHover)
			{
				wLabelButtonClose.ImageIndex = 0;
				mLabelButtonCloseHover = false;
			}
		}

		private void wLabelButtonClose_MouseUp(object sender, MouseEventArgs e)
		{
			wLabelButtonClose.ImageIndex = 0;
		}

		private void BaseScreen_MouseDown(object sender, MouseEventArgs e)
		{
			mDragging = true;
			mDraggingStart = launcherForm.Location;
			mDraggingCursorStart = Cursor.Position;
		}

		private void BaseScreen_MouseMove(object sender, MouseEventArgs e)
		{
			if (mDragging)
			{
				launcherForm.Location = new Point(mDraggingStart.X + Cursor.Position.X - mDraggingCursorStart.X, mDraggingStart.Y + Cursor.Position.Y - mDraggingCursorStart.Y);
			}
		}

		private void BaseScreen_MouseUp(object sender, MouseEventArgs e)
		{
			mDragging = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameLauncher.ProdUI.BaseScreen));
			wLabelButtonMinimize = new System.Windows.Forms.Label();
			wImageListButtonMinimize96 = new System.Windows.Forms.ImageList(components);
			wLabelButtonClose = new System.Windows.Forms.Label();
			wImageListButtonClose96 = new System.Windows.Forms.ImageList(components);
			wImageListButtonMinimize120 = new System.Windows.Forms.ImageList(components);
			wImageListButtonMinimize144 = new System.Windows.Forms.ImageList(components);
			wImageListButtonClose120 = new System.Windows.Forms.ImageList(components);
			wImageListButtonClose144 = new System.Windows.Forms.ImageList(components);
			SuspendLayout();
			wLabelButtonMinimize.BackColor = System.Drawing.Color.Transparent;
			wLabelButtonMinimize.ForeColor = System.Drawing.Color.Transparent;
			wLabelButtonMinimize.ImageIndex = 0;
			wLabelButtonMinimize.ImageList = wImageListButtonMinimize96;
			wLabelButtonMinimize.Location = new System.Drawing.Point(708, 1);
			wLabelButtonMinimize.Name = "wLabelButtonMinimize";
			wLabelButtonMinimize.Size = new System.Drawing.Size(34, 26);
			wLabelButtonMinimize.TabIndex = 17;
			wLabelButtonMinimize.MouseLeave += new System.EventHandler(wLabelButtonMinimize_MouseLeave);
			wLabelButtonMinimize.Click += new System.EventHandler(wLabelButtonMinimize_Click);
			wLabelButtonMinimize.MouseDown += new System.Windows.Forms.MouseEventHandler(wLabelButtonMinimize_MouseDown);
			wLabelButtonMinimize.MouseUp += new System.Windows.Forms.MouseEventHandler(wLabelButtonMinimize_MouseUp);
			wLabelButtonMinimize.MouseEnter += new System.EventHandler(wLabelButtonMinimize_MouseEnter);
			wImageListButtonMinimize96.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButtonMinimize96.ImageStream");
			wImageListButtonMinimize96.TransparentColor = System.Drawing.Color.Red;
			wImageListButtonMinimize96.Images.SetKeyName(0, "nfsw_lp_96dpi_bt_minimize_enabled.bmp");
			wImageListButtonMinimize96.Images.SetKeyName(1, "nfsw_lp_96dpi_bt_minimize_rollover.bmp");
			wImageListButtonMinimize96.Images.SetKeyName(2, "nfsw_lp_96dpi_bt_minimize_down.bmp");
			wLabelButtonClose.BackColor = System.Drawing.Color.Transparent;
			wLabelButtonClose.ForeColor = System.Drawing.Color.Transparent;
			wLabelButtonClose.ImageIndex = 0;
			wLabelButtonClose.ImageList = wImageListButtonClose96;
			wLabelButtonClose.Location = new System.Drawing.Point(742, 1);
			wLabelButtonClose.Name = "wLabelButtonClose";
			wLabelButtonClose.Size = new System.Drawing.Size(33, 26);
			wLabelButtonClose.TabIndex = 18;
			wLabelButtonClose.MouseLeave += new System.EventHandler(wLabelButtonClose_MouseLeave);
			wLabelButtonClose.Click += new System.EventHandler(wLabelButtonClose_Click);
			wLabelButtonClose.MouseDown += new System.Windows.Forms.MouseEventHandler(wLabelButtonClose_MouseDown);
			wLabelButtonClose.MouseUp += new System.Windows.Forms.MouseEventHandler(wLabelButtonClose_MouseUp);
			wLabelButtonClose.MouseEnter += new System.EventHandler(wLabelButtonClose_MouseEnter);
			wImageListButtonClose96.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButtonClose96.ImageStream");
			wImageListButtonClose96.TransparentColor = System.Drawing.Color.Red;
			wImageListButtonClose96.Images.SetKeyName(0, "nfsw_lp_96dpi_bt_close_enabled.bmp");
			wImageListButtonClose96.Images.SetKeyName(1, "nfsw_lp_96dpi_bt_close_rollover.bmp");
			wImageListButtonClose96.Images.SetKeyName(2, "nfsw_lp_96dpi_bt_close_down.bmp");
			wImageListButtonMinimize120.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButtonMinimize120.ImageStream");
			wImageListButtonMinimize120.TransparentColor = System.Drawing.Color.Red;
			wImageListButtonMinimize120.Images.SetKeyName(0, "nfsw_lp_120dpi_bt_minimize_enabled.bmp");
			wImageListButtonMinimize120.Images.SetKeyName(1, "nfsw_lp_120dpi_bt_minimize_rollover.bmp");
			wImageListButtonMinimize120.Images.SetKeyName(2, "nfsw_lp_120dpi_bt_minimize_down.bmp");
			wImageListButtonMinimize144.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButtonMinimize144.ImageStream");
			wImageListButtonMinimize144.TransparentColor = System.Drawing.Color.Red;
			wImageListButtonMinimize144.Images.SetKeyName(0, "nfsw_lp_144dpi_bt_minimize_enabled.bmp");
			wImageListButtonMinimize144.Images.SetKeyName(1, "nfsw_lp_144dpi_bt_minimize_rollover.bmp");
			wImageListButtonMinimize144.Images.SetKeyName(2, "nfsw_lp_144dpi_bt_minimize_down.bmp");
			wImageListButtonClose120.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButtonClose120.ImageStream");
			wImageListButtonClose120.TransparentColor = System.Drawing.Color.Red;
			wImageListButtonClose120.Images.SetKeyName(0, "nfsw_lp_120dpi_bt_close_enabled.bmp");
			wImageListButtonClose120.Images.SetKeyName(1, "nfsw_lp_120dpi_bt_close_rollover.bmp");
			wImageListButtonClose120.Images.SetKeyName(2, "nfsw_lp_120dpi_bt_close_down.bmp");
			wImageListButtonClose144.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButtonClose144.ImageStream");
			wImageListButtonClose144.TransparentColor = System.Drawing.Color.Red;
			wImageListButtonClose144.Images.SetKeyName(0, "nfsw_lp_144dpi_bt_close_enabled.bmp");
			wImageListButtonClose144.Images.SetKeyName(1, "nfsw_lp_144dpi_bt_close_rollover.bmp");
			wImageListButtonClose144.Images.SetKeyName(2, "nfsw_lp_144dpi_bt_close_down.bmp");
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(wLabelButtonClose);
			base.Controls.Add(wLabelButtonMinimize);
			MinimumSize = new System.Drawing.Size(790, 490);
			base.Name = "BaseScreen";
			base.Size = new System.Drawing.Size(790, 490);
			base.MouseMove += new System.Windows.Forms.MouseEventHandler(BaseScreen_MouseMove);
			base.MouseDown += new System.Windows.Forms.MouseEventHandler(BaseScreen_MouseDown);
			base.MouseUp += new System.Windows.Forms.MouseEventHandler(BaseScreen_MouseUp);
			ResumeLayout(false);
		}
	}
}
