using System;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using NUnit.Framework;
using StackExchange.Profiling;


namespace Profiling.Log4Net.Tests
{
    //TODO Регрессионный тест на многопоточность, есть подозрение что не работает из-за профайлера. Типа pForEach(()=> mp.Step(""))

    /// <summary>
    /// Common Test
    /// </summary>
    [TestFixture]
    public class CommonTests
    {
        private MemoryAppender _memoryAppender;

        #region SetUps

        private void SetUpLog(Level level = null)
        {
            if (level == null)
            {
                level = Level.All;
            }

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

        private static Level ConvertToLevel(Log4NetLevels level)
        {
            switch (level)
            {
                case Log4NetLevels.Off:
                    return Level.Off;
                case Log4NetLevels.Fatal:
                    return Level.Fatal;
                case Log4NetLevels.Error:
                    return Level.Error;
                case Log4NetLevels.Warn:
                    return Level.Warn;
                case Log4NetLevels.Info:
                    return Level.Info;
                case Log4NetLevels.Debug:
                    return Level.Debug;
                default:
                    throw new ArgumentOutOfRangeException("level");
            }
        }

        #endregion

        [Test]
        public void OneStep_1MessageWithExpectedText()
        {
            SetUpLog();
            SetUpProfiler(Log4NetLevels.Debug);

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
        public void ProfilerLogLevelOff_1Step_0LogMessage()
        {
            SetUpLog();
            SetUpProfiler(Log4NetLevels.Off);

            StartOneStep();

            var events = _memoryAppender.GetEvents();

            Assert.That(events.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogInfo_ProfilerDebug_0LogMessage()
        {
            SetUpLog(Level.Info);
            SetUpProfiler(Log4NetLevels.Debug);

            StartOneStep();

            Assert.That(_memoryAppender.GetEvents().Count(), Is.EqualTo(0));
        }

        [TestCase(Log4NetLevels.Debug, 2)]
        [TestCase(Log4NetLevels.Info, 3)]
        [TestCase(Log4NetLevels.Warn, 5)]
        [TestCase(Log4NetLevels.Fatal, 1)]
        public void SetUpProfilerWithLevel_SomeStep_ExpectedStepCountAndEventLevel(Log4NetLevels level, int step)
        {
            SetUpLog();
            SetUpProfiler(level);

            for (int i = 0; i < step; i++)
            {
                StartOneStep();
            }


            var events = _memoryAppender.GetEvents();

            Assert.That(events.Count(), Is.EqualTo(step));
            Assert.That(events.Select(e => e.Level), Is.All.EqualTo(ConvertToLevel(level)));
        }


        [Test]
        public void LogDisabled_SeveralStep_1RootTimings()
        {
            SetUpLog(Level.Info);
            SetUpProfiler(Log4NetLevels.Debug);

            MiniProfiler.Start();

            MiniProfiler.StepStatic("Step");
            MiniProfiler.StepStatic("Step");
            MiniProfiler.StepStatic("Step");

            MiniProfiler.Stop();
            

            Assert.That(MiniProfiler.Current.GetTimingHierarchy().Count(), Is.EqualTo(1));
        }

        [Test]
        public void LogEnabled_SeveraStep_SeveralRootTimings()
        {
            SetUpLog();
            SetUpProfiler();

            MiniProfiler.Start();

            MiniProfiler.StepStatic("Step");
            MiniProfiler.StepStatic("Step");

            MiniProfiler.Stop();
          
            Assert.That(MiniProfiler.Current.GetTimingHierarchy().Count(), Is.EqualTo(3));
        }

        [Test]
        public void LogDisable_SwitchInRuntimeWithoutStart_0LogMessage()
        {
            SetUpLog(Level.Info);
            SetUpProfiler(Log4NetLevels.Debug);

            MiniProfiler.Start();
            MiniProfiler.StepStatic("Step");

            SetUpProfiler(Log4NetLevels.Info);


            MiniProfiler.StepStatic("Step");
            MiniProfiler.Stop();

            Assert.That(_memoryAppender.GetEvents().Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogDisable_SwitchInRuntimeAndStart_1LogMessage()
        {
            SetUpLog(Level.Info);
            SetUpProfiler(Log4NetLevels.Debug);

            MiniProfiler.Start();
            MiniProfiler.StepStatic("Step");

            SetUpProfiler(Log4NetLevels.Info);

            MiniProfiler.Start();
            MiniProfiler.StepStatic("Step");
            MiniProfiler.Stop();

            Assert.That(_memoryAppender.GetEvents().Count(), Is.EqualTo(1));
        }
    }
}