using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalSudokuGen
{
    public class Point
    {
        //Represents a coordinate [x, y] on a Puzzle, with x, y being either ints from 1-9 or -1
        
        public int x;
        public int y;

        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public Point()
        {
            x = -1;
            y = -1;
        }
    }
}
