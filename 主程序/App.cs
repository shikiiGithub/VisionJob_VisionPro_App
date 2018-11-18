using System;
using System.Reflection;
using System.Windows.Forms;
using Cognex.VisionPro.Implementation;
using dotNetLab.Common;
using dotNetLab.Vision.VPro;

namespace shikii.VisionJob
{
   public class App 
    {


        //to do  记录各VPP的路径,定义各个VPP对应用的ToolBlockPowerSuite
        public static ToolBlockPowerSuite thisToolBlockSuite;
        public static MainForm frm;
     

        [STAThread]
       static void Main()
       {
           WinFormApp.BegineInvokeApp();
             frm = new MainForm() ;
      
            WinFormApp.EndInvokeApp(frm,frm.PrepareVision,frm.mobileListBox1);
       }
        public static String ProjsFolderName = "Projs";
        public static String OriginProjectPath = "Projs\\0";
        public static String CurrentProject = "Current_Project";
        public static String MainCompactDB = "shikii.db";
        public static string AutoCleanTime = "AutoClearTime";
        public static String ApplyUserPriority = "ApplyUserPriority";
        public static String HideMainForm = "HideMainForm";
    }
}
