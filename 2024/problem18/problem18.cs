namespace Year2024;

using Coord = (int X, int Y);

public class Problem18
{
    public static void Solve()
    {
        string file = "2024/problem18/input.txt";
        List<Coord> obstacles = File.ReadLines(file)
            .Select(l => l.GetNums().ToTuple()).ToList();

        Console.WriteLine("Part 1: " + ShortestPath(obstacles, 1024));

        int i = 1025;
        for (; ShortestPath(obstacles, i) != null; i++) { }
        Console.WriteLine("Part 2: " + obstacles.ToList()[i - 1]);
    }

    public static int? ShortestPath(List<Coord> obstacles, int takeIndex)
    {
        int width = 71;
        int height = 71;
        Grid<char> grid = Grid<char>.Initialize(width, height, '.');
        for (int i = 0; i <= takeIndex; i++) grid.Set(obstacles[i], '#');

        Coord end = (width - 1, height - 1);
        Set<Coord> visited = new();
        PriorityQueue<Coord, int> paths = new([((0, 0), 0)]);
        while (paths.TryDequeue(out Coord pos, out int score))
        {
            if (pos == end) return score;
            if (visited[pos]) continue;
            grid.GetNeighbors(pos)
                .Where(n => grid.At(n) != '#' && !visited[n])
                .ForEach(n => paths.Enqueue(n, score + 1));
            visited.Add(pos);
        }
        return null;
    }
}