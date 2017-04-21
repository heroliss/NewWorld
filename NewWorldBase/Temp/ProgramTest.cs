//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//namespace NerWorldServer
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            int x_size = 5;
//            int y_size = 1;
//            int z_size = 5;
//            int x_middle = x_size / 2;
//            int y_middle = y_size / 2;
//            int z_middle = z_size / 2;
//            Console.Write("输入每个方格石头质量：");
//            double rockmass = double.Parse(Console.ReadLine());
//            WorldGrid world = new WorldGrid(x_size, y_size, z_size);
//            for (int x = 0; x < x_size; x++)
//            {
//                for (int y = 0; y < y_size; y++)
//                {
//                    for (int z = 0; z < z_size; z++)
//                    {
//                        world.Grids[x, y, z].Rock.AddMass(rockmass, 0);
//                        //world.Grids[x, y, z].Silver.AddVolume(0, 0);
//                    }
//                }
//            }
//            //world.Grids[2, 0, 2].InitTemperature(100);

//            while (true)
//            {
//                double heatSum = 0;
//                double temperatureSum = 0;
//                Console.WriteLine("温度：");
//                for (int y = 0; y < y_size; y++)
//                {
//                    for (int x = 0; x < x_size; x++)
//                    {
//                        for (int z = 0; z < z_size; z++)
//                        {
//                            heatSum += world.Grids[x, y, z].Heat;
//                            temperatureSum += world.Grids[x, y, z].Temperature;
//                            Console.Write("{0:00.00} ", world.Grids[x, y, z].Temperature);
//                        }
//                        Console.WriteLine(); Console.WriteLine();
//                    }
//                    Console.WriteLine("------------------");
//                }
//                Console.WriteLine("总热能： {0}   温度和：{1}   平均温度：{2}",
//                        heatSum, temperatureSum, temperatureSum / (x_size * y_size * z_size));
//                Console.WriteLine("\n中间方格：");
//                Console.WriteLine("总质量：{0}   密度：{1}  银质量：{2}",
//                    world.Grids[x_middle, y_middle, z_middle].Mass, world.Grids[x_middle, y_middle, z_middle].Density, world.Grids[x_middle, y_middle, z_middle].Silver.Mass);
//                Console.WriteLine("已用空间：{0}   可用空间：{1}  银占用空间：{2}",
//                    world.Grids[x_middle, y_middle, z_middle].UsedSpace, world.Grids[x_middle, y_middle, z_middle].FreeSpace, world.Grids[x_middle, y_middle, z_middle].Silver.Volume);
//                Console.WriteLine("导热率：{0}   比热容：{1}",
//                    world.Grids[x_middle, y_middle, z_middle].ThermalConductivity, world.Grids[x_middle, y_middle, z_middle].SpecificHeatCapacity);
//                Console.WriteLine("迭代次数：{0}", world.Iterations);
//                Console.WriteLine("\n按[1]增减中间方格的热能，[2]设定中间方格的温度，[3]增减中间银质量,[4]增减中间银体积，按↓执行下一次迭代");

//                while (true)
//                {
//                    bool done = false;
//                    bool success = true;
//                    switch (Console.ReadKey(true).Key)
//                    {
//                        case ConsoleKey.NumPad1:
//                            Console.Write("输入热能改变量：");
//                            double e = double.Parse(Console.ReadLine());
//                            world.Grids[x_middle, y_middle, z_middle].AddHeat(e);
//                            done = true;
//                            break;
//                        case ConsoleKey.NumPad2:
//                            Console.Write("输入温度值：");
//                            double t = double.Parse(Console.ReadLine());
//                            world.Grids[x_middle, y_middle, z_middle].InitTemperature(t);
//                            done = true;
//                            break;
//                        case ConsoleKey.NumPad3:
//                            Console.Write("输入银的质量改变量：");
//                            double m = double.Parse(Console.ReadLine());
//                            success = world.Grids[x_middle, y_middle, z_middle].Silver.AddMass(m, 0);
//                            done = true;
//                            break;
//                        case ConsoleKey.NumPad4:
//                            Console.Write("输入银的体积改变量：");
//                            double v = double.Parse(Console.ReadLine());
//                            success = world.Grids[x_middle, y_middle, z_middle].Silver.AddVolume(v, 0);
//                            done = true;
//                            break;
//                        case ConsoleKey.DownArrow:
//                            world.Next();
//                            done = true;
//                            break;
//                        default:
//                            break;
//                    }
//                    if (!success)
//                    {
//                        Console.WriteLine("更改失败！");
//                        continue;
//                    }
//                    if (done)
//                    {
//                        break;
//                    }
//                }
//                //Thread.Sleep(100000);
//                //world.Next();
//                Console.Clear();

//            }
//        }
//    }
//}

