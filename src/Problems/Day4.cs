namespace AdventOfCode2023 
{
    public class Day4: Problem 
    {
        public Day4(string inputPath): base(inputPath)
        {
            
        }

        public override string Part1()
        {   
            int sum = 0;
            foreach (string card in puzzleInputLines) 
            {
                var values = card.Split(": ")[1];
                var winningNumbers =  Array.ConvertAll(values.Split(" | ")[0].Split(" ", StringSplitOptions.RemoveEmptyEntries), int.Parse);
                var numbers = Array.ConvertAll(values.Split(" | ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries), int.Parse);

                int score = 0;
                foreach (var number in numbers)
                {
                    if (winningNumbers.Contains(number))
                    {
                        score = score == 0 ? 1 : score + score;
                    }
                }
                sum += score;
            }
            return sum.ToString();
        }

        public override string Part2()
        {
            // list of card index (from 0) and number of winning values on that card
            List<(int, int)> originalCards = new();
            foreach (var (card, i) in puzzleInputLines.Select((card, i) => (card, i)))
            {
                var values = card.Split(": ")[1];
                var winningNumbers =  Array.ConvertAll(values.Split(" | ")[0].Split(" ", StringSplitOptions.RemoveEmptyEntries), int.Parse);
                var numbers = Array.ConvertAll(values.Split(" | ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries), int.Parse);
                numbers = numbers.Where(x => winningNumbers.Contains(x)).ToArray();
                originalCards.Add((i, numbers.Length));
            }

            var cards = new List<(int, int)>(originalCards);

            for (int i = 0; i < cards.Count; i++) 
            {
                var card = cards[i];
                for (int j = 1; j <= card.Item2; j++) 
                {
                    cards.Add(originalCards[card.Item1 + j]);
                }
            }
            return cards.Count.ToString();
        }
        
    }
}