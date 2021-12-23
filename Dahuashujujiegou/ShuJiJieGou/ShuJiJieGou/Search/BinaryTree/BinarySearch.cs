namespace ConsoleApp1
{
    //参考https://www.cnblogs.com/kyoner/p/11080078.html

    public class BinarySearchTest
    {
        //基本的二分搜索 主要针对有序数组
        public int BinarySearch(int[] nums, int target)
        {
            int left = 0;
            int right = nums.Length - 1;

            //为什么 while 循环的条件中是 <=，而不是 < ？
            //因为初始化 right 的赋值是 nums.length - 1，即最后一个元素的索引，而不是 nums.length。
            //这二者可能出现在不同功能的二分查找中，区别是：前者相当于两端都闭区间 [left, right]，后者相当于左闭右开区间 [left, right)，因为索引大小为 nums.length 是越界的。
            //我们这个算法中使用的是 [left, right] 两端都闭的区间。这个区间就是每次进行搜索的区间，我们不妨称为「搜索区间」(search space)。
            while (left <= right)
            {
                int mid = (right + left) / 2;
                if (nums[mid] == target)
                {
                    // 找到目标后停止
                    return mid;
                }
                else if (nums[mid] < target)
                {
                    left = mid + 1;
                }
                else if (nums[mid] > target)
                {
                    right = mid - 1;
                }
            }

            //如果没找到，就需要 while 循环终止，然后返回 -1。那 while 循环什么时候应该终止？搜索区间为空的时候应该终止
            return -1;
        }

        //上面算法的缺陷在后续的 搜索中就改为了线性搜索，因为这样能以保证二分查找对数级的时间复杂度
        //查左边界
        int left_bound(int[] nums, int target)
        {
            if (nums.Length == 0) return -1;
            int left = 0; // 含义！！
            int right = nums.Length; // 注意

            //因为初始化 right = nums.length 而不是 nums.length - 1 。因此每次循环的「搜索区间」是 [left, right) 左闭右开
            //while(left < right) 终止的条件是 left == right，此时搜索区间 [left, left) 恰巧为空，所以可以正确终止
            while (left < right) //注意
            {
                int mid = (left + right) / 2;

                //为什么算法不一样？
                //因为我们的「搜索区间」是 [left, right) 左闭右开，所以当 nums[mid] 被检测之后，下一步的搜索区间应该去掉 mid 分割成两个区间，即 [left, mid) 或 [mid + 1, right)
                if (nums[mid] == target)
                {
                    right = mid;
                }
                else if (nums[mid] < target)
                {
                    left = mid + 1;
                }
                else if (nums[mid] > target)
                {
                    right = mid; // 注意
                }
            }
            //为什么没有返回 -1 的操作？如果 nums 中不存在 target 这个值，怎么办?
            //先理解一下这个「左侧边界」有什么特殊含义：
            //对于数组{1,2,2,4} target = 1，算法会返回 0,含义是：nums 中小于 1 的元素有 0 个。如果 target = 8，算法会返回 4，含义是：nums 中小于 8 的元素有 4 个

            return left;
        }

        //查右边界-- 大于target的第一个值的下标
        int right_bound(int[] nums, int target)
        {
            int right = nums.Length;
            int left = 0;
            while (left < right) // 这两个值相等的时候就要跳出循环 nums[mid] > target
            {
                int mid = (left + right) / 2;

                if (nums[mid] == target)
                {
                    left = mid + 1; //*为什么这个算法能够找到右侧边界?
                    //当 nums[mid] == target 时，不要立即返回，而是增大「搜索区间」的下界 left，使得区间不断向右收缩，达到锁定右侧边界的目的
                }
                else if (nums[mid] < target)
                {
                    left = mid + 1;
                }
                else if (nums[mid] > target)
                {
                    right = mid; // 到这里就跳出循环了
                }
            }

            return left - 1; //*为什么最后返回 left - 1 而不像左侧边界的函数,返回 left？而且我觉得这里既然是搜索右侧边界，应该返回 right 才对。
            //首先，while 循环的终止条件是 left == right，所以 left 和 right 是一样的，你非要体现右侧的特点，返回 right - 1 好了
            //因为我们对 left 的更新必须是 left = mid + 1，就是说 while 循环结束时，nums[left] 一定不等于 target 了，而 nums[left - 1]可能是target

            //*为什么没有返回 -1 的操作？如果 nums 中不存在 target 这个值，怎么办？
            //类似左侧边界搜索，因为while的终止条件是left == right， 就是说 left的取值范围是[0,nums.length]，所以可以tian
        }
        
        //二分查找的时间复杂度
        //最好的情况下只需要进行1次比较就能找到目标元素，那么最坏呢？ 借助该序列的二叉树形式进行分析：将一个序列转化位二叉数的过程是这样的，将它的中间元素作为它的根节点，
        //将中间元素之前的前半部分作为它的左子树，将中间元素之后的后半本分作为它的右子树。
        //在创建左，右子树的时候递归利用上面这一规则。
        
        //https://blog.csdn.net/weixin_44688301/article/details/116743922
        //例如：{5, 10, 22, 29, 43, 57, 58, 61, 73, 77, 81}
        //一个有序列：n 个元素，并且它的二叉树是一个满二叉树，且该二叉数的深度为d。
        //一颗深度d的满二叉树的节点数量n等于：2^0 + 2^1 +2^2 +2^3 + …… + 2^(d-1) ,
        //用等比数列的求和公式(或者归纳法)得到一个满二叉树的节点个数：2^d-1 = n,因此(取对数) d=log2^(n+1)  
        //再上式中，因为d和n均为正整数，所以d-1 <= log2(n) <= log2(n+1) ,因此|log2(n) = d-1|
        
        //https://baike.baidu.com/item/%E4%BA%8C%E5%88%86%E6%9F%A5%E6%89%BE/10628618?fr=kg_qa#4
        //时间复杂度即是while循环的次数,总共有n个元素
        //渐渐跟下去就是n,n/2,n/4,....n/2^k（接下来操作元素的剩余个数），其中k就是循环的次数
        //由于你n/2^k取整后>=1, 即令n/2^k=1 可得k=log2n,（是以2为底，n的对数）
        //所以时间复杂度可以表示O(h)=O(log2n)
    }
}