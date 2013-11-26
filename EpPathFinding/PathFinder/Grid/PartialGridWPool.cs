/*! 
@file PartialGridWPool.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding.cs>
@date July 16, 2013
@brief PartialGrid with Pool Interface
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

An Interface for the PartialGrid with Pool Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using General;

namespace EpPathFinding
{
    public class PartialGridWPool : BaseGrid
    {
        protected Dictionary<GridPos, Node> nodes;
        private GridRect gridRect;
        private NodePool nodePool;

        public override int width
        {
            get
            {
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
                return gridRect.maxY - gridRect.minY;
            }
            protected set
            {

            }
        }

        public PartialGridWPool(NodePool iNodePool, GridRect? iGridRect = null)
            : base()
        {
            if (iGridRect == null)
                gridRect = new GridRect();
            else
                gridRect = iGridRect.Value;
            nodePool = iNodePool;
            BuildNodes();
        }

        protected void BuildNodes()
        {
            nodes = new Dictionary<GridPos, Node>();
            for (int travX = gridRect.minX; travX <= gridRect.maxX; travX++)
            {
                for (int travY = gridRect.minY; travY <= gridRect.maxY; travY++)
                {
                    Node curNode=nodePool.GetNode(travX,travY);
                    if(curNode!=null)
                        nodes.Add(new GridPos(travX, travY), curNode);
                }
            }
        }
        protected void UpdateNodes()
        {
            GridPos curPos = new GridPos(0, 0);
            Node curNode = null;
            bool containsKey = false;
            for (int travX = gridRect.minX; travX <= gridRect.maxX; travX++)
            {
                curPos.x = travX;
                for (int travY = gridRect.minY; travY <= gridRect.maxY; travY++)
                {
                    curPos.y = travY;
                    curNode = nodePool.GetNode(travX, travY);
                    containsKey = nodes.ContainsKey(curPos);
                    if (curNode != null && !containsKey)
                        nodes.Add(new GridPos(travX, travY), curNode);
                    else if (curNode == null && containsKey)
                        nodes.Remove(curPos);
                }
            }
        }
        public void SetGridRect(GridRect iGridRect)
        {
            gridRect = iGridRect;
            UpdateNodes();
        }


        public void UpdateFromPool()
        {
            UpdateNodes();
        }

        public bool IsInside(int iX, int iY)
        {
            if (iX < gridRect.minX || iX > gridRect.maxX || iY < gridRect.minY || iY > gridRect.maxY)
                return false;
            return true;
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

        public override bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            if (IsInside(iX,iY))
                return false;
            GridPos pos = new GridPos(iX, iY);

            if (iWalkable)
            {
                if (nodes.ContainsKey(pos))
                {
                    return true;
                }
                else
                {
                    nodes.Add(new GridPos(pos.x, pos.y), nodePool.GetNode(pos.x, pos.y, iWalkable));
                }
            }
            else
            {
                if (nodes.ContainsKey(pos))
                {
                    nodes.Remove(pos);
                    nodePool.RemoveNode(pos);
                }
            }
            return true;
        }

        public bool IsInside(GridPos iPos)
        {
            return IsInside(iPos.x, iPos.y);
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
            return nodes.ContainsKey(iPos);
        }

        public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            UpdateNodes();
            foreach (KeyValuePair<GridPos, Node> keyValue in nodes)
            {
                keyValue.Value.Reset();
            }
        }


        public override BaseGrid Clone()
        {
            PartialGridWPool tNewGrid = new PartialGridWPool(nodePool,gridRect);
            return tNewGrid;
        }
    }

}