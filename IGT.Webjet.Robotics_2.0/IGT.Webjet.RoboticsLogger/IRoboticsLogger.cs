using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.RoboticsLogger
{
    public interface IRoboticsLogger
    {
        void WriteLog(RoboticsLogLevelEnum logLevel, string logString);
    }
}
