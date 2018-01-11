using IGT.Webjet.CommonUtil;
using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.BusinessEntities
{
    public class LogMsg
    {
        public RoboticsLogLevelEnum LogLevel { get; set; }
        public string LogMessage { get; set; }

        public override string ToString()
        {
            return this.GetJsonString();
        }
    }
}
