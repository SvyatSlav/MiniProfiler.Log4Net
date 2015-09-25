namespace Profiling.Log4Net
{
    /// <summary>
    /// Level with profiler write message into log
    /// </summary>
    public enum Log4NetLevels : byte
    {
        /// <summary>
        /// The off
        /// </summary>
        Off = 100,
        /// <summary>
        /// The fatal
        /// </summary>
        Fatal = 4,
        /// <summary>
        /// The error
        /// </summary>
        Error = 3,
        /// <summary>
        /// The warn
        /// </summary>
        Warn = 2,
        /// <summary>
        /// The information
        /// </summary>
        Info = 1,
        /// <summary>
        /// The debug
        /// </summary>
        Debug = 0,
       
    }
}