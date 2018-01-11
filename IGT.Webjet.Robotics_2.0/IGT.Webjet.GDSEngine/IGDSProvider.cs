using IGT.Webjet.BusinessEntities;
using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.GDSEngine
{
    public interface IGDSProvider
    {
        void CloseSession(string _session);
        int GetQCount(int _qNumber, string _pcc);
        ReadQueuePNRMsg ReadQueue(int _qNumber, string _session, PNRMsgTemplateEnum _template);
        ReadQueuePNRMsg RemoveNGetNextPNRMsg(string _session, PNRMsgTemplateEnum _template);
    }
}
