using dotNetLab.Common;
using dotNetLab.Common.Normal;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace shikii.VisionJob
{
	public class MakeupForm : PageBase
	{
		private Panel panel1;

		private Label label3;

		private Label label2;

		private ComboBox cmbx_BorderNum;

		private Label label4;

		private Label lbl_ProjectName;

		private Label label6;

		private Label label7;

		private Label label8;

		private NumericUpDown numTxb_X;

		private NumericUpDown numTxb_Y;

		private NumericUpDown numTxb_A;

		private Label label1;

		private Label label5;

		private string CurrentMakeupTableName;

		public decimal DeltaX
		{
			get
			{
				try
				{
					string s = base.CompactDB.FetchValue(string.Format("DeltaX{0}", cmbx_BorderNum.SelectedIndex), CurrentMakeupTableName, "0", true);
					return decimal.Parse(s);
				}
				catch (Exception)
				{
					return decimal.Zero;
				}
			}
			set
			{
				base.CompactDB.Write(CurrentMakeupTableName, string.Format("DeltaX{0}", cmbx_BorderNum.SelectedIndex), Math.Round(value, 2).ToString());
			}
		}

		public decimal DeltaY
		{
			get
			{
				try
				{
					string s = base.CompactDB.FetchValue(string.Format("DeltaY{0}", cmbx_BorderNum.SelectedIndex), CurrentMakeupTableName, "0", true);
					return decimal.Parse(s);
				}
				catch (Exception)
				{
					return decimal.Zero;
				}
			}
			set
			{
				base.CompactDB.Write(CurrentMakeupTableName, string.Format("DeltaY{0}", cmbx_BorderNum.SelectedIndex), Math.Round(value, 2).ToString());
			}
		}

		public decimal DeltaA
		{
			get
			{
				try
				{
					string s = base.CompactDB.FetchValue(string.Format("DeltaA{0}", cmbx_BorderNum.SelectedIndex), CurrentMakeupTableName, "0", true);
					return decimal.Parse(s);
				}
				catch (Exception)
				{
					return decimal.Zero;
				}
			}
			set
			{
				base.CompactDB.Write(CurrentMakeupTableName, string.Format("DeltaA{0}", cmbx_BorderNum.SelectedIndex), Math.Round(value, 2).ToString());
			}
		}

		protected override void prepareCtrls()
		{
			base.prepareCtrls();
			InitializeComponent();
			ComboBox.ObjectCollection items = cmbx_BorderNum.Items;
			object[] items2 = new string[4]
			{
				"0",
				"1",
				"2",
				"3"
			};
			items.AddRange(items2);
			cmbx_BorderNum.SetIndexByString(App.nCurrentBorder.ToString());
			lbl_ProjectName.Text = "当前项目：" + App.GetShortProjectName();
			CurrentMakeupTableName = string.Format("X{0}_Makeup", App.GetShortProjectName());
			base.StartPosition = FormStartPosition.CenterScreen;
			base.MaximizeBox = false;
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			Text = "补偿值";
			AddBindings();
		}

		private void AddBindings()
		{
			try
			{
				numTxb_A.AddBinding("Value", this, "DeltaA", false, DataSourceUpdateMode.OnPropertyChanged);
				numTxb_X.AddBinding("Value", this, "DeltaX", false, DataSourceUpdateMode.OnPropertyChanged);
				numTxb_Y.AddBinding("Value", this, "DeltaY", false, DataSourceUpdateMode.OnPropertyChanged);
			}
			catch (Exception)
			{
			}
		}

		private void RemoveBindings()
		{
			numTxb_A.DataBindings.Clear();
			numTxb_Y.DataBindings.Clear();
			numTxb_X.DataBindings.Clear();
		}

		private void InitializeComponent()
		{
			panel1 = new System.Windows.Forms.Panel();
			label3 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			cmbx_BorderNum = new System.Windows.Forms.ComboBox();
			label4 = new System.Windows.Forms.Label();
			lbl_ProjectName = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			label7 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			numTxb_X = new System.Windows.Forms.NumericUpDown();
			numTxb_Y = new System.Windows.Forms.NumericUpDown();
			numTxb_A = new System.Windows.Forms.NumericUpDown();
			label5 = new System.Windows.Forms.Label();
			panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numTxb_X).BeginInit();
			((System.ComponentModel.ISupportInitialize)numTxb_Y).BeginInit();
			((System.ComponentModel.ISupportInitialize)numTxb_A).BeginInit();
			SuspendLayout();
			panel1.BackColor = System.Drawing.Color.Purple;
			panel1.Controls.Add(label3);
			panel1.Controls.Add(label2);
			panel1.Controls.Add(label1);
			panel1.Dock = System.Windows.Forms.DockStyle.Top;
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(463, 77);
			panel1.TabIndex = 6;
			label3.AutoSize = true;
			label3.Font = new System.Drawing.Font("MV Boli", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label3.ForeColor = System.Drawing.Color.White;
			label3.Location = new System.Drawing.Point(486, 21);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(145, 31);
			label3.TabIndex = 5;
			label3.Text = "Query Data";
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("等线 Light", 25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label2.ForeColor = System.Drawing.Color.White;
			label2.Location = new System.Drawing.Point(279, 21);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(172, 36);
			label2.TabIndex = 0;
			label2.Text = "Delta X,Y,A";
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("等线 Light", 25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label1.ForeColor = System.Drawing.Color.White;
			label1.Location = new System.Drawing.Point(19, 21);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(117, 36);
			label1.TabIndex = 0;
			label1.Text = "补偿值";
			cmbx_BorderNum.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			cmbx_BorderNum.FormattingEnabled = true;
			cmbx_BorderNum.Location = new System.Drawing.Point(321, 97);
			cmbx_BorderNum.Name = "cmbx_BorderNum";
			cmbx_BorderNum.Size = new System.Drawing.Size(121, 27);
			cmbx_BorderNum.TabIndex = 7;
			cmbx_BorderNum.SelectedIndexChanged += new System.EventHandler(cmbx_BorderNum_SelectedIndexChanged);
			label4.AutoSize = true;
			label4.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label4.Location = new System.Drawing.Point(264, 100);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(51, 20);
			label4.TabIndex = 8;
			label4.Text = "边序号";
			lbl_ProjectName.AutoSize = true;
			lbl_ProjectName.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			lbl_ProjectName.Location = new System.Drawing.Point(12, 100);
			lbl_ProjectName.Name = "lbl_ProjectName";
			lbl_ProjectName.Size = new System.Drawing.Size(79, 20);
			lbl_ProjectName.TabIndex = 9;
			lbl_ProjectName.Text = "当前项目名";
			label6.AutoSize = true;
			label6.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label6.Location = new System.Drawing.Point(123, 205);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(18, 20);
			label6.TabIndex = 10;
			label6.Text = "X";
			label7.AutoSize = true;
			label7.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label7.Location = new System.Drawing.Point(123, 257);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(17, 20);
			label7.TabIndex = 10;
			label7.Text = "Y";
			label8.AutoSize = true;
			label8.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label8.Location = new System.Drawing.Point(123, 306);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(19, 20);
			label8.TabIndex = 10;
			label8.Text = "A";
			numTxb_X.DecimalPlaces = 2;
			numTxb_X.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			numTxb_X.Location = new System.Drawing.Point(167, 205);
			numTxb_X.Maximum = new decimal(new int[4]
			{
				1000000,
				0,
				0,
				0
			});
			numTxb_X.Minimum = new decimal(new int[4]
			{
				99999999,
				0,
				0,
				-2147483648
			});
			numTxb_X.Name = "numTxb_X";
			numTxb_X.Size = new System.Drawing.Size(148, 25);
			numTxb_X.TabIndex = 11;
			numTxb_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			numTxb_Y.DecimalPlaces = 2;
			numTxb_Y.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			numTxb_Y.Location = new System.Drawing.Point(167, 259);
			numTxb_Y.Maximum = new decimal(new int[4]
			{
				1000000,
				0,
				0,
				0
			});
			numTxb_Y.Minimum = new decimal(new int[4]
			{
				99999999,
				0,
				0,
				-2147483648
			});
			numTxb_Y.Name = "numTxb_Y";
			numTxb_Y.Size = new System.Drawing.Size(148, 25);
			numTxb_Y.TabIndex = 11;
			numTxb_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			numTxb_A.DecimalPlaces = 2;
			numTxb_A.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			numTxb_A.Location = new System.Drawing.Point(167, 306);
			numTxb_A.Maximum = new decimal(new int[4]
			{
				1000000,
				0,
				0,
				0
			});
			numTxb_A.Minimum = new decimal(new int[4]
			{
				99999999,
				0,
				0,
				-2147483648
			});
			numTxb_A.Name = "numTxb_A";
			numTxb_A.Size = new System.Drawing.Size(148, 25);
			numTxb_A.TabIndex = 11;
			numTxb_A.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			label5.AutoSize = true;
			label5.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label5.ForeColor = System.Drawing.Color.DarkGreen;
			label5.Location = new System.Drawing.Point(37, 154);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(405, 20);
			label5.TabIndex = 12;
			label5.Text = "请注意每条边对应着三个补偿值(X,Y,A),请注意选择正确的边序号";
			BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(463, 390);
			base.Controls.Add(label5);
			base.Controls.Add(numTxb_A);
			base.Controls.Add(numTxb_Y);
			base.Controls.Add(numTxb_X);
			base.Controls.Add(label8);
			base.Controls.Add(label7);
			base.Controls.Add(label6);
			base.Controls.Add(lbl_ProjectName);
			base.Controls.Add(label4);
			base.Controls.Add(cmbx_BorderNum);
			base.Controls.Add(panel1);
			base.Name = "MakeupForm";
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)numTxb_X).EndInit();
			((System.ComponentModel.ISupportInitialize)numTxb_Y).EndInit();
			((System.ComponentModel.ISupportInitialize)numTxb_A).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		private void cmbx_BorderNum_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				RemoveBindings();
				AddBindings();
			}
			catch (Exception)
			{
			}
		}
	}
}
