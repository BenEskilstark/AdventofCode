namespace Year2024;

using Coord = (int X, int Y);
using Visit = (int X, int Y, char D);

public class Problem6
{
    public static void Solve()
    {
        string file = "2024/problem6/input.txt";
        Grid<char> grid = new([.. File.ReadLines(file).Select(l => l.ToList())], '-');
        Set<Coord> visited = new(grid.Collect((pos, val) => val == '^'));
        Coord initialPosition = visited.ToList()[0];
        Guard guard = new(grid, initialPosition, '^');

        while (grid.At(guard.Pos) != '-')
        {
            visited.Add(guard.Move().Pos);
        }
        visited.Remove(guard.Pos); // included the move out of the grid
        visited.Count.WriteLine("Part 1");

        int numLoops = 0;
        visited.ToList().ForEach(pos =>
        {
            if (grid.At(pos) == '^') return; // skip start
            guard = new(grid, initialPosition, '^'); // move guard back to start
            grid.Set(pos, '#'); // add new obstacle
            CountSet<Visit> loopChecker = new(); // visiting same pos in same dir is a loop
            while (grid.At(guard.Pos) != '-' && loopChecker[guard.ToTuple()] < 2)
            {
                loopChecker.Add(guard.Move().ToTuple());
            }
            if (loopChecker[guard.ToTuple()] > 1)
            {
                numLoops++;
            }
            grid.Set(pos, '.'); // remove obstacle 
        });
        Console.WriteLine("Part 2: " + numLoops);
    }
}

public class Guard(Grid<char> grid, Coord pos, char dir)
{
    public Coord Pos { get; set; } = pos;
    public char Dir { get; set; } = dir;
    public Grid<char> Map = grid;


    public Visit ToTuple()
    {
        return (Pos.X, Pos.Y, Dir);
    }

    public Guard Move()
    {
        Coord next = NextPos();
        while (Map.At(next) == '#')
        {
            Rotate();
            next = NextPos();
        }
        Pos = next;
        return this;
    }

    private Coord NextPos()
    {
        Coord posOffset = Dir switch
        {
            '^' => (0, -1),
            '>' => (1, 0),
            'v' => (0, 1),
            '<' => (-1, 0),
        };
        return (Pos.X + posOffset.X, Pos.Y + posOffset.Y);
    }

    private void Rotate()
    {
        Dir = Dir switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            '<' => '^',
        };
    }
}