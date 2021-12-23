using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class ExtensionTest
    {
        public static void ShowItems<T>(this IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }
}
