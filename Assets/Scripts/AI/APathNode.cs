using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MXAI
{

    public class APathConst
    {
        public const float COST_MIN = 0.0f;
        public const float COST_MID = 9999.0f;
        public const float COST_MAX = 99999.0f;
    }

    public interface IAPathNode
    {
        //
        int X { get; }
        int Y { get; }
        int Z { get; }
        int A { get; }
        bool walkable { get; }
        // 只有在寻路中检测kind时，才会调用
        bool iskind(IAPathNode node);
    }

    public class APathNode<T>
    {
        public int index;
        //
        public int X;
        public int Y;
        public int Z;
        public int A;

        //
        public float fCost { get { return gCost + hCost; } }
        // 从起点到该节点的移动成本
        public float gCost = APathConst.COST_MAX;
        // 从该节点到终点的移动成本为无限远，默认9999
        public float hCost = APathConst.COST_MAX;

        public T current = default(T);
        public APathNode<T> last = default(APathNode<T>);
        public APathNode<T> next = default(APathNode<T>);

        public static Vector3 operator - (APathNode<T> A, APathNode<T> B)
        {
            var v1 = new Vector3(A.X, A.Y, A.Z);
            var v2 = new Vector3(B.X, B.Y, B.Z);
            return v1 - v2;
        }

        /// <summary>
        /// Calculate HCost
        /// Not use
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float CalculateHCost(APathNode<T> target)
        {
            int xDistance = Mathf.Abs(X - target.X);
            int yDistance = Mathf.Abs(Y - target.Y);
            int zDistance = Mathf.Abs(Z - target.Z);
            return this.hCost = xDistance + yDistance + zDistance;
        }
    }

}
