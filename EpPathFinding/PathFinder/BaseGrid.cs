/*! 
@file BaseGrid.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding.cs>
@date July 16, 2013
@brief BaseGrid Interface
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

An Interface for the BaseGrid Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EpPathFinding
{
    public class Node : IComparable
    {
        public int x;
        public int y;
        public bool walkable;
        public float heuristicStartToEndLen; // which passes current node
        public float startToCurNodeLen;
        public float? heuristicCurNodeToEndLen;
        public bool isOpened;
        public bool isClosed;
        public Object parent;

        public Node(int iX, int iY, bool? iWalkable = null)
        {
            this.x = iX;
            this.y = iY;
            this.walkable = (iWalkable.HasValue ? iWalkable.Value : false);
            this.heuristicStartToEndLen = 0;
            this.startToCurNodeLen = 0;
            this.heuristicCurNodeToEndLen = null;
            this.isOpened = false;
            this.isClosed = false;
            this.parent = null;

        }

        public void Reset(bool? iWalkable = null)
        {
            if (iWalkable.HasValue)
                walkable = iWalkable.Value;
            this.heuristicStartToEndLen = 0;
            this.startToCurNodeLen = 0;
            this.heuristicCurNodeToEndLen = null;
            this.isOpened = false;
            this.isClosed = false;
            this.parent = null;
        }


        public int CompareTo(object iObj)
        {
            Node tOtherNode = (Node)iObj;
            float result=this.heuristicStartToEndLen - tOtherNode.heuristicStartToEndLen;
            if (result > 0.0f)
                return 1;
            else if (result == 0.0f)
                return 0;
            return -1;
        }

        public static List<GridPos> Backtrace(Node iNode)
        {
            List<GridPos> path = new List<GridPos>();
            path.Add(new GridPos(iNode.x, iNode.y));
            while (iNode.parent != null)
            {
                iNode = (Node)iNode.parent;
                path.Add(new GridPos(iNode.x, iNode.y));
            }
            path.Reverse();
            return path;
        }
    }

    public abstract class BaseGrid
    {

        public BaseGrid()
        {
        }

        public abstract int width { get; protected set; }

        public abstract int height { get; protected set; }

        public abstract Node GetNodeAt(int iX, int iY);

        public abstract bool IsWalkableAt(int iX, int iY);

        public abstract void SetWalkableAt(int iX, int iY, bool iWalkable);

        public abstract Node GetNodeAt(GridPos iPos);

        public abstract bool IsWalkableAt(GridPos iPos);

        public abstract void SetWalkableAt(GridPos iPos, bool iWalkable);

        public List<Node> GetNeighbors(Node iNode, bool iCrossCorners)
        {
            int tX = iNode.x;
            int tY = iNode.y;
            List<Node> neighbors = new List<Node>();
            bool tS0 = false, tD0 = false,
                tS1 = false, tD1 = false,
                tS2 = false, tD2 = false,
                tS3 = false, tD3 = false;

            GridPos pos = new GridPos();
            if (this.IsWalkableAt(pos.Set(tX, tY - 1)))
            {
                neighbors.Add(GetNodeAt(pos));
                tS0 = true;
            }
            if (this.IsWalkableAt(pos.Set(tX + 1, tY)))
            {
                neighbors.Add(GetNodeAt(pos));
                tS1 = true;
            }
            if (this.IsWalkableAt(pos.Set(tX, tY + 1)))
            {
                neighbors.Add(GetNodeAt(pos));
                tS2 = true;
            }
            if (this.IsWalkableAt(pos.Set(tX - 1, tY)))
            {
                neighbors.Add(GetNodeAt(pos));
                tS3 = true;
            }

            if (!iCrossCorners)
            {
                tD0 = tS3 || tS0;
                tD1 = tS0 || tS1;
                tD2 = tS1 || tS2;
                tD3 = tS2 || tS3;
            }
            else
            {
                tD0 = true;
                tD1 = true;
                tD2 = true;
                tD3 = true;
            }

            if (tD0 && this.IsWalkableAt(pos.Set(tX - 1, tY - 1)))
            {
                neighbors.Add(GetNodeAt(pos));
            }
            if (tD1 && this.IsWalkableAt(pos.Set(tX + 1, tY - 1)))
            {
                neighbors.Add(GetNodeAt(pos));
            }
            if (tD2 && this.IsWalkableAt(pos.Set(tX + 1, tY + 1)))
            {
                neighbors.Add(GetNodeAt(pos));
            }
            if (tD3 && this.IsWalkableAt(pos.Set(tX - 1, tY + 1)))
            {
                neighbors.Add(GetNodeAt(pos));
            }
            return neighbors;
        }

        public abstract void Reset();

        public abstract BaseGrid Clone();

    }
}