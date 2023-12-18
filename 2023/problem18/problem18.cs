namespace Year2023;

using System.Security.Cryptography;
using Coord = (long X, long Y);

public class Problem18
{
    public static void Solve()
    {
        string file = "2023/problem18/input.txt";
        // SolvePt1(file);
        SolvePt2(file);
    }

    public static void SolvePt2(string file)
    {
        Coord digger = (0, 0);
        HashSet<Coord> verts = [(0, 0)];
        (long left, long right, long top, long bottom) = (0, 0, 0, 0);

        // do the digging
        int perimeter = 0;
        int count = 0;
        foreach (string line in File.ReadLines(file))
        {
            (int X, int Y, int num) = ParseIns2(line);
            // Console.WriteLine(X + " " + Y + " " + num);
            digger = (digger.X + X * num, digger.Y + Y * num);
            verts.Add(digger);
            perimeter += num;
            count++;

            if (digger.X < left) left = digger.X;
            if (digger.X > right) right = digger.X;
            if (digger.Y < top) top = digger.Y;
            if (digger.Y > bottom) bottom = digger.Y;
        }
        // Console.WriteLine("perimeter: " + perimeter);
        // normalize by top left
        List<Coord> normVerts = [];
        verts.ToList().ForEach(Key => normVerts.Add((X: Key.X - left, Y: Key.Y - top)));
        normVerts.Reverse();

        long area = 0;
        for (int i = 0; i < normVerts.Count - 1; i++)
        {
            Coord fst = normVerts[i];
            Coord snd = normVerts[i + 1];
            area += (fst.Y + snd.Y) * (fst.X - snd.X);
            // Console.WriteLine(area);
        }
        area /= 2;
        area = Math.Abs(area);
        long picks = area + perimeter / 2 + 1;
        Console.WriteLine(picks);
    }

    public static void SolvePt1(string file)
    {
        Coord digger = (0, 0);
        HashSet<Coord> digHoles = [(0, 0)];
        (long left, long right, long top, long bottom) = (0, 0, 0, 0);

        // do the digging
        int perimeter = 0;
        foreach (string line in File.ReadLines(file))
        {
            (int X, int Y, int num) = ParseIns1(line);
            int i = 1;
            for (; i <= num; i++)
            {
                perimeter++;
                digHoles.Add((digger.X + X * i, digger.Y + Y * i));
            }
            digger = (digger.X + X * (i - 1), digger.Y + Y * (i - 1));
            if (digger.X < left) left = digger.X;
            if (digger.X > right) right = digger.X;
            if (digger.Y < top) top = digger.Y;
            if (digger.Y > bottom) bottom = digger.Y;
        }

        // add a bit of padding
        left--; top--; right++; bottom++;
        // normalize by top left
        HashSet<Coord> normalizedDigHoles = [];
        digHoles.ToList().ForEach(Key => normalizedDigHoles.Add((X: Key.X - left, Y: Key.Y - top)));

        // Grid<string> grid = ToGrid(normalizedDigHoles, (left, right, top, bottom));
        // grid.Print();

        long boxArea = (bottom - top + 1) * (right - left + 1);
        long innerArea = GetArea(normalizedDigHoles, right - left + 1, bottom - top + 1);
        Console.WriteLine("perimeter: " + perimeter);
        Console.WriteLine(innerArea);
    }

    public static (int X, int Y, int Num) ParseIns1(string Line)
    {
        List<string> ins = [.. Line.Split(' ')];
        int X = ins[0] == "R" ? 1 : (ins[0] == "L" ? -1 : 0);
        int Y = ins[0] == "D" ? 1 : (ins[0] == "U" ? -1 : 0);
        int num = int.Parse(ins[1]);

        return (X, Y, num);
    }

    public static (int X, int Y, int Num) ParseIns2(string line)
    {
        string ins = line.Split(' ').ToList()[2];
        int X = ins[^2] == '0' ? 1 : (ins[^2] == '2' ? -1 : 0);
        int Y = ins[^2] == '1' ? 1 : (ins[^2] == '3' ? -1 : 0);
        int num = Convert.ToInt32(ins[2..^2], 16);

        return (X, Y, num);
    }

    public static long GetArea(HashSet<Coord> DigHoles, long Width, long Height)
    {
        HashSet<Coord> visited = [];
        Queue<Coord> toVisit = [];
        toVisit.Enqueue((0, 0));

        while (toVisit.TryDequeue(out Coord next))
        {
            if (visited.Contains(next)) continue;
            if (DigHoles.Contains(next)) continue;
            visited.Add(next);
            if (next.X + 1 < Width) toVisit.Enqueue((X: next.X + 1, Y: next.Y));
            if (next.X - 1 >= 0) toVisit.Enqueue((X: next.X - 1, Y: next.Y));
            if (next.Y + 1 < Height) toVisit.Enqueue((X: next.X, Y: next.Y + 1));
            if (next.Y - 1 >= 0) toVisit.Enqueue((X: next.X, Y: next.Y - 1));
        }

        return (Width * Height) - visited.Count;
    }

    public static Grid<string> ToGrid(
        HashSet<Coord> NormalizedDigHoles,
        (int left, int right, int top, int bottom) R
    )
    {
        List<List<string>> ground = [];
        for (int y = 0; y < R.bottom - R.top + 1; y++)
        {
            List<string> row = [];
            for (int x = 0; x < R.right - R.left + 1; x++)
            {
                if (NormalizedDigHoles.Contains((x, y)))
                {
                    row.Add("#");
                }
                else
                {
                    row.Add(".");
                }
            }
            ground.Add(row);
        }

        return new(ground, "?");
    }
}
