using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class OrderBLL:BaseBLL<My_Order>
    {
        public List<My_Order> GetList(Func<My_Order, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        public bool Add(My_Order model)
        {
            return Dao.Insert(model);
        }
    }
}
