using System.Text.RegularExpressions;

namespace AdventOfCode2023 
{
    public class Day1: Problem 
    {
        readonly Dictionary<string, int> tokenToInt = new Dictionary<string, int>() 
        {
            {"one", 1},
            {"two", 2}, 
            {"three", 3},
            {"four", 4}, 
            {"five", 5}, 
            {"six", 6}, 
            {"seven", 7}, 
            {"eight", 8}, 
            {"nine", 9}
        };

        public Day1(string inputPath): base(inputPath)
        {

        }

        public override string Part1()
        {   
            string regex = @"1|2|3|4|5|6|7|8|9";
            long sum = 0;
            foreach (string line in puzzleInputLines) 
            {
                var matches = Regex.Matches(line, regex);
                string first = matches[0].ToString();
                string last = matches[matches.Count - 1].ToString();

                sum += (10 * int.Parse(first) + int.Parse(last));
            }
            return "" + sum;
        }

        public override string Part2()
        {
            string regex = @"(?=(one|two|three|four|five|six|seven|eight|nine|1|2|3|4|5|6|7|8|9))";
            long sum = 0;
            foreach (string line in puzzleInputLines) 
            {
                var matches = Regex.Matches(line, regex);
                string first = matches[0].Groups[1].Value;
                string last = matches[matches.Count - 1].Groups[1].Value;

                int firstInt = tokenToInt.ContainsKey(first) ? tokenToInt[first] : int.Parse(first);
                int lastInt = tokenToInt.ContainsKey(last) ? tokenToInt[last] : int.Parse(last);

                sum += (10 * firstInt + lastInt);
            }
            return "" + sum;
        }
        
    }
}