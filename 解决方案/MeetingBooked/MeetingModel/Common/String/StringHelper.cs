using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MeetingModel
{
    /// <summary>
    /// 用于字符串处理的公共操作类
    /// </summary>
    [Serializable]
    public class StringHelper
    {
        #region 字段定义

        /// <summary>
        /// 缩进级别
        /// </summary>
        private int _indentLevel;

        /// <summary>
        /// 要生成的字符串
        /// </summary>
        private StringBuilder _str;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化对象
        /// </summary>
        public StringHelper()
        {
            //初始化字符串
            _str = new StringBuilder();
        }

        #endregion

        #region 添加字符串

        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="parameters">参数</param>
        public void Append(string text, params object[] parameters)
        {
            if (parameters.Length == 0)
            {
                //不带参数的
                _str.Append(text);
            }
            else
            {
                //带参数的
                _str.AppendFormat(text, parameters);
            }
        }

        #endregion

        #region 清空字符串

        /// <summary>
        /// 清空字符串
        /// </summary>
        public void Clear()
        {
            Clear(_str);
        }

        #endregion

        #region 字符串长度

        /// <summary>
        /// 字符串长度
        /// </summary>
        public int Length
        {
            get { return _str.Length; }
        }

        #endregion

        #region 增加缩进

        /// <summary>
        /// 增加缩进
        /// </summary>
        public void IncreaseIndent()
        {
            _indentLevel++;
        }

        /// <summary>
        /// 增加缩进
        /// </summary>
        /// <param name="step">增加量</param>
        public void IncreaseIndent(int step)
        {
            _indentLevel += step;
        }

        #endregion

        #region 减少缩进

        /// <summary>
        /// 减少缩进
        /// </summary>
        public void DecreaseIndent()
        {
            _indentLevel--;
        }

        #endregion

        #region 移除末尾指定字符串

        /// <summary>
        /// 移除字符串末尾指定字符串
        /// </summary>
        /// <param name="removeString">要移除的字符串</param>
        public void RemoveEnd(string removeString)
        {
            //创建临时字符串
            string tempString = _str.ToString().TrimEnd(removeString.ToCharArray());

            if (tempString != _str.ToString())
            {
                var temp = new StringBuilder();
                temp.Append(tempString);

                //为字段赋值
                _str = temp;
            }
        }

        #endregion

        #region 静态方法

        #region 将字符转换为ASCII

        public static int Asc(string character)
        {
            if (character.Length != 1)
            {
                throw new Exception("Character is not valid.");
            }
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetBytes(character)[0];
        }

        public static string Chr(int asciiCode)
        {
            if ((asciiCode < 0) || (asciiCode > 0xff))
            {
                throw new Exception("ASCII Code is not valid.");
            }
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] byteArray = new byte[] { (byte)asciiCode };
            return asciiEncoding.GetString(byteArray);
        }

        #endregion

        #region 将ASCII转换为字符

        public static string UnAsc(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                var asciiEncoding = new ASCIIEncoding();
                var byteArray = new[] {(byte) asciiCode};
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            throw new Exception("ASCII Code is not valid.");
        }

        #endregion

        #region 对脚本中的消息进行过滤处理

        /// <summary>
        /// 对脚本中的消息进行过滤处理
        /// </summary>
        /// <param name="msg">脚本中的消息字符串,但不包括脚本函数.
        /// 范例：alert('ab\n\rc'),只传入ab\n\rc,不要把alert('')传进来。</param>
        public static string GetValidScriptMsg(string msg)
        {
            var validMsg = new StringBuilder(msg);
            validMsg.Replace("\\", @"\\");
            validMsg.Replace("\n", "\\n");
            validMsg.Replace("\t", "\\t");
            validMsg.Replace("\r", "\\r");
            validMsg.Replace("'", "\\'");

            //返回有效的字符串
            return validMsg.ToString();
        }

        #endregion

        #region 去除字符串最后的逗号

        /// <summary>
        /// 去除字符串最后的逗号,返回新的字符串
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static string RemoveLastComma(ref string text)
        {
            text = text.TrimEnd(',');
            return text;
        }

        /// <summary>
        /// 去除字符串最后的逗号,返回新的字符串
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static string RemoveLastComma(StringBuilder text)
        {
            return text.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 去除字符串最后的逗号,返回新的字符串
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static string RemoveLastComma(StringHelper text)
        {
            return text.ToString().TrimEnd(',');
        }

        #endregion

        #region 清空字符串

        /// <summary>
        /// 清空字符串
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static void Clear(StringBuilder text)
        {
            text.Remove(0, text.Length);
        }

        #endregion

        #region 获取字符串的最后一个字符

        /// <summary>
        /// 获取字符串的最后一个字符
        /// </summary>
        /// <param name="text">原始字符串</param>        
        public static string GetLastChar(string text)
        {
            //如果字符串为空，则返回
            if (ValidationHelper.IsNullOrEmpty(text))
            {
                return "";
            }

            return text.Substring(text.Length - 1, 1);
        }

        #endregion

        #region 用逗号拼接数组

        #region 重载方法1

        /// <summary>
        /// 获取用逗号拼接数组元素的字符串。返回字符串的数组元素默认不带引号，如果需要引号请使用重载方法。
        /// 范例: 数组为:string[] a = new string[] { "1","2","3" };返回值:1,2,3
        /// </summary>
        /// <param name="stringArray">原始字符串数组</param>
        public static string GetCommaString(params string[] stringArray)
        {
            return GetCommaString(false, stringArray);
        }

        #endregion

        #region 重载方法2

        /// <summary>
        /// 获取用逗号拼接数组元素的字符串。
        /// 范例: 数组为:string[] a = new string[] { "1","2","3" };返回值:1,2,3
        /// </summary>
        /// <param name="isAddQuotationMarks">是否添加单引号,如果传入true，则返回值为'1','2','3'</param>
        /// <param name="stringArray">原始字符串数组</param>
        public static string GetCommaString(bool isAddQuotationMarks, params string[] stringArray)
        {
            //临时字符串
            var list = new StringBuilder();

            //循环字符串数组，添加到临时字符串中
            foreach (string text in stringArray)
            {
                list.AppendFormat(isAddQuotationMarks ? "'{0}'," : "{0},", text);
            }

            //返回字符串
            return RemoveLastComma(list);
        }

        #endregion

        #region 重载方法3

        /// <summary>
        /// 获取用逗号拼接数组元素的字符串。返回字符串的数组元素默认不带引号，如果需要引号请使用重载方法。
        /// 范例: 数组为:int[] a = new int[] { 1,2,3 };返回值:1,2,3
        /// </summary>
        /// <param name="intArray">原始整型数组</param>
        public static string GetCommaString(params int[] intArray)
        {
            return GetCommaString(false, intArray);
        }

        #endregion

        #region 重载方法4

        /// <summary>
        /// 获取用逗号拼接数组元素的字符串。
        /// 范例: 数组为:int[] a = new int[] { 1,2,3 };返回值:1,2,3
        /// </summary>
        /// <param name="isAddQuotationMarks">是否添加单引号,如果传入true，则返回值为'1','2','3'</param>
        /// <param name="intArray">原始整型数组</param>
        public static string GetCommaString(bool isAddQuotationMarks, params int[] intArray)
        {
            //临时字符串
            var list = new StringBuilder();

            //循环字符串数组，添加到临时字符串中
            foreach (int text in intArray)
            {
                list.AppendFormat(isAddQuotationMarks ? "'{0}'," : "{0},", text);
            }

            //返回字符串
            return RemoveLastComma(list);
        }

        #endregion

        #region 重载方法5

        /// <summary>
        /// 获取用逗号拼接元素的字符串。返回字符串的元素默认不带引号，如果需要引号请使用重载方法。
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="columnName">列名</param>
        public static string GetCommaString(DataTable table, string columnName)
        {
            return GetCommaString(table, columnName, false);
        }

        #endregion

        #region 重载方法6

        /// <summary>
        /// 获取用逗号拼接元素的字符串。返回字符串的元素默认不带引号，如果需要引号请使用重载方法。
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="isAddQuotationMarks">是否添加单引号,如果传入true，则返回值为'1','2','3'</param>
        public static string GetCommaString(DataTable table, string columnName, bool isAddQuotationMarks)
        {
            //临时字符串
            var list = new StringBuilder();

            //循环数据集，添加到临时字符串中
            foreach (DataRow row in table.Rows)
            {
                list.AppendFormat(isAddQuotationMarks ? "'{0}'," : "{0},", row[columnName]);
            }

            //返回字符串
            return RemoveLastComma(list);
        }

        #endregion

        #endregion

        #region 获取数字格式的当前时间

        /// <summary>
        /// 获取数字格式的当前时间
        /// </summary>
        public static string GetNumberByNowDate()
        {
            //获取当前时间的字符串表示
            string numberTime = DateTime.Now.ToString("g");

            //转换为纯数字
            numberTime = numberTime.Replace("-", "");
            numberTime = numberTime.Replace(":", "");
            numberTime = numberTime.Replace(" ", "");

            //返回数字格式的当前时间
            return numberTime;
        }

        #endregion

        #region 将字符串的首字母大写

        /// <summary>
        /// 将字符串的首字母大写
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static void FirstCharUpper(ref string text)
        {
            //获取首字母
            string temp = text.Substring(0, 1).ToUpper();

            //将字符串的首字母大写
            text = temp + text.Substring(1, text.Length - 1);
        }

        #endregion

        #region 获取百分比字符串

        /// <summary>
        /// 获取百分比字符串,返回值范例： 100%
        /// </summary>
        /// <param name="percent">0-100之间的整数</param>
        public static string GetPercentage(int percent)
        {
            return percent + "%";
        }

        #endregion

        #region 创建正则表达式的模式字符串

        /// <summary>
        /// 创建正则表达式的模式字符串,添加了开头和结尾的模式字符。
        /// 比如传入字符串"abc",返回"^abc$"
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static string GetPattern(string text)
        {
            return "^" + text + "$";
        }

        #endregion

        #region 分解URL

        /// <summary>
        /// 分解URL,获取url路径中?的前部分( 去掉了查询字符串的部分 ) 与 查询字符串部分
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <param name="hostUrl">去掉了查询字符串的URL</param>
        /// <param name="queryString">查询字符串</param>
        public static void DecomposeUrl(string url, out string hostUrl, out string queryString)
        {
            //如果重写的URL有查询字符串，则分解
            if (url.IndexOf("?") != -1)
            {
                //获取url路径中?的前部分
                hostUrl = url.Substring(0, url.IndexOf('?'));
                //获取查询字符串
                queryString = url.Substring(url.IndexOf('?') + 1);
            }
            else
            {
                //url路径中?的前部分
                hostUrl = url;
                //查询字符串
                queryString = string.Empty;
            }
        }

        #endregion

        #region 分割字符串

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = {strContent};
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int p3)
        {
            var result = new string[p3];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < p3; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }

        #endregion

        #region 判断指定字符串是否属于指定字符串数组中的一个元素

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }
            }
            return -1;
        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
        }

        #endregion

        #region 从字符串的指定位置截取指定长度的子字符串

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length*-1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                if (length + startIndex > 0)
                {
                    length = length + startIndex;
                    startIndex = 0;
                }
                else
                {
                    return "";
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }


        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="pSrcString">要检查的字符串</param>
        /// <param name="pLength">指定长度</param>
        /// <param name="pTailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string pSrcString, int pLength, string pTailString)
        {
            return GetSubString(pSrcString, 0, pLength, pTailString);
        }

        /// <summary>
        /// 截取字符。不区分中英文
        /// </summary>
        /// <param name="pSrcString"></param>
        /// <param name="pLength"></param>
        /// <param name="bdot"></param>
        /// <returns></returns>
        public static string GetSubString(string pSrcString, int pLength, bool bdot)
        {
            string str = "";

            if (pLength >= pSrcString.Length)
                return pSrcString;

            int nRealLen = pLength*2;
            if (bdot)
                nRealLen = nRealLen - 3;

            Encoding encoding = Encoding.GetEncoding("gb2312");
            for (int i = pSrcString.Length; i >= 0; i--)
            {
                str = pSrcString.Substring(0, i);
                if (encoding.GetBytes(str).Length > nRealLen)
                    continue;
                break;
            }

            if (bdot)
                str += "...";

            return str;
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="pSrcString">要检查的字符串</param>
        /// <param name="pStartIndex">起始位置</param>
        /// <param name="pLength">指定长度</param>
        /// <param name="pTailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string pSrcString, int pStartIndex, int pLength, string pTailString)
        {
            string myResult = pSrcString;

            if (pLength >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(pSrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > pStartIndex)
                {
                    int pEndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (pStartIndex + pLength))
                    {
                        pEndIndex = pLength + pStartIndex;
                    }
                    else
                    {
                        //当不在有效范围内时,只取到字符串的结尾

                        pLength = bsSrcString.Length - pStartIndex;
                        pTailString = "";
                    }


                    int nRealLength = pLength;
                    var anResultFlag = new int[pLength];

                    int nFlag = 0;
                    for (int i = pStartIndex; i < pEndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[pEndIndex - 1] > 127) && (anResultFlag[pLength - 1] == 1))
                    {
                        nRealLength = pLength + 1;
                    }

                    var bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, pStartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + pTailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 按一定字节长度截取字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="length">要截取的字节长度</param>
        /// <param name="pTailString"></param>
        /// <returns>返回处理后的字符串，超出长度部分被""替换</returns>
        public static string GetStringByLength(string str, int length, string pTailString)
        {
            byte[] bwrite = Encoding.GetEncoding("GB2312").GetBytes(str.ToCharArray());
            if (bwrite.Length >= length)
            {
                string newStr = Encoding.Default.GetString(bwrite, 0, length - 4);
                if (newStr[newStr.Length - 1] == '?')
                {
                    newStr = Encoding.Default.GetString(bwrite, 0, length - 3);
                }
                return newStr + pTailString;
            }
            return str;
        }

        #endregion

        #region 过滤HTML代码

        public static string ReplaceHtml(string html)
        {
            var regex1 = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            var regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            var regex3 = new Regex(@" no[\s\S]*=", RegexOptions.IgnoreCase);
            var regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            var regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            var regex6 = new Regex(@"\<img[^\>]+\>", RegexOptions.IgnoreCase);
            var regex7 = new Regex(@"</p>", RegexOptions.IgnoreCase);
            var regex8 = new Regex(@"<p>", RegexOptions.IgnoreCase);
            var regex9 = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            html = regex6.Replace(html, ""); //过滤frameset 
            html = regex7.Replace(html, ""); //过滤frameset 
            html = regex8.Replace(html, ""); //过滤frameset 
            html = regex9.Replace(html, "");
            html = html.Replace("&nbsp;", " ");
            html = html.Replace("&#40;", "(");
            html = html.Replace("&#41;", ")");
            html = html.Replace("\n\r", "");
            html = html.Replace("\r\n", "");
            html = html.Replace("\n", "");
            html = html.Replace("\r", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");

            html = html.Replace(" ", "");
            return html;
        }

        #endregion

        #region 替换,恢复html中的特殊字符

        /// <summary>
        /// 替换，恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public string HtmlEncode(string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace(" ", "&nbsp;");
            theString = theString.Replace(" ", "&nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "'");
            theString = theString.Replace("\n", "<br/> ");
            return theString;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public string HtmlDiscode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("'", "\'");
            theString = theString.Replace("<br/> ", "\n");
            return theString;
        }

        #endregion

        #region 替换字符串中是否有非法的字符

        /// <summary>
        /// 过滤非法字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceBadChar(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            const string strBadChar = "@@,+,',--,%,^,&,?,(,),<,>,[,],{,},/,\\,;,:,\",\"\"";
            string[] arrBadChar = SplitString(strBadChar, ",");
            return arrBadChar.Where(t => t.Length > 0).Aggregate(str, (current, t) => current.Replace(t, ""));
        }

        public static string ReplaceSqlChar(string text)
        {
            string reText = text;
            if (!string.IsNullOrEmpty(reText))
            {
                reText = reText.ToLower();
                reText = reText.Replace("'", "''");
                return reText.Trim();
            }
            return "";
        }

        public static string InputText(string text, int maxLength)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " "); //two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //any other tags
            text = text.Replace("'", "''");
            return text;
        }

        public static string InputText(string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = Regex.Replace(text, "[\\s]{2,}", " "); //two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //any other tags
            text = text.Replace("'", "''");
            return text;
        }

        #endregion

        #region 返回字符串真实长度

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetStringLength(string text)
        {
            int len = 0;

            for (int i = 0; i < text.Length; i++)
            {
                byte[] byteLen = Encoding.Default.GetBytes(text.Substring(i, 1));
                if (byteLen.Length > 1)
                    len += 2; //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1; //如果长度等于1，是英文，占一个字节，+1
            }

            return len;
        }

        #endregion

        #region 去除数组内重复元素

        /**/

        /// <summary>
        /// 去除数组内重复元素
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns> 
        public static ArrayList FilterRepeatArrayItem(ArrayList arr)
        {
            //建立新数组
            var newArr = new ArrayList();
            //载入第一个原数组元素
            if (arr.Count > 0)
            {
                newArr.Add(arr[0]);
            }
            //循环比较
            foreach (object t in arr)
            {
                if (!newArr.Contains(t))
                {
                    newArr.Add(t);
                }
            }
            return newArr;
        }

        #endregion

        #region 金额转大写
        /// <summary>
        ///   金额转大写  
        /// </summary>  
        public static string MoneyToChinese(string LowerMoney)
        {
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数  
            if (LowerMoney.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数  
                LowerMoney = LowerMoney.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4  
            LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
            if (LowerMoney.IndexOf(".") > 0)
            {
                if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
                {
                    LowerMoney = LowerMoney + "0";
                }
            }
            else
            {
                LowerMoney = LowerMoney + ".00";
            }
            strLower = LowerMoney;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理  
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }
        #endregion

        #endregion

        #region 重写ToString

        /// <summary>
        /// 获取生成的字符串
        /// </summary>
        public override string ToString()
        {
            return _str.ToString();
        }

        #endregion
    }
}