using System.Xml;
using IGT.Webjet.GALHelper;
using IGT.Webjet.BusinessEntities;
using IGT.Webjet.BusinessEntities.Enum;
using IGT.Webjet.GALHelper.GALRequestProcessor;

namespace IGT.Webjet.GDSEngine
{
    public class GALProvider : IGDSProvider
    {
        GALConnect objGalConn;

        public GALProvider(GDSServiceAuthDetail _credentialObject)
        {
            GDSServiceAuthDetail objHAP = _credentialObject;
            objGalConn = new GALConnect(objHAP);
        }

        public void CloseSession(string _session)
        {
            if (!string.IsNullOrEmpty(_session))
                objGalConn.SessionID = _session;

            objGalConn.CloseSession();
        }

        public int GetQCount(int _qNumber, string _pcc)
        {
            int qCount = 0;

            QueueProcessor objQProc = new QueueProcessor(objGalConn);
            qCount = objQProc.GetQueueCount(_qNumber, _pcc);

            return qCount;
        }

        public ReadQueuePNRMsg ReadQueue(int _qNumber, string _session, PNRMsgTemplateEnum _template)
        {
            ReadQueuePNRMsg pnrMsg = null;
            if (!string.IsNullOrEmpty(_session)) objGalConn.SessionID = _session;

            QueueProcessor objQProc = new QueueProcessor(objGalConn);
            XmlElement xmlPNR = objQProc.ReadPNRFromQ(_qNumber);

            pnrMsg = GetPNRMsg(xmlPNR, _template);
            return pnrMsg;
        }

        public ReadQueuePNRMsg RemoveNGetNextPNRMsg(string _session, PNRMsgTemplateEnum _template)
        {
            ReadQueuePNRMsg pnrMsg = null;
            if (!string.IsNullOrEmpty(_session)) objGalConn.SessionID = _session;

            QueueProcessor objQProc = new QueueProcessor(objGalConn);
            XmlElement xmlPNR = objQProc.RemoveNReadNextPNR();

            pnrMsg = GetPNRMsg(xmlPNR, _template);
            return pnrMsg;
        }

        public ReadQueuePNRMsg GetPNRMsg(XmlElement xmlPNR, PNRMsgTemplateEnum _template)
        {
            ReadQueuePNRMsg pnrMsg = null;
            string recloc = ReclocProcessor.GetReclocFromPNRXml(xmlPNR);

            switch (_template)
            {
                case PNRMsgTemplateEnum.PNRMsg:
                    pnrMsg = new ReadQueuePNRMsg()
                    {
                        Recloc = recloc
                    };
                    break;

                case PNRMsgTemplateEnum.VendorRemarkPNRMsg:
                    pnrMsg = new VendorRemarkQueuePNRMsg()
                    {
                        Recloc = recloc,
                        GeneralRemark = ReclocProcessor.GetGeneralRemarks(xmlPNR),
                        VendorRemark = ReclocProcessor.GetVendorRemarks(xmlPNR),
                    };
                    break;

                default:
                    pnrMsg = new ReadQueuePNRMsg()
                    {
                        Recloc = recloc
                    };
                    break;
            }

            return pnrMsg;
        }
    }
}
