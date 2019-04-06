using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class CategoryBLL : BaseBLL<Mall_Category>
    {
        /// <summary>
        /// 获取商品类别列表
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<Mall_Category> GetList(Func<Mall_Category, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        /// <summary>
        /// 添加商品类别
        /// </summary>
        /// <param  ="model">数据模型</param>
        /// <returns></returns>
        public bool Add(Mall_Category model)
        {
            return Dao.Insert(model);
        }
    }
}
