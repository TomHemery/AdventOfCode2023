using System.Text.RegularExpressions;

namespace AdventOfCode2023 
{
    public class Day6: Problem 
    {

        long[] durations;
        long[] records;

        long part2Duration;
        long part2Record;

        public Day6(string inputPath): base(inputPath)
        {
            durations = Array.ConvertAll(puzzleInputLines[0].Split(":")[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries), long.Parse);
            records = Array.ConvertAll(puzzleInputLines[1].Split(":")[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries), long.Parse);
            part2Duration = long.Parse(Regex.Replace(puzzleInputLines[0].Split(":")[1].Trim(), @"\s+", ""));
            part2Record = long.Parse(Regex.Replace(puzzleInputLines[1].Split(":")[1].Trim(), @"\s+", ""));
        }

        protected long CalculateWinningOptions(long duration, long record)
        {
            long winningOptions = 0;
            for (long i = 1; i <= duration - 1; i++) 
            {
                long distance = i * (duration - i);
                if (distance > record) {
                    winningOptions++;
                }
            }
            return winningOptions;
        }

        protected long CalculateWinningOptionsPolynomial(double duration, double record)
        {
            double minPress = Math.Floor(duration / 2 - Math.Sqrt(duration * duration - 4 * record) / 2) + 1;
            double maxPress = duration / 2 + Math.Sqrt(duration * duration - 4 * record) / 2;

            if (maxPress % 1 == 0) {
                maxPress -= 1;
            } else {
                maxPress = Math.Floor(maxPress);
            }

            return (long)(maxPress - minPress + 1);
        }

        public override string Part1()
        {   
            long product = 1;
            foreach (var (duration, i) in durations.Select((duration, i) => (duration, i))) {
                long record = records[i];
                product *= CalculateWinningOptionsPolynomial(duration, record);
            }
            return product.ToString();
        }

        public override string Part2()
        {

            return CalculateWinningOptionsPolynomial(part2Duration, part2Record).ToString();
        }
    }
}