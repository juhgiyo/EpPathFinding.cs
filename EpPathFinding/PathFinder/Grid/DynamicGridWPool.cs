/*! 
@file DynamicGridWPool.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding.cs>
@date July 16, 2013
@brief DynamicGrid with Pool Interface
@version 2.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

@section DESCRIPTION

An Interface for the DynamicGrid with Pool Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace EpPathFinding
{
    public class DynamicGridWPool : BaseGrid
    {
        protected Dictionary<GridPos, Node> nodes;
        private GridRect gridRect;
        private bool notSet;

        private NodePool nodePool;

        public override int width
        {
            get
            {
                if (notSet)
                    SetBoundingBox();
                return gridRect.maxX - gridRect.minX;
            }
            protected set
            {

            }
        }

        public override int height
        {
            get
            {
                if (notSet)
                    SetBoundingBox();
                return gridRect.maxY - gridRect.minY;
            }
            protected set
            {

            }
        }

        public DynamicGridWPool(NodePool iNodePool, List<GridPos> iWalkableGridList = null)
            : base()
        {
            gridRect = new GridRect();
            gridRect.minX = 0;
            gridRect.minY = 0;
            gridRect.maxX = 0;
            gridRect.maxY = 0;
            notSet = true;
            nodePool = iNodePool;
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

        private void SetBoundingBox()
        {
            notSet = true;
            foreach (KeyValuePair<GridPos, Node> pair in nodes)
            {
                if (pair.Key.x < gridRect.minX || notSet)
                    gridRect.minX = pair.Key.x;
                if (pair.Key.x > gridRect.maxX || notSet)
                    gridRect.maxX = pair.Key.x;
                if (pair.Key.y < gridRect.minY || notSet)
                    gridRect.minY = pair.Key.y;
                if (pair.Key.y > gridRect.maxY || notSet)
                    gridRect.maxY = pair.Key.y;
                notSet = false;
            }
            notSet = false;
        }

        public override bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            GridPos pos = new GridPos(iX, iY);

            if (iWalkable)
            {
                if (nodes.ContainsKey(pos))
                {
                    return true;
                }
                else
                {
                    if (iX < gridRect.minX || notSet)
                        gridRect.minX = iX;
                    if (iX > gridRect.maxX || notSet)
                        gridRect.maxX = iX;
                    if (iY < gridRect.minY || notSet)
                        gridRect.minY = iY;
                    if (iY > gridRect.maxY || notSet)
                        gridRect.maxY = iY;
                    nodes.Add(new GridPos(pos.x, pos.y), nodePool.GetNode(pos.x, pos.y, iWalkable));
                    notSet = false;
                }
            }
            else
            {
                if (nodes.ContainsKey(pos))
                {
                    nodes.Remove(pos);
                    nodePool.RemoveNode(pos);
                    if (iX == gridRect.minX || iX == gridRect.maxX || iY == gridRect.minY || iY == gridRect.maxY)
                        notSet = true;
                }
            }
            return true;
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
            return  nodes.ContainsKey(iPos);
        }

        public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            Reset(null);
        }

        public void Reset(List<GridPos> iWalkableGridList)
        {

            foreach (KeyValuePair<GridPos, Node> keyValue in nodes)
            {
                keyValue.Value.Reset();
            }

            if (iWalkableGridList == null)
                return;
            foreach (KeyValuePair<GridPos, Node> keyValue in nodes)
            {
                if (iWalkableGridList.Contains(keyValue.Key))
                    SetWalkableAt(keyValue.Key, true);
                else
                    SetWalkableAt(keyValue.Key, false);
            }
        }

        public override BaseGrid Clone()
        {
            DynamicGridWPool tNewGrid = new DynamicGridWPool(null);

            foreach (KeyValuePair<GridPos, Node> keyValue in nodes)
            {
                tNewGrid.SetWalkableAt(keyValue.Key.x, keyValue.Key.y, true);

            }

            return tNewGrid;
        }
    }

}