using Serilog;
using Serilog.Events;

namespace User_API.Configurations
{
    public class ConfigureLogger
    {
        public static void CongigureGlobalLogger()
        {
            Log.Logger = new LoggerConfiguration() // Log is a class in Serilog library thata allows logging messages when an error or exception occurs.

            //writing logs to log files based on some conditions.
            .WriteTo.Conditional(
                logEvent => logEvent.Level == LogEventLevel.Information,
                wt => wt.File("Logs/ApiResponses.txt", rollingInterval: RollingInterval.Day))

            .WriteTo.Conditional(
                logEvent => logEvent.Level == LogEventLevel.Error,
                wt => wt.File("Logs/ExceptionLogs.txt", rollingInterval: RollingInterval.Day))

            .WriteTo.File("Logs/GeneralLog.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger(); // make a class to call this file 
        }
    }
}
