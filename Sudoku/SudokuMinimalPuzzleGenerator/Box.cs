using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalSudokuGen
{
    public class Box
    {
        //A Box is a 3x3 array representing one of the nine sub-squares in a Puzzle

        public int[,] values;

        public Box()
        {
            values = new int[3,3];
        }

        public void print()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    System.Console.Write(values[i, j]);
                }

                System.Console.Write("\n");
            }
        }

        public Boolean contains(int value)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (values[j, i] == value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Boolean isSolved()
        {

            for (int i = 1; i < 10; i++)
            {
                Boolean isNumFound = false;

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (values[x,y] == i)
                        {
                            isNumFound = true;
                        }
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