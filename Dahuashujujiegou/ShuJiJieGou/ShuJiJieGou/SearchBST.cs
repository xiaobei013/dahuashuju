using System;

namespace ConsoleApp1
{
    //二叉树结构
    public class BiTree
    {
        public int data;

        public BiTree lchid, rchid;
    }
    
    //查找二叉树
    public static class BST
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="T"></param>
        /// <param name="key"></param>
        /// <param name="f">指向T的双亲，起初是调用值为null</param>
        /// <param name="p">若查找成功，则p为指向该数据，并返回成功。 若查找失败，p指向查找路径上访问的最后一个节点，并返回false</param>
        /// <returns></returns>
        public static bool SearchBST(BiTree T, int key, BiTree f, out BiTree p)
        {
            p = null;
            if (T == null)
            {
                p = f;
                return false;
            }
            else if (key == T.data) // 查找成功
            {
                p = T;
                return true;
            }
            else if (key < T.data) //小于当前节点
            {
                return SearchBST(T.lchid, key, T, out p);
            }
            else
            {
                return SearchBST(T.rchid, key, T, out p);
            }
        }

        
        public static bool InsertBST(ref BiTree T, int key)
        {
            BiTree p, s;
            //查找不成功
            if (!SearchBST(T, key, null, out p))
            {
                s = new BiTree {data = key};
                if (p == null)
                {
                    T = s;
                }
                else if (key < p.data)
                {
                    p.lchid = s;
                }
                else
                {
                    p.rchid = s;
                }

                return true;
            }
            else
            {
                return false;  //相同的关键字节点，不再插入
            }
        }

        /// <summary>
        /// 1. 叶子节点
        /// 2. 仅有左或右子树的节点
        /// 3. 左右子树都有的节点。
        /// </summary>
        public static bool DeleteBST(ref BiTree p, BiTree pre, int key)
        {
            BiTree previous = pre;
            if (p == null)
            {
                return false;
            }
            else
            {
                if (p.data == key)
                {
                    return Delete(ref p, previous, key);
                }
                else if (key < p.data)
                {
                    return DeleteBST(ref p.lchid, p, key);
                }
                else
                {
                    return DeleteBST(ref p.rchid, p, key);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="previous">因为没有指针所以要把前辈传进来</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(ref BiTree p, BiTree previous, int key)
        {
            //右子树是空则只需要重接它的左子树
            if (p.rchid == null)
            {
                if (previous == null) //删除的是根节点
                {
                    p = p.lchid;
                }
                else
                {
                    if (previous.lchid == p) // 没有指针就只能通过判断赋值
                    {
                        previous.lchid = p.lchid;
                    }
                    else
                    {
                        previous.rchid = p.lchid;
                    }
                }
            }
            else if (p.lchid == null)  //左子树是空则只需要重接它的右子树
            {
                if (previous == null) //删除的是根节点
                {
                    p = p.rchid;
                }
                else
                {
                    if (previous.lchid == p)
                    {
                        previous.lchid = p.rchid;
                    }
                    else
                    {
                        previous.rchid = p.rchid;
                    }
                }
            }
            else //左右树均不是空
            {
                if (previous == null)
                {
                    p = null;
                }
                else //删除的不是根节点
                {
                    //找到左子树的最大 or 右子树的最小，替换当前的位置
                    BiTree q = null;
                    //1.转左
                    var s = p.lchid;
                    //2.然后向右到尽头（找待删除节点的前驱）
                    while (s.rchid != null)
                    {
                        q = s; //保存上一个节点
                        s = s.rchid;
                    }

                    //3.把数据赋值到要删除的节点
                    p.data = s.data; 
                    
                    //4.删除s节点
                    if (q != null)
                    {
                        if (q.lchid == s)
                        {
                            q.lchid = null;
                        }
                        else
                        {
                            q.rchid = null;
                        }
                    }
                }
            }
            
            return true;
        }
        
        // 中序遍历
        public static void InorderTraversal(BiTree t)
        {
            if(t==null)return;
            InorderTraversal(t.lchid);
            Console.WriteLine(t.data);
            InorderTraversal(t.rchid);
        }

        //todo
        //种方法是Morris发明的，看完之后感觉精妙无比。这种方法不使用递归，不使用栈，O(1)的空间复杂度完成二叉树的遍历。
        //这种方法的基本思路就是将所有右儿子为NULL的节点的右儿子指向后继节点（对于右儿子不为空的节点，右儿子就是接下来要访问的节点）。
        //这样，对于任意一个节点，当访问完它后，它的右儿子已经指向了下一个该访问的节点。对于最右节点，不需要进行这样的操作。
        //注意，这样的操作是在遍历的时候完成的，完成访问节点后会把树还原。整个循环的判断条件为当前节点是否为空。
        public static void Morris()
        {
            
        }
    }
}