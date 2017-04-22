using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace NewWorldServer.Client
{
    [Serializable]
    abstract public class WorldObjectInfo
    {
        private GridInfo currentGrid; //该物体所在的网格（固定不变）
        public GridInfo CurrentGrid { get { return currentGrid; } }

        public double Mass; //质量
        public double Volume; //体积
        public double Temperature; //温度
        public double Energy; //储能
        public double Heat; //热能 
        public double PotentialEnergy; //重力势能

        abstract public double Density{ get; } 
        abstract public double SpecificHeatCapacity { get; } //比热容（比热容*质量*温差=能量变化，能量/(质量*比热容)=温度 ）
        abstract public double ThermalConductivity { get; } //热导率（单位时间内对每个方向的传导能量 = 该值*温差 ）

        public WorldObjectInfo(GridInfo currentGrid)
        {
            this.currentGrid = currentGrid;
        }


    }
}
