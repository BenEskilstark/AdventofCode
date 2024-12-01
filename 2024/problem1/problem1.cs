namespace Year2024;

using System.Text.RegularExpressions;

public class Problem1
{
    public static void Solve()
    {
        string file = "2024/problem1/input.txt";
        List<int> left = [];
        Dictionary<int, int> right = [];
        foreach (string line in File.ReadAllLines(file))
        {
            int l = int.Parse(line.Split("   ")[0]);
            int r = int.Parse(line.Split("   ")[1]);
            left.Add(l);
            if (!right.TryAdd(r, 1)) right[r]++;
        }

        long sum = 0;
        for (int i = 0; i < left.Count; i++)
        {
            if (right.TryGetValue(left[i], out int num))
            {
                sum += num * left[i];
            }
        }


        Console.WriteLine(sum);
    }

}