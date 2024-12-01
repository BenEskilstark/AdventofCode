namespace Year2024;

using System.Text.RegularExpressions;

public class Problem1
{
    public static void Solve()
    {
        string file = "2024/problem1/input.txt";
        List<int> left = [];
        DefaultDict<int, int> dRight = new(0);
        foreach (string line in File.ReadAllLines(file))
        {
            int l = int.Parse(line.Split("   ")[0]);
            int r = int.Parse(line.Split("   ")[1]);
            left.Add(l);
            dRight[r]++;
        }

        long dsum = 0;
        for (int i = 0; i < left.Count; i++)
        {
            dsum += left[i] * dRight[left[i]];
        }

        Console.WriteLine(dsum);
    }

}