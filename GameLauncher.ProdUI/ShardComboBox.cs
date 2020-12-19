using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class ShardComboBox : UserControl
	{
		public ShardUrlChanged ShardUrlChanged;

		public ShardRegionChanged ShardRegionChanged;

		private IContainer components;

		private ComboBox wComboBoxShard;

		public ShardComboBox()
		{
			InitializeComponent();
			FontFamily fontFamily = FontWrapper.Instance.GetFontFamily("MyriadProSemiCondBold.ttf");
			wComboBoxShard.Font = new Font(fontFamily, 9.749999f, FontStyle.Bold);
			FillShardCombo();
		}

		private void FillShardCombo()
		{
			if (ShardManager.Shards.Values.Count == 0)
			{
				return;
			}
			foreach (string key in ShardManager.Shards.Keys)
			{
				wComboBoxShard.Items.Add(key);
			}
			SetSelectedValue();
			wComboBoxShard.SelectedValueChanged += wComboBoxShard_SelectedValueChanged;
		}

		public void SetSelectedValue(string shardKey)
		{
			if (ShardManager.Shards.ContainsKey(shardKey))
			{
				ShardManager.ShardKey = shardKey;
				wComboBoxShard.SelectedItem = shardKey;
			}
			else
			{
				SetSelectedValue();
			}
		}

		public void SetSelectedValue()
		{
			ShardInfo value = null;
			ShardManager.Shards.TryGetValue(ShardManager.ShardKey, out value);
			string selectedItem = (value != null) ? value.ShardKey : (ShardManager.ShardKey = (string)wComboBoxShard.Items[0]);
			wComboBoxShard.SelectedItem = selectedItem;
		}

		private void wComboBoxShard_SelectedValueChanged(object sender, EventArgs e)
		{
			string text = (string)wComboBoxShard.SelectedItem;
			ShardInfo shardInfo = ShardManager.Shards[text];
			ShardInfo shardInfo2 = ShardManager.Shards[ShardManager.ShardKey];
			ShardManager.ShardKey = text;
			if (!string.Equals(shardInfo.Url, shardInfo2.Url))
			{
				BeginInvoke(ShardUrlChanged);
			}
			else if (shardInfo.RegionId != shardInfo2.RegionId)
			{
				BeginInvoke(ShardRegionChanged);
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
			wComboBoxShard = new System.Windows.Forms.ComboBox();
			SuspendLayout();
			wComboBoxShard.BackColor = System.Drawing.Color.White;
			wComboBoxShard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			wComboBoxShard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			wComboBoxShard.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
			wComboBoxShard.FormattingEnabled = true;
			wComboBoxShard.Location = new System.Drawing.Point(0, 0);
			wComboBoxShard.Name = "wComboBoxShard";
			wComboBoxShard.Size = new System.Drawing.Size(185, 24);
			wComboBoxShard.TabIndex = 40;
			base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			AutoSize = true;
			base.Controls.Add(wComboBoxShard);
			base.Name = "ShardComboBox";
			base.Size = new System.Drawing.Size(188, 27);
			ResumeLayout(false);
		}
	}
}
