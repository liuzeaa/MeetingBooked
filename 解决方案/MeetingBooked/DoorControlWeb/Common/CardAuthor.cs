/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： CardAuthor
文件功能描述： 卡信息
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
    public struct CardAuthor
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string Kahao { get; set; }
        
        /// <summary>
        /// 密码
        /// </summary>
        public string Mima { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        public string YouxiaoRiqi { get; set; }

        /// <summary>
        /// 运行通过门
        /// </summary>
        public string YunxuTongGuomen { get; set; }

        /// <summary>
        /// 主副卡
        /// </summary>
        public string ZhuFuKa { get; set; }

        /// <summary>
        /// 通讯时区
        /// </summary>
        public string TongxingShiqu { get; set; }

        /// <summary>
        /// ？？？？
        /// </summary>
        public string FanQianhui { get; set; }

        /// <summary>
        /// 假日管制
        /// </summary>
        public string JiariGuanzhi { get; set; }
       
    }
}
