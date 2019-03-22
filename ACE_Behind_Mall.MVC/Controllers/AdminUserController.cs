using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using NLog.Fluent;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class AdminUserController : Controller
    {
        protected ModelResponse<dynamic> mr = new ACE_Mall.Common.ModelResponse<dynamic>();
        AdmUserBLL admuserbll = new AdmUserBLL();
        // GET: AdminUser
        public ActionResult AdmUserList()
        {

            return View();
        }
        public string GetAdmUserList([FromUri]PageReq request)
        {
            try
            {
                var item = admuserbll.GetList(x => x.IsDelete == 0);
                if (!string.IsNullOrEmpty(request.key))//关键字搜索
                {
                    item = item.Where(x => x.Account.Contains(request.key.Trim())).ToList();
                }
                if (item.Count>0)
                { 
                    mr.status = 0;
                    mr.total = item.Count;
                    mr.data = item.OrderByDescending(x => x.CreateTime).Skip(request.limit * (request.page - 1)).Take(request.limit);
                }
            }
            catch (Exception e)
            {
                mr.status = 500;
                mr.data = admuserbll.GetList(x => x.IsDelete == 0).ToList();
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}