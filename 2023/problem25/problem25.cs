namespace Year2023;

using Edge = (string A, string B);
using Partition = HashSet<string>;

public class Problem25
{
    public static void Solve()
    {
        string file = "2023/problem25/testinput.txt";
        Graph graph = Graph.FromFile(file);
        // Console.WriteLine(graph);
        Console.WriteLine(graph.GetVertices().Count);

        Graph cut = graph.MinCut();
        // Console.WriteLine(cut);
    }
}
public class Graph
{
    public Dictionary<Edge, int> Edges { get; set; } = [];
    public Dictionary<string, int> Combos { get; set; } = [];
    public Dictionary<string, HashSet<string>> Adj { get; set; } = [];

    public Graph MinCut()
    {
        Graph graph = this;
        int minCutWeight = 100000000;
        List<Edge> minCut = [];
        while (graph.GetVertices().Count > 2)
        {
            List<string> vs = graph.GetVertices();
            (List<Edge> cut, int weight) = graph.MinCutPhase(vs[0], vs[1..]);
            Console.WriteLine("~~~~~~~~WEIGHT: " + weight + " ~~~~~~~~");
            if (weight < minCutWeight)
            {
                minCutWeight = weight;
                minCut = cut;
                if (minCutWeight == 3)
                {
                    // Console.WriteLine("Min Cut: " + string.Join(',', minCut));
                    // Console.WriteLine(graph);
                    // Console.WriteLine(string.Join(',', graph.Combos.ToList()));
                }
            }
        }

        return graph;
    }

    private (List<Edge>, int) MinCutPhase(string a, List<string> r)
    {
        Partition rest = r.ToHashSet();
        Partition A = [a];
        List<string> lastTwo = [];
        List<Edge> minCut = [];
        int minCutWeight = 1000000;
        while (rest.Count > 0)
        {
            (string most, List<Edge> cut, int weight) = this.GetMostConnected(A, rest);
            // if (rest.Count % 100 == 0) Console.WriteLine(most + " " + rest.Count);
            A.Add(most);
            rest.Remove(most);
            if (rest.Count < 2) lastTwo.Add(most);
            if (rest.Count == 0)
            {
                minCut = cut;
                minCutWeight = weight;
            }
        }
        if (minCutWeight == 3)
        {
            Console.WriteLine("Min Cut: " + string.Join(',', minCut));
            // Console.WriteLine(this);
            Console.WriteLine(string.Join(',', this.Combos.ToList()));
            Partition left = [];
            Partition right = [];
            int leftPart = 0;
            int rightPart = 0;
            foreach (Edge e in minCut)
            {
                if (!left.Contains(e.A)) leftPart += this.Combos[e.A];
                if (!right.Contains(e.B)) rightPart += this.Combos[e.B];
                left.Add(e.A);
                right.Add(e.B);
            }


            Console.WriteLine(leftPart + " x " + rightPart + " = " + (leftPart * rightPart));
            Console.WriteLine("done");
            // Console.WriteLine("merging: " + lastTwo[0] + " " + lastTwo[1]);
        }
        this.Merge(lastTwo[0], lastTwo[1]);

        // Console.WriteLine(this);
        return (minCut, minCutWeight);
    }

    public (string, List<Edge>, int) GetMostConnected(Partition A, Partition rest)
    {
        string most = "???";
        int numCons = 0;
        List<Edge> mostCons = [];
        foreach (string v in rest)
        {
            int total = 0;
            string thisMost = "???";
            List<Edge> cons = [];
            foreach (string a in this.Adj[v])
            {
                if (!A.Contains(a)) continue;
                int w = this.GetWeight((v, a));
                if (w > 0)
                {
                    total += w;
                    cons.Add((v, a));
                    thisMost = v;
                }
            }
            if (total > numCons)
            {
                numCons = total;
                most = thisMost;
                mostCons = cons;
            }
        }
        if (most == "???") throw new Exception("no most connected");
        return (most, mostCons, numCons);
    }


    public void AddEdge(Edge edge)
    {
        if (this.Edges.ContainsKey((edge.B, edge.A))) return;
        this.Edges[edge] = 1;
    }
    public bool HasEdge(Edge edge)
    {
        return this.Edges.ContainsKey(edge) || this.Edges.ContainsKey((edge.B, edge.A));
    }
    public int GetWeight(Edge edge)
    {
        if (this.Edges.ContainsKey((edge.B, edge.A))) return this.Edges[(edge.B, edge.A)];
        if (this.Edges.ContainsKey(edge)) return this.Edges[edge];
        return 0;
    }
    public List<string> GetVertices()
    {
        HashSet<string> vertices = [];
        this.Edges.Keys.ToList().ForEach((Edge e) =>
        {
            vertices.Add(e.A);
            vertices.Add(e.B);
        });
        return vertices.ToList();
    }
    public void Merge(string dest, string toMerge)
    {
        HashSet<string> adjacencies = this.Adj[toMerge];
        foreach (string v in adjacencies.ToList())
        {
            if (v == toMerge) continue;
            this.Adj[dest].Add(v);
            this.Adj[v].Remove(toMerge);
            this.Adj[v].Add(dest);
        }
        this.Adj.Remove(toMerge);

        Dictionary<Edge, int> nextEdges = [];
        this.GetVertices().ForEach((string v) =>
        {
            if (v == toMerge || v == dest) return;
            if (this.HasEdge((v, toMerge)))
            {
                nextEdges[(dest, v)] = this.GetWeight((v, toMerge));
                if (this.HasEdge((v, dest))) nextEdges[(dest, v)] += this.GetWeight((v, dest));
            }
        });
        this.Edges.Keys.ToList().ForEach(edge =>
        {
            if (edge.A == toMerge || edge.B == toMerge) return;
            if (nextEdges.ContainsKey(edge) || nextEdges.ContainsKey((edge.B, edge.A))) return;
            nextEdges[edge] = this.Edges[edge];
        });
        this.Edges = nextEdges;
        this.Combos[dest] += this.Combos[toMerge];
        this.Combos.Remove(toMerge);
    }

    public static Graph FromFile(string file)
    {
        Graph graph = new();
        File.ReadAllLines(file).ToList().ForEach(line =>
        {
            string source = line.Split(':')[0].Trim();
            List<string> dests = line.Split(':')[1].Trim().Split(' ').ToList();
            dests.ForEach(dest =>
            {
                graph.AddEdge((source, dest));
                graph.Combos[source] = 1;
                graph.Combos[dest] = 1;
                graph.Adj.TryAdd(source, []);
                graph.Adj[source].Add(dest);
                graph.Adj.TryAdd(dest, []);
                graph.Adj[dest].Add(source);
            });
        });
        return graph;
    }

    public override string ToString()
    {
        return "Num Edges: " + this.Edges.Count + "\n" + string.Join('\n', this.Edges.ToList());
    }
}