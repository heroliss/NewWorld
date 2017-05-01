using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cowboy.Sockets.TestTcpSocketSaeaClient
{
    public class Dispatcher : ITcpSocketSaeaClientMessageDispatcher
    {
        TcpSocketSaeaClient client;
        NetworkStream unityStream;
        byte[] readBuffer = new byte[1024];
        byte[] readResult = new byte[1024];
        int currentIndex;
        private void AsyncReadCallback(IAsyncResult ar)
        {
            //try
            {
                int dataLength = unityStream.EndRead(ar);
                foreach (byte item in readBuffer.Take(dataLength))
                {
                    readResult[currentIndex] = item;
                    currentIndex++;
                }

                if (unityStream.DataAvailable)
                {
                    unityStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(AsyncReadCallback), unityStream);
                }
                else
                {
                    client.SendAsync(readResult, 0, currentIndex).Wait();
                    currentIndex = 0;
                    unityStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(AsyncReadCallback), unityStream);
                }
            }
            //catch
            {
            }
        }
        public Dispatcher(TcpClient unityClient)
        {
            unityStream = unityClient.GetStream();
        }
        public async Task OnServerConnected(TcpSocketSaeaClient client)
        {
            Console.WriteLine("服务器端 {0} 已连接！", client.RemoteEndPoint);
            this.client = client;
            unityStream.BeginRead(readBuffer,0,readBuffer.Length,new AsyncCallback(AsyncReadCallback),unityStream);
            await Task.CompletedTask;
        }

        public async Task OnServerDataReceived(TcpSocketSaeaClient client, byte[] data, int offset, int count)
        {
            Console.WriteLine("传入 --> {0} 字节", count);
            unityStream.Write(data, offset, count);  //传数据给Unity端
            await Task.CompletedTask;
        }

        public async Task OnServerDisconnected(TcpSocketSaeaClient client)
        {
            byte[] disconnectMessage = new byte[] { 4, 0, 4 };
            client.Close().Wait();
            Console.Write("服务器已断开！");
            unityStream.Write(disconnectMessage,0,disconnectMessage.Length); //向unity端通知服务器断开信息
            while (true)
            {
                try
                {
                    client.Connect().Wait();
                    break;
                }
                catch (AggregateException)
                {
                    Console.WriteLine("连接失败，正在重连...");
                    System.Threading.Thread.Sleep(5000);
                }
            }
            await Task.CompletedTask;
        }
    }
}
