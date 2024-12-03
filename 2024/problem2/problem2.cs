namespace Year2024;

public class Problem2
{
    public static void Solve()
    {
        string file = "2024/problem2/input.txt";
        // PART 1
        File.ReadLines(file)
            .Sum(line => IsSafe(line.GetNums()) ? 1 : 0)
            .WriteLine("part 1:");


        // PART 2
        File.ReadLines(file)
            .Select(line => line.GetNums())
            .Sum(n => IsSafe(n) || (0..n.Count).Any(i => IsSafe([.. n.Where((_, j) => j != i)])) ? 1 : 0)
            .WriteLine("part 2: ");
    }


    public static bool IsSafe(List<int> nums)
    {
        return !(1..nums.Count).ToList()
            .Any(i => !Safe(nums[i - 1], nums[i], nums[0] < nums[1]));
    }


    public static bool Safe(int num1, int num2, bool isIncreasing)
    {
        if (num1 == num2) return false;
        if (isIncreasing)
        {
            return num1 < num2 && num2 - num1 <= 3;
        }
        else
        {
            return num1 > num2 && num1 - num2 <= 3;
        }
    }

}