/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： SuperUserListEntity
文件功能描述： 配置文件实体
创建标识： 
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoorControlWeb.Entity
{
    public class WebConfigEntity
    {
        /// <summary>
        /// 统计不良记录产生黑名单的周期（天数）
        /// </summary>
        public string SetPeriod { get; set; }
        /// <summary>
        /// 统计不良记录达到规定的次数而产生黑名单
        /// </summary>
        public string BadRecordCount { get; set; }
        /// <summary>
        /// 黑名单禁用天数（天数）
        /// </summary>
        public string BlackListValidDate { get; set; }
    }
}