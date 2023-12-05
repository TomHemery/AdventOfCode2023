namespace AdventOfCode2023 
{
    public class Day5: Problem 
    {
        List<List<long[]>> maps = new();
        long[] seeds;

        public Day5(string inputPath): base(inputPath)
        {
            seeds = Array.ConvertAll(puzzleInputLines[0].Split(": ")[1].Split(), long.Parse);
            List<long[]> currMap = new();
            for (long i = 3; i < puzzleInputLines.Length; i++)
            {
                string line = puzzleInputLines[i].Trim();
                if (line.Contains(":"))
                {   
                    maps.Add(currMap); 
                    currMap = new();
                } 
                else if (line != "")
                {
                    var spec = Array.ConvertAll(line.Split(), long.Parse);
                    long transformationValue = spec[0] - spec[1];
                    // srcStart, srcEnd, transformationValue
                    currMap.Add([spec[1], spec[1] + spec[2] - 1, transformationValue]);
                }
            }
            maps.Add(currMap);
        }

        protected long ApplyMap(long input, List<long[]> map)
        {
            foreach (var spec in map) 
            {
                if (input >= spec[0] && input <= spec[1])
                {
                    return input + spec[2];
                }
            }
            return input;
        }

        protected long SeedNumToLocation(long seedNum)
        {
            long current = seedNum;
            foreach (var map in maps)
            {
                current = ApplyMap(current, map);
            }
            return current;
        }

        public override string Part1()
        {   
            long minLocation = long.MaxValue;
            foreach (long seed in seeds)
            {
                long location = SeedNumToLocation(seed);
                minLocation = location < minLocation ? location : minLocation;
            }
            return minLocation.ToString();
        }

        public override string Part2()
        {
            long minLocation = long.MaxValue;

            for (int i = 0; i < seeds.Length; i+= 2)
            {
                Console.WriteLine(seeds[i] + " - " + (seeds[i] + seeds[i+1]) + " {" + seeds[i + 1] + "}");
                for (long seed = seeds[i]; seed < seeds[i] + seeds[i+1]; seed++) {
                    if (seed % 1000000 == 0) 
                    {
                        Console.Write(seed + " ");
                    }
                    long location = SeedNumToLocation(seed);
                    minLocation = location < minLocation ? location : minLocation;
                }
                Console.WriteLine();
            }
            return minLocation.ToString();
        }
    }
}