using System.Xml;
using System.Threading.Tasks;
using IGT.Webjet.BusinessEntities;

namespace IGT.Webjet.GALHelper
{
    public class GALConnect : GALProxy
    {
        public string SessionID { get { return _token; } set { _token = value; } }
        private string _token = "";
        private string _filter;
        private GDSServiceAuthDetail _hapDetail;

        public GALConnect(GDSServiceAuthDetail _pHAPDetail)
        {
            _filter = string.Empty;
            _hapDetail = _pHAPDetail;

            base.UserName = _pHAPDetail.UserID;
            base.Password = _pHAPDetail.Password;
            base.Url = _pHAPDetail.GWSConnURL;
            base.Profile = _pHAPDetail.Profile;
        }

        public void CreateSession()
        {
            string strXmlReq = @"<Profile>DynGalileoCopy_5KL6</Profile>";
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(strXmlReq);

            var resp = GalConnSubmitRequest(GalWebMethod.BeginSession, xml.DocumentElement);
            SessionID = resp.InnerText;
        }

        public void CloseSession()
        {
            if(!string.IsNullOrEmpty(_token))
            {
                string strXmlReq = @"<Token>" + _token + "</Token>";
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(strXmlReq);

                var resp = GalConnSubmitRequest(GalWebMethod.EndSession, xml.DocumentElement);
                SessionID = string.Empty;
            }
        }

        public XmlElement GalConnSubmitRequest(GalWebMethod _galMethod, XmlElement _request)
        {
            if (GetAuthTagName(_galMethod) == AuthTagName.Token && string.IsNullOrEmpty(_token))
                CreateSession();

            var resultBodyTask = this.SubmitRequestAsync(_galMethod, _token, _request, _filter);
            resultBodyTask.Wait();
            return resultBodyTask.Result;
        }

        public async Task<XmlElement> GalConnSubmitRequestAsync(GalWebMethod _galMethod, XmlElement _request)
        {
            var resultBody = await this.SubmitRequestAsync(_galMethod, _token, _request, _filter);
            return resultBody;
        }
    }
}
