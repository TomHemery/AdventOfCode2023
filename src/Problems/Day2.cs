using System.Text.RegularExpressions;

namespace AdventOfCode2023 
{
    public class Day2: Problem 
    {
        readonly Dictionary<string, int> colourCounts = new Dictionary<string, int>()
        {
            {"red", 12},
            {"green", 13},
            {"blue", 14}
        };

        public Day2(string inputPath): base(inputPath)
        {

        }

        protected bool ValidateGamePart1(string game)
        {
            var parts = game.Split(": ")[1].Split("; ");
            foreach (var part in parts) 
            {
                var tokens = part.Split(", ");
                foreach (var token in tokens) 
                {
                    var key = token.Split(' ')[1];
                    var count = int.Parse(token.Split(' ')[0]);
                    if (count > colourCounts[key]) 
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override string Part1()
        {   
            int validSum = 0;
            foreach (var game in puzzleInputLines)
            {
                if (ValidateGamePart1(game)) 
                {
                    validSum += int.Parse(game.Split(": ")[0].Split(' ')[1]);
                }
            }
            return "" + validSum;
        }

        public override string Part2()
        {
            long sum = 0;
            foreach (var game in puzzleInputLines)
            {
                var rgbMin = new Dictionary<string, int>() {{"red", 0}, {"green", 0}, {"blue", 0}};
                var parts = game.Split(": ")[1].Split("; ");

                foreach (var part in parts) 
                {
                    var tokens = part.Split(", ");
                    foreach (var token in tokens) 
                    {
                        var key = token.Split(' ')[1];
                        var count = int.Parse(token.Split(' ')[0]);
                        if (count > rgbMin[key]) 
                        {
                            rgbMin[key] = count;
                        }
                    }
                }

                sum += rgbMin["red"] * rgbMin["green"] * rgbMin["blue"];
            }
            return "" + sum;
        }
        
    }
}