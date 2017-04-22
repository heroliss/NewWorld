using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWorldServer.Server.WorldObjects
{
    static public class IndexMapping
    {
        static public Dictionary<string, WorldObject> CreateObjsList(Grid currentGrid)
        {
            Dictionary<string, WorldObject> objs = new Dictionary<string, WorldObject>();
            objs["rock"] = new Rock(currentGrid);
            objs["silver"] = new Silver(currentGrid);

            return objs;
        }
    }
}
