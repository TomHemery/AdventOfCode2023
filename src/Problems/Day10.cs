using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace AdventOfCode2023 
{
    public class Day10: Problem 
    {
        int startX;
        int startY;

        Dictionary<(int, int), (int, int)[]> map = new();

        public Day10(string inputPath): base(inputPath)
        {
            for (int y = 0; y < puzzleInputLines.Length; y++) {
                for (int x = 0; x < puzzleInputLines[0].Length; x++) {
                    map[(x, y)] = GetPipeNeighbours(x, y, puzzleInputLines[y][x]);
                    if (puzzleInputLines[y][x] == 'S') {
                        startX = x;
                        startY = y;
                    }
                }
            }
            ReplaceStartSquare(startX, startY);
            foreach(string line in puzzleInputLines) Console.WriteLine(line);
            map[(startX, startY)] = GetPipeNeighbours(startX, startY, puzzleInputLines[startY][startX]);
        }

        protected (int, int)[] GetPipeNeighbours(int x, int y, char pipe)
        {
            switch (pipe) {
                case '|':
                    return [(x, y + 1), (x, y - 1)];
                case '-':
                    return [(x + 1, y), (x - 1, y)];
                case 'L':
                    return [(x, y - 1), (x + 1, y)];
                case 'J':
                    return [(x, y - 1), (x - 1, y)];
                case '7':
                    return [(x - 1, y), (x, y + 1)];
                case 'F':
                    return [(x + 1, y), (x, y + 1)];
            }
            return [];
        }

        public override string Part1()
        {   
            var start = (startX, startY);
            var prev = start;
            var curr = map[start][0];
            int loopLength = 1;
            do {
                var next = map[curr][0] != prev ? map[curr][0] : map[curr][1];
                prev = curr;
                curr = next;
                loopLength ++;
            } while (curr != start);
            return (loopLength / 2).ToString();
        }
        
        public override string Part2()
        {
            var start = (startX, startY);
            var prev = start;
            var curr = map[start][0];

            char[,] scaledMap = new char[puzzleInputLines[0].Length * 3, puzzleInputLines.Length * 3];
            for (int x = 0; x < scaledMap.GetLength(0); x++) {
                for (int y = 0; y < scaledMap.GetLength(1); y++) {
                    scaledMap[x, y] = ' ';
                }
            }

            DrawPipe(start.Item1, start.Item2, scaledMap);
            do {
                DrawPipe(curr.Item1, curr.Item2, scaledMap);
                var next = map[curr][0] != prev ? map[curr][0] : map[curr][1];
                prev = curr;
                curr = next;
            } while (curr != start);

            char[,] filledScaledMap = (char[,])scaledMap.Clone();
            // Fill in from outside
            for (int x = 0; x < filledScaledMap.GetLength(0); x++) {
                FloodFill(x, 0, filledScaledMap);
                FloodFill(x, filledScaledMap.GetLength(1) - 1, filledScaledMap);
            }
            for (int y = 0; y < filledScaledMap.GetLength(1); y++) {
                FloodFill(0, y, filledScaledMap);
                FloodFill(filledScaledMap.GetLength(0) - 1, y, filledScaledMap);
            }
            SaveBitmap(filledScaledMap);

            return CountInternalSquares(filledScaledMap).ToString();
        }
 
        void FloodFill(int x, int y, char[,] map, char fillChar = 'O', char emptyChar = ' ') 
        {
            Queue<(int, int)> toCheck = new();
            toCheck.Enqueue((x, y));
            while (toCheck.Count > 0) {
                var curr = toCheck.Dequeue();
                if (map[curr.Item1, curr.Item2] != emptyChar) {
                    continue;
                }
                map[curr.Item1, curr.Item2] = fillChar;
                foreach (var neighour in GetNeighbours(curr.Item1, curr.Item2, map, emptyChar)) {
                    toCheck.Enqueue(neighour);
                }
            }
        }

        List<(int, int)> GetNeighbours(int x, int y, char[,] map, char emptyChar) 
        {
            List<(int, int)> results = new();
            if (x - 1 >= 0 && map[x - 1, y] == emptyChar) {
                results.Add((x - 1, y));
            }
            if (x + 1 < map.GetLength(0) && map[x + 1, y] == emptyChar) {
                results.Add((x + 1, y));
            }
            if (y - 1 >= 0 && map[x, y - 1] == emptyChar) {
                results.Add((x, y - 1));
            }
            if (y + 1 < map.GetLength(1) && map[x, y + 1] == emptyChar) {
                results.Add((x, y + 1));
            }
            return results;
        }

        void PrintMap(char[,] map) {
            for (int y = 0; y < map.GetLength(1); y++) {
                for (int x = 0; x < map.GetLength(0); x++) {
                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }
        }

        void SaveBitmap(char[,] map) {
            var bmp = new Bitmap(map.GetLength(0), map.GetLength(1));
            for (int y = 0; y < map.GetLength(1); y++) {
                for (int x = 0; x < map.GetLength(0); x++) {
                    bmp.SetPixel(x, y, map[x, y] == ' ' ? Color.White : Color.Black);
                }
            }
            bmp.Save("AdjustedMap.bmp", ImageFormat.Bmp);
        }

        void ReplaceStartSquare(int x, int y)
        {
            bool left = false;
            bool up = false;
            bool right = false;
            bool down = false;
            if (y - 1 > 0 && "7F|".Contains(puzzleInputLines[y - 1][x])) {
                up = true;
            }
            if (y + 1 < puzzleInputLines.Length && "LJ|".Contains(puzzleInputLines[y + 1][x])) {
                down = true;
            }
            if (x - 1 > 0 && "FL-".Contains(puzzleInputLines[y][x - 1])) {
                left = true;
            }
            if (x + 1 < puzzleInputLines[0].Length && "J7-".Contains(puzzleInputLines[y][x + 1])) {
                right = true;
            }
            if (left && right) {
                puzzleInputLines[y] = puzzleInputLines[y].Replace('S', '-');
            } else if (left && up) {
                puzzleInputLines[y] = puzzleInputLines[y].Replace('S', 'J');
            } else if (left && down) {
                puzzleInputLines[y] = puzzleInputLines[y].Replace('S', '7');
            } else if (right && up) {
                puzzleInputLines[y] = puzzleInputLines[y].Replace('S', 'L');
            } else if (right && down) {
                puzzleInputLines[y] = puzzleInputLines[y].Replace('S', 'F');
            } else if (up && down) {
                puzzleInputLines[y] = puzzleInputLines[y].Replace('S', '|');
            }
        }

        void DrawPipe(int x, int y, char[,] map) {
            var sX = x * 3;
            var sY = y * 3;
            switch (puzzleInputLines[y][x]) {
                case '-': 
                    map[sX, sY + 1] = '■';
                    map[sX + 1, sY + 1] = '■';
                    map[sX + 2, sY + 1] = '■';
                    break;
                case '|': 
                    map[sX + 1, sY] = '■';
                    map[sX + 1, sY + 1] = '■';
                    map[sX + 1, sY + 2] = '■';
                    break;
                case 'J': 
                    map[sX + 1, sY] = '■';
                    map[sX + 1, sY + 1] = '■';
                    map[sX, sY + 1] = '■';
                    break;
                case 'L': 
                    map[sX + 1, sY] = '■';
                    map[sX + 1, sY + 1] = '■';
                    map[sX + 2, sY + 1] = '■';
                    break;
                case '7': 
                    map[sX, sY + 1] = '■';
                    map[sX + 1, sY + 1] = '■';
                    map[sX + 1, sY + 2] = '■';
                    break;
                case 'F': 
                    map[sX + 2, sY + 1] = '■';
                    map[sX + 1, sY + 1] = '■';
                    map[sX + 1, sY + 2] = '■';
                    break;
            }
        }

        int CountInternalSquares(char[,] scaledMap) 
        {
            int count = 0;
            for (int x = 0; x < scaledMap.GetLength(0); x += 3) {
                for (int y = 0; y < scaledMap.GetLength(1); y += 3) {
                    if (SquareInternal(x, y, scaledMap)) {
                        count ++;
                    }
                }
            }
            return count;
        }

        bool SquareInternal(int x, int y, char[,] scaledMap, char spaceChar = ' ') {
            for(int xOff = 0; xOff <= 2; xOff ++) {
                for (int yOff = 0; yOff <= 2; yOff ++) {
                    if (scaledMap[x + xOff, y + yOff] != spaceChar) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}