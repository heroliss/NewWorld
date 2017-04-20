using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewWorldBase;
namespace NewWorldBase.WorldObjects
{
    public class Oxygen : WorldObject
    {
        public Oxygen(Grid currentGrid) : base(currentGrid)
        {
        }

        public override double Density => throw new NotImplementedException();

        public override double SpecificHeatCapacity => throw new NotImplementedException();

        public override double ThermalConductivity => throw new NotImplementedException();

        public override void Do()
        {
            throw new NotImplementedException();
        }
    }
}
