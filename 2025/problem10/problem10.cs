namespace Year2025;

public class Problem10
{
    public static void Solve()
    {
        string file = "2025/problem10/input.txt";
        List<List<char>> targets = [];
        List<List<Button>> buttonLists = [];
        List<List<int>> joltages = [];
        File.ReadAllLines(file)
            .ForEach(line =>
            {
                targets.Add(line.Split(']')[0][1..].ToChars());
                buttonLists.Add(line.Split(']')[1].Split('{')[0].Trim().Split(' ')
                    .Select(s => new Button(s)).ToList());
                joltages.Add(line.Split('{')[1][..^1].GetNums());
            });

        int total = 0;
        // for (int i = 0; i < targets.Count; i++)
        // {
        //     List<char> target = targets[i];
        //     List<Button> buttons = buttonLists[i];
        //     Queue<(List<char>, Button, int)> queue = new();
        //     buttons.ForEach(b => queue.Enqueue(((0..target.Count).Select(_ => '.').ToList(), b, 0)));
        //     while (queue.TryDequeue(out var item))
        //     {
        //         List<char> lights = item.Item1;
        //         Button button = item.Item2;
        //         int depth = item.Item3;
        //         List<char> newLights = button.Toggle(Copy(lights));
        //         if (Same(newLights, target))
        //         {
        //             total += depth + 1;
        //             break;
        //         }
        //         else
        //         {
        //             buttons.ForEach(b => queue.Enqueue((newLights, b, depth + 1)));
        //         }
        //     }
        // }
        // total.WriteLine("Part 1:");

        total = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            List<Button> buttons = buttonLists[i];
            List<int> jTarget = joltages[i];
            Queue<(List<int>, Button, int)> queue = new();
            buttons.ForEach(b => queue.Enqueue(((0..jTarget.Count).Select(_ => 0).ToList(), b, 0)));
            while (queue.TryDequeue(out var item))
            {
                List<int> curJolts = item.Item1;
                Button button = item.Item2;
                int depth = item.Item3;
                List<int> newJolts = button.JoltageToggle(Copy(curJolts));
                if (Same(newJolts, jTarget))
                {
                    Console.WriteLine($"{i}: {depth + 1}");
                    total += depth + 1;
                    break;
                }
                else
                {
                    buttons.ForEach(b =>
                    {
                        if (AllLesser(newJolts, jTarget))
                        {
                            queue.Enqueue((newJolts, b, depth + 1));
                        }
                    });
                }
            }
        }
        total.WriteLine("Part 2:");
    }

    public class Button(string numsFromFile)
    {
        public List<int> Toggles { get; } = numsFromFile.GetNums();

        public List<char> Toggle(List<char> lights)
        {
            Toggles.ForEach(t => lights[t] = lights[t] == '.' ? '#' : '.');
            return lights;
        }

        public List<int> JoltageToggle(List<int> joltages)
        {
            Toggles.ForEach(t => joltages[t] += 1);
            return joltages;
        }

        public override string ToString()
        {
            return $"({string.Join(", ", Toggles)})";
        }
    }

    public static List<int> Copy(List<int> original)
    {
        List<int> copy = [];
        original.ForEach(copy.Add);
        return copy;
    }

    public static List<char> Copy(List<char> original)
    {
        List<char> copy = [];
        original.ForEach(copy.Add);
        return copy;
    }

    public static bool AllLesser(List<int> a, List<int> b)
    {
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] > b[i]) return false;
        }
        return true;
    }

    public static bool Same(List<int> a, List<int> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

    public static bool Same(List<char> a, List<char> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

}