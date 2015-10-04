# MiniProfiler.Log4Net
MiniProfiler Extensions allows save Profiler results into log by log4Net

All what you need is:

```cSharp
var logger = LogManager.GetLogger("Logger"); //Init Log4Net Logger

MiniProfilerLog.SetUpLog4Net(logger); //Set up profiler with logger
```

and use Profiler as usual (see [MiniProfiler site] (http://miniprofiler.com/) or [example Project](https://github.com/SvyatSlav/MiniProfiler.Log4Net/blob/master/Samples/Sample.Console/Program.cs))

##Install from Nuget


```
 Install-Package MiniProfiler.Log4Net 
```
