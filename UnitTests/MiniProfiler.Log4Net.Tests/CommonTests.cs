using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using NUnit.Framework;
using StackExchange.Profiling;


namespace Profiling.Log4Net.Tests
{
    /// <summary>
    /// Common Test
    /// </summary>
    [TestFixture]
    public class CommonTests
    {
        private MemoryAppender _memoryAppender;

        #region SetUps

        private void SetUpLog(Level level)
        {
            _memoryAppender = new MemoryAppender();
            Logger root = ((Hierarchy) LogManager.GetRepository()).Root;
            root.Level = level;
            root.AddAppender(_memoryAppender);
            root.Repository.Configured = true;
        }

        private static void SetUpProfiler(Log4NetLevels level = Log4NetLevels.Debug)
        {
            MiniProfilerLog.SetUpLog4Net(LogManager.GetLogger("InfoLoger"), level);
        }


        private static void StartOneStep()
        {
            MiniProfiler.Start();
            MiniProfiler.StepStatic("Step");
            MiniProfiler.Stop();
        }

        #endregion
        

        [Test]
        public void OneStep_OneEventWithMessage()
        {
            SetUpLog(Level.Debug);
            SetUpProfiler();

            var mp = MiniProfiler.Start();

            mp.Step("SimpleStep");

            MiniProfiler.Stop();

            var events = _memoryAppender.GetEvents();
            Assert.That(events.Count(), Is.EqualTo(1));

            var logEvent = events.First();

            Assert.That(logEvent.RenderedMessage.Contains("SimpleStep"));
            Assert.That(logEvent.Level, Is.EqualTo(Level.Debug));
        }

        [Test]
        public void SetUpWithInfoLevel_2Step_2EventInfoLevel()
        {
            SetUpLog(Level.Info);
            SetUpProfiler(Log4NetLevels.Info);

            //first session
            StartOneStep();
            //second session
            StartOneStep();

            var events = _memoryAppender.GetEvents();

            Assert.That(events.Count(), Is.EqualTo(2));
            Assert.That(events.Select(e => e.Level), Is.All.EqualTo(Level.Info));
        }

        [Test]
        public void LogInfo_ProfilerDebug_ZeroEvents()
        {
            SetUpLog(Level.Info);
            SetUpProfiler(Log4NetLevels.Debug);

            StartOneStep();

            Assert.That(_memoryAppender.GetEvents().Count(), Is.EqualTo(0));
        }

        [Test]
        public void ProfilerLogDisabled_SeveralStep_1RootTimings()
        {
            SetUpLog(Level.Info);
            SetUpProfiler(Log4NetLevels.Debug);

            StartOneStep();
            StartOneStep();

            Assert.That(MiniProfiler.Current.GetTimingHierarchy().Count(), Is.EqualTo(1));
        }
    }
}