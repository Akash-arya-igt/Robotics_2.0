using Serilog;
using Serilog.Sinks.SumoLogic;
using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.RoboticsLogger
{
    public class RoboticsSumoLogger: IRoboticsLogger
    {
        ILogger _logger;

        public RoboticsSumoLogger(string sumoCollectorURL, string sourceName)
        {
            _logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.SumoLogic(sumoCollectorURL, sourceName).CreateLogger();
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
