using System;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace IGT.Webjet.CommonUtil
{
    public static class XmlDomUtil
    {
        public static XmlDocument InitEmptyXmlDocument()
        {
            XmlDocument filter = new XmlDocument();
            filter.LoadXml("<_/>");
            return filter;
        }

        //copied a few methods from main\TSA\ServiceAgents\GWSConnect\GWSCommon.cs
        /// <summary>
        /// Safely gets the inner text of any node (checks for null)
        /// </summary>
        /// <param name="xmlNode">An XML node</param>
        /// <returns>A string representation of the inner text
        /// Returns empty string in note is null
        /// </returns>
        public static string GetXmlNodeString(this XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                return xmlNode.InnerText;
            }
            else
            {
                return "";
            }
        }

        public static string GetXmlNodeString(string xmlString, string xPath)
        {
            string xmlNodeString = string.Empty;

            if (!string.IsNullOrEmpty(xmlString))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);

                XmlNodeList xmlStringNodeList = xmlDoc.SelectNodes(xPath);

                foreach (XmlNode xmlStringNode in xmlStringNodeList)
                {
                    //Yong wanted to replace "-" to "."
                    xmlNodeString = string.Concat(xmlNodeString, xmlStringNode.InnerText, "-");
                }
            }

            return xmlNodeString;
        }

        public static string GetXmlNodeInnerXmlSafe(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                return xmlNode.InnerXml;
            }
            else
            {
                return "";
            }
        }
        /// <returns>A string representation of the child inner text</returns>
        public static string GetChildNodeString(this XmlNode xmlParent, string xpathChild)
        {
            if (xmlParent == null) { throw new ArgumentNullException("xmlParent"); }
            XmlNode xmlNode = xmlParent.SelectSingleNode(xpathChild);
            return GetXmlNodeString(xmlNode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlParent"></param>
        /// <param name="xpathChild"></param>
        /// <param name="nsm"></param>
        /// <returns>Returns empty string in note was not found</returns>
        public static string GetChildNodeString(this XmlNode xmlParent, string xpathChild, XmlNamespaceManager nsm)
        {
            if (xmlParent == null) { throw new ArgumentNullException("xmlParent"); }
            XmlNode xmlNode = xmlParent.SelectSingleNode(xpathChild, nsm);
            return GetXmlNodeString(xmlNode);
        }

        /// <summary>
        /// Safely gets the inner text of a node as an integer (checks for null)
        /// </summary>
        /// <param name="xmlNode">An XML node</param>
        /// <returns>An integer representation of the inner text</returns>
        public static int GetXmlNodeInteger(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                try
                {
                    Double tempValue;
                    if (Double.TryParse(xmlNode.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out tempValue))
                    {
                        return Convert.ToInt32(tempValue);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception)
                {

                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Safely gets the inner text of a node as an integer (checks for null)
        /// </summary>
		/// <param name="xmlParent">An XML node</param>
        /// <returns>An integer representation of the inner text</returns>
        public static int GetChildNodeInteger(this XmlNode xmlParent, string xpathChild)
        {
            if (xmlParent == null) { throw new ArgumentNullException("xmlParent"); }
            XmlNode xmlNode = xmlParent.SelectSingleNode(xpathChild);
            return GetXmlNodeInteger(xmlNode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlParent"></param>
        /// <param name="nsmgr"></param>
        /// <param name="xpathChild">e.g. "g:FareComponent"</param>
        /// <returns></returns>
        public static int GetChildNodeInteger(this XmlNode xmlParent, XmlNamespaceManager nsmgr, string xpathChild)
        {
            if (xmlParent == null) { throw new ArgumentNullException("xmlParent"); }
            XmlNode xmlNode = xmlParent.SelectSingleNode(xpathChild, nsmgr);
            return GetXmlNodeInteger(xmlNode);
        }

        /// <summary>
        /// Safely gets the inner text of a node as an char (checks for null)
        /// </summary>
        /// <param name="xmlNode">An XML node</param>
        /// <returns>An char representation of the inner text</returns>
        public static char GetXmlNodeChar(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                try
                {
                    return System.Convert.ToChar(xmlNode.InnerText, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return (char)0;
                }
            }
            else
            {
                return (char)0;
            }
        }

        /// <summary>
        /// Safely gets the inner text of a node as a decimal (checks for null)
        /// </summary>
        /// <param name="xmlNode">An XML node</param>
        /// <returns>A decimal representation of the inner text</returns>
        public static decimal GetXmlNodeDecimal(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                try
                {
                    return System.Convert.ToDecimal(xmlNode.InnerText, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Safely gets the inner text of a node as a decimal (checks for null)
        /// </summary>
        /// <param name="xmlNode">An XML node</param>
        /// <returns>A decimal representation of the inner text</returns>
        public static decimal GetChildNodeDecimal(XmlNode xmlParent, string xpathChild)
        {
            XmlNode xmlNode = xmlParent.SelectSingleNode(xpathChild);
            return GetXmlNodeDecimal(xmlNode);
        }

        /// <summary>
        /// Safely gets the inner text of a node as a date (checks for null)
        /// </summary>
        /// <param name="xmlNode">An XML node</param>
        /// <param name="format">A datetime format string</param>
        /// <returns>An date time representation of the inner text</returns>
        public static DateTime GetXmlNodeDateTime(XmlNode xmlNode, string format)
        {
            if (xmlNode != null)
            {
                DateTimeFormatInfo dtf = new DateTimeFormatInfo();
                dtf.FullDateTimePattern = format;

                try
                {
                    return DateTime.ParseExact(xmlNode.InnerText, format, dtf);
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime GetChildNodeDateTime(XmlNode xmlParent, string xpathChild)
        {
            return GetChildNodeDateTime(xmlParent, xpathChild, false);
        }
        public static DateTime GetChildNodeDateTime(XmlNode xmlParent, string xpathChild, bool ignoreZsuffix)
        {
            XmlNode xmlNode = xmlParent.SelectSingleNode(xpathChild);
            if (xmlNode != null)
            {
                //DateTimeFormatInfo dtf = new DateTimeFormatInfo();
                //dtf.FullDateTimePattern = format;

                try
                {
                    string sTime = xmlNode.InnerText;
                    if (ignoreZsuffix == true)
                    {  //for JQ All timeInstant variables are in local time, despite the fact that they are in the format yyyy-MM-dd'T'HH:mm:ss.SSSZ
                        sTime = sTime.TrimEnd('Z');
                    }
                    return DateTime.Parse(sTime);
                }
                catch (Exception ex)
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// extract CDATA
        /// </summary>
        /// <param name="xmlParent"></param>
        /// <param name="xPathChild"></param>
        /// <returns></returns>
        public static string GetChildNodeCDataString(this XmlNode xmlParent, string xPathChild)
        {
            string strResult = string.Empty;
            if (xmlParent == null) { throw new NullReferenceException("xmlParent is null"); }
            XmlNode acctNode = xmlParent.SelectSingleNode(xPathChild);
            if (acctNode != null)
            {
                XmlNode childNode = acctNode.ChildNodes[0];
                if (childNode is XmlCDataSection)
                {
                    XmlCDataSection cdataSection = childNode as XmlCDataSection;
                    strResult = cdataSection.Value;
                }
            }

            return strResult;

        }
        public static XmlNode FindNextSibling(this XmlNode startNode, string nodeName)
        {
            return FindNextSibling(startNode, nodeName, int.MaxValue);
        }
        public static XmlNode FindNextSibling(XmlNode startNode, string nodeName, int maxSiblings)
        {
            int k = 0;
            XmlNode returnNode = null;
            XmlNode testNode = startNode;
            while (k < maxSiblings)
            {
                k++;
                testNode = testNode.NextSibling;
                if (testNode == null || testNode.Name == startNode.Name)
                {
                    break;
                }
                if (testNode.Name == nodeName)
                {
                    returnNode = testNode;
                    break;
                }
                continue;
            }
            return returnNode;
        }
        public static XmlNode CloneSibling(XmlNode genAvailNode, string sInnerXml)
        {
            XmlNode newNode = CloneSibling(genAvailNode);
            newNode.InnerXml = sInnerXml;
            return newNode;
        }
        //
        /// <summary>
        ///the function create a new sibling  and appends to the end of the list of child nodes 
        /// More appropriate name may be CloneSiblingAsLastChild 
        /// </summary>
        /// <param name="existingNode"></param>
        /// <returns></returns>
		public static XmlNode CloneSibling(this XmlNode existingNode)
        {
            XmlNode parent = existingNode.ParentNode;
            XmlNode newNode = existingNode.Clone();
            parent.AppendChild(newNode);
            //parent.InsertAfter( newNode,existingNode);
            return newNode;
        }
        //the function create a new sibling and inserts it immidiately after the existingNode
        public static XmlNode CloneSiblingAfterExistingNode(this XmlNode existingNode)
        {
            XmlNode parent = existingNode.ParentNode;
            XmlNode newNode = existingNode.Clone();
            parent.InsertAfter(newNode, existingNode);
            return newNode;
        }
        //the function create a new sibling and inserts it immidiately before the existingNode
        public static XmlNode CloneSiblingBeforeExistingNode(this XmlNode existingNode)
        {
            XmlNode parent = existingNode.ParentNode;
            XmlNode newNode = existingNode.Clone();
            parent.InsertBefore(newNode, existingNode);
            return newNode;
        }
        public static XmlNode CloneSiblingInsertAfterNode(XmlNode existingNode, XmlNode insertAfterNode)
        {
            XmlNode parent = existingNode.ParentNode;
            XmlNode newNode = existingNode.Clone();
            parent.InsertAfter(newNode, insertAfterNode);
            return newNode;
        }
        public static XmlNode SetSingleNodeText(this XmlNode nodeCurrent, string xPath, string innerText)
        {
            if (nodeCurrent == null) { throw new ArgumentNullException("nodeCurrent"); }
            XmlNode changeNode = nodeCurrent.SelectSingleNode(xPath);
            if (changeNode == null)
            { throw new NullReferenceException("xPath " + xPath + " not found in node  " + nodeCurrent.OuterXml); }
            changeNode.InnerText = innerText;
            return changeNode;
        }
        /// <summary>
        /// Call this function if not sure, does the XML contains the xPath.
        /// If you always expect the xPath to exist, better to call SetSingleNodeText, which will throw exception for unexpected xml
        /// </summary>
        /// <param name="nodeCurrent"></param>
        /// <param name="xPath"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        public static XmlNode SetNodeTextIfExist(this XmlNode nodeCurrent, string xPath, string innerText)
        {
            if (nodeCurrent == null) { throw new ArgumentNullException("nodeCurrent"); }
            XmlNode changeNode = nodeCurrent.SelectSingleNode(xPath);
            if (changeNode != null)
            {
                changeNode.InnerText = innerText;
            }
            return changeNode;
        }
        public static XmlNode SetChildNodeCDataStringIfExist(XmlNode xmlParent, string xPathChild, string innerText)
        {
            XmlNode acctNode = xmlParent.SelectSingleNode(xPathChild);
            XmlNode childNode = acctNode.ChildNodes[0];
            if (childNode is XmlCDataSection)
            {
                XmlCDataSection cdataSection = childNode as XmlCDataSection;
                cdataSection.Value = innerText;
            }

            return childNode;
        }
        public static XmlNode SetSingleNodeBool(this XmlNode nodeCurrent, string xPath, bool BooleanValue)
        {
            return SetSingleNodeText(nodeCurrent, xPath, BooleanValue.ToString());
        }
        public static bool RemoveChildIfExist(this XmlNode nodeCurrent, string xPath)
        {
            bool bRet = false;
            XmlNode changeNode = nodeCurrent.SelectSingleNode(xPath);
            if (changeNode != null)
            {
                RemoveChild(nodeCurrent, xPath);
                bRet = true;
            }
            return bRet;
        }

        /// <summary>
        /// Removes child by specified xPath. Function can delete node,even if it is not direct child of the current node.
        /// </summary>
        /// <param name="nodeCurrent"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static XmlNode RemoveChild(this XmlNode nodeCurrent, string xPath)
        {
            if (nodeCurrent == null) { throw new ArgumentNullException("nodeCurrent"); }
            XmlNode childNode = nodeCurrent.SelectSingleNode(xPath);
            if (childNode == null)
            { throw new NullReferenceException("xPath " + xPath + " not found in node  " + nodeCurrent.OuterXml); }
            XmlNode directParent = childNode.ParentNode;
            directParent.RemoveChild(childNode);
            return nodeCurrent;
        }
        public static void RemoveNodeFromParent(this XmlNode node)
        {
            node.ParentNode.RemoveChild(node);
        }
        /// <summary>
        /// Returns levelsUp parent of the child
        /// </summary>
        /// <param name="qualNode"></param>
        /// <param name="levelsUp"> >1 to be useful</param>
        /// <returns></returns>
        public static XmlNode GetParentNode(XmlNode qualNode, int levelsUp)
        {
            XmlNode parentItemNode = qualNode;
            for (int i = 1; i <= levelsUp; i++)
            {
                parentItemNode = parentItemNode.ParentNode;
            }
            return parentItemNode;
        }

        /// <summary>
        /// Returns template section with ParentNode=null, separately returns Parent with node removed
        /// </summary>
        /// <param name="requestDoc"></param>
        /// <param name="airComponentParent"></param>
        /// <returns></returns>
        public static XmlNode ExtractTemplateNode(XmlDocument requestDoc, string sNodePath, out XmlNode Parent)
        {
            XmlNode nodeInTemplateDoc = requestDoc.SelectSingleNode(sNodePath);
            Parent = nodeInTemplateDoc.ParentNode;
            XmlNode nodeTemplate = nodeInTemplateDoc.CloneNode(true);
            Parent.RemoveChild(nodeInTemplateDoc);
            return nodeTemplate;
        }
        /// <summary>
        /// Returns template section with ParentNode=null, separately returns Parent with node removed
        /// </summary>
        /// <param name="requestDoc"></param>
        /// <param name="airComponentParent"></param>
        /// <returns></returns>
        public static XmlNode ExtractTemplateNode(XmlNode Parent, string sNodePath)
        {
            XmlNode nodeInTemplateDoc = Parent.SelectSingleNode(sNodePath);
            XmlNode nodeTemplate = nodeInTemplateDoc.CloneNode(true);
            Parent.RemoveChild(nodeInTemplateDoc);
            return nodeTemplate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeParent">Be careful not to SWAP parameters</param>
        /// <param name="nodeTemplate"></param>
        /// <returns></returns>
        public static XmlNode AppendClone(XmlNode nodeParent, XmlNode nodeTemplate)
        {
            XmlNode nodeCurrent = nodeTemplate.CloneNode(true);
            nodeParent.AppendChild(nodeCurrent);
            return nodeCurrent;
        }
        /// <summary>
        /// If any of parameters null, function will be skipped
        /// </summary>
        /// <param name="originDoc">TODO: Seems redundant, can be extracted from nodeParent</param>
        /// <param name="nodeParent"></param>
        /// <param name="newNode"></param>
        public static void AppendNewNodeFromOtherDoc(XmlDocument originDoc, XmlNode nodeParent, XmlNode newNode)
        {
            if (newNode != null && originDoc != null && nodeParent != null)
            {
                XmlNode importedNode = originDoc.ImportNode(newNode, true);

                nodeParent.AppendChild(importedNode);
            }
        }

        public static void InsertNewNodeFromOtherDocAfterRef(XmlDocument originDoc, XmlNode nodeParent, XmlNode newNode, XmlNode refNode)
        {
            if (newNode != null && originDoc != null && nodeParent != null && refNode != null)
            {
                XmlNode importedNode = originDoc.ImportNode(newNode, true);

                nodeParent.InsertAfter(importedNode, refNode);
            }
        }

        public static void InsertFragmentFromOtherDoc(XmlDocument argHostDoc, string argXPathToRefNode, XmlDocument argFragmentDoc)
        {
            if (argHostDoc != null)
            {
                XmlNode importedNode = argHostDoc.ImportNode(argFragmentDoc.DocumentElement, true);
                XmlNode referenceNodeInOriginalDoc = argHostDoc.SelectSingleNode(argXPathToRefNode);
                if (referenceNodeInOriginalDoc != null && importedNode != null)
                {
                    argHostDoc.DocumentElement.InsertAfter(importedNode, referenceNodeInOriginalDoc);
                }
            }
        }

        public static XmlNode GetLastOrAddNode(this XmlNode nodeParent, string name)
        {
            //From Francesco GetOrAddNode
            if (nodeParent == null) { throw new NullReferenceException("nodeParent"); }
            if (null == nodeParent[name])
                return nodeParent.AppendChild(nodeParent.OwnerDocument.CreateElement(name));
            return nodeParent.SelectNodes(".//" + name + "[last()]")[0];
        }

        //From http://blogs.msdn.com/ericwhite/archive/2008/12/22/convert-xelement-to-xmlnode-and-convert-xmlnode-to-xelement.aspx
        public static XElement GetXElement(this XmlNode node)
        {
            if (node == null)
                return null;
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }
        public static XDocument GetXDocument(this XmlNode node)
        {
            if (node == null)
                return null;
            var xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc;
        }

        public static XmlNode GetXmlNode(this XElement element)
        {
            if (element == null)
                return null;
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        public static XmlDocument GetXmlDocument(this XElement element)
        {
            if (element == null)
                return null;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(element.ToString());
            return xmlDoc;
        }
        /// <summary>
        /// If XmlNode is null retur null, otherwise  OuterXml string
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string OuterXmlOrNull(this XmlNode element)
        {
            return element != null ? element.OuterXml : null;
        }
        public static XElement RemoveAllNamespaces(XElement _xmlDocument)
        {
            if (!_xmlDocument.HasElements)
            {
                XElement xElement = new XElement(_xmlDocument.Name.LocalName);
                xElement.Value = _xmlDocument.Value;

                foreach (XAttribute attribute in _xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(_xmlDocument.Name.LocalName, _xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        /*
        public static XElement RemoveAllNamespaces(XElement _xmlDocument)
        {
            if (!_xmlDocument.HasElements)
            {
                XElement xElement = new XElement(_xmlDocument.Name.LocalName);
                xElement.Value = _xmlDocument.Value;

                foreach (XAttribute attribute in _xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(_xmlDocument.Name.LocalName, _xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }*/

        public static XElement GetChildElement(this XElement _xmlelement, string _path)
        {
            XElement childElement = _xmlelement;
            List<string> subPathList = _path.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToList();

            foreach (var subPath in subPathList)
            {
                childElement = childElement.Element(subPath);

                if (childElement == null)
                    return childElement;
            }

            if (childElement.Elements() != null && childElement.Elements().Count() > 0)
            {
                childElement = childElement.Elements().FirstOrDefault();
            }

            return childElement;
        }

    }
}
