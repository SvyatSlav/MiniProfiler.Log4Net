using System;
using log4net;
using StackExchange.Profiling;

namespace Profiling.Log4Net
{
    /// <summary>
    /// For storage profiling data in log.
    /// Only allows one  instance of a <see cref="MiniProfiler"/> to be the <see cref="MiniProfiler.Current"/> one.
    /// </summary>
    public class Log4NetProfilerProvider : BaseProfilerProvider
    {
        private ILog _log;

        private Log4NetLevels _profilerLevel;
        private MiniProfiler _profiler;


        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetProfilerProvider"/> class.
        /// </summary>
        /// <param name="log">Log4Net Logger</param>
        /// <param name="profilerProfilerLevel">LogLevel for profiling message</param>
        public Log4NetProfilerProvider(ILog log , Log4NetLevels profilerProfilerLevel)
        {
            _log = log ?? LogManager.GetLogger("Log4NetProviderProfiler");
            _profilerLevel = profilerProfilerLevel;

            if (!(MiniProfiler.Settings.Storage is Log4NetStorage))
            {
                MiniProfiler.Settings.Storage = new Log4NetStorage(log, profilerProfilerLevel);
            }
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
            //TODO Тут хочется чтобы при выключенном уровне логинга даже тайминг не приходил, т.е пишем свой Профайлер
            _profiler = new MiniProfiler(sessionName ?? AppDomain.CurrentDomain.FriendlyName);
            SetProfilerActive(_profiler);
            return _profiler;
        }
    }
}