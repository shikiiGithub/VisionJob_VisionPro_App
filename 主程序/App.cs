using System;
using System.Windows.Forms;
using dotNetLab.Common;
namespace shikii.VisionJob
{
   public class App
    {
       public static dotNetLab.Debug.CodeEngine codeEngine;
      
       [STAThread]
       static void Main()
       {
           WinFormApp.BegineInvokeApp();
           MainForm frm = new MainForm() ;
           WinFormApp.EndInvokeApp(frm,frm.PrepareVision,frm.mobileListBox1);
         
         
       }
        public static String ProjsFolderName = "Projs";
        public static String OriginProjectPath = "Projs\\0";
        public static String CurrentProject = "Current_Project";
        public static String MainCompactDB = "shikii.db";
        public static string AutoCleanTime = "AutoClearTime";
        public static String ApplyUserPriority = "ApplyUserPriority";
    }
}
