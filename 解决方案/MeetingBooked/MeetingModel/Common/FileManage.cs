
using MeetingModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace MeetingModel.Common
{
    public class FileManage
    {
        #region 序列化将某个对象存储到文件

        /// <summary>
        /// 序列化将某个对象存储到文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        public static void Save_Entity(Object obj, string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.Create(filePath);
                }
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, obj);
                    fileStream.Flush();
                }
            }

            catch (Exception ex)
            {
                LogHelper.Error(ex);              
            }

        }

        #endregion

        #region 将某个文件反序列化还原成实体对象

        /// <summary>
        /// 将某个文件反序列化还原成实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Load_Entity<T>(string filePath)
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.Create(filePath);
                }
                else
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        if (fileStream.Length > 0L)
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            object obj = binaryFormatter.Deserialize(fileStream);
                            if (obj is T)
                            {
                                entity = (T)obj;
                            }
                        }
                        else
                        {
                            entity = Activator.CreateInstance<T>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return entity;
        }

        #endregion


        #region 序列化将某个对象存储到文件(xml方式)

        /// <summary>
        /// 序列化将某个对象存储到文件(xml方式)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        public static void Save_EntityInXml(Object obj, string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.Create(filePath);
                }
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    XmlSerializer binaryFormatter = new XmlSerializer(obj.GetType());
                    binaryFormatter.Serialize(fileStream, obj);
                    fileStream.Flush();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 将某个文件反序列化还原成实体对象（xml方式）

        public static T Load_EntityInXml<T>(string filePath)
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.Create(filePath);
                }
                else
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        if (fileStream.Length > 0L)
                        {
                            XmlSerializer binaryFormatter = new XmlSerializer(typeof(T));
                            object obj = binaryFormatter.Deserialize(fileStream);
                            if (obj is T)
                            {
                                entity = (T)obj;
                            }
                        }
                        else
                        {
                            entity = Activator.CreateInstance<T>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return entity;
        }

        #endregion

        #region 拷贝文件

        /// <summary>
        /// 确保文件存在系统目录
        /// </summary>
        public static void CheckDebugHasTheFile(string FileName, string sourceFileRoot)
        {
            try
            {
                //paintFile所要生成的文件（本系统输出目录）
                var file = Environment.CurrentDirectory + "\\" + FileName;

                //确保该文件在应用程序启动之后存在（参数设置需要使用该dll文件）
                if (!System.IO.File.Exists(file))
                {
                    //paintFile所备份的文件
                    var file2 = sourceFileRoot + "\\" + FileName;
                    //判断是否需要拷贝文件
                    if (System.IO.File.Exists(file2))
                    {
                        //文件拷贝
                        System.IO.File.Copy(file2, file);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {

            }
        }

        #endregion          


    }
}
