using System.Text.RegularExpressions;

class Problem6
{
    public static void Solve()
    {
        var file = "problem6/testinput.txt";

        List<long> times = [];
        List<long> dists = [];
        foreach (string line in File.ReadAllLines(file))
        {
            if (line.StartsWith("Time"))
            {
                times = Regex.Matches(line, @"\d+")
                    .Select(m => long.Parse(m.Value)).ToList();
            }
            if (line.StartsWith("Distance"))
            {
                dists = Regex.Matches(line, @"\d+")
                    .Select(m => long.Parse(m.Value)).ToList();
            }
        }

        var prod = 1;
        for (var i = 0; i < times.Count; i++)
        {
            var numWays = 0;
            var upper = times[i] / 2;
            while (upper * (times[i] - upper) > dists[i])
            {
                numWays += 2;
                upper--;
            }
            if (times[i] % 2 == 0) numWays--; // double counted wrongly
            prod *= numWays;
        }

        Console.WriteLine(prod);
    }
}
