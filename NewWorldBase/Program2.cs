using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NewWorldBase.Server;
namespace NewWorldBase
{

    class Program2
    {
        static void Main(string[] args)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream("fff.txt", FileMode.Open);

            //WorldGrid worldGrid = new WorldGrid(10, 2, 10);
            //worldGrid.Grids[2, 0, 3].Objs["rock"].AddVolume(500000, 0);
            //Grid grid = worldGrid.Grids[2, 0, 3];
            //formatter.Serialize(file, grid.GetGridInfo());

            Client.GridInfo gridInfo = (Client.GridInfo)formatter.Deserialize(file);
            
            Console.Read();
           
        }
    }
}
