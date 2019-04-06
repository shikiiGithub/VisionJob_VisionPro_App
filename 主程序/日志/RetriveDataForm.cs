using dotNetLab;
using dotNetLab.Common;
using dotNetLab.Data;
using dotNetLab.Data.Uniting;
using dotNetLab.Widgets;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace shikii.VisionJob
{
	public class RetriveDataForm : Form
	{
		public UnitDB ThisDB;

		private IContainer components = null;

		private TextBlock textBlock1;

		private DateTimePicker dateTimePicker1;

		private TextBlock textBlock2;

		private TextBlock textBlock3;

		private MobileButton btn_Search;

		private DataGridView dataGridView1;

		private LinkLabel linkLabel1;

		private DateTimePicker dateTimePicker2;

		private Panel panel1;

		private Label label3;

		private Label label1;

		private RadioButton rbtn_Log;

		private RadioButton rbtn_NGReporting;

		private RadioButton rbtn_NGDetails;

		public RetriveDataForm()
		{
			InitializeComponent();
			ThisDB = R.LogDB;
		}

		protected virtual void btn_Search_Click(object sender, EventArgs e)
		{
			ThisDB = R.CompactDB;
			if (rbtn_Log.Checked)
			{
				QueryLogs();
			}
			if (rbtn_NGReporting.Checked)
			{
				QueryOKNGPercent();
			}
			if (rbtn_NGDetails.Checked)
			{
				QueryNGStatus();
			}
		}

		private void QueryLogs()
		{
			ThisDB = R.LogDB;
			string text = null;
			bool flag = false;
			if (dateTimePicker1.Text.Equals(dateTimePicker2.Text))
			{
				flag = true;
			}
			DateTime value;
			if (dateTimePicker2.Value < dateTimePicker2.Value)
			{
				dotNetLab.Tipper.Error = "第二个日期控件的日期必须大于或者等于前一个日期控件的日期。";
			}
			else if (flag)
			{
				value = dateTimePicker1.Value;
				string arg = value.ToString("yyyy-MM-dd");
				value = dateTimePicker1.Value;
				object arg2 = value.Year;
				value = dateTimePicker1.Value;
				text = string.Format("_{0}_{1}", arg2, value.Month);
				dataGridView1.DataSource = ThisDB.ProvideTable(string.Format("SELECT Fire_Time as 触发时间,Message as 基本信息 FROM {0} where Fire_Time like '{1}%';", text, arg), dotNetLab.Data.DBOperator.OPERATOR_QUERY_TABLE);
			}
			else
			{
				value = dateTimePicker1.Value;
				string arg3 = value.ToString("yyyy-MM-dd");
				value = dateTimePicker2.Value;
				string arg4 = value.AddDays(1.0).ToString("yyyy-MM-dd");
				ThisDB.GetAllTableNames();
				DataTable dataTable = new DataTable();
				dataTable.Columns.Add();
				dataTable.Columns[0].ColumnName = "触发时间";
				dataTable.Columns.Add();
				dataTable.Columns[1].ColumnName = "基本信息";
				int num = 0;
				for (int i = 0; i < ThisDB.AllTableNames.Count; i++)
				{
					if (!ThisDB.DefaultTable.Equals(ThisDB.AllTableNames[i]))
					{
						text = ThisDB.AllTableNames[i];
						DataTable dataTable2 = ThisDB.ProvideTable(string.Format("SELECT Fire_Time as 触发时间,Message  as 基本信息 FROM {0} where Fire_Time >='{1}' and Fire_Time <'{2}';", text, arg3, arg4), dotNetLab.Data.DBOperator.OPERATOR_QUERY_TABLE);
						if (dataTable2 != null && dataTable2.Rows.Count > 0)
						{
							for (int j = 0; j < dataTable2.Rows.Count; j++)
							{
								dataTable.Rows.Add();
								for (int k = 0; k < dataTable2.Columns.Count; k++)
								{
									dataTable.Rows[num][k] = dataTable2.Rows[j][k];
								}
								num++;
							}
						}
					}
				}
				dataGridView1.DataSource = dataTable;
			}
		}

		protected virtual void QueryNGStatus()
		{
			string imageRecord = App.ImageRecord;
			bool flag = false;
			if (dateTimePicker1.Text.Equals(dateTimePicker2.Text))
			{
				flag = true;
			}
			DateTime value;
			if (dateTimePicker2.Value < dateTimePicker2.Value)
			{
				dotNetLab.Tipper.Error = "第二个日期控件的日期必须大于或者等于前一个日期控件的日期。";
			}
			else if (flag)
			{
				value = dateTimePicker1.Value;
				string arg = value.ToString("yyyy-MM-dd");
				imageRecord = App.ImageRecord;
				dataGridView1.DataSource = ThisDB.ProvideTable(string.Format("SELECT * FROM {0} where 图像生成日期 like '{1}%' ;", imageRecord, arg), dotNetLab.Data.DBOperator.OPERATOR_QUERY_TABLE);
			}
			else
			{
				value = dateTimePicker1.Value;
				string arg2 = value.ToString("yyyy-MM-dd");
				value = dateTimePicker2.Value;
				string arg3 = value.AddDays(1.0).ToString("yyyy-MM-dd");
				string sql = string.Format("SELECT * FROM {0} where 图像生成日期 >='{1}' and 图像生成日期 <'{2}'", imageRecord, arg2, arg3);
				DataTable dataSource = ThisDB.ProvideTable(sql, dotNetLab.Data.DBOperator.OPERATOR_QUERY_TABLE);
				dataGridView1.DataSource = dataSource;
			}
		}

		protected virtual void QueryOKNGPercent()
		{
			string imageRecord = App.ImageRecord;
			bool flag = false;
			if (dateTimePicker1.Text.Equals(dateTimePicker2.Text))
			{
				flag = true;
			}
			DateTime value;
			if (dateTimePicker2.Value < dateTimePicker2.Value)
			{
				dotNetLab.Tipper.Error = "第二个日期控件的日期必须大于或者等于前一个日期控件的日期。";
			}
			else if (flag)
			{
				value = dateTimePicker1.Value;
				string arg = value.ToString("yyyy-MM-dd");
				imageRecord = App.ImageRecord;
				dataGridView1.DataSource = ThisDB.ProvideTable(string.Format("select {0}.Name as 日期,{0}.Val as 生产总量,{1}.Val as OKNG率 from {0},{1} where {0}.Name like '{2}%' and {0}.Name={1}.Name;", App.ManufatureTotalNumRecordTable_PerDay, App.StatisticNGOKPercentTable_PerDay, arg), dotNetLab.Data.DBOperator.OPERATOR_QUERY_TABLE);
			}
			else
			{
				value = dateTimePicker1.Value;
				string text = value.ToString("yyyy-MM-dd");
				value = dateTimePicker2.Value;
				string text2 = value.AddDays(1.0).ToString("yyyy-MM-dd");
				string sql = string.Format("select {0}.Name as 日期,{0}.Val as 生产总量,{1}.Val as OKNG率 from {0},{1} where {0}.Name > {2} and {0}.Name<{3} and {0}.Name={1}.Name;", App.ManufatureTotalNumRecordTable_PerDay, App.StatisticNGOKPercentTable_PerDay, text, text2);
				DataTable dataSource = ThisDB.ProvideTable(sql, dotNetLab.Data.DBOperator.OPERATOR_QUERY_TABLE);
				dataGridView1.DataSource = dataSource;
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			PortableOffice.ExportToExcel(dataGridView1);
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
			dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			dataGridView1 = new System.Windows.Forms.DataGridView();
			btn_Search = new dotNetLab.Widgets.MobileButton();
			textBlock3 = new dotNetLab.Widgets.TextBlock();
			textBlock2 = new dotNetLab.Widgets.TextBlock();
			textBlock1 = new dotNetLab.Widgets.TextBlock();
			linkLabel1 = new System.Windows.Forms.LinkLabel();
			dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			panel1 = new System.Windows.Forms.Panel();
			label3 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			rbtn_Log = new System.Windows.Forms.RadioButton();
			rbtn_NGReporting = new System.Windows.Forms.RadioButton();
			rbtn_NGDetails = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			panel1.SuspendLayout();
			SuspendLayout();
			dateTimePicker1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			dateTimePicker1.CalendarFont = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			dateTimePicker1.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			dateTimePicker1.Location = new System.Drawing.Point(57, 128);
			dateTimePicker1.Name = "dateTimePicker1";
			dateTimePicker1.Size = new System.Drawing.Size(200, 25);
			dateTimePicker1.TabIndex = 1;
			dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Location = new System.Drawing.Point(12, 169);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.RowTemplate.Height = 23;
			dataGridView1.Size = new System.Drawing.Size(628, 308);
			dataGridView1.TabIndex = 3;
			btn_Search.Anchor = System.Windows.Forms.AnchorStyles.Top;
			btn_Search.BackColor = System.Drawing.Color.Transparent;
			btn_Search.BorderColor = System.Drawing.Color.Empty;
			btn_Search.BorderThickness = -1;
			btn_Search.CornerAligment = dotNetLab.Widgets.Alignments.All;
			btn_Search.DataBindingInfo = null;
			btn_Search.EnableFlag = false;
			btn_Search.EnableMobileRound = false;
			btn_Search.EnableTextRenderHint = false;
			btn_Search.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			btn_Search.FlagColor = System.Drawing.Color.DodgerBlue;
			btn_Search.FlagThickness = 5;
			btn_Search.Font = new System.Drawing.Font("微软雅黑", 12f);
			btn_Search.ForeColor = System.Drawing.Color.White;
			btn_Search.GapBetweenTextFlag = 10;
			btn_Search.GapBetweenTextImage = 8;
			btn_Search.IConAlignment = System.Windows.Forms.LeftRightAlignment.Left;
			btn_Search.ImageSize = new System.Drawing.Size(0, 0);
			btn_Search.LEDStyle = false;
			btn_Search.Location = new System.Drawing.Point(518, 116);
			btn_Search.MainBindableProperty = "查询";
			btn_Search.Name = "btn_Search";
			btn_Search.NeedAnimation = false;
			btn_Search.NormalColor = System.Drawing.Color.DodgerBlue;
			btn_Search.PressColor = System.Drawing.Color.RoyalBlue;
			btn_Search.Radius = 5;
			btn_Search.Size = new System.Drawing.Size(122, 41);
			btn_Search.Source = null;
			btn_Search.TabIndex = 2;
			btn_Search.Text = "查询";
			btn_Search.UIElementBinders = null;
			btn_Search.UnderLine = false;
			btn_Search.UnderLineColor = System.Drawing.Color.DarkGray;
			btn_Search.UnderLineThickness = 2f;
			btn_Search.Vertical = false;
			btn_Search.WhereReturn = 0;
			btn_Search.Click += new System.EventHandler(btn_Search_Click);
			textBlock3.Anchor = System.Windows.Forms.AnchorStyles.Top;
			textBlock3.BackColor = System.Drawing.Color.Transparent;
			textBlock3.BorderColor = System.Drawing.Color.Empty;
			textBlock3.BorderThickness = -1;
			textBlock3.DataBindingInfo = null;
			textBlock3.EnableFlag = false;
			textBlock3.EnableTextRenderHint = false;
			textBlock3.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock3.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock3.FlagThickness = 8;
			textBlock3.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock3.GapBetweenTextFlag = 10;
			textBlock3.LEDStyle = false;
			textBlock3.Location = new System.Drawing.Point(263, 128);
			textBlock3.MainBindableProperty = "至";
			textBlock3.Name = "textBlock3";
			textBlock3.Radius = -1;
			textBlock3.Size = new System.Drawing.Size(32, 23);
			textBlock3.TabIndex = 0;
			textBlock3.Text = "至";
			textBlock3.UIElementBinders = null;
			textBlock3.UnderLine = false;
			textBlock3.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock3.UnderLineThickness = 2f;
			textBlock3.Vertical = false;
			textBlock3.WhereReturn = 0;
			textBlock2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			textBlock2.BackColor = System.Drawing.Color.Transparent;
			textBlock2.BorderColor = System.Drawing.Color.Empty;
			textBlock2.BorderThickness = -1;
			textBlock2.DataBindingInfo = null;
			textBlock2.EnableFlag = false;
			textBlock2.EnableTextRenderHint = false;
			textBlock2.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock2.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock2.FlagThickness = 8;
			textBlock2.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock2.GapBetweenTextFlag = 10;
			textBlock2.LEDStyle = false;
			textBlock2.Location = new System.Drawing.Point(12, 128);
			textBlock2.MainBindableProperty = "从";
			textBlock2.Name = "textBlock2";
			textBlock2.Radius = -1;
			textBlock2.Size = new System.Drawing.Size(32, 23);
			textBlock2.TabIndex = 0;
			textBlock2.Text = "从";
			textBlock2.UIElementBinders = null;
			textBlock2.UnderLine = false;
			textBlock2.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock2.UnderLineThickness = 2f;
			textBlock2.Vertical = false;
			textBlock2.WhereReturn = 0;
			textBlock1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			textBlock1.BackColor = System.Drawing.Color.Transparent;
			textBlock1.BorderColor = System.Drawing.Color.Empty;
			textBlock1.BorderThickness = -1;
			textBlock1.DataBindingInfo = null;
			textBlock1.EnableFlag = true;
			textBlock1.EnableTextRenderHint = true;
			textBlock1.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock1.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock1.FlagThickness = 8;
			textBlock1.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock1.GapBetweenTextFlag = 10;
			textBlock1.LEDStyle = false;
			textBlock1.Location = new System.Drawing.Point(12, 89);
			textBlock1.MainBindableProperty = "按日期来查";
			textBlock1.Name = "textBlock1";
			textBlock1.Radius = -1;
			textBlock1.Size = new System.Drawing.Size(150, 23);
			textBlock1.TabIndex = 0;
			textBlock1.Text = "按日期来查";
			textBlock1.UIElementBinders = null;
			textBlock1.UnderLine = false;
			textBlock1.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock1.UnderLineThickness = 2f;
			textBlock1.Vertical = false;
			textBlock1.WhereReturn = 0;
			linkLabel1.AutoSize = true;
			linkLabel1.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			linkLabel1.Location = new System.Drawing.Point(524, 91);
			linkLabel1.Name = "linkLabel1";
			linkLabel1.Size = new System.Drawing.Size(107, 20);
			linkLabel1.TabIndex = 4;
			linkLabel1.TabStop = true;
			linkLabel1.Text = "导出为Execel表";
			linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
			dateTimePicker2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			dateTimePicker2.CalendarFont = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			dateTimePicker2.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			dateTimePicker2.Location = new System.Drawing.Point(301, 126);
			dateTimePicker2.Name = "dateTimePicker2";
			dateTimePicker2.Size = new System.Drawing.Size(200, 25);
			dateTimePicker2.TabIndex = 1;
			panel1.BackColor = System.Drawing.Color.Purple;
			panel1.Controls.Add(label3);
			panel1.Controls.Add(label1);
			panel1.Dock = System.Windows.Forms.DockStyle.Top;
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(652, 77);
			panel1.TabIndex = 5;
			label3.AutoSize = true;
			label3.Font = new System.Drawing.Font("MV Boli", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label3.ForeColor = System.Drawing.Color.White;
			label3.Location = new System.Drawing.Point(486, 21);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(145, 31);
			label3.TabIndex = 5;
			label3.Text = "Query Data";
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("等线 Light", 25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label1.ForeColor = System.Drawing.Color.White;
			label1.Location = new System.Drawing.Point(19, 21);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(151, 36);
			label1.TabIndex = 0;
			label1.Text = "检索数据";
			rbtn_Log.AutoSize = true;
			rbtn_Log.Checked = true;
			rbtn_Log.Font = new System.Drawing.Font("微软雅黑", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			rbtn_Log.Location = new System.Drawing.Point(157, 91);
			rbtn_Log.Name = "rbtn_Log";
			rbtn_Log.Size = new System.Drawing.Size(50, 21);
			rbtn_Log.TabIndex = 6;
			rbtn_Log.TabStop = true;
			rbtn_Log.Text = "日志";
			rbtn_Log.UseVisualStyleBackColor = true;
			rbtn_NGReporting.AutoSize = true;
			rbtn_NGReporting.Font = new System.Drawing.Font("微软雅黑", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			rbtn_NGReporting.Location = new System.Drawing.Point(226, 91);
			rbtn_NGReporting.Name = "rbtn_NGReporting";
			rbtn_NGReporting.Size = new System.Drawing.Size(87, 21);
			rbtn_NGReporting.TabIndex = 6;
			rbtn_NGReporting.Text = "OKNG统计";
			rbtn_NGReporting.UseVisualStyleBackColor = true;
			rbtn_NGDetails.AutoSize = true;
			rbtn_NGDetails.Font = new System.Drawing.Font("微软雅黑", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			rbtn_NGDetails.Location = new System.Drawing.Point(334, 91);
			rbtn_NGDetails.Name = "rbtn_NGDetails";
			rbtn_NGDetails.Size = new System.Drawing.Size(69, 21);
			rbtn_NGDetails.TabIndex = 6;
			rbtn_NGDetails.Text = "NG明细";
			rbtn_NGDetails.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(652, 489);
			base.Controls.Add(rbtn_NGDetails);
			base.Controls.Add(rbtn_NGReporting);
			base.Controls.Add(rbtn_Log);
			base.Controls.Add(panel1);
			base.Controls.Add(linkLabel1);
			base.Controls.Add(dataGridView1);
			base.Controls.Add(btn_Search);
			base.Controls.Add(dateTimePicker2);
			base.Controls.Add(dateTimePicker1);
			base.Controls.Add(textBlock3);
			base.Controls.Add(textBlock2);
			base.Controls.Add(textBlock1);
			base.Name = "RetriveDataForm";
			Text = "检索数据";
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
