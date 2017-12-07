/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： sMjAlarm
文件功能描述： 门禁报警实体
创建标识： 
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoorControlWeb.Common
{
     [Serializable]
    public struct sMjAlarm
    {
         /// <summary>
         /// 报警类型
         /// </summary>
         public string BaoJingLeixing { get; set; }

         /// <summary>
         /// 报警起止
         /// </summary>
         public string BaoJingQizhi { get; set; }

         /// <summary>
         /// 门序号
         /// </summary>
         public string MenXuhao { get; set; }

         /// <summary>
         /// 时间
         /// </summary>
         public string Time { get; set; }
    }
}
