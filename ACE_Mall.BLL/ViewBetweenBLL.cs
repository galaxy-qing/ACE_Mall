using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class ViewBetweenBLL : BaseBLL<View_Between>
    {
        public List<View_Between> GetList(Func<View_Between, bool> exp)
        {

            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
    }
}
