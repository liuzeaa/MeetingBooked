using ConferenceCommon.LogHelper;
using DoorControlMonitor.DoorControlWebService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DoorControlMonitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 字段

        /// <summary>
        /// 生成门禁服务客户端对象模型
        /// </summary>
        DoorControlWebService.DoorControlSoapClient client = new DoorControlWebService.DoorControlSoapClient();

        #endregion

        #region 构造函数

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                //搜索门禁控制器
                this.client.SearchDoorControlCompleted += client_SearchDoorControlCompleted;
                this.client.SearchDoorControlAsync();

                //激活服务
                this.client.RunOneMjWorkCompleted += client_RunOneMjWorkCompleted;
                this.client.RunOneMjWorkAsync();

                //时间同步完成事件
                this.client.SetTimeCompleted += client_SetTimeCompleted;
                //时区同步完成事件
                this.client.SetTimeZoneCompleted += client_SetTimeZoneCompleted;
                //取消强制完成事件
                this.client.CanclForceDoorCompleted += client_CanclForceDoorCompleted;
                //设置门禁参数
                this.client.SetMjParamCompleted += client_SetMjParamCompleted;

                //绑定当前上下文
                this.cmb.DataContext = this;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }
        #region 设置门禁参数

        /// <summary>
        /// 设置门禁参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_SetMjParamCompleted(object sender, SetMjParamCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Successed)
                {
                    this.txtTip.Text = "门禁参数设置成功";
                    //清除提示
                    this.ClearTipInfo(2, null);
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

        #region 搜索门禁控制器

        /// <summary>
        /// 搜索门禁控制器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_SearchDoorControlCompleted(object sender, DoorControlWebService.SearchDoorControlCompletedEventArgs e)
        {
            try
            {
                //数据源绑定
                this.cmb.ItemsSource = e.Result.DoorControlInfoItemList;

                //刷新所有门禁
                this.RefleshAllDoorControl();
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

        #endregion

        #region 激活门禁服务完成事件

        /// <summary>
        /// 激活门禁服务完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_RunOneMjWorkCompleted(object sender, DoorControlWebService.RunOneMjWorkCompletedEventArgs e)
        {
            try
            {
                if (e.Result)
                {
                    this.txtTip.Text = "连接成功";
                    //固定时刻进行提示清空
                    this.ClearTipInfo(2, new Action(() =>
                        {
                            this.txtTip.Text = "正在运行";
                        }));
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

        #region 固定时刻进行提示清空

        /// <summary>
        /// 固定时刻进行提示清空
        /// </summary>
        public void ClearTipInfo(int second, Action callBack)
        {
            try
            {
                //定时清除提示信息
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(second);
                timer.Tick += (object sender, EventArgs e) =>
                {
                    try
                    {
                        //清空提示
                        this.txtTip.Text = string.Empty;
                        //停止计时器
                        (sender as DispatcherTimer).Stop();
                        if (callBack != null)
                        {
                            //执行回调
                            callBack();
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManage.WriteLog(this.GetType(), ex);
                    }
                    finally
                    {

                    }
                };
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

        #region 刷新指定门禁

        /// <summary>
        /// 刷新指定门禁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.cmb.SelectedIndex >= 0)
                {
                    //获取当前选择的门禁控制器
                    DoorControlInfoItem selectedItem = this.cmb.SelectedItem as DoorControlInfoItem;

                    //时间同步
                    client.SetTimeAsync(selectedItem.ControlIP, selectedItem.ControlPort);
                    //时区同步
                    client.SetTimeZoneAsync(selectedItem.ControlIP, selectedItem.ControlPort, 1);
                    //取消强制
                    client.CanclForceDoorAsync(selectedItem.ControlIP, selectedItem.ControlPort, 1);

                    #region 设置门禁参数

                    //实例化门禁参数
                    MjParam mjParam = new MjParam()
                    {
                        DoorOpenAlertTime = "1",
                        DoorOpenTime = "1",
                        DoorOpenType = "0",
                        DuoChongShenFen = "2",
                        DoorOpenOutTimeAlert = "0",
                        DoorNoResoneOpenAlert = "0",
                        DoorHuSuoFlag = "0",
                        DoorOpenOrCloseFlag = "0",
                        DoorButtonTimeStoneFlag = "0",
                        CardAndPwdTimeStoneFlag = "0",
                        DuoChongFlag = "0",
                        ReaderModel = "1",

                    };
                    //设置门禁参数
                    client.SetMjParamAsync(selectedItem.ControlIP, selectedItem.ControlPort, 1, mjParam);

                    #endregion
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

        #region 取消强制

        /// <summary>
        /// 取消强制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_CanclForceDoorCompleted(object sender, CanclForceDoorCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Successed)
                {
                    this.txtTip.Text = "取消强制成功";
                    //清除提示
                    this.ClearTipInfo(2, null);
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

        #region 同步时间

        /// <summary>
        /// 同步时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_SetTimeCompleted(object sender, SetTimeCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Successed)
                {
                    this.txtTip.Text = "时间同步成功";
                    //清除提示
                    this.ClearTipInfo(2, null);
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

        #region 同步时区

        /// <summary>
        /// 同步时区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_SetTimeZoneCompleted(object sender, SetTimeZoneCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Successed)
                {
                    this.txtTip.Text = "时区同步成功";
                    //清除提示
                    this.ClearTipInfo(2, null);
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

        #endregion

        #region 刷新所有门禁

        /// <summary>
        /// 刷新所有门禁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //刷新所有门禁
                this.RefleshAllDoorControl();
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
        /// 刷新所有门禁
        /// </summary>
        public void RefleshAllDoorControl()
        {
            try
            {
                //遍历所有控制器
                foreach (var child in this.cmb.Items)
                {
                    if (child is DoorControlInfoItem)
                    {
                        //类型转换
                        var item = child as DoorControlInfoItem;
                        //时间同步
                        client.SetTimeAsync(item.ControlIP, item.ControlPort);
                        //时区同步
                        client.SetTimeZoneAsync(item.ControlIP, item.ControlPort, 1);
                        //取消强制
                        client.CanclForceDoorAsync(item.ControlIP, item.ControlPort, 1);

                        #region 设置门禁参数
                        
                        //实例化门禁参数
                        MjParam mjParam = new MjParam()
                        {
                            DoorOpenAlertTime = "1",
                            DoorOpenTime = "1",
                            DoorOpenType = "0",
                            DuoChongShenFen = "2",
                            DoorOpenOutTimeAlert = "0",
                            DoorNoResoneOpenAlert = "0",
                            DoorHuSuoFlag = "0",
                            DoorOpenOrCloseFlag = "0",
                            DoorButtonTimeStoneFlag = "0",
                            CardAndPwdTimeStoneFlag = "0",
                            DuoChongFlag = "0",
                            ReaderModel = "1",

                        };
                        //设置门禁参数
                        client.SetMjParamAsync(item.ControlIP, item.ControlPort, 1, mjParam);

                        #endregion
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
