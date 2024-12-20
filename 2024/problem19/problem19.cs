namespace Year2024;

public class Problem19
{
    public static void Solve()
    {
        string file = File.ReadAllText("2024/problem19/input.txt");
        List<string> towels = [.. file.Split("\n\n")[0].Split(", ")];
        List<string> patterns = file.Split("\n\n")[1].Split("\n")
            .Where(p => new Memo(towels).GetNumCombos(p) > 0).ToList();

        patterns.Count.WriteLine("Part 1:");
        patterns.Sum(new Memo(towels).GetNumCombos).WriteLine("Part 2:");
    }

    private class Memo(List<string> Towels)
    {
        public Dict<string, long> ToEnd { get; set; } = new(0);
        public long GetNumCombos(string pattern)
        {
            if (pattern == "") return 1;
            if (ToEnd.ContainsKey(pattern)) return ToEnd[pattern];
            ToEnd[pattern] = Towels
                .Where(pattern.StartsWith)
                .Sum(t => GetNumCombos(pattern[t.Length..]));
            return ToEnd[pattern];
        }
    }
}