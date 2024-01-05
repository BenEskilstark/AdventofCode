namespace Year2022;

using Vec = (int X, int Y);

public class Problem9
{
    public static void Solve()
    {
        string file = "2022/problem9/input.txt";
        Snake snake = new((11, 6));
        HashSet<Vec> tailVisits = [];
        foreach (string line in File.ReadLines(file))
        {
            List<Vec> visits = snake.Move(line.Split(' ')[0], int.Parse(line.Split(' ')[1]));
            visits.ForEach(v => tailVisits.Add(v));
        }
        Console.WriteLine(tailVisits.Count);

        // Grid<char> grid = Grid<char>.Initialize(26, 22, '.');
        // tailVisits.ToList().ForEach(v => grid.Set((v.X, 22 - v.Y), '#'));
        // grid.Print();
    }

}

public class Snake(Vec head)
{
    // public List<Vec> Body { get; private set; } = new([head, head]); // for part 1
    public List<Vec> Body { get; private set; } = new([head, head, head, head, head, head, head, head, head, head]);

    public List<Vec> Move(string dirStr, int moves)
    {
        List<Vec> tailVisits = [this.Body.Last()];
        Vec dir = dirStr switch
        {
            "D" => (0, -1),
            "U" => (0, 1),
            "L" => (-1, 0),
            "R" => (1, 0),
            _ => (0, 0)
        };

        for (int m = 0; m < moves; m++)
        {
            this.Body[0] = (this.Body[0].X + dir.X, this.Body[0].Y + dir.Y);
            for (int i = 1; i < this.Body.Count; i++)
            {
                Vec head = this.Body[i - 1];
                Vec tail = this.Body[i];
                if (!Snake.Adjacent(tail, head))
                {
                    Vec nextTail = (tail.X, tail.Y);
                    if (tail.X != head.X)
                    {
                        if (tail.X > head.X) nextTail.X = tail.X - 1;
                        if (tail.X < head.X) nextTail.X = tail.X + 1;
                    }
                    if (tail.Y != head.Y)
                    {
                        if (tail.Y > head.Y) nextTail.Y = tail.Y - 1;
                        if (tail.Y < head.Y) nextTail.Y = tail.Y + 1;
                    }
                    this.Body[i] = (nextTail.X, nextTail.Y);

                    if (i == this.Body.Count - 1)
                    {
                        tailVisits.Add(this.Body[i]);
                    }

                }
            }

        }
        return tailVisits;
    }

    private static bool Adjacent(Vec a, Vec b)
    {
        return Math.Abs(a.X - b.X) <= 1 && Math.Abs(a.Y - b.Y) <= 1;
    }
}