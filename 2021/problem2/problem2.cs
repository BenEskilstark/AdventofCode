namespace Year2021;

using Coord = (int X, int Y);
using Sub = (int X, int Y, int A);

public class Problem2
{
    public static void Solve()
    {
        string file = "2021/problem2/input.txt";

        // part 1
        Coord sub = (0, 0);
        File.ReadAllLines(file).ToList().ForEach(line =>
        {
            string cmd = line.Split(' ')[0];
            int dist = int.Parse(line.Split(' ')[1]);
            sub = cmd switch
            {
                "forward" => (sub.X + dist, sub.Y),
                "down" => (sub.X, sub.Y + dist),
                "up" => (sub.X, sub.Y - dist),
                _ => throw new Exception("no such command"),
            };
        });
        Console.WriteLine(sub.X * sub.Y);

        // part 2
        Sub s = (0, 0, 0);
        File.ReadAllLines(file).ToList().ForEach(line =>
        {
            string cmd = line.Split(' ')[0];
            int dist = int.Parse(line.Split(' ')[1]);
            s = cmd switch
            {
                "forward" => (s.X + dist, s.Y + s.A * dist, s.A),
                "down" => (s.X, s.Y, s.A + dist),
                "up" => (s.X, s.Y, s.A - dist),
                _ => throw new Exception("no such command"),
            };
        });
        Console.WriteLine(s.X * s.Y);
    }

}