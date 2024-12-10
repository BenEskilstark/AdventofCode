// global using Console = System.Diagnostics.Debug;

using System.Diagnostics;
using CurrentYear = Year2024;


Stopwatch timer = new();
timer.Start();
CurrentYear.Problem10.Solve();
timer.Stop();

// Format and display the TimeSpan value.
TimeSpan ts = timer.Elapsed;
string elapsedTime = String.Format("{0:00}hr {1:00}:{2:00}.{3:000}",
    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
Console.WriteLine("Finished in " + elapsedTime);