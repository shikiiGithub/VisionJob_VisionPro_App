using dotNetLab.Common;
using dotNetLab.Vision.VPro;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace shikii.VisionJob
{
	public class App
	{
		public static JobTool ThisJobTool;

		public static MainForm frm;

		public static JobToolEditV2 EditV2;

		public static string CAMERSHOT = "Snap";

		public static int nCurrentBorder = 0;

		public static string ProjsFolderName = "Projs";

		public static string OriginProjectPath = "Projs\\0";

		public static string CurrentProject = "Current_Project";

		public static string MainCompactDB = "shikii.db";

		public static string AutoCleanTime = "AutoClearTime";

		public static string ApplyUserPriority = "ApplyUserPriority";

		public static string HideMainForm = "HideMainForm";

		public static string Contact = "Contact";

		public static string IncName = "IncName";

		public static string ImageRecord = "ImageRecordTable";

		public static string ManufatureTotalNumRecordTable_PerDay = "ManufatureTotalNumRecordTable";

		public static int nTodayImageCount = 0;

		public static string NGNumRecordTable_PerDay = "NGNumRecordTable";

		public static string StatisticNGOKPercentTable_PerDay = "StatisticOKNGPercentTable_PerDay";

		public static int OKPercent = 0;

		public static int NGPercent = 0;

		public static int nNGNum = 0;

		[STAThread]
		private static void Main()
		{
			WinFormApp.BegineInvokeApp();
			EditV2 = new JobToolEditV2();
			frm = new MainForm();
			PrepareLogCleaner();
			WinFormApp.EndInvokeApp(frm, frm.PrepareVision, frm.mobileListBox1);
		}

		public static void PrepareLogCleaner()
		{
			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
			timer.Interval = 1000;
			timer.Tick += delegate
			{
				if (frm.mobileListBox1.Items.Count > 12)
				{
					frm.mobileListBox1.Items.Clear();
				}
			};
			timer.Enabled = true;
			timer.Start();
		}

		public static Form ShowJobManagerWnd(Form owedForm)
		{
			Form frm = new Form();
			frm.Text = "作业管理器";
			frm.StartPosition = FormStartPosition.CenterScreen;
			Form form = frm;
			Size size = EditV2.Size;
			int width = size.Width + 10;
			size = EditV2.Size;
			form.Size = new Size(width, size.Height + 10);
			EditV2.Dock = DockStyle.Fill;
			EditV2.Subject = ThisJobTool;
			frm.Controls.Add(EditV2);
			frm.Show();
			frm.FormClosed += delegate
			{
				frm.Controls.Remove(EditV2);
				frm.Dispose();
			};
			return frm;
		}

		public static string GetShortProjectName()
		{
			string path = R.CompactDB.FetchValue(CurrentProject, true, "0");
			return Path.GetFileName(path);
		}
	}
}
