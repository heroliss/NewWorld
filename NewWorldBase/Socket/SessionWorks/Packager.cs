using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewWorldServer.Core;
using System.Diagnostics;
using NewWorldServer.Core.WorldObjects;
using NewWorldServer.Socket.SessionWorks.GridsTakers;

namespace NewWorldServer.SessionWork
{
    /// <summary>
    /// 矩形区域方格信息打包
    /// </summary>
    public class Packager
    {
        private int objInfoSize;
        
        private byte[] buffer;
        private int currentBufferPos;
        /// <summary>
        /// 区域方块打包
        /// </summary>
        /// <param name="grids">包含区域方块迭代器,从最小下标开始，x，z，y依次递增</param>
        public byte[] EncodeGrids(IGridsTaker gridsTaker,int centerX,int centerY,int centerZ)
        {
            
            IEnumerable<Grid> grids = gridsTaker.TakeGrids(centerX,centerY,centerZ);
            buffer = new byte[GetBufferSize(gridsTaker.GetMaxGridsCount(),objInfoSize)]; //TODO：建立缓冲区可优化

            currentBufferPos = 0;
            PutIntoBuffer((byte)100); // 编码协议号

            IEnumerator<Grid> firstGrid = grids.GetEnumerator();
            firstGrid.MoveNext(); //获得迭代器返回的第一个方格

            PutIntoBuffer((byte)firstGrid.Current.X); // 加入第一个方格的位置信息
            PutIntoBuffer((byte)firstGrid.Current.Y);
            PutIntoBuffer((byte)firstGrid.Current.Z);
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
        
        public Packager(int objInfoSize = 4)
        {
            this.objInfoSize = objInfoSize;
        }

        private int GetBufferSize(int gridsCount,int objInfoSize/*可能会去掉该参数*/)
        {
            int bufferSize =
                       1 // 解码协议，这里恒为100
                     + 3 // 三个 1 byte网格x，y，z坐标（0-255），表示第一个方格坐标
                     + gridsCount //所有要传输信息的方格数量
                       * (1 // 1 byte 方格分割符（用255表示）
                         + 4 // float类型方格温度
                            + (objInfoSize // 物质的信息
                            + 1)  // 物质序号（质量为0的物质信息不传送）
                            * ObjsCreator.ObjTypeCount);
            return bufferSize;
        }

        private void PutIntoBuffer(byte data)
        {
            buffer[currentBufferPos] = data;
            currentBufferPos ++;
        }
        private void PutIntoBuffer(int data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            Debug.Assert(bytes.Length == 4);
            bytes.CopyTo(buffer, currentBufferPos);
            currentBufferPos += bytes.Length;
        }
        private void PutIntoBuffer(float data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            Debug.Assert(bytes.Length == 4);
            bytes.CopyTo(buffer, currentBufferPos);
            currentBufferPos += bytes.Length;
        }

    }
}
