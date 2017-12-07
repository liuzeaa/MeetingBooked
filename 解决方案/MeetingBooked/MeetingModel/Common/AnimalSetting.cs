using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingModel.Common
{
    public class AnimalSetting
    {
        public static Dictionary<string, string> GetAnimal()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string[] shuxiang = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };
            foreach (var item in shuxiang)
            {
                dic.Add(item, "");
            }

            for (int i = 1; i < 50; i++)
            {
                int birthday = DateTime.Now.Year - (i) + 1;
                int tmp = birthday - 1960;

                if (birthday < 1960)
                {
                    var intm = tmp % 12 + 12;
                    var index = shuxiang[intm];
                    dic[index] = dic[index] += i + ",";
                }
                else
                {
                    var index = shuxiang[tmp % 12];
                    dic[index] = dic[index] += i + ",";
                }
            }
            return dic;
        }
    }
}