using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWorldBase
{ 
    public class WorldGrid
    {
        private int size_x, size_y, size_z, size_xz;
        private int totalSize;
        private NoRepeatRandom noRepeatRandom;
        private double maxGridVolume;
        private Grid[,,] grids;
        private int iterations = 0; //世界迭代次数（相当于运行时间）
        private double gravity; //重力加速度
        #region 字段封装
        public int Size_x { get { return size_x; } }
        public int Size_y { get { return size_y; } }
        public int Size_z { get { return size_z; } }
        public int TotalSize { get => totalSize; }
        public double MaxGridVolume { get { return maxGridVolume; } }
        public Grid[,,] Grids { get => grids; }
        public int Iterations { get => iterations; }
        public double Gravity { get => gravity;  }
        #endregion


        public WorldGrid(int size_x, int size_y, int size_z,
            double maxGridVolume = 1000000, double gravity = 10) //10^6 cm^3 即 1 m^3
        {
            this.maxGridVolume = maxGridVolume;
            this.gravity = gravity;
            this.size_x = size_x;
            this.size_y = size_y;
            this.size_z = size_z;
            totalSize = size_x * size_y * size_z;
            size_xz = size_x * size_z;
            noRepeatRandom = new NoRepeatRandom(totalSize);
            //初始化网格
            grids = new Grid[size_x, size_y, size_z];
            for (int x = 0; x < grids.GetLength(0); x++)
            {
                for (int y = 0; y < grids.GetLength(1); y++)
                {
                    for (int z = 0; z < grids.GetLength(2); z++)
                    {
                        grids[x, y, z] = new Grid(this, x, y, z,maxGridVolume); 
                    }
                }
            }
        }

        /// <summary>
        /// 执行一次世界网格循环
        /// </summary>
        /// <returns>返回该次循环内的物体总数（调试用）</returns>
        public void Next()
        {
            int x, y, z, xz;
            noRepeatRandom.Reset();
            foreach (int total in noRepeatRandom)
            {
                y = total / size_xz;
                xz = total % size_xz;
                z = xz / size_x;
                x = xz % size_x;
                grids[x,y,z].DoAll(); //执行网格内的每个物体的行为
                ExchangeHeatAllDirection(grids[x,y,z]); //在六个方向上交换热量
                //TODO: 重力行为
                //TODO: 
            }
            iterations++;
        }
        private void ExchangeHeatAllDirection(Grid grid)
        {
            ExchangeHeat(grid, grid.RightGrid); 
            ExchangeHeat(grid, grid.LeftGrid); 
            ExchangeHeat(grid, grid.ForwardGrid); 
            ExchangeHeat(grid, grid.BackwardGrid); 
            ExchangeHeat(grid, grid.DownGrid); 
            ExchangeHeat(grid, grid.UpGrid); 
        }
        private void ExchangeHeat(Grid grid1,Grid grid2)
        {
            if (grid1 == null || grid2 == null)
            {
                return;
            }
            if (grid1.Mass == 0 || grid2.Mass == 0)
            {
                return;
            }
            /*--------------得到俩个网格间较低的导热率-----------------*/
            double lowerThermalConductivity = grid1.ThermalConductivity;
            if (grid2.ThermalConductivity<grid1.ThermalConductivity)
            {
                lowerThermalConductivity = grid2.ThermalConductivity;
            }
            /*---------------------------------------------------------*/

            //要传递的热能
            double deltaHeat = (grid1.Temperature - grid2.Temperature) * lowerThermalConductivity;
            //防止传递热能超过物体所包含的热能
            if (deltaHeat>grid1.Heat/2)
            {
                deltaHeat = grid1.Heat/2;
            }
            else if(deltaHeat <-grid2.Heat/2)
            {
                deltaHeat = -grid2.Heat/2;
            }

            grid1.AddHeat(-deltaHeat);
            grid2.AddHeat(deltaHeat);
        }
    }
}
