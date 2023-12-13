public class Problem13
{
    public static void Solve()
    {
        string file = "problem13/input.txt";
        long sum = 0;
        foreach (string pattern in File.ReadAllText(file).Split("\r\n\r\n"))
        {
            List<List<char>> grid = [];
            foreach (string line in pattern.Split("\r\n"))
            {
                grid.Add(line.ToCharArray().ToList());
            }

            int pivotPoint = GetReflection(grid);
            bool isVertical = pivotPoint > -1;

            if (!isVertical)
            {
                grid = new Grid<char>(grid, '?').Flip().Matrix;
                pivotPoint = GetReflection(grid);
                sum += (pivotPoint + 1) * 100;
            }
            else
            {
                sum += (pivotPoint + 1);
            }
            Console.WriteLine((isVertical ? "Vertical" : "Horizontal") + " " + pivotPoint);
        }
        Console.WriteLine(sum);
    }

    private static int GetReflection(List<List<char>> Grid)
    {
        for (int pivot = 0; pivot < Grid[0].Count - 1; pivot++)
        {
            if (Mirrors(Grid, pivot)) return pivot;
        }
        return -1;
    }

    private static bool Mirrors(List<List<char>> Grid, int Pivot)
    {
        // Console.WriteLine("~~~ " + Pivot + " ~~~");
        int numSmudges = 0;
        foreach (List<char> row in Grid)
        {
            for (int i = 0; i <= Pivot; i++)
            {
                if (i + Pivot + 1 >= row.Count) break;
                // Console.WriteLine((Pivot - i) + " " + row[Pivot - i] + " == " + (Pivot + i + 1) + " " + row[Pivot + i + 1]);
                if (row[Pivot - i] != row[Pivot + i + 1])
                {
                    numSmudges++;
                    if (numSmudges > 1) return false;
                }
            }
        }
        return numSmudges == 1;
    }

}