/*! 
@file StaticGrid.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding.cs>
@date July 16, 2013
@brief StaticGrid Interface
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

An Interface for the StaticGrid Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EpPathFinding
{
    public class StaticGrid : BaseGrid
    {
        public override int width { get; protected set; }

        public override int height { get; protected set; }

        private Node[][] nodes;

        public StaticGrid(int iWidth, int iHeight, bool[][] iMatrix = null):base()
        {
            width = iWidth;
            height = iHeight;
            this.nodes = BuildNodes(iWidth, iHeight, iMatrix);
        }


        private Node[][] BuildNodes(int iWidth, int iHeight, bool[][] iMatrix)
        {

            Node[][] tNodes = new Node[iWidth][];
            for (int widthTrav = 0; widthTrav < iWidth; widthTrav++)
            {
                tNodes[widthTrav] = new Node[iHeight];
                for (int heightTrav = 0; heightTrav < iHeight; heightTrav++)
                {
                    tNodes[widthTrav][heightTrav] = new Node(widthTrav, heightTrav, null);
                }
            }

            if (iMatrix == null)
            {
                return tNodes;
            }

            if (iMatrix.Length != iWidth || iMatrix[0].Length != iHeight)
            {
                throw new System.ApplicationException("Matrix size does not fit");
            }


            for (int widthTrav = 0; widthTrav < iWidth; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < iHeight; heightTrav++)
                {
                    if (iMatrix[widthTrav][heightTrav])
                    {
                        tNodes[widthTrav][heightTrav].walkable = true;
                    }
                }
            }
            return tNodes;
        }

        public override Node GetNodeAt(int iX, int iY)
        {
            return this.nodes[iX][iY];
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            return IsInside(iX, iY) && this.nodes[iX][iY].walkable;
        }

        protected bool IsInside(int iX, int iY)
        {
            return (iX >= 0 && iX < width) && (iY >= 0 && iY < height);
        }

        public override void SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            this.nodes[iX][iY].walkable = iWalkable;
        }

        protected bool IsInside(GridPos iPos)
        {
            return IsInside(iPos.x, iPos.y);
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            return GetNodeAt(iPos.x, iPos.y);
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return IsWalkableAt(iPos.x, iPos.y);
        }

        public override void SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            Reset(null);
        }

        public void Reset(bool[][] iMatrix)
        {
            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    nodes[widthTrav][heightTrav].Reset();
                }
            }

            if (iMatrix == null)
            {
                return;
            }
            if (iMatrix.Length != width || iMatrix[0].Length != height)
            {
                throw new System.ApplicationException("Matrix size does not fit");
            }

            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    if (!iMatrix[widthTrav][heightTrav])
                    {
                        nodes[widthTrav][heightTrav].walkable = false;
                    }
                }
            }
        }

        public override BaseGrid Clone()
        {
            int tWidth = width;
            int tHeight = height;
            Node[][] tNodes = this.nodes;

            StaticGrid tNewGrid = new StaticGrid(tWidth, tHeight, null);

            Node[][] tNewNodes = new Node[tWidth][];
            for (int widthTrav = 0; widthTrav < tWidth; widthTrav++)
            {
                tNewNodes[widthTrav] = new Node[tHeight];
                for (int heightTrav = 0; heightTrav < tHeight; heightTrav++)
                {
                    tNewNodes[widthTrav][heightTrav] = new Node(widthTrav, heightTrav, tNodes[widthTrav][heightTrav].walkable);
                }
            }
            tNewGrid.nodes = tNewNodes;

            return tNewGrid;
        }
    }


}
