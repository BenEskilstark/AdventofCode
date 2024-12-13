namespace Year2024;

using Pair = (long A, long B);

public class Problem13
{
    public static void Solve()
    {
        string file = "2024/problem13/input.txt";
        long numTokens = 0;
        File.ReadAllText(file).Split("\r\n\r\n").ToList().ForEach(claw =>
        {
            List<List<int>> n = [.. claw.Split("\r\n").Select(l => l.GetNums())];
            Pair res = Solve2(n[0][0], n[1][0], n[0][1], n[1][1], n[2][0], n[2][1]);
            numTokens += res.A * 3 + res.B;
        });
        numTokens.WriteLine("Part 1:");

        numTokens = 0;
        File.ReadAllText(file).Split("\r\n\r\n").ToList().ForEach(claw =>
        {
            List<List<int>> n = [.. claw.Split("\r\n").Select(l => l.GetNums())];
            Pair res = Solve2(
                n[0][0], n[1][0], n[0][1], n[1][1],
                checked(10000000000000 + n[2][0]),
                checked(10000000000000 + n[2][1])
            );
            numTokens += checked(res.A * 3 + res.B);
        });
        numTokens.WriteLine("Part 2:");
    }

    public static Pair Solve2(long a1, long b1, long a2, long b2, long c1, long c2)
    {
        long determinant = checked(a1 * b2 - a2 * b1);
        if (determinant == 0) return (0, 0);

        long A = checked(c1 * b2 - c2 * b1);
        long B = checked(a1 * c2 - a2 * c1);
        if (A % determinant != 0 || B % determinant != 0) return (0, 0);
        return (A / determinant, B / determinant);
    }
}