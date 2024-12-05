namespace Year2024;

public class Problem5
{
    public static void Solve()
    {
        string file = "2024/problem5/input.txt";
        Dict<int, Set<int>> orders = new(new(), () => new());
        File.ReadAllText(file)
            .Split("\r\n\r\n")[0].Split("\r\n")
            .Select(l => l.GetNums()).ToList()
            .ForEach(o => orders[o[0]].Add(o[1]));
        List<List<int>> updates = File.ReadAllText(file)
            .Split("\r\n\r\n")[1].Split("\r\n")
            .Select(l => l.GetNums()).ToList();

        updates
            .Where(u => IsInOrder(orders, u))
            .Sum(u => u[u.Count / 2])
            .WriteLine("Part 1:");

        updates
            .Where(u => !IsInOrder(orders, u))
            .Select(u => u.FSort((a, b) => orders[b][a] ? -1 : orders[a][b] ? 1 : 0))
            .Sum(u => u[u.Count / 2])
            .WriteLine("Part 2:");
    }

    private static bool IsInOrder(Dict<int, Set<int>> orders, List<int> update)
    {
        for (int i = update.Count - 1; i > 0; i--)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (orders[update[i]][update[j]])
                {
                    return false;
                }
            }
        }
        return true;
    }
}