using System.Text.RegularExpressions;

namespace Year2025;

public class Problem2
{
    public static void Solve()
    {
        string file = "2025/problem2/input.txt";
        List<IEnumerable<long>> ranges = File.ReadAllText(file)
            .Split(',').ToList()
            .Select(r =>
            {
                List<long> bounds = r.GetLongs();
                return LongRange(bounds[0], bounds[1] - bounds[0]);
            })
            .ToList();

        long sum1 = 0, sum2 = 0;
        ranges.ForEach(range =>
        {
            range.ForEach(val =>
            {
                if (!IsValid1(val.ToString())) sum1 += val;
                if (!IsValid2(val.ToString())) sum2 += val;
            });
        });

        sum1.WriteLine("Part 1:");
        sum2.WriteLine("Part 2:");
    }

    public static bool IsValid1(string num)
    {
        if (num.Length % 2 == 1) return true;
        for (int i = 0; i < num.Length / 2; i++)
        {
            if (num[i] != num[num.Length / 2 + i]) return true;
        }
        return false;
    }

    public static bool IsValid2(string num)
    {

        for (int p = 1; p <= num.Length / 2; p++) // patterns: 1s, pairs, triplets...
        {
            if (num.Length % p != 0) continue;
            bool isInvalid = true;
            for (int j = 1; j < num.Length / p; j++) // j is each occurrence of the pattern
            {
                for (int i = 0; i < p; i++) // index i within the pattern
                {
                    if (num[i] != num[p * j + i]) isInvalid = false;
                }
            }
            if (isInvalid) return false;
        }
        return true;

        // of course all of that is captured by this regex:
        // return !Regex.IsMatch(num, @"^(\d+)\1+$");
    }

    public static IEnumerable<long> LongRange(long start, long count)
    {
        long limit = start + count;
        while (start <= limit)
        {
            yield return start;
            start++;
        }
    }

}