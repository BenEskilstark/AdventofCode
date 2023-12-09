using System.Text.RegularExpressions;

class Problem8
{
    public static void Solve()
    {
        List<int> indices = [];
        Dictionary<string, List<string>> map = [];
        List<string> allCoords = [];
        List<int> periods = [];
        List<bool> done = [];

        foreach (string line in File.ReadAllLines("problem8/testinput.txt"))
        {
            if (indices.Count == 0)
            {
                indices = line.ToCharArray().Select(c => c == 'L' ? 0 : 1).ToList();
                continue;
            }
            List<string> coords = Regex.Matches(line, @"\w{3}").Select(m => m.Value).ToList();

            if (coords[0].ToCharArray()[2] == 'A')
            {
                allCoords.Add(coords[0]);
                periods.Add(-1);
                done.Add(false);
            }

            map[coords[0]] = [coords[1], coords[2]];
        }

        Console.WriteLine(string.Join(", ", allCoords));

        long numSteps = 0;
        int index = 0;
        while (!done.All(t => t))
        {
            for (int i = 0; i < allCoords.Count; i++)
            {
                string coord = allCoords[i];
                string nextCoord = map[coord][indices[index]];

                if (nextCoord.ToCharArray()[2] == 'Z')
                {
                    if (periods[i] == -1)
                    {
                        periods[i] = 0;
                    }
                    else
                    {
                        done[i] = true;
                    }
                }
                if (!done[i] && periods[i] > -1)
                {
                    periods[i]++;
                }
                allCoords[i] = nextCoord;
            }
            numSteps++;
            index = (index + 1) % indices.Count;
        }
        Console.WriteLine(String.Join(", ", periods));
        Console.WriteLine(numSteps);
        Console.WriteLine(LCM(periods.Select(p => (long)p).ToArray()));
    }

    static long LCM(long[] numbers)
    {
        return numbers.Aggregate(lcm);
    }
    static long lcm(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }
    static long GCD(long a, long b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
}