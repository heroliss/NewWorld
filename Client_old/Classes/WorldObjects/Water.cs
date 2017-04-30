using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewWorldServer.Structs.WorldObjects
{
    [Serializable]
    public class Water : WorldObjectInfo
    {
        public Water(GridInfo currentGrid) : base(currentGrid)
        {
        }

        public override double Density { get { throw new NotImplementedException(); } }

        public override double SpecificHeatCapacity { get { throw new NotImplementedException(); } }

        public override double ThermalConductivity { get { throw new NotImplementedException(); } }
    }
}
