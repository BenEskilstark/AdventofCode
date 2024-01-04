namespace Year2022;

public class Problem6
{
    public static void Solve()
    {
        string file = "2022/problem6/input.txt";
        int markerLen = 14; // 4 for part 1

        string buf = File.ReadAllText(file);
        for (int i = 0; i < buf.Length - markerLen; i++)
        {
            if (AllDifferent(buf[i..(i + markerLen)]))
            {
                Console.WriteLine(i + markerLen);
                break;
            }
        }
    }

    public static bool AllDifferent(string chars)
    {
        for (int i = 0; i < chars.Length - 1; i++)
        {
            for (int j = i + 1; j < chars.Length; j++)
            {
                if (chars[i] == chars[j]) return false;
            }
        }
        return true;
    }

}