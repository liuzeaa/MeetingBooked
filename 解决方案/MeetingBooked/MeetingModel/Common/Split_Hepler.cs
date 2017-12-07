using MeetingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingModel.Common
{
    public static class Split_Hepler
    {

        /// <summary>
        /// 字符串转long lists
        /// </summary>
        public static long[] str_to_longs(string strings)
        {
            long[] arri = new long[1];
            try
            {
                if (!string.IsNullOrEmpty(strings))
                {
                    //联系人的ID获取
                    string[] str = strings.Split(new char[] { ',' });
                    if (str.Count() > 0)
                    {
                        arri = Array.ConvertAll(str, new Converter<string, long>(StrTolong));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return arri;
        }

        /// <summary>
        /// 字符串转int lists
        /// </summary>
        public static int[] str_to_ints(string strings)
        {
            int[] arri = new int[0];
            try
            {
                if (!string.IsNullOrEmpty(strings))
                {
                    //联系人的ID获取
                    string[] str = strings.Split(new char[] { ',' });
                    if (str.Count() > 0)
                    {
                        arri = Array.ConvertAll(str, new Converter<string, int>(StrToint));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return arri;
        }

        /// <summary>
        /// 字符串转long lists
        /// </summary>
        public static string[] str_to_stringss(string strings)
        {
            string[] arri = new string[1];
            try
            {
                if (!string.IsNullOrEmpty(strings))
                {
                    //联系人的ID获取
                    arri = strings.Split(new char[] { ',' });                   
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return arri;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
         static long StrTolong(string str)
        {
            long result = 0;
            try
            {
                bool hasvalue = long.TryParse(str, out  result);
                if (!hasvalue)
                {
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }

         /// <summary>
         /// 类型转换
         /// </summary>
         /// <param name="str"></param>
         /// <returns></returns>
         static int StrToint(string str)
         {
             int result = 0;
             try
             {
                 bool hasvalue = int.TryParse(str, out  result);
                 if (!hasvalue)
                 {
                     result = 0;
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