using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewWorldBase.WorldObjects
{
    public class Rock : WorldObject
    {
        public Rock(Grid currentGrid) : base(currentGrid)
        {
        }

        public override double Density { get { return 0.002; } } // kg/cm^3

        public override double SpecificHeatCapacity { get { return 800; } }// J/( kg·K )

        public override double ThermalConductivity { get { return 2.5 * 100; } }// W/(m·K) 的一百倍

        public override void Do()
        {
            
        }

        public override WorldObject GetObjectFromGridAsTheSameType(Grid grid)
        {
            return grid.Rock;
        }
    }
}
