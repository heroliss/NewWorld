using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWorldBase
{
    abstract public class WorldObject
    {
        /*--------------网格相关字段和函数----------------*/
        private Grid currentGrid; //该物体所在的网格（固定不变）
        public Grid CurrentGrid { get { return currentGrid; } }

        abstract public void Do(); //每次轮到该物体时执行

        /*-----------------物体基本属性---------------------*/
        private double mass; //质量
        private double volume; //体积
        private double temperature; //温度
        private double energy; //储能
        private double heat; //热能 
        private double potentialEnergy; //重力势能
        /// <summary>
        /// 密度（质量/密度=厚度）[]
        /// </summary>
        abstract public double Density { get; } 
        abstract public double SpecificHeatCapacity { get; } //比热容（比热容*质量*温差=能量变化，能量/(质量*比热容)=温度 ）
        abstract public double ThermalConductivity { get; } //热导率（单位时间内对每个方向的传导能量 = 该值*温差 ）

        public double Mass
        {
            get
            {
                return mass;
            }
        }

        public bool AddMass(double deltaMass,double deltaHeat)
        {
            double deltaVolume = deltaMass / Density;
            if (currentGrid.FreeSpace < deltaVolume || -currentGrid.UsedSpace > deltaVolume)
                return false;
            mass += deltaMass;
            volume += deltaVolume;
            currentGrid.MassChanged(deltaMass,deltaVolume,deltaHeat); //热能从这里传递给方格重新分配
            return true;
        }
        public bool AddVolume(double deltaVolume,double deltaHeat)
        {
            if (currentGrid.FreeSpace < deltaVolume || -currentGrid.UsedSpace > deltaVolume)
                return false;
            double deltaMass = deltaVolume * Density;
            mass += deltaMass;
            volume += deltaVolume;
            currentGrid.MassChanged(deltaMass, deltaVolume, deltaHeat); //热能从这里传递给方格重新分配
            return true;
        }
        public double Energy { get { return energy; } }
        public double Heat { get { return heat; } }
        public double Volume { get { return volume; } }

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

        public double GravitationalPotentialEnergy
        {
            get
            {
                return 0;
                //return gravitationalPotentialEnergy;
            }
        }

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


    }
}
