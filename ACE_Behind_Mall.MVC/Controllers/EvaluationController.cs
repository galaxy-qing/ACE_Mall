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
    public class EvaluationController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        EvaluationBLL evaluationbll = new EvaluationBLL();
        UserBLL userbll = new UserBLL();
        GoodBLL goodbll = new GoodBLL();
        // GET: Evaluation
        public ActionResult EvaluationList()
        {
            return View();
        }
        public string GetEvaluationList([FromUri]PageReq request)
        {
            try
            {
                var item = evaluationbll.GetList(x => x.IsDelete == 0).Select(x => new
                {
                    x.ID,
                    x.Evaluation,
                    x.Star,
                    x.IsLook,
                    UserName = userbll.GetList(y => y.ID == x.UserID).FirstOrDefault().ReceiveName,
                    GoodName=goodbll.GetList(y=>y.ID==x.GoodID).FirstOrDefault().Name,
                    x.CreateTime,
                    x.IsDelete,
                }).ToList();
                if (!string.IsNullOrEmpty(request.key1)) //关键字搜索orderNo
                {
                    item = item.Where(x => x.GoodName.Contains(request.key1.Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(request.key2)) //关键字搜索receiveName
                {
                    item = item.Where(x => x.UserName.Contains(request.key2.Trim())).ToList();
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