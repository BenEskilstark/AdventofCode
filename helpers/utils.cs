public class Utils {
    public static void PrintList<T>(List<T> list) {
        Console.WriteLine(string.Join(", ", list));
    }

    public static void PrintDict<T>(Dictionary<string, T> dict) {
        Console.WriteLine(string.Join(", ", dict.ToList()));
    }

    public static string StackStrings(string string1, string string2)
    {
        // Split the two strings into arrays of lines
        string[] lines1 = string1.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        string[] lines2 = string2.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        // Find the length of the longest line in the first string
        int longestLine1 = lines1.Max(line => line.Length);

        // Compute the padding needed for each line in the second string
        int padding = longestLine1 + 1; // Adding one additional space as per specification

        // Prepare the resulting string builder for efficiency
        System.Text.StringBuilder result = new System.Text.StringBuilder();

        // Iterate over the lines to concatenate them side by side
        for (int i = 0; i < Math.Max(lines1.Length, lines2.Length); i++)
        {
            if (i < lines1.Length)
            {
                result.Append(lines1[i]);
            }

            // Pad the remaining space for the line from the second string
            result.Append(new string(' ', padding - (i < lines1.Length ? lines1[i].Length : 0)));

            if (i < lines2.Length)
            {
                result.Append(lines2[i]);
            }

            // Unless we are on the last line, add a line break
            if (i < Math.Max(lines1.Length, lines2.Length) - 1)
            {
                result.AppendLine();
            }
        }

        return result.ToString();
    }
}