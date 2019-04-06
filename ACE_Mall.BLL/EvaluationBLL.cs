using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class EvaluationBLL: BaseBLL<Mall_Good_Evaluation>
    {
        /// <summary>
        /// 获取商品评价
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<Mall_Good_Evaluation> GetList(Func<Mall_Good_Evaluation, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        /// <summary>
        /// 添加商品评价
        /// </summary>
        /// <param  ="model">数据模型</param>
        /// <returns></returns>
        public bool Add(Mall_Good_Evaluation model)
        {
            return Dao.Insert(model);
        }
        public new bool Update(Mall_Good_Evaluation model)
        {
            return Dao.Update(model);
        }
    }
}
