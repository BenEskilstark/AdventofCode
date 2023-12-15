namespace Year2022;

public class Problem4
{
    public static void Solve()
    {
        string file = "2022/problem4/input.txt";
        int res = 0;
        foreach (string line in File.ReadLines(file))
        {
            List<string> rangeStrs = line.Split(',').ToList();
            List<int> range1 = rangeStrs[0].Split('-').Select(int.Parse).ToList();
            List<int> range2 = rangeStrs[1].Split('-').Select(int.Parse).ToList();

            if (
                (range1[0] <= range2[0] && range1[1] >= range2[0]) ||
                (range2[0] <= range1[0] && range2[1] >= range1[0]) ||
                (range1[1] >= range2[1] && range1[0] <= range2[1]) ||
                (range2[1] >= range1[1] && range2[0] <= range1[1])
            )
            {
                res++;
            }
        }

        Console.WriteLine(res);
    }
}