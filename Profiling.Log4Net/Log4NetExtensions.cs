using System;
using log4net;
using StackExchange.Profiling;

namespace Profiling.Log4Net
{
    /// <summary>
    /// Extensions for ILog class
    /// </summary>
    public static class Log4NetExtensions
    {
        /// <summary>
        /// Starts the profiler by default settings
        /// </summary>
        /// <param name="logger">The logger.</param>
        public static MiniProfiler StartProfiler(this ILog logger)
        {
            return StartProfiler(logger, null);
        }

        /// <summary>
        /// tarts the profiler with session name. ProfilerLogLevel == Debug
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sessionName">Name of the session.</param>
        /// <returns></returns>
        public static MiniProfiler StartProfiler(this ILog logger, string sessionName)
        {
            return StartProfiler(logger, sessionName, Log4NetLevels.Debug);
        }

        /// <summary>
        /// Starts the profiler with session name and  ProfilerLogLevel
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sessionName">Name of the session.</param>
        /// <param name="profilerLevel">Level which profiler will write in log</param>
        public static MiniProfiler StartProfiler(this ILog logger, string sessionName, Log4NetLevels profilerLevel)
        {
            MiniProfilerLog.SetUpLog4Net(logger, profilerLevel);

            return MiniProfiler.Start(sessionName);
        }

        /// <summary>
        /// Steps for profiler
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static IDisposable Step(this ILog logger, string name)
        {
            return MiniProfiler.StepStatic(name);
        }

        /// <summary>
        /// Stops the profiler.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public static void StopProfiler(this ILog logger)
        {
            MiniProfiler.Stop();
        }
    }
}