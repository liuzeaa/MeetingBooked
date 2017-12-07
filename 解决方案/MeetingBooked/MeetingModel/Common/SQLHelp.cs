using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MeetingModel
{
    public static class SQLHelp
    {
        private static readonly string conStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
       
        #region 返回执行增加、删除、修改操作后造成影响的行数

        /// <summary>
        /// 返回执行增加、删除、修改操作后造成影响的行数
        /// </summary>
        /// <param name="sql">要执行的Sql语句</param>
        /// <param name="cmdType">要执行的命令类型</param>
        /// <param name="pms">传入的参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            int result = -1;
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.CommandType = cmdType;
                        if (pms != null)
                        {
                            cmd.Parameters.AddRange(pms);
                        }
                        con.Open();
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }

        #endregion

        #region 返回数据库查询结果首行首列的值

        /// <summary>
        /// 返回数据库查询结果首行首列的值
        /// </summary>
        /// <param name="sql">要执行的Sql语句</param>
        /// <param name="cmdType">要执行的命令类型</param>
        /// <param name="pms">传入的参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            object obj = null;
            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.CommandType = cmdType;
                            if (pms != null)
                            {
                                cmd.Parameters.AddRange(pms);
                            }
                            con.Open();

                            obj = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return obj;
        }

        #endregion

        #region 返回SqlDataReader对象

        /// <summary>
        /// 返回SqlDataReader对象
        /// </summary>
        /// <param name="sql">要执行的Sql语句</param>
        /// <param name="cmdType">要执行的命令类型</param>
        /// <param name="pms">传入的参数</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            SqlDataReader reader = null;
            SqlConnection con = new SqlConnection(conStr);
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = cmdType;
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                try
                {
                    con.Open();
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    con.Close();
                    con.Dispose();

                }
            }
            return reader;
        }

        #endregion

        #region 封装一个返回DataTable对象的方法

        /// <summary>
        /// 封装一个返回DataTable对象的方法
        /// </summary>
        /// <param name="sql">要执行的Sql语句</param>
        /// <param name="cmdType">要执行的命令类型</param>
        /// <param name="pms">传入的参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            try
            {
                DataSet dbs = new DataSet();
                using (SqlConnection SqlConn = new SqlConnection(conStr))
                {
                    using (SqlCommand command = new SqlCommand(sql, SqlConn))
                    {
                        //command.CommandType = CommandType.Text;
                        command.CommandType = cmdType;
                        if (pms != null)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddRange(pms);
                        }
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = command;
                        sda.Fill(dbs, "data");
                    }
                }
                return dbs.Tables[0];
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        #endregion

        #region 封装一个返回DataTable对象的方法

        /// <summary>
        /// 封装一个返回DataTable对象的方法
        /// </summary>
        /// <param name="sql">要执行的Sql语句</param>
        /// <param name="cmdType">要执行的命令类型</param>
        /// <param name="pms">传入的参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(SqlTransaction trans, string cmdText, CommandType cmdType, params SqlParameter[] pms)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, pms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "ds");
                cmd.Parameters.Clear();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前查询页</param>
        /// <param name="pagesize">每页记录集数量</param>
        /// <param name="fdname">用于定位记录的主键(惟一键)字段,可以是逗号分隔的多个字段</param>
        /// <param name="filedName">以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段</param>
        /// <param name="TableName">查询表名</param>
        /// <param name="WhereStr">条件语句(可为空)</param>
        /// <param name="OrderByStr">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序(可为空)</param>
        /// <param name="pageCount">输出总页数</param>
        /// <param name="recordcount">记录集总数</param>
        /// <returns></returns>
        public static DataTable pageSearch(int pageIndex, int pagesize, string fdname, string filedName, string TableName, string WhereStr, string OrderByStr, out int pageCount, out int recordcount)
        {
            DataTable dt = null;
            pageCount = 0;
            recordcount = 0;
            try
            {
                DataSet dbs = new DataSet();
                using (SqlConnection SqlConn = new SqlConnection(conStr))
                {
                    if (SqlConn.State != ConnectionState.Open) { SqlConn.Open(); }
                    if (pageIndex <= 0)
                    {
                        pageIndex = 1;
                    }
                    using (SqlCommand command = new SqlCommand("proc_ShowPages", SqlConn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@PageIndex", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@FieldKey", SqlDbType.VarChar, 8000));
                        command.Parameters.Add(new SqlParameter("@FieldShow", SqlDbType.VarChar, 8000));
                        command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.VarChar, 8000));
                        if (!string.IsNullOrEmpty(WhereStr))
                        {
                            command.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar, 8000));
                        } if (!string.IsNullOrEmpty(OrderByStr))
                        {
                            command.Parameters.Add(new SqlParameter("@FieldOrder", SqlDbType.VarChar, 8000));
                        }
                        command.Parameters.Add(new SqlParameter("@TotalCount", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@TotalPageCount", SqlDbType.Int));
                        command.UpdatedRowSource = UpdateRowSource.None;
                        command.Parameters["@PageSize"].Value = pagesize;
                        command.Parameters["@PageIndex"].Value = pageIndex;
                        command.Parameters["@FieldKey"].Value = fdname;
                        command.Parameters["@FieldShow"].Value = filedName;
                        command.Parameters["@TableName"].Value = TableName;
                        if (!string.IsNullOrEmpty(WhereStr))
                        {
                            command.Parameters["@Where"].Value = WhereStr;
                        } if (!string.IsNullOrEmpty(OrderByStr))
                        {
                            command.Parameters["@FieldOrder"].Value = OrderByStr;
                        }
                        command.Parameters["@TotalCount"].Direction = ParameterDirection.Output;
                        command.Parameters["@TotalPageCount"].Direction = ParameterDirection.Output;
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = command;
                        sda.SelectCommand.ExecuteNonQuery();
                        recordcount = int.Parse(sda.SelectCommand.Parameters["@TotalCount"].Value.ToString());
                        pageCount = int.Parse(sda.SelectCommand.Parameters["@TotalPageCount"].Value.ToString());
                        sda.Fill(dbs, "data");
                    }
                }
                dt = dbs.Tables[0];
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dt;
        }

        #endregion

        #region 分页
        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>记录总数</returns>
        public static int GetRecordCount(string TableName, string strWhere, SqlParameter[] parms4org)
        {
            int result = 0;
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select count(1) FROM " + TableName + " T");
                if (strWhere.Trim() != "")
                {
                    sbSql.Append(" where 1=1" + strWhere);
                }

                object obj = ExecuteScalar(sbSql.ToString(), CommandType.Text, parms4org);

                if (obj == null)
                {
                    result = 0;
                }
                else
                {
                    result = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        public static int GetRecordCount_1(string TableName, string strWhere, SqlParameter[] parms4org)
        {
            int result = 0;
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select count(*) from");
                sbSql.Append("(select s_users FROM " + TableName + " T");
                if (strWhere.Trim() != "")
                {
                    sbSql.Append(" where 1=1" + strWhere);
                }
                sbSql.Append(") as TT");
                object obj = ExecuteScalar(sbSql.ToString(), CommandType.Text, parms4org);

                if (obj == null)
                {
                    result = 0;
                }
                else
                {
                    result = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        ///  <param name="fileds">字段</param>
        /// <param name="strWhere">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="startIndex">起始行数</param>
        /// <param name="endIndex">结束行数</param>
        /// <returns>DataSet</returns>
        public static DataTable GetListByPage(string TableName, string fileds, string strWhere, string orderby, int startIndex, int endIndex, bool IsPage, SqlParameter[] parms4org, out int RowCount, string FiledsName = "*")
        {
            DataTable dt = null;
            RowCount = 0;
            try
            {
                RowCount = 0;
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT " + fileds + " FROM ( ");
                sbSql.Append(" SELECT ROW_NUMBER() OVER (");
                if (!string.IsNullOrEmpty(orderby.Trim()))
                {
                    //sbSql.Append("order by T." + orderby);
                    sbSql.Append("order by " + orderby);
                }
                else
                {
                    sbSql.Append("order by T.id desc");
                }
                sbSql.Append(")AS rowNum, T." + FiledsName + "  from " + TableName + " T ");
                if (!string.IsNullOrEmpty(strWhere.Trim()))
                {
                    sbSql.Append(" WHERE 1=1 " + strWhere);
                }
                sbSql.Append(" ) TT");
                if (IsPage)
                {
                    RowCount = GetRecordCount(TableName, strWhere, parms4org);
                    sbSql.AppendFormat(" WHERE TT.rowNum between {0} and {1}", startIndex, endIndex);
                }
                dt = ExecuteDataTable(sbSql.ToString(), CommandType.Text, parms4org);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dt;
        }

        public static DataTable GetListByPage_1(string TableName, string fileds, string strWhere, string orderby, int startIndex, int endIndex, bool IsPage, SqlParameter[] parms4org, out int RowCount, string FiledsName = "*")
        {
            DataTable dt = null;
            RowCount = 0;
            try
            {

                RowCount = 0;
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT " + FiledsName + " FROM ( ");
                sbSql.Append(" SELECT ROW_NUMBER() OVER (");
                if (!string.IsNullOrEmpty(orderby.Trim()))
                {
                    //sbSql.Append("order by T." + orderby);
                    sbSql.Append("order by " + orderby);
                }
                else
                {
                    sbSql.Append("order by T.id desc");
                }
                sbSql.Append(")AS rowNum, T." + fileds + "  from " + TableName + " T ");
                if (!string.IsNullOrEmpty(strWhere.Trim()))
                {
                    sbSql.Append(" WHERE 1=1 " + strWhere);
                }
                sbSql.Append(" ) TT");
                if (IsPage)
                {
                    RowCount = GetRecordCount_1(TableName, strWhere, parms4org);
                    sbSql.AppendFormat(" WHERE TT.rowNum between {0} and {1}", startIndex, endIndex);
                }
                dt = ExecuteDataTable(sbSql.ToString(), CommandType.Text, parms4org);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dt;
        }
        public static DataTable GetList(string TableName, SqlParameter[] parms4org, string Where = "")
        {
            DataTable dt = null;
            try
            {
                dt = ExecuteDataTable(TableName, CommandType.Text, parms4org);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dt;
        }
        public static DataTable GetListrandom(string TableName, SqlParameter[] parms4org, string Where = "")
        {
            DataTable dt = null;
            try
            {
                return ExecuteDataTable(TableName, CommandType.Text, parms4org);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dt;
        }
        public static DataTable GetListrandomone(string TableName, SqlParameter[] parms4org, string Where = "")
        {
            DataTable dt = null;
            try
            {
                dt = ExecuteDataTable(TableName, CommandType.Text, parms4org);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dt;
        }

        /// <summary>
        /// 在事务中执行查询，返回DataSet
        /// </summary>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            int val = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return val;
        }

        public static SqlTransaction BeginTransaction()
        {
            SqlTransaction tran = null;
            try
            {
                SqlConnection myConnection = new SqlConnection(conStr);
                myConnection.Open();
                tran = myConnection.BeginTransaction();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return tran;
        }

        /// <summary>
        /// 生成要执行的命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            try
            {
                // 如果存在参数，则表示用户是用参数形式的SQL语句，可以替换
                if (cmdParms != null && cmdParms.Length > 0)
                    cmdText = cmdText.Replace("?", "@").Replace(":", "@");

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        // 如果存在参数，则表示用户是用参数形式的SQL语句，可以替换
                        parm.ParameterName = parm.ParameterName.Replace("?", "@").Replace(":", "@");
                        if (parm.Value == null)
                            parm.Value = DBNull.Value;
                        cmd.Parameters.Add(parm);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        #region 获取数据库数据【查】【修改：将参数中object[]改为SqlParameter[]】

        /// <summary>
        /// 执行查询操作（数据库表单实体化）
        /// </summary>
        /// <typeparam name="T">指定映射的类型</typeparam>
        /// <param name="commandText">执行命令【文本】</param>
        /// <param name="commandtype">执行类型</param>
        /// <param name="errorString">异常记录</param>
        /// <param name="args">其他参数【数据库】</param>
        /// <returns>返回值</returns>
        public static List<T> ExcuteEntity<T>(string commandText, CommandType commandtype, out string errorString, params SqlParameter[] args)
        {
            //该类型实例的集合
            List<T> tList = new List<T>();
            //定义一个连接
            SqlConnection con = null;
            //异常信息
            errorString = null;
            try
            {
                con = new SqlConnection(conStr);

                //创建数据库命令对象
                using (SqlCommand cmd = new SqlCommand(commandText, con))
                {
                    //命令类型
                    cmd.CommandType = commandtype;
                    //添加数据库参数
                    cmd.Parameters.AddRange(args);//存在错误！！！！！已修改
                    //打开数据库连接
                    con.Open();
                    //创建读取器（只进只读对象）
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        //读取数据库记录【只读、只进不退】
                        while (reader.Read())
                        {
                            //执行命令
                            T objd = ExcuteReader<T>(reader);
                            //添加单条记录
                            tList.Add(objd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                //获取异常信息
                errorString = ex.Message;

            }
            finally
            {
                if (con != null)
                {
                    //释放连接
                    con.Close();
                }
            }

            return tList;
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">执行类型</typeparam>
        /// <param name="reader">数据库读取器</param>
        /// <returns>返回值</returns>
        static T ExcuteReader<T>(SqlDataReader reader)
        {
            //定义默认类型对象
            T obj = default(T);
            //获取对象的类型
            Type type = typeof(T);
            //获取该类的属性集
            PropertyInfo[] propertyInfos = type.GetProperties();
            //创建实例
            obj = Activator.CreateInstance<T>();
            //获取字段数量
            int fieldsCount = reader.FieldCount;
            //遍历属性值
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                //获取当前属性的名称
                string propertyInfoName = propertyInfo.Name;
                //遍历字段
                for (int i = 0; i < fieldsCount; i++)
                {
                    //读取数据库的字段名称
                    string fieldName = reader.GetName(i);
                    //对比是否和实体属性的名称相对应
                    if (string.Compare(propertyInfoName, fieldName, true) == 0)
                    {
                        //该字段的值【数据库】
                        object objec = reader.GetValue(i);
                        try
                        {
                            //为空判断
                            if (propertyInfo.PropertyType == typeof(string) && objec == DBNull.Value)
                            {
                                objec = null;
                            }
                            //给该字段设置值
                            propertyInfo.SetValue(obj, objec, null);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                        }
                        break;
                    }
                }
            }
            return obj;
        }

        #endregion
    }
}
