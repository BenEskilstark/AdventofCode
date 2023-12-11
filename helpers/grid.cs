using System.Xml.Linq;


public class Grid<T>
{
    public List<List<T>> Matrix { get; set; }
    public T Default { get; set; }

    public Grid(List<List<T>> G, T D)
    {
        this.Matrix = G;
        this.Default = D;
    }

    public T At((int X, int Y) Pos)
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

    public void Set((int X, int Y) Pos, T Value)
    {
        Matrix[Pos.Y][Pos.X] = Value;
    }

    public Grid<T> Map(Func<(int X, int Y), T> F)
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

    public Grid<T> ForEach(Action<(int X, int Y), T> F)
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

    public Grid<T> Flip()
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