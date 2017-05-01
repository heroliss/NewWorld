using NewWorldServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWorldServer.Socket.SessionWorks.GridsTakers
{
    public interface IGridsTaker
    {
        IEnumerable<Grid> TakeGrids(int centerX, int centerY, int centerZ);
        int GetMaxGridsCount();
    }
}
