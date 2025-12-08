namespace Year2025;

using System.Net.Mime;
using Coord = (long X, long Y, long Z);

public class Problem8
{
    public static void Solve()
    {
        string file = "2025/problem8/input.txt";
        List<Coord> coords = File.ReadAllLines(file)
            .Select(l => (l.GetLongs()[0], l.GetLongs()[1], l.GetLongs()[2]))
            .ToList();

        // precompute distances between all pairs of coordinates and sort them
        List<Pair> pairs = [];
        for (int i = 0; i < coords.Count - 1; i++)
        {
            Coord a = coords[i];
            for (int j = i + 1; j < coords.Count; j++)
            {
                Coord b = coords[j];
                pairs.Add(new(a, b));
            }
        }
        pairs.Sort();

        List<Circuit> circuits = [];
        for (int i = 0; i < 1000; i++)
        {
            circuits = Circuit.Combine(circuits, pairs[i]);
        }
        circuits.OrderByDescending(c => c.Size).Take(3)
            .Aggregate((long)1, (prod, circuit) => prod * circuit.Size)
            .WriteLine("Part 1:");

        foreach (Pair pair in pairs)
        {
            circuits = Circuit.Combine(circuits, pair);
            if (circuits.Count == 1 && circuits[0].Size == coords.Count)
            {
                Console.WriteLine("Part 2: " + (pair.A.X * pair.B.X));
                break;
            }
        }
    }

    public static double Dist(Coord a, Coord b)
    {
        return Math.Sqrt(
            (a.X - b.X) * (a.X - b.X) +
            (a.Y - b.Y) * (a.Y - b.Y) +
            (a.Z - b.Z) * (a.Z - b.Z)
        );
    }

    private class Circuit()
    {
        public Set<Coord> Coords { get; set; } = new();
        public long Size { get => Coords.Count; }

        public bool ConnectsTo(Pair pair)
        {
            return Coords[pair.A] || Coords[pair.B];
        }

        public static List<Circuit> Combine(List<Circuit> circuits, Pair pair)
        {
            List<Circuit> containedIn = [];
            List<Circuit> notContainedIn = [];
            foreach (Circuit c in circuits)
            {
                if (c.ConnectsTo(pair)) containedIn.Add(c);
                if (!c.ConnectsTo(pair)) notContainedIn.Add(c);
            }
            if (containedIn.Count == 0)
            {
                return [.. circuits, new Circuit() { Coords = new([pair.A, pair.B]) }];
            }
            else if (containedIn.Count == 1)
            {
                containedIn[0].Coords.Add(pair.A);
                containedIn[0].Coords.Add(pair.B);
                return circuits;
            }
            else
            {
                containedIn[0].Coords.Add(pair.A);
                containedIn[0].Coords.Add(pair.B);
                Circuit combined = new();
                foreach (Circuit c2 in containedIn)
                {
                    combined.Coords.AddMany(c2.Coords.ToList());
                }
                return [combined, .. notContainedIn];
            }
        }
    }

    private class Pair(Coord a, Coord b) : IComparable<Pair>
    {
        public Coord A { get; } = a;
        public Coord B { get; } = b;

        public double Dist { get; } = Dist(a, b);

        public int CompareTo(Pair? pair)
        {
            if (pair == null) return 1;
            return Dist.CompareTo(pair.Dist);
        }

        public override string ToString()
        {
            return A.ToString() + B.ToString() + ": " + Math.Round(Dist, 2);
        }
    }

}