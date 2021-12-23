//https://blog.csdn.net/cube454517408/article/details/107381323

using System.Globalization;

namespace ConsoleApp1.AVL
{
    // 平衡因子：我们将节点的左子树高度 - 右子树高度的差值命名为平衡因子BalanceFactor，简称bf
    
    
    public enum AVLRotateMode
    {
        LL,  //左旋
        LR,  //先左旋（左子树），再右旋
        RR,  //右旋
        RL,  //先右旋（右子树），再左旋
    }
    
    
    public interface ICompareSearch
    {
        int GetCompareValue();
    }
    
    
    public class AVLTree<T> where T:ICompareSearch
    {
        public T value;
        public AVLTree<T> lChild;
        public AVLTree<T> rChild;
        public int height;
    }

    public static class AVLExtenion
    {
        /// <summary>
        /// 左旋转
        /// </summary>
        /// <param name="node"></param>
        /// <typeparam name="T"></typeparam>
        public static void LRotate<T>(ref AVLTree<T> node)  where T : ICompareSearch
        {
            AVLTree<T> newNode = node.rChild;
            node.rChild = newNode.lChild;
            newNode.lChild = node;
            node = newNode;

            node.lChild.height = CalculateTreeHeight(node.lChild);
            node.height = CalculateTreeHeight(newNode);
        }

        /// <summary>
        /// 右旋转
        /// </summary>
        /// <param name="node"></param>
        /// <typeparam name="T"></typeparam>
        public static void RRotate<T>(ref AVLTree<T> node) where T : ICompareSearch
        {
            AVLTree<T> newNode = node.rChild;
            node.lChild = newNode.rChild;
            newNode.rChild = node;
            node = newNode;

            node.rChild.height = CalculateTreeHeight(node.rChild);
            node.height = CalculateTreeHeight(node);
        }

        /// <summary>
        /// 平衡树
        /// </summary>
        /// <param name="node"></param>
        /// <typeparam name="T"></typeparam>
        private static void BalanceAVLTree<T>(ref AVLTree<T> root) where T : ICompareSearch
        {
            //计算数高
            root.height = CalculateTreeHeight(root);
            //计算平衡因子，bf 绝对值小于等于1，为平衡状态。
            int bf = GetAVLTreeBalanceFactor(root);
            
            //要旋转
            if (bf < -1 || bf > 1)
            {
                AVLRotateMode mode = AVLRotateMode.LL;
                int bfChild = 0;
                //通过判断子节点，是否需要两次旋转
                if (bf < -1)
                {
                    //要左转
                    bfChild = GetAVLTreeBalanceFactor(root.rChild);
                    //要判断是只有一次左转，还是需要先右转再左转，判断方式就是bf的符号不一样
                    mode = bfChild <= 0 ? AVLRotateMode.LL : AVLRotateMode.RL; 
                }
                else
                {
                    //要右转
                    bfChild = GetAVLTreeBalanceFactor(root.lChild);
                    
                    mode = bfChild >= 0 ? AVLRotateMode.RR : AVLRotateMode.LR;
                }

                switch (mode)
                {
                    case AVLRotateMode.LL:
                        //左转
                        LRotate(ref root);
                        break;
                    case AVLRotateMode.LR:
                        LRotate(ref root.lChild);
                        RRotate(ref root);
                        break;
                    case AVLRotateMode.RR:
                        RRotate(ref root);
                        break;
                    case AVLRotateMode.RL:
                        RRotate(ref root.rChild);
                        LRotate(ref root);
                        break;
                }
                
            }
            
        }
        
        //公共代码
        /// <summary>
        /// 获取平衡二叉树某节点的平衡因子bf
        /// </summary>
        /// <param name="node"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static int GetAVLTreeBalanceFactor<T>(AVLTree<T> node) where T : ICompareSearch
        {
            return node != null ? GetAVLTreeHeight(node.lChild) - GetAVLTreeHeight(node.rChild) : 0;
        }
        
        private static int CalculateTreeHeight<T>(AVLTree<T> node) where T : ICompareSearch
        {
            return Max(GetAVLTreeHeight(node.lChild), GetAVLTreeHeight(node.rChild));
        }

        private static int GetAVLTreeHeight<T>(AVLTree<T> node) where T : ICompareSearch
        {
            return node?.height ?? 0;
        }
        

        private static int Max(int x, int y)
        {
            return x > y ? x : y;
        }

        public static bool InsertAVLTree<T>(T key, ref AVLTree<T> root)  where T: ICompareSearch
        {
            
            //else 中是二分查找，一直找到null后，就是要插入的位置
            if (root == null)
            {
                root = new AVLTree<T>();
                root.value = key;
                root.height = 1; // 树高默认是1
                root.lChild = null;
                root.rChild = null;
                return true;
            }
            else if (root.value.GetCompareValue() == key.GetCompareValue())
            {
                //树种已经存在相同的key，就不在重复插入了
                return false;
            }
            else if (root.value.GetCompareValue() > key.GetCompareValue())
            {
                InsertAVLTree(key, ref root.lChild);
            }
            else
            {
                InsertAVLTree(key, ref root.rChild);
            }

            BalanceAVLTree(ref root);
            return true;
        }

        public static bool RemoveAVLTree<T>(int key, ref AVLTree<T> root) where T : ICompareSearch
        {
            if (root == null)
            {
                return false;
            }

            //找到需要移除的节点
            if (root.value.GetCompareValue() == key)
            {
                //左右巡逻都有孩子
                if (root.lChild != null && root.rChild != null)
                {
                    //左右子树都有值的时候,用了左侧 偏小的节点
                    AVLTree<T> coverNode = root.lChild;
                    if (coverNode.rChild == null)
                    {
                        //左子树没有右子树节点，则直接把左子树替换到
                        coverNode.rChild = root.rChild;
                        root = coverNode;  //替换了root的位置，
                    }
                    else
                    {
                        //当右侧节点不是空，找到左子树上最大的节点，（一直往右找），找到没右儿子的节点。
                        AVLTree<T> coverParant = root;
                        while (coverNode.rChild != null)
                        {
                            coverParant = coverNode;
                            coverNode = coverNode.rChild;
                        }
                        //处理父亲节点的右孩子 检查最右边的子节点的左孩子，如果有左孩子，就替换自己的位置，（父节点右侧）
                        if (coverNode.lChild != null)
                        {
                            coverParant.rChild = coverNode.lChild;
                        }
                        else
                        {
                            coverParant.rChild = null;
                        }
                        
                        //替换要删除的节点
                        coverNode.lChild = root.lChild;
                        coverNode.rChild = root.rChild;
                        root = coverNode;
                        //调整好对左子树左平衡
                        BalanceAVLTree(ref root.lChild);
                    }
                }
                else if (root.lChild != null)  //只有一个孩子（左）
                {
                    root = root.lChild;
                }
                else if (root.rChild  != null) //只有一个孩子（右）
                {
                    root = root.rChild;
                }
                else
                {
                    root = null;
                }
            }
            else if (key < root.value.GetCompareValue())
            {
                RemoveAVLTree<T>(key, ref root.lChild);
            }
            else
            {
                RemoveAVLTree<T>(key, ref root.rChild);
            }
            
            //对二叉树做平衡处理
            if (root != null)
            {
                BalanceAVLTree(ref root);
            }

            return true;
        }
    }
}