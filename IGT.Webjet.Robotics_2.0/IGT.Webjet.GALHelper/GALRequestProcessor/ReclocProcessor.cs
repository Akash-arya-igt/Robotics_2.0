using System.Xml;
using IGT.Webjet.CommonUtil;
using System.Collections.Generic;

namespace IGT.Webjet.GALHelper.GALRequestProcessor
{
    public class ReclocProcessor
    {
        private GALConnect _GalConn;

        public ReclocProcessor(GALConnect galConnect)
        {
            _GalConn = galConnect;
        }

        public static string GetReclocFromPNRXml(XmlElement _pnrXml)
        {
            string strRecloc = string.Empty;

            if (_pnrXml != null)
                strRecloc = XmlDomUtil.GetChildNodeString(_pnrXml, "//GenPNRInfo/RecLoc");

            return strRecloc;
        }

        public static List<string> GetGeneralRemarks(XmlElement _pnrXml)
        {
            List<string> genRemarks = new List<string>();
            XmlNodeList nodes = _pnrXml.SelectNodes("//GenRmkInfo");
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    string remark = XmlDomUtil.GetChildNodeString(nodes[i], "GenRmk").Trim();
                    genRemarks.Add(remark);
                }
            }

            nodes = null;
            return genRemarks;
        }

        public static List<string> GetVendorRemarks(XmlElement _pnrXml)
        {
            List<string> vendorRemarks = new List<string>();

            XmlNodeList vndRmkList = _pnrXml.SelectNodes("//VndRmk");
            for (int i = 0; i < vndRmkList.Count; i++)
            {
                string remark = XmlDomUtil.GetChildNodeString(vndRmkList[i], "Rmk").Trim();
                vendorRemarks.Add(remark);
            }

            return vendorRemarks;
        }
    }
}
