namespace Year2021;

using Coord = (int X, int Y);

public class Problem13
{
    public static void Solve()
    {
        string file = "2021/problem13/input.txt";

        SparseGrid<char> grid = new('.');
        List<Coord> folds = [];
        bool inCoords = true;
        foreach (string line in File.ReadAllLines(file))
        {
            if (inCoords && line == "")
            {
                inCoords = false;
                continue;
            }
            if (inCoords)
            {
                List<int> points = line.Split(',').Select(int.Parse).ToList();
                grid.Set((points[0], points[1]), '#');
            }
            else
            {
                List<string> parts = line.Split("=").ToList();
                Coord fold = parts[0][^1] == 'x'
                    ? (X: int.Parse(parts[1]), Y: 0)
                    : (X: 0, Y: int.Parse(parts[1]));
                folds.Add(fold);
            }
        }

        // part 1
        // grid = grid.Fold(folds[0]);
        // Console.WriteLine(grid.GetNumValues());

        // part 2
        foreach (Coord fold in folds)
        {
            grid = grid.Fold(fold);
        }
        grid.ToGrid().Print();
    }



}

public static class SparseGridExtensionProblem13
{
    public static SparseGrid<T> Fold<T>(this SparseGrid<T> grid, Coord axis)
    {
        SparseGrid<T> newGrid = new(grid.Default);
        grid.ForEach((pos, val) =>
        {
            Coord newPos = pos;
            if (axis.X > 0 && pos.X > axis.X)
            {
                newPos.X = axis.X - (pos.X - axis.X);
            }
            else if (axis.Y > 0 && pos.Y > axis.Y)
            {
                newPos.Y = axis.Y - (pos.Y - axis.Y);
            }
            newGrid.Set(newPos, val);
        });
        return newGrid;
    }
}