using C5;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpPathFinding.cs.PathFinder
{
    public class AStarParam : ParamBase
    {
        public delegate float HeuristicDelegate(int iDx, int iDy);

        public DiagonalMovement DiagonalMovement;
        public float Weight;

        public AStarParam(BaseGrid iGrid, float iweight, HeuristicMode iMode = HeuristicMode.EUCLIDEAN, DiagonalMovement iDiagonalMovement = DiagonalMovement.Always) : base(iGrid, iMode)
        {
            Weight = iweight;
            DiagonalMovement = iDiagonalMovement;
        }

        internal override void _reset(GridPos iStartPos, GridPos iEndPos, BaseGrid iSearchGrid = null)
        {

        }
    }
    public static class AStarFinder
    {
        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                var result = (x.heuristicStartToEndLen - y.heuristicStartToEndLen);
                if (result < 0) return -1;
                else
                if (result > 0) return 1;
                else
                {
                    return 0;
                }
            }
        }
        public static List<GridPos> FindPath(AStarParam iParam)
        {
            object lo = new object();
            var openList = new IntervalHeap<Node>(new NodeComparer());
            var startNode = iParam.StartNode;
            var endNode = iParam.EndNode;
            var heuristic = iParam.HeuristicFunc;
            var grid = iParam.SearchGrid;
            var diagonalMovement = iParam.DiagonalMovement;
            var weight = iParam.Weight;


            startNode.startToCurNodeLen = 0;
            startNode.heuristicStartToEndLen = 0;

            openList.Add(startNode);
            startNode.isOpened = true;

            while (openList.Count != 0)
            {
                var node = openList.DeleteMin();
                node.isClosed = true;

                if (node == endNode)
                {
                    return Node.Backtrace(endNode);
                }

                var neighbors = grid.GetNeighbors(node, diagonalMovement);



                Parallel.ForEach(neighbors, neighbor =>
                {
                    if (neighbor.isClosed) return;
                    var x = neighbor.x;
                    var y = neighbor.y;
                    float ng = node.startToCurNodeLen + (float)((x - node.x == 0 || y - node.y == 0) ? 1 : Math.Sqrt(2));

                    if (!neighbor.isOpened || ng < neighbor.startToCurNodeLen)
                    {
                        neighbor.startToCurNodeLen = ng;
                        if (neighbor.heuristicCurNodeToEndLen == 0) neighbor.heuristicCurNodeToEndLen = weight * heuristic(Math.Abs(x - endNode.x), Math.Abs(y - endNode.y));
                        neighbor.heuristicStartToEndLen = neighbor.startToCurNodeLen + neighbor.heuristicCurNodeToEndLen.Value;
                        neighbor.parent = node;
                        if (!neighbor.isOpened)
                        {
                            lock (lo)
                            {
                                openList.Add(neighbor);
                            }
                            neighbor.isOpened = true;
                        }
                        else
                        {

                        }
                    }
                });
                if (openList.Count == 0) return Node.Backtrace(node);
            }
            return new List<GridPos>();

        }
    }
}
