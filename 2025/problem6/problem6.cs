namespace Year2025;

public class Problem6
{
    public static void Solve()
    {
        string file = "2025/problem6/input.txt";

        List<List<string>> elems = File.ReadAllLines(file)
            .Select(line => line.Split(' ')
                .Where(l => l != " " && l != "").ToList())
            .ToList();
        Grid<string> grid = new(elems, "-1");
        long part1 = 0;
        for (int c = 0; c < grid.Width; c++)
        {
            List<string> col = grid.GetCol(c);
            string opstr = col[^1];
            long total = opstr == "+" ? 0 : 1;
            for (int r = 0; r < col.Count - 1; r++)
            {
                long val = long.Parse(col[r]);
                total = opstr == "+" ? total + val : total * val;
            }
            part1 += total;
        }
        part1.WriteLine("Part 1:");

        Grid<char> grid2 = Grid<char>.CharsFromFile(file, ' ');
        long part2 = 0;
        char op = ' ';
        List<string> nums = [];
        for (int c = 0; c < grid2.Width; c++)
        {
            List<char> col = grid2.GetCol(c);
            if (op == ' ') op = col[^1];

            if (col.All(c => c == ' '))
            {
                part2 += nums.Select(long.Parse).Aggregate(
                    (a, b) => op == '+' ? a + b : a * b);
                op = ' ';
                nums = [];
                continue;
            }
            nums.Add(string.Join("",
                col.Where(c => c != ' ' && c != '+' && c != '*')));
        }
        part2 += nums.Select(long.Parse).Aggregate(
            (a, b) => op == '+' ? a + b : a * b);
        part2.WriteLine("Part 2:");
    }
}