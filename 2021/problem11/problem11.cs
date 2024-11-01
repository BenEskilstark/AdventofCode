namespace Year2021;

using Coord = (int X, int Y);

public class Problem11
{
    public static void Solve()
    {
        string file = "2021/problem11/input.txt";
        List<List<int>> nums = File.ReadAllLines(file)
            .Select(l => l.ToCharArray().ToList().Select(c => int.Parse("" + c)).ToList())
            .ToList();
        Grid<int> octs = new(nums, -1);

        int numSteps = 1000; // should be 100 for part 1
        int numFlashes = 0;
        for (int s = 0; s < numSteps; s++)
        {
            bool didFlash = false;
            HashSet<Coord> flashed = [];
            octs = octs.Map((c, v) => v + 1);
            do
            {
                didFlash = false;
                octs.ForEach((c, v) =>
                {
                    if (v > 9 && !flashed.Contains(c))
                    {
                        didFlash = true;
                        flashed.Add(c);
                        numFlashes++;
                        octs.GetNeighbors(c, true).ForEach(c => octs.Set(c, octs.At(c) + 1));
                    }
                });
            } while (didFlash);
            if (flashed.Count == 100) // part 2
            {
                Console.WriteLine("All flashed on " + (s + 1));
                break;
            }
            octs = octs.Map((c, v) => v > 9 ? 0 : v);
        }
        Console.WriteLine(numFlashes); // part 1
    }

}