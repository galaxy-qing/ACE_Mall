using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
using ACE_Mall.BLL;
namespace ACE_Mall.Common
{
    public class GetUserData
    {
        AdmUserBLL admuserbll = new AdmUserBLL();
        public  void GetUserInfo(string account)
        {
            var usermodel = admuserbll.GetList(x=>x.Account==account).FirstOrDefault();
        }
    }
}
