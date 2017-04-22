using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace NewWorldServer.Client
{
    [Serializable]
    public struct WorldObjectInfo
    {
        public double Mass; //质量
        public double Volume; //体积
        public double Temperature; //温度
        public double Energy; //储能
        public double Heat; //热能 
        public double PotentialEnergy; //重力势能
    }
}
