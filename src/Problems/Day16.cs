
namespace AdventOfCode2023 
{
    public class Day16: Problem 
    {
        char[,] mirrorMap;
        Dictionary<(int, int), bool> energisedTiles = [];
        Dictionary<(int, int, int, int), bool> previousBeams = [];
        List<(int x, int y, int velX, int velY)> beams = [];

        public Day16(string inputPath): base(inputPath)
        {
            mirrorMap = new char[puzzleInputLines[0].Length, puzzleInputLines.Length];
            for (int y = 0; y < puzzleInputLines.Length; y++) {
                for (int x = 0; x < puzzleInputLines[0].Length; x++) {
                    mirrorMap[x, y] = puzzleInputLines[y][x];
                }
            }
        }

        public override string Part1()
        {   
            beams.Add((-1, 0, 1, 0));
            return ProcessBeams().ToString();
        }

        public override string Part2()
        {
            int bestAcivation = int.MinValue;
            for (int x = 0; x < mirrorMap.GetLength(0); x++) {
                Reset();
                beams.Add((x, -1, 0, 1));
                int activation = ProcessBeams();
                if (activation > bestAcivation) {
                    bestAcivation = activation;
                }
                Reset();
                beams.Add((x, mirrorMap.GetLength(1), 0, -1));
                activation = ProcessBeams();
                if (activation > bestAcivation) {
                    bestAcivation = activation;
                }
            }
            for (int y = 0; y < mirrorMap.GetLength(1); y++) {
                Reset();
                beams.Add((-1, y, 1, 0));
                int activation = ProcessBeams();
                if (activation > bestAcivation) {
                    bestAcivation = activation;
                }
                Reset();
                beams.Add((mirrorMap.GetLength(0), y, -1, 0));
                activation = ProcessBeams();
                if (activation > bestAcivation) {
                    bestAcivation = activation;
                }
            }
            return bestAcivation.ToString();
        }

        protected void Reset()
        {
            energisedTiles = [];
            previousBeams = [];
            beams = [];
        }

        protected int ProcessBeams()
        {
            while (beams.Count > 0) {
                ProcessBeamStep();
            }
            return energisedTiles.Values.Count - 1;
        }

        protected void ProcessBeamStep() 
        {
            // Console.WriteLine(string.Join(',', beams));
            for (int i = beams.Count - 1; i >= 0; i--) {
                var beam = beams[i];
                if (previousBeams.ContainsKey(beam)) {
                    beams.RemoveAt(i);
                    continue;
                }
                
                previousBeams[beam] = true;
                energisedTiles[(beam.x, beam.y)] = true;
                (int x, int y) next = VecAdd((beam.x, beam.y), (beam.velX, beam.velY));

                if (next.x < 0 || next.y < 0 || next.x >= mirrorMap.GetLength(0) || next.y >= mirrorMap.GetLength(1)) {
                    beams.RemoveAt(i);
                    continue;
                }

                char nextTile = mirrorMap[next.x, next.y];
                switch (nextTile) {
                    case '\\' :
                        var temp = beam.velX;
                        beam.velX = beam.velY > 0 ? 1 : beam.velY < 0 ? -1 : 0;
                        beam.velY = temp > 0 ? 1 : temp < 0 ? -1 : 0;
                        break;
                    case '/':
                        temp = beam.velX;
                        beam.velX = beam.velY > 0 ? -1 : beam.velY < 0 ? 1 : 0;
                        beam.velY = temp > 0 ? -1 : temp < 0 ? 1 : 0;
                        break;
                    case '|': 
                        if (beam.velX != 0) { // split
                            beam.velX = 0;
                            beam.velY = 1;
                            beams.Add((next.x, next.y, 0, -1));
                        }
                        break;
                    case '-':
                        if (beam.velY != 0) { // split
                            beam.velY = 0;
                            beam.velX = 1;
                            beams.Add((next.x, next.y, -1, 0));
                        }
                        break;
                }
                beam.x = next.x;
                beam.y = next.y;
                beams[i] = beam;
            }
        }

        protected (int, int) VecAdd ((int x, int y) a, (int x, int y) b) 
        {
            return (a.x + b.x, a.y + b.y);
        }

        protected void PrintMap()
        {
            for (int y = 0; y < mirrorMap.GetLength(1); y++) {
                for (int x = 0; x < mirrorMap.GetLength(0); x++) {
                    if (energisedTiles.ContainsKey((x, y))) {
                        Console.Write('#');
                    } else {
                        Console.Write(mirrorMap[x, y]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}