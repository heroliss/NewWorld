using System;
using System.Collections.Generic;
using System.Diagnostics;
using NewWorldServer.Core.WorldObjects;

namespace NewWorldServer.Core
{
    public class Grid
    {
        #region 网格信息
        [NonSerialized]
        private GridWorld worldGrid; //该格子所在的世界网格
        private int x, y, z; //位置
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int Z { get { return z; } }
        public Grid UpGrid { get { if (y + 1 >= worldGrid.Size_y) return null; return worldGrid.Grids[x, y + 1, z]; } }
        public Grid DownGrid { get { if (y - 1 <= worldGrid.Size_y) return null; return worldGrid.Grids[x, y - 1, z]; } }
        public Grid LeftGrid { get { if (x - 1 <= worldGrid.Size_x) return null; return worldGrid.Grids[x - 1, y, z]; } }
        public Grid RightGrid { get { if (x + 1 >= worldGrid.Size_x) return null; return worldGrid.Grids[x + 1, y, z]; } }
        public Grid ForwardGrid { get { if (z + 1 >= worldGrid.Size_z) return null; return worldGrid.Grids[x, y, z + 1]; } }
        public Grid BackwardGrid { get { if (z - 1 <= worldGrid.Size_z) return null; return worldGrid.Grids[x, y, z - 1]; } }
        #endregion

        #region 字段
        private double freeSpace;
        private double usedSpace;
        private double mass;
        private double temperature;
        private double energy;
        private double heat;
        private double potentialEnergy;
        private double density;
        private double specificHeatCapacity;
        private double thermalConductivity;
        #endregion

        #region 属性
        /*-------------------属性---------------------*/
        public double Mass { get { return mass; } }//质量   
        public double FreeSpace { get { return freeSpace; } }//剩余空间
        public double UsedSpace { get { return usedSpace; } }//已用空间
        public double Temperature { get { return temperature; } }//温度

        public double Energy { get { return energy; } }//储能
        public double PotentialEnergy { get { return potentialEnergy; } }//重力势能
        public double Heat { get { return heat; } }//热能
        //密度（质量/密度=厚度）
        public double Density { get { return density; } }
        //比热容（比热容*质量*温差=能量变化，能量/(质量*比热容)=温度 ）
        public double SpecificHeatCapacity { get { return specificHeatCapacity; } }
        //热导率（单位时间内对每个方向的传导能量 = 该值*温差 ）
        public double ThermalConductivity { get { return thermalConductivity; } }
        #endregion

        #region 物体 
        private WorldObject[] objs;
        public WorldObject[] Objs { get { return objs; } }
        #endregion

        #region 构造函数
        public Grid(GridWorld worldGrid, int x, int y, int z, double freeSpace)
        {
            this.worldGrid = worldGrid;
            this.freeSpace = freeSpace;
            this.x = x;
            this.y = y;
            this.z = z;
            objs = ObjsCreator.CreateObjsList(this);
        }
        #endregion

        #region 函数
        public void AddHeat(double deltaHeat) //根据改变的热能重新为每个物体分配热能和温度
        {
            heat += deltaHeat;
            Debug.Assert(heat >= 0);
            temperature = heat / specificHeatCapacity / mass;
            double heatSum = 0; //调试用
            foreach (WorldObject obj in objs)
            {
                obj.Temperature = temperature; //该赋值同时更改了obj的热能值
                heatSum += obj.Heat; //调试用
            }
            Debug.Assert((float)heatSum == (float)heat);
        }
        public void InitTemperature(double temperature) //初始化用的温度设置函数
        {
            if (mass == 0)
            {
                return;
            }
            this.temperature = temperature;
            heat = Temperature * specificHeatCapacity * mass;
            foreach (WorldObject obj in objs)
            {
                obj.Temperature = temperature; //该赋值同时更改了obj的热能值
            }
        }
        /// <summary>
        /// 某个物体质量更改后调用此函数
        /// </summary>
        public void MassChanged(double deltaMass, double deltaVolume, double deltaHeat)
        {
            mass += deltaMass;
            Debug.Assert(mass >= 0);
            freeSpace -= deltaVolume;
            usedSpace += deltaVolume;

            //重新计算加权平均比热容、加权平均热导率、加权平均密度(该值可由v/m计算求得)
            density = mass / usedSpace;
            specificHeatCapacity = 0;
            thermalConductivity = 0;

            if (mass == 0)
            {
                Debug.Assert(usedSpace == 0);
                Debug.Assert(freeSpace == worldGrid.MaxGridVolume);
                temperature = 0;
                heat = 0;
                return;
            }
            double volumeSum = 0, massSum = 0; //调试用
            foreach (WorldObject obj in objs)
            {
                specificHeatCapacity += obj.Mass * obj.SpecificHeatCapacity;
                thermalConductivity += obj.Mass * obj.ThermalConductivity;
                volumeSum += obj.Volume; //调试用
                massSum += obj.Mass;//调试用
                Debug.Assert(obj.Mass == obj.Density * obj.Volume);
            }
            Debug.Assert(volumeSum == usedSpace);
            Debug.Assert(massSum == mass);

            specificHeatCapacity /= mass;//求得比热容
            thermalConductivity /= mass;//求得热导率
            //计算混合后的温度
            AddHeat(deltaHeat); //必须放在计算平均比热容后面
        }
        /// <summary>
        /// 按当前网格内的物质比例转移物质
        /// </summary>
        /// <param name="moveMass">减少（转移）的物质质量</param>
        public bool MoveTo(Grid grid, double moveMass)
        {
            Debug.Assert(moveMass > 0);
            if (mass < moveMass)
            {
                return false;
            }
            for (int i = 0; i < objs.Length; i++)//每个物体按比例减少质量
            {
                objs[i].AddMassWithoutHeat(-moveMass * objs[i].Mass / mass);
                grid.objs[i].AddMassWithoutHeat(moveMass * objs[i].Mass / mass);
            }
            MassChanged(-moveMass, -moveMass / density, -moveMass / mass * heat);
            grid.MassChanged(moveMass, moveMass / density, moveMass / mass * heat);
            return true;
        }
        public void DoAll()
        {
            foreach (WorldObject obj in objs)
            {
                obj.Do();
            }
        }

    //    public Dictionary<string,Dictionary<string,double>> GetGridInfo()
    //    {
    //        Dictionary<string, Dictionary<string, double>> gridInfo = new Dictionary<string, Dictionary<string, double>>();
    //        gridInfo["GridProperty"] = new Dictionary<string, double>();
    //        gridInfo["GridProperty"]["Density"] = density;
    //        gridInfo["GridProperty"]["Energy"] = energy;
    //        gridInfo["GridProperty"]["FreeSpace"] = freeSpace;
    //        gridInfo["GridProperty"]["Heat"] = heat;
    //        gridInfo["GridProperty"]["Mass"] = mass;
    //        gridInfo["GridProperty"]["PotentialEnergy"] = potentialEnergy;
    //        gridInfo["GridProperty"]["SpecificHeatCapacity"] = specificHeatCapacity;
    //        gridInfo["GridProperty"]["Temperature"] = temperature;
    //        gridInfo["GridProperty"]["ThermalConductivity"] = thermalConductivity;
    //        gridInfo["GridProperty"]["UsedSpace"] = usedSpace;
    //        gridInfo["GridProperty"]["X"] = x;
    //        gridInfo["GridProperty"]["Y"] = y;
    //        gridInfo["GridProperty"]["Z"] = z;
            
    //        foreach (string key in objs.Keys)
    //        {
    //            gridInfo[key] = new Dictionary<string, double>();
    //            gridInfo[key]["Energy"] = objs[key].Energy;
    //            gridInfo[key]["Heat"] = objs[key].Heat;
    //            gridInfo[key]["Mass"] = objs[key].Mass;
    //            gridInfo[key]["PotentialEnergy"] = objs[key].PotentialEnergy;
    //            gridInfo[key]["Temperature"] = objs[key].Temperature;
    //            gridInfo[key]["Volume"] = objs[key].Volume;
    //        }
    //        return gridInfo;
    //    }
    }
    #endregion
}
