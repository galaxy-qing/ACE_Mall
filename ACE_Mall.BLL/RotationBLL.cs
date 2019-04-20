using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class RotationBLL:BaseBLL<Mall_Rotation>
    {
        public List<Mall_Rotation> GetList(Func<Mall_Rotation, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        /// <summary>
        /// 添加后台员工模型
        /// </summary>
        /// <param  ="model">数据模型</param>
        /// <returns></returns>
        public bool Add(Mall_Rotation model)
        {
            return Dao.Insert(model);
        }
        public new bool Update(Mall_Rotation model)
        {
            return Dao.Update(model);
        }
    }
}
