using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MeetingModel
{
    public class XmlHelper
    {
        /// <summary>
        /// 传入的xml类型（xmlPath:存储xml文档的路径 xmlString xml字符串）
        /// </summary>
        public enum XmlType
        {
            xmlPath,
            xmlString
        }

        #region 字段定义
        /// <summary>
        /// XML文件的物理路径
        /// </summary>
        private string _filePath = string.Empty;

        private XmlDocument _xml = null;
        /// <summary>
        /// Xml文档
        /// </summary>
        public XmlDocument Xml
        {
            get { return _xml; }
            set { _xml = value; }
        }
        /// <summary>
        /// XML的根节点
        /// </summary>
        private XmlElement _element;
        #endregion

        #region 构造方法
        /// <summary>
        /// 实例化XmlHelper对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        public XmlHelper(XmlType xmltype, string xmlString)
        {
            try
            {
                switch (xmltype)
                {
                    case XmlType.xmlPath:
                        //获取XML文件的绝对路径
                        _filePath = System.AppDomain.CurrentDomain.BaseDirectory + xmlString;
                        break;
                    case XmlType.xmlString:
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlString);
                        this.Xml = xmlDoc;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        #region 创建XML的根节点
        /// <summary>
        /// 创建XML的根节点
        /// </summary>
        private void CreateXMLElement()
        {
            try
            {
                if (_xml == null)
                {
                    //创建一个XML对象
                    _xml = new XmlDocument();
                    if (File.Exists(_filePath))
                    {
                        //加载XML文件
                        _xml.Load(this._filePath);
                    }
                }
                //为XML的根节点赋值
                _element = _xml.DocumentElement;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        #region 获取指定XPath表达式的节点对象
        /// <summary>
        /// 获取指定XPath表达式的节点对象
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNode GetNode(string xPath)
        {
            XmlNode xmlNode = null;
            try
            {
                //创建XML的根节点
                CreateXMLElement();

                //返回XPath节点
                xmlNode = _element.SelectSingleNode(xPath);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return xmlNode;
        }
        #endregion

        #region 获取指定XPath表达式节点的值
        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public string GetValue(string xPath)
        {
            string result = "";
            try
            {
                //创建XML的根节点
                CreateXMLElement();

                //返回XPath节点的值
                result = _element.SelectSingleNode(xPath).InnerText;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }
        #endregion

        #region 设置指定XPath表达式节点的值
        /// <summary>
        /// 设置指定XPath表达式节点的值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public void SetValue(string xPath, string newValue)
        {
            try
            {
                //创建XML的根节点
                CreateXMLElement();

                //返回XPath节点的值
                _element.SelectSingleNode(xPath).InnerText = newValue;
                _xml.Save(_filePath);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        #region 获取指定XPath表达式节点的属性值
        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public string GetAttributeValue(string xPath, string attributeName)
        {
            string result = string.Empty;
            try
            {
                //创建XML的根节点
                CreateXMLElement();

                //返回XPath节点的属性值
                result = _element.SelectSingleNode(xPath).Attributes[attributeName].Value;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }
        #endregion

        #region 新增节点
        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public void AppendNode(XmlNode xmlNode)
        {
            try
            {
                //创建XML的根节点
                CreateXMLElement();

                //导入节点
                XmlNode node = _xml.ImportNode(xmlNode, true);

                //将节点插入到根节点下
                _element.AppendChild(node);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将DataSet中的第一条记录插入Xml文件中。
        /// </summary>        
        /// <param name="ds">DataSet的实例，该DataSet中应该只有一条记录</param>
        public void AppendNode(DataSet ds)
        {
            try
            {
                //创建XmlDataDocument对象
                XmlDataDocument xmlDataDocument = new XmlDataDocument(ds);

                //导入节点
                XmlNode node = xmlDataDocument.DocumentElement.FirstChild;

                //将节点插入到根节点下
                AppendNode(node);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        #region 删除节点
        /// <summary>
        /// 删除指定XPath表达式的节点
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public void RemoveNode(string xPath)
        {
            try
            {
                //创建XML的根节点
                CreateXMLElement();

                //获取要删除的节点
                XmlNode node = _xml.SelectSingleNode(xPath);

                //删除节点
                _element.RemoveChild(node);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion //删除节点

        #region 保存XML文件
        /// <summary>
        /// 保存XML文件
        /// </summary>        
        public void Save()
        {
            try
            {
                //创建XML的根节点
                CreateXMLElement();

                //保存XML文件
                _xml.Save(this._filePath);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion //保存XML文件

        #region 静态方法

        #region 创建根节点对象
        /// <summary>
        /// 创建根节点对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>        
        private static XmlElement CreateRootElement(string xmlFilePath)
        {
            XmlElement _XmlElement = null;
            try
            {
                //定义变量，表示XML文件的绝对路径
                string filePath = "";

                //获取XML文件的绝对路径
                filePath = System.AppDomain.CurrentDomain.BaseDirectory + xmlFilePath;

                //创建XmlDocument对象
                XmlDocument xmlDocument = new XmlDocument();
                //加载XML文件
                xmlDocument.Load(filePath);

                //返回根节点
                _XmlElement = xmlDocument.DocumentElement;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return _XmlElement;
        }
        #endregion

        #region 获取指定XPath表达式节点的值
        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public static string GetValue(string xmlFilePath, string xPath)
        {
            string result = string.Empty;
            try
            {
                //创建根对象
                XmlElement rootElement = CreateRootElement(xmlFilePath);

                //返回XPath节点的值
                result = rootElement.SelectSingleNode(xPath).InnerXml;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }
        #endregion

        #region 获取指定XPath表达式节点的属性值
        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public static string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            string result = string.Empty;
            try
            {
                //创建根对象
                XmlElement rootElement = CreateRootElement(xmlFilePath);

                //返回XPath节点的属性值
                result = rootElement.SelectSingleNode(xPath).Attributes[attributeName].Value;

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }
        #endregion

        #endregion

        public static void SetValue(string xmlFilePath, string xPath, string newtext)
        {
          
            XmlDocument xmldoc = new XmlDocument();
            //定义变量，表示XML文件的绝对路径
            string filePath = "";

            //获取XML文件的绝对路径
            filePath = System.AppDomain.CurrentDomain.BaseDirectory + xmlFilePath;

            //创建XmlDocument对象
            xmldoc.Load(filePath);
            XmlNodeList topM = xmldoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in topM)
            {
                if (element.Name.ToLower() == xPath.ToLower())
                {
                    XmlNodeList _node = element.ChildNodes;
                    try
                    {
                        if (_node.Count > 0)
                        {
                            foreach (XmlNode el in _node)
                            {
                                el.InnerXml = newtext;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        LogHelper.Error(ex);
                        if (_node.Count > 0)
                        {
                            foreach (XmlNode el in _node)
                            {
                                el.InnerText = newtext;
                            }
                        }
                    }

                }
            }
            xmldoc.Save(filePath);
        }
    }
}
