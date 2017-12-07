/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： 会议信息子项视图
文件功能描述： 会议室跟踪平台
创建标识： 2015-8-26
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/

using ConferenceCommon.LogHelper;
using ConferenceRoomOutMonitor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConferenceRoomOutMonitor.View
{
    /// <summary>
    ///会议具体信息视图
    /// </summary>
    public partial class MeetingInfoItem_View : UserControl
    {
        #region 属性

        public MeetingInfoEntity MeetingInfoEntity { get; set; }

        #endregion


        #region 构造函数

        public MeetingInfoItem_View()
        {
                try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }         
        }

        #endregion

        #region 填充数据

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="meetingInfoEntity">会议信息实体</param>
        public void DataBinding(MeetingInfoEntity meetingInfoEntity)
        {
            try
            {
                //绑定会议实体
                this.MeetingInfoEntity = meetingInfoEntity;
                //绑定当前上下文
                this.DataContext = meetingInfoEntity;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        #endregion
    }
}
