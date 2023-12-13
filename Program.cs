// global using Console = System.Diagnostics.Debug;

using System.Diagnostics;
using CurrentYear = Year2022;

internal class Program
{
    private static void Main(string[] args)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        CurrentYear.Problem2.Solve();
        timer.Stop();
        // Format and display the TimeSpan value.
        TimeSpan ts = timer.Elapsed;
        string elapsedTime = String.Format("{0:00}hr {1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        Console.WriteLine("Finished in " + elapsedTime);
    }
}