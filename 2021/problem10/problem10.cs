namespace Year2021;

public class Problem10
{
    public static void Solve()
    {
        string file = "2021/problem10/input.txt";

        int totalCorruption = 0;
        List<long> scores = [];
        foreach (string line in File.ReadAllLines(file))
        {
            Stack<char> brackets = [];
            bool isCorrupted = false;
            foreach (char c in line.ToCharArray().ToList())
            {
                if (c == '(' || c == '{' || c == '<' || c == '[')
                {
                    brackets.Push(c);
                }
                else
                {
                    char open = brackets.Pop();
                    if (c != MatchingBracket(open))
                    {
                        isCorrupted = true;
                        totalCorruption += c switch
                        {
                            ')' => 3,
                            ']' => 57,
                            '}' => 1197,
                            '>' => 25137,
                            _ => throw new Exception("Not a closing bracket")
                        };
                        break;
                    }
                }
            }
            long curScore = 0;
            while (brackets.TryPop(out char open))
            {
                curScore *= 5;
                curScore += open switch
                {
                    '(' => 1,
                    '[' => 2,
                    '{' => 3,
                    '<' => 4,
                };
            }
            if (!isCorrupted && curScore > 0) scores.Add(curScore);
            if (curScore <= 0)
            {
                Console.WriteLine(line + " " + curScore);
            }
        }

        Console.WriteLine(totalCorruption);

        // Console.WriteLine(string.Join(", ", scores));
        scores.Sort();
        Console.WriteLine(scores.Count);
        Console.WriteLine(scores[scores.Count / 2]);
    }

    public static char MatchingBracket(char c)
    {
        return c switch
        {
            '(' => ')',
            ')' => '(',
            '[' => ']',
            ']' => '[',
            '<' => '>',
            '>' => '<',
            '{' => '}',
            '}' => '}',
            _ => throw new Exception("not an opening bracket")
        };
    }

}