/*! 
@file Heuristic.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding.cs>
@date July 16, 2013
@brief Heuristic Function Interface
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

An Interface for the Heuristic Function Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPathFinding
{
    enum HeuristicMode
    {
        MANHATTAN,
        EUCLIDEAN,
        CHEBYSHEV,
        
    };

    class Heuristic
    {
      public static float Manhattan(int iDx, int iDy)
      {
          return (float)iDx + iDy;
      }

      public static float Euclidean(int iDx, int iDy)
      {
          float tFdx = (float)iDx;
          float tFdy = (float)iDy;
          return (float) Math.Sqrt((double)(tFdx * tFdx + tFdy * tFdy));
      }

      public static float Chebyshev(int iDx, int iDy)
      {
          return (float)Math.Max(iDx, iDy);
      }

    }


}
