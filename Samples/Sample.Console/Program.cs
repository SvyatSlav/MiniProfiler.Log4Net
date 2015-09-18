using System.Data.Common;
using System.Threading;
using log4net;
using log4net.Config;
using Profiling.Log4Net;
using StackExchange.Profiling;
using StackExchange.Profiling.Helpers.Dapper;


namespace Sample.Console
{
    internal static class Program
    {
        private static void Main()
        {
            //Configure log4Net
            XmlConfigurator.Configure();

            //Init Logger
            var logger = LogManager.GetLogger("MainLogger");

            //Common usages of this library, profilerProvider
            MiniProfilerProvider(logger);


            //Additional usages, log4net extension
            Log4NetExtensions(logger);
        }

        /// <summary>
        /// This is common of usages
        /// </summary>
        /// <param name="logger">The logger.</param>
        private static void MiniProfilerProvider(ILog logger)
        {
            MiniProfilerEx.SetUpLog4Net(logger);

            var mp = MiniProfiler.Start("Provider");

            using (mp.Step("Level 1"))
            using (var conn = GetConnection())
            {
                conn.Query<long>("select 1");

                using (mp.Step("Level 2"))
                {
                    conn.Query<long>("select 1");
                    conn.Query("select 2");

                    using (mp.Step("Level 3.1"))
                    {
                        Thread.Sleep(500);
                    }
                }

                using (mp.Step("Level 2.2"))
                {
                    conn.Query("select 1");
                    Thread.Sleep(500);
                }
            }

            MiniProfiler.Stop();
        }

        /// <summary>
        /// Returns an open connection that will have its queries profiled.
        /// </summary>
        /// <returns>the database connection abstraction</returns>
        private static DbConnection GetConnection()
        {
            DbConnection cnn = new System.Data.SQLite.SQLiteConnection("Data Source=:memory:");

            // to get profiling times, we have to wrap whatever connection we're using in a ProfiledDbConnection
            // when MiniProfiler.Current is null, this connection will not record any database timings
            if (MiniProfiler.Current != null)
            {
                cnn = new StackExchange.Profiling.Data.ProfiledDbConnection(cnn, MiniProfiler.Current);
            }

            cnn.Open();
            return cnn;
        }

        /// <summary>
        /// Additional, using logManager Exstension
        /// </summary>
        /// <param name="logger">The logger.</param>
        private static void Log4NetExtensions(ILog logger)
        {
            logger.StartProfiler();

            using (logger.Step("StepA"))
            {
                Thread.Sleep(100);

                using (logger.Step("StepA.B"))
                {
                    Thread.Sleep(50);
                }
            }


            logger.StopProfiler();


            logger.StartProfiler("New Session");


            using (logger.Step("StepA"))
            {
                Thread.Sleep(50);
            }

            logger.StopProfiler();
        }
    }
}