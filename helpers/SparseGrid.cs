using Coord = (int X, int Y);

public class SparseGrid<T>
{
    public Dictionary<Coord, T?> Coordinates { get; set; } = [];
    public T? Default { get; set; }


    // Constructors
    public SparseGrid(T? d = default)
    {
        Default = d;
    }
    public SparseGrid(List<List<T?>> g, T? d = default)
    {
        Default = d;
        for (int y = 0; y < g.Count; y++)
        {
            for (int x = 0; x < g[y].Count; x++)
            {
                if (!Equals(g[y][x], d))
                {
                    Coordinates.Add((x, y), g[y][x]);
                }
            }
        }
    }
    public SparseGrid(Grid<T> g)
    {
        Default = g.Default;
        g.ForEach((c, v) =>
        {
            if (!Equals(v, Default))
            {
                Coordinates.Add(c, v);
            }
        });
    }


    // Getters
    public T? At(Coord pos)
    {
        return Coordinates.GetValueOrDefault(pos, Default);
    }
    public int GetNumValues()
    {
        return Coordinates.Values.Where(v => !Equals(v, Default)).Count();
    }


    // Setters
    public void Set(Coord pos, T? value)
    {
        Coordinates[pos] = value;
    }
    public void Delete(Coord pos)
    {
        Coordinates.Remove(pos);
    }
    public void Move(Coord startPos, Coord destPos)
    {
        Set(destPos, At(startPos));
        Delete(startPos);
    }


    // Map Methods 
    // NOTE: these ignore default values in the grid. If you want to iterate
    // over all values in the grid regardless of whether they are defaults,
    // then convert to a regular grid first via ToGrid
    public SparseGrid<T> ForEach(Action<Coord, T?> f)
    {
        Coordinates.ToList().ForEach(pair =>
        {
            if (!Equals(pair, default))
            {
                f(pair.Key, pair.Value);
            }
        });
        return this;
    }
    public SparseGrid<T> Map(Func<Coord, T?, T?> f)
    {
        SparseGrid<T> newGrid = new(Default);
        ForEach((c, v) =>
        {
            newGrid.Set(c, f(c, v));
        });

        return newGrid;
    }


    // Query Methods
    public (Coord, Coord) Bounds(bool forceZeroBounds = false)
    {
        int minX = forceZeroBounds ? 0 : int.MaxValue;
        int minY = forceZeroBounds ? 0 : int.MaxValue;
        int maxX = 0;
        int maxY = 0;

        Coordinates
            .Where((c, v) => !Equals(v, Default)).ToList()
            .ForEach(pair =>
            {
                Coord c = pair.Key;
                if (c.X < minX) minX = c.X;
                if (c.X > maxX) maxX = c.X;
                if (c.Y < minY) minY = c.Y;
                if (c.Y > maxY) maxY = c.Y;
            });

        return ((X: minX, Y: minY), (X: maxX, Y: maxY));
    }

    public List<Coord> GetNeighbors(Coord coord, bool diagonals = false)
    {
        List<Coord> neighbors = [
            (X: coord.X - 1, coord.Y),
            (X: coord.X + 1, coord.Y),
            (coord.X, Y: coord.Y - 1),
            (coord.X, Y: coord.Y + 1)
        ];
        if (diagonals)
        {
            neighbors.AddRange([
                (X: coord.X - 1, Y: coord.Y - 1),
                (X: coord.X + 1, Y: coord.Y + 1),
                (X: coord.X + 1, Y: coord.Y - 1),
                (X: coord.X - 1, Y: coord.Y + 1)
            ]);
        }
        return neighbors.Where(c => !Equals(At(c), Default)).ToList();
    }

    public Grid<T?> ToGrid(bool forceZeroBounds = true)
    {
        (Coord min, Coord max) = Bounds(forceZeroBounds);
        Grid<T?> grid = Grid<T?>.Initialize(max.X - min.X + 1, max.Y - min.Y + 1, Default);
        grid.ForEach((c, v) =>
        {
            grid.Set(c, At(c));
        });
        return grid;
    }


    // Import/Export Methods:
    public override string ToString()
    {
        string str = "";
        Coordinates.ToList().ForEach(pair => str += pair.Key + ": " + pair.Value + "\n");
        return str;
    }

}