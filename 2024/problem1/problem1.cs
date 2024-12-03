namespace Year2024;



public class Problem1
{
    public static void Solve()
    {
        string file = "2024/problem1/input.txt";
        List<int> left = File.ReadLines(file).Select(line => line.GetNums())
            .Aggregate((List<int>)[], (l, n) => [.. l, n[0]]).FSort();
        List<int> right = File.ReadLines(file).Select(line => line.GetNums())
            .Aggregate((List<int>)[], (l, n) => [.. l, n[1]]).FSort();
        CountSet<int> dRight = new(right);

        (0..left.Count).Sum(i => Math.Abs(left[i] - right[i])).WriteLine("part 1:");
        (0..left.Count).Sum(i => left[i] * dRight[left[i]]).WriteLine("part 2:");
    }

}