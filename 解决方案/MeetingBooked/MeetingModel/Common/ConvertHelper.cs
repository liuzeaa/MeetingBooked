using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MeetingModel
{
    /// <summary>
    /// 处理数据类型转换，数制转换、编码转换相关的类
    /// </summary>    
    public class ConvertHelper
    {
        #region 补足位数

        /// <summary>
        /// 指定字符串的固定长度，如果字符串小于固定长度，
        /// 则在字符串的前面补足零，可设置的固定长度最大为9位
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="limitedLength">字符串的固定长度</param>
        public static string RepairZero(string text, int limitedLength)
        {
            //补足0的字符串
            string temp = "";

            try
            {
                //补足0
                for (int i = 0; i < limitedLength - text.Length; i++)
                {
                    temp += "0";
                }

                //连接text
                temp += text;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            //返回补足0的字符串
            return temp;
        }

        #endregion

        #region 各进制数间转换

        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from); //先转成10进制
                string result = Convert.ToString(intValue, to); //再转成目标进制
                if (to == 2)
                {
                    int resultLength = result.Length; //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;
                        case 6:
                            result = "00" + result;
                            break;
                        case 5:
                            result = "000" + result;
                            break;
                        case 4:
                            result = "0000" + result;
                            break;
                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return "0";
            }
        }

        #endregion

        #region 使用指定字符集将string转换成byte[]

        /// <summary>
        /// 将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>        
        public static byte[] StringToBytes(string text)
        {
            byte[] bytes = null;
            try
            {
                bytes = Encoding.UTF8.GetBytes(text);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return bytes;
        }

        /// <summary>
        /// 使用指定字符集将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            byte[] bytes = null;
            try
            {
                bytes = encoding.GetBytes(text);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return bytes;
        }

        #endregion

        #region 使用指定字符集将byte[]转换成string

        /// <summary>
        /// 将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>        
        public static string BytesToString(byte[] bytes)
        {
            string result = string.Empty;
            try
            {
                result = Encoding.UTF8.GetString(bytes);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            string result = string.Empty;
            try
            {
                result = encoding.GetString(bytes);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }

        #endregion

        #region 将流转换成字符串

        /// <summary>
        /// 将流转换成字符串,同时关闭该流
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">字符编码</param>
        public static string StreamToString(Stream stream, Encoding encoding)
        {
            //获取的文本
            string streamText;

            //读取流
            try
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    streamText = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return string.Empty;
            }
            finally
            {
                stream.Close();
            }

            //返回文本
            return streamText;
        }

        /// <summary>
        /// 将流转换成字符串,同时关闭该流
        /// </summary>
        /// <param name="stream">流</param>
        public static string StreamToString(Stream stream)
        {
            string result = string.Empty;
            try
            {
                result = StreamToString(stream, Encoding.Default);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 将byte[]转换成int

        /// <summary>
        /// 将byte[]转换成int
        /// </summary>
        /// <param name="data">需要转换成整数的byte数组</param>
        public static int BytesToInt32(byte[] data)
        {
            //定义要返回的整数
            int num = 0;
            try
            {
                //如果传入的字节数组长度小于4,则返回0
                if (data.Length < 4)
                {
                    return 0;
                }



                //如果传入的字节数组长度大于4,需要进行处理
                if (data.Length >= 4)
                {
                    //创建一个临时缓冲区
                    var tempBuffer = new byte[4];

                    //将传入的字节数组的前4个字节复制到临时缓冲区
                    Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                    //将临时缓冲区的值转换成整数，并赋给num
                    num = BitConverter.ToInt32(tempBuffer, 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            //返回整数
            return num;
        }

        #endregion

        #region 转换为日期

        /// <summary>
        /// 将数据转换为日期,如果数据无效则返回"1900-1-1"
        /// </summary>
        /// <param name="date">日期</param>
        public static DateTime ToDateTime(object date)
        {
            try
            {
                if (ValidationHelper.IsNullOrEmpty(date))
                {
                    return Convert.ToDateTime("1900-1-1");
                }
                else
                {
                    return Convert.ToDateTime(date);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Convert.ToDateTime("1900-1-1");
            }
        }

        #endregion

        #region 将数据转换为GUID

        /// <summary>
        /// 将数据转换为GUID
        /// </summary>
        /// <param name="data"></param>
        public static Guid ToGuid(object data)
        {
            //有效性验证
            if (ValidationHelper.IsNullOrEmpty(data))
            {
                return Guid.Empty;
            }

            try
            {
                return new Guid(data.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Guid.Empty;
            }
        }

        #endregion

        #region 将数据转换为整型

        #region 重载1

        /// <summary>
        /// 将数据转换为整型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static int ToInt32<T>(T data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #region 重载2

        /// <summary>
        /// 将数据转换为整型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static int ToInt32(object data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #endregion

        #region 将数据转换为字符串

        /// <summary>
        /// 将数据转换为字符串
        /// </summary>
        /// <param name="data">数据</param>
        public static string ToString(object data)
        {
            string result = string.Empty;
            try
            {
                //有效性验证
                if (data != null)
                {
                    result = data.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 将数据转换为布尔型

        #region 重载1

        /// <summary>
        /// 将数据转换为布尔型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static bool ToBoolean<T>(T data)
        {
            try
            {
                //如果为空则返回false
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        #endregion

        #region 重载2

        /// <summary>
        /// 将数据转换为布尔型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static bool ToBoolean(object data)
        {
            try
            {
                //如果为空则返回false
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        #endregion

        #endregion

        #region 将数据转换为单精度浮点型

        #region 重载1

        /// <summary>
        /// 将数据转换为单精度浮点型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static float ToFloat<T>(T data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToSingle(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #region 重载2

        /// <summary>
        /// 将数据转换为单精度浮点型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static float ToFloat(object data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty<object>(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToSingle(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #endregion

        #region 将数据转换为双精度浮点型

        #region 重载1

        /// <summary>
        /// 将数据转换为双精度浮点型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static double ToDouble<T>(T data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDouble(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #region 重载2

        /// <summary>
        /// 将数据转换为双精度浮点型,并设置小数位
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        public static double ToDouble<T>(T data, int decimals)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    double temp = Convert.ToDouble(data);
                    return Math.Round(temp, decimals);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #region 重载3

        /// <summary>
        /// 将数据转换为双精度浮点型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static double ToDouble(object data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDouble(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #region 重载4

        /// <summary>
        /// 将数据转换为双精度浮点型,并设置小数位
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        public static double ToDouble(object data, int decimals)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty<object>(data))
                {
                    return 0;
                }
                else
                {
                    double temp = Convert.ToDouble(data);
                    return Math.Round(temp, decimals);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #endregion

        #region 将数据转换为Decimal类型

        #region 重载1

        /// <summary>
        /// 将数据转换为Decimal类型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static decimal ToDecimal<T>(T data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #region 重载2

        /// <summary>
        /// 将数据转换为Decimal类型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        public static decimal ToDecimal<T>(T data, int decimals)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    decimal temp = Convert.ToDecimal(data);
                    return Math.Round(temp, decimals);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #region 重载3

        /// <summary>
        /// 将数据转换为Decimal类型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static decimal ToDecimal(object data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #region 重载4

        /// <summary>
        /// 将数据转换为Decimal类型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        public static decimal ToDecimal(object data, int decimals)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty<object>(data))
                {
                    return 0;
                }
                else
                {
                    decimal temp = Convert.ToDecimal(data);
                    return Math.Round(temp, decimals);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }

        #endregion

        #endregion

        #region 将数据转换为指定类型

        #region 重载一

        /// <summary>
        /// 将数据转换为指定类型
        /// </summary>
        /// <param name="data">转换的数据</param>
        /// <param name="targetType">转换的目标类型</param>
        public static object ConvertTo(object data, Type targetType)
        {
            //如果数据为空，则返回
            if (ValidationHelper.IsNullOrEmpty(data))
            {
                return null;
            }

            try
            {
                //如果数据实现了IConvertible接口，则转换类型
                if (data is IConvertible)
                {
                    return Convert.ChangeType(data, targetType);
                }
                else
                {
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        #endregion

        #region 重载二

        /// <summary>
        /// 将数据转换为指定类型
        /// </summary>
        /// <typeparam name="T">转换的目标类型</typeparam>
        /// <param name="data">转换的数据</param>
        public static T ConvertTo<T>(object data)
        {
            //如果数据为空，则返回
            if (ValidationHelper.IsNullOrEmpty(data))
            {
                return default(T);
            }

            try
            {
                //如果数据是T类型，则直接转换
                if (data is T)
                {
                    return (T)data;
                }

                //如果目标类型是枚举
                if (typeof(T).BaseType == typeof(Enum))
                {
                    return EnumHelper.GetInstance<T>(data);
                }

                //如果数据实现了IConvertible接口，则转换类型
                if (data is IConvertible)
                {
                    return (T)Convert.ChangeType(data, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return default(T);
            }
        }

        #endregion

        #endregion

        #region string型转换为bool型

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string strValue)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(strValue))
                {
                    strValue = strValue.Trim();
                    if (((string.Compare(strValue, "true", true) == 0) || (string.Compare(strValue, "yes", true) == 0)) ||
                            (string.Compare(strValue, "1", true) == 0))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region string型转换为时间型

        /// <summary>
        /// string型转换为时间型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的时间类型结果</returns>
        public static DateTime StrToDateTime(object strValue, DateTime defValue)
        {
            DateTime intValue = default(DateTime);
            try
            {
                if ((strValue == null) || (strValue.ToString().Length > 20))
                {
                    return defValue;
                }

                if (!DateTime.TryParse(strValue.ToString(), out intValue))
                {
                    intValue = defValue;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return intValue;
        }

        #endregion

        #region 将输入的long型转换为DateTime型

        /// <summary>
        /// 将输入的long型转换为DateTime型
        /// </summary>
        /// <param name="inputLong">long数值</param>
        /// <returns></returns>
        public static DateTime BigIntToDateTime(long inputLong)
        {
            DateTime d = default(DateTime);
            try
            {
                DateTime beginTime = DateTime.Now.Date;
                DateTime.TryParse("1970-01-01", out beginTime);
                double addDays = inputLong / (double)(24 * 3600);
                d = beginTime.AddDays(addDays);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return d;
        }

        #endregion

        #region object型转换为decimal型

        /// <summary>
        /// object型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(object strValue)
        {
            decimal result = default(decimal);
            try
            {

                if (!Convert.IsDBNull(strValue) && !Equals(strValue, null))
                {
                    result = StrToDecimal(strValue.ToString());
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region string型转换为decimal型

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string strValue)
        {
            decimal num = default(decimal);
            try
            {
                decimal.TryParse(strValue, out num);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return num;
        }

        #endregion

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string input, decimal defaultValue)
        {
            decimal num = defaultValue;
            try
            {
                if (decimal.TryParse(input, out num))
                {

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return num;
        }

        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的double类型结果</returns>
        public static double StrToDouble(object strValue)
        {
            double result = 0.0;
            try
            {
                if (!Convert.IsDBNull(strValue) && !Equals(strValue, null))
                {
                    result = StrToDouble(strValue.ToString());
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的double类型结果</returns>
        public static double StrToDouble(string strValue)
        {
            double num = default(double);
            try
            {
                double.TryParse(strValue, out num);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return num;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的float类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            float intValue = defValue;
            try
            {
                if ((strValue == null) || (strValue.ToString().Length > 10))
                {
                    return defValue;
                }
                if (strValue != null)
                {
                    bool IsFloat = new Regex(@"^([-]|[0-9])[0-9]*(\.\w*)?$").IsMatch(strValue.ToString());
                    if (IsFloat)
                    {
                        intValue = Convert.ToSingle(strValue);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return intValue;
        }

        /// <summary>
        /// string型转换为int型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果.如果要转换的字符串是非数字,则返回-1.</returns>
        public static int StrToInt(object strValue)
        {
            int defValue = -1;
            int intValue = defValue;
            try
            {

                if ((strValue == null) || (strValue.ToString() == string.Empty) || (strValue.ToString().Length > 10))
                {
                    return defValue;
                }

                string val = strValue.ToString();
                string firstletter = val[0].ToString();

                if (val.Length == 10 && ValidationHelper.IsNumber(firstletter) && int.Parse(firstletter) > 1)
                {
                    return defValue;
                }
                else if (val.Length == 10 && !ValidationHelper.IsNumber(firstletter))
                {
                    return defValue;
                }



                if (strValue != null)
                {
                    bool IsInt = new Regex(@"^([-]|[0-9])[0-9]*$").IsMatch(strValue.ToString());
                    if (IsInt)
                    {
                        intValue = Convert.ToInt32(strValue);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return intValue;
        }

        /// <summary>
        /// string型转换为int型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object strValue, int defValue)
        {
            int intValue = defValue;
            try
            {
                if ((strValue == null) || (strValue.ToString() == string.Empty) || (strValue.ToString().Length > 10))
                {
                    return defValue;
                }

                string val = strValue.ToString();
                string firstletter = val[0].ToString();

                if (val.Length == 10 && ValidationHelper.IsNumber(firstletter) && int.Parse(firstletter) > 1)
                {
                    return defValue;
                }
                else if (val.Length == 10 && !ValidationHelper.IsNumber(firstletter))
                {
                    return defValue;
                }

                if (strValue != null)
                {
                    bool IsInt = new Regex(@"^([-]|[0-9])[0-9]*$").IsMatch(strValue.ToString());
                    if (IsInt)
                    {
                        intValue = Convert.ToInt32(strValue);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return intValue;
        }

        /// <summary>
        /// 将long型数值转换为Int32类型
        /// </summary>
        /// <param name="objNum"></param>
        /// <returns></returns>
        public static int SafeInt32(object objNum)
        {
            int result = 0;
            try
            {
                string strNum = objNum.ToString();
                if (ValidationHelper.IsNumber(strNum))
                {
                    if (strNum.Length > 9)
                    {
                        return int.MaxValue;
                    }
                    result = Int32.Parse(strNum);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }
    }
}