namespace Year2021;

using Coord = (int X, int Y);

public class Problem15
{
    public static void Solve()
    {
        string file = "2021/problem15/input.txt";

        List<List<int>> nums = File
            .ReadLines(file)
            .Select(l => l.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToList())
            .ToList();

        // part 1
        Grid<int> riskLevels = new(nums, -1);
        Console.WriteLine(MinPath(riskLevels));

        // part 2
        Grid<int> biggerGrid = Grid<int>.Initialize(riskLevels.Width * 5, riskLevels.Height * 5, 0);
        for (int tx = 0; tx < 5; tx++)
        {
            for (int ty = 0; ty < 5; ty++)
            {
                riskLevels.ForEach((pos, val) =>
                {
                    biggerGrid.Set(
                        (X: tx * riskLevels.Width + pos.X,
                        Y: ty * riskLevels.Height + pos.Y),
                        (val + tx + ty) > 9 ? val + tx + ty - 9 : val + tx + ty
                    );
                });
            }
        }
        Console.WriteLine(MinPath(biggerGrid));
    }

    public static int MinPath(Grid<int> riskLevels)
    {
        PriorityQueue<Coord, int> queue = new();
        Coord bottomRight = (X: riskLevels.Width - 1, Y: riskLevels.Height - 1);
        queue.Enqueue(bottomRight, riskLevels.At(bottomRight));

        HashSet<Coord> visited = [bottomRight];
        int result = -1;
        while (queue.TryDequeue(out var coord, out int riskLevel))
        {
            if (coord.X == 0 && coord.Y == 0)
            {
                result = riskLevel - riskLevels.At(coord); // don't include val of start
                break;
            }

            riskLevels.GetNeighbors(coord)
                .Where(n => !visited.Contains(n)).ToList()
                .ForEach(n =>
                {
                    queue.Enqueue(n, riskLevels.At(n) + riskLevel);
                    visited.Add(n);
                });
        }
        return result;
    }

}