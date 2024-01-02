using System.Data;
using Range = (char Cat, int Min, int Max); // Upper bound exclusive

namespace Year2023;

public class Problem19
{
    public static void Solve()
    {
        string file = "2023/problem19/testinput.txt";
        Dictionary<string, Workflow> workflows = [];
        Dictionary<string, WTree> wMap = [];
        List<Part> parts = [];

        // parse workflows and parts
        bool parsingWorkflows = true;
        foreach (string line in File.ReadLines(file))
        {
            if (line.Trim() == "")
            {
                parsingWorkflows = false;
                continue;
            }
            if (parsingWorkflows)
            {
                Workflow w = new(line);
                workflows[w.Label] = w;
                WTree t = new(line);
                wMap[t.Label] = t;
            }
            else
            {
                parts.Add(new(line));
            }
        }

        // Part 1:
        long sum = 0;
        foreach (Part part in parts)
        {
            string label = "in";
            while (label != "A" && label != "R")
            {
                label = workflows[label].EvalPart(part);
            }
            if (label == "A") sum += part.Sum();
        }
        // Console.WriteLine(sum);

        // Part 2:
        // Console.WriteLine(string.Join("\n", wMap.ToList()));

        BTree<int> bTree = new(5)
        {
            LeftChild = new(3),
            RightChild = new(7)
        };
        bTree.LeftChild.LeftChild = new(1);
        bTree.LeftChild.LeftChild.LeftChild = new(0);
        bTree.LeftChild.LeftChild.RightChild = new(2);
        bTree.LeftChild.RightChild = new(4);
        bTree.RightChild.LeftChild = new(6);
        bTree.RightChild.LeftChild.RightChild = new(7);
        bTree.RightChild.RightChild = new(8);
        bTree.RightChild.RightChild.RightChild = new(9);

        Console.WriteLine(string.Join(", ", bTree.Flatten()));
        Console.WriteLine(bTree);
    }


}


public class WTree
{
    public string Label { get; }
    public List<Rule> Rules { get; set; } = [];

    public WTree(string Line)
    {
        this.Label = Line.Split('{')[0];
        List<string> rules = [.. Line.Split('{')[1][..^1].Split(',')];
        foreach (string rule in rules)
        {
            if (rule == "A" || rule == "R" || !rule.Contains(':'))
            {
                this.Rules.Add(Rule.LeafRule(rule));
                continue;
            }
            char category = rule[0];
            string label = rule.Split(':')[1];
            int value = int.Parse(rule.Split(':')[0][2..]);
            this.Rules.Add(new(category, rule[1] == '<', value, label));
        }
    }

    // public BTree<Rule> ConstructTree(Dictionary<string, WTree> wMap)
    // {

    // }

    public override string ToString()
    {
        return string.Join("; ", this.Rules);
    }
}






public class Rule(char Cat, bool LessThan, int Value, string child)
{
    public char Cat { get; } = Cat;
    public bool LessThan { get; } = LessThan;
    public int Value { get; } = Value;
    public string Child { get; } = child;

    public override string ToString()
    {
        if (this.Cat == '*')
        {
            return this.Child;
        }
        return this.Cat + (this.LessThan ? " < " : " > ") + this.Value;
    }

    public static Rule LeafRule(string Label)
    {
        return new('*', Label == "R", -1, Label);
    }

    public static List<Range> FullRange()
    {
        return [('x', 1, 4001), ('m', 1, 4001), ('a', 1, 4001), ('s', 1, 4001)];
    }

    public static List<Range> EmptyRange()
    {
        return [('x', 0, 0), ('m', 0, 0), ('a', 0, 0), ('s', 0, 0)];
    }
}




// -----------------------------------------------------------------------
// Part 1 Stuff vvv
// -----------------------------------------------------------------------


public class Workflow
{
    public string Label { get; }
    List<Func<Part, string>> Rules { get; }
    public Workflow(string Line)
    {
        this.Label = Line.Split('{')[0];
        List<string> rules = [.. Line.Split('{')[1][..^1].Split(',')];

        this.Rules = rules.Select(rule => new Func<Part, string>((Part part) =>
        {
            if (rule == "A" || rule == "R" || !rule.Contains(':')) return rule;
            char category = rule[0];
            int value = int.Parse(rule.Split(':')[0][2..]);
            string label = rule.Split(':')[1];
            if (rule[1] == '<' && part.Ratings[category] < value)
            {
                return label;
            }
            if (rule[1] == '>' && part.Ratings[category] > value)
            {
                return label;
            }
            return "";
        })).ToList();
    }

    public string EvalPart(Part P)
    {
        string ret = "";
        foreach (var rule in this.Rules)
        {
            ret = rule(P);
            if (ret != "") break;
        }
        return ret;
    }

    public override string ToString()
    {
        return this.Label + " " + string.Join(" ", this.Rules);
    }
}

public class Part
{
    public Dictionary<char, int> Ratings { get; } = [];

    public Part(string Line)
    {
        List<string> categories = [.. Line[1..^1].Split(',')];
        this.Ratings['x'] = int.Parse(categories[0][2..]);
        this.Ratings['m'] = int.Parse(categories[1][2..]);
        this.Ratings['a'] = int.Parse(categories[2][2..]);
        this.Ratings['s'] = int.Parse(categories[3][2..]);
    }
    public override string ToString()
    {
        return string.Join(' ', this.Ratings.ToList());
    }
    public int Sum()
    {
        return Ratings.ToList().Select(Pair => Pair.Value).Aggregate((a, b) => a + b);
    }
}