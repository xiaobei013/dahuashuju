using System;

namespace ConsoleApp1
{
    public class data
    {
        public int d;
    }
    
    public static class SortTest
    {
        private static void swap(int[] arr, int min, int i)
        {
            var temp = arr[min];
            arr[min] = arr[i];
            arr[i] = temp;
        }
        
        public static void SelectSort(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int min = i; //每一趟循环比较时，min用于存放较小的元素的数组下标，这样一个批次的遍历就把最小的index 存到了min中

                for (int j = i; j < arr.Length ; j++)
                {
                    if ( arr[j] < arr[min])
                    {
                        min = j;
                    }
                }

                if (i != min)
                {
                    swap(arr,min ,i);
                }
            }
            
        }
        
    }


    public class SortHelp
    {
        public delegate TKey SelectHandler<T1, TKey>(T1 t);
        
        public static void SelectSort<T1 ,TKey>(T1[] arr, SelectHandler<T1, TKey> handler )  where TKey : IComparable , IComparable<TKey>
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int min = i;

                for (int j = i; j < arr.Length; j++)
                {
                    if (handler(arr[min]).CompareTo(handler(arr[j])) > 0)
                    {
                        min = j;
                    }
                }

                if (i != min)
                {
                    var temp = arr[min];
                    arr[min] = arr[i];
                    arr[i] = temp;
                }
            }
        }
    }
}