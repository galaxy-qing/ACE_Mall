using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using NLog.Fluent;
using ACE_Mall.Model;
using System.Collections;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class CategoryController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        CategoryBLL categorybll = new CategoryBLL();
        // GET: Category
        public ActionResult CategoryList()
        {
            return View();
        }
        public string Add(Mall_Category obj)
        {
            try
            {
                Mall_Category model = new Mall_Category();
                model.Name = obj.Name;
                model.IsDelete = 0;
                model.CreateTime = DateTime.Now;
                categorybll.Add(model);
                mr.message = "添加成功";
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        /// <summary>
        /// 商品类别列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns>UserList</returns>
        public string GetCategoryList([FromUri]PageReq request)
        {
            try
            {
                var item = categorybll.GetList(x =>x.IsDelete==0);
                mr.status = 0;
                mr.total = item.Count;
                mr.data = item.OrderByDescending(x => x.CreateTime).Skip(request.limit * (request.page - 1)).Take(request.limit);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        /// <summary>
        /// 商品分类是否显示
        /// </summary>
        /// <param name="isShow">是否显示</param>
        /// <param name="id">当前行ID</param>
        /// <returns></returns>
        public ActionResult IsShow(string isShow,int id)
        {
            try
            {
                var item = categorybll.GetList(x => x.ID== id).FirstOrDefault();
                int IsDelete = 0;
                if (isShow == "false") { IsDelete = 1; }
                //NLogHelper.Logs.LogWriter("商品类别显示：【" + id + "】成功",);
                Mall_Category r = categorybll.GetUpdateModel<Mall_Category>(item, "ID");
                r.IsDelete = IsDelete;
                bool flag = categorybll.Update(r);
                Hashtable ht = HashTableHelp.GetHash(flag);
                return Json(ht, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                mr.status = 2;
                mr.message= ex.Message;
                NLogHelper.Logs.Error(ex.Message);
                return Json(mr, JsonRequestBehavior.AllowGet);
            }          
        }
        /// <summary>
        /// 修改商品分类名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult UpdateCategoryName(Mall_Category obj)
        {
            bool flag = false;
            try
            {
                var item = categorybll.GetList(x => x.ID == obj.ID).FirstOrDefault();
                Mall_Category r = categorybll.GetUpdateModel<Mall_Category>(item, "ID");
                r.Name = obj.Name;
                flag = categorybll.Update(r);
                //NLogHelper.Logs.LogWriter("修改公司招聘：【" + obj.Title + "】排序成功", _userData.User.Id, _userData.User.Account, _userData.User.Name, OpType.Edit);
            }
            catch (Exception ex)
            {
                mr.status = 1;
                mr.message = ex.Message;
                NLogHelper.Logs.Error(ex.Message);
            }
            Hashtable ht = HashTableHelp.GetHash(flag);
            return Json(ht, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Delete(Mall_Category model)
        {
            //model.ID= admuserbll.Get
            Mall_Category r = categorybll.GetUpdateModel<Mall_Category>(model, "ID");
            r.IsDelete = 1;
            bool flag = categorybll.Update(r);
            Hashtable ht = HashTableHelp.GetHash(flag);
            return Json(ht, JsonRequestBehavior.AllowGet);
        }
    }
}