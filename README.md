# MiniProfiler.Log4Net
Log4Net Prodiver, allow save profiling result into log

All what you need it's:

```cSharp
var logger = LogManager.GetLogger("Logger"); //Init Log4Net Logger

MiniProfilerEx.SetUpLog4Net(logger); //Set up profiler with logger
```

and use Profiler ordinary (See [MiniProfiler site] (http://miniprofiler.com/) )
