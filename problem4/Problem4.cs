
using System.Numerics;

class Problem4
{
    public static void Solve()
    {
        var numCards = new Dictionary<int, int>();

        var sum = 0;
        var i = 0;
        foreach (string line in File.ReadAllLines("problem4/input.txt"))
        {
            i++;
            var winningNumbers = line.Split(":")[1].Split("|")[0].Trim().Split(" ")
                .Where(s => s != "")
                .Select(s => int.Parse(s));
            var yourNumbers = line.Split(":")[1].Split("|")[1].Trim().Split(" ")
                .Where(s => s != "")
                .Select(s => int.Parse(s));

            // we have atleast one for ourselves:
            numCards.TryAdd(i, 0);
            numCards[i]++;
            sum++;

            var numWon = 0;
            foreach (int num in winningNumbers)
            {
                if (yourNumbers.Contains(num)) numWon++;
            }

            for (var j = 1; j <= numWon; j++)
            {
                numCards.TryAdd(i + j, 0);
                numCards[i + j] += numCards[i];
                sum += numCards[i];
            }
        }

        Console.WriteLine(sum);
    }

    public static void SolvePt1()
    {
        var sum = 0;
        foreach (string line in File.ReadAllLines("problem4/input.txt"))
        {
            var winningNumbers = line.Split(":")[1].Split("|")[0].Trim().Split(" ")
                .Where(s => s != "")
                .Select(s => int.Parse(s));
            var yourNumbers = line.Split(":")[1].Split("|")[1].Trim().Split(" ")
                .Where(s => s != "")
                .Select(s => int.Parse(s));

            var power = 0;
            foreach (int num in winningNumbers)
            {
                if (yourNumbers.Contains(num)) power++;
            }
            if (power > 0)
            {
                sum += (int)Math.Pow(2, power - 1);
            }
        }
        Console.WriteLine(sum);
    }
}

