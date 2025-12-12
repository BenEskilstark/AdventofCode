namespace Year2025;

using Coord = (long X, long Y);

public class Problem9
{
    public static void Solve()
    {
        string file = "2025/problem9/testinput66.txt";
        List<Coord> tiles = File.ReadAllLines(file)
            .Select(l => (l.GetLongs()[0], l.GetLongs()[1]))
            .ToList();

        tiles.Pairwise()
            .Max(tuple => GetArea(tuple.Item1, tuple.Item2))
            .WriteLine("Part 1:");

        Dict<long, List<Coord>> ys = new([], () => []);
        Dict<long, List<Coord>> xs = new([], () => []);
        tiles.ForEach(tile =>
        {
            xs[tile.X].Add(tile);
            ys[tile.Y].Add(tile);
        });

        List<Line> lines = [];
        Coord min = tiles.OrderBy(Magnitude).First();
        Coord max = tiles.OrderByDescending(Magnitude).First();
        Console.WriteLine($"Min: {min}, Max: {max}");

        // SparseGrid<string> grid = new(" ");
        // tiles.ForEach(t =>
        // {
        //     grid.Set(((int)t.X / 50, (int)t.Y / 50), "#");
        //     // grid.Set(((int)t.X, (int)t.Y), "#");
        // });

        Coord cur = min;
        Coord? prev = null;
        Dict<Coord, Coord> dirs = new((0, 0));
        Set<Coord> visited = new();
        Coord curDir = (1, -1);
        dirs[min] = curDir;
        bool useX = true;
        while (visited.Count < tiles.Count)
        {
            Coord next = useX
                ? ClosestTo(cur, xs[cur.X], visited)
                : ClosestTo(cur, ys[cur.Y], visited);
            useX = !useX;
            Line nextLine = new(cur, next);
            lines.Add(nextLine);
            // nextLine.PointsOnLine().ForEach(c => grid.Set(((int)c.X / 50, (int)c.Y / 50), "@"));
            visited.Add(next);
            prev = cur; cur = next;
        }

        // grid.ToGrid().Save("2025/problem9/viz.txt");

        tiles.Pairwise()
            .Where(tuple =>
            {
                (long x1, long y1) = tuple.Item1;
                (long x2, long y2) = tuple.Item2;

                Line line1 = new((x1, y1), (x1, y2));
                Line line2 = new((x1, y2), (x2, y2));
                Line line3 = new((x2, y2), (x2, y1));
                Line line4 = new((x2, y1), (x1, y1));
                // Console.WriteLine($"Checking rectangle {line1}, {line2}, {line3}, {line4}");
                return !lines.Any(l => l.Crosses(line1)) &&
                    !lines.Any(l => l.Crosses(line2)) &&
                    !lines.Any(l => l.Crosses(line3)) &&
                    !lines.Any(l => l.Crosses(line4));
            })
            .OrderByDescending(tuple => GetArea(tuple.Item1, tuple.Item2))
            .First(t =>
            {
                Console.WriteLine(t.Item1);
                Console.WriteLine(t.Item2);
                Console.WriteLine(GetArea(t.Item1, t.Item2));
                return true;
            });
        // .WriteLine("Part 2:");

        // Line vert5 = new Line((0, 0), (0, 5));
        // Line vert2 = new Line((0, 2), (0, 3));
        // Line horz3 = new Line((-1, 1), (1, 1));
        // Console.WriteLine(vert5.Crosses(vert2)); // false
        // Console.WriteLine(vert5.Crosses(horz3)); // true
        // Console.WriteLine(horz3.Crosses(vert5)); // true
        // Console.WriteLine(vert2.Crosses(horz3)); // false
        // Console.WriteLine(horz3.Crosses(vert2)); // false

    }

    public static Coord ClosestTo(Coord a, List<Coord> bs, Set<Coord> exclude)
    {
        Coord min = a;
        long minDist = long.MaxValue;
        foreach (Coord b in bs)
        {
            if (exclude[b]) continue;
            long dist = Distance(a, b);
            if (dist < minDist && dist > 0)
            {
                minDist = Distance(a, b);
                min = b;
            }
        }
        return min;
    }

    public static long Distance(Coord a, Coord b)
    {
        return (a.X - b.X) * (a.X - b.X) +
            (a.Y - b.Y) * (a.Y - b.Y);
    }

    public static long Magnitude(Coord a)
    {
        return a.X * a.X + a.Y * a.Y;
    }

    public static long GetArea(Coord a, Coord b)
    {
        return (1 + Math.Abs(a.X - b.X)) * (1 + Math.Abs(a.Y - b.Y));
    }

    private class Line
    {
        public Coord A { get; }
        public Coord B { get; }

        public Line(Coord a, Coord b)
        {
            A = a; B = b;
            if (a.X == b.X) // force smaller value into A
            {
                if (a.Y > b.Y) { A = b; B = a; }
            }
            else
            {
                if (a.X > b.X) { A = b; B = a; }
            }
        }

        public bool Crosses(Coord c, Coord d)
        {
            return Crosses(new(c, d));
        }

        public bool Crosses(Line other)
        {
            if (A.X == B.X && other.A.X == other.B.X)
            {
                // if (other.A.X == A.X)
                // {
                //     if (other.A.Y == A.Y && other.B.Y > B.Y) return true;
                // }
                return false;
            }
            if (A.Y == B.Y && other.A.Y == other.B.Y)
            {
                // if (other.A.Y == A.Y)
                // {
                //     if (other.A.X == A.X && other.B.X > B.X) return true;
                // }
                return false;
            }
            // return A.Y == other.A.Y && (A.X > other.B.X || B.X < other.A.X);
            if (A.X == B.X && other.A.X < A.X && other.B.X > A.X)
                return other.A.Y > A.Y && other.A.Y < B.Y;
            if (A.Y == B.Y && other.A.Y < A.Y && other.B.Y > A.Y)
                return other.A.X > A.X && other.A.X < B.X;
            return false;
        }

        public override string ToString()
        {
            return $"{A}-{B}";
        }

        public List<Coord> PointsOnLine()
        {
            List<Coord> points = [];
            if (A.X == B.X)
            {
                for (long y = A.Y; y <= B.Y; y++)
                {
                    points.Add((A.X, y));
                }
            }
            else if (A.Y == B.Y)
            {
                for (long x = A.X; x <= B.X; x++)
                {
                    points.Add((x, A.Y));
                }
            }
            return points;
        }
    }

}