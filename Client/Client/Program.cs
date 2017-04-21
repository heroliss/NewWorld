using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NerWorldServer.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client;
            client = new UdpClient(12345);

            IPAddress serverIP = IPAddress.Parse("127.0.0.1"); //服务器地址
            IPEndPoint serverPoint = null;

            byte[] receiveData;
            int i=0;
            while (true)
            {
                i++;  Console.WriteLine(i);
                string receiveString;
                receiveData = client.Receive(ref serverPoint);//接收数据 
                receiveString = Encoding.Default.GetString(receiveData);
                Console.WriteLine(receiveString);
                
            }
            client.Close();//关闭连接 
        }

    }
}
