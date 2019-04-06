using dotNetLab;
using dotNetLab.Common;
using dotNetLab.Common.ModernUI;
using dotNetLab.Vision.VPro;
using dotNetLab.Widgets;
using dotNetLab.Widgets.UIBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace shikii.VisionJob
{
	public class MenuForm : SessionPage
	{
		private Card card2;

		private TextBlock textBlock2;

		private Card card1;

		private MobileTextBox txb_NewProject;

		private TextBlock textBlock4;

		private TextBlock textBlock3;

		private TextBlock textBlock1;

		private MobileButton btn_DeleteProject;

		private TextBlock textBlock5;

		private TextBlock textBlock6;

		private ComboBox cmbx_NewProjectBaseOnWhichProject;

		private ComboBox cmbx_CurrentProjectName;

		private ComboBox cmbx_DeleteProject;

		private Label label2;

		private Label label1;

		private MobileTextBox mobileTextBox4;

		private LinkLabel lnk_RetriveLogs;

		private LinkLabel lnk_CommunicationConfig;

		private LinkLabel lnk_ManualRun;

		private LinkLabel lnk_UseDataCenter;

		private Label label3;

		private Toggle cbx_ApplyPriority;

		private LinkLabel linkLabel1;

		private LinkLabel lnk_OpenCurrentProjectFolder;

		private LinkLabel lnk_CommunicationWizard;

		private LinkLabel lnk_Makeup;

		private ColorDecorator colorDecorator1;

		protected override void prepareCtrls()
		{
			base.prepareCtrls();
			InitializeComponent();
			PrepareProjsUI();
		}

		private void PrepareProjsUI()
		{
			cmbx_CurrentProjectName.Items.Clear();
			cmbx_DeleteProject.Items.Clear();
			cmbx_NewProjectBaseOnWhichProject.Items.Clear();
			string[] array = null;
			if (!Directory.Exists("Projs"))
			{
				Directory.CreateDirectory("Projs");
			}
			array = Directory.GetDirectories("Projs");
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Path.GetFileNameWithoutExtension(array[i]);
			}
			ComboBox.ObjectCollection items = cmbx_CurrentProjectName.Items;
			object[] items2 = array;
			items.AddRange(items2);
			cmbx_CurrentProjectName.Text = Path.GetFileNameWithoutExtension(base.CompactDB.FetchValue("Current_Project", true, "0"));
			ComboBox.ObjectCollection items3 = cmbx_DeleteProject.Items;
			items2 = array;
			items3.AddRange(items2);
			ComboBox.ObjectCollection items4 = cmbx_NewProjectBaseOnWhichProject.Items;
			items2 = array;
			items4.AddRange(items2);
			cmbx_NewProjectBaseOnWhichProject.Text = cmbx_CurrentProjectName.Text;
		}

		protected override void prepareData()
		{
			base.prepareData();
			string s = base.CompactDB.FetchValue(App.ApplyUserPriority, true, "0");
			if (int.Parse(s) > 0)
			{
				cbx_ApplyPriority.Checked = true;
			}
			else
			{
				cbx_ApplyPriority.Checked = false;
			}
			base.CompactDB.GetAllTableNames();
			List<string> nameColumnValues = base.CompactDB.GetNameColumnValues(base.CompactDB.DefaultTable);
			if (nameColumnValues.Count == 0)
			{
				base.ConsolePipe.Error("上述为项目相关的记录不存在");
			}
			if (!nameColumnValues.Contains("Current_Project"))
			{
				base.CompactDB.Write("Current_Project", "0");
			}
			if (!Directory.Exists("Projs"))
			{
				Directory.CreateDirectory("Projs");
			}
			if (!nameColumnValues.Contains("AutoClearTime"))
			{
				base.CompactDB.Write("AutoClearTime", "3");
			}
		}

		protected override void prepareAppearance()
		{
			base.prepareAppearance();
			base.EnableDrawUpDownPattern = true;
			base.Img_Up = UI.RibbonTreeRings;
			base.Img_Down = UI.RibbonDoodleDiamonds;
		}

		protected override void prepareEvents()
		{
			base.prepareEvents();
			txb_NewProject.txb.KeyDown += delegate(object s, KeyEventArgs e)
			{
				if (e.KeyData == Keys.Return)
				{
					string text = string.Format("Projs\\{0}", txb_NewProject.Text.Trim());
					Directory.CreateDirectory(text);
					Thread.Sleep(500);
					string[] files2 = Directory.GetFiles(Path.Combine("Projs", cmbx_NewProjectBaseOnWhichProject.Text));
					for (int j = 0; j < files2.Length; j++)
					{
						File.Copy(files2[j], Path.Combine(text, Path.GetFileName(files2[j])));
					}
					dotNetLab.Tipper.Info = "创建项目完成";
					PrepareProjsUI();
				}
			};
			btn_DeleteProject.Click += delegate
			{
				if (cmbx_DeleteProject.Text == cmbx_CurrentProjectName.Text)
				{
					dotNetLab.Tipper.Error = "不能删除当前正在运行的项目";
				}
				else
				{
					string[] files = Directory.GetFiles(string.Format("Projs\\{0}", cmbx_DeleteProject.Text));
					for (int i = 0; i < files.Length; i++)
					{
						File.Delete(files[i]);
					}
					Directory.Delete(string.Format("Projs\\{0}", cmbx_DeleteProject.Text));
				}
				PrepareProjsUI();
			};
			cmbx_CurrentProjectName.SelectedIndexChanged += delegate
			{
				string strValue = string.Format("Projs\\{0}", cmbx_CurrentProjectName.Text);
				base.CompactDB.Write(App.CurrentProject, strValue);
				App.frm.PrepareVision();
			};
		}

		public void ShowCognexQuickBuildPart(ToolBlockPowerSuite ThisToolBlockPowerSuite)
		{
			PatternForm frm = new PatternForm();
			frm.PrepareToolBlockEditor(frm.editDapter, ThisToolBlockPowerSuite.ThisToolBlock, ThisToolBlockPowerSuite.ThisToolBlock, ThisToolBlockPowerSuite.VppName);
			frm.FormClosed += delegate
			{
				frm.Dispose();
			};
			frm.Show();
		}

		private void lnk_RetriveLogs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			RetriveDataForm frm = new RetriveDataForm();
			frm.Show();
			frm.FormClosed += delegate
			{
				frm.Dispose();
			};
		}

		private void lnk_CommunicationConfig_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			AppManager.ShowPage(typeof(TCPNetConnectForm));
		}

		private void lnk_ManualRun_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
		}

		private void lnk_UseDataCenter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			AppManager.ShowCompactDBEditor("shikii.db");
		}

		private void cbx_ApplyPriority_Click(object sender, EventArgs e)
		{
			if (cbx_ApplyPriority.Checked)
			{
				base.CompactDB.Write(App.ApplyUserPriority, "1");
			}
			else
			{
				base.CompactDB.Write(App.ApplyUserPriority, "0");
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			App.ShowJobManagerWnd(this);
		}

		private void lnk_OpenCurrentProjectFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string arguments = string.Format(Path.GetDirectoryName(Application.ExecutablePath) + "\\Projs\\{0}", cmbx_CurrentProjectName.Text);
			Process.Start("Explorer.exe", arguments);
		}

		private void InitializeComponent()
		{
			dotNetLab.Widgets.UIBinding.UIElementBinderInfo uIElementBinderInfo = new dotNetLab.Widgets.UIBinding.UIElementBinderInfo();
			mobileTextBox4 = new dotNetLab.Widgets.MobileTextBox();
			colorDecorator1 = new dotNetLab.Widgets.ColorDecorator();
			card2 = new dotNetLab.Widgets.Card();
			lnk_OpenCurrentProjectFolder = new System.Windows.Forms.LinkLabel();
			lnk_CommunicationWizard = new System.Windows.Forms.LinkLabel();
			linkLabel1 = new System.Windows.Forms.LinkLabel();
			label3 = new System.Windows.Forms.Label();
			cbx_ApplyPriority = new dotNetLab.Widgets.Toggle();
			lnk_UseDataCenter = new System.Windows.Forms.LinkLabel();
			lnk_ManualRun = new System.Windows.Forms.LinkLabel();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			lnk_Makeup = new System.Windows.Forms.LinkLabel();
			lnk_RetriveLogs = new System.Windows.Forms.LinkLabel();
			lnk_CommunicationConfig = new System.Windows.Forms.LinkLabel();
			textBlock2 = new dotNetLab.Widgets.TextBlock();
			card1 = new dotNetLab.Widgets.Card();
			cmbx_DeleteProject = new System.Windows.Forms.ComboBox();
			cmbx_NewProjectBaseOnWhichProject = new System.Windows.Forms.ComboBox();
			cmbx_CurrentProjectName = new System.Windows.Forms.ComboBox();
			btn_DeleteProject = new dotNetLab.Widgets.MobileButton();
			textBlock5 = new dotNetLab.Widgets.TextBlock();
			txb_NewProject = new dotNetLab.Widgets.MobileTextBox();
			textBlock6 = new dotNetLab.Widgets.TextBlock();
			textBlock4 = new dotNetLab.Widgets.TextBlock();
			textBlock3 = new dotNetLab.Widgets.TextBlock();
			textBlock1 = new dotNetLab.Widgets.TextBlock();
			card2.SuspendLayout();
			card1.SuspendLayout();
			SuspendLayout();
			tipper.Location = new System.Drawing.Point(297, 446);
			mobileTextBox4.ActiveColor = System.Drawing.Color.Cyan;
			mobileTextBox4.BackColor = System.Drawing.Color.Transparent;
			uIElementBinderInfo.DBEngineIndex = 0;
			uIElementBinderInfo.EnableCheckBox_One_Zero = false;
			uIElementBinderInfo.FieldName = "Val";
			uIElementBinderInfo.Filter = "Name='AutoClearTime' ";
			uIElementBinderInfo.Ptr = null;
			uIElementBinderInfo.StoreInDB = true;
			uIElementBinderInfo.StoreIntoDBRealTime = true;
			uIElementBinderInfo.TableName = "App_Extension_Data_Table";
			uIElementBinderInfo.ThisControl = mobileTextBox4;
			mobileTextBox4.DataBindingInfo = uIElementBinderInfo;
			mobileTextBox4.DoubleValue = double.NaN;
			mobileTextBox4.EnableMobileRound = true;
			mobileTextBox4.EnableNullValue = false;
			mobileTextBox4.FillColor = System.Drawing.Color.Transparent;
			mobileTextBox4.FloatValue = float.NaN;
			mobileTextBox4.Font = new System.Drawing.Font("微软雅黑", 13f);
			mobileTextBox4.ForeColor = System.Drawing.Color.Black;
			mobileTextBox4.GreyPattern = false;
			mobileTextBox4.IntValue = -2147483648;
			mobileTextBox4.LineThickness = 2f;
			mobileTextBox4.Location = new System.Drawing.Point(86, 259);
			mobileTextBox4.MainBindableProperty = "";
			mobileTextBox4.Name = "mobileTextBox4";
			mobileTextBox4.Radius = 29;
			mobileTextBox4.Size = new System.Drawing.Size(73, 30);
			mobileTextBox4.StaticColor = System.Drawing.Color.Gray;
			mobileTextBox4.TabIndex = 3;
			mobileTextBox4.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			mobileTextBox4.TextBackColor = System.Drawing.SystemColors.Window;
			mobileTextBox4.TextBoxStyle = dotNetLab.Widgets.MobileTextBox.TextBoxStyles.Mobile;
			mobileTextBox4.UIElementBinders = null;
			mobileTextBox4.WhitePattern = false;
			colorDecorator1.BackColor = System.Drawing.Color.White;
			colorDecorator1.DataBindingInfo = null;
			colorDecorator1.Location = new System.Drawing.Point(10, 466);
			colorDecorator1.MainBindableProperty = "";
			colorDecorator1.Name = "colorDecorator1";
			colorDecorator1.Size = new System.Drawing.Size(150, 53);
			colorDecorator1.TabIndex = 1;
			colorDecorator1.UIElementBinders = null;
			card2.BackColor = System.Drawing.Color.Transparent;
			card2.BorderColor = System.Drawing.Color.Gray;
			card2.BorderThickness = 0;
			card2.Controls.Add(lnk_OpenCurrentProjectFolder);
			card2.Controls.Add(lnk_CommunicationWizard);
			card2.Controls.Add(linkLabel1);
			card2.Controls.Add(label3);
			card2.Controls.Add(cbx_ApplyPriority);
			card2.Controls.Add(lnk_UseDataCenter);
			card2.Controls.Add(lnk_ManualRun);
			card2.Controls.Add(label2);
			card2.Controls.Add(label1);
			card2.Controls.Add(mobileTextBox4);
			card2.Controls.Add(lnk_Makeup);
			card2.Controls.Add(lnk_RetriveLogs);
			card2.Controls.Add(lnk_CommunicationConfig);
			card2.Controls.Add(textBlock2);
			card2.CornerAlignment = dotNetLab.Widgets.Alignments.All;
			card2.DataBindingInfo = null;
			card2.Font = new System.Drawing.Font("微软雅黑", 11f);
			card2.HeadColor = System.Drawing.Color.Purple;
			card2.HeaderAlignment = dotNetLab.Widgets.Alignments.Right;
			card2.HeadHeight = 30;
			card2.ImagePos = new System.Drawing.Point(0, 0);
			card2.ImageSize = new System.Drawing.Size(0, 0);
			card2.Location = new System.Drawing.Point(327, 77);
			card2.MainBindableProperty = "card1";
			card2.Name = "card2";
			card2.NormalColor = System.Drawing.Color.Snow;
			card2.Radius = 10;
			card2.Size = new System.Drawing.Size(247, 383);
			card2.Source = null;
			card2.TabIndex = 2;
			card2.Text = "card1";
			card2.UIElementBinders = null;
			lnk_OpenCurrentProjectFolder.AutoSize = true;
			lnk_OpenCurrentProjectFolder.Location = new System.Drawing.Point(48, 155);
			lnk_OpenCurrentProjectFolder.Name = "lnk_OpenCurrentProjectFolder";
			lnk_OpenCurrentProjectFolder.Size = new System.Drawing.Size(129, 20);
			lnk_OpenCurrentProjectFolder.TabIndex = 10;
			lnk_OpenCurrentProjectFolder.TabStop = true;
			lnk_OpenCurrentProjectFolder.Text = "打开当前作业目录";
			lnk_OpenCurrentProjectFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnk_OpenCurrentProjectFolder_LinkClicked);
			lnk_CommunicationWizard.AutoSize = true;
			lnk_CommunicationWizard.Location = new System.Drawing.Point(80, 122);
			lnk_CommunicationWizard.Name = "lnk_CommunicationWizard";
			lnk_CommunicationWizard.Size = new System.Drawing.Size(54, 20);
			lnk_CommunicationWizard.TabIndex = 10;
			lnk_CommunicationWizard.TabStop = true;
			lnk_CommunicationWizard.Text = "通讯表";
			lnk_CommunicationWizard.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnk_CommunicationWizard_LinkClicked);
			linkLabel1.AutoSize = true;
			linkLabel1.Location = new System.Drawing.Point(75, 83);
			linkLabel1.Name = "linkLabel1";
			linkLabel1.Size = new System.Drawing.Size(69, 20);
			linkLabel1.TabIndex = 10;
			linkLabel1.TabStop = true;
			linkLabel1.Text = "作业管理";
			linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(11, 190);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(69, 20);
			label3.TabIndex = 9;
			label3.Text = "权限管理";
			cbx_ApplyPriority.BackColor = System.Drawing.Color.Transparent;
			cbx_ApplyPriority.BlockColor = System.Drawing.Color.DarkGray;
			cbx_ApplyPriority.BorderColor = System.Drawing.Color.DarkGray;
			cbx_ApplyPriority.BottomColor = System.Drawing.Color.DodgerBlue;
			cbx_ApplyPriority.Checked = false;
			cbx_ApplyPriority.DataBindingInfo = null;
			cbx_ApplyPriority.Location = new System.Drawing.Point(99, 189);
			cbx_ApplyPriority.MainBindableProperty = "";
			cbx_ApplyPriority.Name = "cbx_ApplyPriority";
			cbx_ApplyPriority.Size = new System.Drawing.Size(45, 22);
			cbx_ApplyPriority.TabIndex = 8;
			cbx_ApplyPriority.UIElementBinders = null;
			cbx_ApplyPriority.Click += new System.EventHandler(cbx_ApplyPriority_Click);
			lnk_UseDataCenter.AutoSize = true;
			lnk_UseDataCenter.Location = new System.Drawing.Point(63, 226);
			lnk_UseDataCenter.Name = "lnk_UseDataCenter";
			lnk_UseDataCenter.Size = new System.Drawing.Size(99, 20);
			lnk_UseDataCenter.TabIndex = 5;
			lnk_UseDataCenter.TabStop = true;
			lnk_UseDataCenter.Text = "使用数据中心";
			lnk_UseDataCenter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnk_UseDataCenter_LinkClicked);
			lnk_ManualRun.AutoSize = true;
			lnk_ManualRun.Location = new System.Drawing.Point(75, 48);
			lnk_ManualRun.Name = "lnk_ManualRun";
			lnk_ManualRun.Size = new System.Drawing.Size(69, 20);
			lnk_ManualRun.TabIndex = 5;
			lnk_ManualRun.TabStop = true;
			lnk_ManualRun.Text = "手动操作";
			lnk_ManualRun.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnk_ManualRun_LinkClicked);
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(165, 264);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(24, 20);
			label2.TabIndex = 4;
			label2.Text = "天";
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(11, 263);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(69, 20);
			label1.TabIndex = 4;
			label1.Text = "自动清理";
			lnk_Makeup.AutoSize = true;
			lnk_Makeup.Location = new System.Drawing.Point(75, 331);
			lnk_Makeup.Name = "lnk_Makeup";
			lnk_Makeup.Size = new System.Drawing.Size(54, 20);
			lnk_Makeup.TabIndex = 1;
			lnk_Makeup.TabStop = true;
			lnk_Makeup.Text = "补偿值";
			lnk_Makeup.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnk_Makeup_LinkClicked);
			lnk_RetriveLogs.AutoSize = true;
			lnk_RetriveLogs.Location = new System.Drawing.Point(62, 304);
			lnk_RetriveLogs.Name = "lnk_RetriveLogs";
			lnk_RetriveLogs.Size = new System.Drawing.Size(99, 20);
			lnk_RetriveLogs.TabIndex = 1;
			lnk_RetriveLogs.TabStop = true;
			lnk_RetriveLogs.Text = "查询往期日志";
			lnk_RetriveLogs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnk_RetriveLogs_LinkClicked);
			lnk_CommunicationConfig.AutoSize = true;
			lnk_CommunicationConfig.Location = new System.Drawing.Point(75, 14);
			lnk_CommunicationConfig.Name = "lnk_CommunicationConfig";
			lnk_CommunicationConfig.Size = new System.Drawing.Size(69, 20);
			lnk_CommunicationConfig.TabIndex = 1;
			lnk_CommunicationConfig.TabStop = true;
			lnk_CommunicationConfig.Text = "通讯配置";
			lnk_CommunicationConfig.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnk_CommunicationConfig_LinkClicked);
			textBlock2.BackColor = System.Drawing.Color.Transparent;
			textBlock2.BorderColor = System.Drawing.Color.Empty;
			textBlock2.BorderThickness = 0;
			textBlock2.DataBindingInfo = null;
			textBlock2.EnableFlag = false;
			textBlock2.EnableTextRenderHint = false;
			textBlock2.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock2.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock2.FlagThickness = 5;
			textBlock2.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock2.ForeColor = System.Drawing.Color.White;
			textBlock2.GapBetweenTextFlag = 10;
			textBlock2.LEDStyle = false;
			textBlock2.Location = new System.Drawing.Point(213, 139);
			textBlock2.MainBindableProperty = "杂项";
			textBlock2.Name = "textBlock2";
			textBlock2.Radius = 0;
			textBlock2.Size = new System.Drawing.Size(27, 48);
			textBlock2.TabIndex = 0;
			textBlock2.Text = "杂项";
			textBlock2.UIElementBinders = null;
			textBlock2.UnderLine = false;
			textBlock2.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock2.UnderLineThickness = 2f;
			textBlock2.Vertical = true;
			textBlock2.WhereReturn = 0;
			card1.BackColor = System.Drawing.Color.Transparent;
			card1.BorderColor = System.Drawing.Color.Gray;
			card1.BorderThickness = 0;
			card1.Controls.Add(cmbx_DeleteProject);
			card1.Controls.Add(cmbx_NewProjectBaseOnWhichProject);
			card1.Controls.Add(cmbx_CurrentProjectName);
			card1.Controls.Add(btn_DeleteProject);
			card1.Controls.Add(textBlock5);
			card1.Controls.Add(txb_NewProject);
			card1.Controls.Add(textBlock6);
			card1.Controls.Add(textBlock4);
			card1.Controls.Add(textBlock3);
			card1.Controls.Add(textBlock1);
			card1.CornerAlignment = dotNetLab.Widgets.Alignments.All;
			card1.DataBindingInfo = null;
			card1.Font = new System.Drawing.Font("微软雅黑", 11f);
			card1.HeadColor = System.Drawing.Color.DodgerBlue;
			card1.HeaderAlignment = dotNetLab.Widgets.Alignments.Up;
			card1.HeadHeight = 40;
			card1.ImagePos = new System.Drawing.Point(0, 0);
			card1.ImageSize = new System.Drawing.Size(0, 0);
			card1.Location = new System.Drawing.Point(53, 77);
			card1.MainBindableProperty = "card1";
			card1.Name = "card1";
			card1.NormalColor = System.Drawing.Color.Snow;
			card1.Radius = 10;
			card1.Size = new System.Drawing.Size(259, 383);
			card1.Source = null;
			card1.TabIndex = 3;
			card1.Text = "card1";
			card1.UIElementBinders = null;
			cmbx_DeleteProject.BackColor = System.Drawing.Color.DarkGray;
			cmbx_DeleteProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			cmbx_DeleteProject.ForeColor = System.Drawing.Color.Black;
			cmbx_DeleteProject.FormattingEnabled = true;
			cmbx_DeleteProject.Location = new System.Drawing.Point(24, 282);
			cmbx_DeleteProject.Name = "cmbx_DeleteProject";
			cmbx_DeleteProject.Size = new System.Drawing.Size(218, 28);
			cmbx_DeleteProject.TabIndex = 8;
			cmbx_NewProjectBaseOnWhichProject.BackColor = System.Drawing.Color.DarkGray;
			cmbx_NewProjectBaseOnWhichProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			cmbx_NewProjectBaseOnWhichProject.ForeColor = System.Drawing.Color.Black;
			cmbx_NewProjectBaseOnWhichProject.FormattingEnabled = true;
			cmbx_NewProjectBaseOnWhichProject.Location = new System.Drawing.Point(24, 203);
			cmbx_NewProjectBaseOnWhichProject.Name = "cmbx_NewProjectBaseOnWhichProject";
			cmbx_NewProjectBaseOnWhichProject.Size = new System.Drawing.Size(218, 28);
			cmbx_NewProjectBaseOnWhichProject.TabIndex = 8;
			cmbx_CurrentProjectName.BackColor = System.Drawing.Color.DarkGray;
			cmbx_CurrentProjectName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			cmbx_CurrentProjectName.ForeColor = System.Drawing.Color.Black;
			cmbx_CurrentProjectName.FormattingEnabled = true;
			cmbx_CurrentProjectName.Location = new System.Drawing.Point(24, 75);
			cmbx_CurrentProjectName.Name = "cmbx_CurrentProjectName";
			cmbx_CurrentProjectName.Size = new System.Drawing.Size(218, 28);
			cmbx_CurrentProjectName.TabIndex = 8;
			btn_DeleteProject.BackColor = System.Drawing.Color.Transparent;
			btn_DeleteProject.BorderColor = System.Drawing.Color.Empty;
			btn_DeleteProject.BorderThickness = 0;
			btn_DeleteProject.CornerAligment = dotNetLab.Widgets.Alignments.All;
			btn_DeleteProject.DataBindingInfo = null;
			btn_DeleteProject.EnableFlag = false;
			btn_DeleteProject.EnableMobileRound = true;
			btn_DeleteProject.EnableTextRenderHint = false;
			btn_DeleteProject.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			btn_DeleteProject.FlagColor = System.Drawing.Color.DodgerBlue;
			btn_DeleteProject.FlagThickness = 5;
			btn_DeleteProject.Font = new System.Drawing.Font("微软雅黑", 12f);
			btn_DeleteProject.ForeColor = System.Drawing.Color.White;
			btn_DeleteProject.GapBetweenTextFlag = 10;
			btn_DeleteProject.GapBetweenTextImage = 8;
			btn_DeleteProject.IConAlignment = System.Windows.Forms.LeftRightAlignment.Left;
			btn_DeleteProject.ImageSize = new System.Drawing.Size(0, 0);
			btn_DeleteProject.LEDStyle = false;
			btn_DeleteProject.Location = new System.Drawing.Point(12, 331);
			btn_DeleteProject.MainBindableProperty = "删除";
			btn_DeleteProject.Name = "btn_DeleteProject";
			btn_DeleteProject.NeedAnimation = true;
			btn_DeleteProject.NormalColor = System.Drawing.Color.Red;
			btn_DeleteProject.PressColor = System.Drawing.Color.Cyan;
			btn_DeleteProject.Radius = 34;
			btn_DeleteProject.Size = new System.Drawing.Size(230, 35);
			btn_DeleteProject.Source = null;
			btn_DeleteProject.TabIndex = 7;
			btn_DeleteProject.Text = "删除";
			btn_DeleteProject.UIElementBinders = null;
			btn_DeleteProject.UnderLine = false;
			btn_DeleteProject.UnderLineColor = System.Drawing.Color.DarkGray;
			btn_DeleteProject.UnderLineThickness = 2f;
			btn_DeleteProject.Vertical = false;
			btn_DeleteProject.WhereReturn = 0;
			textBlock5.BackColor = System.Drawing.Color.Transparent;
			textBlock5.BorderColor = System.Drawing.Color.Empty;
			textBlock5.BorderThickness = 0;
			textBlock5.DataBindingInfo = null;
			textBlock5.EnableFlag = true;
			textBlock5.EnableTextRenderHint = true;
			textBlock5.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock5.FlagColor = System.Drawing.Color.Gold;
			textBlock5.FlagThickness = 7;
			textBlock5.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock5.GapBetweenTextFlag = 10;
			textBlock5.LEDStyle = false;
			textBlock5.Location = new System.Drawing.Point(12, 256);
			textBlock5.MainBindableProperty = "删除项目";
			textBlock5.Name = "textBlock5";
			textBlock5.Radius = 0;
			textBlock5.Size = new System.Drawing.Size(96, 20);
			textBlock5.TabIndex = 5;
			textBlock5.Text = "删除项目";
			textBlock5.UIElementBinders = null;
			textBlock5.UnderLine = false;
			textBlock5.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock5.UnderLineThickness = 2f;
			textBlock5.Vertical = false;
			textBlock5.WhereReturn = 0;
			txb_NewProject.ActiveColor = System.Drawing.Color.Silver;
			txb_NewProject.BackColor = System.Drawing.Color.Transparent;
			txb_NewProject.DataBindingInfo = null;
			txb_NewProject.DoubleValue = double.NaN;
			txb_NewProject.EnableMobileRound = true;
			txb_NewProject.EnableNullValue = false;
			txb_NewProject.FillColor = System.Drawing.Color.Silver;
			txb_NewProject.FloatValue = float.NaN;
			txb_NewProject.Font = new System.Drawing.Font("微软雅黑", 13f);
			txb_NewProject.ForeColor = System.Drawing.Color.Black;
			txb_NewProject.GreyPattern = true;
			txb_NewProject.IntValue = -2147483648;
			txb_NewProject.LineThickness = 2f;
			txb_NewProject.Location = new System.Drawing.Point(12, 135);
			txb_NewProject.MainBindableProperty = "";
			txb_NewProject.Name = "txb_NewProject";
			txb_NewProject.Radius = 30;
			txb_NewProject.Size = new System.Drawing.Size(230, 31);
			txb_NewProject.StaticColor = System.Drawing.Color.Silver;
			txb_NewProject.TabIndex = 3;
			txb_NewProject.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
			txb_NewProject.TextBackColor = System.Drawing.Color.Silver;
			txb_NewProject.TextBoxStyle = dotNetLab.Widgets.MobileTextBox.TextBoxStyles.Mobile;
			txb_NewProject.UIElementBinders = null;
			txb_NewProject.WhitePattern = false;
			textBlock6.BackColor = System.Drawing.Color.Transparent;
			textBlock6.BorderColor = System.Drawing.Color.Empty;
			textBlock6.BorderThickness = 0;
			textBlock6.DataBindingInfo = null;
			textBlock6.EnableFlag = true;
			textBlock6.EnableTextRenderHint = true;
			textBlock6.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock6.FlagColor = System.Drawing.Color.DeepSkyBlue;
			textBlock6.FlagThickness = 7;
			textBlock6.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock6.GapBetweenTextFlag = 10;
			textBlock6.LEDStyle = false;
			textBlock6.Location = new System.Drawing.Point(12, 175);
			textBlock6.MainBindableProperty = "基于此项目创建新项目";
			textBlock6.Name = "textBlock6";
			textBlock6.Radius = 0;
			textBlock6.Size = new System.Drawing.Size(193, 20);
			textBlock6.TabIndex = 1;
			textBlock6.Text = "基于此项目创建新项目";
			textBlock6.UIElementBinders = null;
			textBlock6.UnderLine = false;
			textBlock6.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock6.UnderLineThickness = 2f;
			textBlock6.Vertical = false;
			textBlock6.WhereReturn = 0;
			textBlock4.BackColor = System.Drawing.Color.Transparent;
			textBlock4.BorderColor = System.Drawing.Color.Empty;
			textBlock4.BorderThickness = 0;
			textBlock4.DataBindingInfo = null;
			textBlock4.EnableFlag = true;
			textBlock4.EnableTextRenderHint = true;
			textBlock4.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock4.FlagColor = System.Drawing.Color.Green;
			textBlock4.FlagThickness = 7;
			textBlock4.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock4.GapBetweenTextFlag = 10;
			textBlock4.LEDStyle = false;
			textBlock4.Location = new System.Drawing.Point(12, 109);
			textBlock4.MainBindableProperty = "新建项目";
			textBlock4.Name = "textBlock4";
			textBlock4.Radius = 0;
			textBlock4.Size = new System.Drawing.Size(96, 20);
			textBlock4.TabIndex = 1;
			textBlock4.Text = "新建项目";
			textBlock4.UIElementBinders = null;
			textBlock4.UnderLine = false;
			textBlock4.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock4.UnderLineThickness = 2f;
			textBlock4.Vertical = false;
			textBlock4.WhereReturn = 0;
			textBlock3.BackColor = System.Drawing.Color.Transparent;
			textBlock3.BorderColor = System.Drawing.Color.Empty;
			textBlock3.BorderThickness = 0;
			textBlock3.DataBindingInfo = null;
			textBlock3.EnableFlag = true;
			textBlock3.EnableTextRenderHint = true;
			textBlock3.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock3.FlagColor = System.Drawing.Color.Crimson;
			textBlock3.FlagThickness = 7;
			textBlock3.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock3.GapBetweenTextFlag = 10;
			textBlock3.LEDStyle = false;
			textBlock3.Location = new System.Drawing.Point(12, 46);
			textBlock3.MainBindableProperty = "当前项目";
			textBlock3.Name = "textBlock3";
			textBlock3.Radius = 0;
			textBlock3.Size = new System.Drawing.Size(96, 20);
			textBlock3.TabIndex = 1;
			textBlock3.Text = "当前项目";
			textBlock3.UIElementBinders = null;
			textBlock3.UnderLine = false;
			textBlock3.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock3.UnderLineThickness = 2f;
			textBlock3.Vertical = false;
			textBlock3.WhereReturn = 0;
			textBlock1.BackColor = System.Drawing.Color.Transparent;
			textBlock1.BorderColor = System.Drawing.Color.Empty;
			textBlock1.BorderThickness = 0;
			textBlock1.DataBindingInfo = null;
			textBlock1.EnableFlag = false;
			textBlock1.EnableTextRenderHint = false;
			textBlock1.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock1.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock1.FlagThickness = 5;
			textBlock1.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock1.ForeColor = System.Drawing.Color.White;
			textBlock1.GapBetweenTextFlag = 10;
			textBlock1.LEDStyle = false;
			textBlock1.Location = new System.Drawing.Point(67, 14);
			textBlock1.MainBindableProperty = "项目管理";
			textBlock1.Name = "textBlock1";
			textBlock1.Radius = 0;
			textBlock1.Size = new System.Drawing.Size(109, 17);
			textBlock1.TabIndex = 0;
			textBlock1.Text = "项目管理";
			textBlock1.UIElementBinders = null;
			textBlock1.UnderLine = false;
			textBlock1.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock1.UnderLineThickness = 2f;
			textBlock1.Vertical = false;
			textBlock1.WhereReturn = 0;
			base.ClientSize = new System.Drawing.Size(600, 531);
			base.Controls.Add(card2);
			base.Controls.Add(card1);
			base.Controls.Add(colorDecorator1);
			base.Name = "MenuForm";
			Text = "控制面板";
			base.TitlePos = new System.Drawing.Point(80, 15);
			base.Controls.SetChildIndex(colorDecorator1, 0);
			base.Controls.SetChildIndex(card1, 0);
			base.Controls.SetChildIndex(card2, 0);
			card2.ResumeLayout(false);
			card2.PerformLayout();
			card1.ResumeLayout(false);
			ResumeLayout(false);
		}

		private void lnk_CommunicationWizard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			CommunicationWizard wizard = new CommunicationWizard();
			wizard.StartPosition = FormStartPosition.CenterScreen;
			wizard.FormClosed += delegate
			{
				wizard.Dispose();
			};
			wizard.Show();
		}

		private void lnk_Makeup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MakeupForm wizard = new MakeupForm();
			wizard.StartPosition = FormStartPosition.CenterScreen;
			wizard.FormClosed += delegate
			{
				wizard.Dispose();
			};
			wizard.Show();
		}
	}
}
