namespace Year2025;

public class Problem11
{
    public static void Solve()
    {
        string file = "2025/problem11/input.txt";
        Dict<string, List<string>> paths = new([], () => []);
        Dict<string, List<string>> reversePaths = new([], () => []);

        File.ReadAllLines(file)
            .ForEach(line =>
            {
                string key = line.Split(":")[0];
                List<string> values = line.Split(":")[1].Trim().Split(" ").ToList();
                paths[key] = values;
                values.ForEach(v => reversePaths[v].Add(key));
            });

        int total = 0;
        Queue<string> queue = new();
        queue.Enqueue("you");
        while (queue.TryDequeue(out string cur))
        {
            if (cur == "out")
            {
                total++;
                continue;
            }
            paths[cur].ForEach(queue.Enqueue);
        }
        total.WriteLine("Part 1:");

        long dacToEnd = 0;
        Queue<string> queue2 = new();
        queue2.Enqueue("dac");
        Set<string> visited = new();
        while (queue2.TryDequeue(out string cur))
        {
            if (cur != "dac" && cur != "out") visited.Add(cur);
            if (cur == "out")
            {
                dacToEnd++;
                continue;
            }
            paths[cur].ForEach(queue2.Enqueue);
        }
        dacToEnd.WriteLine("dac to end:");

        Queue<string> queue3 = new();
        long svrToFft = 0;
        queue3.Enqueue("fft");
        while (queue3.TryDequeue(out string cur))
        {
            if (cur != "svr" && cur != "fft") visited.Add(cur);
            if (cur == "svr")
            {
                svrToFft++;
                continue;
            }
            paths[cur].ForEach(queue3.Enqueue);
        }
        svrToFft.WriteLine("svr to fft:");

        Queue<string> queue4 = new();
        long fftToDac = 0;
        queue4.Enqueue("dac");
        while (queue4.TryDequeue(out string cur))
        {
            if (cur == "fft")
            {
                fftToDac++;
                continue;
            }
            foreach (string next in reversePaths[cur])
            {
                if (visited[next]) continue;
                queue4.Enqueue(next);
            }
        }
        fftToDac.WriteLine("fft to dac:");
        (svrToFft * fftToDac * dacToEnd).WriteLine("Part 2:");
    }

}