namespace Year2024;

using Register = (long A, long B, long C);

public class Problem17
{
    public static void Solve()
    {
        string file = "2024/problem17/input.txt";
        string[] lines = File.ReadAllLines(file);

        Register reg = (lines[0].GetNums()[0], lines[1].GetNums()[0], lines[2].GetNums()[0]);
        List<long> ins = lines[4].GetNums().Select(i => (long)i).ToList();
        // Part 1:
        RunProgram(reg, ins);

        // Part 2:
        Stack<(long, int)> attempts = new([(0, ins.Count - 1)]);
        long min = (long)Math.Pow(2, 48);
        while (attempts.TryPop(out (long, int) attempt))
        {
            (long a, int i) = attempt;
            if (i == -1)
            {
                min = Math.Min(min, a / 8);
                continue;
            }
            long ubound = a + 8;
            for (; a < ubound; a++)
            {
                if (RunProgram((a, 0, 0), ins, string.Join(',', ins[i..])))
                {
                    attempts.Push((a << 3, i - 1));
                }
            }
        }
        Console.WriteLine("Part 2: " + min);
    }


    public static bool RunProgram(Register reg, List<long> ins, string target = "")
    {
        int ptr = 0;
        string output = "";
        while (ptr < ins.Count)
        {
            bool didJump = false;
            switch (ins[ptr])
            {
                case 0:
                    reg.A = reg.A >> (int)Combo(reg, ins[ptr + 1]);
                    break;
                case 1:
                    reg.B = reg.B ^ ins[ptr + 1];
                    break;
                case 2:
                    reg.B = Combo(reg, ins[ptr + 1]) % 8;
                    break;
                case 3:
                    if (reg.A != 0)
                    {
                        ptr = (int)ins[ptr + 1];
                        didJump = true;
                    }
                    break;
                case 4:
                    reg.B = reg.B ^ reg.C;
                    break;
                case 5:
                    if (output.Length > 0) output += ",";
                    output += "" + (Combo(reg, ins[ptr + 1]) % 8);
                    if (target.Length > 0 && (output.Length > target.Length || target[..output.Length] != output))
                    {
                        return false;
                    }
                    break;
                case 6:
                    reg.B = reg.A >> (int)Combo(reg, ins[ptr + 1]);
                    break;
                case 7:
                    reg.C = reg.A >> (int)Combo(reg, ins[ptr + 1]);
                    break;
            }
            if (!didJump) ptr += 2;
        }
        if (target == "") Console.WriteLine("Part 1: " + output);
        return output == target;
    }


    public static long Combo(Register reg, long operand)
    {
        return operand switch
        {
            0 => operand,
            1 => operand,
            2 => operand,
            3 => operand,
            4 => reg.A,
            5 => reg.B,
            6 => reg.C,
            _ => throw new Exception("invalid combo operand: " + operand)
        };
    }


    public static string State(Register reg, int ptr, string output)
    {
        string state = reg.ToString();
        state += "\nInstruction Pointer: " + ptr + "\nOutput: " + output;
        return state;
    }

}