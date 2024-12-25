namespace Year2024;

public class Problem25
{
    public static void Solve()
    {
        List<List<int>> locks = [];
        List<List<int>> keys = [];
        File.ReadAllText("2024/problem25/input.txt").Split("\n\n").ForEach(str =>
        {
            Grid<char> g = Grid<char>.CharsFromString(str);
            if (str.Split("\n")[0] == "#####")
            {
                locks.Add([.. (0..5).Select(i => g.GetCol(i).IndexOf('.') - 1)]);
            }
            else
            {
                keys.Add([.. (0..5).Select(i => 6 - g.GetCol(i).IndexOf('#'))]);
            }
        });

        int total = 0;
        foreach (List<int> l in locks)
        {
            foreach (List<int> k in keys)
            {
                if ((0..k.Count).All(i => l[i] + k[i] <= 5)) total++;
            }
        }
        total.WriteLine("Part 1:");
    }

}