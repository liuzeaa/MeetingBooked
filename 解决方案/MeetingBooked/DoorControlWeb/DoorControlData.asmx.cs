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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DoorControlWeb.Entity;
using System.Timers;
using DoorControlWeb.Common;
using System.Threading;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;

namespace DoorControlWeb
{
    /// <summary>
    /// 使用多个门禁【使用该方式直接通用】
    /// </summary>  
    public partial class DoorControl : System.Web.Services.WebService
    {
        #region 内部字段

        /// <summary>
        /// 使用该计时器刷新门禁【写入白名单】
        /// </summary>
        System.Timers.Timer timer1 = null;

        /// <summary>
        /// 使用该计时器刷新门禁【删除白名单】
        /// </summary>
        System.Timers.Timer timer2 = null;

        #endregion

        #region 获取白名单数据

        /// <summary>
        /// 获取白名单数据
        /// </summary>
        [WebMethod]
        public List<DoorControlEntity> GetDoorEntityWhiteList()
        {
            //接口调用返回信息
            List<DoorControlEntity> list = null;
            try
            {
                //暂时使用文本命令
                string sql = string.Format("Select * from White_List ");
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<DoorControlEntity>(sql, System.Data.CommandType.Text, out error);
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return list;
        }

        #endregion

        #region 获取当前日期的白名单数据（开始时间加10分钟）

        /// <summary>
        /// 获取当前日期的白名单数据
        /// </summary>
        [WebMethod]
        public List<DoorControlEntity> GetNowDateDoorEntityWhiteList()
        {
            List<DoorControlEntity> list = null;
            try
            {
                //暂时使用文本命令
                string sql = string.Format("Select * from White_List Where IsWrite =0");
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<DoorControlEntity>(sql, System.Data.CommandType.Text, out error).Where(item =>
                DateTime.Now > Convert.ToDateTime(item.StartDate).AddMinutes(-10) && DateTime.Now < Convert.ToDateTime(item.EndDate)).ToList<DoorControlEntity>();
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return list;
        }

        #endregion

        #region 获取当前日期的白名单数据（筛选会议前15分钟到前10分钟的状态）

        /// <summary>
        /// 获取当前日期的白名单数据
        /// </summary>
        [WebMethod]
        public List<DoorControlEntity> GetNowDateBeforeMinitesDoorEntityWhiteList()
        {
            List<DoorControlEntity> list = null;
            try
            {
                //暂时使用文本命令
                string sql = string.Format("Select * from White_List Where IsWrite =0");
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<DoorControlEntity>(sql, System.Data.CommandType.Text, out error).Where(item =>
                DateTime.Now > Convert.ToDateTime(item.StartDate).AddMinutes(-15) && DateTime.Now < Convert.ToDateTime(item.StartDate).AddMinutes(-10)).ToList<DoorControlEntity>();
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return list;
        }

        #endregion

        #region 获取超出日期5分钟以内的白名单数据

        /// <summary>
        /// 获取超出日期5分钟以内的白名单数据
        /// </summary>
        [WebMethod]
        public List<DoorControlEntity> GetOutNowDateDoorEntityWhiteList()
        {
            //执行返回标示
            List<DoorControlEntity> list = null;
            try
            {
                //暂时使用文本命令
                string sql = string.Format("Select * from White_List Where IsWrite =1");
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<DoorControlEntity>(sql, System.Data.CommandType.Text, out error).Where(item =>
                DateTime.Now > Convert.ToDateTime(item.EndDate) && DateTime.Now < Convert.ToDateTime(item.EndDate).AddMinutes(5)).ToList<DoorControlEntity>();
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return list;
        }

        #endregion

        #region 操作一条数据（存在即更新，不存在即插入）【修改：调用ExcuteEntity方法，传递参数对象数组（引入命名空间System.Data.SqlClient）--司晓林】【增加管理员调用的方法，实现首次单卡开门】

        #region 首次三卡开门
        /// <summary>
        /// 操作一条数据（存在即更新，不存在即插入），首次开门需要刷三张卡
        /// </summary>
        /// <param name="ControlIp">门禁IP</param>
        /// <param name="ControlPort">端口号</param>
        /// <param name="StartDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="CardList">卡号列表</param>
        /// <returns></returns>
        [WebMethod]
        public bool OperateOneWhiteList(string ControlIp, string ControlPort, string StartDate, string EndDate, string CardList)
        {
            //接口调用返回信息
            bool Successed = false;
            string strError = string.Empty;
            try
            {
                string salSelect = string.Format(@"select * from White_List where ControlIP = @ControlIp and StartDate = @StartDate 
                                                    and EndDate = @EndDate");

                List<DoorControlEntity> doorControlEntityList = DBHelper.ExcuteEntity<DoorControlEntity>
                    (salSelect, CommandType.Text, out strError,
                    new SqlParameter("@ControlIP", ControlIp),
                    new SqlParameter("@StartDate", StartDate),
                    new SqlParameter("@EndDate", EndDate));
                //sql语句
                string sql = string.Empty;
                string error = string.Empty;

                if (doorControlEntityList.Count == 0)
                {
                    sql = string.Format(@"Insert into White_List(CardList,StartDate,EndDate,ControlIP,ControlPort,IsWrite,CreditCardLimit) 
                                          Values('{0}','{1}','{2}','{3}','{4}',{5},{6})", CardList, StartDate, EndDate, ControlIp, ControlPort, 0, 0);
                }
                else
                {
                    sql = string.Format(@"update White_List set CardList='{0}' where ControlIP='{1}' and StartDate='{2}' and EndDate='{3}'",
                                        CardList, ControlIp, StartDate, EndDate);
                }
                //执行Tsql操作
                DBHelper.Transaction(sql, out error);

                if (string.IsNullOrEmpty(error))
                {
                    //执行返回标示
                    Successed = true;
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
            return Successed;
        }

        #endregion

        #region 首次单卡开门

        /// <summary>
        /// 操作一条数据（存在即更新，不存在即插入），首次可单卡开门
        /// </summary>
        /// <param name="ControlIp">门禁IP</param>
        /// <param name="ControlPort">端口号</param>
        /// <param name="StartDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="CardList">卡号列表</param>
        /// <returns></returns>
        [WebMethod]
        public bool AdminOperateOneWhiteList(string ControlIp, string ControlPort, string StartDate, string EndDate, string CardList)
        {
            //接口调用返回信息
            bool Successed = false;
            string strError = string.Empty;
            try
            {
                string salSelect = string.Format(@"select * from White_List where ControlIP = @ControlIp and StartDate = @StartDate 
                                                    and EndDate = @EndDate");

                List<DoorControlEntity> doorControlEntityList = DBHelper.ExcuteEntity<DoorControlEntity>
                    (salSelect, CommandType.Text, out strError,
                    new SqlParameter("@ControlIP", ControlIp),
                    new SqlParameter("@StartDate", StartDate),
                    new SqlParameter("@EndDate", EndDate));
                //sql语句
                string sql = string.Empty;
                string error = string.Empty;

                if (doorControlEntityList.Count == 0)
                {
                    sql = string.Format(@"Insert into White_List(CardList,StartDate,EndDate,ControlIP,ControlPort,IsWrite,CreditCardLimit) 
                                          Values('{0}','{1}','{2}','{3}','{4}',{5},{6})", CardList, StartDate, EndDate, ControlIp, ControlPort, 0, Constant.CreditCardLimit);
                }
                else
                {
                    sql = string.Format(@"update White_List set CardList='{0}' where ControlIP='{1}' and StartDate='{2}' and EndDate='{3}'",
                                        CardList, ControlIp, StartDate, EndDate);
                }
                //执行Tsql操作
                DBHelper.Transaction(sql, out error);

                if (string.IsNullOrEmpty(error))
                {
                    //执行返回标示
                    Successed = true;
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
            return Successed;
        }

        #endregion

        #endregion

        #region 这个直接启用门禁服务，不需要使用windows服务即可达到预期效果

        /// <summary>
        /// 这个直接启用门禁服务，不需要使用windows服务即可达到预期效果
        /// </summary>
        [WebMethod]
        public bool RunOneMjWork()
        {
            //接口调用返回信息
            bool Successed = false;
            try
            {
                //写入
                if (timer1 == null)
                {
                    timer1 = new System.Timers.Timer();
                    timer1.Elapsed += CheckWrite_Elapsed;
                    timer1.Interval = Constant.timer_Write;
                    timer1.Start();
                }
                //移除
                if (timer2 == null)
                {
                    timer2 = new System.Timers.Timer();
                    timer2.Elapsed += CheckDelete_Elapsed;
                    timer2.Interval = Constant.timer_Delete;
                    timer2.Start();
                }
                //执行返回标示
                Successed = true;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return Successed;
        }

        /// <summary>
        /// 检查白名单并写入门禁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckWrite_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //获取前10分钟有效的门禁白名单记录
                List<DoorControlEntity> doorControlEntityList = this.GetNowDateDoorEntityWhiteList();


                if (doorControlEntityList.Count > 0)
                {
                    #region 获取刷卡记录（有效卡号）


                    //批量sql语句(阀值更新)
                    List<string> sqlList_UpdateCreditCardLimit = new List<string>();

                    //批量sql语句(授权)
                    List<string> sqlList_SetMjAuthor = new List<string>();
                    //异常信息
                    string error = string.Empty;


                    //遍历执行授权（刷卡记录）
                    foreach (var item in doorControlEntityList)
                    {
                        //签到/签退记录
                        List<SignListEntity> signListEntitys = null;

                        //获取指定门禁的刷卡记录
                        var data = this.GetData(item.ControlIP, item.ControlPort);
                        for (int i = 0; i < data.MjRecord.Count(); i++)
                        {
                            //提出每一条刷卡记录
                            var record = data.MjRecord[i];
                            //首先判断是否为当前参会人
                            if (item.CardList.Contains(record.KahaoOrMima))
                            {
                                if (item.CreditCardList == null)
                                {
                                    item.CreditCardList = string.Empty;
                                }
                                //加载刷卡记录（为保证三次刷卡开门）
                                if (!item.CreditCardList.Contains(record.KahaoOrMima))
                                {
                                    if (item.CreditCardList.Length > 0)
                                    {
                                        //保持分割符的存在
                                        item.CreditCardList += Constant.CardListSplitChar + record.KahaoOrMima;
                                        //阀值递增
                                        item.CreditCardLimit++;
                                    }
                                    else
                                    {
                                        //第一条记录（保证分割符的有效性）
                                        item.CreditCardList += record.KahaoOrMima;
                                        //阀值递增
                                        item.CreditCardLimit++;
                                    }
                                }

                            }
                        }

                        //在有刷卡记录的情况下，进行数据库的记录
                        if (data.MjRecord.Count() > 0)
                        {
                            sqlList_UpdateCreditCardLimit.Add(string.Format("update White_List Set CreditCardList ='{0}',CreditCardLimit ={1} where Guid = {2}", item.CreditCardList, item.CreditCardLimit, item.Guid));
                        }
                    }
                    //添加刷卡记录
                    if (sqlList_UpdateCreditCardLimit.Count > 0)
                    {
                        //执行数据库操作
                        DBHelper.Transaction(sqlList_UpdateCreditCardLimit, out error);
                    }

                    #endregion

                    #region 白名单列表授权并强制开门（之后取消强制开门）【20151124修改：取消三卡后安全开门的过程，定位成三卡激活，5秒后再次刷卡开门】

                    //遍历执行授权（多卡刷门）
                    foreach (var item in doorControlEntityList)
                    {
                        //多卡刷门阀值达到一定限度，进行统一授权
                        if (item.CreditCardLimit >= Constant.CreditCardLimit)
                        {
                            //门禁白名单授权【修改：不能清空已授权信息】
                            this.SetMjAuthor(item.ControlIP, item.ControlPort, 0, item.CardList.Split(new char[] { Constant.CardListSplitChar }).ToArray<string>(), string.Empty);

                            //标示为白名单已写入
                            sqlList_SetMjAuthor.Add(string.Format("update White_List Set IsWrite =1 where Guid = '{0}'", item.Guid));
                            ////【修改】安全开门
                            //SafeToOpenDoor(item.ControlIP, item.ControlPort, 1);
                        }
                    }
                    //记录授权标示
                    if (sqlList_SetMjAuthor.Count > 0)
                    {
                        //执行数据库操作
                        DBHelper.Transaction(sqlList_SetMjAuthor, out error);
                    }

                    #endregion
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
        }

        /// <summary>
        /// 移除过期白名单记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckDelete_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                #region 会前校验
                //获取所有会议前15分钟到前10分钟的门禁白名单记录 
                List<DoorControlEntity> doorControlEntityList2 = this.GetNowDateBeforeMinitesDoorEntityWhiteList();

                #region 清除记录（会议前15分钟到前10分钟的门禁记录）

                //清除这个时间段相关门禁的记录【修改：清除后写入超级用户】
                foreach (var item in doorControlEntityList2)
                {
                    //【增加：保存刷卡记录】
                    this.SaveSignList(item.ControlIP, item.ControlPort);

                    //清除门禁记录
                    this.ClearMJAuthor(item.ControlIP, item.ControlPort);

                    #region 操作对所有门禁授权的超级用户卡号
                    //获取超级用户卡号列表
                    string[] cardNumberList = GetSuperUserCardNumberList();
                    if (cardNumberList != null)
                    {
                        //门禁白名单授权
                        this.SetMjAuthor(item.ControlIP, item.ControlPort, 1, cardNumberList, string.Empty);
                    }
                    #endregion

                    #region 操作对指定门禁授权的超级用户卡号
                    //获取指定门禁的超级用户卡号列表
                    string[] specialCardNumberList = GetSuperUserCardNumberListByControlIP(item.ControlIP);
                    if (specialCardNumberList != null)
                    {
                        //门禁白名单授权
                        this.SetMjAuthor(item.ControlIP, item.ControlPort, 1, specialCardNumberList, string.Empty);
                    }
                    #endregion

                }

                #endregion
                #endregion

                #region 会后清除
                //获取超出日期的门禁白名单记录
                List<DoorControlEntity> doorControlEntityList = this.GetOutNowDateDoorEntityWhiteList();

                if (doorControlEntityList.Count > 0)
                {
                    //批量sql语句
                    //List<string> sqlList = new List<string>();
                    //异常信息
                    string error = string.Empty;
                    //遍历执行
                    foreach (var item in doorControlEntityList)
                    {
                        //【增加：保存刷卡记录】
                        this.SaveSignList(item.ControlIP, item.ControlPort);

                        //统计异常信息
                        //签到异常
                        InsertBadRecordBySignInAbnormal(item);
                        //签退异常
                        InsertBadRecordBySignOutAbnormal(item);


                        //清除指定门禁白名单记录
                        this.ClearMJAuthor(item.ControlIP, item.ControlPort);
                        ////清除当前数据库白名单记录
                        //sqlList.Add(string.Format("delete from  White_List where ControlIP = '{0}'", item.ControlIP));

                        #region 操作对所有门禁授权的超级用户卡号
                        //获取超级用户卡号列表
                        string[] cardNumberList = GetSuperUserCardNumberList();
                        if (cardNumberList != null)
                        {
                            //门禁白名单授权
                            this.SetMjAuthor(item.ControlIP, item.ControlPort, 1, cardNumberList, string.Empty);
                        }
                        #endregion

                        #region 操作对指定门禁授权的超级用户卡号
                        //获取指定门禁的超级用户卡号列表
                        string[] specialCardNumberList = GetSuperUserCardNumberListByControlIP(item.ControlIP);
                        if (specialCardNumberList != null)
                        {
                            //门禁白名单授权
                            this.SetMjAuthor(item.ControlIP, item.ControlPort, 1, specialCardNumberList, string.Empty);
                        }
                        #endregion
                    }
                    //DBHelper.Transaction(sqlList, out error);
                }
                #endregion
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
        }

        #endregion

        #region 【增加】安全开门方法
        /// <summary>
        /// 安全开门方法
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">端口号</param>
        /// <param name="nDoorIndex">门禁号</param>
        [WebMethod]
        public void SafeToOpenDoor(string DoorControlIp, string DoorControlPort, int nDoorIndex)
        {
            try
            {
                //强制开门
                this.ForceOpenDoor(DoorControlIp, DoorControlPort, nDoorIndex);
                //线程睡眠3秒
                Thread.Sleep(Constant.SleepTime);
                //取消强制
                this.CanclForceDoor(DoorControlIp, DoorControlPort, nDoorIndex);
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
        }
        #endregion

        #region 多卡刷门控制区域

        /// <summary>
        /// 多卡刷门控制区域
        /// </summary>
        public void MoreDoorCardContrl()
        {
            try
            {
                //this.GetData()
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

        #region 【增加】获取门状态（门关/门开）
        /// <summary>
        /// 获取门状态（门关/门开）
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <param name="nDoorIndex">门禁号</param>
        /// <returns>门状态（"门关"/"门开"）</returns>
        public string QueryDoorStatusIsOpenOrClose(string DoorControlIp, string DoorControlPort, int nDoorIndex)
        {
            //调用QueryDoorStatus方法
            DoorControlStateEntity doorControlStateEntity = QueryDoorStatus(DoorControlIp, DoorControlPort, nDoorIndex);
            //获取并返回门关/门开的状态
            return doorControlStateEntity.DoorStatus;
        }
        /// <summary>
        /// 获取门状态（门关/门开）的JSON字符串
        /// </summary>
        /// <param name="DoorControlIps">门禁IP，可以为多个，中间用逗号分隔</param>
        /// <param name="DoorControlPort">门禁端口</param>
        /// <returns>返回门禁对应的状态</returns>
        [WebMethod]
        public string QueryDoorStatusIsOpenOrCloseToJSON(string DoorControlIps, string DoorControlPort)
        {
            //拆分门禁IP列表
            string[] DoorControlIp = DoorControlIps.Split(',');
            //创建DataTable对象
            DataTable table = new DataTable();
            DataColumn column1 = new DataColumn("DoorControlIp");
            table.Columns.Add(column1);
            DataColumn column2 = new DataColumn("MeetRoomName");
            table.Columns.Add(column2);
            foreach (string item in DoorControlIp)
            {
                table.Rows.Add(item, QueryDoorStatusIsOpenOrClose(item, DoorControlPort, 1));
            }
            //将DataTable转换成JSON
            return SerializeDataTable(table);
        }
        #endregion

        #region 【增加】超级用户管理

        #region 插入一条超级用户信息【已修改为批量插入，由 插入超级用户信息 替代】
        /*
        /// <summary>
        /// 插入一条超级用户信息到数据库
        /// </summary>
        /// <param name="UserName">超级用户姓名</param>
        /// <param name="CardNumber">卡号</param>
        /// <param name="DepartmentName">部门名称</param>
        /// <param name="ValidDate">有效日期</param>
        /// <returns></returns>
        public bool InsertOneSuperUserListToSQL(string UserName, string CardNumber, string DepartmentName, string ValidDate)
        {
            //接口调用返回信息
            bool Successed = false;
            try
            {
                //sql语句
                string sql = string.Empty;
                string error = string.Empty;
                //10位卡号为有效数据
                if (CardNumber.Length == 10)
                {
                    sql = string.Format(@"Insert into SuperUser_List(UserName,CardNumber,DepartmentName,ValidDate) 
                                              Values('{0}','{1}','{2}','{3}')", UserName, CardNumber, DepartmentName, ValidDate);
                }
                else
                {
                    //卡号无效抛出异常信息
                    throw new Exception("无效卡号");
                }
                //执行Tsql操作
                DBHelper.Transaction(sql, out error);
                if (string.IsNullOrEmpty(error))
                {
                    //执行返回标示
                    Successed = true;
                    #region 【添加：将成功添加的超级用户卡号写入所有的门禁】
                    //遍历所有门禁
                    DoorControlInfo doorControlInfo = new DoorControlInfo();
                    doorControlInfo = SearchDoorControl();
                    foreach (DoorControlInfoItem item in doorControlInfo.DoorControlInfoItemList)
                    {
                        //将超级用户卡号写入白名单--不能清空已授权信息
                        this.SetMjAuthor(item.ControlIP, item.ControlPort, 0, CardNumber.Split(new char[] { Constant.CardListSplitChar }), string.Empty);
                    }
                    #endregion
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
            return Successed;
        }
         */
        #endregion

        #region 插入超级用户信息
        /// <summary>
        /// 插入超级用户信息并返回未能正确写入的门禁的IP
        /// </summary>
        /// <param name="UserNames">超级用户姓名</param>
        /// <param name="CardNumbers">卡号</param>
        /// <param name="DepartmentNames">部门名称</param>
        /// <param name="ValidDate">有效日期</param>
        /// <returns>返回未能正确写入的门禁的IP，如果全部写入成功，则返回True</returns>
        [WebMethod]
        public string InsertSuperUserListReturnString(string UserNames, string CardNumbers, string DepartmentNames, string ValidDate)
        {
            bool Successed = false;
            //返回结果（执行成功返回"True"，执行失败返回不通的IP字符串）
            string strResult = string.Empty;
            try
            {
                //用于记录有效卡号
                List<string> CardNumberList = null;
                #region 生成SQL语句
                //sql语句
                string sql = string.Empty;
                string error = string.Empty;
                //验证字符串不为""，即传递了正确的值
                if (UserNames.Length > 0 && CardNumbers.Length > 0 && DepartmentNames.Length > 0 && ValidDate.Length > 0)
                {
                    sql = @"Insert into SuperUser_List( UserName , CardNumber , DepartmentName , ValidDate )  values ";
                    //初始化有效卡号列表集合
                    CardNumberList = new List<string>();
                    string[] shuzu_UserNames = UserNames.Trim().Split(',');
                    string[] shuzu_CardNumbers = CardNumbers.Trim().Split(',');
                    string[] shuzu_DepartmentNames = DepartmentNames.Trim().Split(',');
                    //判断是否需要批量插入SQL
                    if (shuzu_UserNames.Length > 1 && shuzu_CardNumbers.Length > 1 && shuzu_DepartmentNames.Length > 1)
                    {
                        //记录拼接的SQL语句
                        StringBuilder sqlBuilder = new StringBuilder();
                        for (int i = 0; i < shuzu_UserNames.Length; i++)
                        {
                            string UserName = shuzu_UserNames[i];
                            string CardNumber = shuzu_CardNumbers[i];
                            string DepartmentName = shuzu_DepartmentNames[i];

                            //10位卡号为有效数据
                            if (CardNumber.Length == 10)
                            {
                                //记录有效卡号
                                CardNumberList.Add(CardNumber);
                                sqlBuilder.Append(string.Format(" ('{0}','{1}','{2}','{3}'), ", UserName, CardNumber, DepartmentName, ValidDate));
                            }
                            else
                            {
                                //卡号无效抛出异常信息
                                throw new Exception("无效卡号");
                            }
                        }
                        string sqlReady = sqlBuilder.ToString();
                        //去掉最后一个","
                        sqlReady = sqlReady.Substring(0, sqlReady.LastIndexOf(","));
                        sql += sqlReady;
                    }
                    else
                    //只插入一条数据
                    {
                        //10位卡号为有效数据
                        if (CardNumbers.Length == 10)
                        {
                            //记录有效卡号
                            CardNumberList.Add(CardNumbers);
                            sql += string.Format(" ('{0}','{1}','{2}','{3}') ", UserNames, CardNumbers, DepartmentNames, ValidDate);
                        }
                        else
                        {
                            //卡号无效抛出异常信息
                            throw new Exception("无效卡号");
                        }
                    }
                }
                #endregion
                //执行Tsql操作
                DBHelper.Transaction(sql, out error);
                if (string.IsNullOrEmpty(error) && CardNumberList != null)
                {
                    #region 【添加：将成功添加的超级用户卡号写入所有的门禁】
                    //遍历所有门禁
                    DoorControlInfo doorControlInfo = new DoorControlInfo();
                    doorControlInfo = SearchDoorControl();
                    //记录写入白名单的门禁IP列表
                    List<string> ControlIPList = new List<string>();
                    //不论是否有不通的门禁，先遍历写入能通的门禁
                    foreach (DoorControlInfoItem item in doorControlInfo.DoorControlInfoItemList)
                    {
                        //将超级用户卡号写入白名单--不能清空已授权信息
                        this.SetMjAuthor(item.ControlIP, item.ControlPort, 0, CardNumberList.ToArray(), string.Empty);
                        //将写入白名单的门禁IP添加至List集合
                        ControlIPList.Add(item.ControlIP);
                    }
                    if (ControlIPList.Count == 18)
                    {
                        //如果搜索到的门禁总数是18，则写入成功，返回True
                        strResult = "True";
                    }
                    else
                    {
                        //否则，说明有网不通的门禁，则查找不通的门禁
                        List<string> badDoorControlList = new List<string>();
                        //所有的门禁IP的数组
                        string[] DoorControlIPs = Constant.DoorControlIP.Split(',');
                        //遍历所有门禁IP的数组
                        foreach (string item in DoorControlIPs)
                        {
                            //如果门禁IP在已成功写入白名单的数组中找不到，则说明该门禁网不通
                            if (!ControlIPList.Contains(item))
                            {
                                //添加至网不通的门禁集合
                                badDoorControlList.Add(item);
                            }
                        }
                        //网不通的门禁IP以逗号分隔拼接成的字符串
                        strResult = string.Join(",", badDoorControlList.ToArray());
                    }

                    #endregion
                    //执行返回标示
                    Successed = true;
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return strResult;
        }

        /// <summary>
        /// 插入超级用户信息到指定的门禁并返回未能正确写入的门禁的IP
        /// </summary>
        /// <param name="UserNames">超级用户名</param>
        /// <param name="CardNumbers">卡号</param>
        /// <param name="DepartmentNames">部门名称</param>
        /// <param name="ValidDate">有效日期</param>
        /// <param name="AssignedControlIPList">指定门禁IP列表</param>
        /// <param name="ControlPort">门禁端口号</param>
        /// <returns></returns>
        [WebMethod]
        public string InsertSuperUserListIntoAssignedRoomAndReturnString(string UserNames, string CardNumbers, string DepartmentNames, string ValidDate, string AssignedControlIPList, string ControlPort)
        {
            bool Successed = false;
            //返回结果（执行成功返回"True"，执行失败返回不通的IP字符串）
            string strResult = string.Empty;
            try
            {
                //用于记录有效卡号
                List<string> CardNumberList = null;
                #region 生成SQL语句
                //sql语句
                string sql = string.Empty;
                string error = string.Empty;
                //验证字符串不为""，即传递了正确的值
                if (UserNames.Length > 0 && CardNumbers.Length > 0 && DepartmentNames.Length > 0 && ValidDate.Length > 0)
                {
                    sql = @"Insert into SuperUser_List( UserName , CardNumber , DepartmentName , ValidDate , DoorControlList )  values ";
                    //初始化有效卡号列表集合
                    CardNumberList = new List<string>();
                    string[] shuzu_UserNames = UserNames.Trim().Split(',');
                    string[] shuzu_CardNumbers = CardNumbers.Trim().Split(',');
                    string[] shuzu_DepartmentNames = DepartmentNames.Trim().Split(',');
                    //判断是否需要批量插入SQL
                    if (shuzu_UserNames.Length > 1 && shuzu_CardNumbers.Length > 1 && shuzu_DepartmentNames.Length > 1)
                    {
                        //记录拼接的SQL语句
                        StringBuilder sqlBuilder = new StringBuilder();
                        for (int i = 0; i < shuzu_UserNames.Length; i++)
                        {
                            string UserName = shuzu_UserNames[i];
                            string CardNumber = shuzu_CardNumbers[i];
                            string DepartmentName = shuzu_DepartmentNames[i];

                            //10位卡号为有效数据
                            if (CardNumber.Length == 10)
                            {
                                //记录有效卡号
                                CardNumberList.Add(CardNumber);
                                sqlBuilder.Append(string.Format(" ('{0}','{1}','{2}','{3}','{4}'), ", UserName, CardNumber, DepartmentName, ValidDate, AssignedControlIPList));
                            }
                            else
                            {
                                //卡号无效抛出异常信息
                                throw new Exception("无效卡号");
                            }
                        }
                        string sqlReady = sqlBuilder.ToString();
                        //去掉最后一个","
                        sqlReady = sqlReady.Substring(0, sqlReady.LastIndexOf(","));
                        sql += sqlReady;
                    }
                    else
                    //只插入一条数据
                    {
                        //10位卡号为有效数据
                        if (CardNumbers.Length == 10)
                        {
                            //记录有效卡号
                            CardNumberList.Add(CardNumbers);
                            sql += string.Format(" ('{0}','{1}','{2}','{3}','{4}') ", UserNames, CardNumbers, DepartmentNames, ValidDate, AssignedControlIPList);
                        }
                        else
                        {
                            //卡号无效抛出异常信息
                            throw new Exception("无效卡号");
                        }
                    }
                }
                #endregion
                //执行Tsql操作
                DBHelper.Transaction(sql, out error);
                if (string.IsNullOrEmpty(error) && CardNumberList != null)
                {
                    #region 【添加：将成功添加的超级用户卡号写入指定的门禁】
                    //指定门禁IP数组
                    string[] DoorControlIPList = AssignedControlIPList.Trim().Split(',');

                    //记录写入白名单的门禁IP列表
                    List<string> ControlIPList = new List<string>();

                    //记录IP不通的门禁
                    List<string> badDoorControlList = new List<string>();

                    //不论是否有不通的门禁，先遍历写入能通的门禁
                    foreach (string item in DoorControlIPList)
                    {
                        //如果IP是通的
                        if (IPCheck.TestNetConnectity(item))
                        {
                            //将超级用户卡号写入白名单--不能清空已授权信息
                            this.SetMjAuthor(item, ControlPort, 0, CardNumberList.ToArray(), string.Empty);
                            //将写入白名单的门禁IP添加至List集合
                            ControlIPList.Add(item);
                        }
                        else
                        {
                            badDoorControlList.Add(item);
                        }
                    }
                    if (ControlIPList.Count == DoorControlIPList.Length)
                    {
                        //如果搜索到的门禁总数是18，则写入成功，返回True
                        strResult = "True";
                    }
                    else
                    {
                        //网不通的门禁IP以逗号分隔拼接成的字符串
                        strResult = string.Join(",", badDoorControlList.ToArray());
                    }

                    #endregion
                    //执行返回标示
                    Successed = true;
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return strResult;
        }

        /// <summary>
        /// 插入超级用户信息
        /// </summary>
        /// <param name="UserNames">超级用户姓名</param>
        /// <param name="CardNumbers">卡号</param>
        /// <param name="DepartmentNames">部门名称</param>
        /// <param name="ValidDate">有效日期</param>
        /// <returns></returns>
        [WebMethod]
        public bool InsertSuperUserList(string UserNames, string CardNumbers, string DepartmentNames, string ValidDate)
        {
            bool Successed = false;
            try
            {
                //用于记录有效卡号
                List<string> CardNumberList = null;
                #region 生成SQL语句
                //sql语句
                string sql = string.Empty;
                string error = string.Empty;
                //验证字符串不为""，即传递了正确的值
                if (UserNames.Length > 0 && CardNumbers.Length > 0 && DepartmentNames.Length > 0 && ValidDate.Length > 0)
                {
                    sql = @"Insert into SuperUser_List( UserName , CardNumber , DepartmentName , ValidDate )  values ";
                    //初始化有效卡号列表集合
                    CardNumberList = new List<string>();
                    string[] shuzu_UserNames = UserNames.Split(',');
                    string[] shuzu_CardNumbers = CardNumbers.Split(',');
                    string[] shuzu_DepartmentNames = DepartmentNames.Split(',');
                    //判断是否需要批量插入SQL
                    if (shuzu_UserNames.Length > 1 && shuzu_CardNumbers.Length > 1 && shuzu_DepartmentNames.Length > 1)
                    {
                        //记录拼接的SQL语句
                        StringBuilder sqlBuilder = new StringBuilder();
                        for (int i = 0; i < shuzu_UserNames.Length; i++)
                        {
                            string UserName = shuzu_UserNames[i];
                            string CardNumber = shuzu_CardNumbers[i];
                            string DepartmentName = shuzu_DepartmentNames[i];

                            //10位卡号为有效数据
                            if (CardNumber.Length == 10)
                            {
                                //记录有效卡号
                                CardNumberList.Add(CardNumber);
                                sqlBuilder.Append(string.Format(" ('{0}','{1}','{2}','{3}'), ", UserName, CardNumber, DepartmentName, ValidDate));
                            }
                            else
                            {
                                //卡号无效抛出异常信息
                                throw new Exception("无效卡号");
                            }
                        }
                        string sqlReady = sqlBuilder.ToString();
                        //去掉最后一个","
                        sqlReady = sqlReady.Substring(0, sqlReady.LastIndexOf(","));
                        sql += sqlReady;
                    }
                    else
                    //只插入一条数据
                    {
                        //10位卡号为有效数据
                        if (CardNumbers.Length == 10)
                        {
                            //记录有效卡号
                            CardNumberList.Add(CardNumbers);
                            sql += string.Format(" ('{0}','{1}','{2}','{3}') ", UserNames, CardNumbers, DepartmentNames, ValidDate);
                        }
                        else
                        {
                            //卡号无效抛出异常信息
                            throw new Exception("无效卡号");
                        }
                    }
                }
                #endregion
                //执行Tsql操作
                DBHelper.Transaction(sql, out error);
                if (string.IsNullOrEmpty(error) && CardNumberList != null)
                {
                    #region 【添加：将成功添加的超级用户卡号写入所有的门禁】
                    //遍历所有门禁
                    DoorControlInfo doorControlInfo = new DoorControlInfo();
                    doorControlInfo = SearchDoorControl();
                    foreach (DoorControlInfoItem item in doorControlInfo.DoorControlInfoItemList)
                    {
                        //将超级用户卡号写入白名单--不能清空已授权信息
                        this.SetMjAuthor(item.ControlIP, item.ControlPort, 0, CardNumberList.ToArray(), string.Empty);
                    }
                    #endregion
                    //执行返回标示
                    Successed = true;
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return Successed;
        }

        #endregion

        #region 删除过期用户数据
        /// <summary>
        /// 删除过期用户数据
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public bool DeleteExpiredSuperUserList()
        {
            //接口调用返回信息
            bool Successed = false;
            try
            {
                //查询是否存在过期用户
                string[] cardNumberList = GetExpiredSuperUserCardNumberList();

                if (cardNumberList != null)
                {
                    string cardDel = string.Join("", cardNumberList);
                    //sql语句
                    string sql = string.Empty;
                    string error = string.Empty;
                    sql = string.Format(@"delete from superuser_list where CAST(validdate as datetime) <DATEADD(day,-1, GETDATE())");

                    //执行Tsql操作
                    DBHelper.Transaction(sql, out error);
                    if (string.IsNullOrEmpty(error))
                    {
                        #region 【添加：将成功删除的超级用户卡号从所有的门禁中删除】
                        //遍历所有门禁
                        DoorControlInfo doorControlInfo = new DoorControlInfo();
                        doorControlInfo = SearchDoorControl();
                        foreach (DoorControlInfoItem item in doorControlInfo.DoorControlInfoItemList)
                        {
                            //将超级用户卡号删除--不能清空已授权信息
                            this.SetMjAuthor(item.ControlIP, item.ControlPort, 0, null, cardDel);
                        }
                        #endregion
                        //执行返回标示
                        Successed = true;
                    }
                }
                else
                {
                    //不存在过期用户即不执行删除操作，也要标识执行成功
                    Successed = true;
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
            return Successed;
        }
        #endregion

        #region 删除一条超级用户信息
        /// <summary>
        /// 删除一条超级用户信息
        /// </summary>
        /// <param name="CardNumber">卡号</param>
        /// <returns></returns>
        [WebMethod]
        public bool DeleteOneSuperUserList(string CardNumber)
        {
            //接口调用返回信息
            bool Successed = false;
            try
            {
                //sql语句
                string sql = string.Empty;
                string error = string.Empty;
                sql = string.Format(@"delete from superuser_list where cardnumber = '{0}'", CardNumber);

                //执行Tsql操作
                DBHelper.Transaction(sql, out error);
                if (string.IsNullOrEmpty(error))
                {
                    #region 【添加：将成功删除的超级用户卡号从所有的门禁中删除】
                    //遍历所有门禁
                    DoorControlInfo doorControlInfo = new DoorControlInfo();
                    doorControlInfo = SearchDoorControl();
                    foreach (DoorControlInfoItem item in doorControlInfo.DoorControlInfoItemList)
                    {
                        //将超级用户卡号删除--不能清空已授权信息
                        this.SetMjAuthor(item.ControlIP, item.ControlPort, 0, null, CardNumber);
                    }
                    #endregion
                    //执行返回标示
                    Successed = true;
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
            return Successed;
        }
        #endregion

        #region 【20151124增加】根据Guid删除一条超级用户信息
        /// <summary>
        /// 根据Guid删除一条超级用户信息
        /// </summary>
        /// <param name="Guid">Guid</param>
        /// <param name="CardNumber">卡号</param>
        /// <param name="DoorControlList">门禁列表（用,分割）</param>
        /// <param name="ControlPort">端口号</param>
        /// <returns>返回执行是否成功</returns>
        [WebMethod]
        public bool DeleteOneSuperUserListByGuid(string Guid, string CardNumber, string DoorControlList, string ControlPort)
        {
            //接口调用返回信息
            bool Successed = false;
            try
            {
                //sql语句
                string sql = string.Empty;
                string error = string.Empty;
                sql = string.Format(@"delete from SuperUser_List where Guid = {0}", Guid);

                //执行Tsql操作
                DBHelper.Transaction(sql, out error);
                if (string.IsNullOrEmpty(error) && DoorControlList != null)
                {
                    #region 【添加：将成功删除的超级用户卡号从相应的门禁中删除】
                    //遍历指定门禁
                    string[] DoorControlIPList = DoorControlList.Split(',');
                    foreach (string item in DoorControlIPList)
                    {
                        //将超级用户卡号删除--不能清空已授权信息
                        this.SetMjAuthor(item, ControlPort, 0, null, CardNumber);
                    }
                    #endregion
                    //执行返回标示
                    Successed = true;
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
            return Successed;
        }
        #endregion

        #region 获取有效超级用户列表
        /// <summary>
        /// 获取有效超级用户列表
        /// </summary>
        /// <returns></returns>
        public List<SuperUserListEntity> GetSuperUserList()
        {
            //接口调用返回信息
            List<SuperUserListEntity> list = new List<SuperUserListEntity>();
            //删除过期用户数据
            if (DeleteExpiredSuperUserList())
            {
                try
                {
                    //暂时使用文本命令
                    string sql = string.Format("select * from SuperUser_List order by UserName");
                    //异常信息
                    string error = string.Empty;

                    //获取相对应的实体类对象
                    list = DBHelper.ExcuteEntity<SuperUserListEntity>(sql, System.Data.CommandType.Text, out error);
                }
                catch (Exception ex)
                {
                    //日志记录
                    LogManage.WriteLog(typeof(DoorControl), ex);
                }
                finally
                {

                }
            }
            return list;
        }
        #endregion

        #region 获取需要写入所有门禁的有效超级用户列表
        /// <summary>
        /// 获取需要写入所有门禁的有效超级用户列表
        /// </summary>
        /// <returns>返回需要写入所有门禁的有效超级用户列表</returns>
        public List<SuperUserListEntity> GetSpecialSuperUserList()
        {
            //接口调用返回信息
            List<SuperUserListEntity> list = new List<SuperUserListEntity>();
            //删除过期用户数据
            if (DeleteExpiredSuperUserList())
            {
                try
                {
                    //暂时使用文本命令
                    string sql = string.Format("select * from superuser_list where DoorControlList is null");
                    //异常信息
                    string error = string.Empty;

                    //获取相对应的实体类对象
                    list = DBHelper.ExcuteEntity<SuperUserListEntity>(sql, System.Data.CommandType.Text, out error);
                }
                catch (Exception ex)
                {
                    //日志记录
                    LogManage.WriteLog(typeof(DoorControl), ex);
                }
                finally
                {

                }
            }
            return list;
        }
        #endregion

        #region 获取有效超级用户卡号列表
        /// <summary>
        /// 获取有效超级用户卡号列表
        /// </summary>
        /// <returns></returns>
        public string[] GetSuperUserCardNumberList()
        {
            List<SuperUserListEntity> list = new List<SuperUserListEntity>();
            try
            {
                //获取需要写入所有门禁的有效超级用户列表
                list = GetSpecialSuperUserList();
                if (list.Count > 0)
                {
                    //记录超级用户卡号列表
                    List<string> cardNumberList = new List<string>();

                    //遍历超级用户列表，获取卡号列表
                    for (int i = 0; i < list.Count; i++)
                    {
                        //10位卡号（有效卡号）才进行记录
                        if (list[i].CardNumber.Length == 10)
                        {
                            cardNumberList.Add(list[i].CardNumber);
                        }
                    }
                    return cardNumberList.ToArray();
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
            return null;
        }
        #endregion

        #region 获取指定门禁的超级用户列表
        /// <summary>
        /// 获取指定门禁的超级用户列表
        /// </summary>
        /// <param name="controlIP">门禁IP</param>
        /// <returns>返回超级用户信息列表</returns>
        private List<SuperUserListEntity> GetSuperUserListByControlIP(string controlIP)
        {
            //接口调用返回信息
            List<SuperUserListEntity> list = new List<SuperUserListEntity>();
            //删除过期用户数据
            if (DeleteExpiredSuperUserList())
            {
                try
                {
                    //暂时使用文本命令
                    string sql = string.Format("select * from superuser_list where DoorControlList like '%{0}%'", controlIP);
                    //异常信息
                    string error = string.Empty;

                    //获取相对应的实体类对象
                    list = DBHelper.ExcuteEntity<SuperUserListEntity>(sql, System.Data.CommandType.Text, out error);
                }
                catch (Exception ex)
                {
                    //日志记录
                    LogManage.WriteLog(typeof(DoorControl), ex);
                }
                finally
                {

                }
            }
            return list;
        }
        #endregion

        #region 获取指定门禁的超级用户卡号列表
        /// <summary>
        /// 获取指定门禁的超级用户卡号列表
        /// </summary>
        /// <param name="controlIP">门禁IP</param>
        /// <returns>返回指定门禁的超级用户卡号列表</returns>
        private string[] GetSuperUserCardNumberListByControlIP(string controlIP)
        {
            List<SuperUserListEntity> list = new List<SuperUserListEntity>();
            try
            {
                //获取需要写入指定门禁的有效超级用户列表
                list = GetSuperUserListByControlIP(controlIP);
                if (list.Count > 0)
                {
                    //记录超级用户卡号列表
                    List<string> cardNumberList = new List<string>();

                    //遍历超级用户列表，获取卡号列表
                    for (int i = 0; i < list.Count; i++)
                    {
                        //10位卡号（有效卡号）才进行记录
                        if (list[i].CardNumber.Length == 10)
                        {
                            cardNumberList.Add(list[i].CardNumber);
                        }
                    }
                    return cardNumberList.ToArray();
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
            return null;
        }
        #endregion

        #region 获取过期用户数据
        /// <summary>
        /// 获取过期用户数据
        /// </summary>
        /// <returns></returns>
        public List<SuperUserListEntity> GetExpiredSuperUserList()
        {
            //接口调用返回信息
            List<SuperUserListEntity> list = new List<SuperUserListEntity>();

            try
            {
                //暂时使用文本命令
                string sql = string.Format("select * from superuser_list where CAST(validdate as datetime) <DATEADD(day,-1, GETDATE())");
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<SuperUserListEntity>(sql, System.Data.CommandType.Text, out error);
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }

            return list;
        }
        #endregion

        #region 获取过期超级用户卡号列表
        /// <summary>
        /// 获取过期超级用户卡号列表
        /// </summary>
        /// <returns></returns>
        public string[] GetExpiredSuperUserCardNumberList()
        {
            List<SuperUserListEntity> list = new List<SuperUserListEntity>();
            try
            {
                //获取过期超级用户列表
                list = GetExpiredSuperUserList();
                if (list.Count > 0)
                {
                    //记录过期超级用户卡号列表
                    string[] cardNumberList = new string[list.Count];
                    //遍历超级用户列表，获取卡号列表
                    for (int i = 0; i < list.Count; i++)
                    {
                        cardNumberList[i] = list[i].CardNumber;
                    }
                    return cardNumberList;
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
            return null;
        }
        #endregion

        #region 获取有效超级用户列表的JSON字符串
        /// <summary>
        /// 获取有效超级用户列表的JSON字符串
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetSuperUserListToJOSN()
        {
            return Serialize(GetSuperUserList());
        }
        #endregion

        #endregion

        #region 【增加】将对象转为JSON字符串
        /// <summary>
        /// 将对象转为JSON字符串
        /// </summary>
        /// <param name="obj">object对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string str = js.Serialize(obj);
            #region 处理含有DateTime的JSON字符串
            //str = Regex.Replace(str, @"\\/Date\((\d+)\)\\/", match =>
            //{
            //    DateTime dt = new DateTime(1970, 1, 1);
            //    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
            //    dt = dt.ToLocalTime();
            //    return dt.ToString("yyyy-MM-dd HH:mm:ss");
            //});
            #endregion
            return str;
        }
        #endregion

        #region 【增加】将DataTable转换成JSON
        /// <summary>
        /// 将DataTable转换成JSON
        /// </summary>
        /// <param name="dt">DataTable类型对象</param>
        /// <returns>返回JSON字符串</returns>
        public static string SerializeDataTable(DataTable dt)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {

                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (DataColumn dc in dt.Columns)
                {

                    result.Add(dc.ColumnName, dr[dc].ToString());

                }

                list.Add(result);

            }

            return serializer.Serialize(list); ;

        }

        #endregion

        #region 【增加】将JSON字符串转为DataTable（暂时未使用）
        /// <summary>
        /// 将JSON字符串转为DataTable
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns></returns>
        public DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {

                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion

        #region 【20151203增加】将JSON字符串转为实体
        /// <summary>
        /// 将JSON字符串转为实体
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <returns></returns>
        public static List<WebConfigEntity> getObjectEntityByJson(string jsonString)
        {
            // 实例化DataContractJsonSerializer对象，需要待序列化的对象类型  
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<WebConfigEntity>));
            //把Json传入内存流中保存  
            //jsonString = "[" + jsonString + "]";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            // 使用ReadObject方法反序列化成对象  
            object ob = serializer.ReadObject(stream);
            List<WebConfigEntity> ls = (List<WebConfigEntity>)ob;

            return ls;
        }
        #endregion

        #region 【增加】信用管理体系

        #region 保存刷卡记录

        /// <summary>
        /// 保存刷卡记录
        /// </summary>
        /// <param name="DoorControlIp">门禁IP</param>
        /// <param name="DoorControlPort">端口号</param>
        /// <returns></returns>
        public bool SaveSignList(string DoorControlIp, string DoorControlPort)
        {
            //标记插入操作是否执行成功，默认插入不成功
            bool mark = false;
            try
            {
                //存储刷卡记录
                List<SignListEntity> signListEntityList = new List<SignListEntity>();
                //循环获取门禁数据
                while (true)
                {
                    //调用GetData方法采集门禁数据
                    DoorControlCollectData doorControlCollectData = GetData(DoorControlIp, DoorControlPort);
                    //获取到门禁的数据并且存在刷卡记录
                    if (doorControlCollectData.MjRecord != null && doorControlCollectData.MjRecord.Length > 0)
                    {
                        foreach (sMjRecord item in doorControlCollectData.MjRecord)
                        {
                            //创建刷卡记录对象
                            SignListEntity signListEntity = new SignListEntity();
                            signListEntity.KahaoOrMima = item.KahaoOrMima;
                            signListEntity.ControlIP = DoorControlIp;
                            signListEntity.ControlPort = DoorControlPort;
                            signListEntity.Time = ConvertToDateTime(item.Time);
                            signListEntity.MenXuHao = item.MenXuhao;
                            signListEntity.JinChu = item.JinChu;
                            signListEntity.ShuaKaBiaoZhi = item.ShuakaBiaozhi;
                            //将刷卡记录存储到list集合
                            signListEntityList.Add(signListEntity);
                        }
                        //调用插入数据库的方法
                        mark = InsertSignList(signListEntityList.ToArray());

                    }
                    else
                    {
                        //没有获取到门禁数据或者刷卡记录已读取完毕，退出循环
                        mark = true;
                        break;
                    }
                }
                return mark;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
                mark = false;
                return mark;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 将刷卡记录写入数据库
        /// </summary>
        /// <param name="signListEntitys">刷卡记录实体数组</param>
        /// <returns></returns>
        public bool InsertSignList(SignListEntity[] signListEntitys)
        {
            //标记插入操作是否执行成功，默认插入不成功
            bool mark = false;
            try
            {
                //用于拼接SQL语句
                StringBuilder sqlStringBuilder = new StringBuilder();
                sqlStringBuilder.Append("insert into Sign_List (KahaoOrMima,ControlIP,ControlPort,Time,MenXuHao,JinChu,ShuaKaBiaoZhi) values ");
                //如果方法参数有效
                if (signListEntitys != null && signListEntitys.Length > 0)
                {
                    //遍历参数数组
                    foreach (SignListEntity item in signListEntitys)
                    {
                        //判断卡号是否有效（10位）
                        if (item.KahaoOrMima.Length == 10)
                        {
                            //拼接SQL语句
                            sqlStringBuilder.Append(string.Format(" ('{0}','{1}','{2}','{3}','{4}','{5}','{6}'), ", item.KahaoOrMima, item.ControlIP, item.ControlPort, item.Time, item.MenXuHao, item.JinChu, item.ShuaKaBiaoZhi));
                        }
                    }
                    string sql = sqlStringBuilder.ToString();
                    //去掉最后一个','
                    sql = sql.Substring(0, sql.LastIndexOf(','));
                    //执行Tsql操作
                    string error = string.Empty;
                    DBHelper.Transaction(sql, out error);
                    if (error == null)
                    {
                        //插入操作执行成功
                        mark = true;
                    }
                    return mark;
                }
                else
                {
                    return mark;
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
                return mark;
            }
            finally
            {

            }

        }

        #endregion

        #region 按条件筛选查询刷卡记录
        /// <summary>
        /// 根据条件筛选查询刷卡记录
        /// </summary>
        /// <param name="ConferenceRoom">会议室门禁控制器IP</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns>刷卡记录</returns>
        public List<SignListEntity> GetScreenedSignList(string ConferenceRoom, string StartTime, string EndTime)
        {
            List<SignListEntity> signListEntityList = new List<SignListEntity>();
            try
            {
                //查询语句
                string selectText = "select * from Sign_List where 1=1";
                //错误信息
                string errorString = string.Empty;
                if (ConferenceRoom != "0")
                {
                    selectText += string.Format(" and ControlIP='{0}' ", ConferenceRoom);
                }
                if (StartTime != null && EndTime != null)
                {
                    selectText += string.Format(" and Time>='{0}' and Time<='{1}' ", StartTime, EndTime);
                }
                signListEntityList= DBHelper.ExcuteEntity<SignListEntity>(selectText, CommandType.Text, out errorString);
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return signListEntityList;
        }
        #endregion

        #region 按条件筛选查询刷卡记录返回JSON字符串
        /// <summary>
        /// 按条件筛选查询刷卡记录返回JSON字符串
        /// </summary>
        /// <param name="ConferenceRoom">会议室门禁控制器IP</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns>刷卡记录JSON字符串</returns>
        [WebMethod]
        public string GetScreenedSignListToJSON(string ConferenceRoom, string StartTime, string EndTime)
        {
            return Serialize(GetScreenedSignList(ConferenceRoom, StartTime, EndTime));
        }
        #endregion

        #region 不良记录

        #region 查询所有不良记录
        /// <summary>
        /// 查询有效不良记录
        /// </summary>
        /// <returns>不良记录list集合</returns>
        public List<BadRecordListEntity> GetBadRecordList()
        {
            //接口调用返回信息
            List<BadRecordListEntity> list = new List<BadRecordListEntity>();
            //删除过期用户数据

            try
            {
                //暂时使用文本命令

                string sql = "select top 100 * from BadRecord_List order by RecordTime desc";
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<BadRecordListEntity>(sql, System.Data.CommandType.Text, out error);
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }

            return list;
        }
        #endregion

        #region 查询所有不良记录返回JSON字符串
        /// <summary>
        /// 查询有效不良记录返回JSON字符串
        /// </summary>
        /// <returns>不良记录的JSON字符串</returns>
        [WebMethod]
        public string GetBadRecordListToJSON()
        {
            return Serialize(GetBadRecordList());
        }
        #endregion

        #region 【20151120增加】根据卡号查询不良记录
        /// <summary>
        /// 根据卡号查询不良记录
        /// </summary>
        /// <param name="KahaoOrMima">卡号或密码</param>
        /// <returns>返回指定卡号的不良记录</returns>
        public List<BadRecordListEntity> GetBadRecordListByKahaoOrMima(string KahaoOrMima)
        {
            //接口调用返回信息
            List<BadRecordListEntity> list = new List<BadRecordListEntity>();
            //删除过期用户数据

            try
            {
                //暂时使用文本命令
                string sql = string.Format("select * from BadRecord_List where KahaoOrMima='{0}'  order by Guid desc ", KahaoOrMima);
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<BadRecordListEntity>(sql, System.Data.CommandType.Text, out error);
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }

            return list;
        }
        #endregion

        #region 【20151120增加】根据卡号查询不良记录返回JSON字符串
        /// <summary>
        /// 根据卡号查询不良记录返回JSON字符串
        /// </summary>
        /// <param name="KahaoOrMima">卡号或密码</param>
        /// <returns>返回指定卡号不良记录的JSON字符串</returns>
        [WebMethod]
        public string GetBadRecordListByKahaoOrMimaToJSON(string KahaoOrMima)
        {
            return Serialize(GetBadRecordListByKahaoOrMima(KahaoOrMima));
        }
        #endregion

        #region 手动录入不良记录【修改：增加一个参数操作人】
        /// <summary>
        /// 手动录入不良记录
        /// </summary>
        /// <param name="KahaoOrMima">卡号或密码</param>
        /// <param name="ControlIP">门禁IP</param>
        /// <param name="ControlPort">端口号</param>
        /// <param name="Record">不良记录内容</param>
        /// <param name="RecordTime">产生不良记录的时间</param>
        /// <param name="Operator">操作人</param>
        /// <returns>返回是否执行成功</returns>
        [WebMethod]
        public bool InsertOneBadRecordByAdmin(string KahaoOrMima, string ControlIP, string ControlPort, string Record, string RecordTime, string Operator)
        {
            //标记插入操作是否执行成功，默认插入不成功
            bool mark = false;
            try
            {
                //创建不良记录实体
                BadRecordListEntity badRecordListEntity = new BadRecordListEntity();
                badRecordListEntity.KahaoOrMima = KahaoOrMima;
                badRecordListEntity.ControlIP = ControlIP;
                badRecordListEntity.ControlPort = ControlPort;
                badRecordListEntity.Record = Record;
                badRecordListEntity.RecordTime = Convert.ToDateTime(RecordTime);
                //增加操作人
                badRecordListEntity.Operator = Operator;

                //插入操作执行成功
                mark = InsertBadRecordInSQL(badRecordListEntity);

            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return mark;
        }
        #endregion

        #region 手动录入多条不良记录
        /// <summary>
        /// 手动录入多条不良记录
        /// </summary>
        /// <param name="KahaoOrMimas">卡号或密码，可以为多条，中间用逗号分隔</param>
        /// <param name="ControlIP">门禁IP</param>
        /// <param name="ControlPort">端口号</param>
        /// <param name="Record">不良记录内容</param>
        /// <param name="RecordTime">产生不良记录的时间</param>
        /// <param name="Operator">操作人</param>
        /// <returns></returns>
        [WebMethod]
        public bool InsertSomeBadRecordByAdmin(string KahaoOrMimas, string ControlIP, string ControlPort, string Record, string RecordTime, string Operator)
        {
            //执行标记，默认执行成功
            bool mark = true;
            try
            {
                //排除卡号为空的情况
                if (KahaoOrMimas != null)
                {
                    //拆分卡号列表
                    string[] KahaoOrMimaList = KahaoOrMimas.Split(',');
                    //遍历卡号列表
                    foreach (string item in KahaoOrMimaList)
                    {
                        if (item.Length == 10)
                        {
                            //写入数据库
                            mark = InsertOneBadRecordByAdmin(item, ControlIP, ControlPort, Record, RecordTime, Operator);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //标志执行失败
                mark = false;
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return mark;

        }
        #endregion

        #region 根据卡号删除不良记录
        /// <summary>
        /// 根据卡号删除不良记录
        /// </summary>
        /// <param name="KahaoOrMima">卡号或密码</param>
        /// <returns></returns>
        [WebMethod]
        public bool DeleteBadRecordByKahaoOrMima(string KahaoOrMima)
        {
            //标记更新操作是否执行成功，默认更新不成功
            bool mark = false;
            try
            {
                string sql = string.Empty;
                string error = string.Empty;
                //设置标识IsEffective='0'，表示该条数据无效，等同于被删除，实际没有进行删除操作
                sql = string.Format("update BadRecord_List set IsEffective='0' where KahaoOrMima='{0}'", KahaoOrMima);
                DBHelper.Transaction(sql, out error);
                if (string.IsNullOrEmpty(error))
                {
                    //更新操作执行成功
                    mark = true;
                }
                return mark;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
                mark = false;
                return mark;
            }
            finally
            {

            }
        }
        #endregion

        #region 根据ID删除不良记录
        /// <summary>
        /// 根据ID删除不良记录
        /// </summary>
        /// <param name="Guid">不良记录ID</param>
        /// <returns></returns>
        [WebMethod]
        public bool DeleteBadRecordByGuid(string Guid)
        {
            //标记更新操作是否执行成功，默认更新不成功
            bool mark = false;
            try
            {
                string sql = string.Empty;
                string error = string.Empty;
                //设置标识IsEffective='0'，表示该条数据无效，等同于被删除，实际没有进行删除操作
                sql = string.Format("update BadRecord_List set IsEffective='0' where Guid='{0}'", Guid);
                DBHelper.Transaction(sql, out error);
                if (error == null)
                {
                    //更新操作执行成功
                    mark = true;
                }
                return mark;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
                mark = false;
                return mark;
            }
            finally
            {

            }
        }
        #endregion

        #region 删除过期不良记录
        /// <summary>
        /// 删除过期不良记录
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public bool DeleteExpiredBadRecordList()
        {
            //接口调用返回信息
            bool Successed = false;
            try
            {
                //sql语句
                string sql = string.Empty;
                string error = string.Empty;
                sql = string.Format(@"update BadRecord_List set IsEffective='0' where RecordTime <DATEADD(day,-1, GETDATE())");

                //执行Tsql操作
                DBHelper.Transaction(sql, out error);
                if (string.IsNullOrEmpty(error))
                {
                    //执行返回标示
                    Successed = true;
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
            return Successed;
        }
        #endregion

        #region 根据ID修改不良记录

        /// <summary>
        /// 根据ID修改不良记录
        /// </summary>
        /// <param name="Guid">ID</param>
        /// <param name="Record">不良记录内容</param>
        /// <param name="RecordTime">产生不良记录的时间</param>
        /// <param name="Operator">操作人</param>
        /// <returns>返回是否执行成功</returns>
        [WebMethod]
        public bool UpdateBadRecordByGuid(string Guid, string Record, string RecordTime, string Operator)
        {
            //标记更新操作是否执行成功，默认更新不成功
            bool mark = false;
            try
            {
                string sql = string.Empty;
                string error = string.Empty;
                DateTime dtRecordTime = Convert.ToDateTime(RecordTime);
                //sql语句，根据ID修改不良记录内容及产生不良记录的时间
                sql = string.Format("update BadRecord_List set Record='{0}',RecordTime='{1}',Operator='{2}',OperationTime=GETDATE() where Guid='{3}'", Record, dtRecordTime, Operator, Guid);
                DBHelper.Transaction(sql, out error);
                if (error == null)
                {
                    //更新操作执行成功
                    mark = true;
                }
                return mark;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
                mark = false;
                return mark;
            }
            finally
            {

            }
        }

        #endregion

        #region 因签到/签退异常而产生不良记录

        #region 因签到异常而产生不良记录
        /// <summary>
        /// 因签到异常而产生不良记录
        /// </summary>
        /// <param name="doorControlEntity">门禁实体</param>
        public void InsertBadRecordBySignInAbnormal(DoorControlEntity doorControlEntity)
        {
            try
            {
                //应该参加会议的卡号列表
                string[] participantsList = doorControlEntity.CardList.Split(',');
                //获取签到记录
                List<SignListEntity> signInList = GetSingleDoorControlTimeSpanSignList(doorControlEntity.ControlIP, doorControlEntity.ControlPort, doorControlEntity.StartDate, doorControlEntity.EndDate, "进");
                foreach (string item in participantsList)
                {
                    //标记变量，记录是否存在签到记录
                    bool isRepeatMark = false;
                    foreach (SignListEntity item2 in signInList)
                    {
                        if (item == item2.KahaoOrMima)
                        {
                            isRepeatMark = true;
                            if (item2.Time >= Convert.ToDateTime(doorControlEntity.StartDate).AddMinutes(31) && item2.Time <= Convert.ToDateTime(doorControlEntity.EndDate).AddMinutes(5))
                            {
                                //记录不良信息，迟到
                                BadRecordListEntity badRecordListEntity = new BadRecordListEntity();
                                badRecordListEntity.KahaoOrMima = item2.KahaoOrMima;
                                badRecordListEntity.ControlIP = doorControlEntity.ControlIP;
                                badRecordListEntity.ControlPort = doorControlEntity.ControlPort;
                                badRecordListEntity.SignListGuid = item2.Guid;
                                badRecordListEntity.Record = "会议迟到，签到时间为：" + item2.Time;
                                badRecordListEntity.RecordTime = item2.Time;
                                //将不良信息写入数据库
                                InsertBadRecordInSQL(badRecordListEntity);
                            }
                            break;
                        }
                    }
                    if (isRepeatMark == false)
                    {
                        //记录不良信息，未签到
                        BadRecordListEntity badRecordListEntity = new BadRecordListEntity();
                        badRecordListEntity.KahaoOrMima = item;
                        badRecordListEntity.ControlIP = doorControlEntity.ControlIP;
                        badRecordListEntity.ControlPort = doorControlEntity.ControlPort;
                        badRecordListEntity.Record = "会议未签到";
                        badRecordListEntity.RecordTime = Convert.ToDateTime(doorControlEntity.EndDate);
                        //将不良信息写入数据库
                        InsertBadRecordInSQL(badRecordListEntity);
                    }
                }

            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }
        #endregion

        #region 因签退异常而产生不良记录
        /// <summary>
        /// 因签退异常而产生不良记录
        /// </summary>
        /// <param name="doorControlEntity">门禁实体</param>
        public void InsertBadRecordBySignOutAbnormal(DoorControlEntity doorControlEntity)
        {
            try
            {
                //应该参加会议的卡号列表
                string[] participantsList = doorControlEntity.CardList.Split(',');
                //获取签退记录
                List<SignListEntity> signOutList = GetSingleDoorControlTimeSpanSignList(doorControlEntity.ControlIP, doorControlEntity.ControlPort, doorControlEntity.StartDate, doorControlEntity.EndDate, "出");
                foreach (string item in participantsList)
                {
                    //标记变量，记录是否存在签退记录
                    bool isRepeatMark = false;
                    foreach (SignListEntity item2 in signOutList)
                    {
                        if (item == item2.KahaoOrMima)
                        {
                            isRepeatMark = true;
                            if (item2.Time >= Convert.ToDateTime(doorControlEntity.StartDate).AddMinutes(-10) && item2.Time <= Convert.ToDateTime(doorControlEntity.EndDate).AddMinutes(-30))
                            {
                                //记录不良信息，早退
                                BadRecordListEntity badRecordListEntity = new BadRecordListEntity();
                                badRecordListEntity.KahaoOrMima = item2.KahaoOrMima;
                                badRecordListEntity.ControlIP = doorControlEntity.ControlIP;
                                badRecordListEntity.ControlPort = doorControlEntity.ControlPort;
                                badRecordListEntity.SignListGuid = item2.Guid;
                                badRecordListEntity.Record = "会议早退，签退时间为：" + item2.Time;
                                badRecordListEntity.RecordTime = item2.Time;
                                //将不良信息写入数据库
                                InsertBadRecordInSQL(badRecordListEntity);
                            }
                            break;
                        }
                    }
                    if (isRepeatMark == false)
                    {
                        //记录不良信息，未签退
                        BadRecordListEntity badRecordListEntity = new BadRecordListEntity();
                        badRecordListEntity.KahaoOrMima = item;
                        badRecordListEntity.ControlIP = doorControlEntity.ControlIP;
                        badRecordListEntity.ControlPort = doorControlEntity.ControlPort;
                        badRecordListEntity.Record = "会议未签退";
                        badRecordListEntity.RecordTime = Convert.ToDateTime(doorControlEntity.EndDate);
                        //将不良信息写入数据库
                        InsertBadRecordInSQL(badRecordListEntity);
                    }

                }

            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }
        #endregion

        #region 查询指定门禁某一时间段内异常进门/出门的刷卡记录（未使用该方法）
        /// <summary>
        /// 查询指定门禁某一时间段内异常进门/出门的刷卡记录
        /// </summary>
        /// <param name="ControlIP">门禁IP</param>
        /// <param name="ControlPort">端口号</param>
        /// <param name="StartDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="JinChu">"进"/"出"门</param>
        /// <returns></returns>
        public List<SignListEntity> GetSingleDoorControlTimeSpanAbnormalSignList(string ControlIP, string ControlPort, string StartDate, string EndDate, string JinChu)
        {
            //执行返回标示
            List<SignListEntity> list = null;
            try
            {

                list = GetSingleDoorControlTimeSpanSignList(ControlIP, ControlPort, StartDate, EndDate, JinChu);

                //查询进门签到记录
                if (JinChu == "进")
                {
                    //获取相对应的实体类对象（签到迟到）
                    list = list.Where(item =>
                    item.Time > Convert.ToDateTime(StartDate).AddMinutes(30) && item.Time < Convert.ToDateTime(EndDate)).ToList<SignListEntity>();
                }
                //查询出门签退记录
                else if (JinChu == "出")
                {
                    //获取相对应的实体类对象（签退早退）
                    list = list.Where(item =>
                    item.Time > Convert.ToDateTime(StartDate) && item.Time < Convert.ToDateTime(EndDate).AddMinutes(-30)).ToList<SignListEntity>();
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
            return list;
        }
        #endregion

        #region 获取指定门禁某一时间段内签到/签退记录（排除重复刷卡：签到刷卡保留较早刷卡时间的记录，签退刷卡保留较晚刷卡时间的记录）
        /// <summary>
        /// 获取指定门禁某一时间段内签到/签退记录（排除重复刷卡：签到刷卡保留较早刷卡时间的记录，签退刷卡保留较晚刷卡时间的记录）
        /// </summary>
        /// <param name="ControlIP">门禁IP</param>
        /// <param name="ControlPort">端口号</param>
        /// <param name="StartDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="JinChu">进/出门标记（"进"/"出"）</param>
        /// <returns>签到/签退记录</returns>
        public List<SignListEntity> GetSingleDoorControlTimeSpanSignList(string ControlIP, string ControlPort, string StartDate, string EndDate, string JinChu)
        {
            //保存刷卡记录
            List<SignListEntity> list = null;
            //保存签到/签退记录
            List<SignListEntity> signList = null;
            try
            {
                //执行存储过程，查询指定门禁指定时间段进门或出门的刷卡记录，并标记已经查询过的数据，防止重复读取
                string sql = string.Format("Pro_ConditionalQuerySign_List");
                //创建参数数组
                SqlParameter[] sqlParameters ={
                                                 new SqlParameter("@ControlIP",ControlIP),
                                                 new SqlParameter("@ControlPort",ControlPort),
                                                 new SqlParameter("@StartDate",Convert.ToDateTime(StartDate).AddMinutes(-10)),
                                                 new SqlParameter("@EndDate",Convert.ToDateTime(EndDate).AddMinutes(5)),
                                                 new SqlParameter("@JinChu",JinChu)
                                             };
                //异常信息
                string error = string.Empty;
                //获取相对应的实体类对象（进/出门刷卡记录）
                list = DBHelper.ExcuteEntity<SignListEntity>(sql, System.Data.CommandType.StoredProcedure, out error, sqlParameters);
                //排除list为null的可能
                if (list != null)
                {
                    //查询进门签到记录
                    if (JinChu == "进")
                    {

                        //获取签到记录，排除重复刷卡，只保存较早刷卡时间的记录
                        if (list.Count > 0)
                        {
                            signList = new List<SignListEntity>();

                            foreach (SignListEntity item in list)
                            {
                                //标记变量，记录是否存在重复记录
                                bool isRepeatMark = false;
                                foreach (SignListEntity item2 in signList)
                                {
                                    //判断是否是重复刷卡的条件（卡号、门禁IP、端口号一致）
                                    if (item.KahaoOrMima == item2.KahaoOrMima && item.ControlIP == item2.ControlIP && item.ControlPort == item2.ControlPort)
                                    {
                                        //存在重复
                                        isRepeatMark = true;
                                        //不需要继续判断，退出循环，为了只记录时间较早的刷卡记录
                                        break;
                                    }
                                }
                                //如果不存在重复
                                if (isRepeatMark == false)
                                {
                                    //记录刷卡数据
                                    signList.Add(item);
                                }
                            }
                        }
                    }
                    //查询出门签退记录
                    else if (JinChu == "出")
                    {
                        //获取签退记录，排除重复刷卡，只保存较晚刷卡时间的记录
                        if (list.Count > 0)
                        {
                            signList = new List<SignListEntity>();

                            foreach (SignListEntity item in list)
                            {
                                //标记变量，记录是否存在重复记录
                                bool isRepeatMark = false;
                                for (int i = 0; i < signList.Count; i++)
                                {
                                    //判断是否是重复刷卡的条件（卡号、门禁IP、端口号一致）
                                    if (item.KahaoOrMima == signList[i].KahaoOrMima && item.ControlIP == signList[i].ControlIP && item.ControlPort == signList[i].ControlPort)
                                    {
                                        //存在重复
                                        isRepeatMark = true;
                                        //存在重复即替换，退出循环，为了只记录时间较晚的刷卡记录
                                        signList[i] = item;
                                        break;
                                    }
                                }
                                //如果不存在重复
                                if (isRepeatMark == false)
                                {
                                    //记录刷卡数据
                                    signList.Add(item);
                                }
                            }
                        }
                    }
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
            return signList;
        }

        #endregion

        #endregion

        #region 将不良记录写入数据库
        /// <summary>
        /// 将不良记录写入数据库
        /// </summary>
        /// <param name="badRecordListEntity">不良记录实体</param>
        /// <returns></returns>
        public bool InsertBadRecordInSQL(BadRecordListEntity badRecordListEntity)
        {
            //标记插入操作是否执行成功，默认插入不成功
            bool mark = false;
            try
            {
                string sql = string.Empty;
                string error = string.Empty;
                //手动录入的不良记录，不会传递刷卡记录的ID，不需要插入该列的值
                if (badRecordListEntity.SignListGuid == 0)
                {
                    sql = string.Format("insert into BadRecord_List(KahaoOrMima,ControlIP,ControlPort,Record,RecordTime,Operator)values('{0}','{1}','{2}','{3}','{4}','{5}')",
                        badRecordListEntity.KahaoOrMima, badRecordListEntity.ControlIP, badRecordListEntity.ControlPort, badRecordListEntity.Record, badRecordListEntity.RecordTime, badRecordListEntity.Operator);
                }
                else
                {
                    //自动产生的不良记录，不需要记录操作人的信息
                    sql = string.Format("insert into BadRecord_List(KahaoOrMima,ControlIP,ControlPort,SignListGuid,Record,RecordTime)values('{0}','{1}','{2}','{3}','{4}','{5}')",
                        badRecordListEntity.KahaoOrMima, badRecordListEntity.ControlIP, badRecordListEntity.ControlPort, badRecordListEntity.SignListGuid, badRecordListEntity.Record, badRecordListEntity.RecordTime);
                }
                DBHelper.Transaction(sql, out error);
                if (error == null)
                {
                    //插入操作执行成功
                    mark = true;
                }
                return mark;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
                mark = false;
                return mark;
            }
            finally
            {

            }
        }
        #endregion

        #endregion

        #region 黑名单

        #region 查询数据库黑名单列表
        /// <summary>
        /// 查询数据库黑名单列表
        /// </summary>
        /// <returns>黑名单列表list集合</returns>
        public List<BlackListEntity> GetBlackList()
        {
            List<BlackListEntity> list = new List<BlackListEntity>();
            try
            {
                //暂时使用文本命令
                string sql = "select * from Black_List where IsEffective='1'";
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<BlackListEntity>(sql, System.Data.CommandType.Text, out error);
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return list;
        }
        #endregion

        #region 查询数据库黑名单列表的JSON字符串
        /// <summary>
        /// 查询数据库黑名单列表的JSON字符串
        /// </summary>
        /// <returns>黑名单列表的JSON字符串</returns>
        [WebMethod]
        public string GetBlackListToJSON()
        {
            return Serialize(GetBlackList());
        }
        #endregion

        #region 查询卡号是否被列入黑名单
        /// <summary>
        /// 查询卡号是否被列入黑名单
        /// </summary>
        /// <param name="KahaoOrMima">卡号或密码</param>
        /// <returns>返回被查询的卡号是否已被列入黑名单（true：是，false：否）</returns>
        [WebMethod]
        public bool GetBlackListByKahaoOrMima(string KahaoOrMima)
        {
            List<BlackListEntity> list = new List<BlackListEntity>();
            //标记被查询的卡号是否被列入黑名单
            bool mark = false;
            try
            {
                //暂时使用文本命令
                string sql = string.Format("select * from Black_List where IsEffective='1' and KahaoOrMima='{0}'", KahaoOrMima);
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<BlackListEntity>(sql, System.Data.CommandType.Text, out error);
                //如果能够查到数据
                if (list.Count > 0)
                {
                    //标记变量标记查到了相应的数据
                    mark = true;
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return mark;
        }

        /// <summary>
        /// 查询多个卡号中被列入黑名单的卡号
        /// </summary>
        /// <param name="KahaoOrMimas">一个或多个卡号，中间用,分割</param>
        /// <returns>被列入黑名单的卡号</returns>
        [WebMethod]
        public string GetAnyBlackListByKahaoOrMimas(string KahaoOrMimas)
        {
            List<BlackListEntity> list = new List<BlackListEntity>();
            //标记被查询的卡号是否被列入黑名单
            bool mark = false;
            //已经被列入黑名单的卡号List
            List<string> blackKahaoOrMimaList = new List<string>();
            //已经被列入黑名单的卡号string
            string blackKahaoOrMima = "";
            try
            {

                //将卡号列表分割并重新以','分割拼接成以下格式：
                //"111','222','333','444"
                string KahaoOrMimaList = string.Join("','", KahaoOrMimas.Split(','));
                //select * from Black_List where IsEffective='1' and KahaoOrMima in ('2222222222','1','0987654321')
                string sql = string.Format("select * from Black_List where IsEffective='1' and KahaoOrMima in ('{0}')", KahaoOrMimaList);
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<BlackListEntity>(sql, System.Data.CommandType.Text, out error);

                //如果能够查到数据
                if (list.Count > 0)
                {
                    //遍历查询到的数据，收集卡号
                    foreach (BlackListEntity item in list)
                    {
                        blackKahaoOrMimaList.Add(item.KahaoOrMima);
                    }
                    //将卡号列表拼接成字符串
                    blackKahaoOrMima = string.Join(",", blackKahaoOrMimaList.ToArray());
                    //标记变量标记查到了相应的数据
                    mark = true;

                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return blackKahaoOrMima;
        }
        #endregion

        #region 删除过期黑名单
        /// <summary>
        /// 删除过期黑名单
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public bool DeleteExpiredBlackList()
        {
            //接口调用返回信息
            bool Successed = false;
            try
            {
                //查询过期黑名单
                List<BlackListEntity> expiredBlackLists = GetExpiredBlackList();
                if (expiredBlackLists != null)
                {
                    if (expiredBlackLists.Count == 0)
                    {
                        //不存在过期黑名单即不执行删除操作，也要标识执行成功
                        Successed = true;
                    }
                    else
                    {
                        foreach (BlackListEntity item in expiredBlackLists)
                        {
                            //根据卡号删除黑名单
                            Successed = DeleteBlackListByKahaoOrMima(item.KahaoOrMima);
                            //删除失败会回滚
                            //if(Successed==false)
                            //{
                            //    break;
                            //}
                        }
                    }
                }
                else
                {
                    //不存在过期黑名单即不执行删除操作，也要标识执行成功
                    Successed = true;
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
            return Successed;
        }
        #endregion

        #region 根据卡号删除黑名单
        /// <summary>
        /// 根据卡号删除黑名单
        /// </summary>
        /// <param name="KahaoOrMima">卡号或密码</param>
        /// <returns></returns>
        [WebMethod]
        public bool DeleteBlackListByKahaoOrMima(string KahaoOrMima)
        {
            //接口调用返回信息
            bool Successed = false;
            try
            {
                string sql = string.Empty;
                //设置标识IsEffective='0'，表示该条数据无效，等同于被删除，实际没有进行删除操作
                //存储过程名称（删除黑名单同步删除不良记录）
                sql = "DeleteBlackList";
                SqlParameter sqlParameter = new SqlParameter("KahaoOrMima", KahaoOrMima);
                //DBHelper.Transaction(sql, out error);
                int affectedLines = DBHelper.ExecuteNonQuery(CommandType.StoredProcedure, sql, sqlParameter);
                if (affectedLines >= 0)
                {
                    //更新操作执行成功
                    Successed = true;
                }
                return Successed;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return Successed;
        }
        #endregion

        #region 根据ID修改黑名单

        /// <summary>
        /// 根据ID修改黑名单
        /// </summary>
        /// <param name="Guid">ID</param>
        /// <param name="ValidDate">有效时间</param>
        /// <param name="Operator">操作人</param>
        /// <returns></returns>
        public bool UpdateBlackListByGuid(string Guid, string ValidDate, string Operator)
        {
            //标记更新操作是否执行成功，默认更新不成功
            bool mark = false;
            try
            {
                string sql = string.Empty;
                string error = string.Empty;

                DateTime dtValidDate = Convert.ToDateTime(ValidDate);
                //sql语句，根据ID修改黑名单的有效时间
                sql = string.Format("update Black_List set ValidDate='{0}',Operator='{1}',OperationTime=GETDATE() where Guid='{2}'", dtValidDate, Operator, Guid);
                DBHelper.Transaction(sql, out error);
                if (error == null)
                {
                    //更新操作执行成功
                    mark = true;
                }
                return mark;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
                mark = false;
                return mark;
            }
            finally
            {

            }
        }

        #endregion

        #region 查询过期黑名单
        /// <summary>
        /// 查询过期黑名单
        /// </summary>
        /// <returns></returns>
        public List<BlackListEntity> GetExpiredBlackList()
        {
            List<BlackListEntity> list = new List<BlackListEntity>();
            try
            {
                //暂时使用文本命令
                string sql = "select * from Black_List where IsEffective='1' and ValidDate<DATEADD(day,-1, GETDATE())";
                //异常信息
                string error = string.Empty;

                //获取相对应的实体类对象
                list = DBHelper.ExcuteEntity<BlackListEntity>(sql, System.Data.CommandType.Text, out error);
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return list;
        }
        #endregion

        #region 黑名单数据写入数据库
        /// <summary>
        /// 黑名单数据写入数据库
        /// </summary>
        /// <param name="blackList">黑名单list集合</param>
        /// <returns></returns>
        public bool InsertBlackListInSQL(List<BlackListEntity> blackList)
        {
            //标记变量，记录是否执行成功
            bool mark = false;
            try
            {
                //判断blackList中是否有数据
                if (blackList.Count > 0)
                {
                    //拼接SQL语句
                    //插入语句
                    StringBuilder sqlBuilderInsert = new StringBuilder();
                    //更新语句
                    StringBuilder sqlBuilderUpdate = new StringBuilder();
                    sqlBuilderInsert.Append("insert into Black_List (KahaoOrMima,ValidDate,Operator) values ");
                    sqlBuilderUpdate.Append("update BadRecord_List set IsEffective='0' where IsEffective='1' and  KahaoOrMima in ( ");
                    //遍历黑名单列表
                    foreach (BlackListEntity item in blackList)
                    {
                        sqlBuilderInsert.Append(string.Format(" ( '{0}','{1}','{2}' ), ", item.KahaoOrMima, item.ValidDate, item.Operator));
                        sqlBuilderUpdate.Append(string.Format("'{0}',", item.KahaoOrMima));
                    }
                    string sqlInsert = sqlBuilderInsert.ToString();
                    string sqlUpdate = sqlBuilderUpdate.ToString();
                    //去掉最后一个','
                    sqlInsert = sqlInsert.Substring(0, sqlInsert.LastIndexOf(','));
                    sqlUpdate = sqlUpdate.Substring(0, sqlUpdate.LastIndexOf(','));
                    sqlUpdate += " )";
                    //将两条SQL语句拼接成事务
                    StringBuilder sqlTransaction = new StringBuilder();
                    sqlTransaction.Append(@" begin transaction
                                            declare @errorSum int ");
                    //插入操作的SQL语句，写入黑名单数据库
                    sqlTransaction.Append(sqlInsert);
                    sqlTransaction.Append(@" set @errorSum=@errorSum+@@error ");
                    sqlTransaction.Append(sqlUpdate);
                    //更新操作的SQL语句，将写入黑名单的数据对应的不良记录标记为无效
                    sqlTransaction.Append(@" set @errorSum=@errorSum+@@error ");
                    sqlTransaction.Append(@" if @errorSum>0
                                                begin
                                                    rollback transaction
                                                end
                                            else
                                                begin
                                                    commit transaction
                                                end
                                            ");

                    string error = string.Empty;
                    //执行sql语句
                    DBHelper.Transaction(sqlTransaction.ToString(), out error);
                    if (string.IsNullOrEmpty(error))
                    {
                        //执行成功
                        mark = true;
                    }
                }
                else
                {
                    //不存在黑名单也要标识执行成功
                    mark = true;
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
                mark = false;
            }
            finally
            {

            }
            return mark;
        }
        #endregion

        #region 查询需要列入黑名单的数据列表，并写入数据库，有效时间已自动计算
        /// <summary>
        /// 查询需要列入黑名单的数据列表，并写入数据库，有效时间已自动计算
        /// </summary>
        /// <param name="SetPeriod">输出参数，统计不良记录产生黑名单的周期</param>
        /// <returns></returns>
        [WebMethod]
        public bool GetBlackListFromBadRecordListAndInsertIntoSQL(out int SetPeriod)
        {
            //接口调用返回信息
            List<BlackListEntity> list = new List<BlackListEntity>();
            //标记变量，记录是否执行成功
            bool mark = false;
            try
            {

                //统计不良记录达到规定的次数而产生黑名单
                int BadRecordCount = Constant.BadRecordCount;
                //黑名单禁用天数
                int BlackListValidDate = Constant.BlackListValidDate;
                //查询满足加入黑名单条件的不良记录数据
                string sql = "Pro_SelectAndUpdateBadRecord_List";
                //sql参数数组
                SqlParameter[] sqlParameters ={
                                                 new SqlParameter("@BadRecordCount",BadRecordCount),
                                                 new SqlParameter("@BlackListValidDate",BlackListValidDate)
                                             };
                //异常信息
                string error = string.Empty;
                //获取黑名单实体对象集合
                list = DBHelper.ExcuteEntity<BlackListEntity>(sql, System.Data.CommandType.StoredProcedure, out error, sqlParameters);
                if (string.IsNullOrEmpty(error))
                {
                    mark = true;
                    if (list != null)
                    {
                        //黑名单写入数据库
                        mark = InsertBlackListInSQL(list);
                    }
                }
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {
                //输出参数，统计不良记录产生黑名单的周期
                SetPeriod = Constant.SetPeriod;
            }
            return mark;
        }
        #endregion

        #region 手动录入黑名单
        /// <summary>
        /// 手动录入黑名单
        /// </summary>
        /// <param name="KahaoOrMima">卡号或密码，可以为1个或多个，中间用','分割</param>
        /// <param name="Time">过期时间</param>
        /// <param name="Operator">操作人</param>
        /// <returns>返回是否执行成功</returns>
        [WebMethod]
        public bool InsertOneBlackListByAdmin(string KahaoOrMima, string Time, string Operator)
        {
            //标记插入操作是否执行成功，默认插入不成功
            bool mark = false;
            try
            {
                //卡号或密码的数组
                string[] KahaoOrMimas = KahaoOrMima.Split(',');
                //黑名单实体集合
                List<BlackListEntity> blackList = new List<BlackListEntity>();
                foreach (string item in KahaoOrMimas)
                {
                    //创建不良记录实体
                    BlackListEntity blackListEntity = new BlackListEntity();
                    blackListEntity.KahaoOrMima = item;
                    blackListEntity.ValidDate = Convert.ToDateTime(Time);
                    //增加操作人
                    blackListEntity.Operator = Operator;
                    //将黑名单实体添加至list集合
                    blackList.Add(blackListEntity);
                }

                //插入操作执行成功
                mark = InsertBlackListInSQL(blackList);

            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return mark;
        }
        #endregion

        #endregion

        #endregion

        #region 【增加】将14位数字的字符串转换为日期格式
        public DateTime ConvertToDateTime(string strDateTime)
        {
            if (strDateTime.Length == 14)
            {
                StringBuilder stringBuilder = new StringBuilder();
                //截取年份
                stringBuilder.Append(strDateTime.Substring(0, 4));
                stringBuilder.Append("/");
                //截取月份
                stringBuilder.Append(strDateTime.Substring(4, 2));
                stringBuilder.Append("/");
                //截取天
                stringBuilder.Append(strDateTime.Substring(6, 2));
                stringBuilder.Append(" ");
                //截取小时
                stringBuilder.Append(strDateTime.Substring(8, 2));
                stringBuilder.Append(":");
                //截取分钟
                stringBuilder.Append(strDateTime.Substring(10, 2));
                stringBuilder.Append(":");
                //截取秒
                stringBuilder.Append(strDateTime.Substring(12, 2));
                return Convert.ToDateTime(stringBuilder.ToString());
            }
            return Convert.ToDateTime("0000/00/00 00:00:00");
        }
        #endregion

        #region 【增加】更新数据库卡号
        /// <summary>
        /// 更新数据库卡号
        /// </summary>
        /// <param name="oldCardNumber">旧卡号</param>
        /// <param name="newCardNumber">新卡号</param>
        /// <returns>返回执行是否成功</returns>
        [WebMethod]
        public bool UpdateCardNumber(string oldCardNumber, string newCardNumber)
        {
            //接口调用返回信息
            bool Successed = false;
            try
            {
                string sql = string.Empty;
                //存储过程名称（更新数据库所有表中需要更新的卡号为新卡号）
                sql = "Pro_UpdateCardNumber";
                SqlParameter sqlParameter1 = new SqlParameter("oldCardNumber", oldCardNumber);
                SqlParameter sqlParameter2 = new SqlParameter("newCardNumber", newCardNumber);
                //DBHelper.Transaction(sql, out error);
                int affectedLines = DBHelper.ExecuteNonQuery(CommandType.StoredProcedure, sql, sqlParameter1, sqlParameter2);
                if (affectedLines >= 0)
                {
                    //更新操作执行成功
                    Successed = true;
                }
                return Successed;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return Successed;
        }
        #endregion

        #region 【20151203增加】操作配置文件
        /// <summary>
        /// 从配置文件获取节点值，转换为JSON字符串
        /// </summary>
        /// <returns>返回配置文件的节点：值的JSON字符串</returns>
        [WebMethod]
        public string GetBlackListWebConfigInformation()
        {
            //创建配置文件实体
            WebConfigEntity webConfigEntity = new WebConfigEntity();
            try
            {
                //加载XML文件（web.config）
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory + "DoorControlEntityInformation.xml");
                //反射遍历类的属性
                foreach (PropertyInfo item in typeof(WebConfigEntity).GetProperties())
                {
                    XmlNode node = doc.SelectSingleNode(@"//add[@key='" + item.Name + "']");
                    XmlElement ele = (XmlElement)node;
                    //获取节点的value值，赋值给对象相应的属性
                    item.SetValue(webConfigEntity, ele.GetAttribute("value"), null);
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
            return Serialize(webConfigEntity);
        }
        /// <summary>
        /// 修改配置文件的value值
        /// </summary>
        /// <param name="strJSON">JSON字符串</param>
        [WebMethod]
        public bool SetBlackListWebConfigInformation(string strJSON)
        {
            bool Successed = false;
            try
            {
                //将JSON字符串转换为实体对象
                object obj = getObjectEntityByJson(strJSON);
                List<WebConfigEntity> webConfigEntity = getObjectEntityByJson(strJSON);
                //加载XML文件（web.config）
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory + "DoorControlEntityInformation.xml");
                foreach (PropertyInfo item in typeof(WebConfigEntity).GetProperties())
                {
                    XmlNode node = doc.SelectSingleNode(@"//add[@key='" + item.Name + "']");
                    XmlElement ele = (XmlElement)node;
                    ele.SetAttribute("value", item.GetValue(webConfigEntity[0], null).ToString());
                }
                doc.Save(AppDomain.CurrentDomain.BaseDirectory + "DoorControlEntityInformation.xml");
                Successed = true;
            }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return Successed;
        }
        
        public void UpdateConfig(string name, string Xvalue)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "DoorControlEntityInformation.xml");
            XmlNode node = doc.SelectSingleNode(@"//add[@key='" + name + "']");
            XmlElement ele = (XmlElement)node;
            ele.SetAttribute("value", Xvalue);
            doc.Save(AppDomain.CurrentDomain.BaseDirectory + "DoorControlEntityInformation.xml");
        }

        public static string GetConfig(string name)
        {
            try
            {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "DoorControlEntityInformation.xml");
            XmlNode node = doc.SelectSingleNode(@"//add[@key='" + name + "']");
            XmlElement ele = (XmlElement)node;
            return ele.GetAttribute("value");
             }
            catch (Exception ex)
            {
                //日志记录
                LogManage.WriteLog(typeof(DoorControl), ex);
            }
            finally
            {

            }
            return "";
        }
        #endregion

    }
}
