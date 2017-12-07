/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： SuperUserListEntity
文件功能描述： 超级用户实体
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
    public class SuperUserListEntity
    {
        public int Guid { get; set; }
        /// <summary>
        /// 超级用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public string ValidDate { get; set; }
        /// <summary>
        /// 指定门禁IP列表
        /// </summary>
        public string DoorControlList { get; set; }
    }
}