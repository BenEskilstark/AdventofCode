namespace Year2023;

using Coord = (int X, int Y);

public class Problem23
{
    public static void Solve()
    {
        string file = "2023/problem23/input.txt";
        Grid<char> grid = new([.. File.ReadAllLines(file).Select(l => l.ToCharArray().ToList())], '?');
        int periods = 0;
        grid.ForEach((p, c) =>
        {
            if (c == '.') periods++;
        });
        Console.WriteLine("num periods: " + periods);

        Coord start = (1, 0);
        Coord goal = (grid.Width - 2, grid.Height - 1);

        HashSet<Coord> splits = [(start), (goal)];
        List<Node> finishedPaths = [];
        var startNode = new Node(start);
        Dictionary<Coord, Node> visited = new() { [start] = startNode };
        PriorityQueue<Node, int> heads = new();
        heads.Enqueue(startNode, 0);
        while (heads.TryDequeue(out Node? node, out int priority))
        {
            if (node.Pos == goal)
            {
                finishedPaths.Add(node);
                // Console.WriteLine(node.GetScore());
                continue;
            }

            List<Coord> neighbors = node.GetNeighbors().Where(pos => grid.At(pos) != '#').ToList();
            if (neighbors.Count > 2)
            {
                splits.Add(node.Pos);
            }
            neighbors
                .Where(pos => pos.Y >= 0 && pos != node.Prev?.Pos)
                .Where(pos =>
                {
                    if (pos.X < node.Pos.X && grid.At(pos) == '>') return false;
                    if (pos.X > node.Pos.X && grid.At(pos) == '<') return false;
                    if (pos.Y < node.Pos.Y && grid.At(pos) == 'v') return false;
                    if (pos.Y > node.Pos.Y && grid.At(pos) == '^') return false;
                    return true;
                })
                // .Select(p => new Node(p))
                .ToList()
                .ForEach(p =>
                {
                    var n = new Node(p);
                    n.Prev = node;
                    n.Path = [.. node.Path, n.Pos];

                    if (!node.InPath(n.Pos))
                    {
                        heads.Enqueue(n, -1 * n.GetScore());
                        visited[n.Pos] = n;
                    }
                });
        }

        // Console.WriteLine(string.Join(',', finishedPaths));

        foreach (Coord p in splits)
        {
            grid.Set(p, 'O');
        }
        grid.Print();
        Console.WriteLine("num intersections " + splits.Count);

        // construct compressed graph
        Dictionary<(Coord, Coord), int> newGraph = [];
        foreach (Coord splitA in splits)
        {
            foreach (Coord splitB in splits)
            {
                if (splitA == splitB) continue;
                if (newGraph.ContainsKey((splitA, splitB))) continue;
                int score = ShortestPath(grid, splits, splitA, splitB) - 1;
                newGraph[(splitA, splitB)] = score;
                newGraph[(splitB, splitA)] = score;
            }
        }

        Dictionary<(Coord, Coord), int> weights = [];
        Dictionary<Coord, List<Coord>> graph = [];
        int totalWeights = 0;
        foreach (var pair in newGraph)
        {
            if (newGraph[pair.Key] <= 0) continue;
            weights[pair.Key] = pair.Value;
            totalWeights += pair.Value;
            graph.TryAdd(pair.Key.Item1, []);
            graph[pair.Key.Item1].Add(pair.Key.Item2);
        }
        // Console.WriteLine(string.Join('\n', graph.ToList()));
        // Console.WriteLine("num edges: " + weights.Count);
        // Console.WriteLine(string.Join('\n', weights.ToList()));
        // Console.WriteLine("total weights: " + totalWeights);


        // find longest again
        finishedPaths = [];
        startNode = new Node(start);
        visited = new() { [start] = startNode };
        heads = new();
        heads.Enqueue(startNode, 0);
        int max = 0;
        while (heads.TryDequeue(out Node? node, out int priority))
        {
            if (node.Pos == goal)
            {
                finishedPaths.Add(node);

                int score = 0;
                List<Node> path = node.GetPath().ToList();
                for (int i = 0; i < path.Count - 1; i++)
                {
                    score += weights[(path[i].Pos, path[i + 1].Pos)];
                }
                if (score > max)
                {
                    max = score;
                }
                if (finishedPaths.Count % 100000 == 0)
                {
                    Console.WriteLine(finishedPaths.Count + " " + score + " " + max);
                }

                continue;
            }

            graph[node.Pos]
                .Where(pos => pos.Y >= 0 && pos != node.Prev?.Pos)
                // .Select(p => new Node(p))
                .ToList()
                .ForEach(p =>
                {
                    var n = new Node(p);
                    n.Prev = node;
                    n.Path = [.. node.Path, n.Pos];

                    if (!node.InPath(n.Pos))
                    {
                        int score = 0;
                        List<Node> path = n.GetPath().ToList();
                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            score += weights[(path[i].Pos, path[i + 1].Pos)];
                        }
                        heads.Enqueue(n, -1 * score);
                        visited[n.Pos] = n;
                    }
                });
        }

        // Node maxP = finishedPaths[0];
        // foreach (Node p in finishedPaths)
        // {
        //     int score = 0;
        //     List<Node> path = p.GetPath().ToList();
        //     for (int i = 0; i < path.Count - 1; i++)
        //     {
        //         score += weights[(path[i].Pos, path[i + 1].Pos)];
        //     }
        //     if (score > max)
        //     {
        //         max = score;
        //         maxP = p;
        //     }

        //     // Grid<char> copyG = grid.Copy();
        //     // path.GetPath().ToList().ForEach(n => copyG.Set(n.Pos, 'O'));
        //     // copyG.Print();
        // }
        // List<Node> maxPath = maxP.GetPath().ToList();
        // for (int i = 0; i < maxPath.Count - 1; i++)
        // {
        //     grid.Set(maxPath[i].Pos, (char)('0' + i));
        // }
        // grid.Print();

        Console.WriteLine("------");
        Console.WriteLine(max);


    }

    public static int ShortestPath(Grid<char> grid, HashSet<Coord> splits, Coord start, Coord goal)
    {
        var startNode = new Node(start);
        Dictionary<Coord, Node> visited = new() { [start] = startNode };
        PriorityQueue<Node, int> heads = new();
        heads.Enqueue(startNode, 0);
        while (heads.TryDequeue(out Node? node, out int priority))
        {
            if (node.Pos == goal) return node.GetScore();

            node.GetNeighbors()
                .Where(pos => grid.At(pos) != '#' && pos.Y >= 0 && pos.Y < grid.Height && pos != node.Prev?.Pos)
                .Where(pos => goal == pos || !splits.Contains(pos))
                .ToList()
                .ForEach(p =>
                {
                    var n = new Node(p);
                    n.Prev = node;
                    n.Path = [.. node.Path, n.Pos];

                    if (!visited.ContainsKey(n.Pos))
                    {
                        heads.Enqueue(n, n.GetScore());
                        visited[n.Pos] = n;
                    }
                });
        }
        return -1;
    }

}

public class Node(Coord pos)
{
    public Coord Pos { get; } = pos;
    public HashSet<Coord> Path = [pos];
    public Node? Prev { get; set; }
    public List<Coord> GetNeighbors()
    {
        return [
            (this.Pos.X + 1, this.Pos.Y),
            (this.Pos.X - 1, this.Pos.Y),
            (this.Pos.X, this.Pos.Y + 1),
            (this.Pos.X, this.Pos.Y - 1)
        ];
    }

    public override string ToString()
    {
        return "" + this.Pos;
    }

    public bool InPath(Coord pos)
    {
        return this.Path.Contains(pos);
    }

    public int GetScore()
    {
        return this.Path.Count;
    }

    public IEnumerable<Node> GetPath()
    {
        Node? ptr = this;
        while (ptr != null)
        {
            yield return ptr;
            ptr = ptr.Prev;
        }
    }
}
