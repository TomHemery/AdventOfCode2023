namespace AdventOfCode2023 
{
    public class Day7: Problem 
    {
        protected Dictionary<char, int> cardStrengthMap = new() {
            {'2', 2}, {'3', 3}, {'4', 4}, {'5', 5}, {'6', 6}, {'7', 7}, {'8', 8}, {'9', 9}, // number cards
            {'T', 10}, {'J', 11}, {'Q', 12}, {'K', 13}, {'A', 14} // face cards
        };

        protected List<(string, int)> hands;
        public Day7(string inputPath): base(inputPath)
        {
            hands = new();
            foreach (string line in puzzleInputLines)
            {
                hands.Add((line.Split(" ")[0], int.Parse(line.Split(" ")[1])));
            }
        }

        protected Dictionary<char, int> GetCardCount(string hand)
        {
            Dictionary<char, int> cardValueCount = new();
            foreach (char c in hand) 
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
            int max = cardValueCount.Values.Max();
            if (max == 4 || max == 5) {
                return max + 2; //five of a kind or 4 of a kind (6 or 7)
            }
            if (cardValueCount.ContainsValue(3)) {
                if (cardValueCount.ContainsValue(2)) {
                    return 5; // full house
                }
                return 4; // three of a kind
            }
            if (cardValueCount.ContainsValue(2)) {
                if (cardValueCount.Values.Where(x => x == 2).Count() > 1) {
                    return 3; // two pair
                }
                return 2; // one pair
            }
            return 1; // high card
        }

        protected int ScoreHandJokers(string hand)
        {
            if (!hand.Contains('J') || hand == "JJJJJ") {
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

        protected int ScoreHand(string hand)
        {
            Dictionary<char, int> cardValueCount = GetCardCount(hand);
            return ScoreAdjustedHand(cardValueCount);
        }

        protected int GetCardStrength(char card, bool useJokers = false)
        {
            if (useJokers && card == 'J') {
                return 1;
            }
            return cardStrengthMap[card];
        }

        protected int CompareHands(string handA, string handB, bool useJokers = false)
        {
            int handABetter = useJokers ? 
                Math.Sign(ScoreHandJokers(handA) - ScoreHandJokers(handB)) : 
                Math.Sign(ScoreHand(handA) - ScoreHand(handB));
            if (handABetter != 0) {
                return handABetter;
            } else {
                for (int i = 0; i < handA.Length; i++) {
                    var cardAStrength = GetCardStrength(handA[i], useJokers);
                    var cardBStrength = GetCardStrength(handB[i], useJokers);
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
                return CompareHands(handA.Item1, handB.Item1);
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
                return CompareHands(handA.Item1, handB.Item1, true);
            });

            long sum = 0;
            foreach (var (hand, i) in hands.Select((hand, i) => (hand, i))) {
                sum += hand.Item2 * (i + 1);
            }
            return sum.ToString();
        }
    }
}