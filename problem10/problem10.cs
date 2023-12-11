using System.Diagnostics;
using System.Runtime.InteropServices;

public class Problem10
{
    public static void Solve()
    {
        List<List<char>> grid = [];
        Coord10 s = new Coord10(-1, -1);
        foreach (string line in File.ReadAllLines("problem10/cleaninput.txt"))
        {
            List<char> row = line.ToCharArray().ToList();
            grid.Add(row);
            for (int i = 0; i < row.Count; i++)
            {
                if (row[i] == 'S')
                {
                    s = new Coord10(i, grid.Count - 1);
                }
            }
        }
        Grid10<char> maze = new Grid10<char>(grid, '?');

        List<Coord10> periods = [];
        maze.ForEach((p, c) =>
        {
            if (c == '.')
            {
                periods.Add(p);
            }
        });
        // maze.Print();
        Utils.PrintList(periods);
        // foreach (Coord period in periods) {
        //     Console.WriteLine(maze.At(period));
        // }   

        periods = periods.Where(c =>
        {
            int left = Trace(maze, c, -1, 0);
            int right = Trace(maze, c, 1, 0);
            int up = Trace(maze, c, 0, -1);
            int down = Trace(maze, c, 0, 1);
            if (c.Equals(new Coord10(6, 6)))
            {
                Console.WriteLine(up + " " + down + " " + left + " " + right);
            }
            // Console.WriteLine(c + " " + down);
            if (up == 0 || down == 0 || left == 0 || right == 0) return false;
            if (left % 2 == 0 || right % 2 == 0 || up % 2 == 0 || down % 2 == 0)
            {
                return false;
            }
            return true;
        }).ToList();
        Utils.PrintList(periods);
        Console.WriteLine(periods.Count);
    }

    public static int Trace(Grid10<char> Maze, Coord10 Pos, int Dx, int Dy)
    {
        Coord10 marker = new Coord10(Pos.X, Pos.Y);
        char prevPipe = '?';
        int pipesCrossed = 0;
        while (Maze.At(marker) != '?')
        {
            marker.X += Dx;
            marker.Y += Dy;
            char charAt = Maze.At(marker);
            if (charAt == '.') continue;

            if (Dx == 1)
            {
                if (charAt == '|')
                {
                    pipesCrossed++;
                    continue;
                }
                if (charAt == '-') continue;
                if (prevPipe == '?' && (charAt == 'F' || charAt == 'L'))
                {
                    prevPipe = charAt;
                    continue;
                }
                if ((charAt == 'J' && prevPipe == 'F') || (charAt == '7' && prevPipe == 'L'))
                {
                    pipesCrossed++;
                    prevPipe = '?';
                }
                if ((charAt == 'J' && prevPipe == 'L') || (charAt == '7' && prevPipe == 'F'))
                {
                    prevPipe = '?';
                }

            }
            if (Dx == -1)
            {
                if (charAt == '|')
                {
                    pipesCrossed++;
                    continue;
                }
                if (charAt == '-') continue;
                if (prevPipe == '?' && (charAt == 'J' || charAt == '7'))
                {
                    prevPipe = charAt;
                    continue;
                }
                if ((charAt == 'F' && prevPipe == 'J') || (charAt == 'L' && prevPipe == '7'))
                {
                    pipesCrossed++;
                    prevPipe = '?';
                }
                if ((charAt == 'F' && prevPipe == '7') || (charAt == 'L' && prevPipe == 'J'))
                {
                    prevPipe = '?';
                }
            }

            if (Dy == 1)
            {
                if (charAt == '-')
                {
                    pipesCrossed++;
                    continue;
                }
                if (charAt == '|') continue;
                if (prevPipe == '?' && (charAt == '7' || charAt == 'F'))
                {
                    prevPipe = charAt;
                    continue;
                }
                if ((charAt == 'L' && prevPipe == '7') || (charAt == 'J' && prevPipe == 'F'))
                {
                    pipesCrossed++;
                    prevPipe = '?';
                }
                if ((charAt == 'L' && prevPipe == 'F') || (charAt == 'J' && prevPipe == '7'))
                {
                    prevPipe = '?';
                }
            }
            if (Dy == -1)
            {
                if (charAt == '-')
                {
                    pipesCrossed++;
                    continue;
                }
                if (charAt == '|') continue;
                if (prevPipe == '?' && (charAt == 'L' || charAt == 'J'))
                {
                    prevPipe = charAt;
                    continue;
                }
                if ((charAt == '7' && prevPipe == 'L') || (charAt == 'F' && prevPipe == 'J'))
                {
                    pipesCrossed++;
                    prevPipe = '?';
                }
                if ((charAt == '7' && prevPipe == 'J') || (charAt == 'F' && prevPipe == 'L'))
                {
                    prevPipe = '?';
                }
            }

        }
        return pipesCrossed;
    }

    public static void SolvePt1()
    {
        List<List<char>> grid = [];
        Coord10 s = new Coord10(-1, -1);
        foreach (string line in File.ReadAllLines("problem10/cleaninput.txt"))
        {
            List<char> row = line.ToCharArray().ToList();
            grid.Add(row);
            for (int i = 0; i < row.Count; i++)
            {
                if (row[i] == 'S')
                {
                    s = new Coord10(i, grid.Count - 1);
                }
            }
        }
        Grid10<char> maze = new Grid10<char>(grid, '.');
        List<Mouse> mice = GetStarts(maze, s)
            .Select(pos => new Mouse(pos.X, pos.Y, s)).ToList();
        // Utils.PrintList(mice);
        // maze.Print();
        while (!(
            mice[0].Pos.Equals(mice[1].Pos) ||
            (mice[0].PrevPos.Equals(mice[1].PrevPos) && mice[0].StepsTaken > 1) // or same prevPos
        ))
        {
            mice.ForEach(mouse => mouse.Step(maze.At(mouse.Pos)));
        }
        // List<Coord> pipes = mice[0].AllSteps.Concat(mice[1].AllSteps).ToList();
        // Utils.PrintList(pipes);
        // maze.Map(pos => Contains(pipes, pos) ? maze.At(pos) : '.').Save("problem10/cleaninput.txt");

        Console.WriteLine(mice[0].StepsTaken);
        // Utils.PrintList(mice);


    }

    public static bool Contains(List<Coord10> pipes, Coord10 pos)
    {
        foreach (Coord10 pipe in pipes)
        {
            if (pipe.Equals(pos)) return true;
        }
        return false;
    }

    public static List<Coord10> GetStarts(Grid10<char> Grid, Coord10 S)
    {
        List<Coord10> starts = [];
        Coord10 left = new Coord10(S.X - 1, S.Y);
        Coord10 right = new Coord10(S.X + 1, S.Y);
        Coord10 up = new Coord10(S.X, S.Y - 1);
        Coord10 down = new Coord10(S.X, S.Y + 1);
        if (Grid.At(left) == '-' || Grid.At(left) == 'F' || Grid.At(left) == 'L')
        {
            starts.Add(left);
        }
        if (Grid.At(right) == '-' || Grid.At(right) == '7' || Grid.At(right) == 'J')
        {
            starts.Add(right);
        }
        if (Grid.At(up) == '|' || Grid.At(up) == '7' || Grid.At(up) == 'F')
        {
            starts.Add(up);
        }
        if (Grid.At(down) == '|' || Grid.At(down) == 'J' || Grid.At(down) == 'L')
        {
            starts.Add(down);
        }
        return starts;
    }
}


public class Mouse
{
    public Coord10 Pos { get; set; }
    public Coord10 PrevPos { get; set; }
    public int StepsTaken { get; set; } = 1;
    public List<Coord10> AllSteps { get; set; } = [];

    public void Step(char pipe)
    {
        Coord10 next = GetNexts(pipe)
            .Where(pos => !pos.Equals(this.PrevPos)).ToList()[0];
        this.PrevPos = this.Pos;
        this.Pos = next;
        this.AllSteps.Add(this.Pos);
        this.StepsTaken++;
    }

    public List<Coord10> GetNexts(char pipe)
    {
        int X = this.Pos.X;
        int Y = this.Pos.Y;
        return pipe switch
        {
            '|' => [new Coord10(X, Y - 1), new Coord10(X, Y + 1)],
            '-' => [new Coord10(X - 1, Y), new Coord10(X + 1, Y)],
            'J' => [new Coord10(X - 1, Y), new Coord10(X, Y - 1)],
            'F' => [new Coord10(X + 1, Y), new Coord10(X, Y + 1)],
            '7' => [new Coord10(X - 1, Y), new Coord10(X, Y + 1)],
            'L' => [new Coord10(X + 1, Y), new Coord10(X, Y - 1)],
            _ => throw new Exception("no matching pipe"),
        };
    }

    public Mouse(int X, int Y, Coord10 prev)
    {
        this.Pos = new Coord10(X, Y);
        this.PrevPos = prev;
        this.AllSteps.Add(this.Pos);
        this.AllSteps.Add(prev);
    }

    public override string ToString()
    {
        return this.Pos.ToString() + "(" + this.PrevPos.ToString() + ") -> " + this.StepsTaken;
    }
}