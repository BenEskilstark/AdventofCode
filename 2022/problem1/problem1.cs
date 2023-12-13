namespace Year2022;

public class Problem1
{
    public static void Solve()
    {
        string file = "2022/problem1/input.txt";
        List<int> calories = File.ReadAllText(file).Split("\r\n\r\n")
            .Select(elf => elf.Split("\r\n").Select(int.Parse).Aggregate((a, b) => a + b))
            .ToList();

        calories.Sort();
        calories.Reverse();
        // Utils.PrintList(calories);
        Console.WriteLine(calories[0] + calories[1] + calories[2]);
    }
}