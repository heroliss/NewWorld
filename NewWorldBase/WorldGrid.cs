using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
        public int TotalSize { get { return totalSize; } }
        public double MaxGridVolume { get { return maxGridVolume; } }
        public Grid[,,] Grids { get { return grids; } }
        public int Iterations { get { return iterations; } }
        public double Gravity { get { return gravity; } }
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
                        grids[x, y, z] = new Grid(this, x, y, z, maxGridVolume);
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
                grids[x, y, z].DoAll(); //执行网格内的每个物体的行为
                ExchangeHeatAllDirection(grids[x, y, z]); //在六个方向上交换热量
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
        private void ExchangeHeat(Grid grid1, Grid grid2)
        {
            if (grid1 == null || grid2 == null) //防止访问到边缘以外
            {
                return;
            }
            if (grid1.Mass == 0 || grid2.Mass == 0) //防止对真空区热量传递
            {
                return;
            }
            /*--------------得到俩个网格间较低的导热率-----------------*/
            double lowerThermalConductivity = grid1.ThermalConductivity;
            if (grid2.ThermalConductivity < grid1.ThermalConductivity)
            {
                lowerThermalConductivity = grid2.ThermalConductivity;
            }
            /*---------------------------------------------------------*/

            //要传递的热能
            double deltaHeat = (grid1.Temperature - grid2.Temperature) * lowerThermalConductivity;
            if (deltaHeat == 0)
            {
                return;
            }
            //防止传热量传递后高温物体变低温
            double averageTemperature = //最终平均温度
                (grid1.Heat + grid2.Heat) / (grid1.SpecificHeatCapacity * grid1.Mass + grid2.SpecificHeatCapacity * grid2.Mass);
            double  maxDeltaHeat = //最大可传递热能
                (grid1.Temperature - averageTemperature) * grid1.SpecificHeatCapacity * grid1.Mass;

            Debug.Assert((float)((averageTemperature - grid2.Temperature) * grid2.SpecificHeatCapacity * grid2.Mass) == (float)maxDeltaHeat);

            if (deltaHeat > 0)
            {
                if (deltaHeat > maxDeltaHeat)
                {
                    deltaHeat = maxDeltaHeat;
                }
            }
            else//deltaHeat < 0
            {
                if (deltaHeat < maxDeltaHeat)
                {
                    deltaHeat = maxDeltaHeat;
                }
            }
            grid1.AddHeat(-deltaHeat);
            grid2.AddHeat(deltaHeat);
        }
    }
}
