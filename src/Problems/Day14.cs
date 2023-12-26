using System.Text;

namespace AdventOfCode2023 
{
    public class Day14: Problem 
    {
        protected char[,] rockMap = new char[0,0];
        protected (int, int)[] cycleDirs = [(0, -1), (-1, 0), (0, 1), (1, 0)];
        Dictionary<string, int> rockMapDict = new();

        public Day14(string inputPath): base(inputPath)
        {
            ResetRockMap();
        }

        public override string Part1()
        {   
            SlideAll((0, -1));
            return ScoreLoad(rockMap).ToString();
        }

        public override string Part2()
        {
            long cycleCount = 1000000000;
            ResetRockMap();
            int i = 0;
            while (i < cycleCount) { // essentially forever
                i++;
                DoCycle();
                string key = MapToString(rockMap);
                if (rockMapDict.TryGetValue(key, out int loopStart)) {
                    int loopLength = i - loopStart;
                    long remainder = (cycleCount - loopStart) % loopLength;
                    for (long r = 0; r < remainder; r++) {
                        DoCycle();
                    }
                    return ScoreLoad(rockMap).ToString();
                }
                rockMapDict[key] = i;
            }
            return ScoreLoad(rockMap).ToString();
        }

        protected void DoCycle()
        {
            foreach (var dir in cycleDirs) {
                SlideAll(dir);
            }
        }

        protected void ResetRockMap() 
        {
            rockMap = new char[puzzleInputLines[0].Length, puzzleInputLines.Length];
            for (int y = 0; y < puzzleInputLines.Length; y++) {
                for (int x = 0; x < puzzleInputLines[y].Length; x++) {
                    rockMap[x, y] = puzzleInputLines[y][x];
                }
            }
        }

        protected void SlideAll((int x, int y) dir)
        {
            for (int y = dir.y <= 0 ? 0 : rockMap.GetLength(1) - 1; dir.y <= 0 ? y < rockMap.GetLength(1) : y >= 0; y = dir.y <= 0 ? y + 1 : y - 1) {
                for (int x = dir.x <= 0 ? 0 : rockMap.GetLength(0) - 1; dir.x <= 0 ? x < rockMap.GetLength(0) : x >= 0; x = dir.x <= 0 ? x + 1 : x - 1) {
                    if (rockMap[x, y] == 'O') {
                        SlideRockAt((x, y), rockMap, dir);
                    }
                }
            }
        }

        protected void SlideRockAt((int x, int y) pos, char[,] rockMap, (int x, int y) dir) 
        {
            rockMap[pos.x, pos.y] = '.';
            var next = VecAdd(pos, dir);
            while(next.x >= 0 && next.y >= 0 && next.x < rockMap.GetLength(0) && next.y < rockMap.GetLength(1)) {
                if (rockMap[next.x, next.y] != '.') {
                    break;
                }
                pos = next;
                next = VecAdd(next, dir);
            }
            rockMap[pos.x, pos.y] = 'O';
        }

        protected int ScoreLoad(char[,] rockMap) {
            int score = 0;
            for (int y = 0; y < rockMap.GetLength(1); y++) {
                for (int x = 0; x < rockMap.GetLength(0); x++) {
                    if (rockMap[x, y] == 'O') {
                        score += rockMap.GetLength(1) - y;
                    }
                }
            }
            return score;
        }

        protected (int x, int y) VecAdd((int x, int y) a, (int x, int y) b) 
        {
            return (a.x + b.x, a.y + b.y);
        }

        protected string MapToString(char[,] map) 
        {
            StringBuilder sb = new();
            for (int y = 0; y < map.GetLength(1); y++) {
                for (int x = 0; x < map.GetLength(0); x++) {
                    sb.Append(map[x, y]);
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}