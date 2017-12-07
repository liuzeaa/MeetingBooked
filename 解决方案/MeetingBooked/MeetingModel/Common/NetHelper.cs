using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MeetingModel
{
    public class NetHelper
    {
        #region Get方式请求
        /// <summary>
        /// Get方式请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RequestGetUrl(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(resStream, System.Text.Encoding.UTF8);
                String aaa = "";
                aaa = sr.ReadToEnd();
                resStream.Close();
                sr.Close();
                return aaa;
            }
            catch(Exception ex)
            { 
                LogHelper.Error(ex);
                return "";
            }

        }
        #endregion

        #region RequestPostUrl post方式提交数据
        public static string RequestPostUrl(string url, string content)//post方式向页面提交 
        {
            byte[] bs = Encoding.UTF8.GetBytes(content);
            string resultStream = null;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            req.ContentLength = bs.Length;

            req.Timeout = 20000;
            //设置发送内容
            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                    reqStream.Dispose();
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error(ex);
            }
            try
            {
                WebResponse wr = req.GetResponse();
                using (wr)
                {

                    //在这里对接收到的页面内容进行处理
                    Stream stream = wr.GetResponseStream();
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    resultStream = @sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                resultStream = "0";

            }
            return resultStream;

        }
        #endregion
    }
}
