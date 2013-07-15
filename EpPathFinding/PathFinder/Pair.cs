/*! 
@file Pair.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding>
@date July 16, 2013
@brief Generic Pair Interface
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

An Interface for the Generic Pair Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpPathFinding
{
    class Pair<T,U>
    {
        public Pair()
        {
        }

        public Pair(T iFirst, U iSecond)
        {
            this.first = iFirst;
            this.second = iSecond;
        }

        public T first { get; set; }
        public U second { get; set; }
    }
}
