/*! 
@file ResultBox.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding>
@date July 16, 2013
@brief ResultBox Interface
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

An Interface for the ResultBox Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EpPathFinding
{
    enum ResultBoxType { Opened,Closed };
    class ResultBox
    {
        public int x, y, width, height;
        public SolidBrush brush;
        public Rectangle boxRec;
        public ResultBoxType boxType;
        public ResultBox(int iX, int iY, ResultBoxType iType)
        {
            this.x = iX;
            this.y = iY;
            this.boxType = iType;
            switch (iType)
            {
                case ResultBoxType.Opened:
                    brush = new SolidBrush(Color.AliceBlue);
                    break;
                case ResultBoxType.Closed:
                    brush = new SolidBrush(Color.LightGreen);
                    break;
              
            
            }
            width = 18;
            height = 18;
            boxRec = new Rectangle(x, y, width, height);
        }

        public void drawBox(Graphics iPaper)
        {
            boxRec.X = x;
            boxRec.Y = y;
            iPaper.FillRectangle(brush, boxRec);
         
        }


        public void Dispose()
        {
            if(this.brush!=null)
                this.brush.Dispose();

        }
    }
}
