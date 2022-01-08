namespace Sortttt
{
    public class SqList
    {
        private static int MAXSIZE = 10;

        /*public SqList(int size)
        {
            MAXSIZE = size;
        }*/
        //{0,5,3,4,6,2} length == 6 0  r[0] 是哨兵

        public int[] r = {0,5,3,4,6,2}; //多加一位是为了 R[0]存储哨兵或者临时变量

        //注意这里的length，是当前数组的存储数据的个数，只能动态维护
        public int length
        {
            get
            {
                return r.Length - 1;
            }
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 1; i < r.Length; i++)
            {
                if (i == r.Length - 1)
                {
                    s = s + r[i] ;
                }
                else
                {
                    s = s + r[i] + "," ;
                }
            }

            return s;
        }

        public string TestToString()
        {
            string s = "";
            for (int i = 0; i < r.Length; i++)
            {
                if (i == r.Length - 1)
                {
                    s = s + r[i] ;
                }
                else
                {
                    s = s + r[i] + "," ;
                }
            }

            return s;
        }
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
        
        //--总结--无论简单选择拍寻还是冒泡排序时间复杂度都是O(n^2)
        
        //直接插入排序straiht insertion sort

        
        public static void InsertSort(this SqList l)
        {
            int j;
            //从2开始，假设了1已经是再他该在的位置上了
            for (int i = 2; i <= l.length; i++)
            {
                //剩下的问题是看i插入到 i-1 的哪边？
                if (l.r[i] < l.r[i - 1]) //正序
                {
                    //哨兵，把小的放到里面
                    l.r[0] = l.r[i];
                    //注意这里的for循环
                    for (j = i - 1; l.r[j] > l.r[0]; j--)
                    {
                        l.r[j + 1] = l.r[j];//因为i-1更大，就要移动i-1，又因为j=i-1; 所以只要移动j往右移动（j+1）
                    }
                    l.r[j + 1] = l.r[0];
                }       
            }
        }
        
    }
}