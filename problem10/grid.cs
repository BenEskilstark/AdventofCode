using System.Xml.Linq;

public class Grid<T> {
    private List<List<T>> G {get; set;}
    private T Default {get; set;}

    public void Save(string Path) {
        using (StreamWriter writer = new StreamWriter(Path)) {
            foreach (List<T> row in G) {
                writer.WriteLine(string.Join("", row));
            }
        }
    }

    public Grid(List<List<T>> G, T D) {
        this.G = G;
        this.Default = D;
    }

    public T At(Coord Pos) {
        try {
            return G[Pos.Y][Pos.X];
        } catch (ArgumentOutOfRangeException) {
            return this.Default;
        }    
    }

    public Grid<T> Map(Func<Coord, T> F) {
        List<List<T>> g = [];
        for (int y = 0; y < G.Count; y++) {
            List<T> row = G[y];
            List<T> newRow = [];
            for (int x = 0; x < row.Count; x++) {
                newRow.Add(F(new Coord(x, y)));
            }
            g.Add(newRow);
        }
        G = g;
        return this;
    }

    public Grid<T> ForEach(Action<Coord, T> F) {
        for (int y = 0; y < G.Count; y++) {
            List<T> row = G[y];
            for (int x = 0; x < row.Count; x++) {
                F(new Coord(x, y), G[y][x]);
            }
        }
        return this;
    }

    public void Set(Coord Pos, T Value) {
        G[Pos.Y][Pos.X] = Value;
    }

    public void Print() {
        foreach (List<T> row in G) {
            Console.WriteLine(string.Join("", row));
        }
    }

}

public class Coord {
    public int X {get; set;}
    public int Y {get; set;}
    public Coord(int X, int Y) {
        this.X = X;
        this.Y = Y;
    }

    public bool Equals(Coord other) {
        return other.X == this.X && other.Y == this.Y;
    }

    public override string ToString() {
        return "(" + this.X + ", " + this.Y + ")";
    }
}