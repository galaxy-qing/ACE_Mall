using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACE_Mall.BLL;
using ACE_Mall.Common;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class StatisticsController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        OrderBLL orderbll = new OrderBLL();
        OrderGoodBLL ordergoodbll = new OrderGoodBLL();
        CategoryBLL categorybll = new CategoryBLL();
        GoodBLL goodbll = new GoodBLL();
        // GET: Statistics
        public ActionResult GoodSaleList()
        {
            return View();
        }
        //public string GoodStatistics()
        //{
        //    var category_list = categorybll.GetList(x=>x.IsDelete==0);
        //    var 
        //}
        //public string SelectYear(string selectyear)
        //{
        //    var categorymodel = categorybll.GetList(x=>x.IsDelete==0);
        //    var goodmodel = goodbll.GetList(x => x.IsDelete == 0).Select(x => new
        //    {
        //        goodsSaleNumber=goodbll.GetList(y=>y.CategoryID)
        //    });
        //}
    }
}