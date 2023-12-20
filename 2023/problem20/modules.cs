namespace Year2023;


using Input = (string From, string To, Pulse Pulse);

public class Conjunction(string name) : Module(name)
{
    public Dictionary<string, Pulse> Previous { get; set; } = [];

    public override List<Input> SendPulse(Input input)
    {
        List<Input> pulses = [];
        this.Previous[input.From] = input.Pulse;
        Pulse toSend = Pulse.Low;
        foreach (Pulse p in this.Previous.Values)
        {
            if (p != Pulse.High)
            {
                toSend = Pulse.High;
                break;
            }
        }

        foreach (Module dest in this.DestinationModules)
        {
            pulses.Add((this.Name, dest.Name, toSend));
        }
        return pulses;
    }
}

public class FlipFlop(string name) : Module(name)
{
    public State Status { get; private set; } = State.Off;
    public override List<Input> SendPulse(Input input)
    {
        List<Input> pulses = [];
        if (input.Pulse == Pulse.High) return pulses;

        Pulse toSend = Pulse.Low;
        if (this.Status == State.Off)
        {
            this.Status = State.On;
            toSend = Pulse.High;
        }
        else
        {
            this.Status = State.Off;
            toSend = Pulse.Low;
        }
        foreach (Module dest in this.DestinationModules)
        {
            pulses.Add((this.Name, dest.Name, toSend));
        }
        return pulses;
    }
}

public class Broadcaster : Module
{
    public Broadcaster(string name) : base(name) { }

    public override List<Input> SendPulse(Input input)
    {
        List<Input> pulses = [];
        foreach (Module dest in this.DestinationModules)
        {
            pulses.Add((this.Name, dest.Name, input.Pulse));
        }
        return pulses;
    }
}

public class Output(string name) : Module(name)
{
    public override List<Input> SendPulse(Input input)
    {
        return [];
    }
}

public abstract class Module(string name)
{
    public string Name { get; } = name;
    public List<Module> DestinationModules { get; set; } = [];

    // sends a dict mapping name of module to what kind of pulse to send to it
    public abstract List<Input> SendPulse(Input input);
}