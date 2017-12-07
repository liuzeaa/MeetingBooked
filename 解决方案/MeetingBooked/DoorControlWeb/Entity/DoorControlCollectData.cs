/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： DoorControlCollectData
文件功能描述： 门禁数据收集
创建标识： 
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/
using DoorControlWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoorControlWeb.Entity
{
    [Serializable]
    public class DoorControlCollectData : ReturnDataBase
    {
        int mjAlarmCount;
        /// <summary>
        /// 门禁【Alarm数量】
        /// </summary>
        public int MjAlarmCount
        {
            get { return mjAlarmCount; }
            set { mjAlarmCount = value; }
        }

        sMjAlarm[] mjAlarm;
        /// <summary>
        /// Alarm记录数据
        /// </summary>
        public sMjAlarm[] MjAlarm
        {
            get { return mjAlarm; }
            set { mjAlarm = value; }
        }

        sMjRecord[] mjRecord;
        /// <summary>
        /// 门禁记录数据
        /// </summary>
        public sMjRecord[] MjRecord
        {
            get { return mjRecord; }
            set { mjRecord = value; }
        }

        int mjRecordCount;
        /// <summary>
        /// 门禁记录数量
        /// </summary>
        public int MjRecordCount
        {
            get { return mjRecordCount; }
            set { mjRecordCount = value; }
        }
    }
}
