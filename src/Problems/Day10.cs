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
            map[(startX, startY)] = GetStartNeighbours(startX, startY);
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

        protected (int, int)[] GetStartNeighbours(int x, int y)
        {
            List<(int, int)> result = new List<(int, int)>();
            if (x - 1 >= 0 && "-FL".Contains(puzzleInputLines[y][x - 1])) {
                result.Add((x - 1, y));
            }
            if (x + 1 < puzzleInputLines[y].Length && "-7J".Contains(puzzleInputLines[y][x + 1])) {
                result.Add((x + 1, y));
            }
            if (y - 1 >= 0 && "|7F".Contains(puzzleInputLines[y - 1][x])) {
                result.Add((x, y - 1));
            }
            if (y + 1 < puzzleInputLines.Length && "|JL".Contains(puzzleInputLines[y + 1][x])) {
                result.Add((x, y + 1));
            }
            return result.ToArray();
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

            char[,] adjustedMap = new char[puzzleInputLines[0].Length, puzzleInputLines.Length];
            for (int x = 0; x < adjustedMap.GetLength(0); x++) {
                for (int y = 0; y < adjustedMap.GetLength(1); y++) {
                    adjustedMap[x, y] = ' ';
                }
            }

            adjustedMap[curr.Item1, curr.Item2] = '■';

            do {
                var next = map[curr][0] != prev ? map[curr][0] : map[curr][1];
                prev = curr;
                curr = next;
                adjustedMap[curr.Item1, curr.Item2] = '■';
            } while (curr != start);

            PrintMap(adjustedMap);

            char[,] filledMap = (char[,])adjustedMap.Clone();
            
            for (int y = 0; y < adjustedMap.GetLength(1); y++) {
                for (int x = 0; x < adjustedMap.GetLength(0); x++) {
                    if (filledMap[x, y] == ' ') {
                        FloodFill(x, y, filledMap);
                    }
                }
            }

            // Fill in outside
            for (int x = 0; x < filledMap.GetLength(0); x++) {
                FloodFill(x, 0, filledMap, 'O', 'I');
                FloodFill(x, filledMap.GetLength(1) - 1, filledMap, 'O', 'I');
            }
            for (int y = 0; y < filledMap.GetLength(1); y++) {
                FloodFill(0, y, filledMap, 'O', 'I');
                FloodFill(filledMap.GetLength(0) - 1, y, filledMap, 'O', 'I');
            }

            PrintMap(filledMap);

            int count = 0;
            foreach (var c in filledMap) {
                if (c == 'I') {
                    count++;
                }
            }

            return count.ToString();
        }
 
        void FloodFill(int x, int y, char[,] map, char fillChar = 'I', char emptyChar = ' ') 
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
            if (x > 0 && map [x - 1, y] == emptyChar) {
                results.Add((x - 1, y));
            } 
            if (x < map.GetLength(0) - 1 && map [x + 1, y] == emptyChar) {
                results.Add((x + 1, y));
            }
            if (y > 0 && map [x, y - 1] == emptyChar) {
                results.Add((x, y - 1));
            } 
            if (y < map.GetLength(1) - 1 && map [x, y + 1] == emptyChar) {
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
    }
}