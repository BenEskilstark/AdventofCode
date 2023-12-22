namespace Year2023;

using Coord = (int X, int Y, int Z);

public class Problem22
{
    public static void Solve()
    {
        string file = "2023/problem22/input.txt";
        char name = (char)('A' - 1);
        BrickStack bricks = new([.. File.ReadAllLines(file).Select(line => {
            List<string> coords = [..line.Split('~')];
            List<int> coord1 = coords[0].Split(',').Select(int.Parse).ToList();
            List<int> coord2 = coords[1].Split(',').Select(int.Parse).ToList();
            name = (char)(name + 1);
            return new Brick(
                (coord1[0], coord1[1], coord1[2]), (coord2[0], coord2[1], coord2[2]), "" + name
            );
        })]);
        // Console.WriteLine(bricks);
        // Console.WriteLine(bricks.GetBounds());
        // Console.WriteLine(bricks.Perspective(0));

        (List<Brick> dropped, Dictionary<string, List<string>> supports) = bricks.Drop();
        bricks = new(dropped);
        // Console.WriteLine(bricks.Perspective(0));


        HashSet<string> cantRemove = ComputeCantRemove(supports);
        // Console.WriteLine(string.Join(",", cantRemove));
        long res = dropped.Count - cantRemove.Count;
        // part 1:
        Console.WriteLine(res);

        // part2:

        long total = 0;
        foreach (string b in cantRemove.ToList())
        {
            Dictionary<string, List<string>> nextSupports = supports.Where(pair => pair.Value.Count > 0).ToDictionary(); // remove bottom layer that will not fall
            // Console.WriteLine(b + " -------------------");
            Queue<string> removed = new([b]);
            res = 0;
            while (removed.TryDequeue(out string? s))
            {
                nextSupports = RemoveSupport(nextSupports, s);
                // ComputeCantRemove(nextSupports);

                nextSupports = nextSupports.Where(pair =>
                {
                    if (pair.Value.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        res++;
                        removed.Enqueue(pair.Key);
                        return false;
                    }
                }).ToDictionary();
                // Console.WriteLine(string.Join(", ", removed));
            }
            // Console.WriteLine(res);
            total += res;
        }

        Console.WriteLine(total);
    }

    public static Dictionary<string, List<string>> RemoveSupport(Dictionary<string, List<string>> supports, string toRemove)
    {
        Dictionary<string, List<string>> nextSupports = [];
        supports.ToList().ForEach(p => nextSupports[p.Key] = p.Value.Where(e => e != toRemove).ToList());
        return nextSupports;
    }

    public static HashSet<string> ComputeCantRemove(Dictionary<string, List<string>> supports)
    {
        HashSet<string> cantRemove = [];
        foreach (var pair in supports.ToList())
        {
            if (pair.Value.Count == 1) cantRemove.Add(pair.Value[0]);
            // Console.WriteLine(pair.Key + " <-- " + string.Join(",", pair.Value));
        }
        return cantRemove;
    }





}