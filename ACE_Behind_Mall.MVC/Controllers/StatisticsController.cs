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
        //public string GoodStatistics()
        //{
        //    var category_list = categorybll.GetList(x=>x.IsDelete==0);
        //    var 
        //}
        public string SelectYear(string selectyear)
        {
            int year = Convert.ToInt32(selectyear);
            var categorymodel = categorybll.GetList(x => x.IsDelete == 0).Select(x => new
            {
                x.ID,
                x.Name
            });
            var viewcategorymodel = viewcategorybll.GetList(x => x.CategoryDelete == 0 && x.GoodDelete == 0 && x.CreateTime.Year == year);
            Dictionary<string, object> series = new Dictionary<string, object>();
            foreach (var item in categorymodel)
            {
                string Name = item.Name;
                int ID = Convert.ToInt32(item.ID);
                series.Add("categoryName"+ID, Name);
               
               // series.Add("categoryID", ID);
                List<int> Sale = new List<int>();
                for (int i = 1; i < 12; i++)
                {
                    Sale.Add(viewcategorymodel.Where(x => x.CreateTime.Month == i && x.CategoryID == ID).Sum(x => x.GoodNumber));
                }
                series.Add("monthSale"+ ID, Sale);
               // var model = new { categoryName=Name, categoryID=ID, monthSale=Sale};
                //series.Add(model);
            }
            mr.data = new { categorymodel, series };
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}