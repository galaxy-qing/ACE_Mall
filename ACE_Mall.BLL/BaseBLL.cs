using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.DAL;
using ACE_Mall.Common;
namespace ACE_Mall.BLL
{
    public class BaseBLL<T> where T : class, new()
    {
        private BaseDAL<T> dao = null;
        public BaseDAL<T> Dao
        {
            get
            {
                if (dao == null)
                {
                    throw new Exception("DAO is not implament");
                }
                return dao;
            }
        }

        public BaseBLL()
        {
            dao = new BaseDAL<T>();
        }

        #region 增删改查
        public virtual int GetCount()
        {
            return Dao.GetEntitiesCount(x => { return true; });
        }
        public virtual int GetCount(Func<T, bool> exp)
        {
            return Dao.GetEntitiesCount(exp);
        }

        public virtual IEnumerable<T> GetEntities(string sql)
        {
            return Dao.GetEntities(sql);

        }
        public virtual IEnumerable<T> GetEntities(Func<T, bool> exp)
        {
            return Dao.GetEntities(exp);

        }
        public virtual IEnumerable<U> GetEntities<U>(Func<T, bool> exp, Func<T, U> select)
        {
            return Dao.GetEntities(exp, select);
        }

        public virtual T GetEntity(Func<T, bool> exp)
        {
            return Dao.GetEntity(exp);
        }

        public virtual T GetLastOne(Func<T, bool> exp)
        {
            return Dao.GetLastOne(exp);
        }
        public virtual bool Insert(T data)
        {
            try
            {
                return Dao.Insert(data);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public virtual T InsertReturn(T data)
        {
            try
            {
                return Dao.InsertReturn(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual bool Update(T data)
        {
            try
            {
                return Dao.Update(data);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public virtual T UpdateReturn(T data)
        {
            try
            {
                return Dao.UpdateReturn(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual bool Delete(T data)
        {
            try
            {
                return Dao.Delete(data);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public virtual bool Delete(Object ID)
        {
            try
            {
                return Dao.Delete(ID);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual IEnumerable<object> GetVMsBySql(Type t, string sql, params object[] parameters)
        {
            return dao.GetVMsBySql(t, sql, parameters);
        }
        public virtual IEnumerable<T> GetVMExsBySql(Type t, string sql, params object[] parameters)
        {
            return dao.GetVMExsBySql(t, sql, parameters);
        }

        public int ExcuteSqlCommand(string sql, params object[] parameters)
        {
            try
            {
                var result = Dao.ExcuteSqlCommand(sql, parameters);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public object GetBySqlCommandSingle(Type t, string commandText, params object[] parameters)
        {
            return dao.GetBySqlCommandSingle(t, commandText, parameters);
        }
        #endregion

        public bool InsertBatch(IEnumerable<T> entities)
        {
            return Dao.InsertBatch(entities);
        }
        public EF GetUpdateModel<EF>(EF model, string key) where EF : class, new()
        {
            Type type = typeof(EF);
            EF efmodel = (EF)Activator.CreateInstance(type);
            BaseBLL<EF> bll = new BaseBLL<EF>();
            PropertyInfo pro = type.GetProperty(key);
            int value = (int)pro.GetValue(model);
            efmodel = bll.Dao.GetEntity(u => { int p = (int)pro.GetValue(u); return p == value; });
            return GetModelHelper.GetModel<EF, EF>(model, efmodel);
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示几条数据</param>
        /// <param name="totalCount">数据总数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <returns></returns>
        public IEnumerable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Func<T, bool> whereLambda)
        {
            return Dao.LoadPageEntities(pageIndex, pageSize, out totalCount, whereLambda);
        }


    }

}





