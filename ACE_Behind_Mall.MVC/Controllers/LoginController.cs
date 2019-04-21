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
            //获取cookie中的数据
            HttpCookie cookie = Request.Cookies.Get("mycookie");
            if (cookie != null)
            {
                //把保存的用户名和密码赋值给对应的文本框
                //用户名
                var name = cookie.Values["username"].ToString();
                ViewBag.username = name;
                //密码
                var pwd = cookie.Values["password"].ToString();
                ViewBag.password = pwd;
            }
            return View();
        }
        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Forget()
        {
            return View();
        }
        public ActionResult SendEmail()
        {
            MailHelper.SendEmail("1945697586@qq.com", "ACE-MALL接收密码", "111", false);
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
        public string LoginIndex(string username, string password,string remember)
        {
            Hashtable ht = new Hashtable();
            try
            {
                var userList = admuserbll.GetList(x => x.Account == username & x.Password == password&x.IsDelete==0).Count;
                if (userList == 0)
                {
                    ht["message"] = "账号或密码错误";
                    ht["result"] = false;
                }
                else
                {
                    ht["message"] = "登陆成功";
                    ht["result"] = true;
                    if (remember == "on")
                    {
                        HttpCookie hc = new HttpCookie("mycookie");
                        hc["username"] = username;
                        hc["password"] = password;
                        hc.Expires = DateTime.Now.AddDays(5);
                        Response.Cookies.Add(hc);
                    }
                    else
                    {
                        HttpCookie hc = new HttpCookie("mycookie");
                        if (hc != null)
                        {
                            hc.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(hc);
                        }
                    }
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