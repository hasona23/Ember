using System;
using System.Diagnostics;

namespace Ember.Utils;

public class BenchmarkWatch : IDisposable
{
    private readonly string _name;
    private readonly Stopwatch _stopwatch;

    public static BenchmarkWatch Create(string name)
    {
        return new BenchmarkWatch(name);
    }
    public BenchmarkWatch(string name)
    {
        _stopwatch = Stopwatch.StartNew();
        _name = name;
    }

    public void Dispose()
    {
        _stopwatch.Stop();
        Console.WriteLine($"{_name}: {_stopwatch.Elapsed.TotalMilliseconds} ms");
    }
}