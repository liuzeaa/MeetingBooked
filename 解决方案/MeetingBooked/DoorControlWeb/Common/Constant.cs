/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： Constant
文件功能描述： 资源管理区域
创建标识： 
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DoorControlWeb
{
    public class Constant
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string SqlConnectr = ConfigurationManager.ConnectionStrings["SqlConnectr"].ConnectionString;

        /// <summary>
        /// 查询数据写入白名单时间间隔
        /// </summary>
        public static int timer_Write = Convert.ToInt32(ConfigurationManager.AppSettings["timer_Write"]);

        /// <summary>
        /// 查询无效数据【白名单】时间间隔
        /// </summary>
        public static int timer_Delete = Convert.ToInt32(ConfigurationManager.AppSettings["timer_Delete"]);

        /// <summary>
        /// 多卡刷门阀值
        /// </summary>
        public static int CreditCardLimit = Convert.ToInt32(ConfigurationManager.AppSettings["CreditCardLimit"]);
      
        /// <summary>
        /// 卡号分割符
        /// </summary>
        public static char CardListSplitChar = Convert.ToChar(ConfigurationManager.AppSettings["CardListSplitChar"]);

        /// <summary>
        /// 会议之前的10分钟进行清除门禁的记录
        /// </summary>
        public static int BeforeMinites = Convert.ToInt32(ConfigurationManager.AppSettings["BeforeMinites"]);
      
        /// <summary>
        /// 强制开门之后取消强制前的睡眠时间 （毫秒）
        /// </summary>
        public static int SleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["SleepTime"]);

        /// <summary>
        /// 统计不良记录产生黑名单的周期（天数）
        /// </summary>
        public static int SetPeriod = Convert.ToInt32(DoorControl.GetConfig("SetPeriod"));

        /// <summary>
        /// 统计不良记录产生黑名单的次数
        /// </summary>
        public static int BadRecordCount = Convert.ToInt32(DoorControl.GetConfig("BadRecordCount"));
        /// <summary>
        /// 黑名单禁用天数（天数）
        /// </summary>
        public static int BlackListValidDate = Convert.ToInt32(DoorControl.GetConfig("BlackListValidDate"));

        /// <summary>
        /// 所有的门禁IP列表
        /// </summary>
        public static string DoorControlIP = Convert.ToString(ConfigurationManager.AppSettings["DoorControlIP"]);
        
           
    }
}
