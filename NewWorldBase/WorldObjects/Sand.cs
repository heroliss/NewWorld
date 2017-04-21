using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewWorldBase.WorldObjects
{
    public class Sand : WorldObject
    {
        public Sand(Grid currentGrid) : base(currentGrid)
        {
        }

        public override double Density { get { return 5; } }

        public override double SpecificHeatCapacity { get { return 5; } }

        public override double ThermalConductivity { get { return 5; } }

        public override void Do()
        {
            throw new NotImplementedException();
        }

        public override WorldObject GetObjectFromGridAsTheSameType(Grid grid)
        {
            return grid.Sand;
        }
    }
}
