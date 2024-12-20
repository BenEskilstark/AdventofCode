namespace Year2024;

using Coord = (int X, int Y);

public class Problem20
{
    public static void Solve()
    {
        Grid<char> maze = Grid<char>.CharsFromFile("2024/problem20/input.txt");
        Coord start = maze.Collect((pos, val) => val == 'S')[0];
        Coord end = maze.Collect((pos, val) => val == 'E')[0];

        Link shortestPath = ShortestPath(maze, start, end);
        List<Link> links = shortestPath.ToArray();
        CountSet<int> saved1 = new();
        CountSet<int> saved2 = new();
        for (int i = 0; i < links.Count - 2; i++)
        {
            for (int j = i + 2; j < links.Count; j++)
            {
                int distSaved1 = links[i].Cheatable1(links[j]);
                if (distSaved1 > 99) saved1.Add(distSaved1);
                int distSaved2 = links[i].Cheatable2(links[j]);
                if (distSaved2 > 99) saved2.Add(distSaved2);
            }
        }
        saved1.Items.Sum(i => saved1[i]).WriteLine("Part 1:");
        saved2.Items.Sum(i => saved2[i]).WriteLine("Part 2:");
    }

    private static Link ShortestPath(Grid<char> maze, Coord start, Coord end)
    {
        PriorityQueue<Link, int> dfs = new([(new(start, null), 0)]);
        Set<Coord> visited = new();
        while (dfs.TryDequeue(out Link path, out int dist))
        {
            if (path.Pos == end) return path;
            if (visited[path.Pos]) continue;
            visited.Add(path.Pos);
            maze.GetNeighbors(path.Pos)
                .Where(n => !visited[n] && maze.At(n) != '#')
                .ForEach(n => dfs.Enqueue(new Link(n, path), dist + 1));
        }
        return new((0, 0), null); // only reached if there is no path at all
    }

    private class Link(Coord pos, Link? prev)
    {
        public Coord Pos { get; } = pos;
        public Link? Prev { get; } = prev;
        public int Length { get; set; } = prev == null ? 0 : prev.Length + 1;

        public int Cheatable1(Link other)
        {
            Coord d = (Math.Abs(Pos.X - other.Pos.X), Math.Abs(Pos.Y - other.Pos.Y));
            int saved = Length - other.Length - 2;
            if ((d.X == 2 && d.Y == 0) || (d.Y == 2 && d.X == 0)) return saved;
            return 0;
        }

        public int Cheatable2(Link other)
        {
            Coord d = (Math.Abs(Pos.X - other.Pos.X), Math.Abs(Pos.Y - other.Pos.Y));
            int saved = Length - other.Length - (d.X + d.Y);
            if (saved > 0 && d.X + d.Y <= 20) return saved;
            return 0;
        }

        public List<Link> ToArray()
        {
            List<Link> res = [];
            Link? pointer = this;
            for (; pointer != null; res.Add(pointer), pointer = pointer.Prev) { }
            return res;
        }

        public void PrintPath(Grid<char> m)
        {
            Grid<char> maze = m.Copy();
            Link? pointer = this;
            for (; pointer != null; pointer = pointer.Prev)
            {
                maze.Set(pointer.Pos, 'O');
            }
            Console.WriteLine(maze);
        }
    }

}