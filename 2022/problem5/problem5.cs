namespace Year2022;
using System.Text.RegularExpressions;
public class Problem5
{
    public static void Solve()
    {
        string file = "2022/problem5/input.txt";
        int numStacks = 9;

        List<Dequeue<char>> stacks = [];
        for (int i = 0; i < numStacks; i++)
        {
            stacks.Add(new Dequeue<char>());
        }
        bool doneWithStack = false;
        foreach (string line in File.ReadLines(file))
        {
            if (line == "") doneWithStack = true;
            if (line == "" || (line.Length > 1 && line[1] == '1')) continue;
            if (!doneWithStack)
            {
                for (int i = 1; i < line.Length; i += 4)
                {
                    if (line[i] != ' ') stacks[i / 4].Enqueue(line[i]);
                }
            }
            else
            {
                MatchCollection matches = Regex.Matches(line, @"\d+");
                List<int> nums = [.. Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value))];
                // part 1
                // for (int j = 0; j < nums[0]; j++)
                // {
                //     char val = stacks[nums[1] - 1].Pop();
                //     stacks[nums[2] - 1].Push(val);
                // }
                // part 2
                List<char> vals = stacks[nums[1] - 1].PopMany(nums[0]);
                stacks[nums[2] - 1].PushMany(vals);
            }
        }

        foreach (Dequeue<char> stack in stacks)
        {
            Console.WriteLine(stack);
        }

        string res = "";
        foreach (Dequeue<char> stack in stacks)
        {
            res += stack.First();
        }

        Console.WriteLine(res);
    }
}