
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLC_Model;

namespace TLC_BLL
{
    public class BLLCommon
    {
        #region 根据第几页、每页条数增加起始条数、结束条数

        /// <summary>
        /// 根据第几页、每页条数增加起始条数、结束条数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public Hashtable AddStartEndIndex(Hashtable ht)
        {
            try
            {
                int PageIndex = Convert.ToInt32(ht["PageIndex"]);
                int PageSize = Convert.ToInt32(ht["PageSize"]);
                ht.Add("StartIndex", (((PageIndex - 1) * PageSize) + 1).ToString());
                ht.Add("EndIndex", (PageIndex * PageSize).ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ht;
        }

        #endregion

        #region DataTable转换成Json格式

        /// <summary>
        /// DataTable转换成Json格式
        /// </summary>
        /// <param name="dt">要转换的DataTable</param>        
        /// <returns>Json字符串</returns>
        public string DataTableToJson(DataTable dt)
        {
            string result = string.Empty;
            try
            {
                if (dt == null) return string.Empty;
                StringBuilder sb = new StringBuilder();
                sb.Append("{\"");
                sb.Append(dt.TableName);
                sb.Append("\":[");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("{");
                    foreach (DataColumn c in dt.Columns)
                    {
                        sb.Append("\"");
                        sb.Append(c.ColumnName);
                        sb.Append("\":\"");
                        sb.Append(r[c].ToString().Replace("\\", "//"));
                        sb.Append("\",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
                result = sb.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region DataTable转换成List

        /// <summary>
        /// DataTable转换成List
        /// </summary>
        /// <param name="dt">要转换的DataTable</param>        
        /// <returns>List<Dictionary<string, object>></returns>
        public List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dictionary<string, object> result = new Dictionary<string, object>();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            result.Add(dc.ColumnName, dr[dc].ToString());
                        }
                        list.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }

        #endregion

        #region JSON序列化  JSON反序列化

        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            string jsonString = string.Empty;
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, t);
                jsonString = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                //替换Json的Date字符串
                string p = @"\\/Date\((\d+)\+\d+\)\\/";
                MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return jsonString;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            T obj = default(T);
            try
            {
                //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
                string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
                MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                obj = (T)ser.ReadObject(ms);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return obj;
        }

        #endregion

        #region 将Json序列化的时间由/Date(1294499956278+0800)转为字符串

        /// <summary>
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            try
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
                dt = dt.ToLocalTime();
                result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 将时间字符串转为Json时间

        /// <summary>
        /// 将时间字符串转为Json时间
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            try
            {
                DateTime dt = DateTime.Parse(m.Groups[0].Value);
                dt = dt.ToUniversalTime();
                TimeSpan ts = dt - DateTime.Parse("1970-01-01");
                result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 转换

        /// <summary>
        /// DataTable转换成List
        /// </summary>
        /// <param name="dt">要转换的DataTable</param>        
        /// <returns>List<Dictionary<string, object>></returns>
        public static List<Dictionary<string, object>> DataTableToList2(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<string, object> result = new Dictionary<string, object>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        result.Add(dc.ColumnName, dr[dc].ToString());
                    }
                    list.Add(result);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }

        #endregion
    }
}
