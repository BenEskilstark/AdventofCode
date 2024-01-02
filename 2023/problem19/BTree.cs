namespace Year2023;

public class BTree<T>(T value)
{
    public T Value { get; set; } = value;
    public BTree<T>? LeftChild { get; set; } = null;
    public BTree<T>? RightChild { get; set; } = null;

    public int GetDepth()
    {
        if (this.LeftChild == null && this.RightChild == null) return 1;
        int leftDepth = 0;
        int rightDepth = 0;
        if (this.LeftChild != null) leftDepth = this.LeftChild.GetDepth();
        if (this.RightChild != null) rightDepth = this.RightChild.GetDepth();
        return Math.Max(leftDepth, rightDepth) + 1;
    }

    public override string ToString()
    {
        List<(T, int, int)> flatTree = this.Flatten();
        int depth = flatTree[^1].Item2;
        string output = "";
        string line = "";
        int prevDepth = 0;
        int prevCol = 0;
        foreach ((T, int, int) node in flatTree)
        {
            if (node.Item2 != prevDepth)
            {
                output += line + "\n";
                line = "";
                prevDepth = node.Item2;
                prevCol = 0;
            }
            line += new String(' ', (node.Item3 - prevCol)) + node.Item1;
            prevCol = node.Item3;
        }
        output += line;
        return output;
    }

    public List<(T, int, int)> Flatten()
    {
        List<(T, int, int)> flatTree = [];
        int depth = this.GetDepth();
        Queue<(BTree<T>, int, int)> nodes = new([(this, 0, depth * depth)]);
        while (nodes.TryDequeue(out (BTree<T>, int, int) node))
        {
            BTree<T> tree = node.Item1;
            int row = node.Item2;
            int col = node.Item3;
            flatTree.Add((tree.Value, row, col));
            if (tree.LeftChild != null) nodes.Enqueue((tree.LeftChild, row + 1, col - (depth - row) - 1));
            if (tree.RightChild != null) nodes.Enqueue((tree.RightChild, row + 1, col + (depth - row) + 1));
        }
        return flatTree;
    }
}