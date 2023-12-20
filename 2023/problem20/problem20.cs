namespace Year2023;

using Input = (string From, string To, Pulse Pulse);

public enum Pulse { High, Low }
public enum State { On, Off }


public class Problem20
{
    public static void Solve()
    {
        string file = "2023/problem20/input.txt";
        Circuit circuit = new();
        foreach (string line in File.ReadLines(file))
        {
            string moduleStr = line.Split('-')[0].Trim();
            string name = moduleStr;
            List<string> destinations = [.. line.Split('-')[1][1..].Trim().Split(',').Select(n => n.Trim())];
            if (moduleStr.StartsWith('%'))
            {
                name = moduleStr[1..];
                circuit.Configuration[name] = destinations;
                circuit.Modules[name] = new FlipFlop(name);
            }
            else if (moduleStr.StartsWith('&'))
            {
                name = moduleStr[1..];
                circuit.Configuration[name] = destinations;
                Conjunction c = new(name);
                circuit.Modules[name] = c;
                circuit.Conjunctions[name] = c;
            }
            else
            {
                circuit.Configuration[name] = destinations;
                circuit.Modules[name] = new Broadcaster(name);
            }
        }

        circuit.WireUp();

        // part 1
        // Dictionary<Pulse, long> numPulses = new() { [Pulse.Low] = 0, [Pulse.High] = 0 };
        // for (int i = 0; i < 1000; i++)
        // {
        //     Dictionary<Pulse, long> p = circuit.PressButton();
        //     numPulses[Pulse.Low] += p[Pulse.Low];
        //     numPulses[Pulse.High] += p[Pulse.High];
        // }
        // Console.WriteLine(numPulses[Pulse.High] * numPulses[Pulse.Low]);

        // part 2
        Dictionary<string, long> loops = [];
        foreach (string name in circuit.Conjunctions.Keys)
        {
            loops[name] = 0;
        }
        int i = 0;

        Console.WriteLine(string.Join(",", loops.Keys));
        while (true)
        {
            i++;
            circuit.PressForRX(loops, i);
            bool shouldBreak = true;
            foreach (long v in loops.Values)
            {
                if (v == 0) shouldBreak = false;
            }
            if (shouldBreak) break;
        }
        Console.WriteLine(i);
        Console.WriteLine(string.Join(',', loops.Values));

        // Console.WriteLine(circuit);

    }
}

public class Circuit()
{
    public Dictionary<string, Module> Modules { get; set; } = [];
    public Dictionary<string, List<string>> Configuration { get; set; } = [];
    public Dictionary<string, Conjunction> Conjunctions { get; set; } = [];

    public void WireUp()
    {
        foreach (string name in this.Configuration.Keys)
        {
            foreach (string d in this.Configuration[name])
            {
                if (!this.Modules.ContainsKey(d)) this.Modules[d] = new Output(d);
                this.Modules[name].DestinationModules.Add(this.Modules[d]);
                if (this.Conjunctions.TryGetValue(d, out Conjunction? c))
                {
                    c.Previous[name] = Pulse.Low;
                }
            }
        }
    }

    public void PressForRX(Dictionary<string, long> Loops, long numPresses)
    {
        Queue<Input> pulses = new([("button", "broadcaster", Pulse.Low)]);
        while (pulses.TryDequeue(out var input))
        {
            List<Input> outputs = this.Modules[input.To].SendPulse(input);
            if (this.Conjunctions.ContainsKey(input.To) && outputs[0].Pulse == Pulse.Low)
            {
                if (Loops[input.To] == 0)
                {
                    Loops[input.To] = numPresses;
                    Console.WriteLine(input.To + " " + numPresses);
                }
            }
            outputs.ForEach(o => pulses.Enqueue(o));
        }
    }

    public Dictionary<Pulse, long> PressButton()
    {
        Dictionary<Pulse, long> numPulses = new() { [Pulse.Low] = 1, [Pulse.High] = 0 };
        Queue<Input> pulses = new([("button", "broadcaster", Pulse.Low)]);
        while (pulses.TryDequeue(out var input))
        {
            List<Input> outputs = this.Modules[input.To].SendPulse(input);
            foreach (Input o in outputs)
            {
                // Console.WriteLine(input.To + " -" + o.Pulse + "-> " + o.To);
                numPulses[o.Pulse]++;
            }
            outputs.ForEach(o => pulses.Enqueue(o));
        }
        return numPulses;
    }

    public override string ToString()
    {
        string circuit = "";
        foreach (var pair in this.Configuration.ToList())
        {
            foreach (string child in pair.Value)
            {
                circuit += "[" + pair.Key + "] -> [" + child + "]\n";
            }
            // circuit += pair.Key + " -> " + string.Join(',', pair.Value) + "\n";
        }
        return circuit;
    }
}