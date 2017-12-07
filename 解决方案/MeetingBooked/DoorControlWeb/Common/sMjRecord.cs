/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： sMjRecord
文件功能描述： 门禁刷卡记录
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
    public struct sMjRecord
    {
         /// <summary>
         /// 记录卡号或密码
         /// </summary>
         public string KahaoOrMima { get; set; }

         /// <summary>
         /// 时间
         /// </summary>
         public string Time { get; set; }

         /// <summary>
         /// 门序号
         /// </summary>
         public string MenXuhao { get; set; }

         /// <summary>
         /// 进出
         /// </summary>
         public string JinChu { get; set; }

         /// <summary>
         /// 刷卡标志
         /// </summary>
         public string ShuakaBiaozhi { get; set; }
    }
}
