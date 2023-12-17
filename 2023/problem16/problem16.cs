namespace Year2023;

using System.Drawing;
using Ray = ((int X, int Y) Pos, (int DX, int DY) Dir);

public class Problem16
{
    public static void Solve()
    {
        string file = "2023/problem16/input.txt";

        // part 1
        // Console.WriteLine(SimulateRay(file, ((-1, 0), (1, 0))));

        // part 2
        int max = 0;
        int size = 110;
        for (int i = 0; i < size; i++) {
            int res1 = SimulateRays(file, ((i, -1), (0, 1)));
            if (res1 > max) max = res1;
            int res2 = SimulateRays(file, ((i, size), (0, -1)));
            if (res2 > max) max = res2;
            int res3 = SimulateRays(file, ((-1, i), (1, 0)));
            if (res3 > max) max = res3;
            int res4 = SimulateRays(file, ((size, i), (-1, 0)));
            if (res4 > max) max = res4;
        }

        Console.WriteLine(max);
    }

    public static int SimulateRays(string file, Ray StartRay) { 
        Grid<char> grid = new(
            [.. File.ReadAllLines(file).Select(l => l.ToCharArray().ToList())], 
            '?'
        );
        Grid<Dictionary<(int DX, int DY), int>> energized = new(
            File.ReadAllLines(file)
                .Select((string l) => {
                    return l.ToCharArray().Select(c => new Dictionary<(int DX, int DY), int>()).ToList();
                }).ToList(),
            []
        );
        Queue<Ray> rays = [];
        rays.Enqueue(StartRay); // gotta start the ray from outside
        while (rays.Count > 0) {
            var (Pos, Dir) = rays.Dequeue();
            (int X, int Y) = (Pos.X + Dir.DX, Pos.Y + Dir.DY);
            if (X < 0 || Y < 0 || X >= grid.Width || Y >= grid.Height) continue;
            
            // mirrors:
            (int DX, int DY) = Dir;
            if (grid.At((X, Y)) == '\\') (DX, DY) = (DY, DX);
            if (grid.At((X, Y)) == '/') (DX, DY) = (-1 * DY, -1 * DX);

            EnergizePos(energized, (X, Y), (DX, DY));
            if (energized.At((X, Y))[(DX, DY)] > 1) continue; // in a loop now

            // splitters:
            if (grid.At((X, Y)) == '|' && DX != 0) {
                (DX, DY) = (0, -1);
                rays.Enqueue(((X, Y), (0, 1)));
            }
            if (grid.At((X, Y)) == '-' && DY != 0) {
                (DX, DY) = (-1, 0);
                rays.Enqueue(((X, Y), (1, 0)));
            }

            rays.Enqueue(((X, Y), (DX, DY)));
            // PrintEnergized(energized, grid); // viz
            // Console.WriteLine("");
            // Thread.Sleep(500);
        }
        // PrintEnergized(energized, grid);
        return NumEnergized(energized);
    }

    public static int NumEnergized(Grid<Dictionary<(int DX, int DY), int>> Energized) {
        int res = 0;
        Energized.ForEach((Pos, Dirs) => {
            if (Dirs.Count > 0) res++;
        });
        return res;
    }

    public static void EnergizePos(
        Grid<Dictionary<(int DX, int DY), int>> Energized,
        (int X, int Y) Pos, (int DX, int DY) Dir
    ) {
        Dictionary<(int DX, int DY), int> energy = Energized.At(Pos);
        energy.TryAdd(Dir, 0);
        energy[Dir]++;
    }


    public static void PrintEnergized(
        Grid<Dictionary<(int DX, int DY), int>> Energized,
        Grid<char> Grid
    ) {
        for (int row = 0; row < Energized.Matrix.Count; row++) {
            string rowStr = "";
            for (int col = 0; col < Energized.Matrix[row].Count; col++) {
                if (Grid.At((col, row)) != '.') {
                    rowStr += Grid.At((col, row));
                    continue;
                }
                Dictionary<(int DX, int DY), int> dirs = Energized.At((col, row));
                // if (dirs.Count == 0) {
                //     rowStr += '.';
                // } else {
                //     rowStr += '#';
                // }
                if (dirs.Count == 0) {
                    rowStr += '.';
                } else if (dirs.Count > 1) {
                    rowStr += dirs.Count;
                } else {
                    if (dirs.ContainsKey((0, 1))) rowStr += 'v';
                    if (dirs.ContainsKey((0, -1))) rowStr += '^';
                    if (dirs.ContainsKey((-1, 0))) rowStr += '<';
                    if (dirs.ContainsKey((1, 0))) rowStr += '>';
                }
            }
            Console.WriteLine(rowStr);
        }
    }
}