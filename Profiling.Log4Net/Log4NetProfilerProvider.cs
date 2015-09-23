using System;
using log4net;
using StackExchange.Profiling;

namespace Profiling.Log4Net
{
    /// <summary>
    /// For storage profiling data in log.
    /// Only allows one  instance of a <see cref="MiniProfiler"/> to be the <see cref="MiniProfiler.Current"/> one.
    /// </summary>
    internal class Log4NetProfilerProvider : BaseProfilerProvider
    {
        private readonly ILog _logger;

        private readonly Log4NetLevels _profilerLogLevel;
        private MiniProfiler _profiler;


        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetProfilerProvider"/> class.
        /// </summary>
        /// <param name="logger">Instance of log4net logger</param>
        /// <param name="profilerLogLevel">Level which profiler will write in log. Default == Debug</param>
        public Log4NetProfilerProvider(ILog logger, Log4NetLevels profilerLogLevel)
        {
            _logger = logger;
            _profilerLogLevel = profilerLogLevel;

            MiniProfiler.Settings.Storage = new Log4NetStorage(logger, profilerLogLevel);
        }

        /// <summary>
        /// Starts a new MiniProfiler and sets it to be current.  By the end of this method
        ///             <see cref="M:StackExchange.Profiling.BaseProfilerProvider.GetCurrentProfiler"/> should return the new MiniProfiler.
        /// </summary>
        [Obsolete("Please use the Start(string sessionName) overload instead of this one. ProfileLevel is going away.")]
        public override MiniProfiler Start(ProfileLevel level, string sessionName = null)
        {
            return Start(sessionName);
        }

        /// <summary>
        /// Stops the current MiniProfiler (if any is currently running).
        ///             <see cref="M:StackExchange.Profiling.BaseProfilerProvider.SaveProfiler(StackExchange.Profiling.MiniProfiler)"/> should be called if <paramref name="discardResults"/> is false
        /// </summary>
        /// <param name="discardResults">If true, any current results will be thrown away and nothing saved</param>
        public override void Stop(bool discardResults)
        {
            if (_profiler != null)
            {
                StopProfiler(_profiler);
                SaveProfiler(_profiler);
            }
        }

        /// <summary>
        /// Returns the current MiniProfiler.  This is used by <see cref="P:StackExchange.Profiling.MiniProfiler.Current"/>.
        /// </summary>
        public override MiniProfiler GetCurrentProfiler()
        {
            return _profiler;
        }

        /// <summary>
        /// Starts a new MiniProfiler and sets it to be current.  By the end of this method
        ///             <see cref="M:StackExchange.Profiling.BaseProfilerProvider.GetCurrentProfiler"/> should return the new MiniProfiler.
        /// </summary>
        public override MiniProfiler Start(string sessionName = null)
        {
            _profiler = new MiniProfiler(sessionName ?? AppDomain.CurrentDomain.FriendlyName);
            if (IsLogEnabled(_logger, _profilerLogLevel))
            {
                SetProfilerActive(_profiler);
            }
            else
            {
                StopProfiler(_profiler);
            }


            return _profiler;
        }

        private bool IsLogEnabled(ILog log, Log4NetLevels profilerLogLevel)
        {
            if (log == null)
            {
                return false;
            }

            switch (profilerLogLevel)
            {
                case Log4NetLevels.Off:
                    return false;
                case Log4NetLevels.Fatal:
                    return log.IsFatalEnabled;
                case Log4NetLevels.Error:
                    return log.IsErrorEnabled;
                case Log4NetLevels.Warn:
                    return log.IsWarnEnabled;
                case Log4NetLevels.Info:
                    return log.IsInfoEnabled;
                case Log4NetLevels.Debug:
                    return log.IsDebugEnabled;
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is same logger] [the specified logger]. Or ProfilerLogLevel equals
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="profilerLogLevel">The profiler log level.</param>
        /// <returns></returns>
        public bool IsSameLogger(ILog logger, Log4NetLevels profilerLogLevel)
        {
            return _logger == logger && _profilerLogLevel == profilerLogLevel;
        }
    }
}