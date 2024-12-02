namespace Year2024;

public class Problem2
{
    public static void Solve()
    {
        string file = "2024/problem2/input.txt";

        // PART 1
        int numSafe = 0;
        File.ReadLines(file).ToList().ForEach(line =>
        {
            List<int> nums = [.. line.Split(' ').Select(int.Parse)];
            if (ReportIsSafe(nums)) numSafe++;
        });

        Console.WriteLine(numSafe);


        // PART 2
        numSafe = 0;
        File.ReadLines(file).ToList().ForEach(line =>
        {
            List<int> nums = [.. line.Split(' ').Select(int.Parse)];
            if (ReportIsSafe(nums) ||
                Enumerable.Range(0, nums.Count)
                    .Any(i => ReportIsSafe([.. nums.Where((_, j) => j != i)]))
            )
            {
                numSafe++;
            }
        });

        Console.WriteLine(numSafe);

    }


    public static bool ReportIsSafe(List<int> nums)
    {
        for (int i = 1; i < nums.Count; i++)
        {
            if (!IsSafe(nums[i - 1], nums[i], nums[0] < nums[1])) return false;
        }
        return true;
    }


    public static bool IsSafe(int num1, int num2, bool isIncreasing)
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