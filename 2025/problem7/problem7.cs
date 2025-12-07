namespace Year2025;

using QPath = (char Char, long NumPaths);

public class Problem7
{
    public static void Solve()
    {
        string file = "2025/problem7/input.txt";
        Grid<char> grid = Grid<char>.CharsFromFile(file);

        int numSplits = 0;
        for (int y = 0; y < grid.Height - 1; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                char c = grid.At((x, y));
                if (c == '|' || c == 'S')
                {
                    if (grid.At((x, y + 1)) == '^')
                    {
                        numSplits++;
                        grid.Set((x - 1, y + 1), '|');
                        grid.Set((x + 1, y + 1), '|');
                    }
                    else
                    {
                        grid.Set((x, y + 1), '|');
                    }
                }
            }
        }
        numSplits.WriteLine("Part 1:");

        grid = Grid<char>.CharsFromFile(file);
        SparseGrid<QPath> grid2 = new(() => ('.', 0));
        grid.ForEach((pos, c) =>
        {
            if (c == '^') grid2.Set(pos, (c, 0));
            if (c == 'S') grid2.Set(pos, (c, 1));
        });

        for (int y = 0; y < grid.Height - 1; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                (char c, long n) = grid2.At((x, y));
                if (c == '|' || c == 'S')
                {
                    if (grid2.At((x, y + 1)).Char == '^')
                    {
                        grid2.Set((x - 1, y + 1), ('|', grid2.At((x - 1, y + 1)).NumPaths + n));
                        grid2.Set((x + 1, y + 1), ('|', grid2.At((x + 1, y + 1)).NumPaths + n));
                    }
                    else
                    {
                        grid2.Set((x, y + 1), ('|', grid2.At((x, y + 1)).NumPaths + n));
                    }
                }
            }
        }
        (0..grid.Width).ToEnumerable()
            .Sum(x => grid2.At((x, grid.Height - 1)).NumPaths)
            .WriteLine("Part 2:");
    }

}