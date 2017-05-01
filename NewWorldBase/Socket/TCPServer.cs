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
using NewWorldServer.Socket.SessionWorks.GridsTakers;
using System.Timers;

namespace NewWorldServer
{
    class TCPServer
    {
        int port = 54321;
        TcpSocketSaeaServer server;
        GridWorld gridWorld;
        Packager packager;
        SqureGridsTaker squreGridsTaker;
        Stopwatch stopwatch = new Stopwatch();

        Timer atim;
        long maxNextTime = 0;
        long maxBroadTime = 0; 
        public void StartLoop()
        {
            atim = new Timer()
            {
                Interval = 500
            };
            atim.Elapsed += (sender, e) => 
            {
                stopwatch.Restart();
                server.BroadcastAsync(packager.EncodeGrids(squreGridsTaker, 25, 2, 25)).Wait();
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds>maxBroadTime)
                {
                    maxBroadTime = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine("产生最长广播用时：{0}", maxBroadTime);
                }
                stopwatch.Restart();
                gridWorld.Next();
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds > maxNextTime)
                {
                    maxNextTime = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine("产生最长计算用时：{0}", maxNextTime);
                }
            };
            atim.Start();
            Console.WriteLine("循环执行已启动！");
        }
        public void StopLoop()
        {
            atim.Stop();
            Console.WriteLine("循环执行已停止！");
        }
        public void Run()
        {
            gridWorld = CreateGridWorld();
            Console.WriteLine("世界创建完成！");

            var config = new TcpSocketSaeaServerConfiguration();
            squreGridsTaker = new SqureGridsTaker(gridWorld.Grids,5,3,25);
            server = new TcpSocketSaeaServer(port, new Dispatcher(gridWorld,squreGridsTaker), config);
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
                                byte[] message = packager.EncodeGrids(squreGridsTaker, 25, 2, 25);
                                await server.BroadcastAsync(message);
                                Console.WriteLine("广播 -> {0}字节", message.Length);
                                break;
                            case "start loop":
                                StartLoop();
                                break;
                            case "stop loop":
                                StopLoop();
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
            GridWorld gridWorld = new GridWorld(50, 3, 50);
            foreach (var item in gridWorld.Grids)
            {
                item.Objs[0].AddMass(3, 0);
            }
            gridWorld.Grids[25, 2, 25].InitTemperature(30000);
            return gridWorld;
        }
    }
}
