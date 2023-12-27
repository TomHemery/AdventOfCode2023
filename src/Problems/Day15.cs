
namespace AdventOfCode2023 
{
    public class Day15: Problem 
    {
        protected string[] initSequence;
        protected List<(string, int)>[] boxes = new List<(string, int)>[256];

        public Day15(string inputPath): base(inputPath)
        {
            initSequence = rawPuzzleInput.Split(',');
            for (int i = 0; i < boxes.Length; i++) {
                boxes[i] = [];
            }
        }

        public override string Part1()
        {   
            long sum = 0;
            foreach (var step in initSequence) {
                sum += Hash(step);
            }
            return sum.ToString();
        }

        public override string Part2()
        {
            foreach (var step in initSequence) {
                string label = step.Split(['-', '='])[0];
                var key = Hash(label);
                var box = boxes[key];
                var operation = step.Contains('=') ? '=' : '-';
                int innerBoxIndex = BoxContains(box, label);
                switch (operation) {
                    case '=':
                        int value = step[step.Length - 1] - '0';
                        if (innerBoxIndex >= 0) {
                            box[innerBoxIndex] = (label, value);
                        } else {
                            box.Add((label, value));
                        }
                        break;
                    case '-':
                        if (innerBoxIndex >= 0) {
                            box.RemoveAt(innerBoxIndex);
                        }
                        break;
                }
            }

            long sum = 0;
            for (int i = 0; i < boxes.Length; i++)
            {
                sum += GetFocusingPower(boxes[i], i);
            }
            return sum.ToString();
        }

        protected int GetFocusingPower(List<(string label, int value)> box, int boxIndex) 
        {
            int sum = 0;
            for (int i = 0; i < box.Count; i++) {
                var (_, value) = box[i];
                sum += (boxIndex + 1) * (i + 1) * value;
            }
            return sum;
        }

        protected int BoxContains(List<(string label, int)> box, string label) 
        {
            for (int i = 0; i < box.Count; i++) {
                if (box[i].label == label) {
                    return i;
                }
            }
            return -1;
        }

        protected int Hash(string input)
        {
            int currentValue = 0;
            foreach (char c in input) {
                currentValue += c;
                currentValue *= 17;
                currentValue %= 256;
            }

            return currentValue;
        }
    }
}