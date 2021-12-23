using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class TreeNode : IEnumerable<TreeNode>
    {
        public TreeNode(string name , TreeNode parent)
        {
            this.Parent = parent;
            this.Name = name;
            this.ChildList = new List<TreeNode>();
        }
        
        public string Name;
        public List<TreeNode> ChildList;
        public TreeNode Parent = null;

        public void AddNode(TreeNode node)
        {
            if (!ChildList.Contains(node) && node != this)
            {
                node.Parent = this;
                this.ChildList.Add(node);
            }
        }
        
        // 递归，深度算法 DFS
        public static void RecursionDfs(TreeNode root)
        {
            foreach (var node in root.ChildList)
            {
                if (root.ChildList.Count != 0)
                {
                    RecursionDfs(node);  
                }
            }
            Console.WriteLine(root.Name);
        }

        //Bfs 广度遍历
        public static void Bfs(TreeNode root)
        {
            //先进先出的特性保证了可以广度遍历树型结构
            Queue<TreeNode> stack= new Queue<TreeNode>();
            stack.Enqueue(root);
           
            while (stack.Count > 0)
            {
                // 每次弹出队列最前面的
                var node = stack.Dequeue();
                Console.WriteLine(node.Name);

                // 把所有的孩子压入队列
                foreach (var node1 in node.ChildList)
                {
                    stack.Enqueue(node1);
                }
            }
        }
        
        public IEnumerator<TreeNode> GetEnumerator()
        {
            return new IterTreeNode(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class IterTreeNode : IEnumerator<TreeNode>
    {
        private TreeNode _treeNode = null;
        Queue<TreeNode> _queue= new Queue<TreeNode>();
        private TreeNode _current;
        public IterTreeNode(TreeNode treeNode)
        {
            this._treeNode = treeNode;
            _queue.Enqueue(treeNode); // 1 向队列中压入第一个对象
        }
        
        public bool MoveNext()
        {
            if (_queue.Count == 0)
                return false;
            
            if ( _queue.Count > 0)
            {
                _current = _queue.Dequeue();
                foreach (var node1 in _current.ChildList)
                {
                    _queue.Enqueue(node1);
                }
            }
            else
            {
                _current = null;
            }
            
            return true;
        }

        public void Reset()
        {
            _current = null;
            _queue.Clear();
            _queue.Enqueue(_treeNode);
        }

        public TreeNode Current
        {
            get
            {
                return _current;
            }
        }

        object? IEnumerator.Current => Current;

        public void Dispose()
        {
           
        }
    }
    
}