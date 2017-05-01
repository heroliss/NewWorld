using Cowboy.Sockets;
using NewWorldServer.Core;
using NewWorldServer.Socket.SessionWorks.GridsTakers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace NewWorldServer
{
    public class Dispatcher : ITcpSocketSaeaServerMessageDispatcher
    {
        GridWorld gridWorld;
        IGridsTaker iGridsTaker;
        public Dispatcher(GridWorld gridWorld,IGridsTaker iGridsTaker)
        {
            this.gridWorld = gridWorld;
            this.iGridsTaker = iGridsTaker;
        }
        
        public async Task OnSessionStarted(TcpSocketSaeaSession session)
        {
            Console.WriteLine(string.Format("TCP客户端 {0} 已接入", session.RemoteEndPoint, session));
            byte[] data = SqureWindowInitData();
            session.SendAsync(data).Wait();
            await Task.CompletedTask;
        }

        private byte[] SqureWindowInitData()
        {
            byte[] data = new byte[1 + 2 * 3 + 1 + 2 * 3];
            data[0] = 50;//初始化协议码
            BitConverter.GetBytes((ushort)gridWorld.Size_x).CopyTo(data, 1);
            BitConverter.GetBytes((ushort)gridWorld.Size_y).CopyTo(data, 3);
            BitConverter.GetBytes((ushort)gridWorld.Size_z).CopyTo(data, 5);
            data[7] = 0; //视窗类型
            SqureGridsTaker taker = iGridsTaker as SqureGridsTaker;
            BitConverter.GetBytes((ushort)taker.WindowSizeX).CopyTo(data, 8);
            BitConverter.GetBytes((ushort)taker.WindowSizeY).CopyTo(data, 10);
            BitConverter.GetBytes((ushort)taker.WindowSizeZ).CopyTo(data, 12);
            return data;
        }

        public async Task OnSessionDataReceived(TcpSocketSaeaSession session, byte[] data, int offset, int count)
        {
            Console.Write(string.Format("接收自{0} -> ", session.RemoteEndPoint));   
            Console.Write(" {0} byte ", count);
            if (count < 20)
            {
                Console.Write("内容：[ ");
                for (int i = offset; i < offset + count; i++)
                {
                    Console.Write(data[i]);
                    Console.Write(" ");
                }
                Console.Write("]");
            }
            Console.WriteLine();
            switch (data[offset])
            {
                case 50: //初始化请求
                    session.SendAsync(SqureWindowInitData()).Wait(); //发送初始化数据
                    break;
                default:
                    break;
            }
            await Task.CompletedTask;
        }

        public async Task OnSessionClosed(TcpSocketSaeaSession session)
        {
            Console.WriteLine(string.Format("TCP session {0} 断开连接", session));
            await Task.CompletedTask;
        }
    }
}
