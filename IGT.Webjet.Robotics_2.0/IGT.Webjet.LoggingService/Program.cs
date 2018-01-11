using IGT.Webjet.BusinessEntities.Enum;
using IGT.Webjet.MessageComposer;
using IGT.Webjet.RoboticsLogger;
using IGT.Webjet.ServiceConfigurationEngine;
using System;
using System.Threading;

namespace IGT.Webjet.LoggingService
{
    class Program
    {
        static void Main(string[] args)
        {
            LogDumper objLogDumper = null;
            LoggingConfig objLoggingConfig = new LoggingConfig();

            IRoboticsLogger objFileLogger = RoboticsLoggerFactory.GetRoboticsLogger(RoboticsLoggerEnum.FileLogger,
                                                                                    objLoggingConfig.LocalLogPath, string.Empty, string.Empty);

            var logServiceStartMsg = LoggingMsgComposer.GetServiceStartLogMsg(objLoggingConfig.ServiceName);
            //_objCloudLoggingQ.AddMsgToLoggingQ(logServiceStartMsg);

            while(true)
            {
                try
                {
                    objLogDumper = new LogDumper(objLoggingConfig, objFileLogger);
                    objLogDumper.AddLogMsgToSink();
                }
                catch(Exception ex)
                {
                    try
                    {
                        objFileLogger.WriteLog(RoboticsLogLevelEnum.Error,
                                               LoggingMsgComposer.GetStringLogMsg(ex.Message + Environment.NewLine +
                                                                                  ex.StackTrace, objLoggingConfig.ServiceName));
                    }
                    catch { }
                }
                finally
                {
                    objLogDumper = null;
                }

                Thread.Sleep(1000 * objLoggingConfig.DelayInterval);
            }
        }
    }
}
