using Serilog;
using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.RoboticsLogger
{
    public class RoboticsFileLogger : IRoboticsLogger
    {
        ILogger _logger;

        public RoboticsFileLogger(string filePath)
        {
            _logger = new LoggerConfiguration()
                        .WriteTo.File(filePath, rollingInterval: RollingInterval.Day,
                                      retainedFileCountLimit: 10, rollOnFileSizeLimit: true,
                                      fileSizeLimitBytes: 1024)
                        .CreateLogger();
        }

        public void WriteLog(RoboticsLogLevelEnum logLevel, string logString)
        {
            switch (logLevel)
            {
                case RoboticsLogLevelEnum.Error:
                    _logger.Error(logString);
                    break;

                case RoboticsLogLevelEnum.Warning:
                    _logger.Warning(logString);
                    break;

                case RoboticsLogLevelEnum.Info:
                    _logger.Information(logString);
                    break;

                case RoboticsLogLevelEnum.Debug:
                    _logger.Debug(logString);
                    break;

                default:
                    _logger.Information(logString);
                    break;
            }
        }
    }
}
