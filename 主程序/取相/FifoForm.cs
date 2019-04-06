using Cognex.VisionPro;
using dotNetLab.Common.ModernUI;
using dotNetLab.Vision;
using dotNetLab.Widgets;
using dotNetLab.Widgets.Container;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace shikii.VisionJob
{
	public class FifoForm : SessionPage
	{
		public ComboBox comboBox1;

		public CanvasPanel canvasPanel1;

		public TextBlock textBlock1;

		public DspWndLayout DspWndLayoutManager;

		public Dictionary<string, object> dct_Fifos;

		protected override void prepareCtrls()
		{
			base.prepareCtrls();
			InitializeComponent();
			dct_Fifos = new Dictionary<string, object>();
			DspWndLayoutManager = new DspWndLayout();
			ArrangeDspWnds();
			AttachFifoTool();
		}

		private void ArrangeDspWnds()
		{
			int nHowDspWnds = 1;
			DspWndLayoutManager.PrepareDspWnds(typeof(CogRecordDisplay), canvasPanel1, nHowDspWnds);
			comboBox1.Items.AddRange(new object[0]);
		}

		protected void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			LiveDisplay(comboBox1.Text);
		}

		public void AttachFifoTool()
		{
			int num = 0;
			DspWndLayoutManager.DisplayWnds[num++].Tag = "ToolBlock 中的AcqFifoTool";
		}

		public void LiveDisplay(string str)
		{
		}

		private void InitializeComponent()
		{
			textBlock1 = new dotNetLab.Widgets.TextBlock();
			comboBox1 = new System.Windows.Forms.ComboBox();
			canvasPanel1 = new dotNetLab.Widgets.Container.CanvasPanel();
			SuspendLayout();
			textBlock1.BackColor = System.Drawing.Color.Transparent;
			textBlock1.BorderColor = System.Drawing.Color.Empty;
			textBlock1.BorderThickness = -1;
			textBlock1.DataBindingInfo = null;
			textBlock1.EnableFlag = true;
			textBlock1.EnableTextRenderHint = false;
			textBlock1.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock1.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock1.FlagThickness = 7;
			textBlock1.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock1.GapBetweenTextFlag = 10;
			textBlock1.LEDStyle = false;
			textBlock1.Location = new System.Drawing.Point(57, 80);
			textBlock1.MainBindableProperty = "相机实时取相";
			textBlock1.Name = "textBlock1";
			textBlock1.Radius = -1;
			textBlock1.Size = new System.Drawing.Size(166, 23);
			textBlock1.TabIndex = 1;
			textBlock1.Text = "相机实时取相";
			textBlock1.UIElementBinders = null;
			textBlock1.UnderLine = false;
			textBlock1.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock1.UnderLineThickness = 2f;
			textBlock1.Vertical = false;
			textBlock1.WhereReturn = 0;
			comboBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			comboBox1.BackColor = System.Drawing.Color.DarkGray;
			comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			comboBox1.Font = new System.Drawing.Font("微软雅黑", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			comboBox1.ForeColor = System.Drawing.Color.White;
			comboBox1.FormattingEnabled = true;
			comboBox1.Location = new System.Drawing.Point(102, 109);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new System.Drawing.Size(357, 27);
			comboBox1.TabIndex = 2;
			comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
			canvasPanel1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			canvasPanel1.BackColor = System.Drawing.Color.Transparent;
			canvasPanel1.BorderColor = System.Drawing.Color.Empty;
			canvasPanel1.BorderThickness = -1;
			canvasPanel1.CornerAlignment = dotNetLab.Widgets.Alignments.All;
			canvasPanel1.DataBindingInfo = null;
			canvasPanel1.Font = new System.Drawing.Font("微软雅黑", 11f);
			canvasPanel1.ImagePos = new System.Drawing.Point(0, 0);
			canvasPanel1.ImageSize = new System.Drawing.Size(0, 0);
			canvasPanel1.Location = new System.Drawing.Point(60, 142);
			canvasPanel1.MainBindableProperty = "显示区";
			canvasPanel1.Name = "canvasPanel1";
			canvasPanel1.NormalColor = System.Drawing.Color.DarkGray;
			canvasPanel1.Radius = -1;
			canvasPanel1.Size = new System.Drawing.Size(496, 334);
			canvasPanel1.Source = null;
			canvasPanel1.TabIndex = 3;
			canvasPanel1.Text = "显示区";
			canvasPanel1.UIElementBinders = null;
			base.ClientSize = new System.Drawing.Size(600, 500);
			base.Controls.Add(canvasPanel1);
			base.Controls.Add(comboBox1);
			base.Controls.Add(textBlock1);
			base.Name = "FifoForm";
			Text = "取相";
			base.TitlePos = new System.Drawing.Point(70, 15);
			base.Controls.SetChildIndex(textBlock1, 0);
			base.Controls.SetChildIndex(comboBox1, 0);
			base.Controls.SetChildIndex(canvasPanel1, 0);
			ResumeLayout(false);
		}
	}
}
