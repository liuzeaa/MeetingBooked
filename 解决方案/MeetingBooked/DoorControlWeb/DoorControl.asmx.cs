/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： DoorControlWeb
文件功能描述： 门禁服务
创建标识： 
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/
using DoorControlWeb.Common;
using DoorControlWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace DoorControlWeb
{
    /// <summary>
    /// Service1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。
    // [System.Web.Script.Services.ScriptService]
    public partial class DoorControl : System.Web.Services.WebService
    {
        #region 内部使用

        #region 调用门禁基本动态链接库（C++编写）

        /// <summary>
        /// 获取线程数量【？？】
        /// </summary>
        /// <returns></returns>
        [DllImport("ControlComm.dll")]
        public static extern int GetCommThreadCount();

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="strNewControlPassword">门禁新密码</param>
        /// <returns></returns>

        [DllImport("ControlComm.dll")]
        public static extern int UpdateControlPassword(string strControlName, string strNewControlPassword);

        /// <summary>
        /// 更改卡号
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="strCardNum">门禁卡号</param>
        /// <param name="strPassword">门禁密码</param>
        /// <returns></returns>
        [DllImport("ControlComm.dll")]
        public static extern int UpdateControlMainCard(string strControlName, string strCardNum, string strPassword);

        /// <summary>
        /// 设置门禁时间
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <returns>返回值</returns>
        [DllImport("ControlComm.dll")]
        public static extern int SendMjClock(string strControlName);

        /// <summary>
        /// 获取门禁时间
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="strClock">具体时间值</param>
        /// <returns></returns>
        [DllImport("ControlComm.dll", CharSet = CharSet.Ansi)]
        public static extern int RecvMjClock(string strControlName, StringBuilder strClock);

        /// <summary>
        /// 发送门禁参数
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <param name="strMjParam">门禁参数</param>
        /// <returns></returns>
        [DllImport("ControlComm.dll")]
        public static extern int SendMjParam(string strControlName, int nDoorIndex, string strMjParam);

        /// <summary>
        /// 门禁远程发指令【开关门、取消强制等】
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="nType">指令类型</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <returns>返回值</returns>
        [DllImport("ControlComm.dll")]
        public static extern int SendMjRemote(string strControlName, int nType, int nDoorIndex);

        /// <summary>
        /// 获取门禁状态
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <param name="chDoorStatus">门禁状态值</param>
        /// <returns>返回值</returns>
        [DllImport("ControlComm.dll")]
        public static extern int RecvDoorStatus(string strControlName, int nDoorIndex, ref byte chDoorStatus);

        /// <summary>
        /// 设置门禁时区
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <param name="strTimeZone">门禁时区</param>
        /// <returns>返回值</returns>
        [DllImport("ControlComm.dll")]
        public static extern int SendMjTimeZone(string strControlName, int nDoorIndex, string strTimeZone);

        /// <summary>
        /// 设置门禁时区
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <param name="strPreZone">门禁时区</param>
        /// <returns>返回值</returns>
        [DllImport("ControlComm.dll")]
        public static extern int SendMjPreZone(string strControlName, int nDoorIndex, string strPreZone);

        /// <summary>
        /// 设置门禁假日
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <param name="strHoliday">门禁假日</param>
        /// <param name="nClearAll">执行前是否清除</param>
        /// <returns>返回值</returns>
        [DllImport("ControlComm.dll")]
        public static extern int SendMjHoliday(string strControlName, int nDoorIndex, string strHoliday, int nClearAll);

        /// <summary>
        /// 门禁卡片授权
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="nClearAll">执行前是否清除</param>
        /// <param name="strAdd">添加卡号列表</param>
        /// <param name="strDel">移除卡号列表</param>
        /// <returns>返回值</returns>
        [DllImport("ControlComm.dll")]
        public static extern int SendMjAuthor(string strControlName, int nClearAll, string strAdd, string strDel);

        /// <summary>
        /// 采集门禁数据
        /// </summary>
        /// <param name="strControlName">门禁名称</param>
        /// <param name="strMjRecord">门禁记录</param>
        /// <param name="strMjAlarm">门禁时间</param>
        /// <returns>返回值</returns>
        [DllImport("ControlComm.dll")]
        public static extern int RecvMjEvent(string strControlName, StringBuilder strMjRecord, StringBuilder strMjAlarm);

        /// <summary>
        /// 搜索门禁控制器
        /// </summary>
        /// <param name="strControlInfo">门禁控制器信息</param>
        /// <returns>返回值</returns>
        [DllImport("ControlComm.dll")]
        public static extern int SearchTCPControl(StringBuilder strControlInfo);

        /// <summary>
        /// 更改门禁控制器tcp
        /// </summary>
        /// <param name="strMac">mac</param>
        /// <param name="strOld">旧的mac</param>
        /// <param name="strNew">新的mac</param>
        /// <returns></returns>
        [DllImport("ControlComm.dll")]
        public static extern int UpdateTCPControl(string strMac, string strOld, string strNew);

        #endregion

        #region 返回异常错误

        /// <summary>
        /// 返回异常错误
        /// </summary>
        /// <param name="ErrorCode">异常码</param>
        /// <returns>返回数据</returns>
        [WebMethod]
        static string ReturnErrorMessage(int ErrorCode)
        {
            string nRet = "成功";
            try
            {
                switch (ErrorCode)
                {
                    case 0x00:
                        nRet = "正确";
                        break;
                    case 0x01:
                        nRet = "通信协议类型错误";
                        break;
                    case 0x02:
                        nRet = "通信地址错误";
                        break;
                    case 0x03:
                        nRet = "通信端口号错误";
                        break;
                    case 0x04:
                        nRet = "数据长度错误";
                        break;
                    case 0x05:
                        nRet = "通信线程初始化错误";
                        break;
                    case 0x06:
                        nRet = "命令错误或执行失败";
                        break;
                    case 0x07:
                        nRet = "清除刷卡记录或清除报警记录错误";
                        break;
                    case 0x08:
                        nRet = "控制器没有回答";
                        break;
                    case 0x09:
                        nRet = "数据格式错误";
                        break;
                    case 0x0a:
                        nRet = "控制器密码错误";
                        break;
                    case 0x0b:
                        nRet = "通信服务器密码错误";
                        break;
                    case 0x0c:
                        nRet = "控制器名格式错误";
                        break;
                    case 0x0d:
                        nRet = "通信服务器端口号错误";
                        break;
                    case 0x0e:
                        nRet = "控制器地址错误";
                        break;
                    case 0x0f:
                        nRet = "通信线程超时无应答";
                        break;
                    case 0x10:
                        nRet = "通信服务器没有启动";
                        break;
                    case 0x11:
                        nRet = "发送到通信服务器失败";
                        break;
                    case 0x12:
                        nRet = "通信服务器超时无返回";
                        break;
                    case 0x13:
                        nRet = "通信服务器返回格式错误";
                        break;
                    case 0x14:
                        nRet = "入口参数格式错误";
                        break;
                    case 0x15:
                        nRet = "不支持此功能";
                        break;
                    default:
                        nRet = "正确";
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return nRet;
        }

        #endregion

        #region 获取星期

        /// <summary>
        /// 获取星期
        /// </summary>
        /// <param name="WeekDay"></param>
        /// <returns></returns>
        [WebMethod]
        static string GetWeek(string WeekDay)
        {
            string nRet = null;
            try
            {
                switch (WeekDay)
                {
                    case "1":
                        nRet = "星期一";
                        break;
                    case "2":
                        nRet = "星期二";
                        break;
                    case "3":
                        nRet = "星期三";
                        break;
                    case "4":
                        nRet = "星期四";
                        break;
                    case "5":
                        nRet = "星期五";
                        break;
                    case "6":
                        nRet = "星期六";
                        break;
                    case "7":
                        nRet = "星期天";
                        break;
                    default:
                        nRet = "星期天";
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return nRet;
        }

        #endregion

        #region 返回刷卡异常记录

        /// <summary>
        /// 返回刷卡异常记录
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [WebMethod]
        static string ReturnShuakaCode(string Code)
        {
            string nRet = null;
            try
            {
                switch (Code)
                {
                    case "00":
                        nRet = "有效刷卡";
                        break;
                    case "01":
                        nRet = "无效卡号";
                        break;
                    case "02":
                        nRet = "无效时区";
                        break;
                    case "03":
                        nRet = "假日管制";
                        break;
                    case "04":
                        nRet = "无效密码";
                        break;
                    case "05":
                        nRet = "有效密码";
                        break;
                    case "06":
                        nRet = "未知错误";
                        break;
                    case "07":
                        nRet = "潜回尝试";
                        break;
                    case "08":
                        nRet = "远程开门";
                        break;
                    case "09":
                        nRet = "按钮开门";
                        break;
                    case "10":
                        nRet = "DCU 掉电";
                        break;
                    case "11":
                        nRet = "有效期已过";
                        break;
                    case "12":
                        nRet = "无当前权限";
                        break;
                    case "13":
                        nRet = "强制开门";
                        break;
                    case "14":
                        nRet = "强制关门";
                        break;
                    case "15":
                        nRet = "取消强制";
                        break;
                    case "16":
                        nRet = "黑名单管制";
                        break;
                    case "17":
                        nRet = "潜回管制";
                        break;
                    case "18":
                        nRet = "互锁管制";
                        break;
                    case "19":
                        nRet = "胁迫开门";
                        break;
                    default:
                        nRet = "未知状态";
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return nRet;
        }

        #endregion

        #region 返回警报信息

        /// <summary>
        /// 返回警报信息
        /// </summary>
        /// <param name="AlarmCode"></param>
        /// <returns></returns>
        [WebMethod]
        static string ReturnAlarm(string AlarmCode)
        {
            string nRet = null;
            try
            {
                switch (AlarmCode)
                {
                    case "00":
                        nRet = "门无故开启报警";
                        break;
                    case "01":
                        nRet = "门开启超时报警";
                        break;
                    case "02":
                        nRet = "防撬报警";
                        break;
                    case "03":
                        nRet = "红外报警";
                        break;
                    case "04":
                        nRet = "门无故关闭报警";
                        break;
                    case "05":
                        nRet = "DCU 内部错误报警";
                        break;
                    case "16":
                        nRet = "消防联动报警";
                        break;
                    case "17":
                        nRet = "巡查异常报警";
                        break;
                    default:
                        nRet = "未定义报警信息";
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return nRet;
        }

        #endregion

        #region 通过卡号或密码返回特殊信息

        /// <summary>
        /// 通过卡号或密码返回特殊信息
        /// </summary>
        /// <param name="KahaoOrMima"></param>
        /// <returns></returns>
        [WebMethod]
        static string ReturnTeshu(string KahaoOrMima)
        {
            string nRet = null;
            try
            {
                switch (KahaoOrMima)
                {
                    case "0099999999":
                        nRet = "强制开门";
                        break;
                    case "0099999998":
                        nRet = "强制关门";
                        break;
                    case "0099999997":
                        nRet = "按钮开门";
                        break;
                    case "0099999996":
                        nRet = "强制取消";
                        break;
                    case "0099999987":
                        nRet = "远程开门";
                        break;
                    case "0099999985":
                        nRet = "胁迫开门";
                        break;
                    default:
                        nRet = KahaoOrMima;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return nRet;
        }

        #endregion

        #region 获取控制器名称

        /// <summary>
        /// 获取控制器名称
        /// </summary>
        /// <param name="ConCtrlName"></param>
        /// <returns></returns>
        [WebMethod]
        static string GetStrContrlName(DoorCtrlName ConCtrlName)
        {
            //控制器名称
            StringBuilder strTemp = new StringBuilder();
            try
            {
                //服务密码
                ConCtrlName.ServerPwd = string.Empty;
                //服务地址
                ConCtrlName.ServerAddress = string.Empty;
                //服务端口号
                ConCtrlName.ServerPort = "0";
                //门禁密码
                if (string.IsNullOrEmpty(ConCtrlName.DoorPwd))
                {
                    ConCtrlName.DoorPwd = "0000000000";
                }
                //退出模式
                if (string.IsNullOrEmpty(ConCtrlName.ExitMode))
                {
                    ConCtrlName.ExitMode = "0";
                }
                //服务密码
                strTemp.Append(ConCtrlName.ServerPwd + (char)0x0d);
                //服务地址
                strTemp.Append(ConCtrlName.ServerAddress + (char)0x0d);
                //服务端口号
                strTemp.Append(ConCtrlName.ServerPort + (char)0x0d);
                //通讯地址
                strTemp.Append(ConCtrlName.CommAddress + (char)0x0d);
                //通讯端口号
                strTemp.Append(ConCtrlName.CommPort + (char)0x0d);
                //门禁地址
                strTemp.Append(ConCtrlName.DoorAddress + (char)0x0d);
                //门禁类型
                strTemp.Append(ConCtrlName.DoorType + (char)0x0d);
                //门禁密码
                strTemp.Append(ConCtrlName.DoorPwd + (char)0x0d);
                //退出模式
                strTemp.Append(ConCtrlName.ExitMode + (char)0x0d);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return strTemp.ToString();
        }

        #endregion

        #endregion

        #region 搜索门禁控制器

        /// <summary>
        /// 搜索门禁控制器
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DoorControlInfo SearchDoorControl()
        {
            //返回信息
            DoorControlInfo doorControlInfo = new DoorControlInfo();
            try
            {
                //信息收集   
                StringBuilder strControlInfo = new StringBuilder(32768);
                //控制器
                string[] CtrlMachine;
                //控制器信息
                string[] ContrlInfo;
                //临时信息收集
                string Info = string.Empty;
                int Ret;
                //搜索门禁控制器
                Ret = DoorControl.SearchTCPControl(strControlInfo);
                if (Ret == 0)
                {
                    //数据处理
                    CtrlMachine = strControlInfo.ToString().Split('\n');
                    for (int i = 0; i < CtrlMachine.Length - 1; i++)
                    {
                        DoorControlInfoItem item = new DoorControlInfoItem();
                        ContrlInfo = CtrlMachine[i].Split('\r');
                        //控制器地址
                        item.Address = ContrlInfo[0];
                        //MAC地址
                        item.Mac = ContrlInfo[1];
                        //IP地址
                        item.ControlIP = ContrlInfo[2];
                        //端口号
                        item.ControlPort = ContrlInfo[3];
                        //序列号
                        item.SearialNumber = ContrlInfo[4];
                        //控制器类型
                        item.Type = ContrlInfo[5];
                        //添加单条控制器信息
                        doorControlInfo.DoorControlInfoItemList.Add(item);

                        #region old solution(信息展示)

                        //Info = "控制器地址：" + ContrlInfo[0] + "，MAC地址：" + ContrlInfo[1] + ",IP地址：" + ContrlInfo[2] + "，端口号：" + ContrlInfo[3] + "，序列号：" + ContrlInfo[4] + "，控制器类型：" + ContrlInfo[5] + "\r\n";

                        #endregion

                        //执行返回标示
                        doorControlInfo.Successed = true;
                    }
                }
                else
                {
                    //异常信息
                    doorControlInfo.InnerError = "没有搜索到TCP/IP控制器";
                    //执行返回标示
                    doorControlInfo.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                doorControlInfo.ErrMessage = "调用服务发生异常";
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return doorControlInfo;
        }

        #endregion

        #region 设置门禁时间

        /// <summary>
        /// 设置门禁时间
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase SetTime(string DoorControlIp, string DoorControlPort)
        {
            //接口调用返回信息
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));
            try
            {
                //门禁时间设置
                int Ret = SendMjClock(strControlName);
                if (Ret == 0)
                {
                    //执行返回标示
                    returnDataBase.Successed = true;
                }
                else
                {
                    //执行异常信息
                    returnDataBase.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    returnDataBase.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "设置时间失败，原因：" + ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 获取门禁时间

        /// <summary>
        /// 获取门禁时间
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase GetTime(string DoorControlIp, string DoorControlPort)
        {
            //接口调用返回信息
            DoorControlStateEntity returnDataBase = new DoorControlStateEntity();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            //时间信息
            StringBuilder strClock = new StringBuilder();

            //门禁时间
            string timer = string.Empty;
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));

            try
            {
                ///远程获取门禁时间
                int Ret = RecvMjClock(strControlName, strClock);

                //时间格式转换
                if (Ret == 0)
                {
                    timer = strClock.ToString();
                    timer = timer.Substring(0, 4) + "年" + timer.Substring(4, 2) + "月" + timer.Substring(6, 2)
                        + "日" + timer.Substring(8, 2) + "时" + timer.Substring(10, 2) + "分" + timer.Substring(12, 2)
                        + "秒" + GetWeek(timer.Substring(14, 1));
                    //获取门禁时间
                    returnDataBase.CtrlTime = timer;
                    //执行返回标示
                    returnDataBase.Successed = true;
                }
                else
                {
                    //执行异常信息
                    returnDataBase.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    returnDataBase.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "设置时间失败，原因：" + ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {
            }
            return returnDataBase;
        }
        [WebMethod]
        public string GetTime2(string DoorControlIp, string DoorControlPort)
        {
            string flg = "false";
            //接口调用返回信息
            DoorControlStateEntity returnDataBase = new DoorControlStateEntity();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return "false";
            }
            //时间信息
            StringBuilder strClock = new StringBuilder();

            //门禁时间
            string timer = string.Empty;
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));

            try
            {
                ///远程获取门禁时间
                int Ret = RecvMjClock(strControlName, strClock);

                //时间格式转换
                if (Ret == 0)
                {
                    timer = strClock.ToString();
                    timer = timer.Substring(0, 4) + "年" + timer.Substring(4, 2) + "月" + timer.Substring(6, 2)
                        + "日" + timer.Substring(8, 2) + "时" + timer.Substring(10, 2) + "分" + timer.Substring(12, 2)
                        + "秒" + GetWeek(timer.Substring(14, 1));
                    //获取门禁时间
                    flg=timer;
                   
                }
                else
                {
                    flg = "false";
                }
            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "设置时间失败，原因：" + ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {
            }
            return flg;
        }
        #endregion

        #region 设置门禁参数

        /// <summary>
        /// 设置门禁参数
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <param name="mjParam">门禁参数</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase SetMjParam(string DoorControlIp, string DoorControlPort, int nDoorIndex, MjParam mjParam)
        {
          
            //门禁实体
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }

            //门禁参数           
            string strMjParam;
            StringBuilder strTempMjParam = new StringBuilder();
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));
            //门禁开启时间
            if (string.IsNullOrEmpty(mjParam.DoorOpenTime))
            {
                mjParam.DoorOpenTime = "5";
            }
            //收集参数
            strTempMjParam.Append(mjParam.DoorOpenTime + (char)0x0d);

            //门禁弹出时间
            if (string.IsNullOrEmpty(mjParam.DoorOpenAlertTime)) mjParam.DoorOpenAlertTime = "5";
            strTempMjParam.Append(mjParam.DoorOpenAlertTime + (char)0x0d);
            //门禁开启类型
            if (string.IsNullOrEmpty(mjParam.DoorOpenType)) mjParam.DoorOpenType = "0";
            strTempMjParam.Append(mjParam.DoorOpenType + (char)0x0d);
            //多重身份
            if (string.IsNullOrEmpty(mjParam.DuoChongShenFen)) mjParam.DuoChongShenFen = "2";
            strTempMjParam.Append(mjParam.DuoChongShenFen + (char)0x0d);
            //门禁开启超时
            if (string.IsNullOrEmpty(mjParam.DoorOpenOutTimeAlert)) mjParam.DoorOpenOutTimeAlert = "0";
            strTempMjParam.Append(mjParam.DoorOpenOutTimeAlert + (char)0x0d);
            //门禁未响应自动开启
            if (string.IsNullOrEmpty(mjParam.DoorNoResoneOpenAlert)) mjParam.DoorNoResoneOpenAlert = "0";
            //门禁未响应自动开启
            strTempMjParam.Append(mjParam.DoorNoResoneOpenAlert + (char)0x0d);

            if (string.IsNullOrEmpty(mjParam.DoorHuSuoFlag)) mjParam.DoorHuSuoFlag = "0";
            strTempMjParam.Append(mjParam.DoorHuSuoFlag + (char)0x0d);

            //门禁开启关闭标示
            if (string.IsNullOrEmpty(mjParam.DoorOpenOrCloseFlag)) mjParam.DoorOpenOrCloseFlag = "0";
            strTempMjParam.Append(mjParam.DoorOpenOrCloseFlag + (char)0x0d);

            if (string.IsNullOrEmpty(mjParam.DoorButtonTimeStoneFlag)) mjParam.DoorButtonTimeStoneFlag = "0";
            strTempMjParam.Append(mjParam.DoorButtonTimeStoneFlag + (char)0x0d);

            if (string.IsNullOrEmpty(mjParam.CardAndPwdTimeStoneFlag)) mjParam.CardAndPwdTimeStoneFlag = "0";
            strTempMjParam.Append(mjParam.CardAndPwdTimeStoneFlag + (char)0x0d);
            //多重标示
            if (string.IsNullOrEmpty(mjParam.DuoChongFlag)) mjParam.DuoChongFlag = "0";
            strTempMjParam.Append(mjParam.DuoChongFlag + (char)0x0d);
            //读取模式
            if (string.IsNullOrEmpty(mjParam.ReaderModel)) mjParam.ReaderModel = "1";
            strTempMjParam.Append(mjParam.ReaderModel + (char)0x0d);
            //门禁参数
            strMjParam = strTempMjParam.ToString();

            try
            {
                //发送门禁参数
                int Ret = SendMjParam(strControlName, nDoorIndex, strMjParam);
                if (Ret == 0)
                {
                    //执行返回标示
                    returnDataBase.Successed = true;
                }
                else
                {
                    //执行异常信息
                    returnDataBase.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    returnDataBase.Successed = false;
                }
            }
            catch (AccessViolationException ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "设置门禁参数失败，原因：控制器不存在或无法连接";
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "设置门禁参数失败，原因：" + ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 门禁开关门

        #region 强制开门

        /// <summary>
        /// 强制开门
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase ForceOpenDoor(string DoorControlIp, string DoorControlPort, int nDoorIndex)
        {
            //接口调用返回信息
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            try
            {
                //执行开关门通用函数
                returnDataBase = DoorControl.ExcuteMj_Open_Close(this.CreateDoorCtrl(DoorControlIp, DoorControlPort), 0x01, 1);

                if (!string.IsNullOrEmpty(returnDataBase.ErrMessage))
                {
                    //异常信息
                    returnDataBase.ErrMessage = "强制开门失败,原因：" + returnDataBase.ErrMessage;
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 强制关门

        /// <summary>
        /// 强制关门
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase ForceCloseDoor(string DoorControlIp, string DoorControlPort, int nDoorIndex)
        {
            //接口调用返回信息
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            try
            {
                //执行开关门通用函数
                returnDataBase = DoorControl.ExcuteMj_Open_Close(this.CreateDoorCtrl(DoorControlIp, DoorControlPort), 0x02, 1);

                if (!string.IsNullOrEmpty(returnDataBase.ErrMessage))
                {
                    //异常信息
                    returnDataBase.ErrMessage = "强制关门失败,原因：" + returnDataBase.ErrMessage;
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 开门

        /// <summary>
        /// 开门
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase OpenDoor(string DoorControlIp, string DoorControlPort, int nDoorIndex)
        {
            //接口调用返回信息
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            try
            {
                //执行开关门通用函数
                returnDataBase = DoorControl.ExcuteMj_Open_Close(this.CreateDoorCtrl(DoorControlIp, DoorControlPort), 0x00, 1);

                if (!string.IsNullOrEmpty(returnDataBase.ErrMessage))
                {
                    //异常信息
                    returnDataBase.ErrMessage = "关门失败,原因：" + returnDataBase.ErrMessage;
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 取消强制

        /// <summary>
        /// 取消强制开门
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase CanclForceDoor(string DoorControlIp, string DoorControlPort, int nDoorIndex)
        {
            //接口调用返回信息
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            try
            {
                //执行开关门通用函数
                returnDataBase = DoorControl.ExcuteMj_Open_Close(this.CreateDoorCtrl(DoorControlIp, DoorControlPort), 0x03, 1);

                if (!string.IsNullOrEmpty(returnDataBase.ErrMessage))
                {
                    //异常信息
                    returnDataBase.ErrMessage = "取消强制,原因：" + returnDataBase.ErrMessage;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 门禁开关门（辅助）

        /// <summary>
        /// 开关门通用函数
        /// </summary>
        /// <param name="ctlName">门禁名称</param>
        /// <param name="nType">命令类型</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <returns>返回值</returns>
        public static ReturnDataBase ExcuteMj_Open_Close(DoorCtrlName ctlName, int nType, int nDoorIndex)
        {
            ReturnDataBase returnDataBase = new ReturnDataBase();

            //获取门禁控制器名称
            string strControlName = GetStrContrlName(ctlName);

            try
            {
                //门禁发送指令返回参数
                int Ret = SendMjRemote(strControlName, nType, nDoorIndex);
                if (Ret == 0)
                {
                    //执行返回标示
                    returnDataBase.Successed = true;
                }
                else
                {
                    //通过返回参数对应相应的参数
                    returnDataBase.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    returnDataBase.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #endregion

        #region 查询门禁状态

        /// <summary>
        /// 查询门禁状态
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public DoorControlStateEntity QueryDoorStatus(string DoorControlIp, string DoorControlPort, int nDoorIndex)
        {
            //接口调用返回信息
            DoorControlStateEntity doorControlEntity = new DoorControlStateEntity();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                doorControlEntity.ErrMessage = "该门禁不可访问";
                return doorControlEntity;
            }
            //门禁状态（字节数字）
            byte chDoorStatus = 0;
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));

            try
            {
                //获取门禁状态
                int Ret = RecvDoorStatus(strControlName, nDoorIndex, ref chDoorStatus);
                if (Ret == 0)
                {
                    //门禁状态数据加工
                    string StatusCode = Convert.ToString(chDoorStatus, 2);
                    StatusCode = Int64.Parse(StatusCode).ToString("D8");
                    //门禁状态执行流程
                    switch (StatusCode.Substring(5, 2))
                    {
                        case "00":
                            doorControlEntity.ForceStatus = "无强制";
                            break;
                        case "01":
                            doorControlEntity.ForceStatus = "强制开门";
                            break;
                        case "10":
                            doorControlEntity.ForceStatus = "强制关门";
                            break;
                        default:
                            doorControlEntity.ForceStatus = "无强制";
                            break;
                    }

                    //获取门状态
                    doorControlEntity.DoorStatus = (StatusCode.Substring(7, 1) == "0" ? "门关" : "门开");
                    //执行返回标示
                    doorControlEntity.Successed = true;
                }
                else
                {
                    //执行异常信息
                    doorControlEntity.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    doorControlEntity.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                doorControlEntity.ErrMessage = "查询门状态失败，原因：" + ex.Message;
                //执行返回标示
                doorControlEntity.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return doorControlEntity;
        }

        #endregion

        #region 设置时区

        /// <summary>
        /// 设置时区
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase SetTimeZone(string DoorControlIp, string DoorControlPort, int nDoorIndex)
        {
            //接口调用返回信息
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            //时区临时信息存储
            StringBuilder strTimeZoneTemp = new StringBuilder();
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));

            #region 门禁数据处理

            //设置时区（可以提出作为时区参数）
            string[, ,] CtrlTimeZone = new string[19, 7, 4];

            for (int i = 0; i <= 18; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        CtrlTimeZone[i, j, k] = "";
                    }
                }
            }

            for (int i = 0; i <= 18; i++)
            {
                if (i < 15 || i == 16)
                {
                    for (int j = 0; j <= 6; j++)
                    {
                        strTimeZoneTemp.Append(i.ToString("D2") + j.ToString());
                        strTimeZoneTemp.Append(String.IsNullOrEmpty(CtrlTimeZone[i, j, 0]) ? "00002359" : CtrlTimeZone[i, j, 0]);
                        strTimeZoneTemp.Append(String.IsNullOrEmpty(CtrlTimeZone[i, j, 1]) ? "00000000" : CtrlTimeZone[i, j, 1]);
                        strTimeZoneTemp.Append(String.IsNullOrEmpty(CtrlTimeZone[i, j, 2]) ? "00000000" : CtrlTimeZone[i, j, 2]);
                        strTimeZoneTemp.Append(String.IsNullOrEmpty(CtrlTimeZone[i, j, 3]) ? "00000000" : CtrlTimeZone[i, j, 3]);
                    }
                }
                else
                {
                    for (int j = 0; j <= 6; j++)
                    {
                        strTimeZoneTemp.Append(i.ToString("D2") + j.ToString());
                        strTimeZoneTemp.Append(String.IsNullOrEmpty(CtrlTimeZone[i, j, 0]) ? "00000000" : CtrlTimeZone[i, j, 0]);
                        strTimeZoneTemp.Append(String.IsNullOrEmpty(CtrlTimeZone[i, j, 1]) ? "00000000" : CtrlTimeZone[i, j, 1]);
                        strTimeZoneTemp.Append(String.IsNullOrEmpty(CtrlTimeZone[i, j, 2]) ? "00000000" : CtrlTimeZone[i, j, 2]);
                        strTimeZoneTemp.Append(String.IsNullOrEmpty(CtrlTimeZone[i, j, 3]) ? "00000000" : CtrlTimeZone[i, j, 3]);
                    }

                }
            }

            #endregion

            //时区
            string strTimeZone = strTimeZoneTemp.ToString();

            try
            {
                //设置门禁时区
                int Ret = SendMjTimeZone(strControlName, nDoorIndex, strTimeZone);
                if (Ret == 0)
                {
                    //执行返回标示
                    returnDataBase.Successed = true;
                }
                else
                {
                    //执行异常信息
                    returnDataBase.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    returnDataBase.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "下发时区信息失败，原因：" + ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 设置时区（未知）

        /// <summary>
        /// 设置时区（未知）
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <param name="PreTimeZone">门禁时区</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase SetPreTimeZone(string DoorControlIp, string DoorControlPort, int nDoorIndex, string[] PreTimeZone)
        {
            //接口调用返回信息
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            //时区临时信息存储
            StringBuilder strPreTimeZoneTemp = new StringBuilder();
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));

            #region 门禁时区数据处理

            strPreTimeZoneTemp.Append(String.IsNullOrEmpty(PreTimeZone[0]) ? "200001010000200001010000" : PreTimeZone[0]);
            strPreTimeZoneTemp.Append(String.IsNullOrEmpty(PreTimeZone[1]) ? "200001010000200001010000" : PreTimeZone[1]);
            strPreTimeZoneTemp.Append(String.IsNullOrEmpty(PreTimeZone[2]) ? "200001010000200001010000" : PreTimeZone[2]);
            strPreTimeZoneTemp.Append(String.IsNullOrEmpty(PreTimeZone[3]) ? "200001010000200001010000" : PreTimeZone[3]);
            //时区的信息
            string strPreTimeZone = strPreTimeZoneTemp.ToString();

            #endregion

            try
            {
                //设置门禁时区
                int Ret = SendMjPreZone(strControlName, nDoorIndex, strPreTimeZone);
                if (Ret == 0)
                {
                    //执行返回标示
                    returnDataBase.Successed = true;
                }
                else
                {
                    //执行异常信息
                    returnDataBase.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    returnDataBase.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "下发预定时区信息失败，原因：" + ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 设置门禁假日（处于这个状态,门禁一直处于关闭状态）

        /// <summary>
        /// 设置门禁待机
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <param name="strHoliday">假日信息</param>
        /// <param name="nClearAll">是否清空</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase SetMjHoliday(string DoorControlIp, string DoorControlPort, int nDoorIndex, string strHoliday, int nClearAll)
        {
            //接口调用返回信息
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));

            try
            {
                //设置门禁假日
                int Ret = SendMjHoliday(strControlName, nDoorIndex, strHoliday, nClearAll);
                if (Ret == 0)
                {
                    //执行返回标示
                    returnDataBase.Successed = true;
                }
                else
                {
                    //执行异常信息
                    returnDataBase.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    returnDataBase.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "下发节假日信息失败，原因：" + ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 发送卡片授权

        /// <summary>
        /// 发送卡片授权
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nClearAll">执行前是否清空</param>
        /// <param name="cardArray">卡片授权名单</param>
        /// <param name="strDel">卡片移除名单</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public ReturnDataBase SetMjAuthor(string DoorControlIp, string DoorControlPort, int nClearAll, string[] cardArray, string strDel)
        {
            //接口调用返回信息
            ReturnDataBase returnDataBase = new ReturnDataBase();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            //用于存储卡片授权名单
            string strAdd;
            if (cardArray != null)
            {
                //授权人卡片列表
                CardAuthor[] cardAutor = new CardAuthor[cardArray.Length];
                //设置卡片默认设置
                for (int i = 0; i < cardAutor.Length; i++)
                {
                    cardAutor[i].Kahao = Int64.Parse(cardArray[i]).ToString("D10");
                    cardAutor[i].Mima = cardAutor[i].Kahao.Substring(2, 8);
                    cardAutor[i].YouxiaoRiqi = "20391231";
                    cardAutor[i].YunxuTongGuomen = "1111";
                    cardAutor[i].ZhuFuKa = "0000";
                    cardAutor[i].TongxingShiqu = "00000000";
                    cardAutor[i].FanQianhui = "0000";
                    cardAutor[i].JiariGuanzhi = "0000";
                }
                //收集卡片的参数
                StringBuilder strAddTemp = new StringBuilder();

                #region 门禁卡号默认参数收集

                for (int i = 0; i < cardAutor.Length; i++)
                {
                    strAddTemp.Append(Int64.Parse(cardAutor[i].Kahao).ToString("D10"));
                    if (cardAutor[i].Mima.Length > 8) cardAutor[i].Mima = cardAutor[i].Mima.Substring(0, 8);
                    while (cardAutor[i].Mima.Length != 8)
                    {
                        cardAutor[i].Mima = cardAutor[i].Mima + "F";
                    }

                    strAddTemp.Append(cardAutor[i].Mima);
                    strAddTemp.Append(cardAutor[i].YouxiaoRiqi);
                    strAddTemp.Append(cardAutor[i].YunxuTongGuomen);
                    strAddTemp.Append(cardAutor[i].ZhuFuKa);
                    strAddTemp.Append(cardAutor[i].TongxingShiqu);
                    strAddTemp.Append(cardAutor[i].FanQianhui);
                    strAddTemp.Append(cardAutor[i].JiariGuanzhi);
                }
                 strAdd = strAddTemp.ToString();
            }
            else
            {
                strAdd = null;
            }
            #endregion
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));
            try
            {
               
                //门禁卡片授权
                int Ret = SendMjAuthor(strControlName, nClearAll, strAdd, strDel);

                //阻塞当前写入 控制器 半分钟 的时间；
                Thread.SpinWait(5000);

                //再执行一次
                Ret = SendMjAuthor(strControlName, nClearAll, strAdd, strDel); 


                if (Ret == 0)
                {
                    //执行返回标示
                    returnDataBase.Successed = true;
                }
                else
                {
                    //执行异常信息
                    returnDataBase.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    returnDataBase.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "下发卡片授权时失败，原因：" + ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 清空门禁白名单

        /// <summary>
        /// 清空门禁白名单
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public DoorControlCollectData ClearMJAuthor(string DoorControlIp, string DoorControlPort)
        {
            //接口调用返回信息
            DoorControlCollectData returnDataBase = new DoorControlCollectData();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                returnDataBase.ErrMessage = "该门禁不可访问";
                return returnDataBase;
            }
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));
            try
            {
                //门禁卡片授权
                returnDataBase = GetData(DoorControlIp, DoorControlPort);

                //清空授权
                int Ret = SendMjAuthor(strControlName, 1, String.Empty, string.Empty);

            }
            catch (Exception ex)
            {
                //异常信息
                returnDataBase.ErrMessage = "下发卡片授权时失败，原因：" + ex.Message;
                //执行返回标示
                returnDataBase.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return returnDataBase;
        }

        #endregion

        #region 采集门禁数据

        /// <summary>
        /// 采集门禁数据
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <returns>返回值</returns>
        [WebMethod]
        public DoorControlCollectData GetData(string DoorControlIp, string DoorControlPort)
        {
            //接口调用返回信息
            DoorControlCollectData doorControlCollectData = new DoorControlCollectData();
            //测试门禁IP是否可访问
            if (!IPCheck.TestNetConnectity(DoorControlIp))
            {
                doorControlCollectData.ErrMessage = "该门禁不可访问";
                return doorControlCollectData;
            }
            //门禁记录
            string MjRecordString = null;
            //门禁警报参数
            string MjAlarmString = null;
            //门禁记录收集
            StringBuilder strMjRecord = new StringBuilder(1024);
            //门禁警报数据记录
            StringBuilder strMjAlarm = new StringBuilder(1024);
            //门禁记录和警报默认设置
            for (int i = 0; i < 1024; i++)
            {
                strMjRecord.Append((char)0x00);
                strMjAlarm.Append((char)0x00);
            }
            //获取门禁控制器名称
            string strControlName = GetStrContrlName(this.CreateDoorCtrl(DoorControlIp, DoorControlPort));

            try
            {
                //采集门禁数据
                int Ret = RecvMjEvent(strControlName, strMjRecord, strMjAlarm);
                if (Ret == 0)
                {
                    #region 门禁数据加工

                    MjRecordString = strMjRecord.ToString();
                    doorControlCollectData.MjRecordCount = (int)MjRecordString.Length / 28;
                    doorControlCollectData.MjRecord = new sMjRecord[doorControlCollectData.MjRecordCount];
                    for (int i = 0; i < doorControlCollectData.MjRecordCount; i++)
                    {
                        try
                        {
                            doorControlCollectData.MjRecord[i].KahaoOrMima = ReturnTeshu(MjRecordString.Substring(i * 28, 10));
                            doorControlCollectData.MjRecord[i].Time = MjRecordString.Substring(i * 28 + 10, 14);
                            doorControlCollectData.MjRecord[i].MenXuhao = MjRecordString.Substring(i * 28 + 24, 1);
                            doorControlCollectData.MjRecord[i].JinChu = (MjRecordString.Substring(i * 28 + 25, 1) == "0" ? "进" : "出");
                            doorControlCollectData.MjRecord[i].ShuakaBiaozhi = ReturnShuakaCode(MjRecordString.Substring(i * 28 + 26, 2));

                            if (MjRecordString.Substring(i * 28 + 26, 2) == "04" || MjRecordString.Substring(i * 28 + 26, 2) == "05")
                            {
                                doorControlCollectData.MjRecord[i].KahaoOrMima = doorControlCollectData.MjRecord[i].KahaoOrMima.Replace('F', (char)0x20).Trim();
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                    MjAlarmString = strMjAlarm.ToString();
                    doorControlCollectData.MjAlarmCount = (int)MjAlarmString.Length / 18;
                    for (int i = 0; i < doorControlCollectData.MjAlarmCount; i++)
                    {
                        doorControlCollectData.MjAlarm[i].BaoJingLeixing = ReturnAlarm(MjAlarmString.Substring(i * 18, 2));
                        doorControlCollectData.MjAlarm[i].BaoJingQizhi = (MjAlarmString.Substring(i * 18 + 2, 1) == "0" ? "报警开始" : "报警结束");
                        doorControlCollectData.MjAlarm[i].MenXuhao = MjAlarmString.Substring(i * 18 + 3, 1);
                        doorControlCollectData.MjAlarm[i].Time = MjAlarmString.Substring(i * 18 + 4, 14);
                    }
                    //执行返回标示
                    doorControlCollectData.Successed = true;

                    #endregion
                }
                else
                {
                    //执行异常信息
                    doorControlCollectData.InnerError = ReturnErrorMessage(Ret);
                    //执行返回标示
                    doorControlCollectData.Successed = false;
                }
            }
            catch (Exception ex)
            {
                //异常信息
                doorControlCollectData.ErrMessage = "获取控制器数据时失败，原因：" + ex.Message;
                //执行返回标示
                doorControlCollectData.Successed = false;
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            //恢复默认设置
            strMjRecord.Length = 0;
            strMjAlarm.Length = 0;
            strMjRecord = null;
            strMjAlarm = null;

            return doorControlCollectData;
        }

        #endregion

        #region 创建门禁实体

        /// <summary>
        /// 创建门禁实体
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <returns>返回值</returns>
        public DoorCtrlName CreateDoorCtrl(string DoorControlIP, string DoorControlPort)
        {
            //接口调用返回信息
            DoorCtrlName CtrlName = default(DoorCtrlName);
            try
            {
                //门禁实体
                CtrlName = new DoorCtrlName()
                {
                    CommAddress = DoorControlIP,
                    CommPort = DoorControlPort,
                    DoorAddress = "1",
                    DoorPwd = "0000000000",
                    DoorType = "1",
                    ExitMode = "0",
                };
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return CtrlName;
        }

        #endregion

    }
}