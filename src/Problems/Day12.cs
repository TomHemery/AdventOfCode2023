using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2023 
{
    public class Day12: Problem 
    {
        List<(string definition, int[] springs)> rows = [];
        List<(string definition, int[] springs)> expandedRows = [];
        Dictionary<string, bool> validArrangements = [];
        Dictionary<string, long> arrangementCounts = [];
        public Day12(string inputPath): base(inputPath)
        {
            foreach (var line in puzzleInputLines) {
                rows.Add((line.Split(' ')[0], Array.ConvertAll(line.Split(' ')[1].Split(','), int.Parse)));

                expandedRows.Add((
                    string.Join('?', Enumerable.Repeat(line.Split(' ')[0], 5)),
                    Array.ConvertAll(string.Join(',', Enumerable.Repeat(line.Split(' ')[1], 5)).Split(','), int.Parse)
                ));
            }
        }

        public override string Part1()
        {   
            long arrangementCount = 0;
            foreach (var row in rows) {
                arrangementCount += CountArrangements(row.definition, row.springs);
            }
            return arrangementCount.ToString();
        }

        public override string Part2()
        {
            long arrangementCount = 0;
            foreach (var row in expandedRows) {
                arrangementCounts = [];
                validArrangements = [];
                arrangementCount += CountArrangements(row.definition, row.springs);
            }
            return arrangementCount.ToString();
        }

        long CountArrangements(string definition, int[] springs, int springIndex = 0) 
        {
            if (definition == "") { // Base case
                if (springIndex == springs.Length) {
                    return 1;
                } else {
                    return 0;
                }
            }

            string key = definition + string.Join(',', springs) + springIndex; // Cache
            if (arrangementCounts.TryGetValue(key, out long value)) {
                return value;
            }

            long count = 0;
            if (definition[0] == '.') {
                count = CountArrangements(definition.Substring(1), springs, springIndex);
            } else if (definition[0] == '?') {
                count += CountArrangements("#" + definition.Substring(1), springs, springIndex);
                count += CountArrangements("." + definition.Substring(1), springs, springIndex);
            } else if (definition[0] == '#') {
                string nextDefinition = springIndex >= springs.Length ? "E" : RemoveFirstSpring(definition, springs, springIndex);
                if (nextDefinition == "E") {
                    count = 0;
                } else {
                    count = CountArrangements(nextDefinition, springs, springIndex + 1);
                }
            }

            arrangementCounts[key] = count; // Set cache
            return count;
        }

        string RemoveFirstSpring(string definition, int[] springs, int springIndex)
        {
            int springCount = springs[springIndex];
            for (int i = 0; i < definition.Length; i ++) {
                char c = definition[i];
                if (c == '#') {
                    springCount --;
                    if (springCount < 0) {
                        return "E";
                    }
                } else if (c == '.') {
                    if (springCount != 0) {
                        return "E";
                    } else {
                        return definition.Substring(i);
                    }
                } else if (c == '?') {
                    if (springCount > 0) {
                        springCount --;
                    } else if (springCount == 0) {
                        return string.Concat(".", definition.AsSpan(i + 1));
                    }
                }
            }
            if (springCount == 0) {
                return "";
            }
            return "E";
        }
    }
}