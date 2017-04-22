using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewWorldServer.Client.WorldObjects
{
    [Serializable]
    public class Sand : WorldObjectInfo
    {
        public Sand(GridInfo currentGrid) : base(currentGrid)
        {
        }

        public override double Density { get { return 5; } }

        public override double SpecificHeatCapacity { get { return 5; } }

        public override double ThermalConductivity { get { return 5; } }
    }
}
