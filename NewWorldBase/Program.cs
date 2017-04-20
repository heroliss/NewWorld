using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace NewWorldBase
{
    class Program
    {
        static void Main(string[] args)
        {
            WorldGrid world = new WorldGrid(5, 1, 5);
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 1; y++)
                {
                    for (int z = 0; z < 5; z++)
                    {
                        world.Grids[x, y, z].Rock.AddMass(2, 0);
                        world.Grids[x, y, z].Silver.AddMass(8, 0);
                    }
                }
            }
            world.Grids[2, 0, 2].InitTemperature(100);
            
            while (true)
            {
                double heatSum = 0;
                for (int y = 0; y < 1; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        for (int z = 0; z < 5; z++)
                        {
                            heatSum += world.Grids[x, y, z].Heat;
                            Console.Write("{0:00.0000} ", world.Grids[x, y, z].Temperature);                       
                        }
                       Console.WriteLine();
                    }
                    Console.WriteLine("------------------");
                    Console.WriteLine(" {0}", heatSum);
                }

                while (true)
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.UpArrow)
                    {
                        world.Grids[5, 1, 5].Silver.AddMass(int.Parse(Console.ReadLine()), 100000000);
                        break;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        world.Next();
                        break;
                    }
                }
                //Thread.Sleep(100000);
                //world.Next();
                Console.Clear();
                
            }
        }
    }
}

