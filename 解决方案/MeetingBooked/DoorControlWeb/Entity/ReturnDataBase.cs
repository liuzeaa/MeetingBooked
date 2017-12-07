/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： ReturnDataBase
文件功能描述： 数据返回基类
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
    public class ReturnDataBase
    {

        public ReturnDataBase()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private string method;
        /// <summary>
        /// 具体调用的方法名称
        /// </summary>
        public string Method
        {
            get { return method; }
            set { method = value; }
        }

        private string _innerError = string.Empty;
        /// <summary>
        /// 内部异常（用户可分析）
        /// </summary>
        public string InnerError
        {
            set { _innerError = value; }
            get { return _innerError; }
        }

        private string errMessage = string.Empty;
        /// <summary>
        /// 程序异常（管理员分析）
        /// </summary>
        public string ErrMessage
        {
            get { return errMessage; }
            set { errMessage = value; }
        }

        bool successed;
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool Successed
        {
            get { return successed; }
            set { successed = value; }
        }

        string return_Param;
        /// <summary>
        /// 返回的关于设备的数据
        /// </summary>
        public string Return_Param
        {
            get { return return_Param; }
            set { return_Param = value; }
        }
    }
}
