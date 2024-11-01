using System.Xml.Linq;

using Coord = (int X, int Y);

public class Grid<T>
{
    public List<List<T>> Matrix { get; set; }
    public T Default { get; set; }

    public int Width { get; } // num cols
    public int Height { get; } // num rows


    // constructors
    public Grid(List<List<T>> g, T d)
    {
        this.Matrix = g;
        this.Default = d;
        this.Width = g[0].Count;
        this.Height = g.Count;
    }
    public static Grid<T> Initialize(int width, int height, T defaultValue)
    {
        List<List<T>> g = [];
        for (int l = 0; l < height; l++)
        {
            List<T> row = [];
            for (int c = 0; c < width; c++)
            {
                row.Add(defaultValue);
            }
            g.Add(row);
        }
        return new Grid<T>(g, defaultValue);
    }


    // Getters:
    public T At(Coord pos)
    {
        if (pos.Y < 0 || pos.X < 0) return this.Default;
        if (pos.X >= this.Width || pos.Y >= this.Height) return this.Default;
        return Matrix[pos.Y][pos.X];
    }
    public List<T> GetRow(int index)
    {
        return this.Matrix[index];
    }
    public List<T> GetCol(int index)
    {
        List<T> col = [];
        for (int i = 0; i < this.Matrix.Count; i++)
        {
            col.Add(this.Matrix[i][index]);
        }
        return col;
    }


    // Setters:
    public void Set(Coord pos, T value)
    {
        Matrix[pos.Y][pos.X] = value;
    }
    public void SetRow(int index, List<T> row)
    {
        this.Matrix[index] = row;
    }
    public void SetCol(int index, List<T> values)
    {
        for (int i = 0; i < this.Matrix.Count; i++)
        {
            this.Matrix[i][index] = values[i];
        }
    }


    // Map Methods:
    public Grid<T> Map(Func<Coord, T, T> f)
    {
        List<List<T>> g = [];
        for (int y = 0; y < Matrix.Count; y++)
        {
            List<T> row = Matrix[y];
            List<T> newRow = [];
            for (int x = 0; x < row.Count; x++)
            {
                newRow.Add(f((x, y), Matrix[y][x]));
            }
            g.Add(newRow);
        }
        return new Grid<T>(g, this.Default);
    }
    public Grid<T> ForEach(Action<Coord, T> f)
    {
        for (int y = 0; y < Matrix.Count; y++)
        {
            List<T> row = Matrix[y];
            for (int x = 0; x < row.Count; x++)
            {
                f((x, y), Matrix[y][x]);
            }
        }
        return this;
    }
    public Grid<T> Copy()
    {
        return Map((c, v) => v);
    }


    // Query Methods
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
        return neighbors
            .Where(c => c.X >= 0 && c.Y >= 0 && c.X < Width && c.Y < Height)
            .ToList();
    }
    public List<T> GetNeighborValues(Coord coord)
    {
        return GetNeighbors(coord)
            .Select(At)
            .ToList();
    }


    // Mix Methods:
    public Grid<T> Pivot()
    {
        List<List<T>> newLists = [];
        for (int x = 0; x < Matrix[0].Count; x++)
        {
            List<T> row = [];
            for (int y = 0; y < Matrix.Count; y++)
            {
                row.Add(this.Default);
            }
            newLists.Add(row);
        }
        Grid<T> newGrid = new Grid<T>(newLists, this.Default);

        for (int y = 0; y < Matrix.Count; y++)
        {
            for (int x = 0; x < Matrix[y].Count; x++)
            {
                newGrid.Matrix[x][y] = Matrix[y][x];
            }
        }
        return newGrid;
    }


    // Import/Export Methods:
    public void Print()
    {
        Console.WriteLine(this.ToString());
    }
    public override string ToString()
    {
        string str = "";
        foreach (List<T> row in Matrix)
        {
            str += string.Join("", row) + '\n';
        }
        return str;
    }
    public void Save(string filePath)
    {
        using StreamWriter writer = new(filePath);
        foreach (List<T> row in Matrix)
        {
            writer.WriteLine(string.Join("", row));
        }
    }

}