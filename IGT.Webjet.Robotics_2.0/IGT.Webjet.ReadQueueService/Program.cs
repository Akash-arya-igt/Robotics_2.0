using System;
using System.Threading;
using IGT.Webjet.RoboticsLogger;
using IGT.Webjet.ServiceConfigurationEngine;
using IGT.Webjet.CloudEngine;
using IGT.Webjet.BusinessEntities;
using IGT.Webjet.MessageComposer;
using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.ReadQueueService
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadGDSQueue objGDSQScanner = null;

            ///////Add settings in Azure table
            GDSServiceConfig objServiceConfig = new GDSServiceConfig();
            ///////Validate Configuration settings


            IRoboticsLogger objFileLogger = RoboticsLoggerFactory.GetRoboticsLogger(RoboticsLoggerEnum.FileLogger,
                                                                                    objServiceConfig.LocalLogPath, string.Empty, string.Empty);

            IMsgQProvider _objCloudLoggingQ = CloudObjectFactory.GetMsgQProvider(objServiceConfig.CloudProvider,
                                                                                 objServiceConfig.CloudLoggingQName,
                                                                                 objServiceConfig.ConnectionString);

            var logServiceStartMsg = LoggingMsgComposer.GetServiceStartLogMsg(objServiceConfig.ServiceName);
            _objCloudLoggingQ.AddMsgToLoggingQ(logServiceStartMsg);

            while (true)
            {
                try
                {
                    ///////read settings from Azure table and referesh the settings
                    objGDSQScanner = new ReadGDSQueue(objServiceConfig, objFileLogger, _objCloudLoggingQ);
                    objGDSQScanner.ScanGDSQueue();
                }
                catch(Exception ex)
                {
                    try
                    {
                        LogMsg logMsg = LoggingMsgComposer.GetLogMsg(RoboticsLogLevelEnum.Error,
                                                                     "Parent Block: " + Constants.OUTER_CATCH_BLOCK_ERROR_MSG + Environment.NewLine +
                                                                     ex.Message + Environment.NewLine + ex.StackTrace, 
                                                                     objServiceConfig.ServiceName, objServiceConfig.GalQNumber);
                        _objCloudLoggingQ.AddMsgToLoggingQ(logMsg);
                    }
                    catch (Exception innerEx)
                    {
                        //LOG into local file
                        try
                        {
                            objFileLogger.WriteLog(RoboticsLogLevelEnum.Error, 
                                                   LoggingMsgComposer.GetStringLogMsg("Parent Block: " + Constants.OUTER_CATCH_BLOCK_ERROR_MSG +
                                                   Environment.NewLine + innerEx.Message + Environment.NewLine + innerEx.StackTrace,
                                                   objServiceConfig.ServiceName, objServiceConfig.GalQNumber));
                        }
                        catch { /* SUPPRESS EXCEPTTION TO RESTART THE FLOW AFTER DELAY */ }
                    }
                }
                finally
                {
                    objGDSQScanner = null;
                }

                Thread.Sleep(1000 * objServiceConfig.DelayInterval);
            }
        }
    }
}
