namespace Year2024;

using Trail = (int X, int Y, int StartX, int StartY);

public class Problem10
{
    public static void Solve()
    {
        Grid<int> map = Grid<int>.IntsFromFile("2024/problem10/input.txt");
        Stack<Trail> potentialTrails = [];
        map.Collect((pos, val) => val == 0)
            .ForEach(t => potentialTrails.Push((t.X, t.Y, t.X, t.Y)));
        CountSet<Trail> finishedTrails = new();
        while (potentialTrails.TryPop(out Trail trail))
        {
            if (map.At((trail.X, trail.Y)) == 9) finishedTrails.Add(trail);
            map.GetNeighbors((trail.X, trail.Y))
                .Where(n => map.At(n) == map.At((trail.X, trail.Y)) + 1).ToList()
                .ForEach(n => potentialTrails.Push((n.X, n.Y, trail.StartX, trail.StartY)));
        }
        finishedTrails.Count.WriteLine("Part 1");
        finishedTrails.Items.Sum(t => finishedTrails[t]).WriteLine("Part 2:");
    }
}