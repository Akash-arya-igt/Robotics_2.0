using System;
using IGT.Webjet.GDSEngine;
using IGT.Webjet.CommonUtil;
using IGT.Webjet.CloudEngine;
using IGT.Webjet.RoboticsLogger;
using IGT.Webjet.MessageComposer;
using IGT.Webjet.BusinessEntities;
using IGT.Webjet.BusinessEntities.Enum;
using IGT.Webjet.ServiceConfigurationEngine;


namespace IGT.Webjet.ReadQueueService
{
    class ReadGDSQueue
    {
        private int _qNumber = -1;

        private string _pcc = string.Empty;
        private string _logPath = string.Empty;
        private string _serviceName = string.Empty;
        private string _pnrMovementMsg = string.Empty;

        private PNRMsgTemplateEnum _msgTemplate;

        private IGDSProvider _gdsPro = null;
        private IRoboticsLogger _roboticsLogger = null;
        private IMsgQProvider _objCloudLoggingQ = null;
        private IMsgQProvider _objDestinationCloudQ = null;

        public ReadGDSQueue(GDSServiceConfig _pServiceConfig, IRoboticsLogger _pRoboticsLogger, IMsgQProvider _pCloudLoggingQ)
        {
            _pcc = _pServiceConfig.PCC;
            _qNumber = _pServiceConfig.GalQNumber;
            _logPath = _pServiceConfig.LocalLogPath;
            _serviceName = _pServiceConfig.ServiceName;
            _msgTemplate = _pServiceConfig.MsgTemplate;

            _roboticsLogger = _pRoboticsLogger;
            _objCloudLoggingQ = _pCloudLoggingQ;

            _pnrMovementMsg = "PNR: {0}" + Environment.NewLine  + string.Format(Constants.PNR_REMOVED_FROM_GAL_Q, _qNumber);

            _gdsPro = GDSFactory.GetGDSProvider(_pServiceConfig.GDSAuthDetail);

            if (!string.IsNullOrEmpty(_pServiceConfig.DestinationCloudQName))
            {
                _objDestinationCloudQ = CloudObjectFactory.GetMsgQProvider(_pServiceConfig.CloudProvider,
                                                                           _pServiceConfig.DestinationCloudQName,
                                                                           _pServiceConfig.ConnectionString);

                _pnrMovementMsg = _pnrMovementMsg + Environment.NewLine + 
                                  string.Format(Constants.PNR_ADDED_TO_CLOUD_Q, _pServiceConfig.DestinationCloudQName);
            }
        }

        public void ScanGDSQueue()
        {
            int currentQKnt = 0;

            try
            {
                #region GET QUEUE COUNT
                try
                {
                    currentQKnt = _gdsPro.GetQCount(_qNumber, _pcc);
                }
                catch (Exception ex)
                {
                    currentQKnt = 0;
                    LogMsg logMsg = LoggingMsgComposer.GetLogMsg(RoboticsLogLevelEnum.Error,
                                                                 Constants.Q_COUNT_ERROR_MSG + Environment.NewLine + ex.Message,
                                                                 _serviceName, _qNumber);
                    _objCloudLoggingQ.AddMsgToLoggingQ(logMsg);
                }
                #endregion

                if (currentQKnt > 0)
                {
                    ReadQueuePNRMsg currentPNR = null;
                    ReadQueuePNRMsg previousPNR = null;

                    //READ QUEUE
                    currentPNR = _gdsPro.ReadQueue(_qNumber, string.Empty, _msgTemplate);

                    //TRAVERSE GAL QUEUE TO REMOVE PNR AND ADD TO AZURE Q
                    for (int i = 0; i < currentQKnt; i++)
                    {
                        if (currentPNR != null && !string.IsNullOrEmpty(currentPNR.Recloc))
                        {
                            currentPNR.PCC = _pcc;
                            currentPNR.FromQueue = _qNumber;
                            currentPNR.StartTime = DateTime.Now;

                            previousPNR = null;
                            previousPNR = currentPNR.DeepClone();

                            //_pnrMovementMsg = string.Format(_pnrMovementMsg, 

                            // REMOVE CURRENT PNR AND READ NEXT
                            currentPNR = _gdsPro.RemoveNGetNextPNRMsg(string.Empty, _msgTemplate);

                            if (_objDestinationCloudQ != null)
                            {
                                //PUSH MSG INTO AZURE QUEUE (/////IF IT FAILS THEN ADD PNR BACK TO GAL QUEUE/////)
                                _objDestinationCloudQ.AddPNRToCloudQ(previousPNR);
                            }

                            //ADD MESSAGE TO LOGGING QUEUE
                            LogMsg logMsg = LoggingMsgComposer.GetLogMsg(RoboticsLogLevelEnum.Info, _pnrMovementMsg, _serviceName, _qNumber);
                            _objCloudLoggingQ.AddMsgToLoggingQ(logMsg);
                        }
                        else
                            break;

                        currentQKnt = currentQKnt - 1;
                    }

                    try { _gdsPro.CloseSession(string.Empty); } catch { }
                }
            }
            catch (Exception ex)
            {
                //LOG EXECEPTION (/////// RESOLVE ISSUE OF SESSION OUT)
                if (_gdsPro != null)
                    try { _gdsPro.CloseSession(string.Empty); } catch(Exception SessionCloseEx) { }

                try
                {
                    
                    LogMsg logMsg = LoggingMsgComposer.GetLogMsg(RoboticsLogLevelEnum.Error,
                                                                 Constants.OUTER_CATCH_BLOCK_ERROR_MSG + Environment.NewLine + 
                                                                 ex.Message + Environment.NewLine +
                                                                 ex.StackTrace, _serviceName, _qNumber);
                    
                    _objCloudLoggingQ.AddMsgToLoggingQ(logMsg);
                }
                catch (Exception innerEx)
                {
                    //LOG into local file
                    _roboticsLogger.WriteLog(RoboticsLogLevelEnum.Error,
                                             LoggingMsgComposer.GetLogMsg(RoboticsLogLevelEnum.Error,
                                                                          Constants.OUTER_CATCH_BLOCK_ERROR_MSG + Environment.NewLine +
                                                                          innerEx.Message + Environment.NewLine +
                                                                          innerEx.StackTrace, _serviceName, _qNumber).ToString());
                }
            }
        }
    }
}
