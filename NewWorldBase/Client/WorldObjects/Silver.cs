using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewWorldBase.Client.WorldObjects
{
    [Serializable]
    public class Silver : WorldObjectInfo
    {
        public Silver(GridInfo currentGrid) : base(currentGrid)
        {
        }

        public override double Density { get { return 0.01; } } // kg/cm^3

        public override double SpecificHeatCapacity { get { return 230; } }// J/( kg·K )

        public override double ThermalConductivity { get { return 420*100; } }// W/(m·K) 的一百倍
    }
}
