namespace Year2024;

using Coord = (int X, int Y);

public class Problem16
{
    public static void Solve()
    {
        Grid<char> maze = Grid<char>.CharsFromFile("2024/problem16/input.txt");
        Coord start = maze.Collect((pos, val) => val == 'S')[0];
        Coord end = maze.Collect((pos, val) => val == 'E')[0];

        // pre-process to find all the adjacencies:
        Dict<Coord, List<Coord>> nodes = new([], () => []);
        maze.ForEach((pos, val) =>
        {
            if (val == '#') return;
            List<Coord> ns = [.. maze.GetNeighbors(pos).Where(n => maze.At(n) != '#')];
            if (ns.Count > 2 || Contains90Deg(ns) || val == 'S' || val == 'E') nodes[pos] = [];
        });
        nodes.Keys.ToList().ForEach(pos =>
        {
            List<Coord> ns = [.. maze.GetNeighbors(pos).Where(n => maze.At(n) != '#')];
            ns.ForEach(n =>
            {
                Coord d = (n.X - pos.X, n.Y - pos.Y);
                Coord ray = (pos.X + d.X, pos.Y + d.Y);
                bool canGo = true;
                while (!nodes.ContainsKey(ray))
                {
                    if (maze.At(ray) == '#')
                    {
                        canGo = false;
                        break;
                    }
                    ray = (ray.X + d.X, ray.Y + d.Y);
                }
                if (canGo) nodes[pos].Add(ray);
            });
        });

        // Dijkstra's to find the shortest paths:
        PriorityQueue<Link, int> dfs = new();
        Dict<(Coord, Coord), int> visited = new(99999999);
        dfs.Enqueue(new Link(start, (1, 0), 0, null), 0);
        List<Link> allShortestPaths = [];
        int finalScore = 99999999;
        while (dfs.TryDequeue(out Link path, out int score))
        {
            if (path.Pos == end)
            {
                if (allShortestPaths.Count == 0) finalScore = score;
                if (score == finalScore) allShortestPaths.Add(path);
                continue;
            }
            if (path.Score >= finalScore) continue;
            nodes[path.Pos].ForEach(n =>
            {
                if (path.Prev != null && path.Prev.Pos == n) return; // don't go backwards
                Coord d = (n.X - path.Pos.X, n.Y - path.Pos.Y);
                int nextScore = path.Score;
                if ((d.X == 0 && path.Dir.X != 0) || (d.Y == 0 && path.Dir.Y != 0)) nextScore += 1000;
                nextScore += d.X != 0 ? Math.Abs(d.X) : Math.Abs(d.Y);
                if (visited[(n, d)] < nextScore) return;
                visited[(n, d)] = nextScore;
                dfs.Enqueue(new Link(n, d, nextScore, path), nextScore);
            });
        }
        finalScore.WriteLine("Part 1:");

        // go over the paths again, counting up edge lengths and nodes visited
        Set<(Coord, Coord)> edgesVisited = new();
        Set<Coord> nodesVisited = new();
        int total = 1; // must include start
        allShortestPaths.ForEach(path =>
        {
            Link? pointer = path;
            while (pointer.Prev != null)
            {
                if (edgesVisited[(pointer.Pos, pointer.Prev.Pos)])
                {
                    pointer = pointer.Prev;
                    continue;
                }
                Coord d = pointer.Dir;
                total += d.X != 0 ? Math.Abs(d.X) : Math.Abs(d.Y);
                if (nodesVisited[pointer.Pos]) total--;
                edgesVisited.Add((pointer.Pos, pointer.Prev.Pos));
                nodesVisited.Add(pointer.Pos);
                pointer = pointer.Prev;
            }
        });
        total.WriteLine("Part 2:");
    }


    public static bool Contains90Deg(List<Coord> coords)
    {
        for (int i = 0; i < coords.Count - 1; i++)
        {
            for (int j = i + 1; j < coords.Count; j++)
            {
                if (coords[j].X != coords[i].X && coords[j].Y != coords[i].Y) return true;
            }
        }
        return false;
    }
    private class Link(Coord pos, Coord dir, int score, Link? prev)
    {
        public Coord Pos { get; } = pos;
        public Coord Dir { get; } = dir;
        public int Score { get; } = score;
        public Link? Prev { get; set; } = prev;
    }
}

