using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class AdmUserBLL:BaseBLL<Adm_User>
    {
        /// <summary>
        /// 获取后台管理人员列表
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<Adm_User> GetList(Func<Adm_User, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        /// <summary>
        /// 添加后台员工模型
        /// </summary>
        /// <param  ="model">数据模型</param>
        /// <returns></returns>
        public bool Add(Adm_User model)
        {
            return Dao.Insert(model);
        }
        public new  bool Update(Adm_User model)
        {
            return Dao.Update(model);
        }
    }
}
