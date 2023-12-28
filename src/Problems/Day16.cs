
namespace AdventOfCode2023 
{
    public class Day16: Problem 
    {
        char[,] mirrorMap;

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
            return ProcessBeams([(-1, 0, 1, 0)]).ToString();
        }

        public override string Part2()
        {
            int bestAcivation = int.MinValue;
            for (int x = 0; x < mirrorMap.GetLength(0); x++) {
                int activation = ProcessBeams([(x, -1, 0, 1)]);
                if (activation > bestAcivation) {
                    bestAcivation = activation;
                }
                activation = ProcessBeams([(x, mirrorMap.GetLength(1), 0, -1)]);
                if (activation > bestAcivation) {
                    bestAcivation = activation;
                }
            }
            for (int y = 0; y < mirrorMap.GetLength(1); y++) {
                int activation = ProcessBeams([(-1, y, 1, 0)]);
                if (activation > bestAcivation) {
                    bestAcivation = activation;
                }
                activation = ProcessBeams([(mirrorMap.GetLength(0), y, -1, 0)]);
                if (activation > bestAcivation) {
                    bestAcivation = activation;
                }
            }
            return bestAcivation.ToString();
        }

        protected int ProcessBeams(List<(int x, int y, int velX, int velY)> beams)
        {
            Dictionary<(int, int), bool> energisedTiles = [];
            Dictionary<(int, int, int, int), bool> previousBeams = [];
            while (beams.Count > 0) {
                ProcessBeamStep(beams, energisedTiles, previousBeams);
            }
            return energisedTiles.Values.Count - 1;
        }

        protected void ProcessBeamStep(
            List<(int x, int y, int velX, int velY)> beams, 
            Dictionary<(int, int), bool> energisedTiles, 
            Dictionary<(int, int, int, int), bool> previousBeams
        ) {
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
    }
}