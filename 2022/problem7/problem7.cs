namespace Year2022;

public class Problem7
{
    public static void Solve()
    {
        string file = "2022/problem7/input.txt";

        FilePointer root = new FilePointer("/");
        FilePointer curDirectory = root;
        // parse the file structure
        foreach (string line in File.ReadLines(file))
        {
            if (line[0] == '$')
            {
                List<string> cmd = line.Split(' ').ToList();
                if (cmd[1] == "cd")
                {
                    if (cmd[2] == "/") curDirectory = root;
                    if (cmd[2] == "..") curDirectory = curDirectory.Parent ?? root;
                    if (cmd[2] != "/" && cmd[2] != "..")
                    {
                        if (curDirectory.Children.TryGetValue(cmd[2], out FilePointer? child))
                        {
                            curDirectory = child;
                        }
                        else
                        {
                            child = new FilePointer(cmd[2], curDirectory);
                            curDirectory = child;
                        }
                    }
                }
            }
            else
            { // we're in the result of an ls command
                List<string> fileDesc = line.Split(' ').ToList();
                if (fileDesc[0] == "dir")
                {
                    curDirectory.Children[fileDesc[1]] = new FilePointer(fileDesc[1], curDirectory);
                }
                else
                {
                    curDirectory.Children[fileDesc[1]] = new FilePointer(fileDesc[1], curDirectory, long.Parse(fileDesc[0]));
                }
            }
        }
        // Console.WriteLine(root);
        // part 1
        Console.WriteLine(root.GetSizeAtMost(100000));

        // part 2
        long totalMemory = 70000000;
        long minFreeMemory = 30000000;
        long curFreeMemory = totalMemory - root.GetSize();
        Console.WriteLine(root.SizeOfDirToFree(minFreeMemory - curFreeMemory));
    }
}

public class FilePointer(string name, FilePointer? parent = null, long size = 0)
{
    public string Name { get; } = name;
    public FilePointer? Parent { get; } = parent;
    public long Size { get; set; } = size;
    public Dictionary<string, FilePointer> Children { get; set; } = [];

    public override string ToString()
    {
        return this.ToStringHelper(0);
    }

    private string ToStringHelper(int depth)
    {
        string type = this.Size != 0 ? "file" : "dir";
        string res = new string('\t', depth) + "- " + this.Name + " " + type + " size= " + this.GetSize() + "\n";
        this.Children.Values.ToList().ForEach(f => res += f.ToStringHelper(depth + 1));
        return res;
    }

    public long GetSize()
    {
        if (this.Children.Count == 0) return this.Size;
        long total = 0;
        this.Children.Values.ToList().ForEach(f => total += f.GetSize());
        return total;
    }

    public long GetSizeAtMost(long atMost)
    {
        Stack<FilePointer> fps = new([this]);
        long total = 0;
        while (fps.TryPop(out FilePointer? fp))
        {
            if (fp.Children.Count == 0) continue;
            fp.Children.Values.ToList().ForEach(f => fps.Push(f));
            if (fp.GetSize() < atMost) total += fp.GetSize();
        }
        return total;
    }

    public long SizeOfDirToFree(long freeMemoryNeeded)
    {
        Stack<FilePointer> fps = new([this]);
        long minSize = 1000000000000;
        while (fps.TryPop(out FilePointer? fp))
        {
            if (fp.Children.Count == 0) continue;
            fp.Children.Values.ToList().ForEach(f => fps.Push(f));
            long size = fp.GetSize();
            if (size >= freeMemoryNeeded && size < minSize)
            {
                minSize = size;
            }
        }
        return minSize;
    }
}