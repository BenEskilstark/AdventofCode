using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

class Problem3
{
    public static void Solve()
    {
        var numDict = new Dictionary<string, int>();
        var symDict = new Dictionary<string, string>();
        var neiDict = new Dictionary<string, List<int>>();

        var y = 0;
        foreach (string line in File.ReadAllLines("problem3/testinput.txt"))
        {
            Regex.Matches(line, @"\d+").ToList().ForEach(m =>
            {
                numDict[Encode(m.Index, y)] = int.Parse(m.Value);
            });
            Regex.Matches(line, @"\*").ToList().ForEach(m =>
            {
                symDict[Encode(m.Index, y)] = m.Value;
            });
            y++;
        }
        // Console.WriteLine(string.Join(", ", symDict.ToList()));

        numDict.ToList().ForEach(p =>
        {
            var pos = Neighbors(p, symDict);
            if (pos != "")
            {
                if (!neiDict.ContainsKey(pos)) neiDict[pos] = new List<int>();
                neiDict[pos].Add(p.Value);
            }
        });

        var sum = 0;
        neiDict.ToList().ForEach(p =>
        {
            // Console.WriteLine(p.Key + ": " + string.Join(", ", p.Value));
            if (p.Value.Count == 2)
            {
                var mult = 1;
                foreach (int val in p.Value) mult *= val;
                sum += mult;
            }
        });
        Console.WriteLine(sum);
    }

    public static void SolvePt1()
    {
        var numDict = new Dictionary<string, int>();
        var symDict = new Dictionary<string, string>();

        var y = 0;
        foreach (string line in File.ReadAllLines("problem3/input.txt"))
        {
            Regex.Matches(line, @"\d+").ToList()
                .ForEach(m => numDict[Encode(m.Index, y)] = int.Parse(m.Value));
            Regex.Matches(line, @"[^\d\.]").ToList()
                .ForEach(m => symDict[Encode(m.Index, y)] = m.Value);
            y++;
        }

        var sum = 0;
        numDict.Where(n => Neighbors(n, symDict) != "").ToList()
            .ForEach(v => sum += v.Value);
        Console.WriteLine(sum);
    }

    private static string Neighbors(
        KeyValuePair<string, int> pair,
        Dictionary<string, string> symDict
    )
    {
        var pos = Decode(pair.Key);
        var len = ("" + pair.Value).Length;

        var result = "";
        symDict.ToList().ForEach(p =>
        {
            var sPos = Decode(p.Key);
            if (
                Math.Abs(sPos[1] - pos[1]) <= 1 && // within 1 on y
                (Math.Abs(sPos[0] - pos[0]) <= 1 || // within 1 on x
                    (sPos[0] - pos[0] <= len && sPos[0] - pos[0] >= 0))
            )
            {
                result = p.Key;
            }
        });
        return result;
    }

    private static string Encode(int x, int y)
    {
        return x + "," + y;
    }
    private static List<int> Decode(string p)
    {
        return p.Split(",").Select(int.Parse).ToList();
    }

    private static List<string> Matches(string input, string pattern)
    {
        return Regex.Matches(input, pattern)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();
    }
}