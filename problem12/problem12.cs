using System.Text.RegularExpressions;

public class Problem12
{

    public static void SolvePt2()
    {
        long sum = 0;
        foreach (string line in File.ReadLines("problem12/input.txt"))
        {
            string input = line.Split(" ")[0];
            List<int> segments = line.Split(" ")[1].Split(",").Select(int.Parse).ToList();

            string slowInput = "";
            for (int i = 0; i < 5; i++)
            {
                slowInput += input;
                if (i < 4) slowInput += "?";
            }

            List<int> slowSegments = [];
            for (int i = 0; i < 5; i++)
            {
                slowSegments = slowSegments.Concat(segments).ToList();
            }

            // Console.WriteLine(slowInput);
            // Utils.PrintList(slowSegments);

            sum += Compute(slowInput, slowSegments);
        }
        Console.WriteLine(sum);
    }

    private static long Compute(string Input, List<int> Blocks)
    {
        Dictionary<(int, int, int), long> cache = [];

        long res = ComputeHelper(Input, Blocks, cache, 0, 0, 0);
        // Console.WriteLine(Input + " " + res);
        return res;
    }

    private static long ComputeHelper(
        string Input, List<int> Blocks,
        Dictionary<(int, int, int), long> Cache,
        int Index, int BlockIndex, int BlockLength
    )
    {
        (int, int, int) key = (Index, BlockIndex, BlockLength);
        if (Cache.TryGetValue(key, out long val)) return val;

        if (Index == Input.Length) // we are done
        {
            // Console.WriteLine(Index + " " + BlockIndex + " " + BlockLength);
            if (BlockIndex == Blocks.Count && BlockLength == 0)
            {
                return 1;
            }
            else if (BlockIndex == Blocks.Count - 1 && Blocks[BlockIndex] == BlockLength)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        long ans = 0;
        foreach (char c in new List<char>(['.', '#']))
        {
            if (Input[Index] == c || Input[Index] == '?')
            {
                if (c == '.' && BlockLength == 0)
                { // simply go to the next
                    ans += ComputeHelper(Input, Blocks, Cache, Index + 1, BlockIndex, 0);
                }
                else if (c == '.' && BlockLength > 0 && BlockIndex < Blocks.Count && Blocks[BlockIndex] == BlockLength)
                { // going to the next block
                    ans += ComputeHelper(Input, Blocks, Cache, Index + 1, BlockIndex + 1, 0);
                }
                else if (c == '#')
                { // incrementing within block
                    ans += ComputeHelper(Input, Blocks, Cache, Index + 1, BlockIndex, BlockLength + 1);
                }
            }
        }

        Cache[key] = ans;
        return ans;
    }










































































    public static void SolvePt2Bad()
    {

        List<string> inputs = [];
        List<string> regexes = [];
        List<string> slowInputs = [];
        List<string> slowRegexes = [];

        foreach (string line in File.ReadLines("problem12/input.txt"))
        {
            string input = line.Split(" ")[0];
            inputs.Add(input);
            string pattern = line.Split(" ")[1];
            regexes.Add(BuildRegexLong(pattern));
            string slowPattern = "";
            for (int i = 0; i < 5; i++)
            {
                slowPattern += pattern;
                if (i < 4) slowPattern += ",";
            }
            slowRegexes.Add(BuildRegexLong(slowPattern));
            string slowInput = "";
            for (int i = 0; i < 5; i++)
            {
                slowInput += input;
                if (i < 4) slowInput += "?";
            }
            slowInputs.Add(slowInput);
        }


        long sum = 0;
        long sum2 = 0;
        for (int i = 0; i < inputs.Count; i++)
        {
            Dictionary<(string, string), long> cache = [];
            // long res = ExpandedNumValidStrings(inputs[i], regexes[i], slowRegexes[i], i);
            long res = NumValidStringsRec2(cache, inputs[i], inputs[i], regexes[i], regexes[i], 0, 0);
            long res2 = NumValidStrings(inputs[i], regexes[i]);
            sum2 += res2;
            if (res != res2)
            {
                Console.WriteLine((i + 1) + ": " + res + " " + res2);
            }
            sum += res;
        }

        Console.WriteLine(sum + " " + sum2);
    }

    private static long NumValidStringsRec2(
        Dictionary<(string, string), long> Cache, string Input, string WholeInput, string Pattern, string WholePattern, int cur, int depth
    )
    {
        string depthStr = new string('\t', depth);
        // Console.WriteLine(depthStr + WholeInput);
        if (Input.Length == 0 || Pattern.Length == 0) return 0;
        if (Cache.TryGetValue((Input, Pattern), out long ans))
        {
            // Console.WriteLine(depthStr + "Know this already! " + Input + " " + Pattern + " " + ans);
            return ans;
        }

        long sum = 0;
        if (!WholeInput.Contains('?')) // base case
        {
            // Console.WriteLine(depthStr + "Done: " + WholeInput + " " + WholePattern + " ==> " + (Regex.Match(WholeInput, WholePattern).Length == WholeInput.Length ? 1 : 0));
            if (Regex.Match(WholeInput, WholePattern).Length == WholeInput.Length)
            {
                sum = 1;
            }
            else
            {
                sum = 0;
            }
        }
        else
        {
            Regex regex = new(@"\?");
            foreach (string rep in new List<string>([".", "#"]))
            {
                if (rep == "." && Pattern[0] == '#') continue;
                if (rep == "#" && Pattern[2] == '+') continue;
                string replaced = regex.Replace(Input, rep, 1);
                string wholeReplaced = regex.Replace(WholeInput, rep, 1);
                if (!PartiallyValid(replaced, Pattern))
                {
                    // Console.WriteLine(depthStr + "dead branch " + wholeReplaced + " " + WholePattern);
                    continue;
                };

                (string rest, string restPattern) = (replaced, Pattern);
                if (rest.Contains('?'))
                {
                    (rest, restPattern) = GetRestOfInput(replaced, Pattern);
                }
                // Console.WriteLine(depthStr + rep + "" + (rest, restPattern));

                sum += NumValidStringsRec2(Cache, rest, wholeReplaced, restPattern, WholePattern, rep == "#" ? cur + 1 : cur, depth + 1);
            }
        }

        Cache[(Input, Pattern)] = sum;
        return sum;
    }

    private static (string, string) GetRestOfInput(string Input, string Pattern)
    {
        string rest = "";
        string restPattern = "";

        string start = Input.Split('?')[0];
        int numHashes = start.Count(x => x == '#');
        int nH = 0;
        int i = 0;
        while (nH < numHashes && i < Pattern.Count())
        {
            if (Pattern[i] == '#') nH++;
            i++;
        }

        rest = Input.Substring(Input.IndexOf('?') != -1 ? Input.IndexOf('?') : 0);
        restPattern = Pattern.Substring(i);
        if (restPattern.Length > 2 && restPattern[2] == '+' && start[^1] != '#')
        {
            restPattern = restPattern[0..2] + '*' + restPattern[3..];
        }

        return (rest, restPattern);
    }
























    private static long NumValidStrings(string Input, string Pattern)
    {
        Stack<(string, string)> toCheck = new([(Input, Pattern)]);
        Regex regex = new(@"\?");
        long sum = 0;
        Dictionary<(string, string), long> cache = [];
        while (toCheck.TryPop(out (string, string) popped))
        {
            string next = popped.Item1;
            string pattern = popped.Item2;
            if (!next.Contains('?')) // base case
            {
                if (Regex.Match(next, pattern).Length == Input.Length)
                {
                    if (next.Count() == Input.Count())
                    {
                        sum += 1;
                    }
                    else
                    {
                        cache[(next, pattern)] = 1;
                    }
                }
            }
            else if (PartiallyValid(next, pattern))
            {
                // (string rest, string restPattern) = GetRestOfInput(next, pattern);
                // if (cache.TryGetValue((rest, restPattern), out long total))
                // {
                //     sum += total;
                // }
                // else
                // {
                //     cache[(rest, restPattern)] = NumValidStringsRec(rest, restPattern);
                // }
                foreach (string rep in new List<string>([".", "#"]))
                {
                    string replaced = regex.Replace(next, rep, 1);
                    toCheck.Push((replaced, pattern));
                }
            }
            // else
            // {
            //     foreach (string rep in new List<string>([".", "#"]))
            //     {
            //         string replaced = regex.Replace(next, rep, 1);
            //         if (!PartiallyValid(replaced, pattern)) continue;

            //         toCheck.Push((replaced, pattern));
            //     }
            // }
        }
        return sum;
    }


    private static bool PartiallyValid(string Part, string Pattern)
    {
        string start = Part.Split('?')[0];
        int numHashes = start.Count(x => x == '#');
        int nH = 0;
        string startPattern = "";
        int i = 0;
        while (nH < numHashes && i < Pattern.Count())
        {
            startPattern += Pattern[i];
            if (Pattern[i] == '#') nH++;
            i++;
        }
        return Regex.Match(start, startPattern).Success;
    }



    public static string BuildRegexLong(string ValString)
    {
        List<string> vals = ValString.Split(",").ToList();
        string regex = @"\.*";
        for (int i = 0; i < vals.Count; i++)
        {
            for (int j = 0; j < int.Parse(vals[i]); j++)
            {
                regex += @"#";
            }

            if (i < vals.Count - 1)
            {
                regex += @"\.+";
            }
        }
        regex += @"\.*";
        return regex;
    }







    /// <summary>
    /// /////////////////////////////////////////////////////////////////////
    ///  Discarded attempts
    /// /////////////////////////////////////////////////////////////////////
    /// </summary>


    private static long ExpandedNumValidStrings(string Original, string Pattern, string SlowPattern, int I)
    {
        long origWays = NumValidStrings(Original, Pattern);
        string otherInput = Original;
        if (Original[^1] == '?')
        {
            otherInput = Original + "?";
        }
        else if (Original[^1] == '#')
        {
            otherInput = Original + "?";
        }
        else if (Original[^1] == '.')
        {
            otherInput = "?" + Original;
        }
        else
        {
            Console.WriteLine("???? " + Original);
        }
        long otherWays = NumValidStrings(otherInput, Pattern);
        long result = origWays * (long)Math.Pow(otherWays, 4);

        // string slowInput = "";
        // for (int i = 0; i < 5; i++)
        // {
        //     slowInput += Original;
        //     if (i < 4) slowInput += "?";
        // }

        // long slowResult = 0;
        // if (Original == "?###????????")
        // {
        //     slowResult = 506250;
        // }
        // else
        // {
        //     slowResult = NumValidStrings(slowInput, SlowPattern);
        // }

        // string eq = slowResult != result ? @" !!!!!!!!" : @"";
        // Console.WriteLine((I + 1) + ": " + result + " " + slowResult + eq);
        return result;
    }






    /// <summary>
    /// /////////////////////////////////////////////////////////////////////
    ///  Part 1:
    /// /////////////////////////////////////////////////////////////////////
    /// </summary>

    public static void SolvePt1()
    {
        List<string> inputs = [];
        List<string> regexes = [];
        foreach (string line in File.ReadLines("problem12/input.txt"))
        {
            inputs.Add(line.Split(" ")[0]);
            regexes.Add(BuildRegexLong(line.Split(" ")[1]));
        }

        long sum = 0;
        for (int i = 0; i < inputs.Count; i++)
        {
            string input = inputs[i];
            string regex = regexes[i];
            long res = NumValidStrings(input, regex);
            // Console.WriteLine(res);
            sum += res;
        }

        Console.WriteLine(sum);

    }

    // recursive version:
    private static long NumValidStringsRec(string Input, string Pattern)
    {
        long sum = 0;
        if (!Input.Contains('?')) // base case
        {
            if (Regex.Match(Input, Pattern).Length == Input.Length)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        Regex regex = new Regex(@"\?");
        sum += NumValidStringsRec(
            regex.Replace(Input, ".", 1), Pattern
        ) + NumValidStringsRec(
            regex.Replace(Input, "#", 1), Pattern
        );

        return sum;
    }

    public static string BuildRegex(string ValString)
    {
        List<string> vals = ValString.Split(",").ToList();
        string regex = @"\.*";
        for (int i = 0; i < vals.Count; i++)
        {
            regex += @"#{" + vals[i] + @"}";
            if (i < vals.Count - 1)
            {
                regex += @"\.+";
            }
        }
        regex += @"\.*";
        return regex;
    }
}