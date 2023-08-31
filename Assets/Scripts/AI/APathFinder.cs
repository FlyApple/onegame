using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MXAI
{

    //
    public interface IAPathGridManager
    {
        int CX { get; }
        int CY { get; }
        int CZ { get; }
        List<APathNode<T>> NewPathNodeList<T>() where T : IAPathNode;
        List<APathNode<T>> GetPathNodes<T>() where T : IAPathNode;
    }

    //
    public class APathFinder<T> where T : IAPathNode
    {
        protected IAPathGridManager _manager = null;
        protected List<APathNode<T>> _nodeSet = null;
        //
        protected List<APathNode<T>> _nodeOpenedSet = null;
        protected List<APathNode<T>> _nodeClosedSet = null;
        private List<APathNode<T>> _nodeResultSet = null;
        public List<APathNode<T>> resultSet
        {
            get { return this._nodeResultSet; }
        }

        private int _finding_count = 0;
        private float _timeStart = 0.0f;
        private float _timeEnd = 0.0f;
        public float time_since { get { return this._timeEnd - this._timeStart; } }
        public int finding_count { get { return this._finding_count; } }

        //
        public APathFinder(IAPathGridManager manager)
        {
            this._manager = manager;
        }


        protected void InitPathNodes()
        {
            this._nodeSet = this._manager.GetPathNodes<T>();
            //
            this._nodeOpenedSet = this._manager.NewPathNodeList<T>();
            this._nodeClosedSet = this._manager.NewPathNodeList<T>();
        }

        public APathNode<T> GetNode(int x, int y)
        {
            if (this._nodeSet == null)
            {
                return null;
            }

            APathNode<T> node = this._nodeSet.Find((v) =>
            {
                return v.X == x && v.Y == y && v.Z == 0;
            });
            return node;
        }

        public APathNode<T> GetNode(int x, int y, int z)
        {
            if (this._nodeSet == null)
            {
                return null;
            }

            APathNode<T> node = this._nodeSet.Find((v) =>
            {
                return v.X == x && v.Y == y && v.Z == z;
            });
            return node;
        }

        public APathNode<T> GetNode(AGameNode node)
        {
            if (this._nodeSet == null)
            {
                return null;
            }

            APathNode<T> temp = this._nodeSet.Find((v) =>
            {
                return v.index == node.index && v.X == node.X && v.Y == node.Y && v.Z == node.Z;
            });
            return temp;
        }


        public int PathFinding(AGameNode start, AGameNode target, int maxcount = -1, int maxtime = 500)
        {
            this.InitPathNodes();

            this._finding_count = 0;

            this._timeStart = Time.realtimeSinceStartup;
            this._timeEnd = Time.realtimeSinceStartup;

            //
            List<APathNode<T>> openedSet = this._nodeOpenedSet; // 待检查的节点列表
            List<APathNode<T>> closedSet = this._nodeClosedSet; // 已检查的节点列表

            APathNode<T> node_start = this.GetNode(start);
            APathNode<T> node_target = this.GetNode(target);
            if (node_start == null || node_target == null)
            {
                this._timeEnd = Time.realtimeSinceStartup;
                return -1;
            }

            List<APathNode<T>> history_closedSet = this._manager.NewPathNodeList<T>();

            // 起点自身移动成本为0
            node_start.gCost = 0.0f;
            openedSet.Add(node_start);

            //
            int finding_count = 0;
            while ((this._nodeResultSet = this.DoPathFinding(openedSet, closedSet, node_start, node_target)) != null)
            {
                finding_count++;
                if (this._nodeResultSet.Count == 0)
                {
                    break;
                }

                break;
            }

            int count = 0;
            if (this._nodeResultSet != null && this._nodeResultSet.Count > 0)
            {
                count = this._nodeResultSet.Count;
            }

            //Debug.Log(string.Format("{0:F3}", this._timeEnd - this._timeStart));
            this._finding_count = finding_count;
            this._timeEnd = Time.realtimeSinceStartup;
            return count;
        }

        private List<APathNode<T>> DoPathFinding(List<APathNode<T>> openSet, List<APathNode<T>> closedSet,
            APathNode<T> start, APathNode<T> target)
        {
            List<APathNode<T>> resultSet = default(List<APathNode<T>>);

            while (openSet.Count > 0)
            {
                APathNode<T> current = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < current.fCost ||
                        (openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost))
                    {
                        current = openSet[i];
                    }
                }

                openSet.Remove(current);
                closedSet.Add(current);


                if (current == target)
                {
                    resultSet = RetracePath(start, target);
                    break;
                }

                List<APathNode<T>> neighbors = GetNeighbors3(current);
                foreach (APathNode<T> neighbor in neighbors)
                {
                    if (neighbor.current == null)
                    {
                        continue;
                    }
                    if (!neighbor.current.walkable)
                    {
                        if (neighbor != target || !neighbor.current.iskind(start.current))
                        {
                            continue;
                        }
                    }
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }


                    float newCostToNeighbor = current.gCost + GetDistance3(current, neighbor);
                    if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCostToNeighbor;
                        neighbor.hCost += GetDistance3(neighbor, target);
                        neighbor.last = current;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return resultSet;
        }

        // 追溯路径
        private List<APathNode<T>> RetracePath(APathNode<T> start, APathNode<T> end)
        {
            List<APathNode<T>> path = this._manager.NewPathNodeList<T>();
            APathNode<T> current = end;
            while (current != start)
            {
                path.Add(current);
                current = current.last;
            }

            // 
            if (current == null)
            {
                return null;
            }

            // 颠倒列表顺序
            path.Add(start);
            path.Reverse();
            return path;
        }

        // 获取相邻节点列表
        private List<APathNode<T>> GetNeighbors2(APathNode<T> node, bool four = false)
        {
            List<APathNode<T>> neighbors = this._manager.NewPathNodeList<T>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // 原点不计入
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    // 直角，故不使用四个角
                    // 0 1 0
                    // 1 X 1
                    // 0 1 0
                    if (four && 
                        ((x == -1 && y == 1) || (x == 1 && y == 1) ||
                        (x == -1 && y == -1) || (x == 1 && y == -1)))
                    {
                        continue;
                    }

                    //
                    Vector2Int pos = new Vector2Int(node.X + x, node.Y + y);
                    if (pos.x >= 0 && pos.x < this._manager.CX &&
                        pos.y >= 0 && pos.y < this._manager.CY)
                    {
                        APathNode<T> temp = this.GetNode(pos.x, pos.y);
                        if (temp == null) //该位置不存在
                        {
                            //Debug.LogWarning("[APathFinder] (Neighbors) Get (" + pos.x + ", "+ pos.y+ ") null");
                            continue;
                        }
                        neighbors.Add(temp);
                    }
                }
            }

            return neighbors;
        }

        private List<APathNode<T>> GetNeighbors3(APathNode<T> node, bool four = false)
        {
            List<APathNode<T>> neighbors = this._manager.NewPathNodeList<T>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        // 原点不计入
                        if (x == 0 && y == 0 && z == 0)
                        {
                            continue;
                        }

                        // 直角，故不使用四个角
                        // 0 1 0
                        // 1 X 1
                        // 0 1 0
                        if (four &&
                            ((x == -1 && y == 1) || (x == 1 && y == 1) ||
                            (x == -1 && y == -1) || (x == 1 && y == -1)))
                        {
                            continue;
                        }

                        //
                        Vector3Int pos = new Vector3Int(node.X + x, node.Y + y, node.Z + z);
                        if (pos.x >= 0 && pos.x < this._manager.CX &&
                            pos.y >= 0 && pos.y < this._manager.CY &&
                            pos.z >= 0 && pos.z < this._manager.CZ)
                        {
                            APathNode<T> temp = this.GetNode(pos.x, pos.y, pos.z);
                            if (temp == null) //该位置不存在
                            {
                                //Debug.LogWarning("[APathFinder] (Neighbors) Get (" + pos.x + ", "+ pos.y+ ") null");
                                continue;
                            }
                            neighbors.Add(temp);
                        }
                    }
                }
            }
            return neighbors;
        }


        // 获取两个节点之间的距离
        private float GetDistance2(APathNode<T> nodeA, APathNode<T> nodeB)
        {
            int distX = Mathf.Abs(nodeA.X - nodeB.X);
            int distY = Mathf.Abs(nodeA.Y - nodeB.Y);

            return distX + distY;
        }
        private float GetDistance3(APathNode<T> nodeA, APathNode<T> nodeB)
        {
            int distX = Mathf.Abs(nodeA.X - nodeB.X);
            int distY = Mathf.Abs(nodeA.Y - nodeB.Y);
            int distZ = Mathf.Abs(nodeA.Z - nodeB.Z);
            return distX + distY + distZ;
        }
    }


}
