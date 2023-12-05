using System.Formats.Tar;

namespace AdventOfCode2023 
{
    public class Day3: Problem 
    {

        protected char[,] schematic;

        public Day3(string inputPath): base(inputPath)
        {
            schematic = new char[puzzleInputLines[0].Length, puzzleInputLines.Length];
            for (int y = 0; y < puzzleInputLines.Length; y++) 
            {
                for (int x = 0; x < puzzleInputLines[y].Length; x++)
                {
                    schematic[x, y] = puzzleInputLines[y][x];
                }
            }
        }

        protected bool IsInt(char c)
        {
            int intVal = c - '0';
            return intVal >= 0 && intVal <= 9;
        }

        protected string GetNumberAt(int x, int y, char[,] schematic)
        {
            string stringVal = schematic[x, y].ToString();
            while (x + 1 < schematic.GetLength(0) && IsInt(schematic[x + 1, y])) 
            {
                x++;
                stringVal += schematic[x, y];
            }

            return stringVal;
        }

        protected bool SymbolAroundNumber(int x, int y, int length, char[,] schematic, out (int, int) pos, bool gearCheck = false)
        {
            for (int yOff = -1; yOff <= 1; yOff ++)
            {
                if (y + yOff < 0 || y + yOff >= schematic.GetLength(1))
                {
                    continue;
                }
                for (int xOff = -1; xOff <= length; xOff ++) 
                {
                    if (x + xOff < 0 || x + xOff >= schematic.GetLength(0))
                    {
                        continue;
                    }
                    else if (
                        gearCheck && schematic[x + xOff, y + yOff] == '*' ||
                        !gearCheck && schematic[x + xOff, y + yOff] != '.' && !IsInt(schematic[x + xOff, y + yOff])
                    ) {
                        pos = (x + xOff, y + yOff);
                        return true;
                    }
                }
            }
            pos = (-1, -1);
            return false;
        }

        public override string Part1()
        {   
            long sum = 0;
            for (int y = 0; y < schematic.GetLength(1); y++)
            {
                for (int x = 0; x < schematic.GetLength(0); x++)
                {
                    if (IsInt(schematic[x, y])) {
                        string stringVal = GetNumberAt(x, y, schematic);
                        if (SymbolAroundNumber(x, y, stringVal.Length, schematic, out _)) {
                            sum += int.Parse(stringVal);
                        }
                        x += stringVal.Length - 1;
                    }
                }
            }
            return "" + sum;
        }

        // Assumes a number is only ever associated with one symbol
        public override string Part2()
        {
            Dictionary<(int, int), List<int>> gearDict = new();
            for (int y = 0; y < schematic.GetLength(1); y++)
            {
                for (int x = 0; x < schematic.GetLength(0); x++)
                {
                    if (IsInt(schematic[x, y])) {
                        string stringVal = GetNumberAt(x, y, schematic);
                        if (SymbolAroundNumber(x, y, stringVal.Length, schematic, out (int, int) pos, true)) {
                            if (!gearDict.ContainsKey(pos)) {
                                gearDict[pos] = new();
                            }
                            gearDict[pos].Add(int.Parse(stringVal));
                        }
                        x += stringVal.Length - 1;
                    }
                }
            }

            long sum = 0;
            foreach (KeyValuePair<(int, int), List<int>> kvp in gearDict) 
            {
                if (kvp.Value.Count == 2) 
                {
                    sum += kvp.Value[0] * kvp.Value[1];
                }
            }
            return "" + sum;
        }
        
    }
}