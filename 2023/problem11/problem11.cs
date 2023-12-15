public class Problem11
{
    public static void Solve()
    {
        Grid<char> data = new Grid<char>(
            File.ReadAllLines("problem11/input.txt")
                .Select((line) => line.ToCharArray().ToList()).ToList(),
            '?'
        );
        // part 1:
        // data = ExpandRows(data);
        // data = ExpandRows(data.Flip()).Flip();

        List<int> emptyRows = GetEmptyRows(data);
        List<int> emptyCols = GetEmptyRows(data.Pivot());

        List<(int X, int Y)> galaxies = [];
        data.ForEach((pos, c) =>
        {
            if (c == '#') galaxies.Add(pos);
        });

        long sum = 0;
        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                sum += Dist(galaxies[i], galaxies[j], emptyRows, emptyCols, 999999);
            }
        }
        Console.WriteLine(sum);
    }

    public static long Dist(
        (int X, int Y) A, (int X, int Y) B,
        List<int> EmptyRows, List<int> EmptyCols, int Expansion)
    {
        int smallerX = Math.Min(A.X, B.X);
        int smallerY = Math.Min(A.Y, B.Y);
        int biggerX = Math.Max(A.X, B.X);
        int biggerY = Math.Max(A.Y, B.Y);
        long dist = (biggerX - smallerX) + (biggerY - smallerY);

        foreach (int rowIndex in EmptyRows)
        {
            if (rowIndex > smallerY && rowIndex < biggerY)
            {
                dist += Expansion;
            }
        }
        foreach (int colIndex in EmptyCols)
        {
            if (colIndex > smallerX && colIndex < biggerX)
            {
                dist += Expansion;
            }
        }

        return dist;
    }

    public static List<int> GetEmptyRows(Grid<char> grid)
    {
        List<int> emptyRows = [];
        for (int i = 0; i < grid.Matrix.Count; i++)
        {
            List<char> row = grid.Matrix[i];
            bool isEmpty = true;
            foreach (char c in row)
            {
                if (c != '.') isEmpty = false;
            }
            if (isEmpty) emptyRows.Add(i);
        }
        return emptyRows;
    }

    public static Grid<char> ExpandRows(Grid<char> grid)
    {
        List<List<char>> nextGrid = [];
        foreach (List<char> row in grid.Matrix)
        {
            bool isEmpty = true;
            foreach (char c in row)
            {
                if (c != '.') isEmpty = false;
            }
            nextGrid.Add(row);
            if (isEmpty) nextGrid.Add(new List<char>(row));
        }
        return new Grid<char>(nextGrid, grid.Default);
    }
}