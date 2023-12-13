namespace Year2022;

public enum RPS { Rock, Paper, Scissors };

public class Problem2
{
    public static void Solve()
    {
        string file = "2022/problem2/input.txt";

        Dictionary<char, RPS> map = new Dictionary<char, RPS> {
            {'A', RPS.Rock},
            {'B', RPS.Paper},
            {'C', RPS.Scissors},
            {'X', RPS.Rock},
            {'Y', RPS.Paper},
            {'Z', RPS.Scissors},
        };

        Dictionary<RPS, RPS> rps = new Dictionary<RPS, RPS>{
            { RPS.Rock, RPS.Scissors},
            { RPS.Paper, RPS.Rock },
            { RPS.Scissors, RPS.Paper },
        };

        Dictionary<RPS, RPS> loss = new Dictionary<RPS, RPS> {
            { RPS.Rock, RPS.Paper},
            { RPS.Paper, RPS.Scissors },
            { RPS.Scissors, RPS.Rock },
        };

        Dictionary<RPS, int> scores = new Dictionary<RPS, int> {
            {RPS.Rock, 1},
            {RPS.Paper, 2},
            {RPS.Scissors, 3},
        };

        int sum = 0;
        foreach (string line in File.ReadLines(file))
        {
            RPS them = map[line.Split(" ")[0][0]];
            char strat = line.Split(" ")[1][0];

            if (strat == 'X')
            {
                sum += scores[rps[them]];
            }
            else if (strat == 'Y')
            {
                sum += 3 + scores[them];
            }
            else if (strat == 'Z')
            {
                sum += 6 + scores[loss[them]];
            }

        }

        Console.WriteLine(sum);
    }
}