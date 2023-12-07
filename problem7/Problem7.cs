class Problem7
{
    public static void Solve()
    {
        List<CamelPokerHand> hands = [];
        foreach (string line in File.ReadAllLines("problem7/input.txt"))
        {
            hands.Add(new CamelPokerHand(
                line.Split(" ")[0],
                int.Parse(line.Split(" ")[1])
            ));
        }

        hands.Sort();
        hands.Reverse();

        // Console.WriteLine(string.Join("\n", hands));

        int sum = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            sum += (i + 1) * hands[i].Bid;
        }

        Console.WriteLine(sum);
    }
}