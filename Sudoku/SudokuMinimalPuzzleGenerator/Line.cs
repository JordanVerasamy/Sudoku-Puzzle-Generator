using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalSudokuGen
{
    public class Line
    {
        //A Line is an array of length 9 representing either a row or a column in a Puzzle.

        public int[] values;

        public Line()
        {
            values = new int[9];
        }

        public void print()
        {
            for (int i = 0; i < 9; i++)
            {
                System.Console.Write(values[i]);

                if (i % 3 == 2)
                {
                    System.Console.Write(" ");
                }
            }
        }

        public Boolean contains(int value)
        {
            for (int i = 0; i < 9; i++)
            {
                if (values[i] == value)
                {
                    return true;
                }
            }

            return false;
        }
        
        public Boolean isSolved()
        {

            for (int i = 1; i < 10; i++)
            {
                Boolean isNumFound = false;

                for (int x = 0; x < 9; x++)
                {
                    if (values[x] == i)
                    {
                        isNumFound = true;
                    }
                }

                if (isNumFound == false)
                {
                    return false;
                }
            }

            return true;

        }

    }
}
