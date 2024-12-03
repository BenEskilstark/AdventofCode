namespace Year2024;



public class Problem1
{
    public static void Solve()
    {
        string file = "2024/problem1/input.txt";
        // Aggregating each of left and right columns separately:
        List<int> left = File.ReadLines(file).Select(line => line.GetNums())
            .Aggregate((List<int>)[], (l, n) => [.. l, n[0]]).FSort();
        List<int> right = File.ReadLines(file).Select(line => line.GetNums())
            .Aggregate((List<int>)[], (l, n) => [.. l, n[1]]).FSort();
        CountSet<int> dRight = new(right);
        (0..left.Count).Sum(i => Math.Abs(left[i] - right[i])).WriteLine("part 1:");
        (0..left.Count).Sum(i => left[i] * dRight[left[i]]).WriteLine("part 2:");

        // Using Transpose extension method:
        List<List<int>> cols = File.ReadLines(file).Select(line => line.GetNums())
            .Transpose().Select((line) => line.FSort()).ToList();
        CountSet<int> cRight = new(cols[1]);
        (0..cols[0].Count).Sum(i => Math.Abs(cols[0][i] - cols[1][i])).WriteLine("part 1:");
        (0..cols[0].Count).Sum(i => cols[0][i] * cRight[cols[0][i]]).WriteLine("part 2:");
    }

}