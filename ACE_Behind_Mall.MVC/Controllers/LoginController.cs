using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using System.Collections;
using NLog.Fluent;
using System.Web.Caching;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class LoginController : Controller
    {
        AdmUserBLL admuserbll = new AdmUserBLL();
       
        // GET: Login
        /// <summary>
        /// 登录界面
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
           // var model = admuserbll.GetList(x => x.Account == Session["account"].ToString()).FirstOrDefault();
            //NLogHelper.Logs.LogWriter("登录成功",model.ID, model.ReallyName,model.Account,"登录");
            return View();
        }
        /// <summary>
        /// 登录按钮操作
        /// </summary>
        /// <param name="username">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public string LoginIndex(string username, string password)
        {
            Hashtable ht = new Hashtable();
            try
            {
                var userList = admuserbll.GetList(x => (x.Account == username) & x.Password == password).Count;
               
                if (userList == 0)
                {
                    ht["message"] = "账号错误";
                    ht["result"] = false;
                }
                else
                {
                    ht["message"] = "登陆成功";
                    ht["result"] = true;
                    ///System.Web.HttpContext.Current.Session["account"] = username;//存用户Session
                   // System.Web.HttpContext.Current.Session.Timeout = 60;
                   // var model = admuserbll.GetList(x => x.Account == username).FirstOrDefault();
                   // TimeSpan SessTimeOut = new TimeSpan(0, 0, System.Web.HttpContext.Current.Session.Timeout, 0, 0);
                   // HttpRuntime.Cache.Insert("p" + model.Account, "p" + model.Account, null, DateTime.MaxValue, SessTimeOut, CacheItemPriority.NotRemovable, null);
                }
            }
            catch (Exception e)
            {
                ht["message"] = "服务器内部错误";
                ht["result"] = false;
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(ht);
        }
    }
}