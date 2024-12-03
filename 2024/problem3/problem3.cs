namespace Year2024;

using System.Text.RegularExpressions;

public class Problem3
{
    public static void Solve()
    {
        string file = File.ReadAllText("2024/problem3/input.txt");
        // PART 1
        Regex.Matches(file, @"mul\(\d{1,3},\d{1,3}\)")
            .Sum(m => m.Value.GetNums().Aggregate(1, (p, i) => p * i))
            .WriteLine("part 1:");

        // PART 2
        bool enabled = true;
        Regex.Matches(file, @"mul\(\d{1,3},\d{1,3}\)|do(n't)?\(\)")
            .Sum(match =>
            {
                if (match.Value == "do()") enabled = true;
                if (match.Value == "don't()") enabled = false;
                List<int> nums = match.Value.GetNums();
                return enabled && nums.Any() ? nums[0] * nums[1] : 0;
            }).WriteLine("part 2: ");
    }

}