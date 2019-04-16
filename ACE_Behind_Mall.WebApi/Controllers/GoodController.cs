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

namespace ACE_Behind_Mall.WebApi.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// 商品信息
    /// </summary>
    public class GoodController : ApiController
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        GoodBLL goodsbll = new GoodBLL();
        CategoryBLL categorybll = new CategoryBLL();
        EvaluationBLL evaluationbll = new EvaluationBLL();
        UserBLL userbll = new UserBLL();
        SpecificationBLL specificationbll = new SpecificationBLL();
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
                    mr.total = model.Count;
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
                var hotGoods = goodsbll.GetList(x => x.IsDelete == 0).OrderByDescending(x=>x.SaleNumber).Take(15);
                var krisRecommend = goodsbll.GetList(x => x.IsDelete == 0).OrderByDescending(x => x.PresentPrice).Take(5);
                if (hotGoods.Count() != 0&&krisRecommend.Count()!=0)
                {
                    mr.data = new { hotGoods, krisRecommend };
                }
                if (hotGoods.Count() == 0 && krisRecommend.Count() != 0)
                {
                    mr.data = new { hotGoods="暂无商品", krisRecommend };
                }
                if (hotGoods.Count() != 0 && krisRecommend.Count() == 0)
                {
                    mr.data = new { hotGoods, krisRecommend= "暂无商品" };
                }
                if (hotGoods.Count() == 0 && krisRecommend.Count() == 0)
                {
                    mr.data = new { hotGoods= "暂无商品", krisRecommend = "暂无商品" };
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 商品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ModelResponse<dynamic> GetGoods()
        {
            try
            {
                var item = goodsbll.GetList(x => x.IsDelete == 0);
                if (item.Count > 0)
                {
                    mr.total = item.Count;
                    mr.data = item;
                }
            }
            catch (Exception e)
            {
                mr.status = 1;
                mr.data = goodsbll.GetList(x => x.IsDelete == 0);
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 通过分类获取商品列表
        /// </summary>
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
                mr.total = model.Count();
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
                    goodName=x.Name,
                    goodCategory=x.CategoryID,
                    presentPrice=x.PresentPrice,
                    saleNumber=x.SaleNumber,
                    detailImage=x.DetailImage,
                    stock =x.Stock,
                    evaluationNum=evaluationbll.GetList(y=>y.GoodID==x.ID).Count(),
                    infoImage=x.InfoImage,
                });
                mr.data = model ;
                mr.total = model.Count();
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
        /// <param name="goodId">商品ID</param>
        /// <returns></returns>
        [HttpGet]
        public ModelResponse<dynamic> ByGoodIDGetGoodEvaluation(int goodId)
        {
            try
            {
                var model = evaluationbll.GetList(x => x.IsDelete == 0 && x.GoodID == goodId).Select(x => new {
                    addTime=x.CreateTime.ToString(),
                    evaluation = x.Evaluation,
                    account = userbll.GetList(y=>y.ID==x.UserID).FirstOrDefault().Account,
                    image= "http://192.168.0.143:60391"+userbll.GetList(y => y.ID == x.UserID).FirstOrDefault().Image,
                    //specification= specificationbll.GetList(y=>y.)
                });
                mr.data = model;
                mr.total = model.Count();
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
        public ModelResponse<dynamic> AddGoodEvaluation(AddEvaluation model)
        {
            bool flag = false;
            try
            {
                Mall_Good_Evaluation evaluationmodel = new Mall_Good_Evaluation();
                evaluationmodel.GoodID = model.goodId;
                evaluationmodel.UserID = model.userId;
                evaluationmodel.Evaluation = model.content;
                evaluationmodel.CreateTime = DateTime.Now;
                evaluationmodel.IsDelete = 0;
                evaluationmodel.IsLook = 0;
                evaluationmodel.Star = 5;
                flag=evaluationbll.Add(evaluationmodel);
                if (flag == true)
                {
                    mr.message = "评论添加成功";
                }
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
