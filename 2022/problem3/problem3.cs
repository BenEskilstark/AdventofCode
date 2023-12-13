namespace Year2022;

public class Problem3
{
    public static void Solve()
    {
        string file = "2022/problem3/input.txt";

        int sum = 0;
        List<string> lines = File.ReadAllLines(file).ToList();
        for (int i = 0; i < lines.Count - 2; i += 3)
        {
            List<char> first = lines[i].ToCharArray().ToList();
            List<char> second = lines[i + 1].ToCharArray().ToList();
            List<char> third = [.. lines[i + 2].ToCharArray()];
            char inAllThree = '?';
            foreach (char c in first)
            {
                if (second.Contains(c) && third.Contains(c))
                {
                    inAllThree = c;
                    break;
                }
            }


            int res = 0;
            if (inAllThree >= 'a')
            {
                res += inAllThree - 'a' + 1;
            }
            else
            {
                res += inAllThree - 'A' + 27;
            }

            // Console.WriteLine(inBoth + " " + res);
            sum += res;

        }

        Console.WriteLine(sum);
    }
}