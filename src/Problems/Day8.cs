namespace AdventOfCode2023 
{
    public class Day8: Problem 
    {
        Dictionary<string, (string, string)> map = new();
        string instructions;

        public Day8(string inputPath): base(inputPath)
        {
            instructions = puzzleInputLines[0];

            for (int i = 2; i < puzzleInputLines.Length; i++) 
            {
                string line = puzzleInputLines[i];
                map[line.Split(" = ")[0]] = (line.Split(" = ")[1].Trim(['(', ')']).Split(", ")[0], line.Split(" = ")[1].Trim(['(', ')']).Split(", ")[1]);
            }
        }

        public override string Part1()
        {   
            return CountSteps("AAA", "ZZZ").ToString();
        }
        
        public override string Part2()
        {
            string[] startingPoints = map.Keys.Where(x => x[2] == 'A').ToArray();
            long[] pathLengths = new long[startingPoints.Length];
            for (int i = 0; i < startingPoints.Length; i++)
            {
                pathLengths[i] = CountSteps(startingPoints[i]);
            }
            return LCM(pathLengths).ToString();
        }

        int CountSteps(string start, string? dest = null)
        {
            string curr = start;
            int steps = 0;
            int instructionPointer = 0;
            while (dest == null ? curr[2] != 'Z' : curr != dest) 
            {
                curr = instructions[instructionPointer] == 'L' ? map[curr].Item1 : map[curr].Item2;    
                instructionPointer = instructionPointer == instructions.Length - 1 ? 0 : instructionPointer + 1;
                steps++;
            }
            return steps;
        }

        static long LCM(long[] numbers)
        {
            return numbers.Aggregate(LCM);
        }
        static long LCM(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}