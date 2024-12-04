namespace Year2024;

using Coord = (int X, int Y);

public class Problem4
{
    public static void Solve()
    {
        string file = "2024/problem4/input.txt";
        Grid<char> grid = new([.. File.ReadLines(file).Select(l => l.ToChars())], '.');
        grid.Collect().Sum(pos => NumWords(grid, pos)).WriteLine("part 1:");
        grid.Collect((p, v) => IsXMas(grid, p)).Count.WriteLine("part 2:");
    }

    public static int NumWords(Grid<char> grid, Coord pos)
    {
        (int x, int y) = pos;
        List<char> ls = ['X', 'M', 'A', 'S'];
        int sum = 0;

        sum += (0..4).All(i => grid.At((x + i, y)) == ls[i]) ? 1 : 0;
        sum += (0..4).All(i => grid.At((x - i, y)) == ls[i]) ? 1 : 0;
        sum += (0..4).All(i => grid.At((x, y + i)) == ls[i]) ? 1 : 0;
        sum += (0..4).All(i => grid.At((x, y - i)) == ls[i]) ? 1 : 0;
        sum += (0..4).All(i => grid.At((x + i, y + i)) == ls[i]) ? 1 : 0;
        sum += (0..4).All(i => grid.At((x - i, y - i)) == ls[i]) ? 1 : 0;
        sum += (0..4).All(i => grid.At((x - i, y + i)) == ls[i]) ? 1 : 0;
        sum += (0..4).All(i => grid.At((x + i, y - i)) == ls[i]) ? 1 : 0;

        return sum;
    }

    public static bool IsXMas(Grid<char> grid, Coord pos)
    {
        (int x, int y) = pos;
        if (grid.At((x + 1, y + 1)) != 'A') return false;
        return (
            (grid.At((x, y)) == 'M' && grid.At((x + 2, y + 2)) == 'S') ||
            (grid.At((x, y)) == 'S' && grid.At((x + 2, y + 2)) == 'M')
        ) && (
            (grid.At((x, y + 2)) == 'M' && grid.At((x + 2, y)) == 'S') ||
            (grid.At((x, y + 2)) == 'S' && grid.At((x + 2, y)) == 'M')
        );
    }
}