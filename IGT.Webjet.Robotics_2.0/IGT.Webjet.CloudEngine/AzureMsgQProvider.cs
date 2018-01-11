using IGT.Webjet.AzureHelper;
using System.Collections.Generic;
using IGT.Webjet.BusinessEntities;
using IGT.Webjet.CommonUtil;

namespace IGT.Webjet.CloudEngine
{
    public class AzureMsgQProvider : IMsgQProvider
    {
        private AzureQueueHelper _objQHelper;

        public AzureMsgQProvider(string _pQName, string _pConnstionstring)
        {
            _objQHelper = new AzureQueueHelper(_pQName, _pConnstionstring);
        }


        //public void AddMsg(string _pMsg)
        //{
        //    _objQHelper.AddMsg(_pMsg);
        //}

        public void AddMsgToLoggingQ(LogMsg _pLogMsg)
        {
            _objQHelper.AddMsg(_pLogMsg.ToString());
        }

        public void AddPNRToCloudQ<T>(T _pPNR) where T : ReadQueuePNRMsg
        {
            _objQHelper.AddMsg(_pPNR.ToString());
        }

        public LogMsg GetLogMsg()
        {
            LogMsg logMsg = null;
            var azureQMsg = _objQHelper.GetMsg();

            if(!string.IsNullOrEmpty(azureQMsg))
                logMsg = GenericUtil.GetObjectFromJson<LogMsg>(azureQMsg);

            return logMsg;
        }

        public string GetMsg()
        {
            return _objQHelper.GetMsg();
        }

        public List<string> GetMsgs(int _pMsgCount, int _pMsgVisibilityTimeout)
        {
            return _objQHelper.GetMsgs(_pMsgCount, _pMsgVisibilityTimeout);
        }

        public int GetQCount()
        {
            return _objQHelper.GetQCount();
        }

        public string ReadMsg()
        {
            return _objQHelper.ReadMsg();
        }
    }
}
