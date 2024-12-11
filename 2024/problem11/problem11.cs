namespace Year2024;

using Memo = (long Stone, int BlinksLeft);

public class Problem11
{
    public static void Solve()
    {
        string file = "2024/problem11/input.txt";
        List<long> stones = File.ReadAllText(file).GetNums().Select(i => (long)i).ToList();

        Iterate(stones, 25).WriteLine("Part 1:");
        Iterate(stones, 75).WriteLine("Part 2:");

        stones.Sum(s => Recurse(s, 25, [])).WriteLine("Part 1:");
        stones.Sum(s => Recurse(s, 75, [])).WriteLine("Part 2:");
    }

    public static long Recurse(long stone, int blinksLeft, Dictionary<Memo, long> memo)
    {
        if (memo.TryGetValue((stone, blinksLeft), out long mtotal)) return mtotal;
        if (blinksLeft == 0) return 1;
        memo[(stone, blinksLeft)] = Blink(stone).Sum(n => Recurse(n, blinksLeft - 1, memo));
        return memo[(stone, blinksLeft)];
    }

    public static long Iterate(List<long> stones, long numIterations)
    {
        Dict<long, long> counts = new(0);
        stones.ForEach(stone => counts[stone]++);
        for (int i = 0; i < numIterations; i++)
        {
            Dict<long, long> next = new(0);
            counts.Keys.ToList()
                .ForEach(s => Blink(s).ForEach(n => next[n] += counts[s]));
            counts = next;
        }
        return counts.Keys.Sum(k => counts[k]);
    }

    public static List<long> Blink(long stone)
    {
        string digits = "" + stone;
        if (stone == 0)
        {
            return [1];
        }
        if (digits.Length % 2 == 0)
        {
            return [
                long.Parse(digits[..(digits.Length / 2)]),
                long.Parse(digits[(digits.Length / 2)..])
            ];
        }
        return [stone * 2024];
    }

}