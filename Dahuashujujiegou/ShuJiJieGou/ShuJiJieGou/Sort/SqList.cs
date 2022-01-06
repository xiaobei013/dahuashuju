namespace ConsoleApp1.Sort
{
    public class SqList
    {
        private static int MAXSIZE = 10;

        public SqList(int size)
        {
            MAXSIZE = 10;
        }

        public int[] r = new int[MAXSIZE + 1]; //多加一位是为了 R[0]存储哨兵或者临时变量

        //注意这里的length，是当前数组的存储数据的个数，只能动态维护
        public int length;
    }


    public static class SqListExtension
    {
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
                for (int j = i + 1; j < l.length; j++)
                {
                    if (l.r[i] > l.r[j])
                    {
                        l.Swap(i, j);
                    }
                }
            }
        }


        //常规的冒泡，如果把集合空间竖起来（从上往下依次增长），内存会从下往上一次排序，像气泡一样，所以叫冒泡
        public static void BubbleSort(this SqList l)
        {
            for (int i = 1; i < l.length; i++)
            {
                // 注意这里是从后往前排
                for (int j = l.length - 1; j >= i; j--)
                {
                    //****这里的写法要注意差异*** 比的是 j+1
                    if (l.r[j] > l.r[j + 1])
                    {
                        l.Swap(j, j + 1);
                    }
                }
            }
        }

        //冒泡优化：对于已经有序的数据，不再进行循环
        //代码改动的关键就是i再for循环中，增加了flag是否为true的判断，经过这样的改动，避免了无意义的循环判断
        public static void Bubblesort2(this SqList l)
        {
            bool flag = true; //flag作为标记，如果flag是true说明有过数据交换，否则停止循环
            for (int i = 1; i < l.length && flag; i++)
            {
                flag = false;
                for (int j = l.length - 1; j >= i; j--)
                {
                    if (l.r[j] > l.r[j+1])
                    {
                        //交换j 和 j+1 的值
                        l.Swap(j , j+1);
                        //如果有数据交换，则把flag设置成true
                        flag = true;
                    }
                }
            }
        }
    }
}