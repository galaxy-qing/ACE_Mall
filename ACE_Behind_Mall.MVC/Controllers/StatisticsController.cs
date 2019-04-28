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
        ViewCategoryBLL viewcategorybll = new ViewCategoryBLL();
        // GET: Statistics
        public ActionResult GoodSaleList()
        {
            return View();
        }
        public ActionResult SaleMoneyList()
        {
            return View();
        }
        public string SelectYear(string selectyear)
        {
            int year = Convert.ToInt32(selectyear);
            var categorymodel = categorybll.GetList(x => x.IsDelete == 0);
            List<string> category = new List<string>();
            foreach (var item in categorymodel)
            {
                category.Add(item.Name);
            }
            var viewcategorymodel = viewcategorybll.GetList(x => x.CategoryDelete == 0 && x.GoodDelete == 0 && x.CreateTime.Year == year);
            List<object> series = new List<object>();
            foreach (var item in categorymodel)
            {
                string Name = item.Name;
                int ID = item.ID;
                List<int> Sale = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    Sale.Add(viewcategorymodel.Where(x => x.CreateTime.Month == i && x.CategoryID == ID).Sum(x => x.GoodNumber));
                }
                var model = new { name = Name,data =Sale};
                series.Add(model);
            }
            mr.data = new { category, series };
            return JsonHelper.Instance.Serialize(mr);
        }
        public string SelectSaleYear(string selectyear)
        {
            int year = Convert.ToInt32(selectyear);
            var categorymodel = categorybll.GetList(x => x.IsDelete == 0);
            List<string> category = new List<string>();
            foreach (var item in categorymodel)
            {
                category.Add(item.Name);
            }
            var viewcategorymodel = viewcategorybll.GetList(x => x.CategoryDelete == 0 && x.GoodDelete == 0 && x.CreateTime.Year == year);
            List<object> series = new List<object>();
            foreach (var item in categorymodel)
            {
                string Name = item.Name;
                int ID = item.ID;
                List<decimal> Sale = new List<decimal>();
                for (int i = 1; i <=12; i++)
                {
                    Sale.Add(viewcategorymodel.Where(x => x.CreateTime.Month == i && x.CategoryID == ID).Sum(x => x.GoodNumber*x.PresentPrice));
                }
                var model = new { name = Name, data = Sale };
                series.Add(model);
            }
            mr.data = new { category, series };
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}