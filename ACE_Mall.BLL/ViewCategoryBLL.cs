using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class ViewCategoryBLL:BaseBLL<View_Category_Good_Order>
    {
        public List<View_Category_Good_Order> GetList(Func<View_Category_Good_Order, bool> exp)
        {

            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
    }
}
