using Cognex.VisionPro;
using Cognex.VisionPro.FGGigE.Implementation.Internal;
using Cognex.VisionPro.ToolBlock;
using dotNetLab;
using dotNetLab.Common;
using dotNetLab.Common.ModernUI;
using dotNetLab.Data.Network;
using dotNetLab.Data.Uniting;
using dotNetLab.Network;
using dotNetLab.Vision;
using dotNetLab.Vision.VPro;
using dotNetLab.Widgets;
using dotNetLab.Widgets.Container;
using System;
using System.Collections.Generic;

using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace shikii.VisionJob
{
	public class MainForm : PageBase
	{
        private TCPFactoryServer factoryServer;

        private TCPFactoryClient factoryClient;

        private readonly string Y1OPEN = "AA 01 06 0A 00";

        private readonly string Y1CLOSE = "AA 01 06 0B 00";

        private readonly string Y2OPEN = "AA 01 07 0A 00";

        private readonly string Y2CLOSE = "AA 01 07 0B 00";

        private static object LockHardwareTrigger = new object();

		private static object LockSoftwareTrigger = new object();

		private ICogAcqInfo info = null;

		public EventHandler AutoTriggerHandler = null;

		private TextBlock textBlock6;

		private TextBlock lbl_GoodPercent;

		private TextBlock textBlock4;

		private TextBlock lbl_NGNum;

		private TextBlock textBlock2;

		private TextBlock lbl_TotalNum;

		public DspWndLayout DspWndLayoutManager;

		private TextBlock lbl_OutputInfo;

		private CanvasPanel canvasPanel1;

		private ColorDecorator colorDecorator1;

		public MobileListBox mobileListBox1;

		private Label label1;

		private PictureBox pictureBox1;

		private Label label2;

		private Label lbl_IncName;

		private Direction btn_More;
        private NormalPLC normalPLC;

   

        public void PrepareCanvases()
		{

			

			 
                string strLightOnTime = base.CompactDB.FetchValue("LightOnTime", true, "-1");
                if (!strLightOnTime.Equals("-1"))
                {
                    try
                    {
                        App.nLightOnTime = int.Parse(strLightOnTime);
                    }
                    catch (Exception)
                    {
                    }
                }
                //            CurrentMakeupTableName = string.Format("X{0}_Makeup", App.GetShortProjectName());
                //base.CompactDB.CreateKeyValueTable(CurrentMakeupTableName);
            
		}

		public void ShortCutRun()
		{
			 
		}

		public void PrepareCommunication()
		{
            //factoryClient = new TCPFactoryClient();
            //factoryServer = new TCPFactoryServer();
            normalPLC = new NormalPLC();
         
            base.CompactDB.TargetTable = "SerialPort";
            normalPLC.PortName = base.CompactDB.FetchValue("COM", true, "0");
            normalPLC.Parity = (Parity)base.CompactDB.FetchIntValue("Parity");
            normalPLC.StopBits = (StopBits)base.CompactDB.FetchIntValue("StopBits");
            normalPLC.BaudRate = base.CompactDB.FetchIntValue("BaudRate");
            normalPLC.DataBits = base.CompactDB.FetchIntValue("DataBits");
            normalPLC.BufferSize = 128;
            normalPLC.Open();
            base.CompactDB.TargetTable = base.CompactDB.DefaultTable;
        }

		private void ServerRouteMessage(int nWhichClient, byte[] byts)
		{
			string text = GetValidString(Encoding.ASCII.GetString(byts));
			if (text.Contains("\r\n"))
			{
				text = text.TrimEnd('\r', '\n');
			}
			 
			ClearBuffer(byts);
		}

		private void ClientRouteMessage(int nWhichClient, byte[] byts)
		{
			string validString = GetValidString(Encoding.ASCII.GetString(byts));
			ClearBuffer(byts);
		}

		private void ClearBuffer(byte[] byts)
		{
			for (int i = 0; i < byts.Length; i++)
			{
				byts[i] = 0;
			}
		}

		private string GetValidString(string str)
		{
			int length = str.IndexOf('\0');
			return str.Substring(0, length);
		}

		private void HardwareTiggerOccured(string CameraID, Cognex.VisionPro.ICogImage img)
		{


            switch (CameraID)
            {
                case "21699060": Run(App.ThisJobTool.ToolBlockSet[1],false, img);break;
                case "21699078": Run(App.ThisJobTool.ToolBlockSet[1],false, img); break;
            }

            
        }

		private string NGOccured(ToolBlockPowerSuite toolBlockPowerSuite, Canvas cnv, Cognex.VisionPro.ICogRecord irc, object Outputs)
		{
			 
			return "未找到特征";
		}

		private void OKOccured(ToolBlockPowerSuite toolBlockPowerSuite, Canvas cnv, Cognex.VisionPro.ICogRecord irc, object Outputs)
		{
			 
			 
			//cnv.DisplayText(1, text);
		}

	 

		public void RanToolBlock(ToolBlockPowerSuite toolBlockPowerSuite, Canvas cnv, Cognex.VisionPro.ICogRecord irc, object Outputs)
		{
			cnv.Display(irc);
			if (toolBlockPowerSuite.Passed)
			{
				OKOccured(toolBlockPowerSuite, cnv, irc, Outputs);
			}
			else
			{
				App.nNGNum++;
				string NGDescription = NGOccured(toolBlockPowerSuite, cnv, irc, Outputs);
				new Thread((ThreadStart)delegate
				{
					CaptureCurrentImage(toolBlockPowerSuite, NGDescription);
				}).Start();
			}
		}

		private void CaptureCurrentImage(ToolBlockPowerSuite toolBlockPowerSuite, string strDscription = null)
		{
			Cognex.VisionPro.ICogImage cogImage = null;
			try
			{
				cogImage = (toolBlockPowerSuite.ThisToolBlock.Inputs[0].Value as Cognex.VisionPro.ICogImage);
			}
			catch
			{
				try
				{
					cogImage = (toolBlockPowerSuite.ThisToolBlock.Tools[0] as CogAcqFifoTool).OutputImage;
				}
				catch
				{
				}
			}
			Bitmap bmp = cogImage.ToBitmap();
			AutoSaveClearImage(bmp, strDscription, false, "图片", null);
		}

		public void DegbugVPROScript(Cognex.VisionPro.ICogTool tool, bool isBeforeRunTool)
		{
		}

		protected void AutoSaveClearImage(Bitmap bmp, string Description, bool OKNG = false, string picturePath = "图片", UnitDB thisDB = null)
		{
			if (thisDB == null)
			{
				thisDB = base.CompactDB;
			}
			List<string> nameColumnValues = base.CompactDB.GetNameColumnValues(base.CompactDB.DefaultTable);
			if (!nameColumnValues.Contains("AutoClearTime"))
			{
				base.CompactDB.Write("AutoClearTime", "3");
			}
			if (!Directory.Exists(picturePath))
			{
				Directory.CreateDirectory(picturePath);
			}
			DateTime dateTime = DateTime.Now;
			string text = string.Format("{0}\\{1}", picturePath, dateTime.ToString("yyyy-MM-dd"));
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string path = text;
			dateTime = DateTime.Now;
			string text2 = Path.Combine(path, dateTime.ToString("yyyy-MM-dd HH_mm_ss") + ".bmp");
			bmp.Save(text2);
			string text3 = null;
			text3 = ((!OKNG) ? "NG" : "OK");
			UnitDB unitDB = thisDB;
			string imageRecord = App.ImageRecord;
			object[] obj = new object[4]
			{
				text2,
				text3,
				null,
				null
			};
			dateTime = DateTime.Now;
			obj[2] = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			obj[3] = Description;
			unitDB.NewRecord(imageRecord, string.Format("'{0}','{1}','{2}','{3}'", obj));
			int num = base.CompactDB.FetchIntValue("AutoClearTime");
			if (Directory.GetDirectories(picturePath).Length > num)
			{
				dateTime = DateTime.Now;
				dateTime = DateTime.Parse(dateTime.ToString("yyyy-MM-dd"));
				string path2 = dateTime.AddDays((double)(-num)).ToString("yyyy-MM-dd");
				if (Directory.Exists(Path.Combine(picturePath, path2)))
				{
					string text4 = Path.Combine(picturePath, path2);
					string[] files = Directory.GetFiles(text4);
					for (int i = 0; i < files.Length; i++)
					{
						File.Delete(files[i]);
					}
					Directory.Delete(text4);
					base.CompactDB.RemoveRecord(App.ImageRecord, string.Format("图像生成日期 like '{0}%'", text4));
				}
			}
		}

		public virtual void Run(ToolBlockPowerSuite toolBlockPowerSuite, bool Lockable = false, params object[] Inputs)
		{
			if (!Lockable)
			{
				toolBlockPowerSuite.Run(RanToolBlock, Inputs);
				App.nTodayImageCount++;
				new Thread((ThreadStart)delegate
				{
					UnitDB compactDB3 = base.CompactDB;
					string manufatureTotalNumRecordTable_PerDay2 = App.ManufatureTotalNumRecordTable_PerDay;
					DateTime now2 = DateTime.Now;
					compactDB3.Write(manufatureTotalNumRecordTable_PerDay2, now2.ToString("yyyy-MM-dd"), App.nTodayImageCount.ToString());
					UnitDB compactDB4 = base.CompactDB;
					string nGNumRecordTable_PerDay2 = App.NGNumRecordTable_PerDay;
					now2 = DateTime.Now;
					compactDB4.Write(nGNumRecordTable_PerDay2, now2.ToString("yyyy-MM-dd"), App.nNGNum.ToString());
					DoRefreshManufatureIndicators();
				}).Start();
			}
			else
			{
				lock (LockSoftwareTrigger)
				{
					toolBlockPowerSuite.Run(RanToolBlock, Inputs);
					App.nTodayImageCount++;
					new Thread((ThreadStart)delegate
					{
						UnitDB compactDB = base.CompactDB;
						string manufatureTotalNumRecordTable_PerDay = App.ManufatureTotalNumRecordTable_PerDay;
						DateTime now = DateTime.Now;
						compactDB.Write(manufatureTotalNumRecordTable_PerDay, now.ToString("yyyy-MM-dd"), App.nTodayImageCount.ToString());
						UnitDB compactDB2 = base.CompactDB;
						string nGNumRecordTable_PerDay = App.NGNumRecordTable_PerDay;
						now = DateTime.Now;
						compactDB2.Write(nGNumRecordTable_PerDay, now.ToString("yyyy-MM-dd"), App.nNGNum.ToString());
						DoRefreshManufatureIndicators();
					}).Start();
				}
			}
		}

		public CogRecordDisplay GetCogRecordDisplayWndByIndex(int nIndex)
		{
			return DspWndLayoutManager.DisplayWnds[nIndex] as CogRecordDisplay;
		}

		private Cognex.VisionPro.ICogImage GetOutputImage(object sender)
		{
			Cognex.VisionPro.ICogAcqFifo cogAcqFifo = sender as Cognex.VisionPro.ICogAcqFifo;
			int numPending = 0;
			int numReady = 0;
			bool busy = false;
			cogAcqFifo.GetFifoState(out numPending, out numReady, out busy);
			if (info == null)
			{
				info = new CogAcqInfo();
			}
			if (numReady <= 0)
			{
				return null;
			}
			return cogAcqFifo.CompleteAcquireEx(info);
		}

		private string GetCameraSerialNo(object sender)
		{
			CogAcqFifoGigE cogAcqFifoGigE = sender as CogAcqFifoGigE;
			return cogAcqFifoGigE.FrameGrabber.SerialNumber;
		}

		private string GetCameraUniqueID(object sender)
		{
			CogAcqFifoGigE cogAcqFifoGigE = sender as CogAcqFifoGigE;
			return cogAcqFifoGigE.FrameGrabber.UniqueID;
		}

		private string GetCameraName(object sender)
		{
			CogAcqFifoGigE cogAcqFifoGigE = sender as CogAcqFifoGigE;
			return cogAcqFifoGigE.FrameGrabber.Name;
		}

		private ToolBlockPowerSuite GetToolBlock(int nIndex)
		{
			return App.ThisJobTool.ToolBlockSet[nIndex];
		}

		private CogToolBlock GetInnerToolBlock(int nIndex)
		{
			return GetToolBlock(nIndex).ThisToolBlock;
		}

		public TCPBase InitTCP_IPArgs(bool isServer = true, string ArgsTableName = null)
		{
			TCPBase tCPBase = null;
			if (!isServer)
			{
				TCPFactoryClient tCPFactoryClient = (ArgsTableName != null) ? new TCPFactoryClient(ArgsTableName) : new TCPFactoryClient();
				tCPFactoryClient.Route = ClientRouteMessage;
				tCPFactoryClient.Connect();
				return tCPFactoryClient;
			}
			TCPFactoryServer tCPFactoryServer = (ArgsTableName != null) ? new TCPFactoryServer(ArgsTableName) : new TCPFactoryServer();
			tCPFactoryServer.Route = ServerRouteMessage;
			tCPFactoryServer.Boot();
			return tCPFactoryServer;
		}

		public PLCBase InitSerialPortArgs(bool isNormalPlc = true, string ArgsTableName = null)
		{
			PLCBase pLCBase;
			if (isNormalPlc)
			{
				pLCBase = new NormalPLCEx();
				if (ArgsTableName == null)
				{
					((NormalPLCEx)pLCBase).InitArgs("SerialPort");
				}
				else
				{
					((NormalPLCEx)pLCBase).InitArgs(ArgsTableName);
				}
			}
			else
			{
				pLCBase = new AddressPLCEx();
				if (ArgsTableName == null)
				{
					((AddressPLCEx)pLCBase).InitArgs("SerialPort");
				}
				else
				{
					((AddressPLCEx)pLCBase).InitArgs(ArgsTableName);
				}
			}
			pLCBase.Open();
			return pLCBase;
		}

		public void AutoTrigger(object sender, EventArgs e)
		{
			lock (LockHardwareTrigger)
			{
				string cameraSerialNo = GetCameraSerialNo(sender);
				Cognex.VisionPro.ICogImage outputImage = GetOutputImage(sender);
				HardwareTiggerOccured(cameraSerialNo, outputImage);
			}
		}
        private void SendOK2PLC(int LightOnTime = 100)
        {
            InternalSendStatus2PLC(Y1OPEN, Y1CLOSE, LightOnTime);
        }

        private void SendNG2PLC(int LightOnTime = 100)
        {
            InternalSendStatus2PLC(Y2OPEN, Y2CLOSE, LightOnTime);
        }

        private void InternalSendStatus2PLC(string strHexLightOn, string strHexLightOff, int LightOnTime = 100)
        {
            byte[] bytArr2 = normalPLC.DecodeHexString(strHexLightOn);
            normalPLC.Send(bytArr2);
            Thread.Sleep(LightOnTime);
            bytArr2 = normalPLC.DecodeHexString(strHexLightOff);
            normalPLC.Send(bytArr2);
        }
        private void ShowMenuForm()
		{
			foreach (Form openForm in Application.OpenForms)
			{
				if (openForm is MenuForm)
				{
					if (openForm.Owner == this)
					{
						if (openForm.WindowState == FormWindowState.Minimized)
						{
							openForm.WindowState = FormWindowState.Normal;
						}
						openForm.BringToFront();
					}
					return;
				}
			}
			Form form2 = AppManager.ShowFixedPage(typeof(MenuForm));
			form2.Owner = this;
		}

		public void ClearConsoleText()
		{
			mobileListBox1.Items.Clear();
		}

		private void ShowCompactDBEditor(string dbFileName = "shikii.db")
		{
			AppManager.ShowCompactDBEditor(dbFileName);
		}

		protected override void prepareData()
		{
			base.prepareData();
			string text = base.CompactDB.FetchValue(App.AutoCleanTime, true, "0");
			if (text == null)
			{
				base.CompactDB.Write(App.AutoCleanTime, "0");
			}
			string text2 = base.CompactDB.FetchValue(App.ApplyUserPriority, true, "0");
			if (text2 == null)
			{
				base.CompactDB.Write(App.ApplyUserPriority, "0");
			}
			string text3 = base.CompactDB.FetchValue(App.HideMainForm, true, "0");
			if (text3 == null)
			{
				base.CompactDB.Write(App.HideMainForm, "0");
			}
			string text4 = base.CompactDB.FetchValue(App.Contact, true, "0");
			if (text4 == null)
			{
				base.CompactDB.Write(App.Contact, " ");
			}
			string text5 = base.CompactDB.FetchValue(App.IncName, true, "0");
			if (text5 == null)
			{
				base.CompactDB.Write(App.IncName, " ");
			}
			PrepareCommunication();
			base.CompactDB.GetAllTableNames();
			int num = base.CompactDB.AllTableNames.IndexOf(App.ImageRecord);
			if (num == -1)
			{
				base.CompactDB.NewTable(App.ImageRecord, "图片路径 Text not null ,OKNG Text not null,图像生成日期 Text not null,描述 Text not null");
			}
			num = base.CompactDB.AllTableNames.IndexOf(App.ManufatureTotalNumRecordTable_PerDay);
			if (num == -1)
			{
				base.CompactDB.CopyKeyValueTable(App.ManufatureTotalNumRecordTable_PerDay, base.CompactDB.DefaultTable);
				base.CompactDB.ExecuteNonQuery("delete from " + App.ManufatureTotalNumRecordTable_PerDay, dotNetLab.Data.DBOperator.OPERATOR_TRUNCATE);
			}
			UnitDB compactDB = base.CompactDB;
			DateTime now = DateTime.Now;
			App.nTodayImageCount = compactDB.FetchIntValue(now.ToString("yyyy-MM-dd"), App.ManufatureTotalNumRecordTable_PerDay);
			num = base.CompactDB.AllTableNames.IndexOf(App.NGNumRecordTable_PerDay);
			if (num == -1)
			{
				base.CompactDB.CopyKeyValueTable(App.NGNumRecordTable_PerDay, base.CompactDB.DefaultTable);
				base.CompactDB.ExecuteNonQuery("delete from " + App.NGNumRecordTable_PerDay, dotNetLab.Data.DBOperator.OPERATOR_TRUNCATE);
			}
			UnitDB compactDB2 = base.CompactDB;
			now = DateTime.Now;
			App.nNGNum = compactDB2.FetchIntValue(now.ToString("yyyy-MM-dd"), App.NGNumRecordTable_PerDay);
			num = base.CompactDB.AllTableNames.IndexOf(App.StatisticNGOKPercentTable_PerDay);
			if (num == -1)
			{
				base.CompactDB.CopyKeyValueTable(App.StatisticNGOKPercentTable_PerDay, base.CompactDB.DefaultTable);
				base.CompactDB.ExecuteNonQuery("delete from " + App.StatisticNGOKPercentTable_PerDay, dotNetLab.Data.DBOperator.OPERATOR_TRUNCATE);
			}
		}

		private void CheckProjectFolder()
		{
			if (!Directory.Exists(App.ProjsFolderName))
			{
				Directory.CreateDirectory(App.ProjsFolderName);
				if (!Directory.Exists(App.OriginProjectPath))
				{
					Directory.CreateDirectory(App.OriginProjectPath);
					string text = base.CompactDB.FetchValue(App.CurrentProject, true, "0");
					if (string.IsNullOrEmpty(text) || text.Equals("0"))
					{
						base.CompactDB.Write(App.CurrentProject, App.OriginProjectPath);
					}
				}
			}
		}

		public void PrepareVision()
		{
			AutoTriggerHandler = AutoTrigger;
			App.ThisJobTool = new JobTool();
			App.ThisJobTool.CompactDB.Host = base.CompactDB;
			App.ThisJobTool.ConsolePipe.Host = base.ConsolePipe;
			App.ThisJobTool.LogPipe.Host = base.LogPipe;
			App.ThisJobTool.DisplayWnds = DspWndLayoutManager.dspWndArr;
			App.ThisJobTool.mainFormInvoker.Host = this;
			App.ThisJobTool.Deserialize();
			PrepareCanvases();
		}

		public Form ShowQuickBuildForm()
		{
			string a = base.CompactDB.FetchValue(App.HideMainForm, true, "0");
			if (!(a == "1"))
			{
				if (!(a == "2"))
				{
					return this;
				}
				return App.ShowJobManagerWnd(null);
			}
			Form form = AppManager.ShowFixedPage(typeof(MenuForm));
			form.Owner = this;
			form.FormClosed += delegate
			{
				Close();
			};
			return form;
		}

		protected override void prepareCtrls()
		{
			base.prepareCtrls();
			InitializeComponent();
			DspWndLayoutManager = new DspWndLayout();
			string text;
			while (true)
			{
				text = base.CompactDB.FetchValue("AppName", true, "0");
				if (text != null)
				{
					break;
				}
				base.CompactDB.Write("AppName", "视觉检测应用");
			}
			Text = text;
			if (-99999 == base.CompactDB.FetchIntValue("DisplayWndNum"))
			{
				base.CompactDB.Write("DisplayWndNum", "1");
			}
			DspWndLayoutManager.PrepareDspWnds(typeof(CogRecordDisplay), canvasPanel1, base.CompactDB.FetchIntValue("DisplayWndNum"));
			SpecialInfo();
		}

		protected override void prepareEvents()
		{
			base.prepareEvents();
			base.KeyDown += MainForm_KeyDown;
			base.Load += delegate
			{
				DoRefreshManufatureIndicators();
			};
		}

		private void DoRefreshManufatureIndicators()
		{
			Action<Control, double> method = delegate(Control c, double n)
			{
				c.Text = n.ToString();
			};
			Invoke(method, lbl_TotalNum, App.nTodayImageCount);
			Invoke(method, lbl_NGNum, App.nNGNum);
			double num = 0.0;
			num = ((App.nTodayImageCount == 0) ? 0.0 : ((double)(((float)App.nTodayImageCount - (float)App.nNGNum * 1f) / (float)App.nTodayImageCount)));
			num = Math.Round(num, 3);
			num *= 100.0;
			Invoke(method, lbl_GoodPercent, num);
			base.CompactDB.Write(App.StatisticNGOKPercentTable_PerDay, DateTime.Now.ToString("yyyy-MM-dd"), string.Format("{0}^{1}", num, 100.0 - num));
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == (Keys)131155)
			{
				ShortCutRun();
			}
			if (e.KeyData == (Keys)131139)
			{
				mobileListBox1.Items.Clear();
			}
			if (e.KeyData == (Keys)131146)
			{
				ShowMenuForm();
			}
		}

		private void btn_More_Click(object sender, EventArgs e)
		{
			int num = 0;
			try
			{
				string s = base.CompactDB.FetchValue(App.ApplyUserPriority, true, "0");
				num = int.Parse(s);
			}
			catch (Exception)
			{
				dotNetLab.Tipper.Error = "检测到你可能启用了权限管理,但是可能配置不正确！";
			}
			if (num > 0)
			{
				LogInForm logInForm = new LogInForm();
				logInForm.ShowDialog();
				if (!logInForm.bCloseWindow)
				{
					return;
				}
			}
			foreach (Form openForm in Application.OpenForms)
			{
				if (openForm is MenuForm)
				{
					if (openForm.Owner == this)
					{
						if (openForm.WindowState == FormWindowState.Minimized)
						{
							openForm.WindowState = FormWindowState.Normal;
						}
						openForm.BringToFront();
					}
					return;
				}
			}
			Form form2 = AppManager.ShowFixedPage(typeof(MenuForm));
			form2.Owner = this;
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(shikii.VisionJob.MainForm));
			mobileListBox1 = new dotNetLab.Widgets.MobileListBox();
			lbl_OutputInfo = new dotNetLab.Widgets.TextBlock();
			canvasPanel1 = new dotNetLab.Widgets.Container.CanvasPanel();
			colorDecorator1 = new dotNetLab.Widgets.ColorDecorator();
			btn_More = new dotNetLab.Widgets.Direction();
			label1 = new System.Windows.Forms.Label();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			label2 = new System.Windows.Forms.Label();
			lbl_IncName = new System.Windows.Forms.Label();
			textBlock6 = new dotNetLab.Widgets.TextBlock();
			lbl_GoodPercent = new dotNetLab.Widgets.TextBlock();
			textBlock4 = new dotNetLab.Widgets.TextBlock();
			lbl_NGNum = new dotNetLab.Widgets.TextBlock();
			textBlock2 = new dotNetLab.Widgets.TextBlock();
			lbl_TotalNum = new dotNetLab.Widgets.TextBlock();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			tipper.Location = new System.Drawing.Point(556, 480);
			mobileListBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			mobileListBox1.BackColor = System.Drawing.Color.Transparent;
			mobileListBox1.BorderColor = System.Drawing.Color.Gray;
			mobileListBox1.BorderThickness = 1;
			mobileListBox1.CornerAlignment = dotNetLab.Widgets.Alignments.All;
			mobileListBox1.DataBindingInfo = null;
			mobileListBox1.Font = new System.Drawing.Font("微软雅黑", 11f);
			mobileListBox1.ImagePos = new System.Drawing.Point(0, 0);
			mobileListBox1.ImageSize = new System.Drawing.Size(0, 0);
			mobileListBox1.Location = new System.Drawing.Point(610, 93);
			mobileListBox1.MainBindableProperty = "mobileListBox1";
			mobileListBox1.Name = "mobileListBox1";
			mobileListBox1.NormalColor = System.Drawing.Color.White;
			mobileListBox1.Radius = -1;
			mobileListBox1.Size = new System.Drawing.Size(228, 406);
			mobileListBox1.Source = null;
			mobileListBox1.TabIndex = 2;
			mobileListBox1.Text = "mobileListBox1";
			mobileListBox1.UIElementBinders = null;
			lbl_OutputInfo.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			lbl_OutputInfo.BackColor = System.Drawing.Color.Transparent;
			lbl_OutputInfo.BorderColor = System.Drawing.Color.Empty;
			lbl_OutputInfo.BorderThickness = -1;
			lbl_OutputInfo.DataBindingInfo = null;
			lbl_OutputInfo.EnableFlag = true;
			lbl_OutputInfo.EnableTextRenderHint = true;
			lbl_OutputInfo.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			lbl_OutputInfo.FlagColor = System.Drawing.Color.Crimson;
			lbl_OutputInfo.FlagThickness = 10;
			lbl_OutputInfo.Font = new System.Drawing.Font("微软雅黑", 12f);
			lbl_OutputInfo.GapBetweenTextFlag = 0;
			lbl_OutputInfo.LEDStyle = false;
			lbl_OutputInfo.Location = new System.Drawing.Point(612, 71);
			lbl_OutputInfo.MainBindableProperty = "输出信息";
			lbl_OutputInfo.Name = "lbl_OutputInfo";
			lbl_OutputInfo.Radius = -1;
			lbl_OutputInfo.Size = new System.Drawing.Size(104, 16);
			lbl_OutputInfo.TabIndex = 4;
			lbl_OutputInfo.Text = "输出信息";
			lbl_OutputInfo.UIElementBinders = null;
			lbl_OutputInfo.UnderLine = false;
			lbl_OutputInfo.UnderLineColor = System.Drawing.Color.DarkGray;
			lbl_OutputInfo.UnderLineThickness = 2f;
			lbl_OutputInfo.Vertical = false;
			lbl_OutputInfo.WhereReturn = 0;
			canvasPanel1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			canvasPanel1.BackColor = System.Drawing.Color.Transparent;
			canvasPanel1.BorderColor = System.Drawing.Color.Empty;
			canvasPanel1.BorderThickness = -1;
			canvasPanel1.CornerAlignment = dotNetLab.Widgets.Alignments.All;
			canvasPanel1.DataBindingInfo = null;
			canvasPanel1.Font = new System.Drawing.Font("微软雅黑", 11f);
			canvasPanel1.ImagePos = new System.Drawing.Point(0, 0);
			canvasPanel1.ImageSize = new System.Drawing.Size(0, 0);
			canvasPanel1.Location = new System.Drawing.Point(36, 132);
			canvasPanel1.MainBindableProperty = null;
			canvasPanel1.Name = "canvasPanel1";
			canvasPanel1.NormalColor = System.Drawing.Color.Silver;
			canvasPanel1.Radius = 20;
			canvasPanel1.Size = new System.Drawing.Size(554, 367);
			canvasPanel1.Source = null;
			canvasPanel1.TabIndex = 5;
			canvasPanel1.Text = null;
			canvasPanel1.UIElementBinders = null;
			colorDecorator1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			colorDecorator1.BackColor = System.Drawing.Color.White;
			colorDecorator1.DataBindingInfo = null;
			colorDecorator1.Location = new System.Drawing.Point(6, 505);
			colorDecorator1.MainBindableProperty = "";
			colorDecorator1.Name = "colorDecorator1";
			colorDecorator1.Size = new System.Drawing.Size(150, 53);
			colorDecorator1.TabIndex = 6;
			colorDecorator1.UIElementBinders = null;
			btn_More.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			btn_More.ArrowAlignment = dotNetLab.Widgets.Alignments.Right;
			btn_More.BackColor = System.Drawing.Color.Transparent;
			btn_More.BorderColor = System.Drawing.Color.Gray;
			btn_More.BorderThickness = 2f;
			btn_More.CenterImage = true;
			btn_More.ClipCircleRegion = false;
			btn_More.DataBindingInfo = null;
			btn_More.Effect = null;
			btn_More.Fill = false;
			btn_More.FillColor = System.Drawing.Color.Empty;
			btn_More.ImagePostion = new System.Drawing.Point(0, 0);
			btn_More.ImageSize = new System.Drawing.SizeF(25f, 25f);
			btn_More.Location = new System.Drawing.Point(797, 51);
			btn_More.MainBindableProperty = "";
			btn_More.MouseDownColor = System.Drawing.Color.Gray;
			btn_More.Name = "btn_More";
			btn_More.NeedEffect = false;
			btn_More.Size = new System.Drawing.Size(40, 40);
			btn_More.Source = (System.Drawing.Image)componentResourceManager.GetObject("btn_More.Source");
			btn_More.TabIndex = 7;
			btn_More.UIElementBinders = null;
			btn_More.WhichShap = 6;
			btn_More.WhitePattern = false;
			btn_More.Click += new System.EventHandler(btn_More_Click);
			label1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("微软雅黑", 11f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label1.ForeColor = System.Drawing.Color.Teal;
			label1.Location = new System.Drawing.Point(179, 530);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(54, 20);
			label1.TabIndex = 8;
			label1.Text = "联系人";
			pictureBox1.Location = new System.Drawing.Point(9, 12);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(52, 47);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 9;
			pictureBox1.TabStop = false;
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("微软雅黑", 11f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label2.ForeColor = System.Drawing.Color.Teal;
			label2.Location = new System.Drawing.Point(316, 39);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(27, 20);
			label2.TabIndex = 8;
			label2.Text = "by";
			lbl_IncName.AutoSize = true;
			lbl_IncName.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			lbl_IncName.ForeColor = System.Drawing.Color.Crimson;
			lbl_IncName.Location = new System.Drawing.Point(349, 39);
			lbl_IncName.Name = "lbl_IncName";
			lbl_IncName.Size = new System.Drawing.Size(0, 21);
			lbl_IncName.TabIndex = 10;
			textBlock6.Anchor = System.Windows.Forms.AnchorStyles.Top;
			textBlock6.BackColor = System.Drawing.Color.Transparent;
			textBlock6.BorderColor = System.Drawing.Color.Empty;
			textBlock6.BorderThickness = -1;
			textBlock6.DataBindingInfo = null;
			textBlock6.EnableFlag = false;
			textBlock6.EnableTextRenderHint = false;
			textBlock6.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock6.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock6.FlagThickness = 5;
			textBlock6.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock6.GapBetweenTextFlag = 10;
			textBlock6.LEDStyle = false;
			textBlock6.Location = new System.Drawing.Point(434, 91);
			textBlock6.MainBindableProperty = "合格率:";
			textBlock6.Name = "textBlock6";
			textBlock6.Radius = -1;
			textBlock6.Size = new System.Drawing.Size(77, 35);
			textBlock6.TabIndex = 16;
			textBlock6.Text = "合格率:";
			textBlock6.UIElementBinders = null;
			textBlock6.UnderLine = false;
			textBlock6.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock6.UnderLineThickness = 2f;
			textBlock6.Vertical = false;
			textBlock6.WhereReturn = 0;
			lbl_GoodPercent.Anchor = System.Windows.Forms.AnchorStyles.Top;
			lbl_GoodPercent.BackColor = System.Drawing.Color.Transparent;
			lbl_GoodPercent.BorderColor = System.Drawing.Color.Empty;
			lbl_GoodPercent.BorderThickness = -1;
			lbl_GoodPercent.DataBindingInfo = null;
			lbl_GoodPercent.EnableFlag = false;
			lbl_GoodPercent.EnableTextRenderHint = false;
			lbl_GoodPercent.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			lbl_GoodPercent.FlagColor = System.Drawing.Color.DodgerBlue;
			lbl_GoodPercent.FlagThickness = 5;
			lbl_GoodPercent.Font = new System.Drawing.Font("DS-Digital", 30f);
			lbl_GoodPercent.ForeColor = System.Drawing.Color.SeaGreen;
			lbl_GoodPercent.GapBetweenTextFlag = 10;
			lbl_GoodPercent.LEDStyle = true;
			lbl_GoodPercent.Location = new System.Drawing.Point(513, 91);
			lbl_GoodPercent.MainBindableProperty = "100.2";
			lbl_GoodPercent.Name = "lbl_GoodPercent";
			lbl_GoodPercent.Radius = -1;
			lbl_GoodPercent.Size = new System.Drawing.Size(76, 35);
			lbl_GoodPercent.TabIndex = 13;
			lbl_GoodPercent.Text = "100.2";
			lbl_GoodPercent.UIElementBinders = null;
			lbl_GoodPercent.UnderLine = false;
			lbl_GoodPercent.UnderLineColor = System.Drawing.Color.DarkGray;
			lbl_GoodPercent.UnderLineThickness = 2f;
			lbl_GoodPercent.Vertical = false;
			lbl_GoodPercent.WhereReturn = 0;
			textBlock4.Anchor = System.Windows.Forms.AnchorStyles.Top;
			textBlock4.BackColor = System.Drawing.Color.Transparent;
			textBlock4.BorderColor = System.Drawing.Color.Empty;
			textBlock4.BorderThickness = -1;
			textBlock4.DataBindingInfo = null;
			textBlock4.EnableFlag = false;
			textBlock4.EnableTextRenderHint = false;
			textBlock4.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock4.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock4.FlagThickness = 5;
			textBlock4.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock4.GapBetweenTextFlag = 10;
			textBlock4.LEDStyle = false;
			textBlock4.Location = new System.Drawing.Point(257, 91);
			textBlock4.MainBindableProperty = "不合格数:";
			textBlock4.Name = "textBlock4";
			textBlock4.Radius = -1;
			textBlock4.Size = new System.Drawing.Size(77, 35);
			textBlock4.TabIndex = 17;
			textBlock4.Text = "不合格数:";
			textBlock4.UIElementBinders = null;
			textBlock4.UnderLine = false;
			textBlock4.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock4.UnderLineThickness = 2f;
			textBlock4.Vertical = false;
			textBlock4.WhereReturn = 0;
			lbl_NGNum.Anchor = System.Windows.Forms.AnchorStyles.Top;
			lbl_NGNum.BackColor = System.Drawing.Color.Transparent;
			lbl_NGNum.BorderColor = System.Drawing.Color.Empty;
			lbl_NGNum.BorderThickness = -1;
			lbl_NGNum.DataBindingInfo = null;
			lbl_NGNum.EnableFlag = false;
			lbl_NGNum.EnableTextRenderHint = false;
			lbl_NGNum.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			lbl_NGNum.FlagColor = System.Drawing.Color.DodgerBlue;
			lbl_NGNum.FlagThickness = 5;
			lbl_NGNum.Font = new System.Drawing.Font("DS-Digital", 30f);
			lbl_NGNum.ForeColor = System.Drawing.Color.Crimson;
			lbl_NGNum.GapBetweenTextFlag = 10;
			lbl_NGNum.LEDStyle = true;
			lbl_NGNum.Location = new System.Drawing.Point(336, 91);
			lbl_NGNum.MainBindableProperty = "9";
			lbl_NGNum.Name = "lbl_NGNum";
			lbl_NGNum.Radius = -1;
			lbl_NGNum.Size = new System.Drawing.Size(92, 35);
			lbl_NGNum.TabIndex = 14;
			lbl_NGNum.Text = "9";
			lbl_NGNum.UIElementBinders = null;
			lbl_NGNum.UnderLine = false;
			lbl_NGNum.UnderLineColor = System.Drawing.Color.DarkGray;
			lbl_NGNum.UnderLineThickness = 2f;
			lbl_NGNum.Vertical = false;
			lbl_NGNum.WhereReturn = 0;
			textBlock2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			textBlock2.BackColor = System.Drawing.Color.Transparent;
			textBlock2.BorderColor = System.Drawing.Color.Empty;
			textBlock2.BorderThickness = -1;
			textBlock2.DataBindingInfo = null;
			textBlock2.EnableFlag = false;
			textBlock2.EnableTextRenderHint = false;
			textBlock2.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			textBlock2.FlagColor = System.Drawing.Color.DodgerBlue;
			textBlock2.FlagThickness = 5;
			textBlock2.Font = new System.Drawing.Font("微软雅黑", 12f);
			textBlock2.GapBetweenTextFlag = 10;
			textBlock2.LEDStyle = false;
			textBlock2.Location = new System.Drawing.Point(36, 91);
			textBlock2.MainBindableProperty = "已经生产 :";
			textBlock2.Name = "textBlock2";
			textBlock2.Radius = -1;
			textBlock2.Size = new System.Drawing.Size(77, 35);
			textBlock2.TabIndex = 18;
			textBlock2.Text = "已经生产 :";
			textBlock2.UIElementBinders = null;
			textBlock2.UnderLine = false;
			textBlock2.UnderLineColor = System.Drawing.Color.DarkGray;
			textBlock2.UnderLineThickness = 2f;
			textBlock2.Vertical = false;
			textBlock2.WhereReturn = 0;
			lbl_TotalNum.Anchor = System.Windows.Forms.AnchorStyles.Top;
			lbl_TotalNum.BackColor = System.Drawing.Color.Transparent;
			lbl_TotalNum.BorderColor = System.Drawing.Color.Empty;
			lbl_TotalNum.BorderThickness = -1;
			lbl_TotalNum.DataBindingInfo = null;
			lbl_TotalNum.EnableFlag = false;
			lbl_TotalNum.EnableTextRenderHint = false;
			lbl_TotalNum.FlagAlign = dotNetLab.Widgets.Alignments.Left;
			lbl_TotalNum.FlagColor = System.Drawing.Color.DodgerBlue;
			lbl_TotalNum.FlagThickness = 5;
			lbl_TotalNum.Font = new System.Drawing.Font("DS-Digital", 30f);
			lbl_TotalNum.ForeColor = System.Drawing.Color.DodgerBlue;
			lbl_TotalNum.GapBetweenTextFlag = 10;
			lbl_TotalNum.LEDStyle = true;
			lbl_TotalNum.Location = new System.Drawing.Point(119, 91);
			lbl_TotalNum.MainBindableProperty = "0";
			lbl_TotalNum.Name = "lbl_TotalNum";
			lbl_TotalNum.Radius = -1;
			lbl_TotalNum.Size = new System.Drawing.Size(141, 35);
			lbl_TotalNum.TabIndex = 15;
			lbl_TotalNum.Text = "0";
			lbl_TotalNum.UIElementBinders = null;
			lbl_TotalNum.UnderLine = false;
			lbl_TotalNum.UnderLineColor = System.Drawing.Color.DarkGray;
			lbl_TotalNum.UnderLineThickness = 2f;
			lbl_TotalNum.Vertical = false;
			lbl_TotalNum.WhereReturn = 0;
			base.ClientSize = new System.Drawing.Size(859, 565);
			base.ClipboardText = "";
			base.Controls.Add(textBlock6);
			base.Controls.Add(lbl_GoodPercent);
			base.Controls.Add(textBlock4);
			base.Controls.Add(lbl_NGNum);
			base.Controls.Add(textBlock2);
			base.Controls.Add(lbl_TotalNum);
			base.Controls.Add(lbl_IncName);
			base.Controls.Add(pictureBox1);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.Controls.Add(btn_More);
			base.Controls.Add(colorDecorator1);
			base.Controls.Add(canvasPanel1);
			base.Controls.Add(lbl_OutputInfo);
			base.Controls.Add(mobileListBox1);
			base.FontX = new System.Drawing.Font("等线 Light", 30f);
			base.KeyPreview = true;
			base.Name = "MainForm";
			Text = "LED支架检测";
			base.TitlePos = new System.Drawing.Point(60, 18);
			base.Controls.SetChildIndex(mobileListBox1, 0);
			base.Controls.SetChildIndex(lbl_OutputInfo, 0);
			base.Controls.SetChildIndex(canvasPanel1, 0);
			base.Controls.SetChildIndex(colorDecorator1, 0);
			base.Controls.SetChildIndex(btn_More, 0);
			base.Controls.SetChildIndex(label1, 0);
			base.Controls.SetChildIndex(label2, 0);
			base.Controls.SetChildIndex(pictureBox1, 0);
			base.Controls.SetChildIndex(lbl_IncName, 0);
			base.Controls.SetChildIndex(lbl_TotalNum, 0);
			base.Controls.SetChildIndex(textBlock2, 0);
			base.Controls.SetChildIndex(lbl_NGNum, 0);
			base.Controls.SetChildIndex(textBlock4, 0);
			base.Controls.SetChildIndex(lbl_GoodPercent, 0);
			base.Controls.SetChildIndex(textBlock6, 0);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		private void SpecialInfo()
		{
			base.EnableDrawUpDownPattern = true;
			base.Img_Up = UI.RibbonSpring;
			base.Img_Down = UI.RibbonUnderwater;
			base.Load += delegate
			{
				MaxWindow();
			};
			pictureBox1.Image = Image.FromFile("App.png");
			string text = base.CompactDB.FetchValue(App.Contact, true, "0");
			label1.Text = text;
			string text2 = base.CompactDB.FetchValue(App.IncName, true, "0");
			lbl_IncName.Text = text2;
		}
	}
}
