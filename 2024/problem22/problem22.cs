namespace Year2024;

using Sequence = (int, int, int, int);

public class Problem22
{
    public static void Solve()
    {
        string file = "2024/problem22/input.txt";
        List<long> nums = [.. File.ReadLines(file).Select(long.Parse)];
        Dict<long, List<int>> prices = new([], () => []);
        nums.ForEach(num => prices[num].Add((int)(num % 10)));
        nums.Sum(num => (0..2000).ToEnumerable()
            .Aggregate(num, (n, i) =>
            {
                n = ((n * 64) ^ n) % 16777216;
                n = ((long)Math.Floor((double)n / 32) ^ n) % 16777216;
                n = ((2048 * n) ^ n) % 16777216;
                prices[num].Add((int)(n % 10)); // for part 2
                return n;
            })
        ).WriteLine("Part 1: ");

        CountSet<Sequence> pricesBySequence = new();
        prices.Values.ForEach(ps =>
        {
            Set<Sequence> visited = new();
            for (int i = 1; i < ps.Count - 3; i++)
            {
                Sequence diffs = (
                    ps[i] - ps[i - 1], ps[i + 1] - ps[i],
                    ps[i + 2] - ps[i + 1], ps[i + 3] - ps[i + 2]
                );
                if (visited[diffs]) continue;
                visited.Add(diffs);
                pricesBySequence.AddMany(diffs, ps[i + 3]);
            }
        });
        pricesBySequence[pricesBySequence.Max()].WriteLine("Part 2: ");
    }

}