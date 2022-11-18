using System;
using System.Diagnostics;
using Monocle;
namespace BakeryBash;
public class PerfTimerLogger : IDisposable
{
    public PerfTimerLogger(string message)
    {
        this._message = message;
        this._timer = new Stopwatch();
        this._timer.Start();
    }

    string _message;
    Stopwatch _timer;

    public void Dispose()
    {
        this._timer.Stop();
        var ms = this._timer.ElapsedMilliseconds;
        Calc.Log(string.Format("{0} - Elapsed Milliseconds: {1}", this._message, ms));
        // log the performance timing with the Logging library of your choice
        // Example:
        // Logger.Write(
        //     string.Format("{0} - Elapsed Milliseconds: {1}", this._message, ms)
        // );
    }
}