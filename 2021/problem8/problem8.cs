namespace Year2021;

public class Problem8
{
    public static void Solve()
    {
        string file = "2021/problem8/input.txt";

        // part 1
        List<List<string>> outputs = File.ReadAllLines(file)
            .Select(l => l.Split(" | ")[1].Split(" ").ToList())
            .ToList();

        int numUniqueSegments = 0;
        foreach (List<string> output in outputs)
        {
            foreach (string digit in output)
            {
                if (digit.Length == 2 || digit.Length == 3 ||
                    digit.Length == 4 || digit.Length == 7
                )
                {
                    numUniqueSegments++;
                }
            }
        }

        Console.WriteLine(numUniqueSegments);
    }

}