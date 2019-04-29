using System;
using System.Web.Http;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using NLog.Fluent;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class UserController : Controller
    {
        protected ModelResponse<dynamic> mr = new ACE_Mall.Common.ModelResponse<dynamic>();
        UserBLL userbll = new UserBLL();
        // GET: User
        /// <summary>
        /// 用户列表视图
        /// </summary>
        /// <returns>View</returns>
        public ActionResult UserList()
        {
            return View();
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns>UserList</returns>
        public string GetUserList([FromUri]PageReq request)
        {
            try
            {
                var item = userbll.GetList(x => x.IsDelete == 0).Select(x=>new {
                    x.ID,
                    x.Email,
                    x.Image,
                    x.Account,
                    x.Password,
                    x.ReceiveAddress,
                    x.ReceiveName,
                    x.ReceivePhone,
                    x.CreateTime,
                    x.IsDelete
                }).ToList();
                if (!string.IsNullOrEmpty(request.key1))//关键字搜索
                {
                    item = item.Where(x=>x.Account.Contains(request.key1.Trim())).ToList();
                }
                    mr.total = item.Count;
                    mr.data = item.OrderByDescending(x => x.CreateTime).Skip(request.limit * (request.page - 1)).Take(request.limit);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}