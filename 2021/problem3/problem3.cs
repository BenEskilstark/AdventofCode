namespace Year2021;

public class Problem3
{
    public static void Solve()
    {
        string file = "2021/problem3/input.txt";

        // part 1
        (List<int> onesCount, int numRows) = GetOnesCount([.. File.ReadAllLines(file)]);
        int gamma = 0;
        int epsilon = 0;
        for (int i = 0; i < onesCount.Count; i++)
        {
            if (onesCount[i] > numRows - onesCount[i])
            {
                gamma += (int)Math.Pow(2, onesCount.Count - i - 1);
            }
            else
            {
                epsilon += (int)Math.Pow(2, onesCount.Count - i - 1);
            }
        }
        Console.WriteLine(gamma + " " + epsilon + " " + (gamma * epsilon));

        // part 2
        List<string> oxygen = [.. File.ReadAllLines(file)];
        (onesCount, numRows) = GetOnesCount(oxygen);
        for (int i = 0; i < onesCount.Count; i++)
        {
            bool one = onesCount[i] > numRows - onesCount[i];
            bool eq = onesCount[i] == numRows - onesCount[i];
            oxygen = [.. oxygen.Where(line => (line[i] == '1' && (one || eq)) || (line[i] == '0' && !one && !eq))];
            if (oxygen.Count == 1) break;
            (onesCount, numRows) = GetOnesCount(oxygen);
        }
        int oNum = 0;
        for (int i = 0; i < oxygen[0].Length; i++)
        {
            if (oxygen[0][i] == '1') oNum += (int)Math.Pow(2, oxygen[0].Length - i - 1);
        }

        List<string> co2 = [.. File.ReadAllLines(file)];
        (onesCount, numRows) = GetOnesCount(co2);
        for (int i = 0; i < onesCount.Count; i++)
        {
            bool zero = onesCount[i] > numRows - onesCount[i];
            bool eq = onesCount[i] == numRows - onesCount[i];
            co2 = [.. co2.Where(line => (line[i] == '0' && (zero || eq)) || (line[i] == '1' && !zero && !eq))];
            if (co2.Count == 1) break;
            (onesCount, numRows) = GetOnesCount(co2);
        }
        int cNum = 0;
        for (int i = 0; i < co2[0].Length; i++)
        {
            if (co2[0][i] == '1') cNum += (int)Math.Pow(2, co2[0].Length - i - 1);
        }

        Console.WriteLine(cNum * oNum);

    }

    public static (List<int>, int) GetOnesCount(List<string> lines)
    {
        List<int> onesCount = [];
        int numRows = 0;
        lines.ForEach(line =>
        {
            numRows++;
            if (onesCount.Count == 0)
            {
                for (int i = 0; i < line.Length; i++) onesCount.Add(0);
            }
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '1') onesCount[i]++;
            }
        });

        return (onesCount, numRows);
    }

}