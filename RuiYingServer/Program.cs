using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuiYingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var gdServer = new GDProtocolServerV2();
            gdServer.Setup(2015);
            gdServer.NewSessionConnected += gdServer_NewSessionConnected;
            gdServer.NewRequestReceived += gdServer_NewRequestReceived;
            gdServer.SessionClosed += gdServer_SessionClosed;
            gdServer.Start();
            Console.WriteLine("server is:" + gdServer.State.ToString());
            while (true)
            {
                if (Console.ReadKey().KeyChar == 'q')
                {
                    gdServer.Stop();
                    gdServer.Dispose();
                    return;
                }
            }
        }
        static void gdServer_SessionClosed(GDProtocolSessionV2 session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine(session.RemoteEndPoint.ToString() + " closed. reason:" + value);
        }

        static void gdServer_NewRequestReceived(GDProtocolSessionV2 session, GDProtocolRequestInfo requestInfo)
        {
            var info = requestInfo;
            Console.WriteLine("receive from: " + session.RemoteEndPoint.ToString());
            Console.WriteLine("DeviceLogicalCode:" + info.DeviceLogicalCode);
            Console.WriteLine("Seq:" + info.Seq);
            Console.WriteLine("ControlCode:" + info.ControlCode);
            Console.WriteLine("Length:" + info.Length);
            Console.WriteLine("Data:" + info.Data);
            Console.WriteLine("Cs:" + info.Cs);
            Console.WriteLine("-------------------------------------------------------------");
        }

        static void gdServer_NewSessionConnected(GDProtocolSessionV2 session)
        {
            Console.WriteLine(session.RemoteEndPoint.ToString() + " connected.");
        }
    }
}
