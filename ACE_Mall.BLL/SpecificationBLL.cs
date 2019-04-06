using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class SpecificationBLL : BaseBLL<Mall_Good_Specification>
    {
        /// <summary>
        /// 获取商品规格列表
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<Mall_Good_Specification> GetList(Func<Mall_Good_Specification, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        /// <summary>
        /// 添加商品规格
        /// </summary>
        /// <param  ="model">数据模型</param>
        /// <returns></returns>
        public bool Add(Mall_Good_Specification model)
        {
            return Dao.Insert(model);
        }
        /// <summary>
        /// 修改商品规格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public new bool Update(Mall_Good_Specification model)
        {
            return Dao.Update(model);
        }
    }
}
