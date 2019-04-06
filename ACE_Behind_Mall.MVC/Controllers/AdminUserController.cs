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
    public class AdminUserController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        AdmUserBLL admuserbll = new AdmUserBLL();
        RoleBLL rolebll = new RoleBLL();
        // GET: AdminUser
        public ActionResult AdmUserList()
        {
            return View();
        }
        public ActionResult GetRoleList()
        {
            var rolemodel = rolebll.GetList(x => x.IsDelete == 0).ToList();
            return Json(rolemodel,JsonRequestBehavior.AllowGet);
            //List<SelectListItem> roleList = new List<SelectListItem>
            //{
            //    new SelectListItem{ Text="",Value=""},
            //};
            //ViewData["roleList"] = new SelectList(roleList, "Value", "Text", "");
            //return View();
        }
        /// <summary>
        /// 得到员工列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetAdmUserList([FromUri]PageReq request)
         {
            try
            {
                var item = admuserbll.GetList(x => x.IsDelete == 0).Select(x=>new {
                    RoleName= rolebll.GetList(y=>y.IsDelete==0&&y.ID==x.RoleID).FirstOrDefault().Name,
                    x.ReallyName,
                    x.ID,
                    x.RoleID,
                    x.Account,
                    x.Phone,
                    x.Email,
                    x.Birthday,
                    x.Sex,
                    x.CreateTime,
                    x.IsDelete,
                }).ToList();
                if (!string.IsNullOrEmpty(request.key))//关键字搜索
                {
                    item = item.Where(x => x.Account.Contains(request.key.Trim())).ToList();
                }
                    mr.status = 0;
                    mr.total = item.Count;
                    mr.data = item.OrderByDescending(x => x.CreateTime).Skip(request.limit * (request.page - 1)).Take(request.limit);
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Update(Adm_User model)
        {
            Adm_User r = admuserbll.GetUpdateModel<Adm_User>(model, "ID");
            bool flag = admuserbll.Update(r);
            Hashtable ht = HashTableHelp.GetHash(flag);
            return Json(ht, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Delete(Adm_User model)
        {
            //model.ID= admuserbll.Get
            Adm_User r = admuserbll.GetUpdateModel<Adm_User>(model, "ID");
            r.IsDelete = 1;
            bool flag = admuserbll.Update(r);
            Hashtable ht = HashTableHelp.GetHash(flag);
            return Json(ht, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add(Adm_User model)
        {
            model.Password = "123456";
            model.Image = "/images/users/userphoto.jpg";
            model.CreateTime = DateTime.Now;
            bool flag = admuserbll.Add(model);
            Hashtable ht = HashTableHelp.GetHash(flag);
            return Json(ht, JsonRequestBehavior.AllowGet);
        }
    }
}