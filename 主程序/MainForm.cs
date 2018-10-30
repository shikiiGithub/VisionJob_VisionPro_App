using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dotNetLab.Common.ModernUI;
using dotNetLab.Common;
using System.Threading;
using dotNetLab.Vision.VPro;
using Cognex.VisionPro;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.Blob;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
 
using dotNetLab;

namespace shikii.VisionJob
{
    public partial class MainForm : dotNetLab.Common.ModernUI.PageBase
    {

       // 显示相关信息请启用下列代码
        //  Canvas[] cnvs;
         Canvas cnvs;
        TCPFactoryServer factoryServer;
        public  dotNetLab.Vision.DspWndLayout DspWndLayoutManager;

        protected override void prepareData()
        {
            base.prepareData();

            //配置权限
            String str = CompactDB.FetchValue(App.AutoCleanTime);
            if (str == null)
                CompactDB.Write(App.AutoCleanTime, "0");
            String ApplyUserPriority = CompactDB.FetchValue(App.ApplyUserPriority);
            if (ApplyUserPriority == null)
                CompactDB.Write(App.ApplyUserPriority, "0");

            //to do 准备通讯处理
            //提供默认的网络配置窗口，但是只能配置一个TCP/IP对象
            //factoryServer = new TCPFactoryServer();
            //提供默认的网络配置窗口，可以配置多个对象，因为可以为其指定表
            //factoryServer = new TCPFactoryServer("包含配置信息的表");
            //启动网络服务
            //factoryServer.Boot();
            //开始轮询
            //factoryServer.Route = (nWhichClient, byts) =>
            //{
                 //写通信逻辑代码
            //};

        }

        //检查是否存在默认的项目文件夹
        void CheckProjectFolder()
        {
            if (!Directory.Exists(App.ProjsFolderName))
            {
                Directory.CreateDirectory(App.ProjsFolderName);
                if (!Directory.Exists(App.OriginProjectPath))
                {
                    Directory.CreateDirectory(App.OriginProjectPath);
                    String str = CompactDB.FetchValue(App.CurrentProject);
                    if (String.IsNullOrEmpty(str) || str.Equals("0"))
                    {
                        CompactDB.Write(App.CurrentProject, App.OriginProjectPath);
                    }
                }

            }
        }

        // to do 准备视觉库
        public void PrepareVision()
        {

            String ThisAppDir = Path.GetDirectoryName(Application.ExecutablePath);
            String currentProjectShortPath = CompactDB.FetchValue("Current_Project");
            String AbsoluteCurrentProjectPath = Path.Combine(ThisAppDir, currentProjectShortPath);
            //如果不是数组
             cnvs = new Canvas();
            //如果是数组，一个 Canvas 对象对应一个显示窗口
            //cnvs = new Canvas[n];
            //for (int i = 0; i < cnvs.Length; i++)
            //{
            //    cnvs[i] = new Canvas();
            //}
            //如果不是数组，如果多个vpp 则定义多个,
            //记得给App.CurrentToolBlock赋值
               App.thisToolBlockSuite = this.PrepareToolBlockPowerSuit(AbsoluteCurrentProjectPath, cnvs);
       
            //如果是数组，，如果多个vpp 则定义多个,
            //记得给App.CurrentToolBlock赋值
            //App.thisPowerSuite = this.PrepareToolBlockPowerSuitEx(AbsoluteCurrentProjectPath + "你的vpp名（只包含名称和后缀名）.vpp", cnvs);
            //添加窗体

        }
        protected override void prepareCtrls()
        {
            base.prepareCtrls();
            InitializeComponent();
            DspWndLayoutManager = new dotNetLab.Vision.DspWndLayout();

            cc:;
            String str = CompactDB.FetchValue("AppName");
            if (str == null)
            {
                CompactDB.Write("AppName", "视觉检测应用");
                goto cc;
            }
            this.Text = str;
            if (-99999 == CompactDB.FetchIntValue("DisplayWndNum"))
                CompactDB.Write("DisplayWndNum", "1");

            DspWndLayoutManager.PrepareDspWnds(typeof(CogRecordDisplay), this.canvasPanel1, CompactDB.FetchIntValue("DisplayWndNum"));
            this.Load += (sender, e) =>
            {
                //必须使用这个方法来最大化窗体
                this.MaxWindow();
            };
           
        }
        protected override void prepareEvents()
        {
            base.prepareEvents();
            this.KeyDown += MainForm_KeyDown;
            this.EnableDrawUpDownPattern = true;
            this.Img_Up = dotNetLab.UI.RibbonSpring;
            this.Img_Down = dotNetLab.UI.RibbonUnderwater;
            this.FormClosing += (s, e) =>
            {
                Process.GetCurrentProcess().Kill();
            };
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.S))
            {
               // MainCheckLEDSupporting();
            }
            if (e.KeyData == (Keys.Control | Keys.C))
            {
                this.mobileListBox1.Items.Clear();
            }
            if(e.KeyData==(Keys.Control|Keys.J))
            {
                ShowMenuForm();
            }
        }
      

        void ShowMenuForm()
        {
            foreach (Form item in Application.OpenForms)
            {
                if (item is MenuForm)
                {
                    if (item.Owner != this)
                        return;
                    if (item.WindowState == FormWindowState.Minimized)
                        item.WindowState = FormWindowState.Normal;
                    item.BringToFront();
                    return;
                }
            }
            Form frm = AppManager.ShowFixedPage(typeof(MenuForm));
            frm.Owner = this;
        }
        //要使MenuForm 自动清理文本框正常显示请启用下列代码
        protected void AutoSaveClearImage(Bitmap bmp)
        {
            //自动保存及清理图片
            //保存
            List<String> lst = CompactDB.GetNameColumnValues(CompactDB.DefaultTable);
            if (!lst.Contains("AutoClearTime"))
            {
                CompactDB.Write("AutoClearTime", "3");
            }


            String picturePath = "图片";
            if (!Directory.Exists("图片"))
            {
                Directory.CreateDirectory(picturePath);
            }
            //当前图片保存到哪个位置
            String strNowPictureToGo = String.Format("图片\\{0}", DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(strNowPictureToGo))
            {
                Directory.CreateDirectory(strNowPictureToGo);
            }
            bmp.Save(Path.Combine(strNowPictureToGo, DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".bmp"));
            // bmp.Save( DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".bmp");
            int nGapDays = CompactDB.FetchIntValue("AutoClearTime");
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(-nGapDays);
            string deletingFolderName = dt.ToString("yyyy-MM-dd");
            if (!Directory.Exists(Path.Combine(picturePath, deletingFolderName)))
                return;
            string directoryName = Path.Combine(picturePath, deletingFolderName);
            String[] strFiles = Directory.GetFiles(directoryName);
            for (int j = 0; j < strFiles.Length; j++)
            {
                File.Delete(strFiles[j]);
            }
            Directory.Delete(directoryName);
        }
        private void btn_More_Click(object sender, EventArgs e)
        {
            int n = 0;
            try
            {
                String ApplyUserPriority = CompactDB.FetchValue(App.ApplyUserPriority);
                n = int.Parse(ApplyUserPriority);

            }
            catch (Exception ex)
            {

               dotNetLab.Tipper.Error = "检测到你可能启用了权限管理,但是可能配置不正确！";
            }
            if (n > 0)
            {
                LogInForm logIn = new LogInForm();
                logIn.ShowDialog();
                if (!logIn.bCloseWindow)
                    return;
            }

            foreach (Form item in Application.OpenForms)
            {
                if (item is MenuForm)
                {
                    if (item.Owner != this)
                        return;
                    if (item.WindowState == FormWindowState.Minimized)
                        item.WindowState = FormWindowState.Normal;
                    item.BringToFront();
                    return;
                }
            }
            Form frm = AppManager.ShowFixedPage(typeof(MenuForm));
            frm.Owner = this;
        }


        private dotNetLab.Widgets.TextBlock lbl_OutputInfo;
        private dotNetLab.Widgets.Container.CanvasPanel canvasPanel1;
        private dotNetLab.Widgets.ColorDecorator colorDecorator1;
        public dotNetLab.Widgets.MobileListBox mobileListBox1;
        private dotNetLab.Widgets.Direction btn_More;
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mobileListBox1 = new dotNetLab.Widgets.MobileListBox();
            this.lbl_OutputInfo = new dotNetLab.Widgets.TextBlock();
            this.canvasPanel1 = new dotNetLab.Widgets.Container.CanvasPanel();
            this.colorDecorator1 = new dotNetLab.Widgets.ColorDecorator();
            this.btn_More = new dotNetLab.Widgets.Direction();
            this.SuspendLayout();
            // 
            // tipper
            // 
            this.tipper.Location = new System.Drawing.Point(556, 480);
            // 
            // mobileListBox1
            // 
            this.mobileListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mobileListBox1.BackColor = System.Drawing.Color.Transparent;
            this.mobileListBox1.BorderColor = System.Drawing.Color.Gray;
            this.mobileListBox1.BorderThickness = 1;
            this.mobileListBox1.CornerAlignment = dotNetLab.Widgets.Alignments.All;
            this.mobileListBox1.DataBindingInfo = null;
            this.mobileListBox1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.mobileListBox1.ImagePos = new System.Drawing.Point(0, 0);
            this.mobileListBox1.ImageSize = new System.Drawing.Size(0, 0);
            this.mobileListBox1.Location = new System.Drawing.Point(610, 93);
            this.mobileListBox1.MainBindableProperty = "mobileListBox1";
            this.mobileListBox1.Name = "mobileListBox1";
            this.mobileListBox1.NormalColor = System.Drawing.Color.White;
            this.mobileListBox1.Radius = -1;
            this.mobileListBox1.Size = new System.Drawing.Size(228, 448);
            this.mobileListBox1.Source = null;
            this.mobileListBox1.TabIndex = 2;
            this.mobileListBox1.Text = "mobileListBox1";
            this.mobileListBox1.UIElementBinders = null;
            // 
            // lbl_OutputInfo
            // 
            this.lbl_OutputInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_OutputInfo.BackColor = System.Drawing.Color.Transparent;
            this.lbl_OutputInfo.BorderColor = System.Drawing.Color.Empty;
            this.lbl_OutputInfo.BorderThickness = -1;
            this.lbl_OutputInfo.DataBindingInfo = null;
            this.lbl_OutputInfo.EnableFlag = true;
            this.lbl_OutputInfo.EnableTextRenderHint = true;
            this.lbl_OutputInfo.FlagAlign = dotNetLab.Widgets.Alignments.Left;
            this.lbl_OutputInfo.FlagColor = System.Drawing.Color.Crimson;
            this.lbl_OutputInfo.FlagThickness = 10;
            this.lbl_OutputInfo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lbl_OutputInfo.GapBetweenTextFlag = 0;
            this.lbl_OutputInfo.LEDStyle = false;
            this.lbl_OutputInfo.Location = new System.Drawing.Point(612, 71);
            this.lbl_OutputInfo.MainBindableProperty = "输出信息";
            this.lbl_OutputInfo.Name = "lbl_OutputInfo";
            this.lbl_OutputInfo.Radius = -1;
            this.lbl_OutputInfo.Size = new System.Drawing.Size(104, 16);
            this.lbl_OutputInfo.TabIndex = 4;
            this.lbl_OutputInfo.Text = "输出信息";
            this.lbl_OutputInfo.UIElementBinders = null;
            this.lbl_OutputInfo.UnderLine = false;
            this.lbl_OutputInfo.UnderLineColor = System.Drawing.Color.DarkGray;
            this.lbl_OutputInfo.UnderLineThickness = 2F;
            this.lbl_OutputInfo.Vertical = false;
            this.lbl_OutputInfo.WhereReturn = ((byte)(0));
            // 
            // canvasPanel1
            // 
            this.canvasPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvasPanel1.BackColor = System.Drawing.Color.Transparent;
            this.canvasPanel1.BorderColor = System.Drawing.Color.Empty;
            this.canvasPanel1.BorderThickness = -1;
            this.canvasPanel1.CornerAlignment = dotNetLab.Widgets.Alignments.All;
            this.canvasPanel1.DataBindingInfo = null;
            this.canvasPanel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.canvasPanel1.ImagePos = new System.Drawing.Point(0, 0);
            this.canvasPanel1.ImageSize = new System.Drawing.Size(0, 0);
            this.canvasPanel1.Location = new System.Drawing.Point(36, 85);
            this.canvasPanel1.MainBindableProperty = null;
            this.canvasPanel1.Name = "canvasPanel1";
            this.canvasPanel1.NormalColor = System.Drawing.Color.Silver;
            this.canvasPanel1.Radius = 20;
            this.canvasPanel1.Size = new System.Drawing.Size(554, 414);
            this.canvasPanel1.Source = null;
            this.canvasPanel1.TabIndex = 5;
            this.canvasPanel1.Text = null;
            this.canvasPanel1.UIElementBinders = null;
            // 
            // colorDecorator1
            // 
            this.colorDecorator1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.colorDecorator1.BackColor = System.Drawing.Color.White;
            this.colorDecorator1.DataBindingInfo = null;
            this.colorDecorator1.Location = new System.Drawing.Point(6, 505);
            this.colorDecorator1.MainBindableProperty = "";
            this.colorDecorator1.Name = "colorDecorator1";
            this.colorDecorator1.Size = new System.Drawing.Size(150, 53);
            this.colorDecorator1.TabIndex = 6;
            this.colorDecorator1.UIElementBinders = null;
            // 
            // btn_More
            // 
            this.btn_More.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_More.ArrowAlignment = dotNetLab.Widgets.Alignments.Right;
            this.btn_More.BackColor = System.Drawing.Color.Transparent;
            this.btn_More.BorderColor = System.Drawing.Color.Gray;
            this.btn_More.BorderThickness = 2F;
            this.btn_More.CenterImage = true;
            this.btn_More.ClipCircleRegion = false;
            this.btn_More.DataBindingInfo = null;
            this.btn_More.Effect = null;
            this.btn_More.Fill = false;
            this.btn_More.FillColor = System.Drawing.Color.Empty;
            this.btn_More.ImagePostion = new System.Drawing.Point(0, 0);
            this.btn_More.ImageSize = new System.Drawing.SizeF(25F, 25F);
            this.btn_More.Location = new System.Drawing.Point(797, 51);
            this.btn_More.MainBindableProperty = "";
            this.btn_More.MouseDownColor = System.Drawing.Color.Gray;
            this.btn_More.Name = "btn_More";
            this.btn_More.NeedEffect = false;
            this.btn_More.Size = new System.Drawing.Size(40, 40);
            this.btn_More.Source = ((System.Drawing.Image)(resources.GetObject("btn_More.Source")));
            this.btn_More.TabIndex = 7;
            this.btn_More.UIElementBinders = null;
            this.btn_More.WhichShap = 6;
            this.btn_More.WhitePattern = false;
            this.btn_More.Click += new System.EventHandler(this.btn_More_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(859, 565);
            this.Controls.Add(this.btn_More);
            this.Controls.Add(this.colorDecorator1);
            this.Controls.Add(this.canvasPanel1);
            this.Controls.Add(this.lbl_OutputInfo);
            this.Controls.Add(this.mobileListBox1);
            this.FontX = new System.Drawing.Font("等线 Light", 30F);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "LED支架检测";
            this.TitlePos = new System.Drawing.Point(10, 18);
            this.Controls.SetChildIndex(this.mobileListBox1, 0);
            this.Controls.SetChildIndex(this.lbl_OutputInfo, 0);
            this.Controls.SetChildIndex(this.canvasPanel1, 0);
            this.Controls.SetChildIndex(this.colorDecorator1, 0);
            this.Controls.SetChildIndex(this.btn_More, 0);
            this.ResumeLayout(false);

        }
    }
}

//使用范例
/*
     public  void MainCheckLEDSupporting()
        {
            App.thisPowerSuite.Run("CogFixtureTool2.OutputImage",
                   cnv, this, (ir, obj) =>
                   {
                       pnts_Results.Clear();
                       ToolBlockPowerSuite.DisplayResultImage
                        (App.DspWndLayoutManager.DisplayWnds[0] as CogRecordDisplay, ir, cnv);
                      
          
                       //取出输出图片
                       CogImage8Grey img = App.thisPowerSuite.RunOutPuts[0] as CogImage8Grey;
                     

                       int nImageSizeWidth = img.Width;
                       int nImageSizeHeight = img.Height;
                       int nUnitSizeWidth = img.Width / 22;
                       int nUnitSizeHeight = img.Height / 22;

                       AutoSaveClearImage(img.ToBitmap());

                       if (cnv.Labels[0].Text == "OK")
                       {

                           byte[] bytArry = new byte[2] { 0, 0 };
                           factoryServer.Send_Mill(0, bytArry);
                           return;
                       }

                       //拿到Blob分析结果
                       CogBlobResults results = App.thisPowerSuite.RunOutPuts[1] as CogBlobResults;
                       //得到的Blob 数
                       int nNum = results.GetBlobs().Count;
                       //初始化容器
                       CheckPoint[] checkPoints = new CheckPoint[22 * 22];
                      
                       //行
                       for (int i = 0; i < 22; i++)
                       {
                           //列
                           for (int j = 0; j < 22; j++)
                           {
                               checkPoints[i * 22 + j] = new CheckPoint();
                               //容器中的每个元素储存为该元素的行，列值
                               checkPoints[i * 22 + j].pnt = new Point(i, j);
                           }
                       }
                        
                       //从Blob 分析结果中得到每个正常槽的对应的行列
                       for (int i = 0; i < nNum; i++)
                       {

                           int nRow = -1;
                           int nColumn = -1;
                           double X = (int)results.GetBlobs()[i].CenterOfMassX;
                           int Y = (int)results.GetBlobs()[i].CenterOfMassY;
                           double nX = X / nUnitSizeWidth;
                           if (X % nUnitSizeWidth == 0)
                               nColumn = (int)nX;
                           else
                               nColumn = (int)(nX + 1);

                           double nY = Y / nUnitSizeHeight;
                           if (Y % nUnitSizeHeight == 0)
                               nRow = (int)nY;
                           else
                               nRow = (int)(nY + 1);
                           checkPoints[(nRow - 1) * 22 + (nColumn - 1)].isEmpty = false;
                       }
                       //收集行列处槽是否为空的坐标。最终是要得到有缺陷的槽的行列值
                       //行
                       for (int i = 0; i < 22; i++)
                       {
                           //列
                           for (int j = 0; j < 22; j++)
                           {
                               if (checkPoints[i * 22 + j].isEmpty)
                               {
                                   //使用List 来存储缺陷槽的行列
                                   pnts_Results.Add(new Point(i +1, j+1));
                                  
                               }
                           }

                       }
                       //显示输出信息
                       ConsolePipe.Error(String.Format("发现{0}个问题", pnts_Results.Count));
                       for (int i = 0; i < pnts_Results.Count; i++)
                       {
                           ConsolePipe.Info("行，列 ：" + pnts_Results[i].X.ToString() + "," + pnts_Results[i].Y.ToString());
                       }
                       //发送到PLC
                       //StringBuilder sb = new StringBuilder();
                       //sb.Append(0.ToString("X2"));
                       //for (int i = 0; i < pnts_Results.Count; i++)
                       //{
                       //    sb.Append(pnts_Results[i].X.ToString("X2") + pnts_Results[i].Y.ToString("X2"));
                       //}
                       //byte[] bytArr = factoryServer.TextEncode.GetBytes(sb.ToString());

                       byte[] bytArray = new byte[pnts_Results.Count * 2 +1];
                       bytArray[0] = 0;
                       for (int i = 1; i < bytArray.Length; i +=2)
                       {
                           // sb.Append(pnts_Results[i].X.ToString("X2") + pnts_Results[i].Y.ToString("X2"));
                           bytArray[i] = (byte)pnts_Results[(i-1)/2].X;
                           bytArray [i+1] = (byte)pnts_Results[(i -1)/2].Y;

                       }

                       factoryServer.Send_Mill(0, bytArray);
                   }
                 );
        }
     
     */
