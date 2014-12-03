using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MinimalSudokuGen
{
    public class Puzzle 
    {
        //A Puzzle is an object representing a single Sudoku 9x9 array of either integers from 1-9 or a blank spot (represented by 0)

        public int[,] values;

        public Puzzle()
        {
            values = new int[9, 9];
        }

        #region Core Functions
        public void print()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (values[j, i] != 0)
                    {
                        System.Console.Write(values[j, i]);
                    }
                    else
                    {
                        System.Console.Write("_");
                    }

                    if (j % 3 == 2)
                    {
                        System.Console.Write(" ");
                    }
                }

                if (i % 3 == 2)
                {
                    System.Console.Write("\n\n");
                }
                else
                {
                    System.Console.Write("\n");
                }
            }
        }

        //Divide the 9x9 grid into a 2D array a, each a[i,j] being a Box. This returns a[i,j]
        public Box getBox(int x, int y)
        {
            Box box = new Box();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    box.values[i, j] = this.values[3 * x + i, 3 * y + j];
                }
            }

            return box;
        }

        // Returns the y-th row in the puzzle
        public Line getRow(int y)
        {
            Line row = new Line();

            for (int i = 0; i < 9; i++)
            {
                row.values[i] = this.values[i, y];
            }

            return row;
        }

        //Returns the x-th column in the puzzle
        public Line getColumn(int x)
        {
            Line col = new Line();

            for (int i = 0; i < 9; i++)
            {
                col.values[i] = this.values[x, i];
            }

            return col;
        }

        //Returns true if and only if this puzzle satisfies the rules of a Sudoku solution
        public Boolean isSolved()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!this.getBox(j, i).isSolved())
                    {
                        return false;
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                if ((!this.getRow(i).isSolved()) || (!this.getColumn(i).isSolved()))
                {
                    return false;
                }
            }

            return true;
        }

        //Sets this puzzle equal to the text file fileName
        public void pullFromTextFile(String fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    String line = sr.ReadToEnd();

                    string[] lines = Regex.Split(line, "\r\n");

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            int x = (int)(Char.GetNumericValue(lines[i][j]));
                            this.values[j, i] = x;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

        }
        #endregion

        #region Algorithm Helpers
        
        //Returns a Point representing the next slot in the puzzle which is blank, or returns a Point
        //with coordinates (-1, -1) if the puzzle is full (see the Point constructor)
        public Point getNextEmptySlot()
        {
            Point point = new Point();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (this.values[j, i] == 0)
                    {
                        point.x = j;
                        point.y = i;
                        return point;
                    }
                }
            }

            return point;
        }

        //A simplified version of findAllSolutions that uses the same basic logic but ends operation once more than one solution is found.
        //This is more efficient than using findAllSolution.Count() when we just want to know whether a puzzle has exactly one unique solution.
        public List<Puzzle> isSolutionUniqueHelper()
        {
            List<Puzzle> solutions = new List<Puzzle>();
            List<Puzzle> branchesLeftToEval = new List<Puzzle>();

            branchesLeftToEval = this.getNeighbours();

            while (true)
            {
                if (solutions.Count > 1)
                {
                    return solutions;
                }

                if (branchesLeftToEval.Count == 0)
                {
                    if (this.isSolved())
                    {
                        solutions.Add(this);
                    }

                    return solutions;
                }

                Puzzle firstElement = branchesLeftToEval.ElementAt(0);
                List<Puzzle> solutionsOfFirstElement = firstElement.isSolutionUniqueHelper();

                if (solutionsOfFirstElement.Count == 0)
                {
                    branchesLeftToEval.Remove(firstElement);
                }
                else
                {
                    solutions = solutions.Concat(solutionsOfFirstElement).ToList();
                    branchesLeftToEval.Remove(firstElement);
                }
            }
        }

        public Puzzle findReducedPuzzle(int x, int y)
        {
            Puzzle reducedPuzzle = new Puzzle();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    reducedPuzzle.values[j, i] = this.values[j, i];
                }
            }

            if (this.values[x, y] != 0)
            {
                reducedPuzzle.values[x, y] = 0;

                if (reducedPuzzle.isSolutionUnique())
                {
                    reducedPuzzle.print();
                    return reducedPuzzle;
                }
            }

            return null;
        }

        //Returns true if and only if inserting num into this puzzle at location point would not 
        //result in any row, column, or box having two instances of the same number
        public Boolean isValidInsertion(Point point, int num)
        {
            int x = point.x;
            int y = point.y;

            return !(this.getRow(y).contains(num)
                  || this.getColumn(x).contains(num)
                  || this.getBox(x / 3, y / 3).contains(num));
        }
        #endregion

        #region Algorithm Functions

        //Tries to add each number from 1-9 into the next empty slot, returns a list of all 
        //puzzles which have done this successfully without placing duplicates in a row, column, or box
        public List<Puzzle> getNeighbours()
        {
            List<Puzzle> neighbours = new List<Puzzle>();

            Point nextEmptySlot = this.getNextEmptySlot();
            int x = nextEmptySlot.x;
            int y = nextEmptySlot.y;

            if (nextEmptySlot.x == -1 && nextEmptySlot.y == -1)
            {
                return neighbours;
            }

            for (int i = 1; i < 10; i++)
            {
                Puzzle p = new Puzzle();

                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        p.values[j, k] = this.values[j, k];
                    }
                }

                if (this.isValidInsertion(nextEmptySlot, i))
                {
                    p.values[x, y] = i;
                    neighbours.Add(p);
                }
            }

            return neighbours;
        }

        // Returns true if and only if this puzzle has exactly one solution
        public Boolean isSolutionUnique()
        {
            if (this.isSolutionUniqueHelper().Count == 1)
            {
                return true;
            }

            return false;
        }

        //Returns a list of all solutions to this puzzle
        public List<Puzzle> findAllSolutions()
        {
            List<Puzzle> solutions = new List<Puzzle>();
            List<Puzzle> branchesLeftToEval = new List<Puzzle>();

            branchesLeftToEval = this.getNeighbours();

            while (true)
            {
                if (branchesLeftToEval.Count == 0)
                {
                    if (this.isSolved())
                    {
                        solutions.Add(this);
                    }
                    return solutions;
                }

                Puzzle firstElement = branchesLeftToEval.ElementAt(0);
                List<Puzzle> solutionsOfFirstElement = firstElement.findAllSolutions();

                if (solutionsOfFirstElement.Count == 0)
                {
                    branchesLeftToEval.Remove(firstElement);
                }
                else
                {
                    solutions = solutions.Concat(solutionsOfFirstElement).ToList();
                    branchesLeftToEval.Remove(firstElement);
                }
            }
        }

        //Returns true if and only if this puzzle matches p in every cell
        public Boolean isSamePuzzle(Puzzle p)
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (this.values[x, y] != p.values[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //Returns true if and only if listOfPuzzles contains a puzzle that matches this one
        public Boolean isContained(List<Puzzle> listOfPuzzles)
        {
            foreach (Puzzle p in listOfPuzzles)
            {
                if (this.isSamePuzzle(p))
                {
                    return true;
                }
            }

            return false;
        }

        //Returns the minimal puzzle corresponding to this one
        public Puzzle findMinimalPuzzle()
        {
            Boolean isSolutionUnique = this.isSolutionUnique();

            if (!isSolutionUnique)
            {
                System.Console.WriteLine("This isn't unique!");
                return null;
            }

            List<Puzzle> listOfCandidates = new List<Puzzle>();
            Puzzle initialPuzzle = new Puzzle();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    initialPuzzle.values[i, j] = this.values[i, j];
                }
            }

            listOfCandidates.Add(initialPuzzle);

            do
            {
                int len = listOfCandidates.Count;
                for (int i = 0; i < len; i++)
                {
                    if (i >= listOfCandidates.Count)
                    {
                        break;
                    }

                    Puzzle candidate = listOfCandidates.ElementAt(i);

                    for (int y = 0; y < 9; y++)
                    {
                        for (int x = 0; x < 9; x++)
                        {
                            Puzzle nextPuzzle = candidate.findReducedPuzzle(x, y);

                            if (nextPuzzle != null)
                            {
                                if (!nextPuzzle.isContained(listOfCandidates))
                                {
                                    listOfCandidates.Add(nextPuzzle);
                                }
                            }
                        }
                    }

                    if (listOfCandidates.Count > 1)
                    {
                        listOfCandidates.Remove(candidate);
                    }

                }

            } while (listOfCandidates.Count > 1);

            return listOfCandidates.ElementAt(0);

        }

        #endregion
    }
}















