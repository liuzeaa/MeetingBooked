/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： MjParam
文件功能描述： 门禁参数实体
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
    public struct MjParam
    {
        /// <summary>
        /// 门禁开启时间
        /// </summary>
        public string DoorOpenTime { get; set; }

        /// <summary>
        /// 门禁弹出时间
        /// </summary>
        public string DoorOpenAlertTime { get; set; }

        /// <summary>
        /// 门禁开启类型
        /// </summary>
        public string DoorOpenType { get; set; }

        /// <summary>
        /// 多重身份
        /// </summary>
        public string DuoChongShenFen { get; set; }

        /// <summary>
        /// 门禁开启超时
        /// </summary>
        public string DoorOpenOutTimeAlert { get; set; }

        /// <summary>
        /// 门禁未响应自动开启
        /// </summary>
        public string DoorNoResoneOpenAlert { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public string DoorHuSuoFlag { get; set; }

        /// <summary>
        /// 门禁开启关闭标示
        /// </summary>
        public string DoorOpenOrCloseFlag { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public string DoorButtonTimeStoneFlag { get; set; }

        /// <summary>
        /// ????
        /// </summary>
        public string CardAndPwdTimeStoneFlag { get; set; }

        /// <summary>
        /// 多重标示
        /// </summary>
        public string DuoChongFlag { get; set; }

        /// <summary>
        /// 读取模式
        /// </summary>
        public string ReaderModel { get; set; }

    }
}
