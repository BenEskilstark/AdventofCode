using System.Data;
using System.Text.RegularExpressions;

class Problem5
{
    public static void Solve()
    {
        var seeds = new List<(long, long)>();
        var maps = new List<Dictionary<long, (long, long)>>();
        foreach (string line in File.ReadAllLines("problem5/testinput.txt"))
        {
            if (seeds.Count == 0) seeds = ParseSeeds2(line);
            if (Regex.IsMatch(line, @"^\D+-to-\D\D+ map:")) maps.Insert(0, []);

            if (Regex.IsMatch(line, @"^(\d+\s)+"))
            {
                var pLine = line.Split(" ").Select(long.Parse).ToList();
                maps[0][pLine[0]] = (pLine[1], pLine[2]);
            }
        }

        var seedVals = new List<long>();
        var s = -1;
        while (true)
        {
            s++;
            var seed = (long)s;
            if (s % 10000000 == 0)
            {
                Console.WriteLine(seed);
            }
            foreach (var map in maps)
            {
                // Console.WriteLine(string.Join(", ", map));
                var seedBefore = seed;
                foreach (var ranges in map)
                {
                    if (seed >= ranges.Key && seed < ranges.Key + ranges.Value.Item2)
                    {
                        // Console.WriteLine(seed + ", " + ranges.Key);
                        seed = ranges.Value.Item1 + (seed - ranges.Key);
                        break;
                    }
                }

                // Console.WriteLine(seedBefore + ": " + seed);
            }

            // Console.WriteLine(s + ": " + seed);
            foreach (var sR in seeds)
            {
                if (seed > sR.Item1 && seed < sR.Item1 + sR.Item2)
                {
                    Console.WriteLine(s);
                    return;
                }
            }
        }
    }

    public static List<(long, long)> ParseSeeds2(string line)
    {
        var nums = line.Split(":")[1].Trim().Split(" ").Select(long.Parse).ToList();
        var seeds = new List<(long, long)>();
        for (var i = 0; i < nums.Count(); i += 2)
        {
            seeds.Add((nums[i], nums[i + 1]));
        }
        return seeds;
    }

    public static void SolvePt1()
    {
        var seeds = new List<long>();
        var maps = new List<Dictionary<long, (long, long)>>();
        foreach (string line in File.ReadAllLines("problem5/input.txt"))
        {
            if (seeds.Count == 0) seeds = ParseSeeds(line);
            if (Regex.IsMatch(line, @"^\D+-to-\D\D+ map:")) maps.Add([]);

            if (Regex.IsMatch(line, @"^(\d+\s)+"))
            {
                var pLine = line.Split(" ").Select(long.Parse).ToList();
                maps[^1][pLine[1]] = (pLine[0], pLine[2]);
            }
        }
        // foreach (var map in maps)
        // {
        //     Console.WriteLine(string.Join(", ", map.ToList()));
        // }

        foreach (var map in maps)
        {
            var nextSeeds = new List<long>();
            foreach (long seed in seeds)
            {
                var wasAdded = false;
                foreach (var ranges in map)
                {
                    if (seed >= ranges.Key && seed < ranges.Key + ranges.Value.Item2)
                    {
                        wasAdded = true;
                        nextSeeds.Add(ranges.Value.Item1 + (seed - ranges.Key));
                    }
                }
                if (!wasAdded) nextSeeds.Add(seed);
            }
            seeds = nextSeeds;
            // Console.WriteLine(string.Join(", ", seeds));
        }

        Console.WriteLine(seeds.Min());


    }

    static List<long> ParseSeeds(string line)
    {
        return line.Split(":")[1].Trim().Split(" ").Select(long.Parse).ToList();
    }
}