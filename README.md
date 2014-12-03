Minimal-Sudoku-Puzzle-Generator
===============================

Consider a solved Sudoku puzzle. Then, remove one number. Does the resulting puzzle still have one unique solution? Yes, of course. Now do that again. NOW does it still have a unique solution?

What if you did this over and over again until you had a puzzle who still had a unique solution, but removing any number still in the puzzle resulted in a non-unique solution?

That puzzle would be what I call a "minimal" Sudoku puzzle, one that gives as little information as possible while still being unique. This program automatically generates minimal Sudoku puzzles, using backtracking and recursion.
