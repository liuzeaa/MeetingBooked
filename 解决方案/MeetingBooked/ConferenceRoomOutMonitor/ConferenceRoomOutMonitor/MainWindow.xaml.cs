/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： MainWindow
文件功能描述： 会议室跟踪平台
创建标识： 2015-8-26
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/

using ConferenceCommon.LogHelper;
using ConferenceCommon.SharePointHelper;
using ConferenceCommon.WebHelper;
using ConferenceRoomOutMonitor.Common;
using ConferenceRoomOutMonitor.View;
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
using System.Windows.Threading;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;

namespace ConferenceRoomOutMonitor
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 内部字段

        /// <summary>
        /// 流媒体播放器
        /// </summary>
        VlcControl vlcPlayer = null;

        /// <summary>
        /// 会议信息存储列表名称
        /// </summary>
        string meetInfoListName = "Conf_BasicInfo";

        /// <summary>
        /// 会议室信息存储列表名称
        /// </summary>
        string meetRoomListName = "BoardRoom";

        #endregion

        #region 静态字段

        /// <summary>
        /// SharePoint客户端对象模型管理
        /// </summary>
        public static ClientContextManage clientContextManage = new ClientContextManage();

        #endregion

        #region 构造函数

        public MainWindow()
        {
            try
            {
                //日志初始化
                LogManage.LogInit();
                //UI加载
                InitializeComponent();
                //初始化流媒体播放器
                this.VlcMediaPlayerInit();
                //流媒体播放器开始播放
                this.Play(Constant.CameraRtspAddress);
                //设置会议室名称
                this.GetMeetRoomDisplay();
                //会议信息数据填充
                this.MeetingInfoFill();
                //自动更新数据
                this.UpdateView();
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

        #region 初始化流媒体播放器

        /// <summary>
        /// 初始化流媒体播放器
        /// </summary>
        public void VlcMediaPlayerInit()
        {
            try
            {
                //vlc配置参数
                VlcContext.StartupOptions.IgnoreConfig = true;
                VlcContext.StartupOptions.LogOptions.LogInFile = false;
                VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = false;
                VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.None;
                VlcContext.LibVlcPluginsPath = Environment.CurrentDirectory + "\\plugins";
                VlcContext.LibVlcDllsPath = Environment.CurrentDirectory ;
                //流媒体播放器初始化
                VlcContext.Initialize();
                //播放器实例生成
                vlcPlayer = new VlcControl();

                // 创建绑定，绑定Image
                Binding bing = new Binding();
                bing.Source = vlcPlayer;
                bing.Path = new PropertyPath("VideoSource");
                img.SetBinding(Image.SourceProperty, bing);
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

        #region 流媒体播放器开始播放

        /// <summary>
        /// 流媒体播放器开始播放
        /// </summary>
        /// <param name="uri">播放地址</param>
        public void Play(string uri)
        {
            try
            {
                if (this.vlcPlayer != null)
                {
                    //设置播放地址
                    LocationMedia media = new LocationMedia(uri);
                    //播放
                    this.vlcPlayer.Play(media);
                }
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

        #region 会议信息数据填充

        /// <summary>
        /// 会议信息数据填充
        /// </summary>
        public void MeetingInfoFill()
        {
            try
            {
                //查询语句
                string query = "<View><Query><Where><Geq><FieldRef Name='Conf_EndTime'/><Value IncludeTimeValue='TRUE' Type='DateTime'>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "</Value></Geq><Eq><FieldRef Name='Conf_WorkshopId' /><Value Type='Text'>" + Constant.MeetRoomID + "</Value></Eq></Where><OrderBy><FieldRef Name='Conf_EndTime' Ascending='True' /></OrderBy></Query></View>";

                //获取会议数据数据源
                var meetingInfoList = MainWindow.clientContextManage.ClientGetEntityList<MeetingInfoEntity>(Constant.MeetingInfoWebSiteUri, this.meetInfoListName, query);

                //加载数据前先进行清空
                this.stackPanel.Children.Clear();

                //遍历会议信息源
                foreach (var item in meetingInfoList)
                {
                    //生成会议信息子项
                    MeetingInfoItem_View meetingInfoEntity = new MeetingInfoItem_View();
                    //填充会议信息子项
                    meetingInfoEntity.DataBinding(item);
                    //父容器添加子项
                    this.stackPanel.Children.Add(meetingInfoEntity);
                }
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

        #region 设置会议室名称

        /// <summary>
        /// 设置会议室名称
        /// </summary>
        public void GetMeetRoomDisplay()
        {
            try
            {
                //创建客户端对象模型实例
                MainWindow.clientContextManage.CreateClient(Constant.MeetingInfoWebSiteUri, Constant.UserName, Constant.Password, Constant.Domain);

                //查询语句
                string query = "<View><Query><Where> <Eq>  <FieldRef Name='ID' /> <Value Type='Counter'>" + Constant.MeetRoomID + "</Value></Eq></Where></Query></View>";

                //获取会议数据数据源
                var rooInfo = MainWindow.clientContextManage.ClientGetDic(Constant.MeetingInfoWebSiteUri, this.meetRoomListName, query);

                //查看是否包含该字段类型
                if (rooInfo.Count > 0 && rooInfo[0].ContainsKey("Room_Name"))
                {
                    this.txtRoom_Name.Text = Convert.ToString(rooInfo[0]["Room_Name"]);
                }
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

        #region 自动更新数据

        /// <summary>
        /// 自动更新数据
        /// </summary>
        public void UpdateView()
        {
            try
            {
                //使用计时器更新会议信息
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(60);
                timer.Tick += MeetInfoReflesh;
                timer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 计时器更新会议信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MeetInfoReflesh(object sender, EventArgs e)
        {
            try
            {
                #region old solution(更新会议信息)

                ////验证第一条会议信息
                //if (this.stackPanel.Children.Count > 0)
                //{
                //    //获取第一条会议模板
                //    var firstMeetInfo = this.stackPanel.Children[0];
                //    //类型转换
                //    if (firstMeetInfo is MeetingInfoItem_View)
                //    {
                //        //获取会议实体
                //        var meetingInfoEntity = (firstMeetInfo as MeetingInfoItem_View).MeetingInfoEntity;
                //        //无效的第一条会议记录删除
                //        if (meetingInfoEntity.Conf_EndTime > DateTime.Now)
                //        {
                //            this.stackPanel.Children.RemoveAt(0);
                //        }
                //    }
                //}
                
                #endregion

                //会议信息数据填充
                this.MeetingInfoFill();               
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
