namespace Year2025;

public class Problem3
{
    public static void Solve()
    {
        string file = "2025/problem3/input.txt";
        List<List<int>> banks = File.ReadAllLines(file)
            .Select(l => l.ToChars().Select(c => int.Parse(c.ToString())).ToList())
            .ToList();

        banks.Aggregate(0, (sum, bank) =>
        {
            int index1 = 0;
            int digit1 = 0;
            for (int i = 0; i < bank.Count - 1; i++)
            {
                if (bank[i] > digit1)
                {
                    index1 = i;
                    digit1 = bank[i];
                }
            }
            int digit2 = 0;
            for (int j = index1 + 1; j < bank.Count; j++)
            {
                if (bank[j] > digit2)
                {
                    digit2 = bank[j];
                }
            }
            return sum + digit1 * 10 + digit2;
        }).WriteLine("Part 1:");

        banks.Aggregate((long)0, (sum, bank) =>
        {
            List<long> digits = [];
            int index = -1;
            for (int d = 11; d >= 0; d--)
            {
                long max = 0;
                for (int i = index + 1; i < bank.Count - d; i++)
                {
                    if (bank[i] > max)
                    {
                        max = bank[i];
                        index = i;
                    }
                }
                digits.Add((long)max);
            }
            // digits.WriteLine();
            return sum + ToLong(digits);
        })
        .WriteLine("Part 2:");
    }

    public static long ToLong(List<long> nums)
    {
        long total = 0;
        long multiple = 1;
        for (int i = nums.Count - 1; i >= 0; i--)
        {
            total += nums[i] * multiple;
            multiple *= 10;
        }
        return total;
    }
}