using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using ACE_Mall.Model;
using NLog.Fluent;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class GoodController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        GoodBLL goodbll = new GoodBLL();
        // GET: Good
        public ActionResult GoodList(string goodName)
        {
            return View();
        }
        /// <summary>
        /// 商品详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodDetail()
        {
            return View();
        }
        public string GetGoodDetail(int id)
        {
            var goodmodel = goodbll.GetList(x => x.ID == id).Select(x => new
            {
                x.ID,
                x.InfoImage,
                x.IsDelete,
                x.Name,
                x.OriginalPrice,
                x.PresentPrice,
                x.SaleNumber,
                x.Stock,
                x.CategoryID,
                x.CoverImage,
                x.CreateTime,
                x.DetailImage
            });
            mr.data = goodmodel;
            return JsonHelper.Instance.Serialize(mr);
        }
        public string GetGoodList()
        {
            var goodmodel = goodbll.GetList(x => true).ToList().Select(x => new
            {
                x.ID,
                x.Name,
                x.CoverImage,
                x.IsDelete,
                x.PresentPrice,
                x.OriginalPrice
            });
            mr.data = goodmodel;
            return JsonHelper.Instance.Serialize(mr);
            //foreach (var item in goodmodel)
            //{
            //    int ID = item.ID;
            //    string goodName = item.Name;
            //    string coverImage = item.CoverImage;
            //    int isDelete = item.IsDelete; 
            //}
        }
        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public string OffShelves(string idList)
        {
            string[] idsList = idList.Split(',');
            try
            {
                foreach (var item in idsList)
                {
                    var model = goodbll.GetList(x => x.ID == Convert.ToInt32(item)).FirstOrDefault();
                    model.IsDelete = 1;
                    Mall_Good m = goodbll.GetUpdateModel<Mall_Good>(model, "ID");
                    bool flag = goodbll.Update(m);
                    mr.message = "商品下架成功";
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public string OnShelves(string idList)
        {
            string[] idsList = idList.Split(',');
            try
            {
                foreach (var item in idsList)
                {
                    var model = goodbll.GetList(x => x.ID == Convert.ToInt32(item)).FirstOrDefault();
                    model.IsDelete = 0;
                    Mall_Good m = goodbll.GetUpdateModel<Mall_Good>(model, "ID");
                    bool flag = goodbll.Update(m);
                    mr.message = "商品上架成功";
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        public bool GoodSearch(string goodName)
        {
            ViewBag.goodName = goodName;
            return true;
        }
       
    }
}