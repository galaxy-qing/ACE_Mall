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
    public class OrderController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        OrderBLL orderbll = new OrderBLL();
        UserBLL userbll = new UserBLL();
        OrderGoodBLL ordergoodbll = new OrderGoodBLL();
        GoodBLL goodbll = new GoodBLL();
        // GET: Order
        public ActionResult OrderList()
        {
            return View();
        }
        public ActionResult OrderDetail()
        {
            return View();
        }
        /// <summary>
        /// 得到订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetOrderList([FromUri]PageReq request)
        {
            try
            {
                var item = orderbll.GetList(x => x.IsDelete == 0).Select(x => new
                {
                    //RoleName = rolebll.GetList(y => y.IsDelete == 0 && y.ID == x.RoleID).FirstOrDefault().Name,
                    x.ID,
                    x.OrderNo,
                    Name = userbll.GetList(y => y.ID == x.UserID).FirstOrDefault().ReceiveName,
                    Address = userbll.GetList(y => y.ID == x.UserID).FirstOrDefault().ReceiveAddress,
                    Phone = userbll.GetList(y => y.ID == x.UserID).FirstOrDefault().ReceivePhone,
                    x.OrderState,
                    x.PayMoney,
                    x.PayTime,
                    x.PayWay,
                    x.Note,
                    progress = x.OrderState == 6 ? "100%" : (double)x.OrderState / 5 * 100 + "%",
                    x.CompleteTime,
                    x.CourierName,
                    x.CourierNo,
                    x.DeliveryTime,
                    x.CreateTime,
                    x.IsDelete,
                }).ToList();
                if (!string.IsNullOrEmpty(request.state))//下拉框搜索orderState
                {
                    if (request.state == "1") //待发货
                    {
                        item = item.Where(x => x.OrderState==2).ToList();

                    }
                    if (request.state == "2") //已发货
                    {
                        item = item.Where(x => x.OrderState > 2).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(request.key1)) //关键字搜索orderNo
                {
                    item = item.Where(x => x.OrderNo.Contains(request.key1.Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(request.key2)) //关键字搜索receiveName
                {
                    item = item.Where(x => x.Name.Contains(request.key2.Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(request.key3)) //关键字搜索receivePhone
                {
                    item = item.Where(x => x.Phone.Contains(request.key3.Trim())).ToList();
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
        /// <summary>
        /// 编辑快递信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateCourier(int ID,string CourierName,string CourierNo)
        {
            bool flag = false;
            try
            {
                var model = orderbll.GetList(x => x.IsDelete == 0 && x.ID == ID).FirstOrDefault();
                model.CourierName = CourierName;
                model.CourierNo = CourierNo;
                model.DeliveryTime = DateTime.Now;
                model.OrderState = 3;
                My_Order r = orderbll.GetUpdateModel<My_Order>(model, "ID");
                flag = orderbll.Update(r);
                mr.message = "发货成功";
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                mr.message = "发货失败";
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        public string GetOrderGoodList(string id)
        {
            try
            {
                var item = ordergoodbll.GetList(x => x.IsDelete == 0&&x.OrderNo==id).Select(x => new
                {
                    x.ID,
                    x.OrderNo, 
                    GoodName = goodbll.GetList(y => y.ID == x.GoodID).FirstOrDefault().Name,
                    goodbll.GetList(y => y.ID == x.GoodID).FirstOrDefault().PresentPrice,
                    GoodImage = goodbll.GetList(y => y.ID == x.GoodID).FirstOrDefault().CoverImage,
                    x.GoodNumber,
                    x.CreateTime,
                    x.IsDelete,
                }).ToList();
                if (item.Count() == 0)
                {
                    mr.status = 2;
                    mr.message = "请前往登录";
                }
                mr.total = item.Count;
                mr.data = item.OrderByDescending(x => x.CreateTime);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}