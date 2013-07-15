/*! 
@file GridBox.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding>
@date July 16, 2013
@brief GridBox Interface
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

An Interface for the GridBox Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EpPathFinding
{
    enum BoxType { Start, End, Wall, Normal };

    class GridBox:IDisposable
    {
        public int x, y, width, height;
        public SolidBrush brush;
        public Rectangle boxRec;
        public BoxType boxType;
        public GridBox(int iX, int iY,BoxType iType)
        {
            this.x = iX;
            this.y = iY;
            this.boxType = iType;
            switch (iType)
            {
                case BoxType.Normal:
                    brush = new SolidBrush(Color.WhiteSmoke);
                    break;
                case BoxType.End:
                    brush = new SolidBrush(Color.Red);
                    break;
                case BoxType.Start:
                    brush = new SolidBrush(Color.Green);
                    break;
                case BoxType.Wall:
                    brush = new SolidBrush(Color.Gray);
                    break;
            
            }
            width = 18;
            height = 18;
            boxRec = new Rectangle(x, y, width, height);
        }

        public void DrawBox(Graphics iPaper,BoxType iType)
        {
            if (iType == boxType)
            {
                boxRec.X = x;
                boxRec.Y = y;
                iPaper.FillRectangle(brush, boxRec);
            }
        }

        
        public void SwitchBox()
        {
            switch (this.boxType)
            {
                case BoxType.Normal:
                    if (this.brush != null)
                        this.brush.Dispose();
                    this.brush = new SolidBrush(Color.Gray);
                    this.boxType = BoxType.Wall;
                    break;
                case BoxType.Wall:
                    if (this.brush != null)
                        this.brush.Dispose();
                    this.brush = new SolidBrush(Color.WhiteSmoke);
                    this.boxType = BoxType.Normal;
                    break;

            }
        }

        public void SetNormalBox()
        {
            if (this.brush != null)
                this.brush.Dispose();
           this.brush = new SolidBrush(Color.WhiteSmoke);
           this.boxType = BoxType.Normal;
        }

        public void SetStartBox()
        {
            if (this.brush != null)
                this.brush.Dispose();
            this.brush = new SolidBrush(Color.Green);
            this.boxType = BoxType.Start;
        }

        public void SetEndBox()
        {
            if (this.brush != null)
                this.brush.Dispose();
            this.brush = new SolidBrush(Color.Red);
            this.boxType = BoxType.End;
        }


        public void Dispose()
        {
            if(this.brush!=null)
                this.brush.Dispose();

        }
    }
}
