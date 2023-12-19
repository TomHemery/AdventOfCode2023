namespace AdventOfCode2023 
{
    public class Day11: Problem 
    {
        List<(int, int)> galaxyLocations = [];

        char[,] galaxyMap;
        List<int> expansionCols = [];
        List<int> expansionRows = [];

        public Day11(string inputPath): base(inputPath)
        {
            for (int i = 0; i < puzzleInputLines.Length; i++) {
                var line = puzzleInputLines[i];
                if (!line.Contains('#')) {
                    expansionRows.Add(i);
                }
            } 
            for (int i = puzzleInputLines[0].Length - 1; i >= 0; i--) {
                if (ColumnContainsNoGalaxies(i, puzzleInputLines)) {
                    expansionCols.Add(i);
                }
            }

            galaxyMap = new char[puzzleInputLines[0].Length, puzzleInputLines.Length];
            for (int y = 0; y < puzzleInputLines.Length; y++) {
                for (int x = 0; x < puzzleInputLines[0].Length; x++) {
                    galaxyMap[x, y] = puzzleInputLines[y][x];
                    if (puzzleInputLines[y][x] == '#') {
                        galaxyLocations.Add((x, y));
                    }
                }
            }
        }

        protected bool ColumnContainsNoGalaxies(int col, string[] galaxyMap)
        {
            for (int i = 0; i < galaxyMap.Length; i++) {
                if (galaxyMap[i][col] == '#') {
                    return false;
                }
            }
            return true;
        }

        public override string Part1()
        {   
            long totalDist = 0;
            for (int i = 0; i < galaxyLocations.Count - 1; i ++) {
                for (int j = i + 1; j < galaxyLocations.Count; j ++) {
                    totalDist += ExpandedTaxiCabDist(galaxyLocations[i], galaxyLocations[j], 2);
                }
            }
            return totalDist.ToString();
        }

        public override string Part2()
        {
            long totalDist = 0;
            for (int i = 0; i < galaxyLocations.Count - 1; i ++) {
                for (int j = i + 1; j < galaxyLocations.Count; j ++) {
                    totalDist += ExpandedTaxiCabDist(galaxyLocations[i], galaxyLocations[j], 1000000);
                }
            }
            return totalDist.ToString();
        }

        protected int TaxiCabDist((int x, int y) a, (int x, int y) b) 
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        protected long ExpandedTaxiCabDist((long x, long y) a, (long x, long y) b, long expansionFactor) 
        {
            int expansionXCount = 0;
            foreach (int col in expansionCols) {
                if (ValueBetween(col, a.x, b.x)) {
                    expansionXCount++;
                }
            }
            int expansionYCount = 0;
            foreach (int row in expansionRows) {
                if (ValueBetween(row, a.y, b.y)) {
                    expansionYCount++;
                }
            }

            return 
                Math.Abs(a.x - b.x) + expansionXCount * (expansionFactor - 1) + 
                Math.Abs(a.y - b.y) + expansionYCount * (expansionFactor - 1);
        }

        protected bool ValueBetween(long val, long a, long b) 
        {
            if (b < a) {
                (b, a) = (a, b);
            }
            return val > a && val < b;
        }
    }
}