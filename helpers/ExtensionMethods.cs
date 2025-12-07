using System.Text.RegularExpressions;

using Coord = (int X, int Y);

public static class StringExtensions
{
    // Use like  line.GetNums().Select(n => n * n);
    // Can't handle negatives
    public static List<int> GetNums(this string line)
    {
        return Regex.Matches(line, @"\d+")
            .Select(m => int.Parse(m.Value)).ToList();
    }

    public static List<long> GetLongs(this string line)
    {
        return Regex.Matches(line, @"\d+")
            .Select(m => long.Parse(m.Value)).ToList();
    }

    // Because ToCharArray is verbose and doesn't return a list
    public static List<char> ToChars(this string line)
    {
        return line.ToCharArray().ToList();
    }

    public static string WriteLine(this string line)
    {
        Console.WriteLine(line);
        return line;
    }

    public static string Times(this string source, int times)
    {
        string res = "";
        for (int i = 0; i < times; i++)
        {
            res += source;
        }
        return res;
    }
}


public static class EnumerableExtensions
{
    // Use like  [1,2,3].WriteLine(); // prints "1, 2, 3"
    public static IEnumerable<T> WriteLine<T>(this IEnumerable<T> source)
    {
        Console.WriteLine(string.Join(", ", source.ToList()));
        return source;
    }

    // See ListExtension implementation
    public static List<List<T>> Transpose<T>(this IEnumerable<List<T>> source)
    {
        return source.ToList().Transpose();
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> predicate)
    {
        source.ToList().ForEach(predicate);
    }
}


public static class ListExtensions
{
    // in-place sort that returns void is annoying. So still sort in place
    // (because I don't want to deal with generic copying), but then just 
    // return yourself. Use like:
    // [5,4,2,1,3].FSort().Take(2); // returns [1, 2]
    public static List<T> FSort<T>(this List<T> source)
    {
        source.Sort();
        return source;
    }
    public static List<T> FSort<T>(this List<T> source, Comparison<T> comp)
    {
        source.Sort(comp);
        return source;
    }

    public static (int, int) ToTuple(this List<int> source)
    {
        return (source[0], source[1]);
    }
    public static (int, int, int) To3Tuple(this List<int> source)
    {
        return (source[0], source[1], source[2]);
    }

    // Assumes that source is a list of lists and then transposes the rows
    // and columns. So:
    // List<List<int>> foo = [[1, 2], [3, 4], [5, 6]];
    // var bar = foo.Transpose(); // is [[1, 3, 5], [2, 4, 6]]
    public static List<List<T>> Transpose<T>(this List<List<T>> source)
    {
        if (source.Count == 0) return [];
        List<List<T>> result = [.. (0..source[0].Count).Select(i => (List<T>)[])];
        for (int i = 0; i < source.Count; i++)
        {
            for (int j = 0; j < source[i].Count; j++)
            {
                result[j].Add(source[i][j]);
            }
        }
        return result;
    }

    public static string Join<T>(this List<T> source, string delim = "")
    {
        string res = "";
        for (int i = 0; i < source.Count(); i++)
        {
            res += source[i]?.ToString();
            if (i < source.Count - 1) res += delim;
        }
        return res;
    }
}


public static class IntExtensions
{
    // Use like  4.WriteLine(); // prints "4"
    public static int WriteLine(this int value, string prefix = "")
    {
        if (prefix != "" && prefix[^1] != ' ')
        {
            Console.WriteLine(prefix + " " + value);
        }
        else
        {
            Console.WriteLine(prefix + value);
        }

        return value;
    }
}

public static class LongExtensions
{
    // Use like  4.WriteLine(); // prints "4"
    public static long WriteLine(this long value, string prefix = "")
    {
        if (prefix != "" && prefix[^1] != ' ')
        {
            Console.WriteLine(prefix + " " + value);
        }
        else
        {
            Console.WriteLine(prefix + value);
        }

        return value;
    }
}

public static class RangeExtensions
{
    public static bool InRange(this Range range, int value)
    {
        return value >= range.Start.Value && value < range.End.Value;
    }

    // Use like  (0..10).ToList().Select(i => i * i);
    public static List<int> ToList(this Range range)
    {
        return range.ToEnumerable().ToList();
    }

    // To more efficiently use the other enumerable methods without having
    // to implement them here
    public static IEnumerable<int> ToEnumerable(this Range range)
    {
        int start = range.Start.IsFromEnd
            ? throw new ArgumentException("Range must have a start index")
            : range.Start.Value;
        int count = range.End.IsFromEnd
            ? throw new ArgumentException("Range must have an end index")
            : range.End.Value - start;
        return Enumerable.Range(range.Start.Value, count);
    }

    public static void ForEach(this Range range, Action<int> predicate)
    {
        range.ToList().ForEach(predicate);
    }

    public static bool Any(this Range range, Func<int, bool> predicate)
    {
        return range.ToEnumerable().Any(predicate);
    }
    public static bool All(this Range range, Func<int, bool> predicate)
    {
        return range.ToEnumerable().All(predicate);
    }
    public static int Sum(this Range range)
    {
        return range.ToEnumerable().Sum();
    }
    public static int Sum(this Range range, Func<int, int> predicate)
    {
        return range.ToEnumerable().Sum(predicate);
    }
    public static IEnumerable<T> Select<T>(this Range range, Func<int, T> selector)
    {
        return range.ToEnumerable().Select(selector);
    }
    public static IEnumerable<T> Select<T>(this Range range, Func<int, int, T> selector)
    {
        return range.ToEnumerable().Select(selector);
    }

    public static List<int> Concat(this Range range, Range other)
    {
        return range.ToEnumerable().Concat(other.ToEnumerable()).ToList();
    }
}