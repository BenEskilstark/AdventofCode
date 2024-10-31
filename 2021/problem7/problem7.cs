namespace Year2021;

public class Problem7
{
    public static void Solve()
    {
        string file = "2021/problem7/input.txt";
        List<int> positions = File.ReadAllLines(file)[0].Split(',').Select(int.Parse).ToList();
        positions.Sort();

        // part 1
        int index = positions.Count / 2;
        int minSum = SumOfDistances(positions, positions[index]);
        int deltaI = 1;
        while (minSum == SumOfDistances(positions, positions[index + deltaI]))
        {
            index += deltaI;
        }
        if (minSum < SumOfDistances(positions, positions[index + deltaI]))
        {
            deltaI = -1;
        }
        while (minSum >= SumOfDistances(positions, positions[index + deltaI]))
        {
            minSum = SumOfDistances(positions, positions[index + deltaI]);
            index += deltaI;
        }
        minSum = SumOfDistances(positions, positions[index - deltaI]);
        Console.WriteLine(positions[index - deltaI] + " " + minSum);


        // part 2
        int value = positions[positions.Count / 2];
        minSum = SumOfDistances2(positions, positions[index]);
        int delta = 1;
        while (minSum == SumOfDistances2(positions, value))
        {
            value += delta;
        }
        if (minSum < SumOfDistances2(positions, value))
        {
            delta = -1;
        }
        while (minSum >= SumOfDistances2(positions, value))
        {
            minSum = SumOfDistances2(positions, value);
            value += delta;
        }
        minSum = SumOfDistances2(positions, value - delta);
        Console.WriteLine((value - delta) + " " + minSum);
    }

    public static int SumOfDistances(List<int> positions, int pivot)
    {
        int sum = 0;
        foreach (int pos in positions)
        {
            sum += Math.Abs(pos - pivot);
        }
        return sum;
    }

    public static int SumOfDistances2(List<int> positions, int pivot)
    {
        int sum = 0;
        foreach (int pos in positions)
        {
            sum += TriangleNum(Math.Abs(pos - pivot));
        }
        return sum;
    }

    private static int TriangleNum(int n)
    {
        return n * (n + 1) / 2;
    }

}