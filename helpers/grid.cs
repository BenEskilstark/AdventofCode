using System.Xml.Linq;

using Coord = (int X, int Y);

public class Grid<T>
{
    public List<List<T>> Matrix { get; set; }
    public T Default { get; set; }

    public int Width { get; }
    public int Height { get; }

    public Grid(List<List<T>> G, T D)
    {
        this.Matrix = G;
        this.Default = D;
        this.Width = G[0].Count;
        this.Height = G.Count;
    }

    public List<T> GetRow(int Index)
    {
        return this.Matrix[Index];
    }

    public void SetRow(int Index, List<T> Row)
    {
        this.Matrix[Index] = Row;
    }

    public List<T> GetCol(int Index)
    {
        List<T> col = [];
        Console.WriteLine(this.Matrix[0].Count);
        for (int i = 0; i < this.Matrix.Count; i++)
        {
            col.Add(this.Matrix[i][Index]);
        }
        return col;
    }

    public void SetCol(int Index, List<T> values)
    {
        for (int i = 0; i < this.Matrix.Count; i++)
        {
            this.Matrix[i][Index] = values[i];
        }
    }

    public T At(Coord Pos)
    {
        try
        {
            return Matrix[Pos.Y][Pos.X];
        }
        catch (ArgumentOutOfRangeException)
        {
            return this.Default;
        }
    }

    public void Set(Coord Pos, T Value)
    {
        Matrix[Pos.Y][Pos.X] = Value;
    }

    public Grid<T> Map(Func<Coord, T> F)
    {
        List<List<T>> g = [];
        for (int y = 0; y < Matrix.Count; y++)
        {
            List<T> row = Matrix[y];
            List<T> newRow = [];
            for (int x = 0; x < row.Count; x++)
            {
                newRow.Add(F((x, y)));
            }
            g.Add(newRow);
        }
        // G = g;
        return new Grid<T>(g, this.Default);
    }

    public Grid<T> ForEach(Action<Coord, T> F)
    {
        for (int y = 0; y < Matrix.Count; y++)
        {
            List<T> row = Matrix[y];
            for (int x = 0; x < row.Count; x++)
            {
                F((x, y), Matrix[y][x]);
            }
        }
        return this;
    }

    public Grid<T> Copy()
    {
        return this.Map(this.At);
    }

    public List<Coord> GetNeighbors(Coord coord)
    {
        List<Coord> neighbors = [];
        if (coord.X > 0) neighbors.Add((X: coord.X - 1, coord.Y));
        if (coord.X < Width - 1) neighbors.Add((X: coord.X + 1, coord.Y));
        if (coord.Y > 0) neighbors.Add((coord.X, Y: coord.Y - 1));
        if (coord.Y < Height - 1) neighbors.Add((coord.X, Y: coord.Y + 1));

        return neighbors;
    }

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

    public void Save(string Path)
    {
        using StreamWriter writer = new(Path);
        foreach (List<T> row in Matrix)
        {
            writer.WriteLine(string.Join("", row));
        }
    }

}