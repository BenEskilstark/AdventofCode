using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Year2023;

using Coord = (int X, int Y);
using VecStep = (int X, int Y, int Steps);

public class Problem21
{
    public static void Solve()
    {
        string file = "2023/problem21/testinput.txt";
        List<List<char>> chars = [.. File.ReadAllLines(file).Select(line => line.ToCharArray().ToList())];
        Grid<char> grid = new(chars, '?');

        // Part1(grid);
        Part2(grid);
    }

    public static void Part2(Grid<char> grid)
    {
        long numSteps = 22;
        VecStep start = (grid.Width / 2, grid.Height / 2, 0);
        (long, long) square = ComputeSteps(grid, [start], grid.Width * grid.Height);
        Console.WriteLine(square.Item1 + " " + square.Item2);


        Console.WriteLine(GetInnerTotal([square.Item1, square.Item2], grid.Width, numSteps));
        Console.WriteLine(GetInnerTotal([square.Item1, square.Item2], grid.Width, numSteps) + GetOuterTotal(grid, numSteps));
    }

    public static long GetInnerTotal(List<long> square, int width, long numSteps)
    {
        long size = (numSteps / width);
        int evenOddIndex = (int)numSteps % 2;
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

        int edgeSteps = (int)((steps - grid.Width) + steps % grid.Width);
        edgeSteps = pointSteps;
        Console.WriteLine("edge steps: " + edgeSteps);
        (long, long) topRight = ComputeSteps(grid, [startTop, startRight], edgeSteps);
        (long, long) topLeft = ComputeSteps(grid, [startTop, startLeft], edgeSteps);
        (long, long) botRight = ComputeSteps(grid, [startBot, startRight], edgeSteps);
        (long, long) botLeft = ComputeSteps(grid, [startBot, startLeft], edgeSteps);

        long pointTotal = top.Item2 + bot.Item2 + left.Item2 + right.Item2;
        List<long> edgeTotals = [topRight.Item2, topLeft.Item2, botRight.Item2, botLeft.Item2];
        long m = (steps / grid.Width) - 1;
        Console.WriteLine("m: " + m);
        return pointTotal + m * edgeTotals[0] + m * edgeTotals[1] + m * edgeTotals[2] + m * edgeTotals[3];
    }

    public static (long, long) ComputeSteps(Grid<char> grid, List<VecStep> starts, int maxSteps)
    {
        long evenPlots = 0;
        long oddPlots = 0;
        Queue<VecStep> visiting = new(starts);
        HashSet<Coord> visited = new([.. starts.Select(s => (s.X, s.Y))]);
        while (visiting.TryDequeue(out VecStep step))
        {
            if (step.Steps % 2 == 0) evenPlots++;
            if (step.Steps % 2 == 1) oddPlots++;
            if (step.Steps > maxSteps) continue;
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