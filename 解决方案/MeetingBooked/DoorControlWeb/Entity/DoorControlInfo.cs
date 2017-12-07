using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoorControlWeb.Entity
{
    [Serializable]
    public class DoorControlInfo : ReturnDataBase
    {
      
        List<DoorControlInfoItem> doorControlInfoItemList = new List<DoorControlInfoItem>();
        /// <summary>
        /// 门禁控制器集合
        /// </summary>
        public List<DoorControlInfoItem> DoorControlInfoItemList
        {
            get { return doorControlInfoItemList; }
            set { doorControlInfoItemList = value; }
        }
    }

    public class DoorControlInfoItem
    {
        /// <summary>
        /// 门禁控制器IP地址
        /// </summary>
        public string ControlIP { get; set; }

        /// <summary>
        /// 门禁控制器端口
        /// </summary>
        public string ControlPort { get; set; }

        /// <summary>
        /// MAC
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string SearialNumber { get; set; }

        /// <summary>
        /// 控制器地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
    }
}
