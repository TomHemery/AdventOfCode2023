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
                Console.WriteLine(row.definition);
                arrangementCounts = [];
                validArrangements = [];
                arrangementCount += CountArrangements(row.definition, row.springs);
            }
            return arrangementCount.ToString();
        }

        long CountArrangements(string definition, int[] springs) 
        {
            string key = definition + string.Join(',', springs);

            if (arrangementCounts.TryGetValue(key, out long value)) {
                return value;
            }

            if (!ArrangementValid(definition, springs)) {
                arrangementCounts[key] = 0;
                return 0;
            }

            int changeIndex = definition.IndexOf('?');
            if (changeIndex < 0) {
                if (ArrangementValid(definition, springs)) {
                    return 1; 
                } else { 
                    return 0;
                }
            }

            StringBuilder sb = new(definition);
            sb[changeIndex] = '#';
            long count = CountArrangements(sb.ToString(), springs);
            sb[changeIndex] = '.';
            count += CountArrangements(sb.ToString(), springs);

            arrangementCounts[key] = count;

            return count;
        }

        bool ArrangementValid(string arrangement, int[] springs)
        {
            string key = arrangement + string.Join(',', springs);
            if (validArrangements.TryGetValue(key, out bool value)) {
                return value;
            }

            int springIndex = 0;
            int springCounter = springs[springIndex];
            bool onSpring = false;
            foreach (char c in arrangement) {
                if (c == '?') {
                    validArrangements[key] = true;
                    return true; // encountered a question mark without failing, we don't know what to do yet
                }

                if (c == '#') {
                    if (springIndex >= springs.Length) {
                        validArrangements[key] = false;
                        return false;
                    }
                    springCounter --;
                    onSpring = true;
                } else if (c == '.') {
                    if (onSpring && springCounter != 0) {
                        validArrangements[key] = false;
                        return false;
                    } else if (onSpring) {
                        springIndex ++;
                        springCounter = springIndex < springs.Length ? springs[springIndex] : springCounter; 
                        onSpring = false;
                    }
                } 
            }

            if (springCounter != 0 || springIndex < springs.Length - 1) {
                validArrangements[key] = false;
                return false;
            }

            validArrangements[key] = true;
            return true;
        }
    }
}