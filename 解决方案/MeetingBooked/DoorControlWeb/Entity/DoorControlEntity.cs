/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： DoorControlEntity
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

namespace DoorControlWeb.Entity
{
    public class DoorControlEntity
    {
        public int Guid { get; set; }

        /// <summary>
        /// 卡号列表
        /// </summary>
        public string CardList { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 门禁控制器IP地址
        /// </summary>
        public string ControlIP { get; set; }

        /// <summary>
        /// 门禁控制器端口
        /// </summary>
        public string ControlPort { get; set; }

        /// <summary>
        /// 是否已写入
        /// </summary>
        public int IsWrite { get; set; }

        /// <summary>
        /// 刷卡记录,达到一定阀值进行统一白名单授权
        /// </summary>
        public int CreditCardLimit { get; set; }

        /// <summary>
        /// 刷卡列表（映射卡号列表）
        /// </summary>
        public string CreditCardList { get; set; }
    }
}
