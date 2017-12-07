/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： BlackListEntity
文件功能描述： 黑名单实体
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
    public class BlackListEntity
    {
        public int Guid { get; set; }
        /// <summary>
        /// 卡号或密码
        /// </summary>
        public string KahaoOrMima { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime ValidDate { get; set; }
        /// <summary>
        /// 是否有效（标记是否删除）
        /// </summary>
        public string IsEffective { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }
    }
}