using System;
using IGT.Webjet.CloudEngine;
using IGT.Webjet.RoboticsLogger;
using IGT.Webjet.MessageComposer;
using IGT.Webjet.BusinessEntities.Enum;
using IGT.Webjet.ServiceConfigurationEngine;


namespace IGT.Webjet.LoggingService
{
    class LogDumper
    {
        private IMsgQProvider _objMsgQ = null;
        private IRoboticsLogger _objFileLogger = null;
        private IRoboticsLogger _objSumoLogger = null;

        private string _serviceName = string.Empty;

        public LogDumper(LoggingConfig _pConfig, IRoboticsLogger _pFileLogger)
        {
            _objFileLogger = _pFileLogger;
            _serviceName = _pConfig.ServiceName;
            _objSumoLogger = RoboticsLoggerFactory.GetRoboticsLogger(BusinessEntities.Enum.RoboticsLoggerEnum.SumoLogger,
                                                                     string.Empty, _pConfig.SumoCollectionURL, _pConfig.SourceName);

            _objMsgQ = CloudObjectFactory.GetMsgQProvider(_pConfig.CloudProvider, _pConfig.CloudLoggingQName, _pConfig.ConnectionString);
        }

        public void AddLogMsgToSink()
        {
            try
            {
                var logMsg = _objMsgQ.GetLogMsg();

                while(logMsg != null)
                {
                    _objSumoLogger.WriteLog(logMsg.LogLevel, logMsg.LogMessage);
                    logMsg = _objMsgQ.GetLogMsg();
                }
            }
            catch(Exception ex)
            {
                try
                {
                    _objSumoLogger.WriteLog(RoboticsLogLevelEnum.Error, 
                                            LoggingMsgComposer.GetStringLogMsg(ex.Message + Environment.NewLine +  ex.StackTrace, _serviceName));
                }
                catch(Exception innerEx)
                {
                    _objFileLogger.WriteLog(RoboticsLogLevelEnum.Error,
                                            LoggingMsgComposer.GetStringLogMsg(innerEx.Message + Environment.NewLine + innerEx.StackTrace, _serviceName));
                }
            }
        }
    }
}
