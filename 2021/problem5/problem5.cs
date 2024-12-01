namespace Year2021;

using Coord = (int X, int Y);

public class Problem5
{
    public static void Solve()
    {
        string file = "2021/problem5/input.txt";

        List<Line> lines = File.ReadAllLines(file).Select(Line.Parse).ToList();
        SparseGrid<int> grid = new(0);

        lines.ForEach(line =>
        {
            // if (line.IsVertical() || line.IsHorizontal())
            // { part 1
            line.GetPoints().ForEach(p => grid.Set(p, grid.At(p) + 1));
            // }
        });

        // grid.ToGrid().Print();

        int numOverlaps = 0;
        grid.ForEach((c, v) => numOverlaps += v >= 2 ? 1 : 0);

        Console.WriteLine(numOverlaps);
    }

}

public class Line(Coord _start, Coord _end)
{
    public Coord Start = _start;
    public Coord End = _end;

    public static Line Parse(string lineStr)
    {
        string[] parts = lineStr.Split(" -> ");
        Coord s = (X: int.Parse(parts[0].Split(',')[0]), Y: int.Parse(parts[0].Split(',')[1]));
        Coord e = (X: int.Parse(parts[1].Split(',')[0]), Y: int.Parse(parts[1].Split(',')[1]));
        return new Line(s, e);
    }

    public static (Coord, Coord) Bounds(List<Line> lines)
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = 0;
        int maxY = 0;
        lines.ForEach(line =>
        {
            if (line.Start.X < minX) minX = line.Start.X;
            if (line.Start.Y < minY) minY = line.Start.Y;
            if (line.Start.X > maxX) maxX = line.Start.X;
            if (line.Start.Y > maxY) maxY = line.Start.Y;
            if (line.End.X < minX) minX = line.End.X;
            if (line.End.Y < minY) minY = line.End.Y;
            if (line.End.X > maxX) maxX = line.End.X;
            if (line.End.Y > maxY) maxY = line.End.Y;
        });
        return ((X: minX, Y: minY), (X: maxX, y: maxY));
    }

    public bool IsHorizontal()
    {
        return Start.Y == End.Y;
    }
    public bool IsVertical()
    {
        return Start.X == End.X;
    }

    public List<Coord> GetPoints()
    {
        List<Coord> points = [];
        int deltaX = Start.X < End.X ? 1 : -1;
        int deltaY = Start.Y < End.Y ? 1 : -1;
        if (Start.X == End.X) deltaX = 0;
        if (Start.Y == End.Y) deltaY = 0;
        Coord pos = (Start.X, Start.Y);
        while (pos != End)
        {
            points.Add((pos.X, pos.Y));
            pos.X += deltaX;
            pos.Y += deltaY;
        }
        points.Add((pos.X, pos.Y));

        return points;
    }
}