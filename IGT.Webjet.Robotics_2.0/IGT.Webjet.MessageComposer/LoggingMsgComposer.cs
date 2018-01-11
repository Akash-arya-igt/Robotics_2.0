
using IGT.Webjet.BusinessEntities;
using IGT.Webjet.BusinessEntities.Enum;
using System;

namespace IGT.Webjet.MessageComposer
{
    public static class LoggingMsgComposer
    {
        public static LogMsg GetLogMsg(RoboticsLogLevelEnum _pLogLevel, string _pLogString, string _pServiceName, int _pQNumber)
        {
            LogMsg logMsg = new LogMsg()
            {
                LogLevel = _pLogLevel,
                LogMessage = GetStringLogMsg(_pLogString, _pServiceName, _pQNumber)
            };

            return logMsg;
        }

        public static string GetStringLogMsg(string _pLogString, string _pServiceName)
        {
            string errorLogMsgFormat = "From: " + _pServiceName + Environment.NewLine
                                        + "Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + Environment.NewLine
                                        + "Log Message: {0}" + Environment.NewLine;

            return string.Format(errorLogMsgFormat, _pLogString);
        }

        public static string GetStringLogMsg(string _pLogString, string _pServiceName, int _pQNumber)
        {
            string errorLogMsgFormat = "From: " + _pServiceName + "-" + _pQNumber + Environment.NewLine
                                        + "Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + Environment.NewLine 
                                        + "Log Message: {0}" + Environment.NewLine;

            return string.Format(errorLogMsgFormat, _pLogString);
        }

        public static LogMsg GetServiceStartLogMsg(string _pServiceName)
        {
            LogMsg logMsg = new LogMsg()
            {
                LogLevel = RoboticsLogLevelEnum.Info,
                LogMessage = _pServiceName + ": service is started at: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };

            return logMsg;
        }
    }
}
