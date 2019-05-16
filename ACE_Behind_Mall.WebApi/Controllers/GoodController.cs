using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using NLog.Fluent;
using ACE_Mall.Model;
using static ACE_Behind_Mall.WebApi.Models.UserBindingModels;
using ACE_Behind_Mall.WebApi.App_Start;
using System.Web;

namespace ACE_Behind_Mall.WebApi.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// 商品信息
    /// </summary>
    public class GoodController : BasicController
    {
        GoodBLL goodsbll = new GoodBLL();
        CategoryBLL categorybll = new CategoryBLL();
        EvaluationBLL evaluationbll = new EvaluationBLL();
        UserBLL userbll = new UserBLL();
        SpecificationBLL specificationbll = new SpecificationBLL();
        RotationBLL rotationbll = new RotationBLL();
        OrderGoodBLL ordergoodbll = new OrderGoodBLL();
        OrderBLL orderbll = new OrderBLL();
        /// <summary>
        /// 获取轮播图
        /// </summary>
        /// <returns></returns>
        public ModelResponse<dynamic> GetRotationPicture()
        {
            try
            {
                var model = rotationbll.GetList(x => x.IsDelete == 0).Select(x => new
                {
                    goodID = x.GoodID,
                    picture = x.Picture,
                });
                mr.data = model;
                mr.message = "获取成功";
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 获取商品类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ModelResponse<dynamic> GetGoodsCategory()
        {
            try
            {
                var model = categorybll.GetList(x => x.IsDelete == 0).Select(x=>new {
                    categoryId=x.ID,
                    categoryName =x.Name,
                }).ToList();
                if (model.Count > 0)
                {
                    mr.data = model;
                }
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;

        }
        /// <summary>
        /// 获取首页热卖商品及推荐商品
        /// </summary>
        /// <returns></returns>
        public ModelResponse<dynamic> GetHomeGoods()
        {
            try
            {
                 var goodsList1 = goodsbll.GetList(x => x.IsDelete == 0).OrderByDescending(x => x.SaleNumber).Take(15).Select(x => new
                {
                    img = x.CoverImage,
                    id = x.ID,
                    name = x.Name,
                    originalPrice = x.OriginalPrice,
                    presentPrice = x.PresentPrice,
                });
                var goodsList2 = goodsbll.GetList(x => x.IsDelete == 0).OrderByDescending(x => x.PresentPrice).Take(5).Select(x=>new {
                    img = x.CoverImage,
                    id = x.ID,
                    name = x.Name,
                    originalPrice = x.OriginalPrice,
                    presentPrice = x.PresentPrice,
                });
                var hotGoods = new { title = "热卖商品", goodsList= goodsList1 };
                var krisRecommend = new { title = "Kris wu 推荐", goodsList = goodsList2 };
                mr.data = new { hotGoods, krisRecommend };
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 通过分类获取商品列表
        /// </summary>
        /// <param name="pageSize">每页数量</param>
        /// <param name="page">第几页</param>
        /// <param name="categoryId">商品类别ID</param>
        /// <param name="sortNo">排序号</param>
        /// <returns></returns>
        [HttpGet]
        public ModelResponse<dynamic> ByCategoryGetGoods(int categoryId,int sortNo)
        {
            //sortNo  0 默认，1，价格高，2，价格低，3，销量高，4，销量低
            try
            {
                var model = goodsbll.GetList(x => x.IsDelete == 0 & x.CategoryID == categoryId).Select(x => new
                {
                    name = x.Name,
                    img = x.CoverImage,
                    presentPrice=x.PresentPrice,
                    originalPrice=x.OriginalPrice,
                    saleNamber=x.SaleNumber,
                    id=x.ID,
                });
                if (sortNo == 1)//价格降序排列
                {
                    model = model.OrderByDescending(x => x.presentPrice).ToList();
                }
                if (sortNo == 2)//价格升序排列
                {
                    model = model.OrderBy(x => x.presentPrice).ToList();
                }
                if (sortNo == 3)//销量降序排列
                {
                    model = model.OrderByDescending(x => x.saleNamber).ToList();
                }
                if (sortNo == 4)//销量升序排列
                {
                    model = model.OrderBy(x => x.saleNamber).ToList();
                }
                mr.data = new { goodsList = model };
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 通过商品ID获取商品详情
        /// </summary>
        /// <param name="goodId">商品ID</param>
        /// <returns></returns>
        [HttpGet]
        public ModelResponse<dynamic> ByGoodIDGetGoodDetail(int goodId)
        {
            try
            {
                var model = goodsbll.GetList(x=>x.IsDelete==0&&x.ID==goodId).Select(x=>new {
                    categoryID=x.CategoryID,
                    categoryName=categorybll.GetList(y=>y.IsDelete==0&&y.ID==x.CategoryID).FirstOrDefault().Name,
                    goodName =x.Name,
                    goodCategory=x.CategoryID,
                    presentPrice=x.PresentPrice,
                    saleNumber=x.SaleNumber,
                    detailImage=x.DetailImage,
                    stock =x.Stock,
                    evaluationNum=evaluationbll.GetList(y=>y.GoodID==x.ID).Count(),
                    infoImage=x.InfoImage,
                    isShow=x.IsDelete,//0,未下架，1，已下架
                });
                mr.data = model ;
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 获取商品评价
        /// </summary>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="page">第几页</param>
        /// <param name="goodId">商品ID</param>
        /// <returns></returns>
        [HttpGet]
        public ModelResponse<dynamic> ByGoodIDGetGoodEvaluation(int pageSize,int page,int goodId)
        {
            try
            {
                var model = evaluationbll.GetList(x => x.IsDelete == 0 && x.GoodID == goodId).Take(pageSize * page).Skip(pageSize * (page - 1)).Select(x => new {
                    addTime = Convert.ToDateTime(x.CreateTime).ToString("yyyy-MM-dd HH:MM:ss"),
                    star = x.Star,
                    evaluation = x.Evaluation,
                    account = userbll.GetList(y=>y.ID==x.UserID).FirstOrDefault().Account,
                    image= userbll.GetList(y => y.ID == x.UserID).FirstOrDefault().Image,
                });
                int total = evaluationbll.GetList(x => x.IsDelete == 0 && x.GoodID == goodId).Count();
                mr.data = new { model, total };
               
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 添加商品评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> AddGoodEvaluation(AddEvaluation model)
        {
            int userId = GetTicket();
            bool flag1 = false;
            bool flag2 = false;
            try
            {
                var ordergoodmodel = ordergoodbll.GetList(x => x.OrderNo == model.orderNo && x.GoodID == model.goodId);
                if (ordergoodmodel.Count() > 0)
                {
                    Mall_Good_Evaluation evaluationmodel = new Mall_Good_Evaluation();
                    evaluationmodel.GoodID = model.goodId;
                    evaluationmodel.UserID = userId;
                    evaluationmodel.Evaluation = model.content;
                    evaluationmodel.CreateTime = DateTime.Now;
                    evaluationmodel.IsDelete = 0;
                    evaluationmodel.IsLook = 0;
                    evaluationmodel.Star = model.star;
                    flag1 = evaluationbll.Add(evaluationmodel);
                    var ordermodel = orderbll.GetList(x=>x.OrderNo==model.orderNo).FirstOrDefault();
                    ordermodel.OrderState = 5;
                    My_Order m = orderbll.GetUpdateModel<My_Order>(ordermodel, "ID");
                    flag2 = orderbll.Update(m);
                    if (flag1 == true&& flag2 == true)
                    {
                        mr.message = "评论添加成功";
                    }
                }
                else
                {
                    mr.status = 1;
                    mr.message = "您尚未购买此商品，请勿评价";
                }
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;

        }
        /// <summary>
        /// 商品搜索
        /// </summary>
        /// <param name="goodName">商品名称</param>
        /// <returns></returns>
        [HttpGet]
        public ModelResponse<dynamic> SearchGood(string goodName)
        {
            try
            {
                var model = goodsbll.GetList(x => x.IsDelete == 0).Select(x => new
                {
                    name = x.Name,
                    img = x.CoverImage,
                    presentPrice = x.PresentPrice,
                    originalPrice = x.OriginalPrice,
                    saleNamber = x.SaleNumber,
                    id = x.ID,
                });
                if (!string.IsNullOrEmpty(goodName))
                {
                    model = model.Where(x => x.name.Contains(goodName)).ToList();
                }
                mr.data = new { goodsList = model };
                mr.total = model.Count();
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;
        }
    }
}
