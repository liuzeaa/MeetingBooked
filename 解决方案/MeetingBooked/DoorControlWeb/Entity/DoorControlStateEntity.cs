/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： DoorControlStateEntity
文件功能描述： 门禁状态实体
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
    [Serializable]
   public  class DoorControlStateEntity : ReturnDataBase
    {
        string forceStatus;
        /// <summary>
        /// 门禁强制状态
        /// </summary>
        public string ForceStatus
        {
            get { return forceStatus; }
            set { forceStatus = value; }
        }

        string doorStatus;
        /// <summary>
        /// 门禁状态
        /// </summary>
        public string DoorStatus
        {
            get { return doorStatus; }
            set { doorStatus = value; }
        }

        string ctrlTime;
        /// <summary>
        /// 门禁时间
        /// </summary>
        public string CtrlTime
        {
            get { return ctrlTime; }
            set { ctrlTime = value; }
        }
    }
}
