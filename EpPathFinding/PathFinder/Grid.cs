/*! 
@file Grid.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding>
@date July 16, 2013
@brief Grid Interface
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

An Interface for the Grid Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EpPathFinding
{
    class Node:IComparable{
       public int x;
       public int y;
       public bool walkable;

       public float heuristicStartToEndLen; // which passes current node
       public float startToCurNodeLen;
       public float? heuristicCurNodeToEndLen;
       public bool isOpened;
       public bool isClosed;
       public Object parent;

       public Node(int iX, int iY, bool? iWalkable=null)
       {
          this.x=iX;
          this.y=iY;
          this.walkable = (iWalkable.HasValue ? iWalkable.Value : true);
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
           return (int)(this.heuristicStartToEndLen - tOtherNode.heuristicStartToEndLen);
       }
    }

    class Grid
    {
        public int width{get;private set;}
        public int height{get;private set;}
        private Node[][] nodes;

        public Grid(int iWidth, int iHeight, bool[][] iMatrix=null)
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
                    if (!iMatrix[widthTrav][heightTrav])
                    {
                        tNodes[widthTrav][heightTrav].walkable = false;
                    }
                }
            }
            return tNodes;
      }
        public Node GetNodeAt(int iX, int iY)
        {
            return this.nodes[iX][iY];
        }
     
        public bool IsWalkableAt(int iX, int iY)
        {
            return IsInside(iX, iY) && this.nodes[iX][iY].walkable;
        }

        public bool IsInside(int iX, int iY)
        {
            return (iX >= 0 && iX < width) && (iY >= 0 && iY < height);
        }

        public void SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            this.nodes[iX][iY].walkable = iWalkable;
        }

        public List<Node> GetNeighbors(Node iNode, bool iDontCrossCorners)
        {
            int tX = iNode.x;
            int tY = iNode.y;
            List<Node> neighbors = new List<Node>();
            bool tS0=false,tD0=false,
                tS1=false,tD1=false,
                tS2=false,tD2=false,
                tS3=false,tD3=false;
            Node[][] tNodes=this.nodes;

            if (this.IsWalkableAt(tX, tY - 1))
            {
                neighbors.Add(tNodes[tX][tY - 1]);
                tS0=true;
            }
            if (this.IsWalkableAt(tX + 1, tY))
            {
                neighbors.Add(tNodes[tX + 1][tY]);
                tS1=true;
            }
            if (this.IsWalkableAt(tX, tY + 1))
            {
                neighbors.Add(tNodes[tX][tY + 1]);
                tS2=true;
            }
            if (this.IsWalkableAt(tX - 1, tY))
            {
                neighbors.Add(tNodes[tX - 1][tY]);
                tS3=true;
            }

            if (iDontCrossCorners)
            {
                tD0=tS3 || tS0;
                tD1=tS0 || tS1;
                tD2=tS1 || tS2;
                tD3=tS2 || tS3;
            }
            else
            {
                tD0 = true;
                tD1 = true;
                tD2 = true;
                tD3 = true;
            }

            if (tD0 && this.IsWalkableAt(tX - 1, tY - 1))
            {
                neighbors.Add(tNodes[tX - 1][tY - 1]);
            }
            if (tD1 && this.IsWalkableAt(tX + 1, tY - 1))
            {
                neighbors.Add(tNodes[tX + 1][tY - 1]);
            }
            if (tD2 && this.IsWalkableAt(tX + 1, tY + 1))
            {
                neighbors.Add(tNodes[tX + 1][tY + 1]);
            }
            if (tD3 && this.IsWalkableAt(tX - 1, tY + 1))
            {
                neighbors.Add(tNodes[tX - 1][tY + 1]);
            }
            return neighbors;
        }

        public Grid Clone()
        {
            int tWidth = width;
            int tHeight =height;
            Node[][] tNodes = this.nodes;

            Grid tNewGrid = new Grid(tWidth, tHeight, null);

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
