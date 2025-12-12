namespace Year2025;

using Coord = (int X, int Y);

public class Problem12
{
    public static void Solve()
    {
        string file = "2025/problem12/testinput.txt";
        List<Shape> shapes = File.ReadAllText(file).Split("\n\n")[..^1]
            .Select(g => new Shape(int.Parse(g.Split(":")[0]), g.Split(":")[1].Trim()))
            .ToList();

        List<Grid<char>> grids = [];
        List<List<int>> targets = [];
        File.ReadAllText(file).Split("\n\n")[^1]
            .Split("\n")
            .ForEach(l =>
            {
                int width = int.Parse(l.Split(':')[0].Split("x")[0]);
                int height = int.Parse(l.Split(':')[0].Split("x")[1]);
                Grid<char> grid = Grid<char>.Initialize(width, height, '.');
                grid.Default = '#'; // out of bounds
                grids.Add(grid);
                targets.Add(l.Split(':')[1].Trim().Split(" ").Select(int.Parse).ToList());
            });

        int total = 0;
        for (int i = 0; i < grids.Count; i++)
        {
            int totalArea = (0..targets[i].Count).Sum(j => targets[i][j] * shapes[j].Area);
            int gridArea = grids[i].Width * grids[i].Height;
            if (gridArea < totalArea) continue;

            // Stack<Shape> toPlace = [];
            // for (int j = 0; j < targets[i].Count; j++)
            // {
            //     Shape shape = shapes[j];
            //     int numToPlace = targets[i][j];
            //     (0..numToPlace).ForEach(_ => toPlace.Push(shape.Copy()));
            // }

            // while (toPlace.TryPop(out Shape shape))
            // {
            //     if (shape.TryInsertAt)
            // }

            total++;
        }
        total.WriteLine("Part 1:");
    }



    private class Shape
    {
        public int Index { get; }
        public List<Coord> Coords { get; private set; } = [];

        public Coord? InsertedAt { get; private set; } = null;
        public int NumRotations { get; private set; } = 0;

        public int Area { get => Coords.Count; }

        public Shape(int index, string blueprint)
        {
            List<List<char>> chars = blueprint.Split("\n")
                .Select(l => l.ToChars()).ToList();

            Index = index;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (chars[y][x] == '#') Coords.Add((x, y));
                }
            }
        }

        public Shape(int index, List<Coord> coords)
        {
            Index = index;
            Coords = coords.Select(c => c).ToList();
        }

        public bool TryInsertAt(Grid<char> grid, Coord pos)
        {
            if (Coords.Any(c => grid.At(c) != '.')) return false;
            InsertedAt = pos;
            Coords.ForEach(c => grid.Set(c, '#'));
            return true;
        }

        public void RemoveFrom(Grid<char> grid)
        {
            if (InsertedAt == null) return;
            Coords.ForEach(c => grid.Set(c, '.'));
            InsertedAt = null;
        }

        public Shape Rotate(int numRotations = 1)
        {
            for (int i = 0; i < numRotations; i++)
            {
                Coords = Coords.Transpose().Select(c => (-1 * c.X + 2, c.Y)).ToList();
            }
            NumRotations += numRotations;
            return this;
        }

        public Shape Copy()
        {
            return new Shape(Index, Coords);
        }

        public override string ToString()
        {
            List<char> chars = (0..11).ToEnumerable()
                .Select(i => i == 3 || i == 7 ? '\n' : '.')
                .ToList();
            Coords.ForEach(c => chars[4 * c.Y + c.X] = '#');
            return chars.Join();
        }
    }
}