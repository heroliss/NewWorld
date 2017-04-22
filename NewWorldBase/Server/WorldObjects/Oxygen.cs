using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewWorldServer;
namespace NewWorldServer.Server.WorldObjects
{
    public class Oxygen : WorldObject
    {
        public Oxygen(Grid currentGrid) : base(currentGrid)
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
