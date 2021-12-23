using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

    //拓扑排序
    public class ActivityOnVertexNetWork
    {
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
            //顶点的入度(为了使用拓扑排序)
            public int In;
            //顶点域存储顶点信息
            public string data;
            //边表头指针
            public EdgeNode firstedge;
        }

        //邻接表 是图的一种顺序存储与链式存储相结合的存储结构，类似于树的孩子链表表示法
        //==空间== 若无向图中有 n 个顶点和 e 条边，则它的邻接表需 n 个顶点结点和 2e 个邻接表结点，在边稀疏 (e << n(n-1)/2) 的情况下，用邻接表存储图比用邻接矩阵节省存储空间，当与边相关的信息较多时更是如此
        public class GraphAdjList
        {
            //顺序存储指的是图中的顶点信息用一个顶点数组来存储
            public VertexNode[] adjList;
            public int NumVertexes, NumEdges;

            public GraphAdjList(int numVertexes, int numEdges, string[] datas)
            {
                this.NumVertexes = numVertexes;
                this.NumEdges = numEdges;
                this.adjList = new VertexNode[numVertexes];
                //读入顶点信息，建立定点表
                for (int i = 0; i < numVertexes; i++)
                {
                    adjList[i] = new VertexNode {data = datas[i], firstedge = null};
                }

                //初始化遍历图时的节点缓存
                _visited = new int[numVertexes];
            }

            //无向图设置边
            public void SetEdges(int fromVertexNode, int toVertexNode)
            {
                if (adjList == null)
                {
                    return;
                }

                //无向邻接表即为双向邻接表
                var e = new EdgeNode(fromVertexNode) {next = adjList[toVertexNode].firstedge};
                adjList[toVertexNode].firstedge = e;

                e = new EdgeNode(toVertexNode) {next = adjList[fromVertexNode].firstedge};
                adjList[fromVertexNode].firstedge = e;
            }

            //获得顶点数量
            public int GetNumOfVertex()
            {
                return adjList.Length;
            }

            //获得边数
            public int GetNumOfEdges()
            {
                int i = 0;

                for (int j = 0; j < adjList.Length; j++)
                {
                    var edge = adjList[j].firstedge;

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
                for (int i = 0; i < adjList.Length; i++)
                {
                    if (adjList[i].Equals(node))
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
                for (int i = 0; i < adjList.Length; i++)
                {
                    if (v1 == adjList[i])
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
            public static void DFS(GraphAdjList GL, int i)
            {
                EdgeNode p = null;
                _visited[i] = 1; // 设置为已经遍历过
                Console.WriteLine("DFS 访问的节点 --> " + GL.adjList[i].data);
                p = GL.adjList[i].firstedge;

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
                        DFS(GL, j);
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
                        Console.WriteLine("邻接表广度优先 => " + GL.adjList[i].data);
                        queue.Enqueue(i);

                        //中层循环   小循环会把节点 根据顺序 压入 queue ，循环遍历就好
                        while (queue.Count > 0)
                        {
                            var index = queue.Dequeue();
                            edgeNode = GL.adjList[index].firstedge;

                            //小循环是当前节点的 所有连通路径，这样能保证循环完所有的儿子
                            while (edgeNode != null)
                            {
                                //如果节点没有被访问过
                                if (_visited[edgeNode.adjVex] == 0)
                                {
                                    Console.WriteLine("邻接表广度优先 => " + GL.adjList[edgeNode.adjVex].data);
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
                for (int i = 0; i < adjList.Length; i++)
                {
                    _builder.Append("顶点 i: " + i + " 数据: " + adjList[i].data + "--FirstEdge--\n");

                    var temp = adjList[i].firstedge;
                    while (temp != null)
                    {
                        _builder.Append("to: " + temp.adjVex + "\n");
                        temp = temp.next;
                    }
                }

                return _builder.ToString();
            }
        }

        #endregion
     
        //拓扑排序 若GL无回路，则输出拓扑排序序列并返回OK，若有回路返回Error
        //拓扑排序：就是对一个有向图构造拓扑序列的过程，构造过程会有两个结果：
        //1. 此网的全部顶点都被输出，则说明它是不存在的环90(回路)的AOV网
        //2. 如果输出的路径少了，哪怕少了一个，也说明这个网存在环，不是AOV网。
        // 主要解决一个工程是否能够顺序执行的问题
        public bool TopologicalSort(GraphAdjList GL)
        {
            EdgeNode e;
            int  k, gettop;
            //用于栈指针下标
            //int top = 0;
            //用于统计输出顶点的个数
            int count = 0;
            //建栈存储入度为0的顶点
            //int stack;
            Stack<int> stack = new Stack<int>();
            
            //遍历所有顶点
            for (int i = 0; i < GL.NumVertexes; i++)
                if (GL.adjList[i].In == 0)
                    stack.Push(i);   //入度为0的 顶点入栈
            
            while (stack.Count > 0)
            {
                //出栈
                gettop = stack.Pop();
                //打印此顶点
                Console.WriteLine($"key-value : {gettop} = {GL.adjList[gettop].data}" );
                count++;
                //对此顶点弧表遍历
                for (e = GL.adjList[gettop].firstedge; e != null; e = e.next)
                {
                    k = e.adjVex;
                    //将k号顶点邻接点的入度减1
                    if ( --GL.adjList[k].In == 0)
                    {
                        stack.Push(k);
                    }
                }
            }
            
            //如果count小于顶点数，说明存在环
            if (count < GL.NumVertexes)
            {
                return false;
            }

            return true;
        }
        
    }
