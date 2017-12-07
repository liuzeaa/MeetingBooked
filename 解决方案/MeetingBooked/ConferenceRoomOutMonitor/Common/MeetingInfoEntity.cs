/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： 会议信息子项
文件功能描述： 会议室跟踪平台
创建标识： 2015-8-26
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ConferenceRoomOutMonitor.Common
{
    public class MeetingInfoEntity : INotifyPropertyChanged
    {
        #region 实时更新

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region 映射属性

        string conf_WorkshopId;
        /// <summary>
        /// 会议室ID号
        /// </summary>
        public string Conf_WorkshopId
        {
            get { return conf_WorkshopId; }
            set
            {
                if (conf_WorkshopId != value)
                {
                    this.OnPropertyChanged("ID");
                    conf_WorkshopId = value;
                }
            }
        }

        string conf_Title;
        /// <summary>
        /// 会议标题
        /// </summary>
        public string Conf_Title
        {
            get { return conf_Title; }
            set
            {
                if (conf_Title != value)
                {
                    conf_Title = value;
                    this.OnPropertyChanged("Conf_Title");
                }
            }
        }

        DateTime conf_MeetDate;
        /// <summary>
        /// 会议日期
        /// </summary>
        public DateTime Conf_MeetDate
        {
            get { return conf_MeetDate; }
            set
            {
                if (conf_MeetDate != value)
                {
                    conf_MeetDate = value.ToLocalTime();
                    this.OnPropertyChanged("Conf_MeetDate");
                }
            }
        }

        DateTime conf_StartTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Conf_StartTime
        {
            get { return conf_StartTime; }
            set
            {
                if (conf_StartTime != value)
                {
                    conf_StartTime = value.ToLocalTime();
                    this.OnPropertyChanged("StartTime");
                }
            }
        }

        DateTime conf_EndTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Conf_EndTime
        {
            get { return conf_EndTime; }
            set
            {
                if (conf_EndTime != value)
                {
                    conf_EndTime = value.ToLocalTime();
                    this.OnPropertyChanged("EndTime");
                }
            }
        }

        #endregion
    }
}
