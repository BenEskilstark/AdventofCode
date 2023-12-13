using System.Xml.Linq;

public class Grid10<T>
{
    public List<List<T>> Matrix { get; set; }
    public T Default { get; set; }

    public Grid10(List<List<T>> G, T D)
    {
        this.Matrix = G;
        this.Default = D;
    }

    public T At(Coord10 Pos)
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

    public void Set(Coord10 Pos, T Value)
    {
        Matrix[Pos.Y][Pos.X] = Value;
    }

    public Grid10<T> Map(Func<Coord10, T> F)
    {
        List<List<T>> g = [];
        for (int y = 0; y < Matrix.Count; y++)
        {
            List<T> row = Matrix[y];
            List<T> newRow = [];
            for (int x = 0; x < row.Count; x++)
            {
                newRow.Add(F(new Coord10(x, y)));
            }
            g.Add(newRow);
        }
        // G = g;
        return new Grid10<T>(g, this.Default);
    }

    public Grid10<T> ForEach(Action<Coord10, T> F)
    {
        for (int y = 0; y < Matrix.Count; y++)
        {
            List<T> row = Matrix[y];
            for (int x = 0; x < row.Count; x++)
            {
                F(new Coord10(x, y), Matrix[y][x]);
            }
        }
        return this;
    }

    public Grid10<T> Copy()
    {
        return this.Map(this.At);
    }

    public Grid10<T> Flip()
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
        Grid10<T> newGrid = new Grid10<T>(newLists, this.Default);

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
        foreach (List<T> row in Matrix)
        {
            Console.WriteLine(string.Join("", row));
        }
    }

    public void Save(string Path)
    {
        using (StreamWriter writer = new StreamWriter(Path))
        {
            foreach (List<T> row in Matrix)
            {
                writer.WriteLine(string.Join("", row));
            }
        }
    }

}

public class Coord10
{
    public int X { get; set; }
    public int Y { get; set; }
    public Coord10(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

    public bool Equals(Coord10 other)
    {
        return other.X == this.X && other.Y == this.Y;
    }

    public override string ToString()
    {
        return "(" + this.X + ", " + this.Y + ")";
    }
}