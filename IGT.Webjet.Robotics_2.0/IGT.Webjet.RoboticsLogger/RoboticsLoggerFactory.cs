using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.RoboticsLogger
{
    public static class RoboticsLoggerFactory
    {
        public static IRoboticsLogger GetRoboticsLogger(RoboticsLoggerEnum _pRoboLogger, string _pFilePath,
                                                        string _pSumoCollectorURL, string _pSourceName)
        {
            IRoboticsLogger _roboLogger = null;

            switch(_pRoboLogger)
            {
                case RoboticsLoggerEnum.FileLogger:
                    _roboLogger = new RoboticsFileLogger(_pFilePath);
                    break;

                case RoboticsLoggerEnum.SumoLogger:
                    _roboLogger = new RoboticsSumoLogger(_pSumoCollectorURL, _pSourceName);
                    break;

                default:
                    _roboLogger = new RoboticsFileLogger(_pFilePath);
                    break;
            }

            return _roboLogger;
        }
    }
}
