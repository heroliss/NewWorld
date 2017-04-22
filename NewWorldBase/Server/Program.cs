using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NewWorldServer.Client;

namespace NewWorldServer.Server
{
    class Program
    {
        static UdpClient udpServer;
        ~Program()
        {
            udpServer.Close();
        }
        static void Main(string[] args)
        {
            bool sending = true;
            int sizeX=10, sizeY=2, sizeZ=10;
            WorldGrid world;
            world = CreateWorld(sizeX, sizeY, sizeZ);

            IPAddress remoteIP = IPAddress.Parse("127.0.0.1");
            int remotePort = 12345;
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);//实例化一个远程端点 

            using (udpServer = new UdpClient(54321))
            {
                while (sending)
                {
                    Console.ReadKey(true);
                    /*List<Dictionary<string, Dictionary<string, double>>> gridInfoList 
                        = new List<Dictionary<string, Dictionary<string, double>>>();
                    foreach (var item in world.Grids)
                    {
                        gridInfoList.Add(item.GetGridInfo());
                    }*/

                    foreach (var item in world.Grids)
                    {
                        MemoryStream stream = new MemoryStream();
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, item.GetGridInfo());
                        byte[] buffer = stream.GetBuffer();
                        Console.WriteLine(buffer.Length);
                        udpServer.Send(buffer, buffer.Length, remotePoint);
                        System.Threading.Thread.Sleep(50);
                    }
                }
            }
        }

        private static WorldGrid CreateWorld(int sizeX, int sizeY, int sizeZ,double gravity = 10)
        {
            WorldGrid newWorld = new WorldGrid(sizeX, sizeY, sizeZ, gravity);
            foreach (var item in newWorld.Grids)
            {
                item.Objs["rock"].AddVolume(200000, 0);
            }
            return newWorld;
        }
    }
}
