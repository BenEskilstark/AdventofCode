namespace Year2021;

public class Problem12
{
    public static void Solve()
    {
        string file = "2021/problem12/input.txt";
        AdjacencyMatrix map = AdjacencyMatrix.FromFile([.. File.ReadAllLines(file)]);

        // part 1
        // Console.WriteLine(NumPaths(map));

        // part 2
        Console.WriteLine(NumPaths(map, true));
    }

    public static int NumPaths(AdjacencyMatrix map, bool isPartTwo = false)
    {
        List<CavePath> finishedPaths = [];
        Queue<CavePath> paths = [];
        paths.Enqueue(new(map, isPartTwo) { Path = ["start"] });
        while (paths.TryDequeue(out CavePath? path))
        {
            if (path.IsDone())
            {
                finishedPaths.Add(path);
                continue;
            }
            path.GetNextCaves().ForEach(cave =>
            {
                CavePath next = path.Copy();
                next.Add(cave);
                paths.Enqueue(next);
            });
        }

        // Console.WriteLine(string.Join("\n", finishedPaths));
        return finishedPaths.Count;
    }

}


public class CavePath(AdjacencyMatrix map, bool isPartTwo = false)
{
    public List<string> Path { get; set; } = ["start"];
    private AdjacencyMatrix Map = map;

    // for part 2
    private bool IsPartTwo = isPartTwo;
    private bool VisitedSmallTwice = false;

    public CavePath Copy()
    {
        return new CavePath(Map, IsPartTwo)
        {
            Path = Path.Select(s => s).ToList(),
            VisitedSmallTwice = VisitedSmallTwice
        };
    }

    public void Add(string cave)
    {
        if (IsPartTwo && char.IsLower(cave[0]) && Path.Contains(cave))
        {
            VisitedSmallTwice = true;
        }
        Path.Add(cave);
    }

    public bool IsDone()
    {
        return Path.Count > 0 && Path[^1] == "end";
    }


    public List<string> GetNextCaves()
    {
        List<string> caves = Map.Get(Path[^1]);
        if (IsPartTwo)
        {
            return caves
                .Where(c => char.IsUpper(c[0])
                    || (VisitedSmallTwice && !Path.Contains(c))
                    || (!VisitedSmallTwice && c != "start")
                ).ToList();
        }
        else
        {
            return caves
                .Where(c => char.IsUpper(c[0]) || !Path.Contains(c))
                .ToList();
        }

    }

    public override string ToString()
    {
        return string.Join(", ", Path);
    }

}


public class AdjacencyMatrix()
{
    private Dictionary<string, List<string>> Adjacencies { get; set; } = [];

    public static AdjacencyMatrix FromFile(List<string> input)
    {
        AdjacencyMatrix matrix = new();
        input.ForEach(edge =>
        {
            string[] nodes = edge.Split("-");
            matrix.AddEdge(nodes[0], nodes[1]);
            matrix.AddEdge(nodes[1], nodes[0]);
        });
        return matrix;
    }

    public void AddEdge(string from, string dest)
    {
        if (!Adjacencies.ContainsKey(from))
        {
            Adjacencies[from] = [];
        }
        Adjacencies[from].Add(dest);
    }

    public List<string> Get(string from)
    {
        return Adjacencies.GetValueOrDefault(from, []);
    }
}