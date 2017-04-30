using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewWorldServer.Core;
using System.Diagnostics;
using NewWorldServer.Core.WorldObjects;
namespace NewWorldServer.SessionWork
{
    public class Packager
    {
        private int objInfoSize;
        private int sizeX, sizeY, sizeZ;
        private byte[] buffer;
        private int currentBufferPos;
        /// <summary>
        /// 正方形区域打包
        /// </summary>
        /// <param name="grids">包含正方形区域的方块迭代器,从最小下标开始，x，z，y依次递增</param>
        public byte[] PackageGrids(IEnumerable<Grid> grids)
        {
            int bufferSize = 3 * 4 + sizeX * sizeY * sizeZ * (4 + 1 + (objInfoSize + 1) * ObjsCreator.ObjTypeCount);
            buffer = new byte[bufferSize];

            IEnumerator<Grid> firstGrid = grids.GetEnumerator();
            firstGrid.MoveNext();
            currentBufferPos = 0;
            PutIntoBuffer(firstGrid.Current.X);
            PutIntoBuffer(firstGrid.Current.Y);
            PutIntoBuffer(firstGrid.Current.Z);

            foreach (Grid grid in grids)
            {
                for (int i = 0; i < grid.Objs.Length; i++)
                {
                    if (grid.Objs[i].Mass != 0)
                    {
                        PutIntoBuffer((byte)i);
                        PutIntoBuffer((float)grid.Objs[i].Mass);
                    }
                }
                PutIntoBuffer(byte.MaxValue); //网格的结束符
                PutIntoBuffer((float)grid.Temperature);
            }
            return buffer.Take(currentBufferPos).ToArray();
        }
        private void PutIntoBuffer(byte data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            bytes.CopyTo(buffer, currentBufferPos);
            currentBufferPos += bytes.Length;
        }
        private void PutIntoBuffer(int data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            bytes.CopyTo(buffer, currentBufferPos);
            currentBufferPos += bytes.Length;
        }
        private void PutIntoBuffer(float data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            bytes.CopyTo(buffer,currentBufferPos);
            currentBufferPos += bytes.Length;
        }

        public IEnumerable<Grid> getSquareGrids(Grid[,,] grids,int centerX,int centerY,int centerZ)
        {
            int startX = centerX - sizeX / 2 < 0 ? 0 : centerX - sizeX / 2;
            int startY = centerY - sizeY / 2 < 0 ? 0 : centerY - sizeY / 2;
            int startZ = centerZ - sizeZ / 2 < 0 ? 0 : centerZ - sizeZ / 2;
            int endX = centerX + sizeX / 2 > grids.GetLength(0) - 1 ? grids.GetLength(0) - 1 : centerX + sizeX / 2;
            int endY = centerY + sizeY / 2 > grids.GetLength(1) - 1 ? grids.GetLength(1) - 1 : centerY + sizeY / 2;
            int endZ = centerZ + sizeZ / 2 > grids.GetLength(2) - 1 ? grids.GetLength(2) - 1 : centerZ + sizeZ / 2;

            for (int x = startX; x < endX; x++)
            {
                for (int z = startZ; z < endZ; z++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        yield return grids[x, y, z];
                    }
                }
            }
        }

        public Packager(int sizeX = 25, int sizeZ = 25, int sizeY = 10, int objInfoSize = 4)
        {
            this.objInfoSize = objInfoSize;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
        }
    }
}
