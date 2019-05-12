using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class PageBLL:BaseBLL<Adm_Page>
    {
        public List<Adm_Page> GetList(Func<Adm_Page, bool> exp)
        {

            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
    }
}
