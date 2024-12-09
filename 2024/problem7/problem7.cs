namespace Year2024;

using PartialEval = (long Part, int Index);
using PartialEquation = (string Part, int Index);
using System.Text.RegularExpressions;

public class Problem7
{
    public static void Solve()
    {
        Part1();
        Part2();
    }

    public static void Part2()
    {
        string file = "2024/problem7/input.txt";

        List<List<long>> cals = [.. File.ReadLines(file).Select(l => l.GetLongs())];
        List<char> ops = ['+', '*', '|'];

        long total = 0;
        int index = 0;
        cals.ForEach(cal =>
        {
            long res = cal[0];
            List<long> nums = cal[1..];
            Stack<PartialEquation> dfs = new();
            Set<(string, int)> seen = new();
            dfs.Push((nums[0].ToString(), 1));
            while (dfs.TryPop(out PartialEquation part))
            {
                if (seen[(res.ToString(), index)]) return;

                if (part.Index >= nums.Count)
                {
                    if (part.Part == res.ToString())
                    {
                        seen.Add((res.ToString(), index));
                        total += res;
                    }
                    continue;
                };
                foreach (char op in ops)
                {
                    string nextPartial = op switch
                    {
                        '+' => (long.Parse(part.Part) + nums[part.Index]).ToString(),
                        '*' => (long.Parse(part.Part) * nums[part.Index]).ToString(),
                        '|' => part.Part.ToString() + nums[part.Index].ToString()
                    };
                    PartialEquation next = (nextPartial, part.Index + 1);
                    dfs.Push(next);
                }
            }
            index++;
        });
        Console.WriteLine("Part 2: " + total);
    }

    public static void Part1()
    {
        string file = "2024/problem7/input.txt";

        List<List<long>> cals = [.. File.ReadLines(file).Select(l => l.GetLongs())];
        List<char> ops = ['+', '*'];

        long total = 0;
        int index = 0;
        cals.ForEach(cal =>
        {
            long res = cal[0];
            List<long> nums = cal[1..];
            long partialResult = nums[0];
            Stack<PartialEval> dfs = new();
            Set<(long, int)> seen = new();
            dfs.Push((nums[0], 1));
            while (dfs.TryPop(out PartialEval part))
            {
                if (seen[(res, index)]) return;

                if (part.Index >= nums.Count)
                {
                    if (part.Part == res)
                    {
                        seen.Add((res, index));
                        total += res;
                        // nums.WriteLine();
                    }
                    continue;
                };
                foreach (char op in ops)
                {
                    long nextPartial = op switch
                    {
                        '+' => part.Part + nums[part.Index],
                        '*' => part.Part * nums[part.Index],
                    };
                    PartialEval next = (nextPartial, part.Index + 1);
                    dfs.Push(next);
                }
            }
            index++;
        });
        Console.WriteLine("Part 1: " + total);
    }

}