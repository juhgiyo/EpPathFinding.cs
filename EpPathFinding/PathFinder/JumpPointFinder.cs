/*! 
@file JumpPointFinder.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding>
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

    class JumpPointFinder
    {
        public delegate float HeuristicDelegate(int iDx, int iDy);
        private HeuristicDelegate heuristic;

        private Node startNode;
        private Node endNode;

        private Grid grid;
        private List<Node> openList;

        private bool dontCrossCorner;
        public JumpPointFinder(HeuristicMode iMode, bool iDontCrossCorner=false)
        {
            this.dontCrossCorner = iDontCrossCorner;

            switch (iMode)
            {
                case HeuristicMode.MANHATTAN:
                    heuristic=new HeuristicDelegate(Heuristic.Manhattan);
                    break;
                case HeuristicMode.EUCLIDEAN:
                    heuristic=new HeuristicDelegate(Heuristic.Euclidean);
                    break;
                case HeuristicMode.CHEBYSHEV:
                    heuristic=new HeuristicDelegate(Heuristic.Chebyshev);
                    break;
                default:
                    heuristic = new HeuristicDelegate(Heuristic.EuclideanSqr);
                    break;
            }
        }

        public List<GridPos> FindPath(GridPos iStartPos, GridPos iEndPos, Grid iGrid)
        {

            List<Node> tOpenList = this.openList = new List<Node>();
            Node tStartNode = this.startNode = iGrid.GetNodeAt(iStartPos.x, iStartPos.y);
            Node tEndNode = this.endNode = iGrid.GetNodeAt(iEndPos.x, iEndPos.y);
            Node tNode;
            this.grid = iGrid;


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
                    return Util.Backtrace(tEndNode); // rebuilding path
                }

                this.IdentifySuccessors(tNode);
            }

            // fail to find the path
            return new List<GridPos>();
        }

        private void IdentifySuccessors(Node iNode)
        {
            Grid tGrid = this.grid;
            HeuristicDelegate tHeuristic = this.heuristic;
            List<Node> tOpenList = this.openList;
            int tEndX = this.endNode.x;
            int tEndY = this.endNode.y;
            GridPos tNeighbor,tJumpPoint;
            Node tJumpNode;

            List<GridPos> tNeighbors = this.FindNeighbors(iNode);
            for (int i = 0; i < tNeighbors.Count; i++) 
            {
                tNeighbor = tNeighbors[i];
                tJumpPoint = this.Jump(tNeighbor.x, tNeighbor.y, iNode.x, iNode.y);
                if (tJumpPoint != null) 
                {
                    tJumpNode = grid.GetNodeAt(tJumpPoint.x, tJumpPoint.y);

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
        
        private GridPos Jump(int iX,int iY,int iPx,int iPy) 
        {
            Grid grid = this.grid;


            if (!grid.IsWalkableAt(iX, iY))
            {
                return null;
            }
            else if (grid.GetNodeAt(iX, iY).Equals(this.endNode))
            {
                return new GridPos(iX, iY);
            }
            int tDx = iX - iPx;
            int tDy = iY - iPy;

            // check for forced neighbors
            // along the diagonal
            if (tDx != 0 && tDy != 0)
            {
                if ((grid.IsWalkableAt(iX - tDx, iY + tDy) && !grid.IsWalkableAt(iX - tDx, iY)) ||
                    (grid.IsWalkableAt(iX + tDx, iY - tDy) && !grid.IsWalkableAt(iX, iY - tDy)))
                {
                    return new GridPos(iX, iY);
                }
            }
            // horizontally/vertically
            else {
                if (tDx != 0)
                { // moving along x
                    if ((grid.IsWalkableAt(iX + tDx, iY + 1) && !grid.IsWalkableAt(iX, iY + 1)) ||
                       (grid.IsWalkableAt(iX + tDx, iY - 1) && !grid.IsWalkableAt(iX, iY - 1)))
                    {
                        return new GridPos(iX, iY);
                    }
                }
                else {
                    if ((grid.IsWalkableAt(iX + 1, iY + tDy) && !grid.IsWalkableAt(iX + 1, iY)) ||
                       (grid.IsWalkableAt(iX - 1, iY + tDy) && !grid.IsWalkableAt(iX - 1, iY)))
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
                jx = this.Jump(iX + tDx, iY, iX, iY);
                jy = this.Jump(iX, iY + tDy, iX, iY);
                if (jx !=null || jy!=null) 
                {
                    return new GridPos(iX, iY);
                }
            }

            // moving diagonally, must make sure one of the vertical/horizontal
            // neighbors is open to allow the path
            if (grid.IsWalkableAt(iX + tDx, iY) || grid.IsWalkableAt(iX, iY + tDy))
            {
                return this.Jump(iX + tDx, iY + tDy, iX, iY);
            }
            else if (!dontCrossCorner)
            {
                return this.Jump(iX + tDx, iY + tDy, iX, iY);
            }
            else {
                return null;
            }
        }

        private List<GridPos> FindNeighbors(Node iNode)
        {
            Node tParent = (Node)iNode.parent;
            int tX = iNode.x;
            int tY = iNode.y;
            Grid tGrid = this.grid;
            int tPx, tPy,  tDx, tDy;
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
                if (tDx != 0 && tDy != 0) {
                    if (tGrid.IsWalkableAt(tX, tY + tDy)) 
                    {
                        tNeighbors.Add(new GridPos(tX, tY + tDy));
                    }
                    if (tGrid.IsWalkableAt(tX + tDx, tY)) 
                    {
                        tNeighbors.Add(new GridPos(tX + tDx, tY));
                    }

                    if (tGrid.IsWalkableAt(tX + tDx, tY + tDy))
                    {
                        if (tGrid.IsWalkableAt(tX, tY + tDy) || tGrid.IsWalkableAt(tX + tDx, tY)) 
                        {
                            tNeighbors.Add(new GridPos(tX + tDx, tY + tDy));
                        }
                        else if (!dontCrossCorner)
                        {
                           tNeighbors.Add(new GridPos(tX + tDx, tY + tDy));
                        }
                    }

                    if (!tGrid.IsWalkableAt(tX - tDx, tY) && tGrid.IsWalkableAt(tX - tDx, tY + tDy))
                    {
                        if (tGrid.IsWalkableAt(tX, tY + tDy))
                        {
                            tNeighbors.Add(new GridPos(tX - tDx, tY + tDy));
                        }
                        else if (!dontCrossCorner)
                        {
                            tNeighbors.Add(new GridPos(tX - tDx, tY + tDy));
                        }
                    }

                    if (!tGrid.IsWalkableAt(tX, tY - tDy) && tGrid.IsWalkableAt(tX + tDx, tY - tDy))
                    {
                        if (tGrid.IsWalkableAt(tX + tDx, tY))
                        {
                            tNeighbors.Add(new GridPos(tX + tDx, tY - tDy));
                        }
                        else if (!dontCrossCorner)
                        {
                            tNeighbors.Add(new GridPos(tX + tDx, tY - tDy));
                        }
                    }
                    
                   
                }
                // search horizontally/vertically
                else {
                    if(tDx == 0) {
                        if (tGrid.IsWalkableAt(tX, tY + tDy))
                        {
                            tNeighbors.Add(new GridPos(tX, tY + tDy));

                            if (!tGrid.IsWalkableAt(tX + 1, tY) && tGrid.IsWalkableAt(tX + 1, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX + 1, tY + tDy));
                            }
                            if (!tGrid.IsWalkableAt(tX - 1, tY) && tGrid.IsWalkableAt(tX - 1, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX - 1, tY + tDy));
                            }
                        }
                        else if (!dontCrossCorner)
                        {
                            if (!tGrid.IsWalkableAt(tX + 1, tY) && tGrid.IsWalkableAt(tX + 1, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX + 1, tY + tDy));
                            }
                            if (!tGrid.IsWalkableAt(tX - 1, tY) && tGrid.IsWalkableAt(tX - 1, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX - 1, tY + tDy));
                            }
                        }
                    }
                    else {
                        if (tGrid.IsWalkableAt(tX + tDx, tY))
                        {

                            tNeighbors.Add(new GridPos(tX + tDx, tY));

                            if (!tGrid.IsWalkableAt(tX, tY + 1) && tGrid.IsWalkableAt(tX + tDx, tY + 1))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY + 1));
                            }
                            if (!tGrid.IsWalkableAt(tX, tY - 1) && tGrid.IsWalkableAt(tX + tDx, tY - 1))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY - 1));
                            }
                        }
                        else if (!dontCrossCorner)
                        {
                            if (!tGrid.IsWalkableAt(tX, tY + 1) && tGrid.IsWalkableAt(tX + tDx, tY + 1))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY + 1));
                            }
                            if (!tGrid.IsWalkableAt(tX, tY - 1) && tGrid.IsWalkableAt(tX + tDx, tY - 1))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY - 1));
                            }
                        }
                    }
                }
            }
            // return all neighbors
            else {
                tNeighborNodes = tGrid.GetNeighbors(iNode, this.dontCrossCorner);
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
