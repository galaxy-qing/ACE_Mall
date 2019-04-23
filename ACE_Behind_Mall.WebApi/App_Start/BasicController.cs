using ACE_Mall.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ACE_Mall.BLL;
using System.Web.Security;

namespace ACE_Behind_Mall.WebApi.App_Start
{
    [AllowAnonymous]
    public class BasicController : ApiController
    {
        //protected int userId = Convert.ToInt32(HttpRuntime.Cache.Get("userid"));
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        //protected static int userId = GetTicket();
        UserBLL userbll = new UserBLL();
        //int aaa=ValidateTicket("111");
        public int GetTicket()
        {
            if (HttpContext.Current.Request.Headers["Authorization"] == ""|| HttpContext.Current.Request.Headers["Authorization"] == null)
            {
                return 0;
            }
           string ticket = HttpContext.Current.Request.Headers["Authorization"].Substring(10);
            //解密Ticket
            var strTicket = FormsAuthentication.Decrypt(ticket).UserData;
            //从Ticket里面获取用户名和密码
            var index = strTicket.IndexOf("&");
            string strUser = strTicket.Substring(0, index);
            string strPwd = strTicket.Substring(index + 1);
            var userList = userbll.GetList(x => (x.Account == strUser || x.Email == strUser) & x.Password == strPwd & x.IsDelete == 0);
            if (userList.Count() > 0)
            {
                int userid = userList.FirstOrDefault().ID;
                //HttpRuntime.Cache.Insert("userid", userid);
                return userid;
            }
            else
            {
                return 0;
            }
        }
    }
}
