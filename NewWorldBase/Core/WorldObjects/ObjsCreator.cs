using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWorldServer.Core.WorldObjects
{
    public static partial class ObjsCreator
    {   
        public static int ObjTypeCount = 2;
        static public WorldObject[] CreateObjsList(Grid currentGrid)
        {
            WorldObject[] objs = new WorldObject[ObjTypeCount];
            objs[0] = new Rock(currentGrid);
            objs[1] = new Silver(currentGrid);
            return objs;
        }
    }
}
