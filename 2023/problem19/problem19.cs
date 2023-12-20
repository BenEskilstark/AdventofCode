using System.Data;
using System.Reflection.Emit;
using Range = (char Cat, int Min, int Max); // Upper bound exclusive

namespace Year2023;

/*@
    TODO:
        - Implement InvertRange (AndRanges might be broken)
        - Implement OrRange
        - Recursion algo:
            - walk down the children
                - Invert the previous rule and "And" it with the current rule
                - "And" the current rule with the result of recursively evaluating the child
                - if it is A or R, then just use that range
                - if it is a direct link then just recurse into that one
                - range that results in from all these Ands is the answer
@*/

public class Problem19
{
    public static void Solve()
    {
        string file = "2023/problem19/testinput.txt";
        Dictionary<string, Workflow> workflows = [];
        Dictionary<string, WTree> wTrees = [];
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
                wTrees[t.Label] = t;
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
        Console.WriteLine(wTrees["in"].ComputeChildRanges(wTrees));
    }


}


public class WTree
{
    public string Label { get; }
    public Dictionary<string, Rule> Children = [];
    public List<Rule> Rules { get; set; } = [];
    public bool Computed { get; set; } = false;

    public WTree(string Line)
    {
        this.Label = Line.Split('{')[0];
        List<string> rules = [.. Line.Split('{')[1][..^1].Split(',')];
        foreach (string rule in rules)
        {
            if (rule == "A" || rule == "R" || !rule.Contains(':'))
            {
                this.Children[rule] = Rule.LeafRule(rule);
                continue;
            }
            char category = rule[0];
            string label = rule.Split(':')[1];
            int value = int.Parse(rule.Split(':')[0][2..]);
            this.Children[label] = new(category, rule[1] == '<', value, label);
        }
    }

    public override string ToString()
    {
        return string.Join(";", this.Rules);
    }

    public List<Range> ComputeChildRanges(Dictionary<string, WTree> Dict)
    {
        List<Range> ranges = Rule.FullRange();
        foreach (var child in this.Children.ToList())
        {
            Rule rule = child.Value;
            if (child.Key == "A" || child.Key == "R")
            {
                ranges = Rule.AndRanges(ranges, rule.Ranges);
            }
            else if (rule.Cat != '?')
            {

            }
        }
        return ranges;
    }
}






public class Rule
{
    public char Cat { get; }
    public bool LessThan { get; }
    public int Value { get; }
    public string Child { get; }
    public List<Range> Ranges { get; set; }

    public Rule(char Cat, bool LessThan, int Value, string Child)
    {
        this.Cat = Cat;
        this.LessThan = LessThan;
        this.Value = Value;
        this.Child = Child;
        this.Ranges = Rule.FullRange();
        if (Child == "R" && Cat == '?')
        {
            this.Ranges = Rule.EmptyRange();
            return;
        };

        this.Ranges = this.Ranges.Select((Range range) =>
        {
            if (range.Cat == Cat)
            {
                if (LessThan) return (Cat, range.Min, Value);
                if (!LessThan) return (Cat, Value + 1, range.Max);
            }
            return range;
        }).ToList();
    }

    public override string ToString()
    {
        return this.Child + " " + string.Join(' ', this.Ranges);
    }

    public static List<Range> FullRange()
    {
        return [('x', 1, 4001), ('m', 1, 4001), ('a', 1, 4001), ('s', 1, 4001)];
    }

    public static List<Range> EmptyRange()
    {
        return [('x', 0, 0), ('m', 0, 0), ('a', 0, 0), ('s', 0, 0)];
    }

    public static Rule LeafRule(string Label)
    {
        return new('?', false, -1, Label);
    }

    public static List<Range> AndRanges(List<Range> A, List<Range> B)
    {
        List<Range> combo = [];
        for (int i = 0; i < A.Count; i++)
        {
            Range r = (A[i].Cat, Math.Max(A[i].Min, B[i].Min), Math.Min(A[i].Max, B[i].Max));
            if (r.Min >= r.Max) r = (A[i].Cat, 0, 0);
            combo.Add(r);
        }
        return combo;
    }

    public static List<Range> InvertRanges(List<Range> ranges)
    {
        List<Range> inv = [];

        return inv;
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