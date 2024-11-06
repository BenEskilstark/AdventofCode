namespace Year2021;

public class Problem14
{
    public static void Solve()
    {
        string file = "2021/problem14/testinput.txt";

        string polymer = "";
        Dictionary<string, string> replacements = [];
        bool onTemplate = true;
        foreach (var line in File.ReadAllLines(file))
        {
            if (line == "")
            {
                onTemplate = false;
                continue;
            }
            if (onTemplate)
            {
                polymer = line;
            }
            else
            {
                var pair = line.Split(" -> ");
                replacements.Add(pair[0], pair[1]);
            }
        }

        // part 1
        int numSteps = 3;
        for (int s = 0; s < numSteps; s++)
        {
            string nextPolymer = "";
            for (int i = 0; i < polymer.Length - 1; i++)
            {
                nextPolymer += polymer[i]
                    + replacements.GetValueOrDefault("" + polymer[i] + polymer[i + 1], "");
            }
            polymer = nextPolymer + polymer[^1];
        }
        Dictionary<char, long> freq = [];
        foreach (char c in polymer)
        {
            freq.TryAdd(c, 0);
            freq[c]++;
        }
        Console.WriteLine(string.Join("\n", freq));
        Console.WriteLine(
            freq.Max(pair => pair.Value) -
            freq.Min(pair => pair.Value)
        );
        // Console.WriteLine(polymer);


        // part 2
        polymer = File.ReadLines(file).First();
        Dictionary<string, long> polyPairs = [];
        for (int i = 0; i < polymer.Length - 1; i++)
        {
            polyPairs.TryAdd("" + polymer[i] + polymer[i + 1], 1);
        }

        // numSteps = 2;
        for (int s = 0; s < numSteps; s++)
        {
            Dictionary<string, long> nextPolyPairs = [];
            foreach (string pair in polyPairs.Keys)
            {
                string next = replacements.GetValueOrDefault(pair, "");
                if (next != "")
                {
                    nextPolyPairs.TryAdd(pair, polyPairs[pair]);
                    nextPolyPairs[pair]--; // original pair no longer occurs here

                    nextPolyPairs.TryAdd(pair[0] + next, polyPairs.GetValueOrDefault(pair[0] + next, 0));
                    nextPolyPairs[pair[0] + next]++;
                    nextPolyPairs.TryAdd(next + pair[1], polyPairs.GetValueOrDefault(next + pair[1], 0));
                    nextPolyPairs[next + pair[1]]++;
                }
            }
            polyPairs = [];
            foreach (string key in nextPolyPairs.Keys)
            {
                if (nextPolyPairs[key] != 0)
                {
                    polyPairs[key] = nextPolyPairs[key];
                }
            }
            Console.WriteLine(string.Join("\n", polyPairs));
            Console.WriteLine("");
        }


        freq = [];
        foreach (string key in polyPairs.Keys)
        {
            freq.TryAdd(key[0], 0);
            freq[key[0]] += polyPairs[key];
        }
        freq.TryAdd(polymer[^1], 0);
        freq[polymer[^1]]++;
        Console.WriteLine(string.Join("\n", freq));
    }

}