namespace AdventOfCode2023
{
    public abstract class Problem 
    {
        protected string rawPuzzleInput;
        protected string[] puzzleInputLines;

        public Problem(string inputPath)
        {
            rawPuzzleInput = File.ReadAllText(inputPath);
            puzzleInputLines = File.ReadAllLines(inputPath);
        }

        public abstract string Part1();
        public abstract string Part2();
    }
}