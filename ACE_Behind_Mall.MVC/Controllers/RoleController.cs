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
        public string Add(Adm_User_Role obj)
        {
            try
            {
                Adm_User_Role model = new Adm_User_Role();
                model.Name = obj.Name;
                model.Describe = obj.Describe;
                model.IsDelete = 0;
                model.CreateTime = DateTime.Now;
                rolebll.Add(model);
                mr.message = "添加成功";
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        public ActionResult Delete(Adm_User_Role model)
        {
            //model.ID= admuserbll.Get
            Adm_User_Role r = rolebll.GetUpdateModel<Adm_User_Role>(model, "ID");
            r.IsDelete = 1;
            bool flag = rolebll.Update(r);
            Hashtable ht = HashTableHelp.GetHash(flag);
            return Json(ht, JsonRequestBehavior.AllowGet);
        }
        public string UpdateRoleName(Adm_User_Role obj)
        {
            bool flag = false;
            try
            {
                var item = rolebll.GetList(x => x.ID == obj.ID).FirstOrDefault();
                Adm_User_Role r = rolebll.GetUpdateModel<Adm_User_Role>(item, "ID");
                r.Name = obj.Name;
                flag = rolebll.Update(r);
                mr.message = "修改成功";
                //NLogHelper.Logs.LogWriter("修改公司招聘：【" + obj.Title + "】排序成功", _userData.User.Id, _userData.User.Account, _userData.User.Name, OpType.Edit);
            }
            catch (Exception ex)
            {
                NLogHelper.Logs.Error(ex.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        public string UpdateDescribeName(Adm_User_Role obj)
        {
            bool flag = false;
            try
            {
                var item = rolebll.GetList(x => x.ID == obj.ID).FirstOrDefault();
                Adm_User_Role r = rolebll.GetUpdateModel<Adm_User_Role>(item, "ID");
                r.Describe = obj.Describe;
                flag = rolebll.Update(r);
                mr.message = "修改成功";
                //NLogHelper.Logs.LogWriter("修改公司招聘：【" + obj.Title + "】排序成功", _userData.User.Id, _userData.User.Account, _userData.User.Name, OpType.Edit);
            }
            catch (Exception ex)
            {
                NLogHelper.Logs.Error(ex.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}