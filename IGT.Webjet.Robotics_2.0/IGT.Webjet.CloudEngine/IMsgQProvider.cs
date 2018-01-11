using System.Collections.Generic;
using IGT.Webjet.BusinessEntities;

namespace IGT.Webjet.CloudEngine
{
    public interface IMsgQProvider
    {
        void AddMsgToLoggingQ(LogMsg _pLogMsg);
        LogMsg GetLogMsg();
        void AddPNRToCloudQ<T>(T _pPNR) where T : ReadQueuePNRMsg;
        string ReadMsg();
        string GetMsg();
        int GetQCount();
        List<string> GetMsgs(int _pMsgCount, int _pMsgVisibilityTimeout);
    }
}
