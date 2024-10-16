namespace Year2022;

public record CPU(int Register, List<int> Pipeline);

public class Problem10
{
    public static void Solve()
    {
        string file = "2022/problem10/input.txt";
        CPU cpu = new(1, [0, 0]);
        int cycleNum = 0;
        HashSet<int> cyclesOfInterest = [20, 60, 100, 140, 180, 220];
        int res = 0;
        File.ReadAllLines(file).ToList().ForEach(line =>
        {
            cycleNum++;
            if (cyclesOfInterest.Contains(cycleNum))
            {
                Console.WriteLine(cycleNum + " " + cpu.Register + " " + string.Join(", ", cpu.Pipeline));
                res += cycleNum * cpu.Register;
            }
            if (line == "noop") cpu = Cycle(cpu, 0);
            if (line != "noop")
            {
                cycleNum++;
                cpu = Cycle(cpu, int.Parse(line.Split(' ')[1]));
                cpu = Cycle(cpu, 0);
                if (cyclesOfInterest.Contains(cycleNum))
                {
                    Console.WriteLine(cycleNum + " " + cpu.Register + " " + string.Join(", ", cpu.Pipeline));
                    res += cycleNum * cpu.Register;
                }
                cpu = Cycle(cpu, 0);

            }
        });
        while (!AllZeroes(cpu))
        {
            cycleNum++;
            if (cyclesOfInterest.Contains(cycleNum))
            {
                Console.WriteLine(cycleNum + " " + cpu.Register + " " + string.Join(", ", cpu.Pipeline));
                res += cycleNum * cpu.Register;
            }
            cpu = Cycle(cpu, 0);
        }
        Console.WriteLine(res);
    }

    public static CPU Cycle(CPU cpu, int ins)
    {
        int nextRegister = cpu.Register;
        List<int> pipe = cpu.Pipeline;
        for (int i = pipe.Count - 1; i > 0; i--)
        {
            if (i == pipe.Count - 1) nextRegister += pipe[i];
            pipe[i] = pipe[i - 1];
        }
        pipe[0] = ins;
        return new(nextRegister, pipe);
    }

    public static bool AllZeroes(CPU cpu)
    {
        for (int i = 0; i < cpu.Pipeline.Count; i++)
        {
            if (cpu.Pipeline[i] != 0) return false;
        }
        return true;
    }
}