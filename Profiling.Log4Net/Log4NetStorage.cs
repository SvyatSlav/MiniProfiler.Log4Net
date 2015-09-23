using System;
using System.Collections.Generic;
using log4net;
using log4net.Util;
using StackExchange.Profiling;
using StackExchange.Profiling.Storage;

namespace Profiling.Log4Net
{
    /// <summary>
    /// Storage use log4Net logger
    /// </summary>
    internal class Log4NetStorage : IStorage
    {
        private readonly ILog _log;
        private readonly Log4NetLevels _profilerProfilerLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetStorage"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="profilerProfilerLevel">The profiler profiler level.</param>
        public Log4NetStorage(ILog log, Log4NetLevels profilerProfilerLevel)
        {
            _log = log;
            _profilerProfilerLevel = profilerProfilerLevel;
        }

        /// <summary>
        /// List the latest profiling results.
        /// </summary>
        public IEnumerable<Guid> List(int maxResults, DateTime? start = null, DateTime? finish = null, ListResultsOrder orderBy = ListResultsOrder.Descending)
        {
            //TODO NotImplementedException();
            return null;
        }

        /// <summary>
        /// Stores <paramref name="profiler"/> under its <see cref="P:StackExchange.Profiling.MiniProfiler.Id"/>.
        /// </summary>
        /// <param name="profiler">The results of a profiling session.</param>
        /// <remarks>
        /// Should also ensure the profiler is stored as being un-viewed by its profiling <see cref="P:StackExchange.Profiling.MiniProfiler.User"/>.
        /// </remarks>
        public void Save(MiniProfiler profiler)
        {
            if (_log == null)
            {
                return;
            }

            switch (_profilerProfilerLevel)
            {
                case Log4NetLevels.Off:
                    //Off
                    return;
                case Log4NetLevels.Fatal:
                    _log.FatalExt(GetProfilerText(profiler));
                    break;
                case Log4NetLevels.Error:
                    _log.ErrorExt(GetProfilerText(profiler));
                    break;
                case Log4NetLevels.Warn:
                    _log.WarnExt(GetProfilerText(profiler));
                    break;
                case Log4NetLevels.Info:
                    _log.InfoExt(GetProfilerText(profiler));
                    break;
                case Log4NetLevels.Debug:
                    _log.DebugExt(GetProfilerText(profiler));
                    break;
            }
        }

        private string GetProfilerText(MiniProfiler profiler)
        {
            return profiler.RenderPlainText();
        }

        /// <summary>
        /// Returns a <see cref="T:StackExchange.Profiling.MiniProfiler"/> from storage based on <paramref name="id"/>, 
        ///             which should map to <see cref="P:StackExchange.Profiling.MiniProfiler.Id"/>.
        /// </summary>
        /// <remarks>
        /// Should also update that the resulting profiler has been marked as viewed by its 
        ///             profiling <see cref="P:StackExchange.Profiling.MiniProfiler.User"/>.
        /// </remarks>
        public MiniProfiler Load(Guid id)
        {
            //TODO NotImplementedException();
            return null;
        }

        /// <summary>
        /// Sets a particular profiler session so it is considered "un-viewed"  
        /// </summary>
        public void SetUnviewed(string user, Guid id)
        {
            //TODO NotImplementedException();
        }

        /// <summary>
        /// Sets a particular profiler session to "viewed"
        /// </summary>
        public void SetViewed(string user, Guid id)
        {
            //TODO NotImplementedException();
        }

        /// <summary>
        /// Returns a list of <see cref="P:StackExchange.Profiling.MiniProfiler.Id"/>s that haven't been seen by <paramref name="user"/>.
        /// </summary>
        /// <param name="user">User identified by the current <c>MiniProfiler.Settings.UserProvider</c></param>
        public List<Guid> GetUnviewedIds(string user)
        {
            //TODO NotImplementedException();
            return null;
        }
    }
}