namespace Year2021;

public class Problem4
{
    public static void Solve()
    {
        string file = "2021/problem4/input.txt";

        List<int> draws = [];
        bool firstLine = true;
        List<List<int>> curNums = [];
        List<BingoBoard> boards = [];
        foreach (string line in File.ReadAllLines(file))
        {
            if (firstLine)
            {
                firstLine = false;
                draws = line.Split(',').Select(int.Parse).ToList();
                continue;
            }
            if (line == "" && curNums.Count > 0)
            {
                boards.Add(new(curNums, draws));
                curNums = [];
            }
            else if (line != "")
            {
                List<int> row = line.Split(' ')
                    .Select(s => s.Trim())
                    .Where(s => s != "")
                    .Select(int.Parse).ToList();
                curNums.Add(row);
            }
        }
        boards.Add(new(curNums, draws));

        // part 1
        bool done = false;
        for (int i = 0; i < draws.Count; i++)
        {
            foreach (BingoBoard board in boards)
            {
                if (board.IsWinningAt(i))
                {
                    Console.WriteLine(board.GetScore(i));
                    done = true;
                    break;
                }
            }
            if (done) break;
        }

        // part 2
        int numWon = 0;
        for (int i = 0; i < draws.Count; i++)
        {
            foreach (BingoBoard board in boards)
            {
                if (!board.HasWon && board.IsWinningAt(i))
                {
                    board.HasWon = true;
                    numWon++;
                    if (numWon == boards.Count)
                    {
                        Console.WriteLine(board.GetScore(i));
                    }
                }
            }
        }
    }
}

public class BingoBoard(List<List<int>> numbers, List<int> draws)
{
    public Grid<int> Numbers = new(numbers, -1);
    public List<int> Draws = draws;
    public bool HasWon { get; set; } = false;

    public bool IsWinningAt(int index)
    {
        if (index < 4) return false;
        List<int> markedNums = Draws[..(index + 1)];

        for (int i = 0; i < Numbers.Width; i++)
        {
            if (Numbers.GetCol(i).All(markedNums.Contains)) return true;
        }
        for (int i = 0; i < Numbers.Height; i++)
        {
            if (Numbers.GetRow(i).All(markedNums.Contains)) return true;
        }

        return false;
    }

    public int GetScore(int index)
    {
        List<int> markedNums = Draws[..(index + 1)];
        int sumOfUnmarked = 0;
        Numbers.ForEach((coord, val) =>
        {
            if (!markedNums.Contains(val)) sumOfUnmarked += val;
        });

        // Console.WriteLine(sumOfUnmarked + " " + index + " " + Draws[index]);

        return sumOfUnmarked * Draws[index];
    }

    public override string ToString()
    {
        return Numbers.ToString();
    }
}