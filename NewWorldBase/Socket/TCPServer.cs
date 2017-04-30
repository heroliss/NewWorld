using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cowboy.Sockets;
using Logrila.Logging;
using NewWorldServer.Core;
using NewWorldServer.SessionWork;
using System.Diagnostics;

namespace NewWorldServer
{
    class TCPServer
    {
        int port = 54321;
        TcpSocketSaeaServer server;
        GridWorld gridWorld;
        Packager packager;
        Stopwatch stopwatch = new Stopwatch();
        public void Run()
        {
            gridWorld = CreateGridWorld();
            Console.WriteLine("世界创建完成！");

            var config = new TcpSocketSaeaServerConfiguration();
            server = new TcpSocketSaeaServer(port, new Dispatcher(), config);
            server.Listen();
            Console.WriteLine("TCP服务端已启动！监听端口：{0}", server.ListenedEndPoint);

            packager = new Packager();
            while (true)
            {
                try
                {
                    string text = Console.ReadLine();

                    Task.Run(async () =>
                    {
                        switch (text)
                        {
                            case "broad":
                                IEnumerable<Grid> squareGrids = packager.getSquareGrids(gridWorld.Grids, 15, 5, 15);
                                byte[] message = packager.PackageGrids(squareGrids);
                                await server.BroadcastAsync(message);
                                Console.WriteLine("广播 -> {0}字节", message.Length);
                                break;
                            case "next":
                                stopwatch.Restart();
                                gridWorld.Next();
                                stopwatch.Stop();
                                Console.WriteLine("完成一次next，用时{0}ms", stopwatch.ElapsedMilliseconds);
                                break;
                            case "test":
                                byte[] mes = new byte[600000];
                                for (int i = 0; i < mes.Length; i++)
                                {
                                    mes[i] = (byte)(i % 256);
                                }
                                await server.BroadcastAsync(mes);
                                Console.WriteLine("广播 -> {0}字节", mes.Length);
                                break;
                            default:
                                try
                                {
                                    List<byte> s = new List<byte>();
                                    for (int i = 0; i < int.Parse(text); i++)
                                    {
                                        s.Add(94);
                                    }
                                    await server.BroadcastAsync(s.ToArray());
                                    Console.WriteLine("广播 -> {0}字节", s.Count);
                                }
                                catch { }
                                break;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Logger.Get<TCPServer>().Error(ex.Message, ex);
                }
            }
        }

        private GridWorld CreateGridWorld()
        {
            GridWorld gridWorld = new GridWorld(100, 10, 100);
            foreach (var item in gridWorld.Grids)
            {
                item.Objs[0].AddMass(2000, 50000);
            }
            gridWorld.Grids[15, 9, 15].InitTemperature(300);
            return gridWorld;
        }
    }
}
