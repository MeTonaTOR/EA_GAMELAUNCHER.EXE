using GameLauncher.ProdUI.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class Tos : Form
	{
		private IContainer components;

		private TextBox wTextBoxTOS;

		private Label wLabelTOS;

		private ImageList wImageListButton96;

		private ImageList wImageListButton120;

		private ImageList wImageListButton144;

		private Label wLabelButtonAccept;

		private Label wLabelButtonDecline;

		private bool mDragging;

		private Point mDraggingStart;

		private Point mDraggingCursorStart;

		private bool mLabelButtonDeclineHOver;

		private bool mLabelButtonAcceptHOver;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameLauncher.ProdUI.Tos));
			wTextBoxTOS = new System.Windows.Forms.TextBox();
			wLabelTOS = new System.Windows.Forms.Label();
			wImageListButton96 = new System.Windows.Forms.ImageList(components);
			wImageListButton120 = new System.Windows.Forms.ImageList(components);
			wImageListButton144 = new System.Windows.Forms.ImageList(components);
			wLabelButtonAccept = new System.Windows.Forms.Label();
			wLabelButtonDecline = new System.Windows.Forms.Label();
			SuspendLayout();
			wTextBoxTOS.BackColor = System.Drawing.Color.Black;
			wTextBoxTOS.Font = new System.Drawing.Font("MyriadProSemiCondBold", 8.999999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wTextBoxTOS.ForeColor = System.Drawing.Color.White;
			wTextBoxTOS.Location = new System.Drawing.Point(19, 31);
			wTextBoxTOS.Multiline = true;
			wTextBoxTOS.Name = "wTextBoxTOS";
			wTextBoxTOS.ReadOnly = true;
			wTextBoxTOS.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			wTextBoxTOS.Size = new System.Drawing.Size(481, 353);
			wTextBoxTOS.TabIndex = 3;
			wLabelTOS.BackColor = System.Drawing.Color.Transparent;
			wLabelTOS.Font = new System.Drawing.Font("Reg-B-I", 12f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			wLabelTOS.ForeColor = System.Drawing.Color.FromArgb(76, 178, 255);
			wLabelTOS.Location = new System.Drawing.Point(18, 6);
			wLabelTOS.Name = "wLabelTOS";
			wLabelTOS.Size = new System.Drawing.Size(481, 21);
			wLabelTOS.TabIndex = 2;
			wLabelTOS.Text = "NFS WORLD TERMS OF SERVICE";
			wLabelTOS.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			wLabelTOS.MouseMove += new System.Windows.Forms.MouseEventHandler(Tos_MouseMove);
			wLabelTOS.MouseDown += new System.Windows.Forms.MouseEventHandler(Tos_MouseDown);
			wLabelTOS.MouseUp += new System.Windows.Forms.MouseEventHandler(Tos_MouseUp);
			wImageListButton96.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButton96.ImageStream");
			wImageListButton96.TransparentColor = System.Drawing.Color.Red;
			wImageListButton96.Images.SetKeyName(0, "nfsw_lp_96dpi_bg_s2_bt_logout_enabled.bmp");
			wImageListButton96.Images.SetKeyName(1, "nfsw_lp_96dpi_bg_s2_bt_logout_rollover.bmp");
			wImageListButton96.Images.SetKeyName(2, "nfsw_lp_96dpi_bg_s2_bt_logout_down.bmp");
			wImageListButton120.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButton120.ImageStream");
			wImageListButton120.TransparentColor = System.Drawing.Color.Red;
			wImageListButton120.Images.SetKeyName(0, "nfsw_lp_120dpi_bg_s2_bt_logout_enabled.bmp");
			wImageListButton120.Images.SetKeyName(1, "nfsw_lp_120dpi_bg_s2_bt_logout_rollover.bmp");
			wImageListButton120.Images.SetKeyName(2, "nfsw_lp_120dpi_bg_s2_bt_logout_down.bmp");
			wImageListButton144.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("wImageListButton144.ImageStream");
			wImageListButton144.TransparentColor = System.Drawing.Color.Red;
			wImageListButton144.Images.SetKeyName(0, "nfsw_lp_144dpi_bg_s2_bt_logout_enabled.bmp");
			wImageListButton144.Images.SetKeyName(1, "nfsw_lp_144dpi_bg_s2_bt_logout_rollover.bmp");
			wImageListButton144.Images.SetKeyName(2, "nfsw_lp_144dpi_bg_s2_bt_logout_down.bmp");
			wLabelButtonAccept.BackColor = System.Drawing.Color.Transparent;
			wLabelButtonAccept.Font = new System.Drawing.Font("Reg-DB-I", 14.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			wLabelButtonAccept.ForeColor = System.Drawing.Color.White;
			wLabelButtonAccept.ImageIndex = 0;
			wLabelButtonAccept.ImageList = wImageListButton96;
			wLabelButtonAccept.Location = new System.Drawing.Point(374, 394);
			wLabelButtonAccept.Name = "wLabelButtonAccept";
			wLabelButtonAccept.Size = new System.Drawing.Size(131, 31);
			wLabelButtonAccept.TabIndex = 4;
			wLabelButtonAccept.Text = "I ACCEPT";
			wLabelButtonAccept.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			wLabelButtonAccept.MouseLeave += new System.EventHandler(wLabelButtonAccept_MouseLeave);
			wLabelButtonAccept.Click += new System.EventHandler(wLabelButtonAccept_Click);
			wLabelButtonAccept.MouseDown += new System.Windows.Forms.MouseEventHandler(wLabelButtonAccept_MouseDown);
			wLabelButtonAccept.MouseUp += new System.Windows.Forms.MouseEventHandler(wLabelButtonAccept_MouseUp);
			wLabelButtonAccept.MouseEnter += new System.EventHandler(wLabelButtonAccept_MouseEnter);
			wLabelButtonDecline.BackColor = System.Drawing.Color.Transparent;
			wLabelButtonDecline.Font = new System.Drawing.Font("Reg-DB-I", 14.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			wLabelButtonDecline.ForeColor = System.Drawing.Color.White;
			wLabelButtonDecline.ImageIndex = 0;
			wLabelButtonDecline.ImageList = wImageListButton96;
			wLabelButtonDecline.Location = new System.Drawing.Point(18, 394);
			wLabelButtonDecline.Name = "wLabelButtonDecline";
			wLabelButtonDecline.Size = new System.Drawing.Size(131, 31);
			wLabelButtonDecline.TabIndex = 5;
			wLabelButtonDecline.Text = "I DECLINE";
			wLabelButtonDecline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			wLabelButtonDecline.MouseLeave += new System.EventHandler(wLabelButtonDecline_MouseLeave);
			wLabelButtonDecline.Click += new System.EventHandler(wLabelButtonDecline_Click);
			wLabelButtonDecline.MouseDown += new System.Windows.Forms.MouseEventHandler(wLabelButtonDecline_MouseDown);
			wLabelButtonDecline.MouseUp += new System.Windows.Forms.MouseEventHandler(wLabelButtonDecline_MouseUp);
			wLabelButtonDecline.MouseEnter += new System.EventHandler(wLabelButtonDecline_MouseEnter);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Black;
			BackgroundImage = GameLauncher.ProdUI.Properties.Resources.nfsw_lp_tos_96dpi_bg;
			base.ClientSize = new System.Drawing.Size(517, 438);
			base.Controls.Add(wLabelButtonDecline);
			base.Controls.Add(wLabelButtonAccept);
			base.Controls.Add(wLabelTOS);
			base.Controls.Add(wTextBoxTOS);
			ForeColor = System.Drawing.Color.FromArgb(51, 153, 255);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(517, 438);
			base.Name = "Tos";
			base.ShowInTaskbar = false;
			Text = "Terms of Service";
			base.TopMost = true;
			base.TransparencyKey = System.Drawing.Color.Red;
			base.MouseUp += new System.Windows.Forms.MouseEventHandler(Tos_MouseUp);
			base.MouseDown += new System.Windows.Forms.MouseEventHandler(Tos_MouseDown);
			base.MouseMove += new System.Windows.Forms.MouseEventHandler(Tos_MouseMove);
			ResumeLayout(false);
			PerformLayout();
		}

		public Tos(string tos)
		{
			InitializeComponent();
			wTextBoxTOS.Text = tos.Replace("\n", Environment.NewLine);
			wTextBoxTOS.Select(wTextBoxTOS.Text.Length, 0);
			ApplyEmbeddedFonts();
			ApplyLocalization();
		}

		private void ApplyEmbeddedFonts()
		{
			FontFamily fontFamily = FontWrapper.Instance.GetFontFamily("MyriadProSemiCondBold.ttf");
			FontFamily fontFamily2 = FontWrapper.Instance.GetFontFamily("Reg-B-I.ttf");
			FontFamily fontFamily3 = FontWrapper.Instance.GetFontFamily("Reg-DB-I.ttf");
			wLabelTOS.Font = new Font(fontFamily2, 12f, FontStyle.Bold | FontStyle.Italic);
			wTextBoxTOS.Font = new Font(fontFamily, 9f, FontStyle.Bold);
			wLabelButtonAccept.Font = new Font(fontFamily3, 14f, FontStyle.Italic);
			wLabelButtonDecline.Font = new Font(fontFamily3, 14f, FontStyle.Italic);
		}

		private void ApplyLocalization()
		{
			wLabelTOS.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "TOS00001");
			wLabelButtonDecline.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "TOS00003");
			wLabelButtonAccept.Text = ResourceWrapper.Instance.GetString("GameLauncher.ProdUI.LanguageStrings", "TOS00004");
		}

		private void Tos_MouseDown(object sender, MouseEventArgs e)
		{
			mDragging = true;
			mDraggingStart = base.Location;
			mDraggingCursorStart = Cursor.Position;
		}

		private void Tos_MouseMove(object sender, MouseEventArgs e)
		{
			if (mDragging)
			{
				base.Location = new Point(mDraggingStart.X + Cursor.Position.X - mDraggingCursorStart.X, mDraggingStart.Y + Cursor.Position.Y - mDraggingCursorStart.Y);
			}
		}

		private void Tos_MouseUp(object sender, MouseEventArgs e)
		{
			mDragging = false;
		}

		private void wLabelButtonDecline_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			Close();
		}

		private void wLabelButtonDecline_MouseDown(object sender, MouseEventArgs e)
		{
			wLabelButtonDecline.ImageIndex = 2;
		}

		private void wLabelButtonDecline_MouseEnter(object sender, EventArgs e)
		{
			if (!mLabelButtonDeclineHOver)
			{
				wLabelButtonDecline.ImageIndex = 1;
				mLabelButtonDeclineHOver = true;
			}
		}

		private void wLabelButtonDecline_MouseLeave(object sender, EventArgs e)
		{
			if (mLabelButtonDeclineHOver)
			{
				wLabelButtonDecline.ImageIndex = 0;
				mLabelButtonDeclineHOver = false;
			}
		}

		private void wLabelButtonDecline_MouseUp(object sender, MouseEventArgs e)
		{
			wLabelButtonDecline.ImageIndex = 1;
		}

		private void wLabelButtonAccept_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			Close();
		}

		private void wLabelButtonAccept_MouseDown(object sender, MouseEventArgs e)
		{
			wLabelButtonAccept.ImageIndex = 2;
		}

		private void wLabelButtonAccept_MouseEnter(object sender, EventArgs e)
		{
			if (!mLabelButtonAcceptHOver)
			{
				wLabelButtonAccept.ImageIndex = 1;
				mLabelButtonAcceptHOver = true;
			}
		}

		private void wLabelButtonAccept_MouseLeave(object sender, EventArgs e)
		{
			if (mLabelButtonAcceptHOver)
			{
				wLabelButtonAccept.ImageIndex = 0;
				mLabelButtonAcceptHOver = false;
			}
		}

		private void wLabelButtonAccept_MouseUp(object sender, MouseEventArgs e)
		{
			wLabelButtonAccept.ImageIndex = 1;
		}
	}
}
