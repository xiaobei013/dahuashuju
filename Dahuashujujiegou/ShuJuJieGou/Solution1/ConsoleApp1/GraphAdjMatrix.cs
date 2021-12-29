using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Text;
//https://www.cnblogs.com/SimplePoint/p/9270805.html
namespace ConsoleApp1
{
    //1. 无向图或无向网的邻接矩阵一定是一个对称矩阵。因此，在具体存放邻接矩阵时只需存放上（或下）三角矩阵的元素即可
    //2. 可以很方便地查找图中任一顶点的度。对于无向图或无向网而言，
    //顶点 vi 的度就是邻接矩阵中第 i 行或第 i 列中非 0 或非∞的元素的个数。
    //对于有向图或有向网而言，顶点 vi 的入度是邻接矩阵中第 i 列中非 0 或非∞的元素的个数，
    //顶点 vi 的出度是邻接矩阵中第 i 行中非 0 或非∞的元素的个数
    //3. 可以很方便地查找图中任一条边或弧的权值，只要 A[i][j]为 0 或 ∞，
    //就说明顶点 vi 和 vj 之间不存在边或弧。
    //但是，要确定图中有多少条边或弧，则必须按行、按列对每个元素进行检测，所花费的时间代价是很大的。这是用邻接矩阵存储图的局限性。 
    
    #region 邻接矩阵 AdjacencyMatrix
    //图的存储结构，邻接矩阵， 用数组的方式存储数据
    public class GraphAdjMatrix
    {
        //设施两个数组
        //顶点数组 ver-tex[4]={v0,v1,v2,v3}
        //边数组[4][4] 为矩阵
        //无向图 ， 有向图 ， 网图 均能使用此方式
        //例子使用无向图
        
        //顶点表 存储图中顶点的信息(可以是任意类型的数组)
        public int[] vertex;
        //邻接矩阵 存储顶点之间相邻的信息
        public int[,] matrix;
        //图中当前的顶点数和边数
        public int NumVertexes, NumEdges;

        public GraphAdjMatrix(){}
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numVertexes"> 顶点数</param>
        /// <param name="numEdges"> 边数</param>
        public GraphAdjMatrix(int numVertexes ,int numEdges)
        {
            NumVertexes = numVertexes;
            NumEdges = numEdges;
            
            vertex = new int[numVertexes];
            for (int i = 0; i < numVertexes; i++)
            {
                vertex[i] = i;
            }
            //o(n^2)
            matrix = new int[numVertexes,numVertexes] ;
            for (int j = 0; j < numVertexes; j++)
            {
                for (int k = 0; k < numVertexes; k++)
                {
                    matrix[j,k] = Graph.INFINITY;
                }
            }
            
        }

        public void SetEdges(int x, int y, int w)
        {
            matrix[x, y] = w;
            matrix[y, x] = w;
        }


        public static Edge[] GetEdges(GraphAdjMatrix G)
        {
            Edge[] edges = new Edge[G.NumEdges];
            int[] visited = new int[G.NumVertexes];
            int x = 0;
            for (int i = 0; i < G.NumVertexes; i++)
            {
                for (int j = 0; j < G.NumVertexes; j++)
                {
                    if (visited[j] != 1 && G.matrix[i,j] != Graph.INFINITY)
                    {
                        Edge e = new Edge() {begin = i, end = j, weight = G.matrix[i, j]};
                        edges[x] = e;
                        x++;
                    }
                }
                visited[i] = 1;
            }
            return edges;
        }
        
        //用于记录每次查询到的节点
        public static int[] visited;
        // 邻接矩阵的深度优先递归算法
        public static void DFS(GraphAdjMatrix G, int i)
        {
            visited[i] = 1;
           
            Console.WriteLine("邻接矩阵DFS ==> i : " + G.vertex[i].ToString());
            
            for (int j = 0; j < G.NumVertexes; j++)
            {
                //有联通 并且 还没有走过
                //注意这里 visited 的 下标是 j
                if (G.matrix[i,j] == 1 && visited[j] != 1)
                {
                    DFS(G, j);
                }
            }
        }

        // 深度优先（遍历）
        public static void DFSTraverse(GraphAdjMatrix G)
        {
            visited = new int[G.NumVertexes];
            
            for (int i = 0; i < G.NumVertexes; i++)
            {
                if (visited[i] != 1)
                {
                    DFS(G, i);
                } 
            }
        }
        
        //邻接矩阵的广度优先算法 (遍历)
        public static void BFSTraverse(GraphAdjMatrix G)
        {
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < G.NumVertexes; i++)
                visited[i] = 0;  // 初始化访问列表

            for (int j = 0; j < G.NumVertexes; j++)
            {
                if (visited[j] == 0)
                {
                    visited[j] = 1;
                    Console.WriteLine("邻接矩阵广度优先 ==> " + G.vertex[j]);
                    queue.Enqueue(j); // 压入栈
                    
                    while (queue.Count > 0)
                    {
                        j = queue.Dequeue();
                        
                        //查找每一个和当前节点相连的点
                        for (int k = 0; k < G.NumVertexes; k++)
                        {
                            if (visited[k] == 0 && G.matrix[j,k] == 1) //可以连通
                            {
                                visited[k] = 1;
                                Console.WriteLine("邻接矩阵广度优先 ==> " + G.vertex[k]);
                                queue.Enqueue(k); //压入了 j 所有连通的节点。
                            }
                        }
                    }
                }
            }
        }
    }
    
    #endregion

    #region 连接表
    //邻接表 数组与链表相结合的存储方式 
    //边表节点 它有两个域，一个是邻接顶点域 adjvex，存放邻接顶点的信息，实际上就是邻接顶点在顶点数组中的序号；一个是引用域 next，存放下一个邻接顶点的结点的地址
    public class EdgeNode
    {
        //顶点在数组中的下标
        public int adjVex;
        //存储权值，对于非网图可以不要
        public int weight;

        public EdgeNode next;

        public EdgeNode(int adjVex)
        {
            this.adjVex = adjVex;
            this.next = null;
        }
    }
    // 顶点表结构 顶点结点有两个域 1. 存储数据的数据域 ，2. 引用域 firstedge，存放该顶点的邻接表的第一个结点的地址 实际上保存的是与该顶点相关的边或弧的信息
    public class VertexNode
    {
        //顶点域存储顶点信息
        public string data;
        public EdgeNode firstedge;
    }
    
    //邻接表 是图的一种顺序存储与链式存储相结合的存储结构，类似于树的孩子链表表示法
    //==空间== 若无向图中有 n 个顶点和 e 条边，则它的邻接表需 n 个顶点结点和 2e 个邻接表结点，在边稀疏 (e << n(n-1)/2) 的情况下，用邻接表存储图比用邻接矩阵节省存储空间，当与边相关的信息较多时更是如此
    public class GraphAdjList
    {
        //顺序存储指的是图中的顶点信息用一个顶点数组来存储
        public VertexNode[] VertexNodes;
        public int NumVertexes, NumEdges;
        
        public GraphAdjList(int numVertexes, int numEdges ,string[] datas)
        {
            this.NumVertexes = numVertexes;
            this.NumEdges = numEdges;
            this.VertexNodes = new VertexNode[numVertexes];
            //读入顶点信息，建立定点表
            for (int i = 0; i < numVertexes; i++)
            {
                VertexNodes[i] = new VertexNode {data = datas[i], firstedge = null};
            }
            
            //初始化遍历图时的节点缓存
            _visited = new int[numVertexes];
            
        }
        
        //无向图设置边
        public void SetEdges(int fromVertexNode, int toVertexNode)
        {
            if (VertexNodes == null)
            {
                return;
            }
            //无向邻接表即为双向邻接表
            var e = new EdgeNode(fromVertexNode) {next = VertexNodes[toVertexNode].firstedge};
            VertexNodes[toVertexNode].firstedge = e;
            
            e = new EdgeNode(toVertexNode) {next = VertexNodes[fromVertexNode].firstedge};
            VertexNodes[fromVertexNode].firstedge = e;
        }

        //获得顶点数量
        public int GetNumOfVertex()
        {
            return VertexNodes.Length;
        }
        
        //获得边数
        public int GetNumOfEdges()
        {
            int i = 0;

            for (int j = 0; j < VertexNodes.Length; j++)
            {
                var edge = VertexNodes[j].firstedge;

                while (edge != null)
                {
                    i++;
                    edge = edge.next;
                }
            }
            return i / 2;
        }
        
        //判断是否是图的顶点
        public bool IsNode(VertexNode node)
        {
            for (int i = 0; i < VertexNodes.Length; i++)
            {
                if (VertexNodes[i].Equals(node))
                {
                    return true;
                }
            }
            return false;
        }
        
        //判断v1,和 v2 之间的边是否存在
        public bool IsEdge(VertexNode v1, VertexNode v2)
        {
            //在图里面
            if (!IsNode(v1) || !IsNode(v2))
            {
                return false;
            }
            //不能是一样的

            int index_V1 = 0;
            for (int i = 0; i < VertexNodes.Length; i++)
            {
                if (v1 == VertexNodes[i])
                {
                    index_V1 = i;   
                }
            }

            var e = v2.firstedge;

            while (e != null)
            {
                if (e.adjVex == index_V1)
                {
                    return true;
                }

                e = e.next;
            }
            return false;
        }

        private static int[] _visited = null;

        public static void ClearVisited()
        {
            for (int i = 0; i < _visited.Length; i++)
            {
                _visited[i] = 0;
            }
        }
        
        //邻接表找邻接点所需要的时间取决于 顶点 和 边的数量，所以时O(n+e)。显然对于点多，边少的稀疏图来说，邻接表的结构是的算法在时间效率上大大的提升
        //邻接表的深度优先递归算法
        public static void DFS( GraphAdjList GL, int i )
        {
            EdgeNode p = null;
            _visited[i] = 1; // 设置为已经遍历过
            Console.WriteLine("DFS 访问的节点 --> " + GL.VertexNodes[i].data);
            p = GL.VertexNodes[i].firstedge;

            while (p != null)
            {
                if (_visited[p.adjVex] != 1) //没有访问过的就可以递归调用
                    DFS(GL, p.adjVex);

                p = p.next;
            }
        }
        
        //邻接表的深度优先遍历操作
        public static void DFSTraverse(GraphAdjList GL)
        {
            ClearVisited();
            for (int j = 0; j < GL.NumVertexes; j++)
            {
                if (_visited[j] == 0)
                {
                    DFS(GL,j);
                }
            }
        }

        //广度优先算法
        public static void BFSTraverse(GraphAdjList GL)
        {
            ClearVisited();
            Queue<int> queue = new Queue<int>();
            EdgeNode edgeNode;
            
            for (int i = 0; i < GL.NumVertexes; i++)
            {
                if (_visited[i] == 0)
                {
                    _visited[i] = 1;
                    Console.WriteLine("邻接表广度优先 => " + GL.VertexNodes[i].data);
                    queue.Enqueue(i);
                    
                    //中层循环   小循环会把节点 根据顺序 压入 queue ，循环遍历就好
                    while (queue.Count > 0)
                    {
                        var index = queue.Dequeue();
                        edgeNode = GL.VertexNodes[index].firstedge;
                        
                        //小循环是当前节点的 所有连通路径，这样能保证循环完所有的儿子
                        while (edgeNode != null)
                        {
                            //如果节点没有被访问过
                            if (_visited[edgeNode.adjVex] == 0)
                            {
                                Console.WriteLine("邻接表广度优先 => " + GL.VertexNodes[edgeNode.adjVex].data);
                                _visited[edgeNode.adjVex] = 1;
                                //压入下一个
                                queue.Enqueue(edgeNode.adjVex);
                            }
                            edgeNode = edgeNode.next;
                        }
                    }
                }
            }
        }
        
        private StringBuilder _builder = new StringBuilder();
        /// <summary>
        /// 展示邻接表
        /// </summary>
        public override string ToString()
        {
            _builder.Clear();
            for (int i = 0; i < VertexNodes.Length; i++)
            {
                _builder.Append("顶点 i: " + i + " 数据: " + VertexNodes[i].data + "--FirstEdge--\n");

                var temp = VertexNodes[i].firstedge;
                while (temp != null)
                {
                    _builder.Append("to: " + temp.adjVex + "\n");
                    temp = temp.next;
                }
            }
            return _builder.ToString();
        }
    }
    //有时，为了便于确定顶点的入度或者以顶点 vi 为头的弧，可以建立一个 **有向图** 的逆邻接表，即对每个顶点 vi 建立一个以 vi 为头的弧的邻接表。下图（b）是（a）邻接表和（b）逆邻接表。 
    //在建立邻接表或逆邻接表时，若输入的顶点信息即为顶点的编号，则建立邻接表的时间复杂度为 O（n+e），否则，需要查找才能得到顶点在图中的位置，则时间复杂度为 O（n*e）。 
    //在邻接表上很容易找到任一顶点的第一个邻接点和下一个邻接点。但要判定任意两个顶点（vi 和 v j）之间是否有边或弧相连，则需查找第 i 个或 j 个邻接表，因此，不如邻接矩阵方便。
    // 有向图只存在可以通行和不可同行的问题，其遍历方式和无向图一致
   
    //==时间== 对于 n个顶点，e条边的图 两种储存结构的 深度优先算法 ：
    //邻接矩阵由于是二维数组，要查找每个顶点的邻接点需要访问矩阵中所有元素，因此需要O(n^2)的时间。
    //邻接表做存储结构时，找邻接点所需要时间取决于 顶点 和 边 的数量，所以是O(n+e)。显然对于点多边少的稀疏图来说，邻接表的结构使得算法在时间效率上大大提高
    
    #endregion

    public struct Edge
    {
        public int begin;
        public int end;
        public int weight;
    }
    
    public static class Graph
    {
        public static int INFINITY = Int32.MaxValue;
        //private static int MAXVEX = 20; //此处设置为顶点个数的最大值就可以
        /// <summary>
        /// Prim算法--最小生成树
        /// </summary>
        /// <param name="G"></param>
        public static void MiniSpanTree_Prim(GraphAdjMatrix G)
        {
            int _min = 0;
            int j = 0;
            int k = 0;
            //保存相关顶点下标
            int[] adjvex = new int[G.NumVertexes];
            //保存相关顶点间边的权值
            int[] lowcost = new int[G.NumVertexes];

            //初始化权重为0，在这里就是此下标的顶点已加入生成树，此处表示V0 已经被收纳到最小生成树中， 之后凡是 lowcost数组中的值被设置为0就表示此下标的顶点被纳入最小生成树。
            lowcost[0] = 0;
            //初始化第一个顶点的下标为0（事实上，最小生成树从哪个顶点开始计算都无所谓）
            adjvex[0] = 0;

            //遍历除了 邻接矩阵的 第一行数据 ，将数值赋值给lowcost函数 ，并初始化 adjvex 的值
            for (int i = 1; i < G.NumVertexes; i++)
            {
                // 将 V0 顶点与之有 边的权值存入数组
                lowcost[i] = G.matrix[0, i];  // 第一行的边权重赋值 第一次{0，10，65535，65535，65535，11，65535，65535，65535}
                // 初始话所有的v0的下标？？
                adjvex[i] = 0;
            }
            
            // 两次for NumVertexes 给了时间复杂度 O(n^2)
            for (int i = 1; i < G.NumVertexes; i++)
            {
                _min = INFINITY; //
                j = 1; //是用来做顶点下标循环的变量，因为lowcost的下标0，就是树的根节点。所以初始化为1.
                k = 0; //存下权重最小的index ，遍历后使用
                while (j < G.NumVertexes)
                {
                    //如果权值不为0 不为0意味着已经是树的顶点，不再参与树的创建。且权值小于min，一开始的时候_min是等于INFINITY的。即可以连通的才能通过断言
                    if (lowcost[j] != 0 && lowcost[j] < _min)
                    {
                        //设置当前的权重的最小值。 这里遍历了一个节点所有的路径，并找出最小权重的路径。
                        _min = lowcost[j];
                        //将当前最小值的下标存入k
                        k = j;
                    }
                    j++;
                }
                
                //打印当前顶点边中权值最小边链接的顶点
                Console.WriteLine("(" + adjvex[k] + "," + k + ")");
                //将当前顶点的权值设置0，表示此顶点已经完成任务
                lowcost[k] = 0;
                
                //循环所有顶点 刷新lowcost的值，当前的 matrix[k] 行中所有的贯通的边都加入到lowcost中
                for (int z = 1; z < G.NumVertexes; z++)
                {
                    //若下标为 k 顶点各边权值小于此前这些顶点未被加入生成树权值
                    if (lowcost[z] != 0 && G.matrix[k,z] < lowcost[z])
                    {
                        //将较小的权值存入lowcost
                        lowcost[z] = G.matrix[k, z];
                        //将下标为k的顶点存入adjvex；对应lowcost 的边的权重 再adjvex中存入对应的 index
                        adjvex[z] = k;  
                    }
                }
            }
        }


        
        // 直接以边来构建生成树，只不过构建时要考虑是否会形成环路。
        /// <summary>
        /// 克鲁斯卡尔 算法 主要针对边来扩展，变数少的时候效率会非常高，对于稀疏图由很大的优势，  普里姆对于稠密图，季边数非常多的情况会更好一些
        /// </summary>
        /// <param name="G"></param>
        public static void MiniSpanTree_Kruskal(GraphAdjMatrix G)
        {
            int n;
            int m;
            
            //定义边集数组 , 边集合数组由邻接矩阵G转化为边集数组，并按权重由小到大排序 
            GetEdges(G);
            //上面的方法之后， edges 初始化赋值
            
            //边根据权重排序
            SortHelp.SelectSort<Edge, int> (edges, p=>p.weight);

            for (int i = 0; i < G.NumEdges; i++)
            {
                Console.WriteLine($"排序后的边起点：{edges[i].begin} 终点：{edges[i].end} 权重：{edges[i].weight}");
            }
            
            //判断边与边是否形成环路 
            int[] parent = new int[G.NumEdges];
            Console.WriteLine("------------Kruskal形成最小树-----------");
            //            
            for (int i = 0; i < G.NumEdges; i++)
            {
                n = Find(parent, edges[i].begin);  //第一次返回0
                
                m = Find(parent, edges[i].end);  // 
                if (n != m) //连通的分量
                {
                    //将此边的 结尾 和 起点 存入 parent中， index：起点， value：终点
                    parent[n] = m;
                    Console.WriteLine($"({edges[i].begin},{edges[i].end}){edges[i].weight}");
                }
                
            }
        }
        
        //记录到Parent中的 顶点和index形成了边的连通
        private static int Find(int[] parent, int f )
        {
            while (parent[f] > 0)
            {
                f = parent[f];
            }

            return f;
        }

        private static int[] visited;

        private static Edge[] edges;
        
        private static void GetEdges(GraphAdjMatrix G)
        {
            edges = new Edge[G.NumEdges];
            visited = new int[G.NumVertexes];
            int index = 0;
            Console.WriteLine("顶点数：" + G.NumVertexes);
            for (int i = 0; i < G.NumVertexes; i++)
            {
                visited[i] = 1;
                
                for (int j = 0; j < G.NumVertexes; j++)
                {
                    if ( visited[j] != 1 && G.matrix[i,j] != INFINITY)
                    {
                        var edge = new Edge {begin = i, end = j, weight = G.matrix[i, j]};

                        edges[index] = edge;
                        index++;
                        Console.WriteLine($"GetEdges起点：{edge.begin} 终点：{edge.end} 权重：{edge.weight}");
                    }
                }
            }
        }

        //dijkstra算法， 最小路径 迪科斯彻算法（无启发函数版A星）
        /// <summary>
        /// 找最小路径
        /// </summary>
        /// <param name="G"></param>
        /// <param name="v0"></param>
        /// <param name="Patharc">用于存储最短路径下标的数组 </param>
        /// <param name="ShortPathTable"> 用于存储到各点最短路径的权值和 , 就是V0到各个顶点的最短路径</param>
        static void ShortestPath_Dijkstra(GraphAdjMatrix G, int v0, int[] Patharc, int[] ShortPathTable)
        {
            int k = 0, min = INFINITY;
            // final[w] 表示求得顶点V0至Vw的最短路径 
            int[] final = new int[9];
            
            //初始化所有的顶点 为位置最短路径状态
            for (int v = 0; v < G.NumVertexes; v++)
            {
                final[v] = 0;
                ShortPathTable[v] = G.matrix[v0, v];//将与v0点有连线的顶点加上权值， v0到各个顶点的权重 初始化权重
                Patharc[v] = -1; //初始化路径数组P为-1
            }
            
            //v0至v0的路径是0
            ShortPathTable[v0] = 0;
            //v0到v0不需要求路径
            final[v0] = 1;
            
            //开始主循环，每次求得 v0与一个顶点得最短路径，为此v从1开始！
            for (int v = 1; v < G.NumVertexes; v++)
            {
                //当前所知离 v0顶点的最近距离
                min = INFINITY; //先设置成极大值，通过w的循环，找的最小的 k 和  ShortPathTable[w] (就是循环找最小)
                for (int w = 0; w < G.NumVertexes; w++)
                {
                    //final 是0的时候并且 ShortPathTable[w] 小于当前的最小
                    if (final[w] == 0 && ShortPathTable[w] < min)
                    {
                        k = w; // 存下来能走的顶点权值最小得 index；
                        min = ShortPathTable[w];
                    }
                }

                //k表示与V0最近的顶点是V1，并且由ShortPathTable[k] = 
                
                final[k] = 1; //访问路径最后会变为{1,1,1,1,...}
                
                //修正当前最短路径及距离 修改权重
                //虽然找了从V0能到达的最小路径，但是找到的点Vx，能到达这个点的最短距离不一定就是V0到达的
                for (int w = 0; w < G.NumVertexes; w++)
                {
                    //如果经过Vk顶点的路径比现在这条路径的长度短的话，就要从新替换, 这里很重要，从新补救一次
                    //此时ShortPathTable还是上次行的权重，k这次遍历找到的最小的一行的index
                    //final[w] == 1排除已经查找过的路径
                    if (final[w] == 0 && min + G.matrix[k, w] < ShortPathTable[w])
                    {
                        // 说明有最小路径，修改D[w], P[w]
                        ShortPathTable[w] = min + G.matrix[k, w];
                        Patharc[w] = k; //存的是V0 到 Vn，Vm，Vz 的前驱是Vk
                    }
                }
            }
        }


        /// <summary>
        /// floyd算法 O(n^3)
        /// </summary>
        public static void ShortestPath_Floyd(GraphAdjMatrix G)
        {
            int[,] P = new int[G.NumVertexes, G.NumVertexes]; //Pathmatirx
            int[,] D = new int[G.NumVertexes, G.NumVertexes]; //ShortPathTable
            
            // 初始化P和D
            for (int v = 0; v < G.NumVertexes; v++)
            {
                for (int w = 0; w < G.NumVertexes; w++)
                {
                    D[v, w] = G.matrix[v, w];
                    P[v, w] = w;
                }
            }
            
            // 核心算法
            for (int k = 0; k < G.NumVertexes; k++)
            {
                for (int v = 0; v < G.NumVertexes; v++)
                {
                    for (int w = 0; w < G.NumVertexes; w++)
                    {
                        if ( D[v, w] > D[v, k] + D[k, w])
                        {
                            // 如果经过下标为k顶点路径比原两点间路径更短，将当前两点间权值设为更小的一个
                            D[v, w] = D[v, k] + D[k, w];
                            // 路径设置经过下标为k的顶点
                            P[v, w] = P[v, k];
                        }
                    }
                }
            }
        }

        //floyd 算法获得最小路径的方法
        public static void ShowShortestPath_Floyd()
        {
            
        }
    }
}