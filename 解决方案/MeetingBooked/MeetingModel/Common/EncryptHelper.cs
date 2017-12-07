using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeetingModel
{
    public class EncryptHelper
    {
        #region MD5加密

        #region MD5算法加密字符串( 16位 )

        /// <summary>
        /// MD5算法加密字符串( 16位 )
        /// </summary>
        /// <param name="text">要加密的字符串</param>    
        public static string Md5By16(string text)
        {
            //如果字符串为空，则返回
            if (ValidationHelper.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            try
            {
                //创建MD5密码服务提供程序
                var md5 = new MD5CryptoServiceProvider();

                //获取加密字符串
                string result = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(text)), 4, 8);

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                return result.Replace("-", "");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return string.Empty;
            }
        }

        #endregion

        #region MD5算法加密字符串( 32位 )

        #region 重载1

        /// <summary>
        /// MD5算法加密字符串( 32位 )
        /// </summary>
        /// <param name="text">要加密的字符串</param>    
        /// <param name="encoding">字符编码</param>    
        public static string MD5By32(string text, Encoding encoding)
        {
            //如果字符串为空，则返回
            if (ValidationHelper.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            try
            {
                //创建MD5密码服务提供程序
                var md5 = new MD5CryptoServiceProvider();

                //计算传入的字节数组的哈希值
                byte[] hashCode = md5.ComputeHash(encoding.GetBytes(text));

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                string temp = "";
                int len = hashCode.Length;
                for (int i = 0; i < len; i++)
                {
                    temp += hashCode[i].ToString("x").PadLeft(2, '0');
                }
                return temp;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return string.Empty;
            }
        }

        #endregion

        #region 重载2

        /// <summary>
        /// MD5算法加密字符串( 32位 )
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        public static string Md5By32(string text)
        {
            string result = string.Empty;
            try
            {
                result = MD5By32(text, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 重载3

        /// <summary>
        /// MD5算法加密字符串( 支付宝专用 )
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        public static string Md5ByAlipay(string text)
        {
            string result = string.Empty;
            try
            {
                result = MD5By32(text, Encoding.GetEncoding("gb2312"));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #endregion

        #endregion

        #region Base64加密

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string text)
        {
            string result = string.Empty;
            try
            {
                //如果字符串为空，则返回
                if (ValidationHelper.IsNullOrEmpty<string>(text))
                {
                    return "";
                }

                var base64Code = new[]
                                 {
                                     'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                                     'Q', 'R', 'S', 'T',
                                     'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                                     'k', 'l', 'm', 'n',
                                     'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3',
                                     '4', '5', '6', '7',
                                     '8', '9', '+', '/', '='
                                 };
                const byte empty = 0;
                var byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
                int messageLen = byteMessage.Count;
                int page = messageLen / 3;
                int use;
                if ((use = messageLen % 3) > 0)
                {
                    for (int i = 0; i < 3 - use; i++)
                        byteMessage.Add(empty);
                    page++;
                }
                var outmessage = new StringBuilder(page * 4);
                for (int i = 0; i < page; i++)
                {
                    var instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    var outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    else
                        outstr[2] = 64;
                    if (!instr[2].Equals(empty))
                        outstr[3] = (instr[2] & 0x3f);
                    else
                        outstr[3] = 64;
                    outmessage.Append(base64Code[outstr[0]]);
                    outmessage.Append(base64Code[outstr[1]]);
                    outmessage.Append(base64Code[outstr[2]]);
                    outmessage.Append(base64Code[outstr[3]]);
                }
                result = outmessage.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region Base64解密

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string text)
        {
            string result = string.Empty;
            try
            {
                //如果字符串为空，则返回
                if (ValidationHelper.IsNullOrEmpty<string>(text))
                {
                    result = string.Empty;
                }

                //将空格替换为加号
                text = text.Replace(" ", "+");

                if ((text.Length % 4) != 0)
                {
                    result = "包含不正确的BASE64编码";
                }
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    result = "包含不正确的BASE64编码";
                }
                const string base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int page = text.Length / 4;
                var outMessage = new ArrayList(page * 3);
                char[] message = text.ToCharArray();
                for (int i = 0; i < page; i++)
                {
                    var instr = new byte[4];
                    instr[0] = (byte)base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)base64Code.IndexOf(message[i * 4 + 3]);
                    var outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64)
                    {
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    if (instr[3] != 64)
                    {
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                        outMessage.Add(outstr[1]);
                    if (outstr[2] != 0)
                        outMessage.Add(outstr[2]);
                }
                var outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                result = Encoding.Default.GetString(outbyte);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region HMAC加密

        /// <summary>
        /// HMAC加密
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <param name="key">键</param>
        public static string Hmac32(string text, string key)
        {
            string result = string.Empty;
            try
            {
                //如果字符串为空，则返回
                if (ValidationHelper.IsNullOrEmpty(text))
                {
                    result = string.Empty;
                }

                const string ipad = "6666666666666666666666666666666666666666666666666666666666666666";
                const string opad = @"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\";
                string kIpad = FunMd5(StrXor(key, ipad) + text);

                string kOpad = StrXor(key, opad);

                byte[] test = Hexstr2Array(kIpad);

                char[] b = Encoding.GetEncoding(1252).GetChars(test);
                string temp = b.Aggregate("", (current, t) => current + t);
                temp = kOpad + temp;

                result = FunMd5(temp).ToLower();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        private static string FunMd5(string str)
        {
            string ret = "";
            try
            {
                byte[] b = Encoding.GetEncoding(1252).GetBytes(str);
                b = new MD5CryptoServiceProvider().ComputeHash(b);

                for (int i = 0; i < b.Length; i++)
                    ret += b[i].ToString("x").PadLeft(2, '0');
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }

        private static Byte[] Hexstr2Array(string hexStr)
        {
            byte[] result = null;
            try
            {
                const string hex = "0123456789ABCDEF";
                string str = hexStr.ToUpper();
                int len = str.Length;
                result = new byte[len / 2];
                for (int i = 0; i < len / 2; i++)
                {
                    int numHigh = hex.IndexOf(str[i * 2]);
                    int numLow = hex.IndexOf(str[i * 2 + 1]);
                    result[i] = Convert.ToByte(numHigh * 16 + numLow);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        private static string StrXor(string password, string pad)
        {
            string iResult = "";
            try
            {
                int kLen = password.Length;

                for (int i = 0; i < 64; i++)
                {
                    if (i < kLen)
                        iResult += Convert.ToChar(pad[i] ^ password[i]);
                    else
                        iResult += Convert.ToChar(pad[i]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return iResult;
        }

        #endregion

        #region 使用 缺省密钥字符串 加密/解密string

        /// <summary>
        /// 使用缺省密钥字符串加密string
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original)
        {
            string result = string.Empty;
            try
            {
                result = Encrypt(original, "LITIANPING");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 使用缺省密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original)
        {
            string result = string.Empty;
            try
            {
                result = Decrypt(original, "LITIANPING", Encoding.Default);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 使用 给定密钥字符串 加密/解密string

        /// <summary>
        /// 使用给定密钥字符串加密string
        /// </summary>
        /// <param name="original">原始文字</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key)
        {
            string result = string.Empty;
            try
            {
                byte[] buff = Encoding.Default.GetBytes(original);
                byte[] kb = Encoding.Default.GetBytes(key);

                result = Convert.ToBase64String(Encrypt(buff, kb));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 使用给定密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original, string key)
        {
            string result = string.Empty;
            try
            {
                result = Decrypt(original, key, Encoding.Default);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            string result = string.Empty;
            try
            {
                byte[] buff = Convert.FromBase64String(encrypted);
                byte[] kb = Encoding.Default.GetBytes(key);
                result = encoding.GetString(Decrypt(buff, kb));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }

        #endregion

        #region 使用 缺省密钥字符串 加密/解密/byte[]

        /// <summary>
        /// 使用缺省密钥字符串解密byte[]
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted)
        {
            byte[] result = null;
            try
            {
                byte[] key = Encoding.Default.GetBytes("LITIANPING");
                result = Decrypt(encrypted, key);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 使用缺省密钥字符串加密
        /// </summary>
        /// <param name="original">原始数据</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original)
        {
            byte[] result = null;
            try
            {
                byte[] key = Encoding.Default.GetBytes("LITIANPING");
                result = Encrypt(original, key);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region  使用 给定密钥 加密/解密/byte[]

        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        public static byte[] MakeMd5(byte[] original)
        {
            byte[] keyhash = null;
            try
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyhash = hashmd5.ComputeHash(original);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return keyhash;
        }


        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            byte[] result = null;
            try
            {
                var des = new TripleDESCryptoServiceProvider { Key = MakeMd5(key), Mode = CipherMode.ECB };

                result = des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            byte[] result = null;
            try
            {
                var des = new TripleDESCryptoServiceProvider { Key = MakeMd5(key), Mode = CipherMode.ECB };

                result = des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion
    }
}
