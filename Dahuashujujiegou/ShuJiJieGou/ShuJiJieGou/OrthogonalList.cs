using System.Collections.Generic;

namespace ConsoleApp1
{
    public class OrthogonalList
    {
        public class VertexNode
        {
            public string data;
            //表示出边表的头
            public EdgeNode firstin;
            //指向该顶点的出边表中的第一个结点
            public EdgeNode firstout;
        }

        public class EdgeNode
        {
            //弧起点在顶点表的下标
            public int tailvex;
            //弧终点在顶点表的下标
            public int headvex;
            //入边表指针域
            public VertexNode headlink;
            public VertexNode taillink;
        }

        private VertexNode[] VertexNodes;

        public OrthogonalList(int count)
        {
            VertexNodes = new VertexNode[count];
        }

        public void SetNodeIn(int index, string data,EdgeNode headlink, EdgeNode taillink)
        {
            VertexNodes[index] ??= new VertexNode();


            VertexNodes[index].firstin = headlink;
        }
        
        public void SetNodeOut(int index, string data,EdgeNode node)
        {
            VertexNodes[index] ??= new VertexNode();
            if (VertexNodes[index].firstout == null)
            {
                VertexNodes[index].firstout = node;
            }
            else
            {
                
                
            }
        }
    }
}