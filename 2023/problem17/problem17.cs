namespace Year2023;

using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Coord = (int X, int Y);

public class Problem17 {
    public static void Solve() {
        string file = "2023/problem17/input.txt";
        PathFinder pathFinder = new([..File.ReadAllLines(file)]);
        pathFinder.Run();
        (long score, PathNode? path) = pathFinder.GetBestPath();
        if (path != null) {
            // Console.WriteLine(Utils.StackStrings(pathFinder.PrintPath(path), pathFinder.World.ToString()));
            Console.WriteLine(pathFinder.PrintPath(path));
        }
        // Utils.PrintList(pathFinder.FinishedPaths);
        Console.WriteLine(score);
    }
}

public class PathFinder {
    public Coord Start {get;} = (0,0);
    public Coord Dest {get;}
    public Grid<int> World { get;}
    public Dictionary<(int, int, char, int), PathNode> PathMap {get;}
    public int MaxSameDir { get; set; } = 10; // 3 for part 1
    public int MinSameDir {get; set; } = 3; // 0 for part 1
    public Queue<PathNode> PathEnds {get; private set;} = [];
    public List<PathNode> FinishedPaths {get; private set;} = [];

    public PathFinder(List<string> Data) {
        this.World = new([..Data.Select(l => l.Select(c => (int)Char.GetNumericValue(c)).ToList())], 999);
        this.Dest = (this.World.Width - 1, this.World.Height - 1);
        
        PathNode start = new(null, (0, 0), 0); // starting heat doesn't count
        this.PathMap = new(){[(0,0, '.', 0)] = start};
        this.PathEnds.Enqueue(start);
    }

    public Boolean AllPathsFinished() {
        foreach (PathNode p in this.PathEnds) {
            if (p.Pos != this.Dest) return false;
        }
        return true;
    }

    public (long, PathNode?) GetBestPath() {
        long min = 1000000000000000;
        PathNode? path = null;
        foreach (PathNode p in this.FinishedPaths) {
            if (p.GetScore() < min) {
                min = p.GetScore();
                path = p;
            }
        }
        return (min, path);
    }

    public void Run() {
        int i = 0;
        while(this.PathEnds.TryDequeue(out PathNode? path)) {
            // Console.WriteLine(this.PrintPath(path) + path.GetScore() + "\n");
            List<PathNode> nextPaths = [];
            if (path.Pos == this.Dest && path.NumSameDirection() > this.MinSameDir) {
                this.FinishedPaths.Add(path);
                continue;
            }
            List<PathNode> branches = GetPathBranches(path);
            List<PathNode> prunedBranches = PruneBranches(branches);
            prunedBranches.ForEach(this.PathEnds.Enqueue);
            if (i % 1000 == 0) {
                List<PathNode> sortedNodes = this.PathEnds.ToList().OrderBy(p => p.GetScore()).ToList();
                this.PathEnds = new(sortedNodes);
            }
            
            i++;
        }
    }

    private List<PathNode> PruneBranches(List<PathNode> Branches) {
        List<PathNode> prunedBranches = [];
        foreach (PathNode node in Branches) {
            Boolean continuePath = true;
            if (PathMap.TryGetValue(node.ToTuple(), out PathNode? otherNode)) {
                if (node.GetScore() < otherNode.GetScore()) {
                    otherNode.Previous = node.Previous;
                    otherNode.Direction = node.Direction;
                    prunedBranches.Add(otherNode);
                }
                continuePath = false;
            } else {
                this.PathMap[node.ToTuple()] = node;
            }

            if (continuePath) {
                prunedBranches.Add(node);
            }
        }
        return prunedBranches;
    }

    private List<PathNode> GetPathBranches(PathNode Node) {
        List<Coord> next = [];
        (int x, int y) = Node.Pos;
        int w = this.World.Width;
        int h = this.World.Height;
        char d = Node.Direction;
        int n = Node.NumSameDirection();
        int min = this.MinSameDir;
        // in bounds and not doubling back
        if (d != '<' && x < w - 1 && (d == '>' || n > min || d == '.')) next.Add((1, 0));
        if (d != '>' && x > 0 && (d == '<' || n > min || d == '.')) next.Add((-1, 0));
        if (d != '^' && y < h - 1 && (d == 'v' || n > min || d == '.')) next.Add((0, 1));
        if (d != 'v' && y > 0 && (d == '^' || n > min || d == '.')) next.Add((0, -1));

        next = next.Select(p => (x + p.X, y + p.Y)).ToList();

        // not going more than max in a row
        List<PathNode> nextNodes = [];
        foreach(Coord p in next) {
            PathNode node = new(Node, p, this.World.At(p));
            if (node.NumSameDirection() > this.MaxSameDir) continue;
            nextNodes.Add(node);
        }
        return nextNodes;
    }

    public string PrintPath(PathNode Path) {
        List<List<char>> worldPath = [];
        for (int y = 0; y < this.World.Height; y++) {
            List<char> row = [];
            for (int x = 0; x < this.World.Width; x++) {
                // row.Add(this.World.At((x, y)).ToString()[0]);
                row.Add(' ');
            }
            worldPath.Add(row);
        }

        foreach (PathNode node in Path.GetPath()) {
            worldPath[node.Pos.Y][node.Pos.X] = node.Direction;
        }

        return new Grid<char>(worldPath, '?').ToString();
    }
}

public class PathNode {
    public int Heat { get; }
    public Coord Pos {get;} = (0, 0);
    public PathNode? Previous {get; set;} 
    public char Direction {get; set;} = '.';
    public PathNode(PathNode? Prev, Coord P, int H) {
        this.Pos = P;
        this.Previous = Prev;
        this.Heat = H;
        if (Prev != null) {
            (int DX, int DY) = (P.X - Prev.Pos.X, P.Y - Prev.Pos.Y);
            if (DX == 1) this.Direction = '>';
            if (DX == -1) this.Direction = '<';
            if (DY == 1) this.Direction = 'v';
            if (DY == -1) this.Direction = '^';
        }
    }

    public long GetScore() {
        // This looks cleaner but runs slower!
        // return this.GetPath().Select(p => p.Heat).Aggregate((a,b) => a + b);
        long score = 0;
        for (PathNode? ptr = this; ptr != null; ptr = ptr.Previous) {
            score += ptr.Heat;
        }
        return score;
    }

    public int NumSameDirection() {
        int res = 0;
        PathNode? ptr = this;
        while (ptr != null && ptr.Direction == this.Direction) {
            ptr = ptr.Previous;
            res++;
        }
        return res;
    }

    public List<PathNode> GetPath() {
        List<PathNode> path = [this];
        PathNode? ptr = this.Previous;
        while (ptr != null) {
            path.Add(ptr);
            ptr = ptr.Previous;
        }
        return path;
    }

    public (int, int, char, int) ToTuple() {
        return (this.Pos.X, this.Pos.Y, this.Direction, this.NumSameDirection());
    }

    public override string ToString() {
        return this.Pos + " " + this.Direction + " " + this.GetScore();
    }
}