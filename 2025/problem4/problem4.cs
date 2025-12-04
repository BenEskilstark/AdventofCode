namespace Year2025;

using Coord = (int X, int Y);

public class Problem4
{
    public static void Solve()
    {
        string file = "2025/problem4/input.txt";
        Grid<char> grid = Grid<char>.CharsFromFile(file);

        int total = RemoveAccessibleRolls(grid);
        total.WriteLine("Part 1:");
        for (int num = total; num > 0; total += num)
        {
            num = RemoveAccessibleRolls(grid);
        }
        total.WriteLine("Part 2");
    }

    public static int RemoveAccessibleRolls(Grid<char> grid)
    {
        int numAccessibleRolls = 0;

        grid.ForEach((pos, c) =>
        {
            if (c == '.') return;
            int numNeighbors = grid.GetNeighborValues(pos, true /* diagonals */)
                .Where(v => v == '@' || v == '*')
                .Count();
            if (numNeighbors < 4)
            {
                numAccessibleRolls++;
                grid.Set(pos, '*'); // set to placeholder to switch to . after
            }
        });

        grid.ForEach((pos, c) =>
        {
            if (c == '*') grid.Set(pos, '.');
        });
        return numAccessibleRolls;
    }
}