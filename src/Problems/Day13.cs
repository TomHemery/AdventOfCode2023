using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2023 
{
    public class Day13: Problem 
    {
        List<List<string>> patterns = new();
        List<List<string>> rotatedPatterns;

        public Day13(string inputPath): base(inputPath)
        {
            List<string> curr = new();
            foreach (string line in puzzleInputLines) {
                if (line.Trim() == "") {
                    patterns.Add(curr);
                    curr = new();
                } else {
                    curr.Add(line.Trim());
                }
            }
            patterns.Add(curr);
            rotatedPatterns = new();

            for (int i = 0; i < patterns.Count; i++) {
                var pattern = patterns[i];

                List<StringBuilder> patternBuilder = new();
                foreach (var c in pattern[0]) patternBuilder.Add(new());
                foreach (string row in pattern) {
                    for (int j = 0; j < row.Length; j++) {
                        patternBuilder[row.Length - 1 - j].Append(row[j]);
                    }
                }

                rotatedPatterns.Add(patternBuilder.Select(x => x.ToString()).ToList());
            }
        }

        public override string Part1()
        {   
            long sum = 0;
            for (int i = 0; i < patterns.Count; i++) {
                var rowsAbove = GetHorizontalReflection(patterns[i]);
                if (rowsAbove >= 0) {
                    sum += 100 * rowsAbove;
                } else {
                    sum += rotatedPatterns[i].Count - GetHorizontalReflection(rotatedPatterns[i]);
                }
            }
            return sum.ToString();
        }

        public override string Part2()
        {
            return "";
        }

        protected int GetHorizontalReflection(List<string> pattern)
        {
            for (int i = 0; i < pattern.Count - 1; i++) {
                if (pattern[i] == pattern[i + 1]) {
                    if (CheckPotentialReflection(pattern, i)) {
                        return i + 1;
                    }
                }
            }
            return -1;
        }

        protected bool CheckPotentialReflection(List<string> pattern, int i)
        {
            int j = i + 1;
            while (i >= 0 && j < pattern.Count) {
                if (pattern[i] != pattern[j]) {
                    return false;
                }
                i--;
                j++;
            }
            return true;
        }
    }
}