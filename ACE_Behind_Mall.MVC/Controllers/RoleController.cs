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
        AdmUserBLL admuserbll = new AdmUserBLL();
        ModuleBLL modulebll = new ModuleBLL();
        PageBLL pagebll = new PageBLL();
        ViewBetweenBLL vbetweenbll = new ViewBetweenBLL();
        BetweenBLL betweenbll = new BetweenBLL();
        // GET: Role
        public ActionResult RoleList()
        {
            return View();
        }
        public ActionResult AssignPower()
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
            bool flag = false;
            flag = rolebll.Delete(model);
            var admusermodel = admuserbll.GetList(x => x.RoleID == model.ID);
            foreach (var item in admusermodel)
            {
                admuserbll.Delete(item);
            }
            
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
        public string GetPageList()
        {
            var model = modulebll.GetList(x => true).ToList();
            List<object> json = new List<object>();
            foreach (var item in model)
            {
                string title = item.Name;
                string value = "m"+item.ID.ToString();
                var data = pagebll.GetList(x => x.ModuleID == item.ID).ToList().Select(x => new
                {
                   value="p"+x.ID.ToString(),
                   title=x.Name,
                   data= new int[] { }
            });
                json.Add(new { title, value, data });
            }
            mr.data = new { json };
            mr.total = model.Count();
            return JsonHelper.Instance.Serialize(mr);
        }
        public string SetPower(string roleId, int[] checkPage)
        {
            try
            {
                var betweenModel = vbetweenbll.GetList(x => x.RoleID == Convert.ToInt32(roleId));
                if (betweenModel != null)
                {
                    foreach (var item in betweenModel)
                    {
                        betweenbll.Delete(item.ID);
                    }
                }
                if (checkPage.Length > 0)
                {
                    foreach (int item in checkPage)
                    {
                        var btnModel = new Adm_Between();
                        btnModel.IsDelete = 0;
                        btnModel.PageID = item;
                        btnModel.RoleID = Convert.ToInt32(roleId);
                        btnModel.Createtime = DateTime.Now;
                        betweenbll.Add(btnModel);
                    }
                }
                mr.message = "权限分配成功";
            }
            catch (Exception e)
            {
                mr.status = 1;
                mr.message = e.Message;
            }

            return JsonHelper.Instance.Serialize(mr);
        }
        public string GetPowerPageList(string roleId)
        {
            try
            {
                var betweenModel = vbetweenbll.GetList(x => x.RoleID == Convert.ToInt32(roleId));
                List<int?> pagemodel = new List<int?>();
                foreach (var item in betweenModel)
                {
                    pagemodel.Add(item.PageID);
                }
                mr.data = pagemodel;
            }
            catch (Exception e)
            {
                mr.status = 1;
                mr.message = e.Message;
            }
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}