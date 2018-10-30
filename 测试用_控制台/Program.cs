using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace 测试用_控制台
{
    class Program
    {
        static void Main(string[] args)
        {

            OutPut("wmic cpu get processorid");
            Console.ReadKey();
        }

        static void OutPut(String str)
        {
            

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
           // string output = p.StandardOutput.ReadToEnd();
            //StringReader sr = new StringReader(output);
            //sr.
            List<String> lst = new List<string>();

            while (true)
            {
                if (p.StandardOutput.EndOfStream)
                    break;
                String line = p.StandardOutput.ReadLine();

                if(!String.IsNullOrEmpty(line))
                   lst.Add(line);
            }
            p.WaitForExit();//等待程序执行完退出进程
            p.Close();


            Console.WriteLine("");

        }
       
    }

}
