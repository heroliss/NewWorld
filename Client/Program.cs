using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Logrila.Logging;
using Logrila.Logging.NLogIntegration;
using System.Net.Sockets;
using System.Threading;

namespace Cowboy.Sockets.TestTcpSocketSaeaClient
{
    class Program
    {
        static TcpSocketSaeaClient _client;
        static string serverIP = "127.0.0.1";
        static int serverPort = 54321;
        static int clientPort = 54320; //此客户中转端端口号
        static TcpClient unityClient;
        static TcpListener listener;
        static void Main(string[] args)
        {
           
            NLogLogger.Use();
            listener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), clientPort));
            listener.Start();
            Console.WriteLine("正在等待Unity程序接入...");
            unityClient = listener.AcceptTcpClient();
            Console.WriteLine("Unity程序已连接！");

            try
            {
                var config = new TcpSocketSaeaClientConfiguration();
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
                _client = new TcpSocketSaeaClient(remoteEP, new Dispatcher(unityClient), config);
                Console.WriteLine("正在连接服务器{0}...", remoteEP);
                while (true)
                {
                    try
                    {
                        _client.Connect().Wait();
                        break;
                    }
                    catch (AggregateException)
                    {
                        Console.WriteLine("连接失败，正在重连...");
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Debug1:" + ex.Message);
                Logger.Get<Program>().Error(ex.Message, ex);
            }

            while(true)
                Console.ReadKey(true);
        }
    }
}
