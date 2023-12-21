using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Year2023;

using Coord = (int X, int Y);
using VecStep = (int X, int Y, int Steps);

public class Problem21
{
    public static void Solve()
    {
        string file = "2023/problem21/input.txt";
        List<List<char>> chars = [.. File.ReadAllLines(file).Select(line => line.ToCharArray().ToList())];
        Grid<char> grid = new(chars, '?');

        // Part1(grid);
        Part2(grid);
    }

    public static void Part2(Grid<char> grid)
    {
        // Dictionary<int, int> expected = new() { [11] = 103, [16] = 216, [22] = 387, [27] = 588, [33] = 853, [38] = 1142, [44] = 1501, [49] = 1878 };
        Dictionary<int, int> expected = new() { [26501365] = 0 };
        foreach (var pair in expected)
        {
            Console.WriteLine("---------");
            long numSteps = pair.Key;
            Console.WriteLine("Num Steps: " + numSteps);
            VecStep start = (grid.Width / 2, grid.Height / 2, 0);
            (long, long) square = ComputeSteps(grid, [start], grid.Width * grid.Height);
            Console.WriteLine(square.Item1 + " " + square.Item2);

            // Console.WriteLine("inner square: " + GetInnerTotal([square.Item1, square.Item2], grid.Width, numSteps));
            long res = GetInnerTotal([square.Item1, square.Item2], grid.Width, numSteps) + GetOuterTotal(grid, numSteps);
            Console.WriteLine("got: " + res + " expected: " + pair.Value);
        }

    }

    public static long GetInnerTotal(List<long> square, int width, long numSteps)
    {
        long size = (numSteps / width);
        int evenOddIndex = (int)size % 2;
        if (numSteps % width != 0)
        {
            evenOddIndex = (int)(size + 1) % 2;
        }
        Console.WriteLine("even or odd center? " + evenOddIndex);
        long total = square[evenOddIndex];
        for (int s = 1; s <= size; s++)
        {
            total += square[evenOddIndex] * ((s - 1) * 4);
            evenOddIndex = (evenOddIndex + 1) % 2;
        }

        return total;
    }

    public static long GetOuterTotal(Grid<char> grid, long steps)
    {
        int pointSteps = (int)steps % grid.Width + grid.Width / 2;
        Console.WriteLine("pointsteps: " + pointSteps);
        VecStep startTop = (grid.Width / 2, grid.Height - 1, 0);
        (long, long) top = ComputeSteps(grid, [startTop], pointSteps);
        VecStep startBot = (grid.Width / 2, 0, 0);
        (long, long) bot = ComputeSteps(grid, [startBot], pointSteps);
        VecStep startLeft = (grid.Width - 1, grid.Height / 2, 0);
        (long, long) left = ComputeSteps(grid, [startLeft], pointSteps);
        VecStep startRight = (0, grid.Height / 2, 0);
        (long, long) right = ComputeSteps(grid, [startRight], pointSteps);

        Console.WriteLine("top point: " + top);
        Console.WriteLine("bottom point: " + bot);
        Console.WriteLine("left point: " + left);
        Console.WriteLine("right point: " + right);

        int edgeSteps = pointSteps;
        Console.WriteLine("edge steps: " + edgeSteps);
        (long, long) topRight = ComputeSteps(grid, [startTop, startRight], edgeSteps);
        (long, long) topLeft = ComputeSteps(grid, [startTop, startLeft], edgeSteps);
        (long, long) botRight = ComputeSteps(grid, [startBot, startRight], edgeSteps);
        (long, long) botLeft = ComputeSteps(grid, [startBot, startLeft], edgeSteps);

        // Console.WriteLine("bottom left edge: " + botLeft);

        int cornerSteps = (int)((steps - grid.Width) % grid.Width);
        Console.WriteLine("cornerSteps: " + cornerSteps);
        (long, long) cTopRight = ComputeSteps(grid, [(grid.Width - 1, grid.Height - 1, 0)], cornerSteps);
        (long, long) cTopLeft = ComputeSteps(grid, [(0, grid.Height - 1, 0)], cornerSteps);
        (long, long) cBotRight = ComputeSteps(grid, [(0, 0, 0)], cornerSteps);
        (long, long) cBotLeft = ComputeSteps(grid, [(grid.Width - 1, 0, 0)], cornerSteps);

        Console.WriteLine("Corner top right: " + cTopRight);
        Console.WriteLine("Corner top left: " + cTopLeft);
        Console.WriteLine("Corner bottom right: " + cBotRight);
        Console.WriteLine("Corner bottom left: " + cBotLeft);

        long pointTotal = top.Item1 + bot.Item1 + left.Item1 + right.Item1;
        if (pointSteps % 2 == 1)
        {
            pointTotal = top.Item2 + bot.Item2 + left.Item2 + right.Item2;
        }
        long edgeTotal = new List<long>([topRight.Item1, topLeft.Item1, botRight.Item1, botLeft.Item1]).Aggregate((a, b) => a + b);
        if (edgeSteps % 2 == 1)
        {
            edgeTotal = new List<long>([topRight.Item2, topLeft.Item2, botRight.Item2, botLeft.Item2]).Aggregate((a, b) => a + b);
        }
        long cornerTotal = new List<long>([cTopRight.Item1, cTopLeft.Item1, cBotRight.Item1, cBotLeft.Item1]).Aggregate((a, b) => a + b);

        long m = (steps / grid.Width) - 1;
        long n = m + 1;
        Console.WriteLine("Corner Total: " + cornerTotal);
        Console.WriteLine("m: " + m);
        Console.WriteLine(pointTotal + " " + (m * edgeTotal) + " " + n * cornerTotal);
        return pointTotal + m * edgeTotal + n * cornerTotal;
    }

    public static (long, long) ComputeSteps(Grid<char> grid, List<VecStep> starts, int maxSteps)
    {
        if (maxSteps <= 1) return (0, 0); // HACK
        long evenPlots = 0;
        long oddPlots = 0;
        Queue<VecStep> visiting = new(starts);
        HashSet<Coord> visited = new([.. starts.Select(s => (s.X, s.Y))]);
        while (visiting.TryDequeue(out VecStep step))
        {
            if (step.Steps % 2 == 0) evenPlots++;
            if (step.Steps % 2 == 1) oddPlots++;
            if (step.Steps >= maxSteps) continue;
            List<VecStep> neighbors = grid.GetNeighbors((step.X, step.Y))
                .Where(p => grid.At(p) != '#' && !visited.Contains(p))
                .Select(p => (p.X, p.Y, step.Steps + 1))
                .ToList();
            neighbors.ForEach(s => visited.Add((s.X, s.Y)));
            neighbors.ForEach(s => visiting.Enqueue(s));
        }
        return (evenPlots, oddPlots);
    }






    public static void Part2Brute(Grid<char> grid, int maxSteps)
    {
        VecStep start = (grid.Width / 2, grid.Height / 2, 0);
        long totalPlots = 0;
        Queue<VecStep> visiting = new([start]);
        HashSet<Coord> visited = new([(start.X, start.Y)]);
        while (visiting.TryDequeue(out VecStep step))
        {
            if (step.Steps % 2 == maxSteps % 2) totalPlots++;
            if (step.Steps > maxSteps) continue;
            List<VecStep> neighbors = GetNeighbors((step.X, step.Y))
                .Where(p => ModAt(grid, p) != '#' && !visited.Contains(p))
                .Select(p => (p.X, p.Y, step.Steps + 1))
                .ToList();
            neighbors.ForEach(s => visited.Add((s.X, s.Y)));
            neighbors.ForEach(s => visiting.Enqueue(s));
        }

        Console.WriteLine(totalPlots);
    }

    public static char ModAt(Grid<char> grid, Coord pos)
    {
        int xToConsider = pos.X;
        while (xToConsider < 0) xToConsider += grid.Width;
        int yToConsider = pos.Y;
        while (yToConsider < 0) yToConsider += grid.Height;
        return grid.At((
            X: xToConsider % grid.Width,
            Y: yToConsider % grid.Height
        ));
    }

    public static List<Coord> GetNeighbors(Coord coord)
    {
        return new List<Coord>([
            (coord.X + 1, coord.Y),
            (coord.X - 1, coord.Y),
            (coord.X, coord.Y + 1),
            (coord.X, coord.Y - 1)
        ]);
    }


    public static void Part1(Grid<char> grid)
    {
        VecStep start = (grid.Width / 2, grid.Height / 2, 0);
        int maxSteps = 64;
        long totalPlots = 0;
        Queue<VecStep> visiting = new([start]);
        HashSet<Coord> visited = new([(start.X, start.Y)]);
        while (visiting.TryDequeue(out VecStep step))
        {
            if (step.Steps % 2 == maxSteps % 2) totalPlots++;
            if (step.Steps > maxSteps) continue;
            List<VecStep> neighbors = grid.GetNeighbors((step.X, step.Y))
                .Where(p => grid.At(p) != '#' && !visited.Contains(p))
                .Select(p => (p.X, p.Y, step.Steps + 1))
                .ToList();
            neighbors.ForEach(s => visited.Add((s.X, s.Y)));
            neighbors.ForEach(s => visiting.Enqueue(s));
        }

        Console.WriteLine(totalPlots);
    }

}