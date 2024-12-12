namespace Year2021;

using Coord = (int X, int Y);

public class Problem9
{
    public static void Solve()
    {
        string file = "2021/problem9/input.txt";

        Grid<int> area = new(
            File.ReadAllLines(file)
                .Select(l => l.ToCharArray().ToList().Select(c => int.Parse("" + c)).ToList())
                .ToList(),
            9
        );

        // part 1
        int riskLevel = 0;
        List<Coord> lows = [];
        area.ForEach((pos, val) =>
        {
            if (area.GetNeighborValues(pos).All(v => v > val))
            {
                lows.Add(pos);
                riskLevel += val + 1;
            }
        });
        Console.WriteLine(riskLevel);

        // part 2
        List<SparseGrid<int>> basins = [];
        foreach (Coord lowPoint in lows)
        {
            SparseGrid<int> thisBasin = new(9);
            Queue<Coord> flood = new([lowPoint]);
            while (flood.TryDequeue(out Coord pos))
            {
                List<Coord> inBasin = thisBasin.GetNeighbors(pos);
                if (inBasin.Count == 0 || inBasin.Any(n => thisBasin.At(n) <= area.At(pos)))
                {
                    thisBasin.Set(pos, area.At(pos));
                    area.GetNeighbors(pos)
                        .Where(n => thisBasin.At(n) == 9 && area.At(n) != 9)
                        .ToList()
                        .ForEach(flood.Enqueue);
                }
            }
            basins.Add(thisBasin);
        }
        List<int> basinSizes = basins.Select(b => b.Count()).OrderByDescending(v => v).ToList();
        // Console.WriteLine(string.Join(", ", basinSizes));
        Console.WriteLine(basinSizes[0] * basinSizes[1] * basinSizes[2]);
    }

}