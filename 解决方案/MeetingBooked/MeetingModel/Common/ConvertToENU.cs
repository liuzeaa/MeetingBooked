using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MeetingModel
{
    public static class ConvertToENU<T> where T : new()
    {
    
        /// <summary>
        /// 将list对象集合转为dic
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ListToDic(IEnumerable<T> list)
        {
            List<Dictionary<string, object>> dic = new List<Dictionary<string, object>>();
            try
            {
                //定义一个临时变量  
                string tempName = string.Empty;
                Type t = typeof(T);
                PropertyInfo[] propInfos = t.GetProperties();
                foreach (var u1 in list)
                {
                    Dictionary<string, object> dd = new Dictionary<string, object>();
                    foreach (var pi in propInfos)
                    {
                        string name = pi.Name;
                        //if (pi.PropertyType == typeof(DateTime?))
                        //{
                        //    string value = ((DateTime)pi.GetValue(u1, null)).ToString("yyyy-MM-dd HH:mm:ss");
                        //    dd.Add(name, value);
                        //}

                        object value = pi.GetValue(u1, null);
                        dd.Add(name, value);

                    }
                    dic.Add(dd);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dic;
        }

     
    }
}
