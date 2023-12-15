using System.Dynamic;

namespace Year2023;

public class Problem14
{
    public static void Solve()
    {
        string file = "2023/problem14/input.txt";

        List<List<char>> input = [];
        File.ReadAllLines(file).ToList()
            .ForEach((string line) => input.Add([.. line.ToCharArray()]));

        TiltBoard tiltBoard = new(input);
        List<long> loads = [];
        int doneIndex = -1;
        int numCycles = 1000000000;
        for (int i = 0; i < numCycles; i++)
        {
            tiltBoard.TiltNorth();
            tiltBoard.TiltWest();
            tiltBoard.TiltSouth();
            tiltBoard.TiltEast();
            if (tiltBoard.CycleDict.TryGetValue(tiltBoard.ToString(), out doneIndex)) break;
            tiltBoard.CycleDict[tiltBoard.ToString()] = i;
            loads.Add(tiltBoard.GetLoadNorth());

        }
        Console.WriteLine("doneIndex " + doneIndex);
        int cycleLength = loads.Count - doneIndex;
        Console.WriteLine("cycle length " + cycleLength);
        Console.WriteLine((numCycles - doneIndex) % cycleLength + doneIndex - 1);
        // Console.WriteLine(numCycles % tiltBoard.CycleDict.Values.Count);
        Utils.PrintList(loads);
        // tiltBoard.Board.Print();
        // Console.WriteLine(tiltBoard);
        Console.WriteLine(loads[(numCycles - doneIndex) % cycleLength + doneIndex - 1]);
    }
}

public class TiltBoard
{
    public Grid<char> Board { get; set; }
    public Dictionary<string, int> CycleDict { get; set; } = [];

    public TiltBoard(List<List<char>> input)
    {
        this.Board = new Grid<char>(input, '#');
    }

    public override string ToString()
    {
        return string.Join("",
            this.Board.Matrix.Select((List<char> row) => string.Join("", row))
        );
    }

    public void TiltNorth()
    {
        Grid<char> board = this.Board.Pivot();

        List<List<char>> nextBoardInput = [];
        foreach (List<char> row in board.Matrix)
        {
            nextBoardInput.Add(TiltRow(row));
        }
        board.Matrix = nextBoardInput;

        board = board.Pivot();
        this.Board = board;
    }
    public void TiltSouth()
    {
        Grid<char> board = this.Board.Pivot();

        List<List<char>> nextBoardInput = [];
        foreach (List<char> row in board.Matrix)
        {
            row.Reverse();
            List<char> tilted = TiltRow(row);
            tilted.Reverse();
            nextBoardInput.Add(tilted);
        }
        board.Matrix = nextBoardInput;

        board = board.Pivot();
        this.Board = board;
    }
    public void TiltWest()
    {
        List<List<char>> nextBoardInput = [];
        foreach (List<char> row in this.Board.Matrix)
        {
            nextBoardInput.Add(TiltRow(row));
        }
        this.Board.Matrix = nextBoardInput;
    }
    public void TiltEast()
    {
        List<List<char>> nextBoardInput = [];
        foreach (List<char> row in this.Board.Matrix)
        {
            row.Reverse();
            List<char> tilted = TiltRow(row);
            tilted.Reverse();
            nextBoardInput.Add(tilted);
        }
        this.Board.Matrix = nextBoardInput;
    }

    private static List<char> TiltRow(List<char> row)
    {
        List<char> final = Enumerable.Repeat('.', row.Count).ToList();
        int lastHashIndex = 0;
        for (int i = 0; i < row.Count; i++)
        {
            if (row[i] == '#')
            {
                final[i] = '#';
                lastHashIndex = i;
            }
            if (row[i] == 'O')
            {
                for (int j = lastHashIndex; j <= i; j++)
                {
                    if (final[j] == '.')
                    {
                        final[j] = 'O';
                        break;
                    }
                }
            }
        }
        return final;
    }

    public long GetLoadNorth()
    {
        long load = 0;
        for (int i = 0; i < this.Board.Matrix.Count; i++)
        {
            for (int j = 0; j < this.Board.Matrix[i].Count; j++)
            {
                if (this.Board.Matrix[i][j] == 'O')
                {
                    load += this.Board.Matrix.Count - i;
                }
            }
        }
        return load;
    }
}