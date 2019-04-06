using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class ShopCartBLL:BaseBLL<My_Shopcart>
    {
        public List<My_Shopcart> GetList(Func<My_Shopcart, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        public bool Add(My_Shopcart model)
        {
            return Dao.Insert(model);
        }
        public new bool Update(My_Shopcart model)
        {
            return Dao.Update(model);
        }
    }
}
