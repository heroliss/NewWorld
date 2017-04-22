using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NewWorldServer.Client
{
    [Serializable]
    public struct GridInfo
    {
        #region 网格信息
        public int x, y, z; //位置
        #endregion

        #region 网格属性
        public double FreeSpace;
        public double UsedSpace;
        public double Mass;
        public double Temperature;
        public double Energy;
        public double Heat;
        public double PotentialEnergy;
        public double Density;
        public double SpecificHeatCapacity;
        public double ThermalConductivity;
        #endregion

        #region 物体
        public Dictionary<string,WorldObjectInfo> objs;
        #endregion
    }
}
