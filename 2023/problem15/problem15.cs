using System.Security.Cryptography;

namespace Year2023;
using Label = string;
using Index = int;
using Lens = (string Label, int Val);

public class Problem15
{
    public static void Solve()
    {
        string file = "2023/problem15/input.txt";
        List<Step> steps = [.. File.ReadAllText(file).Split(',').Select(s => new Step(s))];

        List<List<Lens>> boxes = [];
        for (int i = 0; i < 256; i++)
        {
            boxes.Add([]);
        }
        Dictionary<Label, Index> boxMap = [];

        foreach (Step step in steps)
        {
            if (step.Op == '-') DoRemove(boxes, boxMap, step);
            if (step.Op == '=') DoAdd(boxes, boxMap, step);
            // Console.WriteLine(step);
            // PrintBoxes(boxes);
            // Console.WriteLine("");
        }

        long res = 0;
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = 0; j < boxes[i].Count; j++)
            {
                res += (i + 1) * (j + 1) * boxes[i][j].Val;
            }
        }
        Console.WriteLine(res);
    }

    public static void PrintBoxes(List<List<Lens>> Boxes)
    {
        for (int i = 0; i < Boxes.Count; i++)
        {
            List<Lens> box = Boxes[i];
            if (box.Count > 0)
            {
                Console.WriteLine("Box " + i + ": " + string.Join(" ", box));
            }
        }
    }

    public static void DoAdd(
        List<List<Lens>> Boxes, Dictionary<Label, int> BoxMap, Step S
    )
    {
        List<Lens> box = Boxes[S.GetBox()];
        if (BoxMap.TryGetValue(S.Label, out Index lensIndex))
        {
            box[lensIndex] = (S.Label, S.Val);
        }
        else
        {
            box.Add((S.Label, S.Val));
            BoxMap[S.Label] = box.Count - 1;
        }
    }

    public static void DoRemove(
        List<List<Lens>> Boxes, Dictionary<Label, int> BoxMap, Step S
    )
    {
        List<Lens> box = Boxes[S.GetBox()];
        if (BoxMap.TryGetValue(S.Label, out Index lensIndex))
        {
            box.RemoveAt(lensIndex);
            BoxMap.Remove(S.Label);
            for (int i = lensIndex; i < box.Count; i++)
            {
                BoxMap[box[i].Label]--;
            }
        }
    }

}

public class Step
{
    public Label Label { get; set; }
    public char Op { get; set; }
    public int Val { get; set; } = -1;
    private string Input { get; set; }
    public Step(string step)
    {
        if (step[^1] == '-')
        {
            this.Label = step[..(step.Length - 1)];
            this.Op = '-';
        }
        else
        {
            this.Label = step[..(step.Length - 2)];
            this.Op = '=';
            this.Val = (int)Char.GetNumericValue(step[^1]);
        }
        this.Input = step;
    }


    public int GetBox()
    {
        int sum = 0;
        foreach (char c in this.Label.ToCharArray())
        {
            sum += (int)c;
            sum *= 17;
            sum %= 256;
        }
        return sum;
    }

    public override string ToString()
    {
        //return this.Label + " " + this.Op + " " + this.Val;
        return this.Input;
    }
}