using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewWorldBase.Server.WorldObjects
{
    public class Water : WorldObject
    {
        public Water(Grid currentGrid) : base(currentGrid)
        {
        }

        public override double Density { get { throw new NotImplementedException(); } }

        public override double SpecificHeatCapacity { get { throw new NotImplementedException(); } }

        public override double ThermalConductivity { get { throw new NotImplementedException(); } }

        public override void Do()
        {
            throw new NotImplementedException();
        }
    }
}
