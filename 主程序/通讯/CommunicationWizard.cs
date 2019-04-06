using dotNetLab;
using dotNetLab.Common;
using dotNetLab.Widgets;
using dotNetLab.Widgets.Container;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace shikii.VisionJob
{
	public class CommunicationWizard : Form
	{
		private Panel panel1;

		private Label label3;

		private TextBlock textBlock1;

		private CanvasPanel canvasPanel1;

		private TextBlock textBlock2;

		private MobileComboBox cmbx_Kind;

		private MobileTextBox txb_TableName;

		private TextBlock textBlock3;

		private MobileButton btn_DataCenter;

		private TextBlock lbl_Status;

		private TextBlock textBlock4;

		private TextBlock textBlock6;

		private Label label1;

		public CommunicationWizard()
		{
			InitializeComponent();
			base.MaximizeBox = false;
			txb_TableName.txb.KeyUp += txb_TableName_KeyUp;
		}

		private void InitializeComponent()
		{
			panel1 = new System.Windows.Forms.Panel();
			label3 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			textBlock1 = new dotNetLab.Widgets.TextBlock();
			canvasPanel1 = new dotNetLab.Widgets.Container.CanvasPanel();
			textBlock4 = new dotNetLab.Widgets.TextBlock();
			txb_TableName = new dotNetLab.Widgets.MobileTextBox();
			cmbx_Kind = new dotNetLab.Widgets.MobileComboBox();
			textBlock3 = new dotNetLab.Widgets.TextBlock();
			textBlock6 = new dotNetLab.Widgets.TextBlock();
			textBlock2 = new dotNetLab.Widgets.TextBlock();
			btn_DataCenter = new dotNetLab.Widgets.MobileButton();
			lbl_Status = new dotNetLab.Widgets.TextBlock();
			panel1.SuspendLayout();
			canvasPanel1.SuspendLayout();
			SuspendLayout();
			panel1.BackColor = System.Drawing.Color.BlueViolet;
			panel1.Controls.Add(label3);
			panel1.Controls.Add(label1);
			panel1.Dock = System.Windows.Forms.DockStyle.Top;
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(611, 100);
			panel1.TabIndex = 2;
			label3.AutoSize = true;
			label3.Font = new System.Drawing.Font("MV Boli", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label3.ForeColor = System.Drawing.Color.White;
			label3.Location = new System.Drawing.Point(330, 32);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(269, 31);
			label3.TabIndex = 5;
			label3.Text = "Communication Wizard";
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("等线 Light", 25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label1.ForeColor = System.Drawing.Color.White;
			label1.Location = new System.Drawing.Point(27, 32);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(151, 36);
			label1.TabIndex = 0;
			label1.Text = "通讯向导";
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
			textBlock1.ForeColor = System.Drawing.Color.DimGray;
			textBlock1.GapBetweenTextFlag = 10;
			textBlock1.LEDStyle = false;
			textBlock1.Location = new System.Drawing.Point(12, 121);
			textBlock1.MainBindableProperty = "新增通讯表";
			textBlock1.Name = "textBlock1";
			textBlock1.Radius = -1;
			textBlock1.Size = new System.Drawing.Size(113, 25);
			textBlock1.TabIndex = 3;
			textBlock1.Text = "新增通讯表";
			textBlock1.UIElementBinders = null;
			textBlock1.UnderLine = false;
			textBlock1.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock1.UnderLineThickness = 2f;
			textBlock1.Vertical = false;
			textBlock1.WhereReturn = 0;
			canvasPanel1.BackColor = System.Drawing.Color.Transparent;
			canvasPanel1.BorderColor = System.Drawing.Color.Silver;
			canvasPanel1.BorderThickness = -1;
			canvasPanel1.Controls.Add(textBlock4);
			canvasPanel1.Controls.Add(txb_TableName);
			canvasPanel1.Controls.Add(cmbx_Kind);
			canvasPanel1.Controls.Add(textBlock3);
			canvasPanel1.Controls.Add(textBlock6);
			canvasPanel1.Controls.Add(textBlock2);
			canvasPanel1.CornerAlignment = dotNetLab.Widgets.Alignments.All;
			canvasPanel1.DataBindingInfo = null;
			canvasPanel1.Font = new System.Drawing.Font("微软雅黑", 11f);
			canvasPanel1.ImagePos = new System.Drawing.Point(0, 0);
			canvasPanel1.ImageSize = new System.Drawing.Size(0, 0);
			canvasPanel1.Location = new System.Drawing.Point(24, 152);
			canvasPanel1.MainBindableProperty = null;
			canvasPanel1.Name = "canvasPanel1";
			canvasPanel1.NormalColor = System.Drawing.Color.LightSteelBlue;
			canvasPanel1.Radius = 8;
			canvasPanel1.Size = new System.Drawing.Size(565, 253);
			canvasPanel1.Source = null;
			canvasPanel1.TabIndex = 4;
			canvasPanel1.Text = null;
			canvasPanel1.UIElementBinders = null;
			textBlock4.BackColor = System.Drawing.Color.Transparent;
			textBlock4.BorderColor = System.Drawing.Color.Empty;
			textBlock4.BorderThickness = -1;
			textBlock4.DataBindingInfo = null;
			textBlock4.EnableFlag = false;
			textBlock4.EnableTextRenderHint = true;
			textBlock4.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock4.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock4.FlagThickness = 5;
			textBlock4.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock4.ForeColor = System.Drawing.Color.Firebrick;
			textBlock4.GapBetweenTextFlag = 10;
			textBlock4.LEDStyle = false;
			textBlock4.Location = new System.Drawing.Point(0, 146);
			textBlock4.MainBindableProperty = "向导将在数据中心创建一张用于存储通讯参数的表格，你可以在创建\r\n\r\n后访问数据中心以修改默认值";
			textBlock4.Name = "textBlock4";
			textBlock4.Radius = -1;
			textBlock4.Size = new System.Drawing.Size(552, 83);
			textBlock4.TabIndex = 3;
			textBlock4.Text = "向导将在数据中心创建一张用于存储通讯参数的表格，你可以在创建\r\n\r\n后访问数据中心以修改默认值";
			textBlock4.UIElementBinders = null;
			textBlock4.UnderLine = false;
			textBlock4.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock4.UnderLineThickness = 2f;
			textBlock4.Vertical = false;
			textBlock4.WhereReturn = 30;
			txb_TableName.ActiveColor = System.Drawing.Color.White;
			txb_TableName.BackColor = System.Drawing.Color.Transparent;
			txb_TableName.DataBindingInfo = null;
			txb_TableName.DoubleValue = double.NaN;
			txb_TableName.EnableMobileRound = true;
			txb_TableName.EnableNullValue = false;
			txb_TableName.FillColor = System.Drawing.Color.White;
			txb_TableName.FloatValue = float.NaN;
			txb_TableName.Font = new System.Drawing.Font("微软雅黑", 13f);
			txb_TableName.ForeColor = System.Drawing.Color.Black;
			txb_TableName.GreyPattern = false;
			txb_TableName.IntValue = -2147483648;
			txb_TableName.LineThickness = 2f;
			txb_TableName.Location = new System.Drawing.Point(112, 100);
			txb_TableName.MainBindableProperty = "";
			txb_TableName.Name = "txb_TableName";
			txb_TableName.Radius = 30;
			txb_TableName.Size = new System.Drawing.Size(206, 31);
			txb_TableName.StaticColor = System.Drawing.Color.White;
			txb_TableName.TabIndex = 2;
			txb_TableName.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
			txb_TableName.TextBackColor = System.Drawing.Color.White;
			txb_TableName.TextBoxStyle = dotNetLab.Widgets.MobileTextBox.TextBoxStyles.Mobile;
			txb_TableName.UIElementBinders = null;
			txb_TableName.WhitePattern = true;
			txb_TableName.KeyUp += new System.Windows.Forms.KeyEventHandler(txb_TableName_KeyUp);
			cmbx_Kind.BackColor = System.Drawing.Color.White;
			cmbx_Kind.BorderColor = System.Drawing.Color.Gray;
			cmbx_Kind.DataBindingInfo = null;
			cmbx_Kind.DisplayItems = 0;
			cmbx_Kind.EnableAnimation = false;
			cmbx_Kind.Items = new string[2]
			{
				"TCP/IP",
				"串口"
			};
			cmbx_Kind.Location = new System.Drawing.Point(116, 39);
			cmbx_Kind.MainBindableProperty = "";
			cmbx_Kind.Name = "cmbx_Kind";
			cmbx_Kind.SelectedItem = "TCP/IP";
			cmbx_Kind.Size = new System.Drawing.Size(353, 35);
			cmbx_Kind.TabIndex = 1;
			cmbx_Kind.UIElementBinders = null;
			textBlock3.BackColor = System.Drawing.Color.Transparent;
			textBlock3.BorderColor = System.Drawing.Color.Empty;
			textBlock3.BorderThickness = -1;
			textBlock3.DataBindingInfo = null;
			textBlock3.EnableFlag = false;
			textBlock3.EnableTextRenderHint = true;
			textBlock3.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock3.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock3.FlagThickness = 5;
			textBlock3.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock3.GapBetweenTextFlag = 10;
			textBlock3.LEDStyle = false;
			textBlock3.Location = new System.Drawing.Point(54, 102);
			textBlock3.MainBindableProperty = "表名";
			textBlock3.Name = "textBlock3";
			textBlock3.Radius = -1;
			textBlock3.Size = new System.Drawing.Size(57, 25);
			textBlock3.TabIndex = 0;
			textBlock3.Text = "表名";
			textBlock3.UIElementBinders = null;
			textBlock3.UnderLine = false;
			textBlock3.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock3.UnderLineThickness = 2f;
			textBlock3.Vertical = false;
			textBlock3.WhereReturn = 0;
			textBlock6.BackColor = System.Drawing.Color.Transparent;
			textBlock6.BorderColor = System.Drawing.Color.Empty;
			textBlock6.BorderThickness = -1;
			textBlock6.DataBindingInfo = null;
			textBlock6.EnableFlag = false;
			textBlock6.EnableTextRenderHint = true;
			textBlock6.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock6.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock6.FlagThickness = 5;
			textBlock6.Font = new System.Drawing.Font("微软雅黑", 10f);
			textBlock6.ForeColor = System.Drawing.Color.Crimson;
			textBlock6.GapBetweenTextFlag = 10;
			textBlock6.LEDStyle = false;
			textBlock6.Location = new System.Drawing.Point(337, 102);
			textBlock6.MainBindableProperty = "输完表名后请回车以创建表";
			textBlock6.Name = "textBlock6";
			textBlock6.Radius = -1;
			textBlock6.Size = new System.Drawing.Size(183, 25);
			textBlock6.TabIndex = 0;
			textBlock6.Text = "输完表名后请回车以创建表";
			textBlock6.UIElementBinders = null;
			textBlock6.UnderLine = false;
			textBlock6.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock6.UnderLineThickness = 2f;
			textBlock6.Vertical = false;
			textBlock6.WhereReturn = 0;
			textBlock2.BackColor = System.Drawing.Color.Transparent;
			textBlock2.BorderColor = System.Drawing.Color.Empty;
			textBlock2.BorderThickness = -1;
			textBlock2.DataBindingInfo = null;
			textBlock2.EnableFlag = false;
			textBlock2.EnableTextRenderHint = true;
			textBlock2.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock2.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock2.FlagThickness = 5;
			textBlock2.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock2.GapBetweenTextFlag = 10;
			textBlock2.LEDStyle = false;
			textBlock2.Location = new System.Drawing.Point(29, 45);
			textBlock2.MainBindableProperty = "何种类型";
			textBlock2.Name = "textBlock2";
			textBlock2.Radius = -1;
			textBlock2.Size = new System.Drawing.Size(82, 25);
			textBlock2.TabIndex = 0;
			textBlock2.Text = "何种类型";
			textBlock2.UIElementBinders = null;
			textBlock2.UnderLine = false;
			textBlock2.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock2.UnderLineThickness = 2f;
			textBlock2.Vertical = false;
			textBlock2.WhereReturn = 0;
			btn_DataCenter.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			btn_DataCenter.BackColor = System.Drawing.Color.Transparent;
			btn_DataCenter.BorderColor = System.Drawing.Color.Empty;
			btn_DataCenter.BorderThickness = -1;
			btn_DataCenter.CornerAligment = dotNetLab.Widgets.Alignments.All;
			btn_DataCenter.DataBindingInfo = null;
			btn_DataCenter.EnableFlag = false;
			btn_DataCenter.EnableMobileRound = false;
			btn_DataCenter.EnableTextRenderHint = false;
			btn_DataCenter.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			btn_DataCenter.FlagColor = System.Drawing.Color.DodgerBlue;
			btn_DataCenter.FlagThickness = 5;
			btn_DataCenter.Font = new System.Drawing.Font("微软雅黑", 12f);
			btn_DataCenter.ForeColor = System.Drawing.Color.White;
			btn_DataCenter.GapBetweenTextFlag = 10;
			btn_DataCenter.GapBetweenTextImage = 8;
			btn_DataCenter.IConAlignment = System.Windows.Forms.LeftRightAlignment.Left;
			btn_DataCenter.ImageSize = new System.Drawing.Size(0, 0);
			btn_DataCenter.LEDStyle = false;
			btn_DataCenter.Location = new System.Drawing.Point(462, 411);
			btn_DataCenter.MainBindableProperty = "数据中心";
			btn_DataCenter.Name = "btn_DataCenter";
			btn_DataCenter.NeedAnimation = false;
			btn_DataCenter.NormalColor = System.Drawing.Color.Gray;
			btn_DataCenter.PressColor = System.Drawing.Color.DimGray;
			btn_DataCenter.Radius = 25;
			btn_DataCenter.Size = new System.Drawing.Size(127, 46);
			btn_DataCenter.Source = null;
			btn_DataCenter.TabIndex = 5;
			btn_DataCenter.Text = "数据中心";
			btn_DataCenter.UIElementBinders = null;
			btn_DataCenter.UnderLine = false;
			btn_DataCenter.UnderLineColor = System.Drawing.Color.DarkGray;
			btn_DataCenter.UnderLineThickness = 2f;
			btn_DataCenter.Vertical = false;
			btn_DataCenter.WhereReturn = 0;
			btn_DataCenter.Click += new System.EventHandler(btn_DataCenter_Click);
			lbl_Status.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			lbl_Status.BackColor = System.Drawing.Color.Transparent;
			lbl_Status.BorderColor = System.Drawing.Color.Empty;
			lbl_Status.BorderThickness = -1;
			lbl_Status.DataBindingInfo = null;
			lbl_Status.EnableFlag = false;
			lbl_Status.EnableTextRenderHint = false;
			lbl_Status.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			lbl_Status.FlagColor = System.Drawing.Color.DodgerBlue;
			lbl_Status.FlagThickness = 5;
			lbl_Status.Font = new System.Drawing.Font("微软雅黑", 10f);
			lbl_Status.GapBetweenTextFlag = 10;
			lbl_Status.LEDStyle = false;
			lbl_Status.Location = new System.Drawing.Point(4, 443);
			lbl_Status.MainBindableProperty = "就绪";
			lbl_Status.Name = "lbl_Status";
			lbl_Status.Radius = -1;
			lbl_Status.Size = new System.Drawing.Size(79, 23);
			lbl_Status.TabIndex = 6;
			lbl_Status.Text = "就绪";
			lbl_Status.UIElementBinders = null;
			lbl_Status.UnderLine = false;
			lbl_Status.UnderLineColor = System.Drawing.Color.DarkGray;
			lbl_Status.UnderLineThickness = 2f;
			lbl_Status.Vertical = false;
			lbl_Status.WhereReturn = 0;
			BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(611, 469);
			base.Controls.Add(lbl_Status);
			base.Controls.Add(btn_DataCenter);
			base.Controls.Add(canvasPanel1);
			base.Controls.Add(textBlock1);
			base.Controls.Add(panel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Name = "CommunicationWizard";
			Text = "通讯向导";
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			canvasPanel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		private void txb_TableName_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(txb_TableName.Text.Trim()))
			{
				try
				{
					if (cmbx_Kind.SelectedItem.Equals("串口"))
					{
						R.CompactDB.CopyKeyValueTable(txb_TableName.Text.Trim(), "SerialPort");
					}
					else
					{
						R.CompactDB.CopyKeyValueTable(txb_TableName.Text.Trim(), "TCP");
					}
					lbl_Status.Text = "创建成功";
					lbl_Status.ForeColor = Color.Green;
				}
				catch (Exception ex)
				{
					Tipper.Error = ex.Message;
					lbl_Status.Text = "创建失败";
					lbl_Status.ForeColor = Color.Red;
				}
			}
		}

		private void btn_DataCenter_Click(object sender, EventArgs e)
		{
			AppManager.ShowCompactDBEditor("shikii.db");
		}
	}
}
