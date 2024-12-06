namespace Year2024;



public class Problem1
{
    public static void Solve()
    {
        string file = "2024/problem1/input.txt";
        List<List<int>> cols = File.ReadLines(file)
            .Select(line => line.GetNums())
            .Transpose()
            .Select((line) => line.FSort()).ToList();
        CountSet<int> cRight = new(cols[1]);
        (0..cols[0].Count)
            .Sum(i => Math.Abs(cols[0][i] - cols[1][i]))
            .WriteLine("part 1:");
        (0..cols[0].Count)
            .Sum(i => cols[0][i] * cRight[cols[0][i]])
            .WriteLine("part 2:");
    }

}