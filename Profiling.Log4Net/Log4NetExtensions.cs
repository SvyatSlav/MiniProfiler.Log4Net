using System;
using log4net;
using StackExchange.Profiling;

namespace Profiling.Log4Net
{
    public static class Log4NetExtensions
    {
        //TODO Return ProfilerLogger
        public static void StartProfiler(this ILog logger)
        {
            StartProfiler(logger, null);
        }


        public static void StartProfiler(this ILog logger, string sessionName)
        {
            if (!(MiniProfiler.Settings.ProfilerProvider is SingletonProfilerProvider))
            {
                MiniProfiler.Settings.ProfilerProvider = new SingletonProfilerProvider();
            }

            MiniProfiler.Start(sessionName);
        }

        public static IDisposable Step(this ILog logger, string name)
        {
            return MiniProfiler.StepStatic(name);
        }

        public static void StopProfiler(this ILog logger)
        {
            MiniProfiler.Stop();

            //TODO в Экстеншен
            if (logger.IsDebugEnabled)
            {
                logger.Debug(MiniProfiler.Current.RenderPlainText());
            }
        }
    }
}