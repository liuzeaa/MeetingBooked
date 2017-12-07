/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： DoorCtrlName
文件功能描述： 门禁实体
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
    public struct DoorCtrlName
    {
         /// <summary>
         /// 服务登陆密码
         /// </summary>
         public string ServerPwd { get; set; }

         /// <summary>
         /// 服务地址
         /// </summary>
         public string ServerAddress { get; set; }

         /// <summary>
         /// 服务端口
         /// </summary>
         public string ServerPort { get; set; }

         /// <summary>
         /// 通讯地址
         /// </summary>
         public string CommAddress { get; set; }

         /// <summary>
         /// 通讯端口号
         /// </summary>
         public string CommPort { get; set; }

         /// <summary>
         /// 门禁地址
         /// </summary>
         public string DoorAddress { get; set; }

         /// <summary>
         /// 门禁类型
         /// </summary>
         public string DoorType { get; set; }

         /// <summary>
         /// 门禁密码
         /// </summary>
         public string DoorPwd { get; set; }

         /// <summary>
         /// 退出模式
         /// </summary>
         public string ExitMode { get; set; }
    }
}
