# MiniProfiler.Log4Net
MiniProfiler Extensions allows save MiniProfiler results into file (or another source) by log4Net

[![Build status](https://ci.appveyor.com/api/projects/status/2kkwren9loyqw9is?svg=true)](https://ci.appveyor.com/project/SvyatSlav/miniprofiler-log4net)

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
