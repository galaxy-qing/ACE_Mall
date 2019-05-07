using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using System.Collections;
using NLog.Fluent;
using ACE_Mall.Model;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class LoginController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
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
        public string SendEmail(string email)
        {
            string code = Utils.GetCheckCode(6,true);
            var model = admuserbll.GetList(x => x.IsDelete == 0 & x.Email == email).FirstOrDefault();
            if (model != null)
            {
                MailHelper.SendEmail(email, "ACE-MALL验证码", code, false);
                model.Code = Utils.MD5(code);
                Adm_User r = admuserbll.GetUpdateModel<Adm_User>(model, "ID");
                bool flag = admuserbll.Update(r);
                mr.message = "success";
            }
            else
            {
                mr.message = "error";
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        public string VerifyCode(string vercode,string picturecode,string cellphone)
        {
            var model = admuserbll.GetList(x => x.IsDelete == 0 & x.Email == cellphone).FirstOrDefault();
            if (model != null)
            {
                string code = Utils.MD5(vercode);
                if (model.Code == code)
                {
                    mr.message = model.Code;
                }
                else
                {
                    mr.status = 1;
                    mr.message = "邮箱验证不通过";
                }
            }
          
            return JsonHelper.Instance.Serialize(mr);
        }
        public string SetPwd(string id, string password)
        {
            try
            {
                var model = admuserbll.GetList(x => x.IsDelete == 0 & x.Code == id).FirstOrDefault();
                model.Password = Utils.MD5(password);
                Adm_User r = admuserbll.GetUpdateModel<Adm_User>(model, "ID");
                admuserbll.Update(r);
            }
            catch (Exception e)
            {
                mr.message="服务器内部错误";
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
            // var model = admuserbll.GetList(x => x.Account == Session["account"].ToString()).FirstOrDefault();
            //NLogHelper.Logs.LogWriter("登录成功",model.ID, model.ReallyName,model.Account,"登录");
            int userid = Convert.ToInt32(Session["userID"]);
            if (userid != 0)
            {
                string account = admuserbll.GetList(x => x.IsDelete == 0 && x.ID == userid).FirstOrDefault().Account;
                ViewBag.account = account;
                return View();
            }
            else
            {
                return Redirect("/Login/Login");
            }
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
                var pwd = Utils.MD5(password);
                var userList = admuserbll.GetList(x => x.Account == username & x.Password == pwd&x.IsDelete==0);
                if (userList.Count() == 0)
                {
                    ht["message"] = "账号或密码错误";
                    ht["result"] = false;
                }
                else
                {
                    ht["message"] = "登陆成功";
                    ht["result"] = true;
                    Session["userID"] = userList.FirstOrDefault().ID;
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