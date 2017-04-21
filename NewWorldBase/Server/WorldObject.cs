using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace NerWorldServer.Server
{
    abstract public class WorldObject
    {
        #region 网格相关字段和函数
        private Grid currentGrid; //该物体所在的网格（固定不变）
        public Grid CurrentGrid { get { return currentGrid; } }

        abstract public void Do(); //每次轮到该物体时执行
        #endregion

        #region 物体基本属性
        private double mass; //质量
        private double volume; //体积
        private double temperature; //温度
        private double energy; //储能
        private double heat; //热能 
        private double potentialEnergy; //重力势能

        abstract public double Density { get; }
        abstract public double SpecificHeatCapacity { get; } //比热容（比热容*质量*温差=能量变化，能量/(质量*比热容)=温度 ）
        abstract public double ThermalConductivity { get; } //热导率（单位时间内对每个方向的传导能量 = 该值*温差 ）
        #endregion

        #region 属性
        public double Energy { get { return energy; } }
        public double Heat { get { return heat; } }
        public double Volume { get { return volume; } }
        public double PotentialEnergy { get { return potentialEnergy; } }
        public double Mass { get { return mass; } }
        public double Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                temperature = value;
                heat = temperature * SpecificHeatCapacity * mass;
            }
        }
        #endregion

        #region 函数
        /// <summary>
        /// 不重新分配热能质量增加（多次调用此函数后必须之后统一调用currentGrid.MassChanged）
        /// </summary>
        public bool AddMassWithoutHeat(double deltaMass)
        {
            Debug.Assert(deltaMass != 0);
            double deltaVolume = deltaMass / Density;
            if (currentGrid.FreeSpace < deltaVolume || -mass > deltaMass)
                return false;
            mass += deltaMass;
            volume += deltaVolume;
            return true;
        }
        public bool AddMass(double deltaMass, double deltaHeat)
        {
            Debug.Assert(deltaMass != 0);
            double deltaVolume = deltaMass / Density;
            if (currentGrid.FreeSpace < deltaVolume || -mass > deltaMass)
                return false;
            mass += deltaMass;
            volume += deltaVolume;
            currentGrid.MassChanged(deltaMass, deltaVolume, deltaHeat); //热能从这里传递给方格重新分配
            return true;
        }
        public bool AddVolume(double deltaVolume, double deltaHeat)
        {
            if (currentGrid.FreeSpace < deltaVolume || -volume > deltaVolume)
                return false;
            double deltaMass = deltaVolume * Density;
            mass += deltaMass;
            volume += deltaVolume;
            currentGrid.MassChanged(deltaMass, deltaVolume, deltaHeat); //热能从这里传递给方格重新分配
            return true;
        }
        //public bool MoveTo(Grid grid,double moveMass)
        //{
        //    double moveVolume = moveMass / Density;
        //    if (mass < moveMass || grid.FreeSpace < moveVolume)
        //    {
        //        return false;
        //    }
        //}

        public WorldObject(Grid currentGrid)
        {
            this.currentGrid = currentGrid;
        }

        //public bool moveTo(Grid grid, int moveMass)
        //{
        //    if (moveMass < 0 || moveMass > Mass)  //调试用
        //    {
        //        throw new System.ApplicationException("移动质量无效！");
        //    }

        //}

        #endregion
    }
}
