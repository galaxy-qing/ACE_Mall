using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class ModuleBLL:BaseBLL<Adm_Module>
    {
        public List<Adm_Module> GetList(Func<Adm_Module, bool> exp)
        {

            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
    }
}
