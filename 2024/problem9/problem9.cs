namespace Year2024;

using Block = (int ID, int Size);

public class Problem9
{
    public static void Solve()
    {
        string file = "2024/problem9/input.txt";
        List<int> exp = Expand([.. File.ReadAllText(file).Select(c => c - '0')]);
        // exp.Select(i => i == -1 ? "." : "" + i).Aggregate((s, i) => s += i).WriteLine();
        ulong checksum = 0;

        int index = 0;
        for (int i = exp.Count - 1; i > 0; i--)
        {
            if (exp[i] == -1) continue;
            bool flag = false;
            for (; flag == false && index < i; index++)
            {
                if (exp[index] == -1)
                {
                    flag = true;
                    exp[index] = exp[i];
                    exp[i] = -1;
                }
            }
            if (!flag) break;
        }
        // exp.Select(i => i == -1 ? "." : "" + i).Aggregate((s, i) => s += i).WriteLine();

        for (int i = 0; i < exp.Count; i++)
        {
            if (exp[i] == -1) break;
            checksum += (ulong)(i * exp[i]);
        }
        Console.WriteLine("Part 1: " + checksum);

        checksum = 0;
        exp = Expand([.. File.ReadAllText(file).Select(c => c - '0')]);
        // exp.Select(i => i == -1 ? "." : "" + i).Aggregate((s, i) => s += i).WriteLine();
        for (int i = exp.Count - 1; i > 0; i--)
        {
            if (exp[i] == -1) continue;
            int si = i;
            for (; si > 0 && exp[si] == exp[i]; si--) { }
            bool flag = false;
            for (int j = 0; j < i; j++)
            {
                if (exp[j] != -1) continue;
                int ej = j;
                for (; ej < i && exp[ej] == exp[j]; ej++) { }
                if (i - si <= ej - j)
                {
                    flag = true;
                    for (int k = 0; k < i - si; k++)
                    {
                        exp[j + k] = exp[i - k];
                        exp[i - k] = -1;
                    }
                    break;
                }
            }
            if (!flag)
            {
                i = si + 1;
            }
        }
        // exp.Select(i => i == -1 ? "." : "" + i).Aggregate((s, i) => s += i).WriteLine();

        for (int i = 0; i < exp.Count; i++)
        {
            if (exp[i] == -1) continue;
            checksum += (ulong)(i * exp[i]);
        }
        Console.WriteLine("Part 2: " + checksum);
    }

    public static List<int> Expand(List<int> nums)
    {
        List<int> expansion = [];
        for (int i = 0; i < nums.Count; i++)
        {
            for (int j = 0; j < nums[i]; j++)
            {
                if (i % 2 == 0)
                {
                    expansion.Add(i / 2);
                }
                else
                {
                    expansion.Add(-1);
                }
            }
        }
        return expansion;
    }
}