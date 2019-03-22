using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.DAL
{
    public class BaseDAL<T> where T : class, new()
    {
        public const string cmdTextIsNError = "传入sql语句为空！";
       // private log4net.ILog log = log4net.LogManager.GetLogger("");
        #region 查询普通实现方案(基于Lambda表达式的Where查询)
        /// <summary>
        /// 获取所有Entity
        /// </summary>
        /// <param cookieName="exp">Lambda条件的where</param>
        /// <returns></returns>
        public virtual List<T> GetEntities(Func<T, bool> exp)//
        {

            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                try
                {
                    var data = Entities.Set<T>().Where(exp).ToList();
                    return data;
                }
                catch (Exception e)
                {
                    //log.Error(e.ToString());
                    throw e;
                }
            }

        }

        public virtual IEnumerable<U> GetEntities<U>(Func<T, bool> exp, Func<T, U> select)
        {

            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                var querys = Entities.Set<T>().Where(exp).Select(select);
                //string sql = (((System.Data.Objects.ObjectQuery)querys.AsQueryable()).ToTraceString());
                return querys.ToList();
            }

        }


        /// <summary>
        /// 计算总个数(分页)
        /// </summary>
        /// <param cookieName="exp">Lambda条件的where</param>
        /// <returns></returns>
        public virtual int GetEntitiesCount(Func<T, bool> exp)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                try
                {

                    return Entities.Set<T>().Count(exp);
                }
                catch (Exception e)
                {
                   // log.Error(e.ToString());
                    return 0;
                }
            }
        }
        /// <summary>
        /// 分页查询(Linq分页方式)
        /// </summary>
        /// <param cookieName="pageNumber">当前页</param>
        /// <param cookieName="pageSize">页码</param>
        /// <param cookieName="orderName">lambda排序名称</param>
        /// <param cookieName="sortOrder">排序(升序or降序)</param>
        /// <param cookieName="exp">lambda查询条件where</param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetEntitiesForPaging(int pageNumber, int pageSize, Func<T, string> orderName, string sortOrder, Func<T, bool> exp)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                if (sortOrder == "asc") //升序排列
                {
                    return Entities.Set<T>().Where(exp).OrderBy(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                    return Entities.Set<T>().Where(exp).OrderByDescending(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

        }
        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param cookieName="exp">lambda查询条件where</param>
        /// <returns></returns>
        public virtual T GetEntity(Func<T, bool> exp)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                var entity = Entities.Set<T>().Where(exp).SingleOrDefault();
                if (entity != null)
                {
                    Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Detached;
                }
                return entity;
            }
        }

        public virtual U GetEntity<U>(Func<T, bool> exp, Func<T, U> select)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                var entity = Entities.Set<T>().Where(exp).Select(select).SingleOrDefault();
                //if (entity != null)
                //{
                //    Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Detached;
                //}
                return entity;
            }
        }

        public virtual T GetLastOne(Func<T, bool> exp)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                var entity = Entities.Set<T>().Where(exp).LastOrDefault();
                if (entity != null)
                {
                    Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Detached;
                }
                return entity;
            }
        }
        #endregion

        #region 查询Entity To Sql语句外接接口的查询实现
        /// <summary>
        /// 获取所有Entity(立即执行请使用ToList()
        /// </summary>
        /// <param cookieName="commandText">Sql语句</param>
        /// <param cookieName="objParams">可变参数</param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetEntities(string commandText)
        {

            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                if (string.IsNullOrEmpty(commandText))
                {
                    throw new Exception(cmdTextIsNError);
                }


                return Entities.Database.SqlQuery<T>(commandText).ToList();
            }

        }
        public virtual IEnumerable<T> GetEntities(string commandText, params object[] parameters)
        {

            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                if (string.IsNullOrEmpty(commandText))
                {
                    throw new Exception(cmdTextIsNError);
                }
                var objectContext = ((IObjectContextAdapter)Entities).ObjectContext;
                return objectContext.ExecuteStoreQuery<T>(commandText, new object[] { }).ToList();

            }

        }

        /// <summary>
        /// 根据Sql语句返回ViewModel，返回结果为List<object>，需在返回后进行处理，适用于多表查询。
        /// </summary>
        /// <param cookieName="t">ViewModel的类型</param>
        /// <param cookieName="commandText">sql</param>
        /// <param cookieName="parameters">参数</param>
        /// <returns></returns>
        public virtual List<object> GetVMsBySql(Type t, string commandText, params object[] parameters)
        {
            var result = new List<object>();
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                if (string.IsNullOrEmpty(commandText))
                {
                    throw new Exception(cmdTextIsNError);
                }
                var quertResult = Entities.Database.SqlQuery(t, commandText, parameters).GetEnumerator();
                try
                {
                    while (quertResult.MoveNext())
                    {
                        var item = quertResult.Current;
                        result.Add(item);
                    }
                }
                catch (Exception e)
                {
                    //log.Error(e.ToString());
                    throw e;
                }

                return result;
            }

        }

        public virtual List<T> GetVMsBySql<T>(string commandText, params object[] parameters)
        {
            var result = new List<T>();
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                if (string.IsNullOrEmpty(commandText))
                {
                    throw new Exception(cmdTextIsNError);
                }
                var quertResult = Entities.Database.SqlQuery(typeof(T), commandText, parameters).GetEnumerator();
                try
                {
                    while (quertResult.MoveNext())
                    {
                        var item = quertResult.Current;
                        result.Add((T)item);
                    }
                }
                catch (Exception e)
                {
                    //log.Error(e.ToString());
                    throw e;
                }

                return result;
            }

        }
        /// <summary>
        /// 跟前Sql语句返回ViewModel集合，ViewModel必须继承自Model，并且只能获取Model有的字段，适用于单表查询
        /// </summary>
        /// <param cookieName="t">ViewModel的类型</param>
        /// <param cookieName="commandText">sql</param>
        /// <param cookieName="parameters">参数</param>
        /// <returns></returns>
        public List<T> GetVMExsBySql(Type t, string commandText, params object[] parameters)
        {
            var result = new List<T>();
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                if (string.IsNullOrEmpty(commandText))
                {
                    throw new Exception(cmdTextIsNError);
                }
                var quertResult = Entities.Database.SqlQuery(t, commandText, parameters).GetEnumerator();
                while (quertResult.MoveNext())
                {
                    var item = (T)quertResult.Current;
                    result.Add(item);
                }
                return result;
            }
        }

        /// <summary>
        /// 计算总个数(分页)
        /// </summary>
        /// <param cookieName="commandText">>Sql 语句</param>
        /// <returns></returns>
        public virtual int GetEntitiesCount(string commandText)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                if (string.IsNullOrEmpty(commandText))
                {
                    throw new Exception(cmdTextIsNError);
                }
                return Entities.Database.SqlQuery<T>(commandText).Count();
            }
        }

        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param cookieName="CommandText">Sql语句</param>
        /// <param cookieName="objParams">可变参数</param>
        /// <returns></returns>
        public virtual T GetEntity(string CommandText)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                var entity = Entities.Database.SqlQuery<T>("select * from " + typeof(T).Name + " where " + CommandText).SingleOrDefault();
                if (entity != null)
                {
                    Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Detached;
                }
                return entity;
            }
        }
        #endregion

        #region 增删改实现
        /// <summary>
        /// 插入Entity
        /// </summary>
        /// <param cookieName="model"></param>
        /// <returns></returns>
        public virtual bool Insert(T entity)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                try
                {
                    //var obj = Entities.Set<T>();
                    //obj.Attach(entity);
                    Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
                    return Entities.SaveChanges() > 0;
                }

                catch (DbEntityValidationException ee)
                {
                    //LogHelper.WriteLog("", ee);
                    //log.Error(ee.ToString());
                    System.Diagnostics.Debug.WriteLine(ee.EntityValidationErrors.ElementAt(0).ValidationErrors.ElementAt(0).ErrorMessage);
                    throw new Exception(ee.EntityValidationErrors.ElementAt(0).ValidationErrors.ElementAt(0).ErrorMessage.ToString());
                }

            }
        }
        /// <summary>
        /// 插入Entity
        /// </summary>
        /// <param cookieName="model"></param>
        /// <returns></returns>
        public virtual T InsertReturn(T entity)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                try
                {
                    Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
                    if (Entities.SaveChanges() > 0)
                    {
                        return entity;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    //log.Error(dbEx.ToString());
                    //LogHelper.WriteLog("", dbEx);
                    System.Diagnostics.Debug.WriteLine(dbEx.Message);
                    throw dbEx;

                }
                catch (Exception ee)
                {
                    //log.Error(ee.ToString());
                    //LogHelper.WriteLog("", ee);
                    System.Diagnostics.Debug.WriteLine(ee.Message);
                    throw ee;

                }

            }
        }
        /// <summary>
        /// 更新Entity(注意这里使用的傻瓜式更新,可能性能略低)
        /// </summary>
        /// <param cookieName="model"></param>
        /// <returns></returns>
        public virtual bool Update(T entity)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                try
                {
                    Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                    var result = Entities.SaveChanges();
                    return result > 0;
                }
                catch (DbEntityValidationException ee)
                {
                    //log.Error(ee.ToString());
                    throw ee;
                }
                catch (Exception e)
                {
                   // log.Error(e.ToString());
                    //LogHelper.WriteLog("", e);
                    throw e;
                }

            }

        }
        public virtual bool UpdateBatch(List<T> entitys)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                try
                {
                    foreach (var entity in entitys)
                    {
                        Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                        var result1 = Entities.SaveChanges();
                        if (result1 <= 0)
                        {
                            Entities.ChangeTracker.DetectChanges();
                            return false;
                        }
                    }
                    return true;
                }
                catch (DbEntityValidationException ee)
                {
                    //log.Error(ee.ToString());
                    //throw ee;
                    return false;
                }
                catch (Exception e)
                {
                    //log.Error(e.ToString());
                    //LogHelper.WriteLog("", e);
                    //throw e;
                    return false;
                }
            }

        }
        /// <summary>
        /// 更新Entity(注意这里使用的傻瓜式更新,可能性能略低)
        /// </summary>
        /// <param cookieName="model"></param>
        /// <returns></returns>
        public virtual T UpdateReturn(T entity)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    if (Entities.SaveChanges() > 0)
                    {
                        return entity;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception e)
                {
                    //log.Error(e.ToString());
                    //LogHelper.WriteLog("", e);
                    throw e;
                }

            }

        }
        /// <summary>
        /// 删除Entity
        /// </summary>
        /// <param cookieName="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(T entity)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                if (entity != null)
                {
                    Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
                    return Entities.SaveChanges() > 0;
                }
                return false;
            }
        }
        ///// <summary>
        ///// 删除实现 存储过程实现方式(调用spDelete+表名+ 主键ID)
        ///// </summary>
        ///// <param cookieName="ID">删除的主键</param>
        ///// <returns></returns>
        //public virtual bool Delete(object ID)
        //{
        //    using (ACE_MallEntities Entities = new ACE_MallEntities())
        //    {
        //        //存储过程实现方式(调用spDelete+表名+ 主键ID),存储过程命名为spDelete+表名的格式
        //        return Entities.Database.ex("spDelete" + typeof(T).Name + " " + ID.ToString()) > 0;

        //    }
        //}
        #endregion
        public virtual bool Delete(object ID)
        {
            throw new NotImplementedException();
        }

        public int ExcuteSqlCommand(string sql, params object[] parameters)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                try
                {
                    return Entities.Database.ExecuteSqlCommand(sql, parameters);
                }
                catch (Exception e)
                {
                    //log.Error(e.ToString());
                    //LogHelper.WriteLog("执行sql出错：" + sql, e);
                    throw e;
                }
            }
        }


        public object GetBySqlCommandSingle(Type t, string commandText, params object[] parameters)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                if (string.IsNullOrEmpty(commandText))
                {
                    throw new Exception(cmdTextIsNError);
                }
                try
                {
                    var quertResult = Entities.Database.SqlQuery(t, commandText, parameters).GetEnumerator();
                    quertResult.MoveNext();
                    var item = quertResult.Current;
                    return item;
                }
                catch (Exception e)
                {
                    //log.Error(e.ToString());
                    throw e;
                }
            }
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool InsertBatch(IEnumerable<T> entities)
        {
            using (ACE_MallEntities Entities = new ACE_MallEntities())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        Entities.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
                    }
                    return Entities.SaveChanges() > 0;
                }

                catch (DbEntityValidationException ee)
                {
                    //LogHelper.WriteLog("", ee);
                    throw new Exception(ee.EntityValidationErrors.ElementAt(0).ValidationErrors.ElementAt(0).ErrorMessage.ToString());
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示几条数据</param>
        /// <param name="totalCount">总共有几条数据</param>
        /// <param name="whereLambda">查询条件</param>
        /// <returns></returns>
        public IEnumerable<T> LoadPageEntities(int pageIndex, int pageSize, out int totalCount, Func<T, bool> whereLambda)
        {
            ACE_MallEntities Entities = new ACE_MallEntities();
            var temp = Entities.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            temp = temp.Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            return temp;
        }

    }
}

   

