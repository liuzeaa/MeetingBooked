using TLC_BLL;
using TLC_Model;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLC_BLL
{
    public abstract class BaseService<T> where T : class, new()
    {
        #region 字段

        public BaseDal<T> CurrentDal;//依赖抽象的接口。

        private T currentEntity;

        public T CurrentEntity
        {
            get { return new T(); }
        }

        #endregion

        #region 构造函数

        public BaseService()
        {
            try
            {
                //执行给当前CurrentDal赋值。
                //强迫子类给当前类的CurrentDal属性赋值。
                SetCurrentDal();//调用了一个抽象方法。
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 抽象方法

        //纯抽象方法：子类必须重写此方法。
        public abstract void SetCurrentDal();

        #endregion

        #region 添加一条数据

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual JsonModel Add(T entity)
        {
            JsonModel jsonModel = null;
            try
            {
                int result = CurrentDal.Add(entity);
                if (result > 0)
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = result
                    };
                }
                else
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 5,
                        errMsg = "名称重复",
                        retData = result
                    };
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        #endregion

        #region 更新单条数据

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual JsonModel Update(T entity, SqlTransaction trans = null)
        {
            JsonModel jsonModel = null;
            try
            {
                int result = CurrentDal.Update(entity, trans);

                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }

        }

        #endregion

        #region 伪删除单条数据

        /// <summary>
        /// 伪删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual JsonModel DeleteFalse(int id)
        {
            JsonModel jsonModel = null;
            try
            {
                bool result = CurrentDal.DeleteFalse(CurrentEntity, id);

                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }

        }

        #endregion

        #region 批量伪删除数据

        /// <summary>
        /// 批量伪删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual JsonModel DeleteBatchFalse(params int[] ids)
        {
            JsonModel jsonModel = null;
            try
            {
                int result = CurrentDal.DeleteBatchFalse(CurrentEntity, ids);

                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }

        }

        #endregion

        #region 删除单条数据

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual JsonModel Delete(int id)
        {
            JsonModel jsonModel = null;
            try
            {
                bool result = CurrentDal.Delete(CurrentEntity, id);

                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        #endregion

        #region 批量删除数据

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual JsonModel DeleteBatch(params int[] ids)
        {
            JsonModel jsonModel = null;
            try
            {
                int result = CurrentDal.DeleteBatch(CurrentEntity, ids);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
            //return CurrentDal.DeleteBatch(CurrentEntity, ids);

        }

        #endregion

        #region 根据ID获取信息

        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual JsonModel GetEntityById(int id)
        {
            JsonModel jsonModel = null;
            try
            {
                T entity = CurrentDal.GetEntityById(CurrentEntity, id);
                if (entity != null)
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = entity
                    };
                }
                else
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 999,
                        errMsg = "success",
                        retData = null
                    };
                }

                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        #endregion



        #region 根据某个字段获取信息

        /// <summary>
        /// 根据某个字段获取信息
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual JsonModel GetEntityListByField(string filed, string value)
        {
            JsonModel jsonModel = null;
            try
            {
                List<T> list = CurrentDal.GetEntityListByField(CurrentEntity, filed, value);
                if (list != null && list.Count == 0)
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 999,
                        errMsg = "无数据",
                        retData = list
                    };
                    return jsonModel;
                }
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = list
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        #endregion

        #region 指定页获取信息（根据条件)【返回的是jsmodel数据】

        /// <summary>
        /// 指定页获取信息（根据条件）
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="IsPage"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual JsonModel GetPage(Hashtable ht, string fileds, bool IsPage = true, string where = "")
        {
            int RowCount = 0;
            BLLCommon common = new BLLCommon();
            try
            {
                int PageIndex = 0;
                int PageSize = 0;
                if (IsPage)
                {
                    //增加起始条数、结束条数
                    ht = common.AddStartEndIndex(ht);
                    PageIndex = Convert.ToInt32(ht["PageIndex"]);
                    PageSize = Convert.ToInt32(ht["PageSize"]);
                }

                DataTable modList = CurrentDal.GetListByPage(ht, fileds, out RowCount, IsPage, where);

                //定义分页数据实体
                PagedDataModel<Dictionary<string, object>> pagedDataModel = null;
                //定义JSON标准格式实体中
                JsonModel jsonModel = null;
                if (modList == null || modList.Rows.Count <= 0)
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 999,
                        errMsg = "无数据",
                        retData = ""
                    };
                    return jsonModel;
                }
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                list = common.DataTableToList(modList);

                if (IsPage)
                {

                    //list.Add(common.DataTableToJson(modList));
                    //总条数
                    //int RowCount = modList.Rows.Count;// CurrentDal.GetRecordCount(ht, null);

                    //总页数
                    int PageCount = (int)Math.Ceiling(RowCount * 1.0 / PageSize);
                    //将数据封装到PagedDataModel分页数据实体中
                    pagedDataModel = new PagedDataModel<Dictionary<string, object>>()
                    {
                        PageCount = PageCount,
                        PagedData = list,
                        PageIndex = PageIndex,
                        PageSize = PageSize,
                        RowCount = RowCount
                    };
                    //将分页数据实体封装到JSON标准实体中
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = pagedDataModel
                    };
                }
                else
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = list
                    };
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                JsonModel jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }


        public virtual JsonModel GetPage_1(Hashtable ht, string fileds, bool IsPage = true, string where = "")
        {
            int RowCount = 0;
            BLLCommon common = new BLLCommon();
            try
            {
                int PageIndex = 0;
                int PageSize = 0;
                if (IsPage)
                {
                    //增加起始条数、结束条数
                    ht = common.AddStartEndIndex(ht);
                    PageIndex = Convert.ToInt32(ht["PageIndex"]);
                    PageSize = Convert.ToInt32(ht["PageSize"]);
                }

                DataTable modList = CurrentDal.GetListByPage_1(ht, fileds, out RowCount, IsPage, where);

                //定义分页数据实体
                PagedDataModel<Dictionary<string, object>> pagedDataModel = null;
                //定义JSON标准格式实体中
                JsonModel jsonModel = null;
                if (modList == null || modList.Rows.Count <= 0)
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 999,
                        errMsg = "无数据",
                        retData = ""
                    };
                    return jsonModel;
                }
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                list = common.DataTableToList(modList);

                if (IsPage)
                {

                    //list.Add(common.DataTableToJson(modList));
                    //总条数
                    //int RowCount = modList.Rows.Count;// CurrentDal.GetRecordCount(ht, null);

                    //总页数
                    int PageCount = (int)Math.Ceiling(RowCount * 1.0 / PageSize);
                    //将数据封装到PagedDataModel分页数据实体中
                    pagedDataModel = new PagedDataModel<Dictionary<string, object>>()
                    {
                        PageCount = PageCount,
                        PagedData = list,
                        PageIndex = PageIndex,
                        PageSize = PageSize,
                        RowCount = RowCount
                    };
                    //将分页数据实体封装到JSON标准实体中
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = pagedDataModel
                    };
                }
                else
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = list
                    };
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                JsonModel jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        #endregion

        #region 指定页获取信息（根据条件)【返回的是Dataset数据】

        /// <summary>
        /// 指定页获取信息（根据条件)【返回的是Dataset数据】
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="IsPage"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual DataTable GetData(Hashtable ht, bool IsPage = true, string where = "")
        {
            DataTable modList = null;
            int RowCount = 0;
            BLLCommon common = new BLLCommon();
            try
            {
                modList = new DataTable();
                int PageIndex = 0;
                int PageSize = 0;
                if (IsPage)
                {
                    //增加起始条数、结束条数
                    ht = common.AddStartEndIndex(ht);
                    PageIndex = Convert.ToInt32(ht["PageIndex"]);
                    PageSize = Convert.ToInt32(ht["PageSize"]);
                }

                modList = CurrentDal.GetListByPage(ht, "*", out RowCount, IsPage, where);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                JsonModel jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
            }
            return modList;
        }

        #endregion

        #region 判断是否含有某条数据

        /// <summary>
        /// 判断是否含有某条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool CheckInfoById(int id)
        {
            bool Flag = false;
            try
            {
                Flag = CurrentDal.CheckInfoById(currentEntity, id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Flag;
        }

        #endregion
    }
}


