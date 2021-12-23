namespace Tree.BinaryTree
{
    public class TreeNode<T>
    {
        public T data;               //数据域
        public TreeNode<T> lChild;   //左孩子   树中一个结点的子树的根结点称为这个结点的孩子
        public TreeNode<T> rChild;   //右孩子
    }
}