using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewWorldBase
{
    public class NoRepeatRandom : IEnumerable<int>  //剔除模式随机数生成器
    {
        private Random random;
        private int total;
        private int[] sequence;  //所有可能取值的列表
        public NoRepeatRandom(int total)
        {
            this.total = total;
            random = new Random();
            sequence = new int[total];
            Reset();
        }
        public void Reset()
        {
            for (int i = 0; i < total; i++)   //填充列表
            {
                sequence[i] = i;
            }
        }
        public IEnumerator<int> GetEnumerator()
        {
            int endIndex = total - 1;  //最后一个元素索引
            while (endIndex >= 0)
            {
                int index = random.Next(0, endIndex + 1);
                yield return sequence[index];
                sequence[index] = sequence[endIndex];
                endIndex--;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
