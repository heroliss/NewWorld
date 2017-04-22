using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewWorldServer;
namespace NewWorldServer.Client.WorldObjects
{
    [Serializable]
    public class Oxygen : WorldObjectInfo
    {
        public Oxygen(GridInfo currentGrid) : base(currentGrid)
        {

        }

        public override double Density { get { throw new NotImplementedException(); } }

        public override double SpecificHeatCapacity { get { throw new NotImplementedException(); } }

        public override double ThermalConductivity { get { throw new NotImplementedException(); } }

    }
}
