namespace Year2024;

using Coord = (int X, int Y);

public class Problem8
{
    public static void Solve()
    {
        string file = "2024/problem8/input.txt";
        Grid<char> grid = Grid<char>.FromFile(file, '-');
        List<Coord> antennas = grid.Collect((pos, val) => val != '.' && val != '#');
        Dict<char, List<Coord>> sortedAntennas = new([], () => []);
        antennas.ForEach(pos => sortedAntennas[grid.At(pos)].Add(pos));
        Set<Coord> antinodes = new();
        sortedAntennas.Keys.ToList()
            .ForEach(antenna =>
            {
                List<Coord> coords = sortedAntennas[antenna];
                // Console.WriteLine(antenna + ": " + string.Join("; ", coords));
                for (int i = 0; i < coords.Count - 1; i++)
                {
                    for (int j = 0; j < coords.Count; j++)
                    {
                        (int x1, int y1) = coords[i];
                        (int x2, int y2) = coords[j];
                        Coord diff = (x1 - x2, y1 - y2);
                        Coord dist1 = (x1 + diff.X, y1 + diff.Y);
                        Coord dist2 = (x2 - diff.X, y2 - diff.Y);
                        if (dist1 != coords[i] && dist1 != coords[j])
                        {
                            antinodes.Add(dist1);
                        }
                        if (dist2 != coords[i] && dist2 != coords[j])
                        {
                            antinodes.Add(dist2);
                        }
                    }
                }
            });
        antinodes.ToList().Where(p => grid.At(p) != '-').Count().WriteLine("Part 1: ");


        antinodes = new();
        sortedAntennas.Keys.ToList()
            .ForEach(antenna =>
            {
                List<Coord> coords = sortedAntennas[antenna];
                // Console.WriteLine(antenna + ": " + string.Join("; ", coords));
                for (int i = 0; i < coords.Count - 1; i++)
                {
                    for (int j = 0; j < coords.Count; j++)
                    {
                        (int x1, int y1) = coords[i];
                        (int x2, int y2) = coords[j];
                        Coord diff = (x1 - x2, y1 - y2);
                        int div = GreatestCommonDivisor(diff.X, diff.Y);
                        if (div != 0) diff.X /= div;
                        if (div != 0) diff.Y /= div;
                        Coord dist1 = (x1 + diff.X, y1 + diff.Y);
                        Coord dist2 = (x2 - diff.X, y2 - diff.Y);
                        while (grid.At(dist1) != '-' || grid.At(dist2) != '-')
                        {
                            // if (antinodes[dist1] && antinodes[dist2]) break;
                            if (grid.At(dist1) != '-')
                            {
                                antinodes.Add(dist1);
                            }
                            if (grid.At(dist2) != '-')
                            {
                                antinodes.Add(dist2);
                            }
                            dist1 = (dist1.X + diff.X, dist1.Y + diff.Y);
                            dist2 = (dist2.X - diff.X, dist2.Y - diff.Y);
                            if (diff.X == 0 && diff.Y == 0) break;
                        }
                        antinodes.Add(coords[i]);
                        antinodes.Add(coords[j]);
                    }
                }
            });
        antinodes.ToList().Where(p => grid.At(p) != '-').Count().WriteLine("Part 2: ");

        // Grid<char> res = Grid<char>.Initialize(grid.Width, grid.Height, '.');
        // antinodes.ToList().ForEach(coord => res.Set(coord, '#'));
        // Console.WriteLine(res);


    }

    static int LeastCommonDenominator(int a, int b)
    {
        return a * b / GreatestCommonDivisor(a, b);
    }

    static int GreatestCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

}