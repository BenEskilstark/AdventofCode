namespace Year2024;

using Coord = (int X, int Y);

public class Problem12
{
    public static void Solve()
    {
        Grid<char> grid = Grid<char>.CharsFromFile("2024/problem12/input.txt");
        List<SparseGrid<char>> regions = [];
        grid.ForEach((pos, val) =>
        {
            if (regions.Any(r => r.Contains(pos))) return; // already added to a region

            SparseGrid<char> region = new();
            Stack<Coord> stack = new([pos]);
            while (stack.TryPop(out Coord next))
            {
                region.Set(next, val);
                grid.GetNeighbors(next).ForEach(n =>
                {
                    if (grid.At(n) == val && !region.Contains(n)) stack.Push(n);
                });
            }
            regions.Add(region);
        });
        regions.Sum(r => r.Count() * GetPerimeter(r)).WriteLine("Part 1:");
        regions.Sum(r => r.Count() * GetSides(r)).WriteLine("Part 2:");
    }


    public static int GetSides(SparseGrid<char> region)
    {
        (Coord min, Coord max) = region.Bounds();
        int numSides = 0;
        for (int x = min.X; x <= max.X + 1; x++)
        {
            bool OnASide = false;
            for (int i = min.Y; i <= max.Y + 1; i++)
            {
                if (OnASide && region.At((x, i)) != region.At((x, i - 1))) OnASide = false;

                if (region.At((x, i)) != region.At((x - 1, i)))
                {
                    if (!OnASide) numSides++;
                    OnASide = true;
                }
                else
                {
                    OnASide = false;
                }
            }
        }

        for (int y = min.Y; y <= max.Y + 1; y++)
        {
            bool OnASide = false;
            for (int i = min.X; i <= max.X + 1; i++)
            {
                if (OnASide && region.At((i, y)) != region.At((i - 1, y))) OnASide = false;
                if (region.At((i, y)) != region.At((i, y - 1)))
                {
                    if (!OnASide) numSides++;
                    OnASide = true;
                }
                else
                {
                    OnASide = false;
                }
            }
        }
        return numSides;
    }


    public static int GetPerimeter(SparseGrid<char> region)
    {
        return region.Sum((pos, val) =>
        {
            return region.GetAllNeighbors(pos)
                .Where(n => region.At(n) != val).Count();
        });
    }
}