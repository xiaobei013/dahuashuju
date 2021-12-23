using System;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        private const bool V = true;

        static void Main(string[] args)
        {
            //TreeTest();
            //GraphAdjListText();
            //GraphAdjMatrixText1();

            //TestMiniSpanTree_Prim();
            //TestMiniSpanTree_Kruskal();
            //sortTest();

            // 静态扩展
            Action a = "jimer".ShowItems;

            a();
        }
        
        static void sortTest()
        {
            // int[] arr = {9, 2, 1, 0, 8, 3, 5, 4, 6, 7};
            // SortTest.SelectSort(arr);
            //
            // for (int i = 0; i < arr.Length; i++)
            // {
            //     Console.WriteLine(arr[i]);
            // }
            
            data[] datas = {new data{d =1}, 
                new data{d =9},
                new data{d =2}, 
                new data{d =0}, 
                new data{d =8}, 
                new data{d =3}, 
                new data{d =5}, 
                new data{d =4}, 
                new data{d =6}, 
                new data{d =7}};

            SortHelp.SelectSort(datas, p => p.d);

            for (int i = 0; i < datas.Length; i++)
            {
                Console.WriteLine(datas[i].d);
            }
        }
        
        
        #region 图测试代码
        
        //图：
        //          0
        //        /   \   
        //      1  --- 2
        //     / |    /
        //    4  5   3
        static void GraphAdjMatrixText1()
        {
            GraphAdjMatrix adjMatrix = new GraphAdjMatrix(6,6);
            adjMatrix.SetEdges(0, 1, 1);
            adjMatrix.SetEdges(0, 2, 1);
            adjMatrix.SetEdges(1, 2, 1);
            adjMatrix.SetEdges(3, 2, 1);
            adjMatrix.SetEdges(3, 2, 1);
            adjMatrix.SetEdges(4, 1, 1);
            adjMatrix.SetEdges(5, 1, 1);
            
            GraphAdjMatrix.visited = new[] {0, 0, 0, 0, 0, 0};
            //GraphAdjMatrix.DFS(adjMatrix,0);
            //GraphAdjMatrix.DFSTraverse(adjMatrix);
            GraphAdjMatrix.BFSTraverse(adjMatrix);
        }
        
        //         A
        //     /  /  \
        //   B -- C -- D
        //  /      
        // E
        //  \
        //   F
        static void GraphAdjListText()
        {
            GraphAdjList graph = new GraphAdjList(6, 7 ,new string[] {"A","B","C","D","E","F"});
            graph.SetEdges(0,1);
            graph.SetEdges(0,2);
            graph.SetEdges(0,3);
            
            graph.SetEdges(1,2);
            graph.SetEdges(2,3);
            graph.SetEdges(1,4);
            graph.SetEdges(4,5);
            //GraphAdjList.DFS(graph, 0);

            GraphAdjList.BFSTraverse(graph);
        }


        static void TestMiniSpanTree_Prim()
        {
            GraphAdjMatrix graph = new GraphAdjMatrix(9, 13);
            graph.SetEdges(0, 1, 10);
            graph.SetEdges(0, 5, 11);
            
            graph.SetEdges(1, 2, 18);
            graph.SetEdges(1, 6, 16);
            graph.SetEdges(1, 8, 12);
            
            graph.SetEdges(2, 3, 22);
            graph.SetEdges(2, 8, 8);
            
            graph.SetEdges(3, 4, 20);
            graph.SetEdges(3, 6, 24);
            graph.SetEdges(3, 7, 16);
            graph.SetEdges(3, 8, 21);
            
            graph.SetEdges(4, 5, 26);
            graph.SetEdges(4, 7, 7);
            
            graph.SetEdges(5, 6, 17);
            
            graph.SetEdges(6, 7, 19);
            
            Graph.MiniSpanTree_Prim(graph);
        }
        
        static void TestMiniSpanTree_Kruskal()
        {
            GraphAdjMatrix graph = new GraphAdjMatrix(9, 15);
            graph.SetEdges(0, 1, 10);
            graph.SetEdges(0, 5, 11);
            
            graph.SetEdges(1, 2, 18);
            graph.SetEdges(1, 6, 16);
            graph.SetEdges(1, 8, 12);
            
            graph.SetEdges(2, 3, 22);
            graph.SetEdges(2, 8, 8);
            
            graph.SetEdges(3, 4, 20);
            graph.SetEdges(3, 6, 24);
            graph.SetEdges(3, 7, 16);
            graph.SetEdges(3, 8, 21);
            
            graph.SetEdges(4, 5, 26);
            graph.SetEdges(4, 7, 7);
            
            graph.SetEdges(5, 6, 17);
            
            graph.SetEdges(6, 7, 19);
            
            Graph.MiniSpanTree_Kruskal(graph);
        }
        #endregion

        
        
        static void TreeTest()
        {
            TreeNode node1 = new TreeNode("1",null);
            TreeNode node2 = new TreeNode("2",null);
            TreeNode node3 = new TreeNode("3",null);
            TreeNode node4 = new TreeNode("4",null);
            TreeNode node5 = new TreeNode("5",null);
            TreeNode node6 = new TreeNode("6",null);
            TreeNode node7 = new TreeNode("7",null);
            node1.AddNode(node2);
            node1.AddNode(node3);
            node1.AddNode(node4);
            node2.AddNode(node5);
            node2.AddNode(node6);
            node4.AddNode(node7); 
            //            1
            //         /  \   \
            //       2    3     4
            //     /  \          \
            //    5    6           7
            
            // TreeNode.RecursionDfs(node1); // 5 6 2 3 7 4 1
            // TreeNode.Bfs(node1);
            foreach (var node in node1)
            {
                Console.WriteLine(node.Name);
            }
        }
    }
}
