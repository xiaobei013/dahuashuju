using System.Reflection.Emit;

namespace ConsoleApp1.Sort
{
    public class SqList
    {
        private static int MAXSIZE = 10;

        public SqList(int size)
        {
            MAXSIZE = 10;
        }

        public int[] r = new int[MAXSIZE +1]; //多加一位是为了 R[0]存储哨兵或者临时变量

        public int length;
    }
    
    
    public static class SqListExtension{
        private static void Swap(this SqList l, int i, int j)
        {
            int temp = l.r[i];
            l.r[i] = l.r[j];
            l.r[j] = temp;
        }
    
        //最简单实现,只是简单的交换，没有满足交换相邻的两个关键字的思想
        public static void BubbleSort0(this SqList l)
        {
            //注意这里i是从1开始
            for (int i = 1; i < l.length; i++)
            {
                for (int j = i+1; j < l.length; j++)
                {
                    if (l.r[i] > l.r[j])
                    {
                        l.Swap(i,j);
                    }
                }
            }
        }

        public static void BubbleSort(this SqList l)
        {
            for (int i = 1; i < l.length; i++)
            {
                // 注意这里是从后往前排
                for (int j = l.length -1; j >= i; j--)
                {
                    //这里的写法要注意差异 
                    if (l.r[j] > l.r[j+1])
                    {
                        l.Swap(j,j+1);
                    }
                }
            }
        }
    }
    
}