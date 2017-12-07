/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： SignListEntity
文件功能描述： 签到记录实体
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
    public class SignListEntity
    {
        public int Guid { get; set; }
        /// <summary>
        /// 卡号或密码
        /// </summary>
        public string KahaoOrMima { get; set; }
        /// <summary>
        /// 门禁IP
        /// </summary>
        public string ControlIP { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public string ControlPort { get; set; }
        /// <summary>
        /// 刷卡时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 门序号
        /// </summary>
        public string MenXuHao { get; set; }
        /// <summary>
        /// 签到或签退（"0"：签到，"1"：签退）
        /// </summary>
        public string JinChu { get; set; }
        /// <summary>
        /// 刷卡标志
        /// </summary>
        public string ShuaKaBiaoZhi { get; set; }
        /// <summary>
        /// 是否有效（"0"无效，"1"有效）
        /// </summary>
        public string IsEffective { get; set; }
    }
}