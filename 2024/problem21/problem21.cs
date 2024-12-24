namespace Year2024;

using KeyPair = (char Source, char Dest);
using MemoTuple = (char Source, char Dest, int NumTimes);
using Coord = (int X, int Y);

public class Problem21
{
    public static void Solve()
    {
        List<string> goals = [.. File.ReadAllLines("2024/problem21/input.txt")];
        Grid<char> numpad = new([
            ['7', '8', '9'],
            ['4', '5', '6'],
            ['1', '2', '3'],
            [' ', '0', 'A']
        ], ' ');
        Grid<char> dirpad = new([
            [' ', '^', 'A'],
            ['<', 'v', '>']
        ], ' ');

        Dict<KeyPair, string> paths = new("", () => "");

        List<char> keys = [.. numpad.Collect((p, v) => v != ' ').Select(numpad.At)];
        for (int i = 0; i < keys.Count; i++)
        {
            for (int j = 0; j < keys.Count; j++)
            {
                if (i == j) paths[(keys[i], keys[j])] = "A";
                paths[(keys[i], keys[j])] = ShortestPathNumPad(numpad, keys[i], keys[j]) + "A";
            }
        }

        keys = [.. dirpad.Collect((p, v) => v != ' ').Select(dirpad.At)];
        for (int i = 0; i < keys.Count; i++)
        {
            for (int j = 0; j < keys.Count; j++)
            {
                if (i == j) paths[(keys[i], keys[j])] = "A";
                paths[(keys[i], keys[j])] = ShortestPathDirPad(dirpad, keys[i], keys[j]) + "A";
            }
        }

        Dict<MemoTuple, long> memo = new(0);
        Expander E = new(memo, paths);
        goals.Sum(goal =>
        {
            string exp = string.Join("", ToPairs("A" + goal).Select(p => paths[p]));
            return E.ExpandedLen(exp, 2) * goal.GetNums()[0];
        }).WriteLine("Part 1:");

        goals.Sum(goal =>
        {
            string exp = string.Join("", ToPairs("A" + goal).Select(p => paths[p]));
            return E.ExpandedLen(exp, 25) * goal.GetNums()[0];
        }).WriteLine("Part 2:");
    }

    private class Expander(Dict<MemoTuple, long> memo, Dict<KeyPair, string> paths)
    {
        public long ExpandPair(KeyPair pair, int numTimes)
        {
            MemoTuple m = (pair.Source, pair.Dest, numTimes);
            if (memo[m] != 0) return memo[m];
            if (numTimes == 1)
            {
                memo[m] = paths[pair].Length;
                return memo[m];
            }
            memo[m] = ToPairs("A" + paths[pair])
                .Select(p => ExpandPair(p, numTimes - 1)).Sum();
            return memo[m];
        }

        public long ExpandedLen(string str, int numTimes)
        {
            return ToPairs("A" + str).Select(p => ExpandPair(p, numTimes)).Sum();
        }
    }

    public static List<KeyPair> ToPairs(string str)
    {
        List<KeyPair> pairs = [];
        for (int i = 0; i < str.Length - 1; pairs.Add((str[i], str[i + 1])), i++) { }
        return pairs;
    }

    public static string ShortestPathNumPad(Grid<char> grid, char source, char dest)
    {
        Coord start = grid.Collect((pos, val) => val == source)[0];
        Coord end = grid.Collect((pos, val) => val == dest)[0];
        Coord diff = (end.X - start.X, end.Y - start.Y);

        string horiz = diff.X == 0 ? "" : (diff.X > 0 ? ">" : "<").Times(Math.Abs(diff.X));
        string vert = diff.Y == 0 ? "" : (diff.Y > 0 ? "v" : "^").Times(Math.Abs(diff.Y));
        if (start.Y == 3 && end.X == 0) return vert + horiz;
        if (start.X == 0 && end.Y == 3) return horiz + vert;

        if (diff.X < 0) return horiz + vert;
        if (diff.X > 0) return vert + horiz;
        return vert + horiz; // order doesn't matter because one of these is ""
    }

    public static string ShortestPathDirPad(Grid<char> grid, char source, char dest)
    {
        Coord start = grid.Collect((pos, val) => val == source)[0];
        Coord end = grid.Collect((pos, val) => val == dest)[0];
        Coord diff = (end.X - start.X, end.Y - start.Y);

        string horiz = diff.X == 0 ? "" : (diff.X > 0 ? ">" : "<").Times(Math.Abs(diff.X));
        string vert = diff.Y == 0 ? "" : (diff.Y > 0 ? "v" : "^").Times(Math.Abs(diff.Y));
        if (start.X == 0) return horiz + vert;
        if (end.X == 0) return vert + horiz;

        if (diff.X < 0) return horiz + vert;
        if (diff.X > 0) return vert + horiz;
        return vert + horiz; // order doesn't matter because one of these is ""
    }
}