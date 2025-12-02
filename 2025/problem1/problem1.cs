namespace Year2025;

public class Problem1
{
    public static void Solve()
    {
        string file = "2025/problem1/input.txt";
        List<int> rotations = File.ReadLines(file)
            .Select(line => line.Replace('L', '-'))
            .Select(line => line.Replace('R', ' ').Trim())
            .Select(int.Parse)
            .ToList();

        int dialPointer = 50;
        int numTimesAtZero = 0;
        rotations.ForEach(rot =>
        {
            dialPointer = (100 + dialPointer + rot) % 100;
            if (dialPointer == 0) numTimesAtZero++;
        });
        numTimesAtZero.WriteLine("Part 1:");

        dialPointer = 50;
        numTimesAtZero = 0;
        rotations.ForEach(rot =>
        {
            int click = rot < 0 ? -1 : 1;
            for (int i = 0; i < Math.Abs(rot); i++)
            {
                dialPointer += click;
                if (dialPointer > 99) dialPointer = 0;
                if (dialPointer < 0) dialPointer = 99;
                if (dialPointer == 0) numTimesAtZero++;
            }
        });
        numTimesAtZero.WriteLine("Part 2:");
    }

}