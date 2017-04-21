﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWorldBase.Client.WorldObjects
{
    static public class IndexMapping
    {
        static public Dictionary<string,WorldObjectInfo> CreateObjsInfoList(GridInfo currentGrid)
        {
            Dictionary<string, WorldObjectInfo> objs = new Dictionary<string, WorldObjectInfo>();
            objs["rock"] = new Rock(currentGrid);
            objs["silver"] = new Silver(currentGrid);

            return objs;
        }
    }
}
