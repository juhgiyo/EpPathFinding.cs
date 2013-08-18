/*! 
@file JumpPointFinder.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding.cs>
@date July 16, 2013
@brief Jump Point Search Algorithm Interface
@version 2.0

@section LICENSE

Copyright (C) 2013  Woong Gyu La <juhgiyo@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

@section DESCRIPTION

An Interface for the Jump Point Search Algorithm Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EpPathFinding
{

    public delegate float HeuristicDelegate(int iDx, int iDy);

    class JumpPointParam
    {

        public JumpPointParam(BaseGrid iGrid, GridPos iStartPos, GridPos iEndPos, bool iCrossCorner = true, HeuristicMode iMode = HeuristicMode.EUCLIDEAN)
        {
            switch (iMode)
            {
                case HeuristicMode.MANHATTAN:
                    heuristic = new HeuristicDelegate(Heuristic.Manhattan);
                    break;
                case HeuristicMode.EUCLIDEAN:
                    heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
                case HeuristicMode.CHEBYSHEV:
                    heuristic = new HeuristicDelegate(Heuristic.Chebyshev);
                    break;
                default:
                    heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
            }
            crossCorner = iCrossCorner;

            openList = new List<Node>();

            searchGrid = iGrid;
            startNode = searchGrid.GetNodeAt(iStartPos.x, iStartPos.y);
            endNode = searchGrid.GetNodeAt(iEndPos.x, iEndPos.y);
        }

        public JumpPointParam(BaseGrid iGrid, bool iCrossCorner = true, HeuristicMode iMode = HeuristicMode.EUCLIDEAN)
        {
            switch (iMode)
            {
                case HeuristicMode.MANHATTAN:
                    heuristic = new HeuristicDelegate(Heuristic.Manhattan);
                    break;
                case HeuristicMode.EUCLIDEAN:
                    heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
                case HeuristicMode.CHEBYSHEV:
                    heuristic = new HeuristicDelegate(Heuristic.Chebyshev);
                    break;
                default:
                    heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
            }
            crossCorner = iCrossCorner;

            openList = new List<Node>();

            searchGrid = iGrid;
            startNode = null;
            endNode = null;
        }

        public void SetHeuristic(HeuristicMode iMode)
        {
            heuristic = null;
            switch (iMode)
            {
                case HeuristicMode.MANHATTAN:
                    heuristic = new HeuristicDelegate(Heuristic.Manhattan);
                    break;
                case HeuristicMode.EUCLIDEAN:
                    heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
                case HeuristicMode.CHEBYSHEV:
                    heuristic = new HeuristicDelegate(Heuristic.Chebyshev);
                    break;
                default:
                    heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
            }
        }

        public void Reset(GridPos iStartPos, GridPos iEndPos)
        {
            openList.Clear();
            startNode = null;
            endNode = null;

            searchGrid.Reset();
            startNode = searchGrid.GetNodeAt(iStartPos.x, iStartPos.y);
            endNode = searchGrid.GetNodeAt(iEndPos.x, iEndPos.y);


        }

        public bool CrossCorner
        {
            get
            {
                return crossCorner;
            }
            set
            {
                crossCorner = value;
            }
        }

        public HeuristicDelegate HeuristicFunc
        {
            get
            {
                return heuristic;
            }
        }

        public BaseGrid SearchGrid
        {
            get
            {
                return searchGrid;
            }
        }

        public Node StartNode
        {
            get
            {
                return startNode;
            }
        }
        public Node EndNode
        {
            get
            {
                return endNode;
            }
        }
        protected HeuristicDelegate heuristic;
        protected bool crossCorner;

        protected BaseGrid searchGrid;
        protected Node startNode;
        protected Node endNode;

        public List<Node> openList;
    }
    class JumpPointFinder
    {
        public static List<GridPos> FindPath(JumpPointParam iParam)
        {

            List<Node> tOpenList = iParam.openList;
            Node tStartNode = iParam.StartNode;
            Node tEndNode = iParam.EndNode;
            Node tNode;


            // set the `g` and `f` value of the start node to be 0
            tStartNode.startToCurNodeLen = 0;
            tStartNode.heuristicStartToEndLen = 0;

            // push the start node into the open list
            tOpenList.Add(tStartNode);
            tStartNode.isOpened = true;

            // while the open list is not empty
            while (tOpenList.Count > 0)
            {
                // pop the position of node which has the minimum `f` value.
                tOpenList.Sort();
                tNode = (Node)tOpenList[0];
                tOpenList.RemoveAt(0);
                tNode.isClosed = true;

                if (tNode.Equals(tEndNode))
                {
                    return Node.Backtrace(tEndNode); // rebuilding path
                }

                IdentifySuccessors(iParam, tNode);
            }

            // fail to find the path
            return new List<GridPos>();
        }

        private static void IdentifySuccessors(JumpPointParam iParam, Node iNode)
        {
            HeuristicDelegate tHeuristic = iParam.HeuristicFunc;
            List<Node> tOpenList = iParam.openList;
            int tEndX = iParam.EndNode.x;
            int tEndY = iParam.EndNode.y;
            GridPos tNeighbor, tJumpPoint;
            Node tJumpNode;

            List<GridPos> tNeighbors = FindNeighbors(iParam, iNode);
            for (int i = 0; i < tNeighbors.Count; i++)
            {
                tNeighbor = tNeighbors[i];
                tJumpPoint = Jump(iParam, tNeighbor.x, tNeighbor.y, iNode.x, iNode.y);
                if (tJumpPoint != null)
                {
                    tJumpNode = iParam.SearchGrid.GetNodeAt(tJumpPoint.x, tJumpPoint.y);

                    if (tJumpNode.isClosed)
                    {
                        continue;
                    }
                    // include distance, as parent may not be immediately adjacent:
                    float tCurNodeToJumpNodeLen = tHeuristic(Math.Abs(tJumpPoint.x - iNode.x), Math.Abs(tJumpPoint.y - iNode.y));
                    float tStartToJumpNodeLen = iNode.startToCurNodeLen + tCurNodeToJumpNodeLen; // next `startToCurNodeLen` value

                    if (!tJumpNode.isOpened || tStartToJumpNodeLen < tJumpNode.startToCurNodeLen)
                    {
                        tJumpNode.startToCurNodeLen = tStartToJumpNodeLen;
                        tJumpNode.heuristicCurNodeToEndLen = (tJumpNode.heuristicCurNodeToEndLen == null ? tHeuristic(Math.Abs(tJumpPoint.x - tEndX), Math.Abs(tJumpPoint.y - tEndY)) : tJumpNode.heuristicCurNodeToEndLen);
                        tJumpNode.heuristicStartToEndLen = tJumpNode.startToCurNodeLen + tJumpNode.heuristicCurNodeToEndLen.Value;
                        tJumpNode.parent = iNode;

                        if (!tJumpNode.isOpened)
                        {
                            tOpenList.Add(tJumpNode);
                            tJumpNode.isOpened = true;
                        }
                    }
                }
            }
        }

        private static GridPos Jump(JumpPointParam iParam, int iX, int iY, int iPx, int iPy)
        {
            if (!iParam.SearchGrid.IsWalkableAt(iX, iY))
            {
                return null;
            }
            else if (iParam.SearchGrid.GetNodeAt(iX, iY).Equals(iParam.EndNode))
            {
                return new GridPos(iX, iY);
            }
            int tDx = iX - iPx;
            int tDy = iY - iPy;

            // check for forced neighbors
            // along the diagonal
            if (tDx != 0 && tDy != 0)
            {
                if ((iParam.SearchGrid.IsWalkableAt(iX - tDx, iY + tDy) && !iParam.SearchGrid.IsWalkableAt(iX - tDx, iY)) ||
                    (iParam.SearchGrid.IsWalkableAt(iX + tDx, iY - tDy) && !iParam.SearchGrid.IsWalkableAt(iX, iY - tDy)))
                {
                    return new GridPos(iX, iY);
                }
            }
            // horizontally/vertically
            else
            {
                if (tDx != 0)
                { // moving along x
                    if ((iParam.SearchGrid.IsWalkableAt(iX + tDx, iY + 1) && !iParam.SearchGrid.IsWalkableAt(iX, iY + 1)) ||
                        (iParam.SearchGrid.IsWalkableAt(iX + tDx, iY - 1) && !iParam.SearchGrid.IsWalkableAt(iX, iY - 1)))
                    {
                        return new GridPos(iX, iY);
                    }
                }
                else
                {
                    if ((iParam.SearchGrid.IsWalkableAt(iX + 1, iY + tDy) && !iParam.SearchGrid.IsWalkableAt(iX + 1, iY)) ||
                        (iParam.SearchGrid.IsWalkableAt(iX - 1, iY + tDy) && !iParam.SearchGrid.IsWalkableAt(iX - 1, iY)))
                    {
                        return new GridPos(iX, iY);
                    }
                }
            }

            GridPos jx;
            GridPos jy;
            // when moving diagonally, must check for vertical/horizontal jump points
            if (tDx != 0 && tDy != 0)
            {
                jx = Jump(iParam, iX + tDx, iY, iX, iY);
                jy = Jump(iParam, iX, iY + tDy, iX, iY);
                if (jx != null || jy != null)
                {
                    return new GridPos(iX, iY);
                }
            }

            // moving diagonally, must make sure one of the vertical/horizontal
            // neighbors is open to allow the path
            if (iParam.SearchGrid.IsWalkableAt(iX + tDx, iY) || iParam.SearchGrid.IsWalkableAt(iX, iY + tDy))
            {
                return Jump(iParam, iX + tDx, iY + tDy, iX, iY);
            }
            else if (iParam.CrossCorner)
            {
                return Jump(iParam, iX + tDx, iY + tDy, iX, iY);
            }
            else
            {
                return null;
            }
        }

        private static List<GridPos> FindNeighbors(JumpPointParam iParam, Node iNode)
        {
            Node tParent = (Node)iNode.parent;
            int tX = iNode.x;
            int tY = iNode.y;
            int tPx, tPy, tDx, tDy;
            List<GridPos> tNeighbors = new List<GridPos>();
            List<Node> tNeighborNodes;
            Node tNeighborNode;

            // directed pruning: can ignore most neighbors, unless forced.
            if (tParent != null)
            {
                tPx = tParent.x;
                tPy = tParent.y;
                // get the normalized direction of travel
                tDx = (tX - tPx) / Math.Max(Math.Abs(tX - tPx), 1);
                tDy = (tY - tPy) / Math.Max(Math.Abs(tY - tPy), 1);

                // search diagonally
                if (tDx != 0 && tDy != 0)
                {
                    if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy))
                    {
                        tNeighbors.Add(new GridPos(tX, tY + tDy));
                    }
                    if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                    {
                        tNeighbors.Add(new GridPos(tX + tDx, tY));
                    }

                    if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY + tDy))
                    {
                        if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy) || iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                        {
                            tNeighbors.Add(new GridPos(tX + tDx, tY + tDy));
                        }
                        else if (iParam.CrossCorner)
                        {
                            tNeighbors.Add(new GridPos(tX + tDx, tY + tDy));
                        }
                    }

                    if (!iParam.SearchGrid.IsWalkableAt(tX - tDx, tY) && iParam.SearchGrid.IsWalkableAt(tX - tDx, tY + tDy))
                    {
                        if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy))
                        {
                            tNeighbors.Add(new GridPos(tX - tDx, tY + tDy));
                        }
                        else if (iParam.CrossCorner)
                        {
                            tNeighbors.Add(new GridPos(tX - tDx, tY + tDy));
                        }
                    }

                    if (!iParam.SearchGrid.IsWalkableAt(tX, tY - tDy) && iParam.SearchGrid.IsWalkableAt(tX + tDx, tY - tDy))
                    {
                        if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                        {
                            tNeighbors.Add(new GridPos(tX + tDx, tY - tDy));
                        }
                        else if (iParam.CrossCorner)
                        {
                            tNeighbors.Add(new GridPos(tX + tDx, tY - tDy));
                        }
                    }


                }
                // search horizontally/vertically
                else
                {
                    if (tDx == 0)
                    {
                        if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy))
                        {
                            tNeighbors.Add(new GridPos(tX, tY + tDy));

                            if (!iParam.SearchGrid.IsWalkableAt(tX + 1, tY) && iParam.SearchGrid.IsWalkableAt(tX + 1, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX + 1, tY + tDy));
                            }
                            if (!iParam.SearchGrid.IsWalkableAt(tX - 1, tY) && iParam.SearchGrid.IsWalkableAt(tX - 1, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX - 1, tY + tDy));
                            }
                        }
                        else if (iParam.CrossCorner)
                        {
                            if (!iParam.SearchGrid.IsWalkableAt(tX + 1, tY) && iParam.SearchGrid.IsWalkableAt(tX + 1, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX + 1, tY + tDy));
                            }
                            if (!iParam.SearchGrid.IsWalkableAt(tX - 1, tY) && iParam.SearchGrid.IsWalkableAt(tX - 1, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX - 1, tY + tDy));
                            }
                        }
                    }
                    else
                    {
                        if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                        {

                            tNeighbors.Add(new GridPos(tX + tDx, tY));

                            if (!iParam.SearchGrid.IsWalkableAt(tX, tY + 1) && iParam.SearchGrid.IsWalkableAt(tX + tDx, tY + 1))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY + 1));
                            }
                            if (!iParam.SearchGrid.IsWalkableAt(tX, tY - 1) && iParam.SearchGrid.IsWalkableAt(tX + tDx, tY - 1))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY - 1));
                            }
                        }
                        else if (iParam.CrossCorner)
                        {
                            if (!iParam.SearchGrid.IsWalkableAt(tX, tY + 1) && iParam.SearchGrid.IsWalkableAt(tX + tDx, tY + 1))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY + 1));
                            }
                            if (!iParam.SearchGrid.IsWalkableAt(tX, tY - 1) && iParam.SearchGrid.IsWalkableAt(tX + tDx, tY - 1))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY - 1));
                            }
                        }
                    }
                }
            }
            // return all neighbors
            else
            {
                tNeighborNodes = iParam.SearchGrid.GetNeighbors(iNode, iParam.CrossCorner);
                for (int i = 0; i < tNeighborNodes.Count; i++)
                {
                    tNeighborNode = tNeighborNodes[i];
                    tNeighbors.Add(new GridPos(tNeighborNode.x, tNeighborNode.y));
                }
            }

            return tNeighbors;
        }
    }
}
