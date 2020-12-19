using log4net;
using log4net.Config;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class CustomMessageBox : Form
	{
		internal static class UnsafeNativeMethods
		{
			[DllImport("shell32.dll")]
			public static extern IntPtr ExtractAssociatedIconEx(IntPtr intPtr, string p, ref IntPtr intPtr_3, out IntPtr intPtr_4);

			[DllImport("user32.dll")]
			public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

			[DllImport("user32.dll")]
			public static extern int EnableMenuItem(IntPtr hMenu, int wIDEnableItem, int wEnable);
		}

		public enum CustomMessageBoxIcon
		{
			Exclamation = 1,
			Question,
			Error,
			Information
		}

		public enum CustomMessageBoxButtons
		{
			OK,
			YesNo
		}

		private struct LinkDetection
		{
			private LinkArea _area;

			private bool _linkDetected;

			public LinkArea Area => _area;

			public bool LinkDetected => _linkDetected;

			public LinkDetection(LinkArea area, bool linkDetected)
			{
				_area = area;
				_linkDetected = linkDetected;
			}
		}

		private const int SC_CLOSE = 61536;

		private const int MF_GRAYED = 1;

		private IContainer components;

		private Button wButton1;

		private PictureBox wPictureBoxIcon;

		private Button wButton2;

		private Panel panel1;

		private string _link = string.Empty;

		private CustomMessageBoxButtons _buttons;

		private int MinLabelY = 22;

		private int MaxLabelX = 358;

		private int MinDialogX = 123;

		private int MaxDialogX = 407;

		private int MinDialogY = 145;

		private int ButtonSeparation = 8;

		private int ButtonEdgeSeparation = 13;

		private int LabelAdjustX = 87;

		private int LabelAdjustY = 113;

		private LinkLabel wLinkLabelText;

		private static readonly ILog mLogger = LogManager.GetLogger(typeof(CustomMessageBox));

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
			wButton1 = new System.Windows.Forms.Button();
			wPictureBoxIcon = new System.Windows.Forms.PictureBox();
			wButton2 = new System.Windows.Forms.Button();
			panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)wPictureBoxIcon).BeginInit();
			panel1.SuspendLayout();
			SuspendLayout();
			wButton1.BackColor = System.Drawing.SystemColors.ButtonFace;
			wButton1.Location = new System.Drawing.Point(34, 10);
			wButton1.Name = "wButton1";
			wButton1.Size = new System.Drawing.Size(74, 22);
			wButton1.TabIndex = 0;
			wButton1.Text = "1";
			wButton1.UseVisualStyleBackColor = false;
			wButton1.Click += new System.EventHandler(wButton1_Click);
			wPictureBoxIcon.Location = new System.Drawing.Point(21, 22);
			wPictureBoxIcon.Name = "wPictureBoxIcon";
			wPictureBoxIcon.Size = new System.Drawing.Size(32, 32);
			wPictureBoxIcon.TabIndex = 1;
			wPictureBoxIcon.TabStop = false;
			wButton2.BackColor = System.Drawing.SystemColors.ButtonFace;
			wButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			wButton2.Location = new System.Drawing.Point(116, 10);
			wButton2.Name = "wButton2";
			wButton2.Size = new System.Drawing.Size(74, 22);
			wButton2.TabIndex = 2;
			wButton2.Text = "2";
			wButton2.UseVisualStyleBackColor = false;
			wButton2.Click += new System.EventHandler(wButton2_Click);
			panel1.BackColor = System.Drawing.SystemColors.Control;
			panel1.Controls.Add(wButton2);
			panel1.Controls.Add(wButton1);
			panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			panel1.Location = new System.Drawing.Point(0, 76);
			panel1.MinimumSize = new System.Drawing.Size(413, 42);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(413, 42);
			panel1.TabIndex = 3;
			base.AcceptButton = wButton1;
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			BackColor = System.Drawing.SystemColors.Window;
			base.CancelButton = wButton2;
			base.ClientSize = new System.Drawing.Size(405, 118);
			base.Controls.Add(wPictureBoxIcon);
			base.Controls.Add(panel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CustomMessageBox";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "CustomMessageBox";
			base.Load += new System.EventHandler(CustomMessageBox_Load);
			base.KeyDown += new System.Windows.Forms.KeyEventHandler(CustomMessageBox_KeyDown);
			((System.ComponentModel.ISupportInitialize)wPictureBoxIcon).EndInit();
			panel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		public CustomMessageBox(string text, string caption, CustomMessageBoxButtons buttons, CustomMessageBoxIcon icon)
		{
			BasicConfigurator.Configure();
			InitializeComponent();
			wLinkLabelText = new LinkLabel();
			wLinkLabelText.Font = SystemFonts.DialogFont;
			wLinkLabelText.Location = new Point(60, 33);
			wLinkLabelText.Name = "wLinkLabelText";
			wLinkLabelText.Size = new Size(88, 13);
			wLinkLabelText.TabIndex = 3;
			wLinkLabelText.TabStop = true;
			base.Controls.Add(wLinkLabelText);
			AdjustGraphicConstantsByDpi();
			Text = caption;
			wLinkLabelText.Text = text;
			wLinkLabelText.Font = SystemFonts.DialogFont;
			wPictureBoxIcon.Image = MapIcon(icon);
			_buttons = buttons;
			SetButtonsBehavior();
		}

		private void SetButtonsBehavior()
		{
			switch (_buttons)
			{
			case CustomMessageBoxButtons.OK:
				wButton1.Visible = false;
				wButton2.Text = "OK";
				base.AcceptButton = wButton2;
				base.CancelButton = null;
				break;
			case CustomMessageBoxButtons.YesNo:
				wButton1.Text = "Yes";
				wButton2.Text = "No";
				base.AcceptButton = wButton1;
				base.CancelButton = wButton2;
				break;
			}
		}

		private void AdjustGraphicConstantsByDpi()
		{
			using (Graphics graphics = CreateGraphics())
			{
				float num = graphics.DpiX / 96f;
				MinLabelY = (int)((float)MinLabelY * num);
				MaxLabelX = (int)((float)MaxLabelX * num);
				MinDialogX = (int)((float)MinDialogX * num);
				MaxDialogX = (int)((float)MaxDialogX * num);
				MinDialogY = (int)((float)MinDialogY * num);
				ButtonSeparation = (int)((float)ButtonSeparation * num);
				ButtonEdgeSeparation = (int)((float)ButtonEdgeSeparation * num);
				LabelAdjustX = (int)((float)LabelAdjustX * num);
				LabelAdjustY = (int)((float)LabelAdjustY * num);
			}
		}

		private Image MapIcon(CustomMessageBoxIcon index)
		{
			try
			{
				IntPtr intPtr_ = (IntPtr)(long)index;
				IntPtr intPtr_2;
				IntPtr handle = UnsafeNativeMethods.ExtractAssociatedIconEx(base.Handle, Path.Combine(Environment.SystemDirectory, "user32.dll"), ref intPtr_, out intPtr_2);
				return Icon.FromHandle(handle).ToBitmap();
			}
			catch (Exception ex)
			{
				mLogger.Error("MapIcon Exception: " + ex.ToString());
			}
			return null;
		}

		private void CustomMessageBox_Load(object sender, EventArgs e)
		{
			try
			{
				string text = wLinkLabelText.Text;
				LinkDetection linkDetection = DetectLinks(text);
				if (linkDetection.LinkDetected)
				{
					wLinkLabelText.LinkArea = linkDetection.Area;
					_link = text.Substring(linkDetection.Area.Start, linkDetection.Area.Length);
					wLinkLabelText.LinkClicked += wLinkLabelText_LinkClicked;
				}
				else
				{
					wLinkLabelText.LinkArea = new LinkArea(0, 0);
				}
				if (!string.IsNullOrEmpty(text))
				{
					using (Graphics graphics = CreateGraphics())
					{
						Font dialogFont = SystemFonts.DialogFont;
						RectangleF layoutRect = new RectangleF(0f, 0f, MaxLabelX, 1000f);
						CharacterRange[] measurableCharacterRanges = new CharacterRange[1]
						{
							new CharacterRange(0, text.Length)
						};
						StringFormat stringFormat = new StringFormat();
						stringFormat.FormatFlags = StringFormatFlags.NoClip;
						stringFormat.SetMeasurableCharacterRanges(measurableCharacterRanges);
						Region[] array = graphics.MeasureCharacterRanges(text, dialogFont, layoutRect, stringFormat);
						float height = array[0].GetBounds(graphics).Height;
						CharacterRange[] measurableCharacterRanges2 = new CharacterRange[1]
						{
							new CharacterRange(0, 1)
						};
						StringFormat stringFormat2 = new StringFormat();
						stringFormat2.FormatFlags = StringFormatFlags.NoClip;
						stringFormat2.SetMeasurableCharacterRanges(measurableCharacterRanges2);
						Region[] array2 = graphics.MeasureCharacterRanges("a", dialogFont, layoutRect, stringFormat2);
						float height2 = array2[0].GetBounds(graphics).Height;
						if (height <= height2)
						{
							wLinkLabelText.AutoSize = true;
						}
						else
						{
							wLinkLabelText.Size = new Size((int)array[0].GetBounds(graphics).Width, (int)height);
						}
						wLinkLabelText.Location = new Point(wLinkLabelText.Location.X, Math.Max(MinLabelY, wLinkLabelText.Location.Y - (int)((height / height2 - 1f) * wLinkLabelText.Font.Size)));
					}
				}
				int num = wLinkLabelText.Size.Width + LabelAdjustX;
				int num2 = wLinkLabelText.Size.Height + LabelAdjustY;
				base.Size = new Size((num < MinDialogX) ? MinDialogX : num, (num2 < MinDialogY) ? MinDialogY : num2);
				wButton2.Location = new Point(base.Width - ButtonEdgeSeparation - wButton2.Width, wButton2.Location.Y);
				if (wButton1.Visible)
				{
					wButton1.Location = new Point(base.Width - ButtonEdgeSeparation - ButtonSeparation - wButton1.Width - wButton2.Width, wButton1.Location.Y);
					UnsafeNativeMethods.EnableMenuItem(UnsafeNativeMethods.GetSystemMenu(base.Handle, bRevert: false), 61536, 1);
				}
			}
			catch (Exception ex)
			{
				mLogger.Error("CustomMessageBox_Load Exception: " + ex.ToString());
			}
		}

		private void wLinkLabelText_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string[] array = null;
			string text = string.Empty;
			try
			{
				array = RetrieveWebApplicationData();
				if (!string.IsNullOrEmpty(array[0]))
				{
					text = array[1].Replace("%1", _link);
					Process.Start(array[0], text);
				}
			}
			catch (Exception ex)
			{
				mLogger.ErrorFormat("Problem running {0} {1}", array[0], text);
				mLogger.Error("wLinkLabelText_LinkClicked Exception: " + ex.ToString());
			}
		}

		private string[] RetrieveWebApplicationData()
		{
			string[] array = new string[2];
			string arg = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\http\\UserChoice", "Progid", string.Empty);
			string text = (string)Registry.GetValue($"HKEY_CURRENT_USER\\Software\\Classes\\{arg}\\shell\\open\\command", "", string.Empty);
			if (string.IsNullOrEmpty(text))
			{
				text = (string)Registry.GetValue($"HKEY_CLASSES_ROOT\\{arg}\\shell\\open\\command", "", string.Empty);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Classes\\http\\shell\\open\\command", "", string.Empty);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\http\\shell\\open\\command", "", string.Empty);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Classes\\Applications\\iexplore.exe\\shell\\open\\command", "", string.Empty);
			}
			if (string.IsNullOrEmpty(text))
			{
				string environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
				if (string.IsNullOrEmpty(environmentVariable))
				{
					environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles");
				}
				string text2 = Path.Combine(environmentVariable, "Internet Explorer\\IEXPLORE.EXE");
				if (!File.Exists(text2))
				{
					return array;
				}
				array[0] = text2;
			}
			string[] array2 = text.Split('"');
			if (array2.Length > 1)
			{
				array[0] = array2[1];
			}
			if (array2.Length > 2)
			{
				array[1] = string.Join("\"", array2, 2, array2.Length - 2);
			}
			if (!array[1].Contains("%1"))
			{
				array[1] += " \"%1\"";
			}
			return array;
		}

		private void wButton1_Click(object sender, EventArgs e)
		{
			switch (_buttons)
			{
			case CustomMessageBoxButtons.OK:
				throw new InvalidOperationException("The user clicked the button 1 and it should not be available");
			case CustomMessageBoxButtons.YesNo:
				base.DialogResult = DialogResult.Yes;
				Close();
				break;
			default:
				throw new NotImplementedException();
			}
		}

		private void wButton2_Click(object sender, EventArgs e)
		{
			switch (_buttons)
			{
			case CustomMessageBoxButtons.OK:
				base.DialogResult = DialogResult.OK;
				break;
			case CustomMessageBoxButtons.YesNo:
				base.DialogResult = DialogResult.No;
				break;
			default:
				throw new NotImplementedException();
			}
			Close();
		}

		private LinkDetection DetectLinks(string text)
		{
			LinkArea area = new LinkArea(0, text.Length);
			bool linkDetected = false;
			Regex regex = new Regex("(https?://\\S+[\\w\\d/])");
			string empty = string.Empty;
			int num = text.IndexOf("http://");
			if (num != -1)
			{
				empty = text.Substring(num);
				Match match = regex.Match(empty);
				if (match.Success)
				{
					area.Start = num;
					area.Length = match.Groups[0].Length;
					linkDetected = true;
				}
			}
			else
			{
				num = text.IndexOf("https://");
				if (num != -1)
				{
					empty = text.Substring(num);
					Match match2 = regex.Match(empty);
					if (match2.Success)
					{
						area.Start = num;
						area.Length = match2.Groups[0].Length;
						linkDetected = true;
					}
				}
			}
			return new LinkDetection(area, linkDetected);
		}

		private void CustomMessageBox_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.Control && e.KeyCode == Keys.C) || (e.Control && e.KeyCode == Keys.Insert))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("---------------------------");
				stringBuilder.AppendLine(Text);
				stringBuilder.AppendLine("---------------------------");
				stringBuilder.AppendLine(wLinkLabelText.Text);
				stringBuilder.AppendLine("---------------------------");
				if (wButton1.Visible)
				{
					stringBuilder.Append(wButton1.Text + " ");
				}
				stringBuilder.AppendLine(wButton2.Text);
				stringBuilder.AppendLine("---------------------------");
				Clipboard.SetDataObject(stringBuilder.ToString());
			}
		}
	}
}
