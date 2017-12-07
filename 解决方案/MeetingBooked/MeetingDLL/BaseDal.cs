
using MeetingModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MeetingDLL
{
    public class BaseDal<T> where T : class , new()
    {
        #region 增删改查的辅助方法
        /// <summary>
        /// 添加数据的辅助方法
        /// </summary>
        /// <param name="entity">要添加的实体对象</param>
        /// <param name="sql">返回添加的SQL语句</param>
        /// <returns>返回Parameter的集合</returns>
        public List<SqlParameter> DalAddHelp(T entity, out string sql)
        {
            sql = string.Empty;
            List<SqlParameter> pms = new List<SqlParameter>();
            try
            {
                if (entity != null)
                {
                    Type ty = entity.GetType();
                    StringBuilder strFirst = new StringBuilder();
                    StringBuilder strSecond = new StringBuilder();
                    PropertyInfo[] pros = ty.GetProperties();

                    for (int i = 0; i < pros.Count(); i++)
                    {
                        if (ty.GetProperties()[i].Name.ToUpper() != "ID" && pros[i].GetValue(entity, null) != null)
                        {
                            strFirst.Append(pros[i].Name + ",");
                            strSecond.Append("@" + pros[i].Name + ",");
                            SqlParameter para = new SqlParameter("@" + pros[i].Name, pros[i].GetValue(entity, null));
                            pms.Add(para);
                        }
                    }
                    sql = string.Format("insert into {0}({1}) values({2});select SCOPE_IDENTITY();", ty.Name, strFirst.ToString().TrimEnd(','), strSecond.ToString().TrimEnd(','));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return pms;
        }

        /// <summary>
        /// 更新数据的辅助方法
        /// </summary>
        /// <param name="entity">要更新的实体对象</param>
        /// <param name="sql">返回更新的SQL语句</param>
        /// <returns>返回Parameter的集合</returns>
        public List<SqlParameter> DalUpdateHelp(T entity, out string sql)
        {
            sql = string.Empty;
            List<SqlParameter> pms = new List<SqlParameter>();
            try
            {
                Type ty = entity.GetType();
                StringBuilder strFirst = new StringBuilder();

                PropertyInfo[] pros = ty.GetProperties();

                for (int i = 0; i < pros.Count(); i++)
                {
                    if (ty.GetProperties()[i].Name.ToUpper() != "ID" && pros[i].GetValue(entity, null) != null)
                    {
                        strFirst.Append(pros[i].Name + "=@" + pros[i].Name + ",");
                    }

                    if (pros[i].GetValue(entity, null) != null)
                    {
                        SqlParameter para = new SqlParameter("@" + pros[i].Name, pros[i].GetValue(entity, null));
                        pms.Add(para);
                    }


                }
                sql = string.Format("update {0} set {1} where id=@id", ty.Name, strFirst.ToString().TrimEnd(','));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return pms;
        }

        /// <summary>
        /// 删除数据的辅助方法
        /// </summary>
        /// <param name="pros">要删除对象的属性集合</param>
        /// <param name="entity">要删除的实体</param>
        /// <returns>返回实体属性和属性值得对应集合</returns>
        private Dictionary<string, object> DalDeleteHelp(PropertyInfo[] pros, T entity)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                foreach (PropertyInfo pro in pros)
                {
                    dic.Add(pro.Name, pro.GetValue(entity, null));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dic;
        }

        /// <summary>
        /// 批量删除数据的辅助方法
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="ids">要删除的对象Id集合</param>
        /// <returns></returns>
        public List<SqlParameter> DalDeleteBatchHelp(T entity, out string sql, params int[] ids)
        {
            sql = string.Empty;
            List<SqlParameter> pms = new List<SqlParameter>();
            try
            {
                StringBuilder strFirst = new StringBuilder();

                foreach (int item in ids)
                {
                    strFirst.Append("@id" + item.ToString() + ",");
                    pms.Add(new SqlParameter("@id" + item.ToString(), item));
                }
                sql = string.Format("delete from {0} where id in({1})", entity.GetType().Name, strFirst.ToString().TrimEnd(','));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return pms;
        }
        #endregion

        #region 根据id判读数据是否存在

        /// <summary>
        /// 根据id判读数据是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool CheckInfoById(T entity, int id)
        {
            bool result = false;
            try
            {
                string sql = "select * from " + entity + "where ID=" + id;
                result = SQLHelp.ExecuteNonQuery(sql, System.Data.CommandType.Text, null) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 添加单条数据

        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Add(T entity)
        {
            int result = -1;
            try
            {
                string sql = string.Empty;
                List<SqlParameter> pms = DalAddHelp(entity, out sql);
                object re = SQLHelp.ExecuteScalar(sql, System.Data.CommandType.Text, pms.ToArray());
                result = Convert.ToInt32(re);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;

        }

        #endregion

        #region 更新单挑数据

        /// <summary>
        /// 更新单挑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Update(T entity, SqlTransaction trans = null)
        {
            int result = -1;
            try
            {
                string sql = string.Empty;
                List<SqlParameter> pms = DalUpdateHelp(entity, out sql);
                result = Convert.ToInt32(SQLHelp.ExecuteScalar(sql, System.Data.CommandType.Text, pms.ToArray()));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;

        }

        #endregion

        #region 根据Id获取单条数据

        /// <summary>
        /// 根据Id获取单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetEntityById(T entity, int id)
        {
            try
            {
                string sql = string.Format("select * from {0} where Id=@id", entity.GetType().Name);
                SqlParameter pms = new SqlParameter("@id", id);
                SqlDataReader reader = SQLHelp.ExecuteReader(sql, CommandType.Text, pms);
                if (reader.HasRows)
                {
                    PropertyInfo[] pros = entity.GetType().GetProperties();
                    while (reader.Read())
                    {

                        foreach (PropertyInfo item in pros)
                        {
                            item.SetValue(entity, reader.IsDBNull(reader.GetOrdinal(item.Name)) ? null : reader[item.Name]);
                        }
                    }
                }
                else { entity = null; }
                return entity;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }

        }

        #endregion

        #region 根据某个字段获取数据

        /// <summary>
        /// 根据某个字段获取数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="filed"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual List<T> GetEntityListByField(T entity, string filed, string value)
        {
            List<T> tlist = new List<T>();
            try
            {
                string sql = string.Format("select * from {0} where {1}=@id", entity.GetType().Name, filed);
                SqlParameter pms = new SqlParameter("@id", value);
                SqlDataReader reader = SQLHelp.ExecuteReader(sql, CommandType.Text, pms);
                if (reader.HasRows)
                {
                    PropertyInfo[] pros = entity.GetType().GetProperties();
                    while (reader.Read())
                    {
                        T newentity = new T();
                        foreach (PropertyInfo item in pros)
                        {
                            if (string.IsNullOrEmpty(reader[item.Name].SafeToString()))
                            {
                                // item.SetValue(newentity, "");

                            }
                            else
                            {
                                item.SetValue(newentity, reader[item.Name]);
                            }
                        }
                        tlist.Add(newentity);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return tlist;
        }

        #endregion

        #region 伪删除单条数据

        /// <summary>
        /// 伪删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool DeleteFalse(T entity, int id)
        {
            return false;
        }

        #endregion

        #region 批量伪删除数据

        /// <summary>
        /// 批量伪删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual int DeleteBatchFalse(T entity, params int[] ids)
        {
            return 0;

        }

        #endregion

        #region 删除单条数据

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(T entity, int id)
        {
            bool result = false;
            try
            {
                string sql = string.Format("delete from {0} where id=@Id", entity.GetType().Name);
                SqlParameter pms = new SqlParameter("@Id", id);
                result = SQLHelp.ExecuteNonQuery(sql, CommandType.Text, pms) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }

        #endregion

        #region 批量删除数据

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual int DeleteBatch(T entity, params int[] ids)
        {
            int result = -1;
            try
            {
                string sql = string.Empty;

                List<SqlParameter> pms = DalDeleteBatchHelp(entity, out sql, ids);
                result = SQLHelp.ExecuteNonQuery(sql, CommandType.Text, pms.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }

        #endregion

        #region 根据条件获取所有数据

        /// <summary>
        /// 根据条件获取所有数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="where">条件（例如：id>1）</param>
        /// <param name="order">排序（例如：createtime desc）</param>
        /// <returns></returns>
        public virtual DataTable GetData(T entity, string where, string order)
        {
            DataTable dt = null;
            try
            {
                string sql = string.Format(@"select * from {0} ", entity.GetType().Name);
                StringBuilder strFirst = new StringBuilder();
                List<SqlParameter> pms = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(where))
                {
                    sql += string.Format(@" where {0}", where);
                }
                if (!string.IsNullOrEmpty(order))
                {
                    sql += string.Format(@" Order by {0}", order);
                }
                dt = SQLHelp.ExecuteDataTable(sql, CommandType.Text, null);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dt;

        }

        #endregion

        #region 获取数据（返回datatable）

        public virtual DataTable GetListByPage(Hashtable ht, string fileds, out int RowCount, bool IsPage = true, string Where = "")// string TableName, string strWhere, string orderby, int startIndex, int endIndex, SqlParameter[] parms4org)
        {
            RowCount = 0;
            SqlParameter[] parms4org = { };
            int StartIndex = 0;
            int EndIndex = 0;
            if (IsPage)
            {
                StartIndex = Convert.ToInt32(ht["StartIndex"].ToString());
                EndIndex = Convert.ToInt32(ht["EndIndex"].ToString());
            }
            try
            {
                string order = "";
                if (ht.ContainsKey("Order") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["Order"]))) order = ht["Order"].ToString();
                DataTable dt = SQLHelp.GetListByPage((string)ht["TableName"], fileds, Where, order, StartIndex, EndIndex, IsPage, parms4org, out RowCount);

                //DataTable dt = SQLHelp.GetListByPage(ht, parms4org);
                return dt;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                //写入日志
                //throw;
                return null;
            }

        }

        public virtual DataTable GetListByPage_1(Hashtable ht, string fileds, out int RowCount, bool IsPage = true, string Where = "")// string TableName, string strWhere, string orderby, int startIndex, int endIndex, SqlParameter[] parms4org)
        {
            RowCount = 0;
            SqlParameter[] parms4org = { };
            int StartIndex = 0;
            int EndIndex = 0;
            if (IsPage)
            {
                StartIndex = Convert.ToInt32(ht["StartIndex"].ToString());
                EndIndex = Convert.ToInt32(ht["EndIndex"].ToString());
            }
            try
            {
                string order = "";
                if (ht.ContainsKey("Order") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["Order"]))) order = ht["Order"].ToString();
                DataTable dt = SQLHelp.GetListByPage_1((string)ht["TableName"], fileds, Where, order, StartIndex, EndIndex, IsPage, parms4org, out RowCount);

                //DataTable dt = SQLHelp.GetListByPage(ht, parms4org);
                return dt;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                //写入日志
                //throw;
                return null;
            }

        }


        #endregion

        #region 联表查询并获取数据

        //public virtual DataTable GetDataByJoin(List<string> tables, List<string> Kcm)
        //{

        //    try
        //    {
        //        StringBuilder sbSql4org = new StringBuilder();

        //        sbSql4org.Append("select * from PortalTreeData where PId=@Id ");
        //        sbSql4org.Append("  Order by SortId desc,CreateTime asc");
        //        SqlParameter[] param = new SqlParameter[] {
        //        new SqlParameter("@Id",ht["Id"])
        //    };
        //        DataTable dt = SQLHelp.ExecuteDataTable(trans, sbSql4org.ToString(), CommandType.Text, param);
        //        return dt;
        //    }
        //    catch (Exception)
        //    {

        //        return null;
        //    }
        //}

        #endregion
    }
}
