using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class BetweenBLL:BaseBLL<Adm_Between>
    {
        public List<Adm_Between> GetList(Func<Adm_Between, bool> exp)
        {

            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        /// <summary>
        /// 添加用户模型
        /// </summary>
        /// <param  ="model">数据模型</param>
        /// <returns></returns>
        public bool Add(Adm_Between model)
        {
            return Dao.Insert(model);
        }
        public new bool Update(Adm_Between model)
        {
            return Dao.Update(model);
        }
        #region 获取点击列表所在行的id
        public Adm_Between GetErrorSingle(int eid)
        {
            Adm_Between model = Dao.GetEntity(x => x.ID == eid);
            return model;
        }
        #endregion
        #region 删除
        public bool Delete(int eid)
        {
            BetweenBLL bll = new BetweenBLL();
            Adm_Between emodel = bll.GetErrorSingle(eid);
            bool result = true;
            if (emodel != null)
            {
                var model = bll.GetList(x => x.ID == eid).FirstOrDefault();
                result = Dao.Delete(model);
            }
            return result;
        }
        #endregion
    }
}
