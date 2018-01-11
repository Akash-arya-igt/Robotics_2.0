using System.Xml;
using IGT.Webjet.CommonUtil;
using System.Collections.Generic;

namespace IGT.Webjet.GALHelper.GALRequestProcessor
{
    public class QueueProcessor
    {
        private GALConnect _GalConn;

        public QueueProcessor(GALConnect _galConnect)
        {
            _GalConn = _galConnect;
        }

        public int GetQueueCount(int _queueNumber, string _pcc)
        {
            XmlElement requestXml = GetQueueCountRequest(_queueNumber, _pcc);
            XmlElement responseXml = _GalConn.GalConnSubmitRequest(GalWebMethod.SubmitXml, requestXml);

            string strKntPath = "QueueCount/HeaderCount/TotPNRBFCnt";
            return XmlDomUtil.GetChildNodeInteger(responseXml, strKntPath);
        }

        public Dictionary<int, int> GetCount(string _pcc)
        {
            int intQNum = 0;
            int intQKnt = 0;
            Dictionary<int, int> pccKnt = new Dictionary<int, int>();

            XmlElement requestXml = GetQueueCountRequest(-1, _pcc);
            XmlElement responseXml = _GalConn.GalConnSubmitRequest(GalWebMethod.SubmitXml, requestXml);
            
            var kntNodes = responseXml.SelectNodes("//GenRmkInfo");

            foreach (XmlNode node in kntNodes)
            {
                intQNum = 0;
                intQKnt = 0;

                var qNumNode = XmlDomUtil.GetChildNodeString(node, "QNum");
                var qKntNode = XmlDomUtil.GetChildNodeString(node, "TotPNRBFCnt");

                if (!string.IsNullOrEmpty(qNumNode) && !string.IsNullOrEmpty(qKntNode))
                {
                    if (int.TryParse(qNumNode, out intQNum)
                        && int.TryParse(qKntNode, out intQKnt))
                    {
                        pccKnt.Add(intQNum, intQKnt);
                    }
                }
            }

            return pccKnt;
        }

        private XmlElement GetQueueCountRequest(int _queueNumber, string _pcc)
        {
            XmlElement reqTemplate = FileUtil.GetXmlDocument(PathConstants.RootPath, PathConstants.QueueProcessingRequest).DocumentElement;

            string strActionPath = "QueueMods/QueueSignInCountListMods/Action";
            string strQNumPath = "QueueMods/QueueSignInCountListMods/PCCAry/PCCInfo/QNum";
            string strPCCPath = "QueueMods/QueueSignInCountListMods/PCCAry/PCCInfo/PCC";

            XmlDomUtil.SetNodeTextIfExist(reqTemplate, strActionPath, QueueProcessingAction.QCT.ToString());

            if (_queueNumber >= 0)
                XmlDomUtil.SetNodeTextIfExist(reqTemplate, strQNumPath, _queueNumber.ToString());

            if (!string.IsNullOrEmpty(_pcc))
                XmlDomUtil.SetNodeTextIfExist(reqTemplate, strPCCPath, _pcc);

            return reqTemplate;
        }

        public XmlElement ReadPNRFromQ(int _queueNumber)
        {
            XmlElement reqTemplate = FileUtil.GetXmlDocument(PathConstants.RootPath, PathConstants.QueueProcessingRequest).DocumentElement;
            reqTemplate.SetNodeTextIfExist("//Action", QueueProcessingAction.Q.ToString());
            reqTemplate.SetNodeTextIfExist("//QNum", _queueNumber.ToString());
            
            XmlElement response = _GalConn.GalConnSubmitRequest(GalWebMethod.SubmitXmlOnSession, reqTemplate);
            return response;
        }

        public XmlElement RemoveNReadNextPNR()
        {
            XmlElement response = null;

            if (!string.IsNullOrEmpty(_GalConn.SessionID))
            {
                XmlElement reqTemplate = FileUtil.GetXmlDocument(PathConstants.RootPath, PathConstants.QueueRemoveSignOut).DocumentElement;
                response = _GalConn.GalConnSubmitRequest(GalWebMethod.SubmitXmlOnSession, reqTemplate);
            }

            return response;
        }
    }

    public enum QueueProcessingAction
    {
        QCT,    //QueueCountAction
        Q,      //ReadQueueAction
        QXI,    //SignoutQueueAction
        QR,     //RemoveAction
        QLD,    //GetPNRListAction
        QLDS    //GetUniquePNRListAction
    }
}
