namespace AdventOfCode2023 
{
    public class Day9: Problem 
    {
        List<int[]> sequences;

        public Day9(string inputPath): base(inputPath)
        {
            sequences = new();
            foreach (string line in puzzleInputLines) {
                sequences.Add(Array.ConvertAll(line.Split(), int.Parse));
            }
        }

        public override string Part1()
        {   
            long sum = 0;
            foreach (var sequence in sequences) {
                sum += GetNextValue(sequence);
            }
            return sum.ToString();
        }
        
        public override string Part2()
        {
            long sum = 0;
            foreach (var sequence in sequences) {
                sum += GetPreviousValue(sequence);
            }
            return sum.ToString();
        }

        int GetNextValue(int[] sequence)
        {
            if (AllSame(sequence)) {
                return sequence[0];
            }
            return sequence[sequence.Length - 1] + GetNextValue(GetSubSequence(sequence));
        }

        int GetPreviousValue(int[] sequence)
        {
            if (AllSame(sequence)) {
                return sequence[0];
            }
            return sequence[0] - GetPreviousValue(GetSubSequence(sequence));
        }

        bool AllSame(int[] sequence)
        {
            int test = sequence[0];
            return sequence.Skip(1).All(x => x == test);
        }

        int[] GetSubSequence(int[] sequence)
        {
            int[] newSequence = new int[sequence.Length - 1];
            for (int i = 0; i < sequence.Length - 1; i++) 
            {
                newSequence[i] = sequence[i + 1] - sequence [i];
            }
            return newSequence;
        }
    }
}