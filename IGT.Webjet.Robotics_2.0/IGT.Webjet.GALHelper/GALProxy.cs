using System;
using System.Xml;
using System.Text;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using IGT.Webjet.CommonUtil;

namespace IGT.Webjet.GALHelper
{
    public class GALProxy
    {
        public string Url { get; set; }
        public string Profile { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public const string WebServiceNS = "http://webservices.galileo.com";
        private XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";


        public async Task<XmlElement> SubmitRequestAsync(GalWebMethod _methodName, string _token, XmlElement _request, string _filter)
        {
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(UserName + ":" + Password));

            XDocument soapEnvelopeXml = GetSoapEnvelope(_methodName, _token, _request.GetXElement(), _filter);
            HttpClient httpClient = new HttpClient();
            HttpContent httpContent = new StringContent(soapEnvelopeXml.ToString());
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, Url);
            req.Headers.Add("SOAPAction", "\"" + WebServiceNS + _methodName.ToString() + "\"");
            req.Headers.Add("Authorization", "Basic " + svcCredentials);
            req.Method = HttpMethod.Post;
            req.Content = httpContent;
            req.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml; charset=utf-8");

            HttpResponseMessage response;
            response = await httpClient.SendAsync(req);
            var responseBodyAsText = await response.Content.ReadAsStringAsync();

            #region C O M M E N T E D  C O D E  U S I N G   HttpWebRequest
            /*
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);
            webRequest.Headers["SOAPAction"] = "http://webservices.galileo.com/" + _methodName.ToString();
            webRequest.Headers["Authorization"] = "Basic " + svcCredentials;
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            
            
            using (var stream = await Task.Factory.FromAsync<Stream>(webRequest.BeginGetRequestStream, webRequest.EndGetRequestStream, null))
            {
                soapEnvelopeXml.Save(stream);
            }



            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
            }
            */
            #endregion

            return GetResponseBody(_methodName, responseBodyAsText);
        }

        public AuthTagName GetAuthTagName(GalWebMethod _methodName)
        {
            if (_methodName == GalWebMethod.EndSession || _methodName == GalWebMethod.SubmitXmlOnSession
                || _methodName == GalWebMethod.SubmitTerminalTransaction)
                return AuthTagName.Token;
            else
                return AuthTagName.Profile;
        }

        private XDocument GetSoapEnvelope(GalWebMethod _methodName, string _token, XElement _request, string _filter)
        {
            XNamespace methodNS = WebServiceNS;
            XElement xmlRequestBody = new XElement(methodNS + _methodName.ToString(),
                                                    new XAttribute("xmlns", WebServiceNS));

            if (GetAuthTagName(_methodName) == AuthTagName.Token)
                xmlRequestBody.Add(new XElement(AuthTagName.Token.ToString(), _token));
            else
                xmlRequestBody.Add(new XElement(AuthTagName.Profile.ToString(), Profile));

            if (_methodName != GalWebMethod.SubmitTerminalTransaction)
                xmlRequestBody.Add(new XElement("LDVOverride", string.Empty));

            if (_methodName != GalWebMethod.BeginSession && _methodName != GalWebMethod.EndSession)
                xmlRequestBody.Add(new XElement("Request", _request));

            xmlRequestBody.Add(new XElement("Filter", new XElement("_", new XAttribute("xmlns", _filter))));

            XDocument docBody = new XDocument(new XDeclaration("1.0", "utf-8", String.Empty),
                                                new XElement(soap + "Envelope",
                                                              new XAttribute(XNamespace.Xmlns + "soap", soap),
                                                              new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                                              new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                                                              new XElement(soap + "Body", xmlRequestBody)));

            return docBody;
        }

        public XmlElement GetResponseBody(GalWebMethod _method, string _rawResponse)
        {
            string xPath = string.Empty;
            XElement responseBody = null;
            XElement tempResponse = XmlDomUtil.RemoveAllNamespaces(XElement.Parse(_rawResponse));

            switch (_method)
            {
                case GalWebMethod.BeginSession:
                    xPath = "/Body/BeginSessionResponse/BeginSessionResult";
                    break;
                case GalWebMethod.EndSession:
                    xPath = "/Body/EndSessionResponse";
                    break;
                case GalWebMethod.GetIdentityInfo:
                    xPath = "Body/GetIdentityInfoResponse";
                    break;
                case GalWebMethod.MultiSubmitXml:
                    xPath = "Body";
                    break;
                case GalWebMethod.SubmitCruiseTransaction:
                    xPath = "Body";
                    break;
                case GalWebMethod.SubmitTerminalTransaction:
                    xPath = "Body/SubmitTerminalTransactionResponse/SubmitTerminalTransactionResult";
                    break;
                case GalWebMethod.SubmitXml:
                    xPath = "Body/SubmitXmlResponse/SubmitXmlResult";
                    break;
                case GalWebMethod.SubmitXmlOnSession:
                    xPath = "Body/SubmitXmlOnSessionResponse/SubmitXmlOnSessionResult";
                    break;
                default:
                    xPath = "NoImplementation";
                    break;
            }

            responseBody = XmlDomUtil.GetChildElement(tempResponse, xPath);
            var respBody = responseBody.GetXmlDocument();

            return respBody != null ? respBody.DocumentElement : null;
        }
    }

    public enum AuthTagName
    {
        Profile,
        Token
    }

    public enum GalWebMethod
    {
        SubmitXml,
        MultiSubmitXml,
        SubmitCruiseTransaction,
        BeginSession,
        EndSession,
        SubmitXmlOnSession,
        SubmitTerminalTransaction,
        GetIdentityInfo
    }
}
