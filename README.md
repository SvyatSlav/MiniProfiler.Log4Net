# MiniProfiler.Log4Net
Log4Net Prodiver  allows save Profiler results into log

All what you need is:

```cSharp
var logger = LogManager.GetLogger("Logger"); //Init Log4Net Logger

MiniProfilerEx.SetUpLog4Net(logger); //Set up profiler with logger
```

and use Profiler as usual (see [MiniProfiler site] (http://miniprofiler.com/) )
