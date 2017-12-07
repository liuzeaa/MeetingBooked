using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MeetingModel

{
    /// <summary>
    /// 数据验证类
    /// </summary>
    public class ValidationHelper
    {
        /*一些常用的正则表达式
         * 
         * 
         * 
            ^\d+$　　//匹配非负整数（正整数 + 0） 
            ^[0-9]*[1-9][0-9]*$　　//匹配正整数 
            ^((-\d+)|(0+))$　　//匹配非正整数（负整数 + 0） 
            ^-[0-9]*[1-9][0-9]*$　　//匹配负整数 
            ^-?\d+$　　　　//匹配整数 
            ^\d+(\.\d+)?$　　//匹配非负浮点数（正浮点数 + 0） 
            ^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$　　//匹配正浮点数 
            ^((-\d+(\.\d+)?)|(0+(\.0+)?))$　　//匹配非正浮点数（负浮点数 + 0） 
            ^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$　　//匹配负浮点数 
            ^(-?\d+)(\.\d+)?$　　//匹配浮点数 
            ^[A-Za-z]+$　　//匹配由26个英文字母组成的字符串 
            ^[A-Z]+$　　//匹配由26个英文字母的大写组成的字符串 
            ^[a-z]+$　　//匹配由26个英文字母的小写组成的字符串 
            ^[A-Za-z0-9]+$　　//匹配由数字和26个英文字母组成的字符串 
            ^\w+$　　//匹配由数字、26个英文字母或者下划线组成的字符串 
            ^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$　　　　//匹配email地址 
            ^[a-zA-z]+://匹配(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$　　//匹配url 

            匹配中文字符的正则表达式： [\u4e00-\u9fa5] 
            匹配双字节字符(包括汉字在内)：[^\x00-\xff] 
            匹配空行的正则表达式：\n[\s| ]*\r 
            匹配HTML标记的正则表达式：/<(.*)>.*<\/>|<(.*) \/>/ 
            匹配首尾空格的正则表达式：(^\s*)|(\s*$) 
            匹配Email地址的正则表达式：\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)* 
            匹配网址URL的正则表达式：^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$ 
            匹配帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线)：^[a-zA-Z][a-zA-Z0-9_]{4,15}$ 
            匹配国内电话号码：(\d{3}-|\d{4}-)?(\d{8}|\d{7})? 
            匹配腾讯QQ号：^[1-9]*[1-9][0-9]*$ 
         * */
        private static readonly Regex RegChzn = new Regex("[\u4e00-\u9fa5]");
        #region 检测对象是否为空

        #region 重载1

        /// <summary>
        /// 检测对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(T data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(string))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
                return false;
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }

        #endregion

        #region 重载2

        /// <summary>
        /// 检测对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(object data)
        {
            return IsNullOrEmpty<object>(data);
        }

        #endregion

        #region 重载3

        /// <summary>
        /// 检测字符串是否为空，为空返回true
        /// </summary>
        /// <param name="text">要检测的字符串</param>
        public static bool IsNullOrEmpty(string text)
        {
            //检测是否为null
            if (text == null)
            {
                return true;
            }

            //检测字符串空值
            if (string.IsNullOrEmpty(text.Trim()))
            {
                return true;
            }
            return false;
        }

        #endregion

        #endregion

        #region 弱口令检测
        /// <summary>
        /// 若口令检测
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsWeakPasswurd(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string[] WeakKey = new string[]{
            "qwerasdf","computer","zxczxczxc","dddddddd","299792458","135792468","20082008","369369369","5845211314","yangyang","csdncsdn","google250",
            "woaini520","zhang123","1234567b","wocaonima","1233211234567","9876543210","qaz123456","q123456789","321654987","369258147","aaa123456",
            "1357924680","123321aa","25257758","wojiushiwo","ssssssss","qazwsx123","123456aaa","1234567a","z123456789","woainima","44444444","buzhidao",
            "ffffffff","100200300","12345679","12369874","1122334455","111111","woaini123","qwe123456","xiaoxiao","123456654321","woshishui","12301230",
            "1234554321","5201314520","12345612","lilylily","123456asd","10101010","1q2w3e4r5t","11235813","12345600","11111111111111111111","wwwwwwww ",
            "0987654321","5845201314","zxcvbnm123","kingcom5","123456987","05962514787","321321321","woaiwojia","1qazxsw2","123qweasd","1234abcd","woaini1314",
            "12345678a","q1w2e3r4","asdfghjk","1123581321","123698745","asdf1234","521521521","147852369","123456qq","3.1415926","qweqweqwe","111222333","zzzzzzzz",
            "ms0083jxj","11112222","code8925","qweasdzxc","77777777","asd123456","qwer1234","33333333","55555555","741852963","963852741","520520520","123456123456",
            "999999999","123456aa","99999999","asdfasdf","aa123456","123456789a","qwertyui","1234qwer","a1234567","123456123","123456","a12345678","abc123456","123321123",
            "22222222","asdasdasd","110110110","12341234","abcd1234","qazwsxedc","12121212","123654789","0123456789","123456abc","1q2w3e4r","asdfghjkl","0000000000","12344321",
            "31415926","iloveyou","qq123456","qwertyuiop","000000000","qqqqqqqq","87654321","password","789456123","xiazhili","1qaz2wsx","11223344","a123456789","66666666","1111111111",
            "aaaaaaaa","987654321","47258369","111111111","88888888","1234567890","123123123","00000000","dearbook","11111111","12345678","123456789","123456"        
            };
            for (int i = 0; i < WeakKey.Length; i++)
            {
                ///忽略大小写比较；
                if (WeakKey[i].Equals(text, StringComparison.CurrentCultureIgnoreCase))
                {
                    return false;
                }
            }
            //如果完全通过验证，则返回正确
            return true;
        }
        #endregion

        #region 验证IP地址是否合法

        /// <summary>
        /// 验证IP地址是否合法
        /// </summary>
        /// <param name="ip">要验证的IP地址</param>        
        public static bool IsIp(string ip)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(ip))
            {
                return true;
            }

            //清除要验证字符串中的空格
            ip = ip.Trim();

            //模式字符串
            const string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            //验证
            return IsMatch(ip, pattern);
        }

        #endregion

        #region  验证EMail是否合法

        /// <summary>
        /// 验证EMail是否合法
        /// </summary>
        /// <param name="email">要验证的Email</param>
        public static bool IsEmail(string email)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(email))
            {
                return true;
            }

            //清除要验证字符串中的空格
            email = email.Trim();

            //模式字符串
            const string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            //验证
            return IsMatch(email, pattern);
        }

        #endregion

        #region 验证是否为整数

        /// <summary>
        /// 验证是否为整数
        /// </summary>
        /// <param name="number">要验证的整数</param>        
        public static bool IsInt(string number)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(number))
            {
                return true;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            const string pattern = @"^[1-9]+[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }

        #endregion

        #region 验证是否为数字

        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="number">要验证的数字</param>        
        public static bool IsNumber(string number)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(number))
            {
                return true;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            const string pattern = @"^[1-9]+[0-9]*[.]?[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }

        #endregion

        #region 验证日期是否合法

        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date">日期</param>
        public static bool IsDate(ref string date)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(date))
            {
                return true;
            }

            //清除要验证字符串中的空格
            date = date.Trim();

            //替换\
            date = date.Replace(@"\", "-");
            //替换/
            date = date.Replace(@"/", "-");

            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }

            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsInt(date))
                {
                    return false;
                }

                #region 对纯数字进行解析

                //对8位纯数字进行解析
                if (date.Length == 8)
                {
                    //获取年月日
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    string year = date.Substring(0, 4);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }

                #endregion

                return false;
            }
        }

        #endregion

        #region 验证身份证是否合法

        /// <summary>
        /// 验证身份证是否合法
        /// </summary>
        /// <param name="idCard">要验证的身份证</param>        
        public static bool IsIdCard(string idCard)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(idCard))
            {
                return true;
            }

            //清除要验证字符串中的空格
            idCard = idCard.Trim();

            //模式字符串
            var pattern = new StringBuilder();
            pattern.Append(@"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
            pattern.Append(@"50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
            pattern.Append(@"(\d{13}|\d{15}[\dx])$");

            //验证
            return IsMatch(idCard, pattern.ToString());
        }

        #endregion

        #region 检测客户的输入中是否有危险字符串

        /// <summary>
        /// 检测客户输入的字符串是否有效,并将原始字符串修改为有效字符串或空字符串。
        /// 当检测到客户的输入中有攻击性危险字符串,则返回false,有效返回true。
        /// </summary>
        /// <param name="input">要检测的字符串</param>
        public static bool IsValidInput(ref string input)
        {
            try
            {
                if (IsNullOrEmpty(input))
                {
                    //如果是空值,则跳出
                    return true;
                }
                //过滤 ' --  
                string pattern1 = @"(\%27)|(\')|(\-\-)";
                //防止执行 ' or  
                string pattern2 = @"((\%27)|(\'))\s*((\%6F)|o|(\%4F))((\%72)|r|(\%52))";
                //防止执行sql server 内部存储过程或扩展存储过程  
                string pattern3 = @"\s+exec(\s|\+)+(s|x)p\w+";
                input = Regex.Replace(input, pattern1,"-", RegexOptions.IgnoreCase);
                input = Regex.Replace(input, pattern2, "o r", RegexOptions.IgnoreCase);
                input = Regex.Replace(input, pattern3, "e xec", RegexOptions.IgnoreCase); 
                //替换单引号
                input = input.Replace("'", "''").Trim();
                //未检测到攻击字符串
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查是否含有非法字符
        /// </summary>
        /// <remarks>检查是否含有非法字符</remarks>
        /// <param name="str">要检查的字符串</param>
        /// <returns></returns>
        public static bool IsValidInput(string str)
        {
            bool result = false;
            if (string.IsNullOrEmpty(str))
                return false;
            const string strBadChar = "@@,+,',--,%,^,&,?,(,),<,>,[,],{,},/,\\,;,:,\",\"\"";
            string[] arrBadChar = StringHelper.SplitString(strBadChar, ",");
            string tempChar = str;
            for (int i = 0; i < arrBadChar.Length; i++)
            {
                if (tempChar.IndexOf(arrBadChar[i]) >= 0)
                    result = true;
            }
            return result;
        }

        #endregion

        #region 验证输入字符串是否与模式字符串匹配

        #region 重载1

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        #endregion

        #region 重载2

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件,比如是否忽略大小写</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        #endregion

        #endregion

        #region 获取匹配的值

        #region 重载1

        /// <summary>
        /// 获取匹配的值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="resultPattern">结果模式字符串,范例："$1"用来获取第一个( )内的值</param>
        /// <param name="options">筛选条件,比如是否忽略大小写</param>
        public static string GetMatchValue(string input, string pattern, string resultPattern, RegexOptions options)
        {
            //判断是否匹配
            return Regex.IsMatch(input, pattern, options)
                       ? Regex.Match(input, pattern, options).Result(resultPattern)
                       : string.Empty;
        }

        #endregion

        #region 重载2

        /// <summary>
        /// 获取匹配的值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="resultPattern">结果模式字符串,范例："$1"用来获取第一个( )内的值</param>
        public static string GetMatchValue(string input, string pattern, string resultPattern)
        {
            return GetMatchValue(input, pattern, resultPattern, RegexOptions.IgnoreCase);
        }

        #endregion

        #region 重载3

        /// <summary>
        /// 获取匹配的值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        public static string GetMatchValue(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase)
                       ? Regex.Match(input, pattern, RegexOptions.IgnoreCase).Value
                       : string.Empty;
        }

        #endregion

        #endregion

        #region 检测是否有中文字符

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasChzn(string inputData)
        {
            Match m = RegChzn.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 检测是否符合电话格式

        /// <summary>
        /// 检测是否符合电话格式
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)");
        }

        #endregion

        #region 检测是否手机号码格式

        /// <summary>
        /// 检测是否手机号码格式
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsMobiletelePhone(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, "^13[0-9]{1}[0-9]{8}$|^15[9]{1}[0-9]{8}$");
        }

        #endregion

        #region 检测是否符合url格式,前面必需含有http://

        /// <summary>
        /// 检测是否符合url格式,前面必需含有http://
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            return Regex.IsMatch(url, @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
        }

        public static bool IsNoHttpUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            return Regex.IsMatch(url, @"^/([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
        }

        #endregion

        #region 检测是否符合时间格式

        /// <summary>
        /// 检测是否符合时间格式
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval,
                                 @"20\d{2}\-[0-1]{1,2}\-[0-3]?[0-9]?(\s*((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?))?");
        }

        #endregion

        #region 检测是否符合邮编格式

        /// <summary>
        /// 检测是否符合邮编格式
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        public static bool IsPostCode(string postCode)
        {
            return Regex.IsMatch(postCode, @"^\d{6}$");
        }

        #endregion

        #region 验证是否为汉字,拼音数字

        /// <summary>
        ///  验证是否为汉字,拼音数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNts(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9\\u4e00-\\u9fa5]+$");
        }

        #endregion
    }
}