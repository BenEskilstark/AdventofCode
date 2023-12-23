using Coord = (int X, int Y);
public class PathNode
{
    public int Heat { get; }
    public Coord Pos { get; } = (0, 0);
    public PathNode? Previous { get; set; }
    public char Direction { get; set; } = '.';
    public PathNode(PathNode? Prev, Coord P, int H)
    {
        this.Pos = P;
        this.Previous = Prev;
        this.Heat = H;
        if (Prev != null)
        {
            (int DX, int DY) = (P.X - Prev.Pos.X, P.Y - Prev.Pos.Y);
            if (DX == 1) this.Direction = '>';
            if (DX == -1) this.Direction = '<';
            if (DY == 1) this.Direction = 'v';
            if (DY == -1) this.Direction = '^';
        }
    }

    public long GetScore()
    {
        // This looks cleaner but runs slower!
        // return this.GetPath().Select(p => p.Heat).Aggregate((a, b) => a + b);
        long score = 0;
        for (PathNode? ptr = this; ptr != null; ptr = ptr.Previous)
        {
            score += ptr.Heat;
        }
        return score;
    }

    public IEnumerable<PathNode> GetPath()
    {
        PathNode? ptr = this.Previous;
        while (ptr != null)
        {
            yield return ptr;
            ptr = ptr.Previous;
        }
    }

    public int NumSameDirection()
    {
        int res = 0;
        PathNode? ptr = this;
        while (ptr != null && ptr.Direction == this.Direction)
        {
            ptr = ptr.Previous;
            res++;
        }
        return res;
    }

    public (int, int, char, int) ToTuple()
    {
        return (this.Pos.X, this.Pos.Y, this.Direction, this.NumSameDirection());
    }

    public override string ToString()
    {
        return this.Pos + " " + this.Direction + " " + this.GetScore();
    }
}