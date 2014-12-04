using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalSudokuGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Puzzle p = new Puzzle();

            //Consumes a puzzle from an input text file
            p.pullFromTextFile("input_almost_minimal.txt");
            Console.WriteLine("Initial puzzle:\n");
            p.print();

            //Prints the minimal puzzle
            findAndPrintMinimal(p);

            Console.ReadLine();
        }

        static void findAndPrintMinimal(Puzzle p)
        {
            p = p.findMinimalPuzzle();

            if (p != null)
            {
                Console.WriteLine("The puzzle with the fewest starting numbers whose solution is unique and matches the solution of the input puzzle is: \n");
                p.print();
            }
        }
    }
}
