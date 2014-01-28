using System;
using System.Diagnostics;
using PostSharp.Aspects;

namespace Common.Types.Aspect
{
    /// <summary>
    /// Aspect that, when applied on a method, increments a performance counter
    /// by the time elapsed during the execution of this method.
    /// </summary>
    [Serializable]
    public class TimePerformanceCounterAttribute : OnMethodBoundaryAspect
    {
        static readonly Stopwatch stopwatch = new Stopwatch();

        static TimePerformanceCounterAttribute()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Method invoked before the execution of the method to which the current
        /// aspect is applied.
        /// </summary>
        /// <param name="args">Unused.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = stopwatch.ElapsedTicks;
            base.OnEntry(args);
        }

        /// <summary>
        /// Method invoked after the execution of the method to which the current
        /// aspect is applied.
        /// </summary>
        /// <param name="args">Unused.</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            long time = stopwatch.ElapsedTicks - (long)args.MethodExecutionTag;
            Console.WriteLine("Method {0} performs by {1} ticks.", args.Method.Name, time);
            base.OnExit(args);
        }
    }
}
