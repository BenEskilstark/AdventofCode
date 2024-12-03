using System.Text.RegularExpressions;


public static class StringExtensions
{
    // Use like  line.GetNums().Select(n => n * n);
    public static List<int> GetNums(this string line)
    {
        return Regex.Matches(line, @"\d+")
            .Select(m => int.Parse(m.Value)).ToList();
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


public static class RangeExtensions
{
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

    public static bool Any(this Range range, Func<int, bool> predicate)
    {
        return range.ToEnumerable().Any(predicate);
    }
    public static int Sum(this Range range)
    {
        return range.ToEnumerable().Sum();
    }
    public static int Sum(this Range range, Func<int, int> predicate)
    {
        return range.ToEnumerable().Sum(predicate);
    }

    public static List<int> Concat(this Range range, Range other)
    {
        return range.ToEnumerable().Concat(other.ToEnumerable()).ToList();
    }
}