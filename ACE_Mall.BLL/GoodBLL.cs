using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;

namespace ACE_Mall.BLL
{
    public class GoodBLL : BaseBLL<Mall_Good>
    {
        /// <summary>
        /// 获取后台职位列表
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<Mall_Good> GetList(Func<Mall_Good, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        public bool Add(Mall_Good model)
        {
            return Dao.Insert(model);
        }
    }
}