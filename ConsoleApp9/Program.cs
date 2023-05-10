namespace ConsoleApp9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Spelling Correction");
            Console.WriteLine("2. Stock Trading");
            Console.Write("Enter your choice (1/2): ");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                SpellingCorrection();
            }
            else if (choice == 2)
            {
                StockTrading();
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter 1 or 2.");
            }
        }

        static void SpellingCorrection()
        {
            string[] allWords = File.ReadAllLines("word_list.txt");
            string[] allWordsLower = Array.ConvertAll(allWords, x => x.ToLower());
            Console.WriteLine("Write your sentence:");
            string input = Console.ReadLine();
            string[] splitedInput = input.Split(new char[] { ' ', ',', '.', ':', ';', '!', '?' });
            var mistakes = new List<string>();
            foreach (var word in splitedInput)
            {
                if (word == "")
                {
                    continue;
                }

                if (!allWordsLower.Contains(word.ToLower()))
                {
                    mistakes.Add(word);
                }
            }

            Console.WriteLine("-------------------------");

            if (mistakes.Count > 0)
            {
                Console.WriteLine($"You have made mistakes in these words: {string.Join(", ", mistakes)}");
            }

            Console.WriteLine("-------------------------");

            foreach (var mistake in mistakes)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                foreach (var word in allWords)
                {
                    var distance = DamerauLevenshteinDistance(mistake, word);
                    dictionary[word] = distance;
                }

                List<string> similarWords = new List<string>();
                var sortedDictionary = dictionary.OrderBy(x => x.Value)
                    .ToDictionary(x => x.Key, x => x.Value);
                for (int i = 0; i < 3; i++)
                {
                    similarWords.Add(sortedDictionary.Keys.ElementAt(i));
                }

                Console.WriteLine($"It seems that for {mistake} can be used {string.Join(", ", similarWords)} ");
            }
        }

        static int DamerauLevenshteinDistance(string s, string t)
        {
            int m = s.Length;
            int n = t.Length;
            int[,] distance = new int[m + 1, n + 1];

            if (m == 0) return n;
            if (n == 0) return m;

            for (int i = 0; i <= m; i++)
            {
                distance[i, 0] = i;
            }

            for (int j = 0; j <= n; j++)
            {
                distance[0, j] = j;
            }

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    int del = distance[i - 1, j] + 1;
                    int ins = distance[i, j - 1] + 1;
                    int sub = distance[i - 1, j - 1] + cost;
                    distance[i, j] = Math.Min(Math.Min(del, ins), sub);

                    if (i > 1 && j > 1 && s[i - 1] == t[j - 2] && s[i - 2] == t[j - 1])
                    {
                        distance[i, j] = Math.Min(distance[i, j], distance[i - 2, j - 2] + cost);
                    }
                }
            }

            return distance[m, n];
        }

        static void StockTrading()
        {
            int[] prices = new int[] { 543, 564, 532, 546, 551, 583, 576, 523, 534, 557 };
            int maxProfit = MaxProfit(prices);
            Console.WriteLine($"Максимальний прибуток: {maxProfit}");
        }

        public static int MaxProfit(int[] prices)
        {
            if (prices.Length <= 1)
            {
                return 0;
            }

            int maxProfit = 0;
            int minPrice = prices[0];

            for (int i = 1; i < prices.Length; i++)
            {
                if (prices[i] < minPrice)
                {
                    minPrice = prices[i];
                }
                else
                {
                    maxProfit = Math.Max(maxProfit, prices[i] - minPrice);
                }
            }

            return maxProfit;
        }
    }
}