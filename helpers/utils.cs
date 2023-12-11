public class Utils {
    public static void PrintList<T>(List<T> list) {
        Console.WriteLine(string.Join(", ", list));
    }

    public static void PrintDict<T>(Dictionary<string, T> dict) {
        Console.WriteLine(string.Join(", ", dict.ToList()));
    }
}