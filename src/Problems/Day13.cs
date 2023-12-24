using System.Text;

namespace AdventOfCode2023 
{
    public class Day13: Problem 
    {
        List<List<char[]>> patterns = new();
        List<List<char[]>> rotatedPatterns;
        List<int> originalScores = new();

        public Day13(string inputPath): base(inputPath)
        {
            List<char[]> curr = new();
            foreach (string line in puzzleInputLines) {
                if (line.Trim() == "") {
                    patterns.Add(curr);
                    curr = new();
                } else {
                    curr.Add(line.Trim().ToCharArray());
                }
            }
            patterns.Add(curr);
            rotatedPatterns = new();

            for (int i = 0; i < patterns.Count; i++) {
                var pattern = patterns[i];

                List<StringBuilder> patternBuilder = new();
                foreach (var c in pattern[0]) patternBuilder.Add(new());
                foreach (char[] row in pattern) {
                    for (int j = 0; j < row.Length; j++) {
                        patternBuilder[row.Length - 1 - j].Append(row[j]);
                    }
                }

                rotatedPatterns.Add(patternBuilder.Select(x => x.ToString().ToCharArray()).ToList());
            }
        }

        public override string Part1()
        {   
            long sum = 0;
            for (int i = 0; i < patterns.Count; i++) {
                int score = ScorePattern(patterns[i], rotatedPatterns[i]);
                originalScores.Add(score);
                sum += score;
            }
            return sum.ToString();
        }

        public override string Part2()
        {
            long sum = 0;
            for (int i = 0; i < patterns.Count; i++) {
                sum += GetSmudgedScore(patterns[i], rotatedPatterns[i], originalScores[i]);
            }
            return sum.ToString();
        }

        protected int GetSmudgedScore(List<char[]> pattern, List<char[]> rotatedPattern, int originalScore)
        {
            for (int y = 0; y < pattern.Count; y++) {
                for (int x = 0; x < pattern[y].Length; x++) {
                    // De smudge
                    pattern[y][x] = pattern[y][x] == '#' ? '.' : '#';
                    var rotatedY = rotatedPattern.Count - 1 - x;
                    var rotatedX = y;
                    rotatedPattern[rotatedY][rotatedX] = pattern[y][x];

                    // Test
                    int score = ScorePattern(pattern, rotatedPattern, originalScore);
                    if (score >= 0) {
                        return score;
                    }

                    // Reset
                    pattern[y][x] = pattern[y][x] == '#' ? '.' : '#';
                    rotatedPattern[rotatedY][rotatedX] = pattern[y][x];
                }
            }

            Console.WriteLine("Pattern failed: ");
            PrintPattern(pattern);
            throw new Exception("No valid smudge found!");
        }

        protected int ScorePattern(List<char[]> pattern, List<char[]> rotatedPattern, int originalScore = -1) 
        {
            var rowsAbove = GetHorizontalReflection(pattern, originalScore >= 100 ? originalScore / 100 : -1);
            if (rowsAbove >= 0) {
                return 100 * rowsAbove;
            } 
            var rowsRight = GetHorizontalReflection(rotatedPattern, originalScore < 100 ? rotatedPattern.Count - originalScore : -1);
            if (rowsRight >= 0) {
                return rotatedPattern.Count - rowsRight;
            }
            return -1;
        }

        protected int GetHorizontalReflection(List<char[]> pattern, int previousRow)
        {
            for (int i = 0; i < pattern.Count - 1; i++) {
                if (previousRow != i + 1 && RowsEqual(pattern[i], pattern[i + 1])) {
                    if (CheckPotentialReflection(pattern, i)) {
                        return i + 1;
                    }
                }
            }
            return -1;
        }

        protected bool CheckPotentialReflection(List<char[]> pattern, int i)
        {
            int j = i + 1;
            while (i >= 0 && j < pattern.Count) {
                if (!RowsEqual(pattern[i], pattern[j])) {
                    return false;
                }
                i--;
                j++;
            }
            return true;
        }

        protected bool RowsEqual (char[] a, char[] b) 
        {
            for (int i = 0; i < a.Length; i++) {
                if (a[i] != b[i]) {
                    return false;
                }
            }
            return true;
        }

        protected void PrintPattern(List<char[]> pattern) {
            foreach (var row in pattern) {
                Console.WriteLine(string.Join("", row));
            }
        }
    }
}