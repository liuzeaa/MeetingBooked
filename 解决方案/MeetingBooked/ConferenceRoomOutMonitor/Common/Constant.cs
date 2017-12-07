/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： 公共资源【一般对应config配置文件里的数据】
文件功能描述： 会议室跟踪平台
创建标识： 2015-8-26
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceRoomOutMonitor.Common
{
    public class Constant
    {
        /// <summary>
        /// 摄像头rtsp访问地址
        /// </summary>
        public static string CameraRtspAddress = System.Configuration.ConfigurationManager.AppSettings["CameraRtspAddress"];

        /// <summary>
        /// 会议信息地址
        /// </summary>
        public static string MeetingInfoWebSiteUri = System.Configuration.ConfigurationManager.AppSettings["MeetingInfoWebSiteUri"];
        
              /// <summary>
        /// 会议信息地址IP
        /// </summary>
        public static string MeetingInfoWebSiteIP = System.Configuration.ConfigurationManager.AppSettings["MeetingInfoWebSiteIP"];

        /// <summary>
        /// 会议室ID号
        /// </summary>
        public static string MeetRoomID = System.Configuration.ConfigurationManager.AppSettings["MeetRoomID"];

        /// <summary>
        /// 设置会议信息更新时间间隔
        /// </summary>
        public static int InformationUpdateTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["InformationUpdateTime"]);

        /// <summary>
        /// 会议信息存储列表名称
        /// </summary>
        public static string meetInfoListName = System.Configuration.ConfigurationManager.AppSettings["meetInfoListName"];

        /// <summary>
        /// 会议室信息存储列表名称
        /// </summary>
        public static string meetRoomListName = System.Configuration.ConfigurationManager.AppSettings["meetRoomListName"];

        /// <summary>
        /// 验证用户名
        /// </summary>
        public static string UserName = "Administrator";

        /// <summary>
        /// 验证密码
        /// </summary>
        public static string Password = "STPassword2015";

        /// <summary>
        /// 域
        /// </summary>
        public static string Domain = "st";

        /// <summary>
        /// 当前客户端版本
        /// </summary>
        public static string CurrentVersion = "5.0";

        /// <summary>
        /// 需要打开的版本更新应用程序
        /// </summary>
        public static string VersionAppName =   Environment.CurrentDirectory+"\\"+ System.Configuration.ConfigurationManager.AppSettings["VersionAppName"];

    }
}
