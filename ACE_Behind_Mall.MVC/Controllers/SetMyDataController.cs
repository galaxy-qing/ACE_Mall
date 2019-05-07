using ACE_Mall.BLL;
using ACE_Mall.Common;
using ACE_Mall.Model;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACE_Behind_Mall.MVC.Controllers
{

    public class SetMyDataController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        AdmUserBLL admuserbll = new AdmUserBLL();
        RoleBLL rolebll = new RoleBLL();
        // GET: SetMyData
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <returns></returns>
        public string GetMyInfo()
        {
            int userId = Convert.ToInt32(Session["userID"]);
            var usermodel = admuserbll.GetList(x => x.ID == userId).Select(x => new {
                RoleName = rolebll.GetList(y => y.IsDelete == 0 && y.ID == x.RoleID).FirstOrDefault().Name,
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
            }).FirstOrDefault();
            mr.data = usermodel;
            return JsonHelper.Instance.Serialize(mr);
        }
        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateMyInfo(Adm_User model)
        {
            try
            {
                int userId = Convert.ToInt32(Session["userID"]);
                var usermodel = admuserbll.GetList(x => x.ID == userId).FirstOrDefault();
                usermodel.ReallyName = model.ReallyName;
                usermodel.Sex = model.Sex;
                usermodel.Phone = model.Phone;
                usermodel.Email = model.Email;
                usermodel.Birthday = model.Birthday;
                Adm_User r = admuserbll.GetUpdateModel<Adm_User>(usermodel, "ID");
                bool flag = admuserbll.Update(r);
                if (flag == true)
                {
                    mr.message = "修改成功";
                }
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}