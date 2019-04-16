using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using ACE_Mall.Model;
using NLog.Fluent;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class RoleController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        RoleBLL rolebll = new RoleBLL();
        // GET: Role
        public ActionResult RoleList()
        {
            return View();
        }
        public string GetRoleList([FromUri]PageReq request)
        {
            var model = rolebll.GetList(x=>x.IsDelete==0).ToList();
            mr.data = model.OrderByDescending(x => x.CreateTime).Skip(request.limit * (request.page - 1)).Take(request.limit);
            mr.total = model.Count();
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}