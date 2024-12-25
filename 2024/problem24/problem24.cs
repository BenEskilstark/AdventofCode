namespace Year2024;

using Bit = int?;
using Wire = (string Name, int? Value);

public class Problem24
{
    public static void Solve()
    {
        Circuit circuit = new("2024/problem24/input.txt");
        circuit.Compute().WriteLine("Part 1:");

        List<string> outputs = circuit.OutputsToGate.Keys.ToList();
        outputs.Sort();
        List<(string, string)> swaps = [
            ("z09", "rkf"), ("z20", "jgb"), ("z24", "vcg"), ("rrs", "rvc")
        ];
        Random rand = new();
        int leastSigWrongBit = 45;
        for (int i = 0; i < 100; i++)
        {
            circuit.InitCircuit();
            foreach (var swap in swaps)
            {
                circuit.Swap(swap.Item1, swap.Item2);
            }
            long num1 = rand.NextInt64((long)Math.Pow(2, 44));
            long num2 = rand.NextInt64((long)Math.Pow(2, 44));
            long res = circuit.Compute(num1, num2);
            List<Bit> resBits = NumToBits(res);
            resBits.Reverse();
            List<Bit> tarBits = NumToBits(num1 + num2);
            tarBits.Reverse();
            for (int j = 0; j < resBits.Count; j++)
            {
                if (resBits[j] != tarBits[j] && j < leastSigWrongBit)
                {
                    leastSigWrongBit = j;
                    Console.WriteLine(string.Join("", resBits));
                    Console.WriteLine(string.Join("", tarBits));
                    Console.WriteLine("-------------------------------");
                }
            }

        }
        Console.WriteLine("Least Significant Wrong Bit: " + leastSigWrongBit);
        List<string> final = [];
        swaps.ForEach(s =>
        {
            final.Add(s.Item1);
            final.Add(s.Item2);
        });
        final.Sort();
        Console.WriteLine("Part 2: " + string.Join(",", final));
    }



    private class Circuit
    {
        public string SourceFile { get; }
        Dict<string, Bit> WireVals { get; set; } = new(null);
        Dict<(string, string), List<Gate>> WiresToGates { get; set; } = new([], () => []);
        public Dict<string, Gate> OutputsToGate { get; private set; } = new(default); // for part 2

        public Circuit(string sourceFile)
        {
            SourceFile = sourceFile;
            InitCircuit();
        }

        public void InitCircuit()
        {
            WireVals = new(null);
            WiresToGates = new([], () => []);
            OutputsToGate = new(default);

            File.ReadAllText(SourceFile).Split("\n\n")[0].Split("\n").ForEach(line =>
            {
                string wire = line.Split(":")[0];
                Bit val = int.Parse(line.Split(":")[1]);
                WireVals[wire] = val;
            });

            File.ReadAllText(SourceFile).Split("\n\n")[1].Split("\n").ForEach(line =>
            {
                string outWire = line.Split("->")[1].Trim();
                List<string> ins = line.Split("->")[0].Split(" ").ToList();
                string inWire1 = ins[0];
                string inWire2 = ins[2];
                string gateType = ins[1];
                Gate gate = new(
                    gateType,
                    (inWire1, WireVals[inWire1]), (inWire2, WireVals[inWire2]),
                    (outWire, WireVals[outWire] /* seeding with null */)
                );
                WiresToGates[(inWire1, inWire2)].Add(gate);
                OutputsToGate[outWire] = gate;
            });
        }

        public void Swap(string out1, string out2)
        {
            Gate gateA = OutputsToGate[out1];
            Gate gateB = OutputsToGate[out2];

            gateA.Out = (out2, WireVals[out2]);
            gateB.Out = (out1, WireVals[out1]);
            OutputsToGate[out1] = gateB;
            OutputsToGate[out2] = gateA;
        }

        public long Compute(long? in1 = null, long? in2 = null)
        {
            if (in1 != null && in2 != null)
            {
                List<Bit> xs = NumToBits((long)in1);
                List<Bit> ys = NumToBits((long)in2);
                xs.Reverse(); // put least significant bits first;
                ys.Reverse();
                for (int i = 0; i < 64; i++)
                {
                    string paddedStr = i == 0 ? "00" : i < 10 ? "0" + i : "" + i;
                    string xKey = "x" + paddedStr;
                    string yKey = "y" + paddedStr;
                    if (WireVals.ContainsKey(xKey)) WireVals[xKey] = xs.Count > i ? xs[i] : 0;
                    if (WireVals.ContainsKey(yKey)) WireVals[yKey] = ys.Count > i ? ys[i] : 0;
                }
            }
            while (ZWireVals().Any(z => z == null))
            {
                WiresToGates.Keys.ForEach(wirePair =>
                {
                    Wire wire1 = (wirePair.Item1, WireVals[wirePair.Item1]);
                    Wire wire2 = (wirePair.Item2, WireVals[wirePair.Item2]);
                    if (wire1.Value == null || wire2.Value == null) return;
                    foreach (Gate gate in WiresToGates[wirePair])
                    {
                        gate.In1Val = wire1.Value;
                        gate.In2Val = wire2.Value;
                        Bit output = gate.Compute();
                        WireVals[gate.Out.Name] = gate.Out.Value;
                    }
                });
            }
            return BitsToNum(ZWireVals());
        }

        public List<Bit> ZWireVals()
        {
            return WireVals.Keys
                .Where(wire => wire[0] == 'z')
                .OrderByDescending(wire => wire.GetNums()[0])
                .Select(wire => WireVals[wire])
                .ToList();
        }
    }

    public static List<Bit> NumToBits(long num)
    {
        List<Bit> bits = [];
        if (num == 0) return [0];
        for (; num > 0; bits.Add((int)(num % 2)), num /= 2) { }
        bits.Reverse();
        return bits;
    }

    public static long BitsToNum(List<Bit> bits)
    {
        return (0..bits.Count).ToEnumerable()
            .Aggregate(0, (long sum, int i) =>
            {
                return sum + (bits[i] == 0 ? 0 : (long)Math.Pow(2, bits.Count - 1 - i));
            });
    }


    private class Gate(string Type, Wire In1, Wire In2, Wire _out)
    {
        public Bit In1Val { get; set; } = In1.Value;
        public Bit In2Val { get; set; } = In2.Value;
        public Wire Out = _out;

        public Bit Compute()
        {
            if (In1Val == null || In2Val == null) return null;
            Out.Value = Type switch
            {
                "AND" => In1Val == 1 && In2Val == 1 ? 1 : 0,
                "OR" => In1Val == 1 || In2Val == 1 ? 1 : 0,
                "XOR" => In1Val != In2Val ? 1 : 0,
                _ => throw new Exception("No Gate of Type: " + Type),
            };
            return Out.Value;
        }
    }

}