using log4net;
using StackExchange.Profiling;

namespace Profiling.Log4Net
{
    public static class MiniProfilerEx
    {
        /// <summary>
        /// Setup profiler with log4Net-logger and log level
        /// </summary>
        /// <param name="logger">Instance of log4net logger</param>
        /// <param name="profilerLogLevel">Level which profiler will write in log. Default == Debug</param>
        public static void SetUpLog4Net(ILog logger, Log4NetLevels profilerLogLevel = Log4NetLevels.Debug)
        {
            var provider = new Log4NetProfilerProvider(logger, profilerLogLevel);

            MiniProfiler.Settings.ProfilerProvider = provider;
        }

        /// <summary>
        /// Initializes profiler with log4Net by default
        /// </summary>
        public static void SetUpLog4Net()
        {
            SetUpLog4Net(null);
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Setup profiler with log4Net-logger and log level
        /// </summary>
        /// <param name="profiler">The profiler.</param>
        /// <param name="log">Instance of log4net logger</param>
        /// <param name="levels">The levels.</param>
        public static void SetUpLog4Net(this StackExchange.Profiling.MiniProfiler profiler, ILog log, Log4NetLevels levels)
        {
            MiniProfilerEx.SetUpLog4Net(log, levels);
        }
    }
}