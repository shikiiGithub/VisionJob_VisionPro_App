using dotNetLab;
using dotNetLab.Common;
using dotNetLab.Network;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace shikii.VisionJob
{
	public class TCPFactoryServer : XTCPServer
	{
		public delegate void TCPClientConnectedInvokeCallback(int nExecuteCode);

		private string TCPTABLENAME = null;

		public TCPClientConnectedInvokeCallback tcpClientConnectedInvoke;

		private byte[] byt_Arr = null;

		public TCPFactoryServer()
		{
			InitNetArgs("TCP");
		}

		public TCPFactoryServer(string strTableName)
		{
			InitNetArgs(strTableName);
		}

		private void InitNetArgs(string strTableName)
		{
			TCPTABLENAME = strTableName;
			byt_Arr = new byte[256];
			base.TextEncode = Encoding.ASCII;
			try
			{
				R.CompactDB.GetAllTableNames();
				if (!R.CompactDB.AllTableNames.Contains(TCPTABLENAME))
				{
					R.CompactDB.CreateKeyValueTable(TCPTABLENAME);
				}
				R.CompactDB.TargetTable = TCPTABLENAME;
				List<string> nameColumnValues = R.CompactDB.GetNameColumnValues(R.CompactDB.TargetTable);
				if (nameColumnValues.Count == 0)
				{
					R.Pipe.Error("读取网络配置时失败，将增加新记录");
				}
				if (!nameColumnValues.Contains("Port"))
				{
					R.CompactDB.Write("Port", "8040");
				}
				if (!nameColumnValues.Contains("LoopGapTime"))
				{
					R.CompactDB.Write("LoopGapTime", "500");
				}
				if (!nameColumnValues.Contains("IP"))
				{
					R.CompactDB.Write("IP", "127.0.0.1");
				}
				base.Port = int.Parse(R.CompactDB.FetchValue("Port", true, "0"));
				base.LoopGapTime = int.Parse(R.CompactDB.FetchValue("LoopGapTime", true, "0"));
				base.IP = R.CompactDB.FetchValue("IP", true, "0");
				R.CompactDB.TargetTable = R.CompactDB.DefaultTable;
			}
			catch (Exception)
			{
			}
		}

		public bool Send_Mill(string strClientID, byte[] byt_Content)
		{
			try
			{
				int index = lstStrArr_ClientID.IndexOf(strClientID);
				int num = lst_Clients[index].Send(byt_Content);
				if (num <= 0)
				{
					return false;
				}
				return true;
			}
			catch (Exception)
			{
				Tipper.Ask = "是否未连接到指定的客户端？\r\n建议重新启动本程序重试。";
				return false;
			}
		}

		public bool Send_Mill(int ClientIndex, byte[] byt_Content)
		{
			try
			{
				int num = lst_Clients[ClientIndex].Send(byt_Content);
				if (num <= 0)
				{
					return false;
				}
				return true;
			}
			catch (Exception)
			{
				Tipper.Ask = "是否未连接到指定的客户端？\r\n建议重新启动本程序重试。";
				return false;
			}
		}

		public bool Send_Mill(string strClientID, string strWord)
		{
			byte[] bytes = base.TextEncode.GetBytes(strWord);
			return Send_Mill(strClientID, bytes);
		}

		private void DecodeHexString(string str)
		{
			string[] array = str.Split(' ');
			int num = 0;
			string[] array2 = array;
			foreach (string s in array2)
			{
				byt_Arr[num++] = byte.Parse(s, NumberStyles.HexNumber);
			}
		}

		public virtual bool SendHexStr(string strClientID, string strContent)
		{
			DecodeHexString(strContent);
			return Send_Mill(strClientID, byt_Arr);
		}

		protected override void ImplementClientCon_DisCon_Delegate()
		{
			base.ClientConnected += delegate(int nIndex)
			{
				try
				{
					R.Pipe.Info(string.Format("已经连接到客户端：{0}", GetClientIP(nIndex)));
				}
				catch (Exception)
				{
				}
			};
			base.ClientDisconnected += delegate(string ClientIP)
			{
				try
				{
					R.Pipe.Info(string.Format("客户端：{0}已经断开", ClientIP));
				}
				catch (Exception)
				{
				}
			};
		}
	}
}
