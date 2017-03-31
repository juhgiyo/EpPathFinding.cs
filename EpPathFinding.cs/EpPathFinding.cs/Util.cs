using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPathFinding.cs
{
    class Util
    {
        public static DiagonalMovement GetDiagonalMovement(bool iCrossCorners, bool iCrossAdjacentPoint)
        {

            if (iCrossCorners && iCrossAdjacentPoint)
            {
                return DiagonalMovement.Always;
            }
            else if (iCrossCorners)
            {
                return DiagonalMovement.IfAtMostOneObstacle;
            }
            else
            {
                return DiagonalMovement.OnlyWhenNoObstacles;
            }
        }
    }
}
