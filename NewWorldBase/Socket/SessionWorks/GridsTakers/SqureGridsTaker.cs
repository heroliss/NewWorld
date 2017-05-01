using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewWorldServer.Core;
using NewWorldServer.Core.WorldObjects;

namespace NewWorldServer.Socket.SessionWorks.GridsTakers
{
    class SqureGridsTaker : IGridsTaker
    {
        private int windowSizeX, windowSizeY, windowSizeZ;
        private Grid[,,] grids;

        public int WindowSizeX { get => windowSizeX; }
        public int WindowSizeY { get => windowSizeY; }
        public int WindowSizeZ { get => windowSizeZ; }

        public IEnumerable<Grid> TakeGrids(int centerX, int centerY, int centerZ)
        {
            int startX = centerX - windowSizeX / 2 < 0 ? 0 : centerX - windowSizeX / 2;
            int startY = centerY - windowSizeY / 2 < 0 ? 0 : centerY - windowSizeY / 2;
            int startZ = centerZ - windowSizeZ / 2 < 0 ? 0 : centerZ - windowSizeZ / 2;
            int endX = centerX + windowSizeX / 2 > grids.GetLength(0) - 1 ? grids.GetLength(0) - 1 : centerX + windowSizeX / 2;
            int endY = centerY + windowSizeY / 2 > grids.GetLength(1) - 1 ? grids.GetLength(1) - 1 : centerY + windowSizeY / 2;
            int endZ = centerZ + windowSizeZ / 2 > grids.GetLength(2) - 1 ? grids.GetLength(2) - 1 : centerZ + windowSizeZ / 2;

            for (int y = startY; y <= endY; y++)
            {
                for (int z = startZ; z <= endZ; z++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        yield return grids[x, y, z];
                    }
                }
            }
        }

        public int GetMaxGridsCount()
        {
            return windowSizeX * windowSizeY * windowSizeZ;
        }

        public SqureGridsTaker(Grid[,,] grids, int sizeX, int sizeY, int sizeZ)
        {
            this.grids = grids;
            this.windowSizeX = sizeX;
            this.windowSizeY = sizeY;
            this.windowSizeZ = sizeZ;

        }
    }
}
