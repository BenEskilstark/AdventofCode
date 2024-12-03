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
        int start = range.Start.IsFromEnd
            ? throw new ArgumentException("Range must have a start index")
            : range.Start.Value;
        int count = range.End.IsFromEnd
            ? throw new ArgumentException("Range must have an end index")
            : range.End.Value - start;
        return Enumerable.Range(range.Start.Value, count).ToList();
    }

    public static bool Any(this Range range, Func<int, bool> predicate)
    {
        int start = range.Start.IsFromEnd
            ? throw new ArgumentException("Range must have a start index")
            : range.Start.Value;
        int count = range.End.IsFromEnd
            ? throw new ArgumentException("Range must have an end index")
            : range.End.Value - start;
        return Enumerable.Range(range.Start.Value, count).Any(predicate);
    }

    public static List<int> Concat(this Range range, Range other)
    {
        return range.ToList().Concat(other.ToList()).ToList();
    }
}