namespace AdventOfCode2023 
{
    public class Day7: Problem 
    {
        protected List<(string, int)> hands;
        public Day7(string inputPath): base(inputPath)
        {
            hands = new();
            foreach (string line in puzzleInputLines)
            {
                hands.Add((line.Split(" ")[0], int.Parse(line.Split(" ")[1])));
            }
        }

        protected Dictionary<char, int> GetCardCount((string, int) hand)
        {
            Dictionary<char, int> cardValueCount = new();
            foreach (char c in hand.Item1) 
            {
                if (cardValueCount.ContainsKey(c)) {
                    cardValueCount[c] ++;
                } else {
                    cardValueCount[c] = 1;
                }
            }
            return cardValueCount;
        }

        protected int ScoreAdjustedHand(Dictionary<char, int> cardValueCount)
        {
            bool threeOfAKindPresent = false;
            bool pairPresent = false;
            foreach (int val in cardValueCount.Values)
            {
                switch (val)
                {
                    case 5:
                        return 7; // five of a kind
                    case 4:
                        return 6; // four of a kind
                    case 3:
                        if (pairPresent)
                        {
                            return 5; // full house
                        }
                        threeOfAKindPresent = true;
                        break;
                    case 2:
                        if (threeOfAKindPresent)
                        {
                            return 5; // full house
                        }
                        else if (pairPresent) 
                        {
                            return 3; // two pair
                        }
                        pairPresent = true;
                        break;
                }
            }
            if (threeOfAKindPresent) {
                return 4; // three of a kind
            }
            if (pairPresent) {
                return 2; // pair
            }
            return 1; // high card
        }

        protected int ScoreHandJokers((string, int) hand)
        {
            if (!hand.Item1.Contains('J') || hand.Item1 == "JJJJJ") {
                return ScoreHand(hand);
            }
            Dictionary<char, int> cardValueCount = GetCardCount(hand);
            Dictionary<char, int> jokerAdjustedCardCount = new();
            foreach (KeyValuePair<char, int> kvp in cardValueCount)
            {
                if (kvp.Key != 'J') {
                    jokerAdjustedCardCount[kvp.Key] = kvp.Value;
                }
            }

            int maxCardNum = jokerAdjustedCardCount.Values.Max();
            char bestCard = jokerAdjustedCardCount.Keys.Where(x => cardValueCount[x] == maxCardNum).MaxBy(x => GetCardStrength(x));
            jokerAdjustedCardCount[bestCard] += cardValueCount['J'];

            return ScoreAdjustedHand(jokerAdjustedCardCount);
        }

        protected int ScoreHand((string, int) hand)
        {
            Dictionary<char, int> cardValueCount = GetCardCount(hand);

            return ScoreAdjustedHand(cardValueCount);
        }

        protected int GetCardStrength(char card, bool useJokers = false)
        {
            if (int.TryParse(card.ToString(), out int strength)) 
            {
                return strength;
            }
            switch (card)
            {
                case 'T':
                    return 10;
                case 'J':
                    if (useJokers) {
                        return 1;
                    }
                    return 11;
                case 'Q':
                    return 12;
                case 'K':
                    return 13;
                case 'A':
                    return 14;
            }

            throw new Exception(String.Format("Card {0} was not valid", card));
        }

        protected int CompareHands((string, int) handA, (string, int) handB, bool useJokers = false)
        {
            int handABetter = useJokers ? 
                Math.Sign(ScoreHandJokers(handA) - ScoreHandJokers(handB)) : 
                Math.Sign(ScoreHand(handA) - ScoreHand(handB));
            if (handABetter != 0) {
                return handABetter;
            } else {
                for (int i = 0; i < handA.Item1.Length; i++) {
                    var cardAStrength = GetCardStrength(handA.Item1[i], useJokers);
                    var cardBStrength = GetCardStrength(handB.Item1[i], useJokers);
                    if (cardAStrength > cardBStrength) {
                        return 1;
                    } else if (cardAStrength < cardBStrength) {
                        return -1;
                    }
                }
            }
            return 0;
        }

        public override string Part1()
        {   
            hands.Sort(delegate((string, int) handA, (string, int) handB)
            {
                return CompareHands(handA, handB);
            });

            long sum = 0;
            foreach (var (hand, i) in hands.Select((hand, i) => (hand, i))) {
                sum += hand.Item2 * (i + 1);
            }
            return sum.ToString();
        }

        public override string Part2()
        {
            hands.Sort(delegate((string, int) handA, (string, int) handB)
            {
                return CompareHands(handA, handB, true);
            });

            long sum = 0;
            foreach (var (hand, i) in hands.Select((hand, i) => (hand, i))) {
                sum += hand.Item2 * (i + 1);
            }
            return sum.ToString();
        }
    }
}