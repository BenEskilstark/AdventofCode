namespace Year2024;

public class Problem19
{
    public static void Solve()
    {
        string file = "2024/problem19/input.txt";
        List<string> towels = [.. File.ReadAllText(file).Split("\r\n\r\n")[0].Split(", ")];
        List<string> patterns = [.. File.ReadAllText(file).Split("\r\n\r\n")[1].Split("\r\n")];

        List<string> possiblePatterns = patterns.Where(pattern =>
        {
            Set<string> visited = new();
            Stack<string> attempts = new(towels);
            while (attempts.TryPop(out string sofar))
            {
                if (sofar == pattern) return true;
                if (sofar.Length > pattern.Length) continue;
                if (!pattern.StartsWith(sofar)) continue;
                if (visited[sofar]) continue;
                visited.Add(sofar);
                towels.ForEach(t => attempts.Push(sofar + t));
            }
            return false;
        }).ToList();
        possiblePatterns.Count.WriteLine("Part 1:");

        possiblePatterns
            .Sum(new Memo(towels).GetNumCombos)
            .WriteLine("Part 2:");
    }



    private class Memo(List<string> towels)
    {
        public List<string> Towels { get; } = towels;
        public Dict<string, long> ToEnd { get; set; } = new(0);

        public long GetNumCombos(string pattern)
        {
            if (pattern == "") return 1;
            if (ToEnd[pattern] > 0) return ToEnd[pattern];
            long res = Towels
                .Where(pattern.StartsWith)
                .Sum(t => GetNumCombos(pattern[t.Length..]));
            ToEnd[pattern] += res;
            return res;
        }
    }

}