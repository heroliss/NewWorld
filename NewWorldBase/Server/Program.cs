using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NerWorldServer.Server
{
    class Program
    {

        static void Main(string[] args)
        {
            UdpClient udpServer;
            udpServer = new UdpClient(54321);

            IPAddress remoteIP = IPAddress.Parse("127.0.0.1");
            int remotePort = 12345;
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);//实例化一个远程端点 

            while (true)
            {
                string sendString = Console.ReadLine();
                byte[] buffer = Encoding.Default.GetBytes(sendString);
                udpServer.Send(buffer, buffer.Length, remotePoint);//将数据发送到远程端点 
            }
            udpServer.Close();//关闭连接 
        }
    }
}
