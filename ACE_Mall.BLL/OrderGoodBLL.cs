using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class OrderGoodBLL:BaseBLL<My_Order_Good>
    {
        public List<My_Order_Good> GetList(Func<My_Order_Good, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        public bool Add(My_Order_Good model)
        {
            return Dao.Insert(model);
        }
    }
}
