namespace Year2022;

using Vec = (int X, int Y);

public class Problem8
{
    public static void Solve()
    {
        string file = "2022/problem8/input.txt";

        // parse grid
        List<List<(int, bool)>> ints = [];
        File.ReadAllLines(file).ToList().ForEach(line =>
        {
            ints.Add([.. line.Select(c => (int.Parse("" + c), false))]);
        });
        Grid<(int, bool)> grid = new(ints, (-1, true));

        for (int i = 0; i < grid.Width; i++)
        {
            Scan(grid, (i, 0), (0, 1));
            Scan(grid, (i, grid.Height - 1), (0, -1));
            Scan(grid, (0, i), (1, 0));
            Scan(grid, (grid.Width - 1, i), (-1, 0));
        }

        // part 1
        int numVisible = 0;
        Grid<string> viz = Grid<string>.Initialize(grid.Width, grid.Height, ".");
        Grid<int> pt2 = Grid<int>.Initialize(grid.Width, grid.Height, -1);
        grid.ForEach((Vec pos, (int val, bool visible) item) =>
        {
            if (item.visible)
            {
                numVisible++;
                viz.Set(pos, "" + item.val);
            }
            pt2.Set(pos, item.val);
        });
        // viz.Print();
        Console.WriteLine(numVisible);

        // part 2
        int max = 0;
        for (int r = 1; r < grid.Height - 1; r++)
        {
            for (int c = 1; c < grid.Width - 1; c++)
            {
                int val = ScanFrom(pt2, (c, r));
                if (val > max) max = val;
            }
        }
        Console.WriteLine(max);

    }

    public static int ScanFrom(Grid<int> grid, Vec from)
    {
        int val = grid.At(from);
        List<int> ints = [0, 0, 0, 0];
        List<Vec> scanDirs = [(0, 1), (1, 0), (0, -1), (-1, 0)];
        List<Vec> scanLines = [(from.X, from.Y + 1), (from.X + 1, from.Y), (from.X, from.Y - 1), (from.X - 1, from.Y)];

        for (int i = 0; i < scanLines.Count; i++)
        {
            Vec line = scanLines[i];
            Vec dir = scanDirs[i];
            while (grid.At(line) < val && grid.At(line) != -1)
            {
                ints[i]++;
                line = (line.X + dir.X, line.Y + dir.Y);
            }
            if (grid.At(line) != -1) ints[i]++;
        }
        // Console.WriteLine(from + " (" + val + ") " + string.Join(", ", ints));
        return ints.Aggregate((x, y) => x * y);
    }

    public static void Scan(Grid<(int, bool)> grid, Vec start, Vec dir)
    {
        Vec curPos = (start.X, start.Y);
        int prev = -1;
        while (grid.At(curPos).Item1 != -1)
        {
            if (grid.At(curPos).Item1 > prev)
            {
                grid.Set(curPos, (grid.At(curPos).Item1, true));
                prev = grid.At(curPos).Item1;
            }

            curPos = (curPos.X + dir.X, curPos.Y + dir.Y);
        }
    }

}