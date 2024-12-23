namespace Year2024;

using LAN = (string C1, string C2, string C3);

public class Problem23
{
    public static void Solve()
    {
        string file = "2024/problem23/input.txt";
        Dict<string, Set<string>> adj = new(new(), () => new());
        Set<string> startsWithT = new();
        File.ReadLines(file).Select(line => line.Split('-'))
            .ForEach(pair =>
            {
                if (pair[0][0] == 't') startsWithT.Add(pair[0]);
                if (pair[1][0] == 't') startsWithT.Add(pair[1]);
                adj[pair[0]].Add(pair[1]);
                adj[pair[1]].Add(pair[0]);
            });

        // Part 1:
        Set<LAN> lans = new();
        startsWithT.ForEach(com =>
        {
            Stack<List<string>> findLANs = new([[com]]);
            while (findLANs.TryPop(out var lan))
            {
                if (lan.Count == 3)
                {
                    lan.Sort();
                    lans.Add((lan[0], lan[1], lan[2]));
                    continue;
                }
                adj[lan[^1]].Items
                    .Where(c => lan.All(l => adj[l][c]))
                    .ForEach(c => findLANs.Push([.. lan, c]));
            }
        });
        // lans.ForEach(l => Console.WriteLine(l));
        lans.Count.WriteLine("Part 1:");

        List<string> maxLanComs = [];
        adj.Keys.ForEach(com =>
        {
            Set<string> visited = new();
            Stack<List<string>> findLANs = new([[com]]);
            while (findLANs.TryPop(out var lan))
            {
                if (visited[lan[^1]]) continue;
                if (lan.Count > maxLanComs.Count) maxLanComs = lan;
                visited.Add(lan[^1]);
                adj[lan[^1]].Items
                    .Where(c => !visited[c] && lan.All(l => adj[l][c]))
                    .ForEach(c => findLANs.Push([.. lan, c]));
            }
        });
        maxLanComs.Sort();
        Console.WriteLine("Part 2: " + string.Join(",", maxLanComs));
    }

}