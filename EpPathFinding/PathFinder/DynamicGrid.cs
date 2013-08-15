/*! 
@file DynamicGrid.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding.cs>
@date July 16, 2013
@brief DynamicGrid Interface
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

An Interface for the DynamicGrid Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EpPathFinding
{
    public class DynamicGrid : BaseGrid
    {
        protected Dictionary<GridPos, Node> nodes;

        private int minX;
        private int maxX;
        private int minY;
        private int maxY;
        private bool notSet;

        public override int width
        {
            get
            {
                return maxX - minX;
            }
            protected set
            {

            }
        }
        public override int height
        {
            get
            {
                return maxY - minY;
            }
            protected set
            {

            }
        }

        public DynamicGrid(List<GridPos> iWalkableGridList = null)
            : base()
        {
            minX = 0;
            minY = 0;
            maxX = 0;
            maxY = 0;
            notSet = true;
            BuildNodes(iWalkableGridList);
        }


        protected void BuildNodes(List<GridPos> iWalkableGridList)
        {

            nodes = new Dictionary<GridPos, Node>();
            if (iWalkableGridList == null)
                return;
            foreach (GridPos gridPos in iWalkableGridList)
            {
                SetWalkableAt(gridPos.x, gridPos.y, true);
            }
        }

        protected override bool IsInside(int iX, int iY)
        {
            GridPos pos = new GridPos(iX, iY);
            return IsInside(pos);
        }

        public override Node GetNodeAt(int iX, int iY)
        {
            GridPos pos = new GridPos(iX, iY);
            return GetNodeAt(pos);
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            GridPos pos = new GridPos(iX, iY);
            return IsWalkableAt(pos);
        }

        public override void SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            GridPos pos = new GridPos(iX, iY);
            if (nodes.ContainsKey(pos))
            {
                this.nodes[pos].walkable = iWalkable;
            }
            else
            {

                if (iX < minX || notSet)
                    minX = iX;
                if (iX > maxX || notSet)
                    maxX = iX;
                if (iY < minY || notSet)
                    minY = iY;
                if (iY > maxX || notSet)
                    maxY = iY;
                nodes.Add(new GridPos(pos.x, pos.y), new Node(pos.x, pos.y, iWalkable));
                notSet = false;
            }
        }

        protected override bool IsInside(GridPos iPos)
        {
            if (nodes.ContainsKey(iPos))
            {
                return true;
            }
            return false;
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            if (nodes.ContainsKey(iPos))
            {
                return nodes[iPos];
            }
            return null;
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return IsInside(iPos) && nodes[iPos].walkable;
        }

        public override void SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }


        public override void Reset()
        {
            Reset(null);
        }
        public void Reset(Dictionary<GridPos, bool> iWalkableMatrix)
        {

            foreach (KeyValuePair<GridPos, Node> keyValue in nodes)
            {
                keyValue.Value.Reset();
            }

            if (iWalkableMatrix == null)
                return;
            foreach (KeyValuePair<GridPos, bool> keyValue in iWalkableMatrix)
            {
                nodes[keyValue.Key].walkable = keyValue.Value;
            }

        }

        public override BaseGrid Clone()
        {
            DynamicGrid tNewGrid = new DynamicGrid(null);

            foreach (KeyValuePair<GridPos, Node> keyValue in nodes)
            {
                tNewGrid.SetWalkableAt(keyValue.Key.x, keyValue.Key.y, keyValue.Value.walkable);

            }

            return tNewGrid;
        }
    }


}

