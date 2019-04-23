using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using NLog.Fluent;
using ACE_Mall.Model;
using System.Web.Http.Description;
using static ACE_Behind_Mall.WebApi.Models.UserBindingModels;
using System.Web;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.Web.Security;
using ACE_Behind_Mall.WebApi.App_Start;

namespace ACE_Behind_Mall.WebApi.Controllers
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public class OrderController : BasicController
    {
        UserBLL userbll = new UserBLL();
        ShopCartBLL shopcartbll = new ShopCartBLL();
        GoodBLL goodbll = new GoodBLL();
        OrderBLL orderbll = new OrderBLL();
        OrderGoodBLL ordergoodbll = new OrderGoodBLL();
        SpecificationBLL specificationbll = new SpecificationBLL();
        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestAuthorize]
        public ModelResponse<dynamic> SubmitOrder(SubmitOrder model)
        {
            int userId = GetTicket();
            try
            {
                My_Order myorder = new My_Order();
                myorder.UserID = userId;
                myorder.OrderNo = "ACE" + Utils.GetOrderNumber();
                myorder.OrderState = 1;
                myorder.PayMoney = model.payMoney;
                myorder.Name = userbll.GetList(x => x.ID == userId && x.IsDelete == 0).FirstOrDefault().ReceiveName;
                myorder.Phone = userbll.GetList(x => x.ID ==userId && x.IsDelete == 0).FirstOrDefault().ReceivePhone;
                myorder.Address = userbll.GetList(x => x.ID ==userId && x.IsDelete == 0).FirstOrDefault().ReceiveAddress;
                myorder.CreateTime = DateTime.Now;
                myorder.IsDelete = 0;
                orderbll.Add(myorder);
                var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.IsChecked == true);
                foreach (var item in shopcartmodel)
                {
                    My_Order_Good myordergood = new My_Order_Good();
                    myordergood.OrderNo = myorder.OrderNo;
                    myordergood.GoodNumber = Convert.ToInt32(item.Number);
                    myordergood.GoodID = item.GoodID;
                    myordergood.CreateTime = DateTime.Now;
                    myordergood.IsDelete = 0;
                    ordergoodbll.Add(myordergood);
                    item.IsDelete = 1;
                    My_Shopcart m = shopcartbll.GetUpdateModel<My_Shopcart>(item, "ID");
                    shopcartbll.Update(m);
                }
                mr.message = "提交订单成功";
                mr.data = new { orderNo = myorder, payMoney = myorder.PayMoney };
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestAuthorize]
        public ModelResponse<dynamic> PayOrder(PayOrder model)
        {
            try
            {
                var ordermodel = orderbll.GetList(x => x.IsDelete == 0 && x.OrderNo == model.orderNo).FirstOrDefault();
                ordermodel.OrderState = 2;
                ordermodel.PayTime = DateTime.Now;
                My_Order m = orderbll.GetUpdateModel<My_Order>(ordermodel, "ID");
                bool flag = orderbll.Update(m);
                mr.data = new { status = flag };
                mr.message = "支付成功";
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 获取订单状态下的商品数量
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [RequestAuthorize]
        public ModelResponse<dynamic> GetOrderStatusNumber()
        {
            int userId = GetTicket();
            try
            {
                var ordermodel = orderbll.GetList(x => x.IsDelete == 0 && x.UserID == userId);
                if (ordermodel.Count() == 0)
                {
                    mr.status = 2;
                    mr.message = "请前往登录";
                }
                else
                {
                    int all = ordermodel.Count();
                    int waitPay = ordermodel.Where(x => x.OrderState == 1).Count();
                    int waitDelivery = ordermodel.Where(x => x.OrderState == 2).Count();
                    int waitReceive = ordermodel.Where(x => x.OrderState == 3).Count();
                    int waitEvaluate = ordermodel.Where(x => x.OrderState == 4).Count();
                    int isComplete = ordermodel.Where(x => x.OrderState == 5).Count();
                    int isCancel = ordermodel.Where(x => x.OrderState == 6).Count();
                    mr.message = "获取成功";
                    List<int> models = new List<int>();
                    models.Add(all);
                    models.Add(waitPay);
                    models.Add(waitDelivery);
                    models.Add(waitReceive);
                    models.Add(waitEvaluate);
                    models.Add(isComplete);
                    models.Add(isCancel);
                    mr.data = models;
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
        ///  获取用户订单列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        [HttpGet]
        [RequestAuthorize]
        public ModelResponse<dynamic> GetOrderList(int orderStatus)
        {
            int userId = GetTicket();
            try
            {
                var List = orderbll.GetList(x => x.IsDelete == 0 && x.UserID == userId).OrderBy(x => x.OrderState).ToList();
                if (List.Count == 0)
                {
                    mr.status = 2;
                    mr.message = "请前往登录";
                }
                else
                {
                    if (orderStatus != 0)
                    {
                        List = List.Where(x => x.OrderState == orderStatus).ToList();
                    }
                    var orderList = List.Select(x => new
                    {
                        orderNo = x.OrderNo,
                        submitTime = Convert.ToDateTime(x.CreateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                        goodNumber = ordergoodbll.GetList(y => y.IsDelete == 0 && y.OrderNo == x.OrderNo).Count(),
                        totalMoney = x.PayMoney,
                        receiveName = x.Name,
                        payWay = x.PayWay,
                        orderState = x.OrderState,
                        goodList = ordergoodbll.GetList(y => y.OrderNo == x.OrderNo).Select(y => new
                        {
                            goodName = goodbll.GetList(z => z.IsDelete == 0 && z.ID == y.GoodID).FirstOrDefault().Name,
                            goodImage = goodbll.GetList(z => z.IsDelete == 0 && z.ID == y.GoodID).FirstOrDefault().CoverImage,
                            goodPrice = goodbll.GetList(z => z.IsDelete == 0 && z.ID == y.GoodID).FirstOrDefault().PresentPrice,
                            goodNumber = y.GoodNumber
                        })
                    });
                    mr.data = orderList;
                    mr.message = "获取成功";
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [RequestAuthorize]
        public ModelResponse<dynamic> GetOrderDetail(string orderNo)
        {
            int userId = GetTicket();
            try
            {
                var ordermodel = orderbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.OrderNo == orderNo).FirstOrDefault();
                List<string> times = new List<string>();
                times.Add(ordermodel.CreateTime.ToString());
                times.Add(ordermodel.PayTime.ToString());
                times.Add(ordermodel.DeliveryTime.ToString());
                times.Add(ordermodel.CompleteTime.ToString());
                var orderDetail = orderbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.OrderNo == orderNo).Select(x => new
                {
                    orderNo = x.OrderNo,
                    orderState=x.OrderState,
                    courierName = x.CourierName,
                    courierNo = x.CourierNo,
                    receiveName = x.Name,
                    receivePhone = x.Phone,
                    receiveAddress = x.Address,
                    payWay = x.PayWay,
                    payMoney = x.PayMoney,

                    timeProgress= times,
                    goodList = ordergoodbll.GetList(y => y.IsDelete == 0 && y.OrderNo == orderNo).Select(y => new
                    {
                        goodName = goodbll.GetList(z => z.IsDelete == 0 && z.ID == y.GoodID).FirstOrDefault().Name,
                        goodImage = goodbll.GetList(z => z.IsDelete == 0 && z.ID == y.GoodID).FirstOrDefault().CoverImage,
                        goodPrice = goodbll.GetList(z => z.IsDelete == 0 && z.ID == y.GoodID).FirstOrDefault().PresentPrice,
                        goodNumber = y.GoodNumber,
                    }),  
                });
                mr.data = orderDetail;
                mr.total = 1;
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
        /// 取消订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ModelResponse<dynamic> CancelOrder(PayOrder model)
        {
            try
            {
                var ordermodel = orderbll.GetList(x => x.IsDelete == 0 && x.OrderNo == model.orderNo).FirstOrDefault();
                ordermodel.OrderState = 6;
                My_Order m = orderbll.GetUpdateModel<My_Order>(ordermodel, "ID");
                orderbll.Update(m);
                mr.message = "您已成功取消订单";
            }
            catch (Exception e)
            {
                mr.status = 1;
                mr.message = "服务器错误";
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ModelResponse<dynamic> ConfirmReceipt(PayOrder model)
        {
            try
            {
                var ordermodel = orderbll.GetList(x => x.IsDelete == 0 && x.OrderNo == model.orderNo).FirstOrDefault();
                ordermodel.OrderState = 4;
                ordermodel.CompleteTime = DateTime.Now;
                My_Order m = orderbll.GetUpdateModel<My_Order>(ordermodel, "ID");
                orderbll.Update(m);
                mr.message = "您已成功确认收货";
            }
            catch (Exception e)
            {
                mr.status = 1;
                mr.message = "服务器错误";
                Log.Error(e.Message);
            }
            return mr;
        }
    }
}
