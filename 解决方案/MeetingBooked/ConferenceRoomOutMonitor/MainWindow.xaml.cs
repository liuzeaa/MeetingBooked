using ConferenceCommon.DetectionHelper;
using ConferenceCommon.IconHelper;
using ConferenceCommon.LogHelper;
using ConferenceCommon.SharePointHelper;
using ConferenceCommon.TimerHelper;
using ConferenceRoomOutMonitor.Common;
using ConferenceRoomOutMonitor.ConferenceOutWebService;
using ConferenceRoomOutMonitor.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;

namespace ConferenceRoomOutMonitor
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 内部字段

        /// <summary>
        /// 流媒体播放器
        /// </summary>
        VlcControl vlcPlayer = null;

        /// <summary>
        /// 版本更新模块
        /// </summary>
        VersionUpdateSoapClient versionUpdateSoapClient = null;

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
                //鼠标移动到角落
                this.MoveMouseToCorner();

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
                //时间显示
                this.TimerDisplay();

                //判断并创建版本更新模块的实例
                if (this.versionUpdateSoapClient == null)
                {
                    this.versionUpdateSoapClient = new VersionUpdateSoapClient();
                    //版本获取完成事件
                    this.versionUpdateSoapClient.NeedVersionUpdateCompleted += versionUpdateSoapClient_NeedVersionUpdateCompleted;
                    //获取服务器完成事件
                    this.versionUpdateSoapClient.GetServiceDateTimeCompleted += versionUpdateSoapClient_GetServiceDateTimeCompleted;
                }

                //更新本地时间
                this.versionUpdateSoapClient.GetServiceDateTimeAsync();

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
                VlcContext.LibVlcDllsPath = Environment.CurrentDirectory;
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
                if (!DetectionManage.TestNetConnectity(Constant.MeetingInfoWebSiteIP))
                {
                    return;
                }
                //查询语句
                string query = "<View><Query><Where><And><Eq><FieldRef Name='Conf_WorkshopId' /><Value Type='Text'>" + Constant.MeetRoomID + "</Value></Eq><And><Geq><FieldRef Name='Conf_EndTime'/><Value IncludeTimeValue='TRUE' Type='DateTime'>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "</Value></Geq><Eq><FieldRef Name='Conf_ReleaseState' /><Value Type='Text'>审批成功</Value></Eq></And></And></Where><OrderBy><FieldRef Name='Conf_EndTime' Ascending='True' /></OrderBy></Query></View>";

                //获取会议数据数据源
                var meetingInfoList = MainWindow.clientContextManage.ClientGetEntityList<MeetingInfoEntity>(Constant.MeetingInfoWebSiteUri, Constant.meetInfoListName, query);

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
                if (!DetectionManage.TestNetConnectity(Constant.MeetingInfoWebSiteIP))
                {
                    return;
                }

                //创建客户端对象模型实例
                MainWindow.clientContextManage.CreateClient(Constant.MeetingInfoWebSiteUri, Constant.UserName, Constant.Password, Constant.Domain);

                //查询语句
                string query = "<View><Query><Where> <Eq>  <FieldRef Name='ID' /> <Value Type='Counter'>" + Constant.MeetRoomID + "</Value></Eq></Where></Query></View>";

                //获取会议室数据数据源
                var rooInfo = MainWindow.clientContextManage.ClientGetDic(Constant.MeetingInfoWebSiteUri, Constant.meetRoomListName, query);

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
                //timer.Interval = TimeSpan.FromMinutes(Constant.InformationUpdateTime);   
                timer.Interval = TimeSpan.FromSeconds(5);//修改：10分钟执行一次
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
                //会议信息数据填充
                this.MeetingInfoFill();
                //检测版本更新
                this.CheckVersion();
                //流媒体播放器开始播放
                this.Play(Constant.CameraRtspAddress);
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

        #region 检测版本更新

        /// <summary>
        /// 检测版本更新
        /// </summary>
        public void CheckVersion()
        {
            try
            {              
                //判断是否需要更新
                this.versionUpdateSoapClient.NeedVersionUpdateAsync(Constant.CurrentVersion);

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
        /// 版本获取完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void versionUpdateSoapClient_NeedVersionUpdateCompleted(object sender, NeedVersionUpdateCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result)
                    {
                        //通过进程去启动版本更新应用程序
                        Process process = new Process();
                        process.StartInfo.FileName = Constant.VersionAppName;
                        process.Start();
                        //关闭当前应用程序
                        Application.Current.Shutdown(0);
                    }
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

        #region 鼠标移动到角落

        /// <summary>
        /// 鼠标移动到角落
        /// </summary>
        public void MoveMouseToCorner()
        {
            try
            {
                //模仿鼠标点击
                Win32API.SetCursorPos(0, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height + 100);
                Win32API.mouse_event(Win32API.MouseEventFlag.Move, 0, 0, 0, UIntPtr.Zero);
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

        #region 时间显示

        /// <summary>
        /// 时间显示
        /// </summary>
        public void TimerDisplay()
        {
            try
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += (object sender, EventArgs e) =>
                    {
                        //显示当前时间
                        var nowTimer = DateTime.Now.ToString("yyyy-MM-dd   HH:mm:ss");
                        //
                        this.txtNowTime.Text = nowTimer;
                    };
                //设置为每一秒更新一次
                timer.Interval = TimeSpan.FromSeconds(1);
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

        #endregion

        #region 更新本地时间
        
        /// <summary>
        /// 更新本地时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void versionUpdateSoapClient_GetServiceDateTimeCompleted(object sender, GetServiceDateTimeCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (!string.IsNullOrEmpty(e.Result))
                    {
                        //获取服务器时间
                        DateTime serverDateTime = Convert.ToDateTime(e.Result);
                        //设置本地时间
                        TimerManage.SetDate(serverDateTime);
                    }
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
    }
}
