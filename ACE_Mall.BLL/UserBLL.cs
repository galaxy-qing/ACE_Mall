using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class UserBLL : BaseBLL<My_Data>
    {
        public List<My_Data> GetList(Func<My_Data, bool> exp)
        {
            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
    }
}
