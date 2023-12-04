
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
// Console.WriteLine("Hello, World!asdf");
// System.Diagnostics.Debug.WriteLine("hello worldf");

class Problem1
{
    public static void Solve()
    {
        Console.WriteLine(getRegexSum());
    }

    static int getSimpleSum()
    {
        var sum = 0;
        foreach (string line in File.ReadLines("input2.txt"))
        {
            var first = -1;
            var last = -1;
            foreach (char c in line.ToCharArray())
            {
                if (Char.IsNumber(c))
                {
                    if (first == -1)
                    {
                        first = (int)(c - '0');
                    }
                    else
                    {
                        last = (int)(c - '0');
                    }
                }
            }
            if (last == -1) { last = first; }
            var num = int.Parse(first + "" + last);
            sum += num;
        }
        return sum;
    }

    static int getRegexSum()
    {
        var sum = 0;
        foreach (string line in File.ReadLines(@"problem1\input2.txt"))
        {
            List<int> matches = GetRegexMatches(line);
            sum += int.Parse(matches[0] + "" + matches[^1]);
        }
        return sum;
    }

    static List<int> GetRegexMatches(string input)
    {
        string pattern = @"(?=(\d|one|two|three|four|five|six|seven|eight|nine))";

        // Get a collection of matches
        MatchCollection matches = Regex.Matches(input, pattern);

        return matches
            .Select(m => m.Groups[1].Value).Where(m => m != String.Empty).ToList()
            .ConvertAll<int>(match => {
                return match switch
                {
                    "one" => 1,
                    "two" => 2,
                    "three" => 3,
                    "four" => 4,
                    "five" => 5,
                    "six" => 6,
                    "seven" => 7,
                    "eight" => 8,
                    "nine" => 9,
                    _ => int.Parse(match),
                };
            });
    }
}

