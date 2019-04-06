using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class RoleBLL : BaseBLL<Adm_User_Role>
    {
        /// <summary>
        /// 获取后台职位列表
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<Adm_User_Role> GetList(Func<Adm_User_Role, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        /// <summary>
        /// 添加后台职位模型
        /// </summary>
        /// <param  ="model">数据模型</param>
        /// <returns></returns>
        public bool Add(Adm_User_Role model)
        {
            return Dao.Insert(model);
        }
        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public new bool Update(Adm_User_Role model)
        {
            return Dao.Update(model);
        }
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(int ID)
        {
            return Dao.Delete(ID);
        }
    }
}
