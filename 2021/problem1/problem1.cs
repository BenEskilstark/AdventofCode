namespace Year2021;

public class Problem1
{
    public static void Solve()
    {
        string file = "2021/problem1/testinput.txt";
        List<int> nums = File.ReadAllLines(file).Select(int.Parse).ToList();

        int numLarger = 0;
        for (int i = 0; i < nums.Count - 3; i++)
        {
            // if (nums[i] > nums[i - 1]) numLarger++;
            if (nums[i] < nums[i + 3]) numLarger++;
        }
        Console.WriteLine(numLarger);
    }

}