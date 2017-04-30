using Cowboy.Sockets;
using System;
using System.Text;
using System.Threading.Tasks;

namespace NewWorldServer
{
    public class Dispatcher : ITcpSocketSaeaServerMessageDispatcher
    {
        public async Task OnSessionStarted(TcpSocketSaeaSession session)
        {
            Console.WriteLine(string.Format("TCP客户端 {0} 已接入", session.RemoteEndPoint, session));
            await Task.CompletedTask;
        }

        public async Task OnSessionDataReceived(TcpSocketSaeaSession session, byte[] data, int offset, int count)
        {
            var text = Encoding.UTF8.GetString(data, offset, count);
            Console.Write(string.Format("接收自{0} --> ", session.RemoteEndPoint));
            if (count < 1024 * 1024 * 1)
            {
                Console.WriteLine(text);
            }

            Console.WriteLine("{0} 字节  offset:{1} ", count, offset);


            await Task.CompletedTask;
        }

        public async Task OnSessionClosed(TcpSocketSaeaSession session)
        {
            Console.WriteLine(string.Format("TCP session {0} 断开连接", session));
            await Task.CompletedTask;
        }
    }
}
