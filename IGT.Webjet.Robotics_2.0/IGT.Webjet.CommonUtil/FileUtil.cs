using System.IO;
using System.Xml;

namespace IGT.Webjet.CommonUtil
{
    public static class FileUtil
    {
        public static string GetFileText(string _pFilePath, string _pFileName)
        {
            return File.ReadAllText(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), _pFilePath, _pFileName));
        }

        public static XmlDocument GetXmlDocument(string _pFilePath, string _pFileName)
        {
            string strRequest = GetFileText(_pFilePath, _pFileName);

            XmlDocument xmlTemplate = new XmlDocument();
            if (!string.IsNullOrEmpty(strRequest))
                xmlTemplate.LoadXml(strRequest);

            return xmlTemplate;
        }
    }
}
