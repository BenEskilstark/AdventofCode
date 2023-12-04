class Problem2 {
    public static void Solve() {
        var sumOfPowers = 0;
        List<string> colors = ["red", "green", "blue"];

        foreach (string line in File.ReadAllLines("problem2/input.txt"))
        {
            var gameIndex = int.Parse(line.Split(":")[0].Split(" ")[1]);
            var gameStrs = line.Split(":")[1].Split(";");
            var maxDict = colors.ToDictionary(c => c, c => 0);

            foreach (string gset in gameStrs)
            {
                var gameDict = colors.ToDictionary(c => c, c => 0);

                foreach (string pair in gset.Split(","))
                {
                    gameDict[pair.Trim().Split(" ")[1]] += int.Parse(pair.Trim().Split(" ")[0]);
                }

                colors.ForEach(c => maxDict[c] = Math.Max(maxDict[c], gameDict[c]));
            }
            var power = maxDict["red"] * maxDict["green"] * maxDict["blue"];
            sumOfPowers += power;
        }
        Console.WriteLine("66681 " + sumOfPowers);
    }
}
