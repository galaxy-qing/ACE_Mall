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
using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using ACE_Mall.Common.App_Code;
using Newtonsoft.Json;

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
        [AllowAnonymous]
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
                myorder.Note = model.note;
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

                //DefaultAopClient client = new DefaultAopClient(config.gatewayUrl, config.app_id, config.private_key, "json", "1.0", config.sign_type, config.alipay_public_key, config.charset, false);
                //IAopClient client = new DefaultAopClient(config.gatewayUrl, config.app_id, config.private_key, "json", "1.0", config.sign_type, config.alipay_public_key, config.charset, false);

                //外部订单号，商户网站订单系统中唯一的订单号
                //string out_trade_no = "2019581723241";

                //订单名称
                //string subject = "测试";

                //付款金额
                //string total_amount = "0.01";

                //商品描述
                //string body = "111";
                //string store_id = "";
                //string timeout_express = "";
                //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：
                //AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();//创建API对应的request类


                //SDK已经封装掉了公共参数，这里只需要传入业务参数


                //request.BizContent = AliPayInfo(out_trade_no, total_amount, subject);
                //AlipayTradePrecreateResponse response = client.Execute(request);
                //if (response.IsError == true)
                //{

                //    mr.message = "失败";
                //}
                //if (response.IsError == false)
                //{
                //    mr.message = "成功";
                //}
                 mr.data = new { orderNo = myorder, payMoney = myorder.PayMoney };
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;
        }
        public static string AliPayInfo(string out_trade_no, string total_amount, string subject)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //Log.Error("用户个人佣金或余额提现开始", "");
            dic.Add("out_trade_no", out_trade_no);//商户转账唯一订单号
            dic.Add("total_amount", total_amount);//收款方账户类型
            dic.Add("subject", "");//收款方账户
            //dic.Add("store_id", "");//转账金额
            //dic.Add("timeout_express", "90m");//付款方姓名
            //dic.Add("payee_real_name", payee_real_name);//收款方姓名
            //dic.Add("remark", remark);//转账备注
            return JsonConvert.SerializeObject(dic);
        }
        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
                    //超过20分钟未付款自动取消订单
                    var noPayOrder = orderbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.OrderState == 1).ToList();
                    DateTime nowTime = DateTime.Now;
                    foreach (var item in noPayOrder)
                    {
                        TimeSpan ts = nowTime - Convert.ToDateTime(item.CreateTime);
                        if (Convert.ToInt64(ts.TotalMinutes) > 20)
                        {
                            item.OrderState = 6;
                            My_Order m = orderbll.GetUpdateModel<My_Order>(item, "ID");
                            orderbll.Update(m);
                        }
                    }
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
        [AllowAnonymous]
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
                        goodNumber = ordergoodbll.GetList(y => y.OrderNo == x.OrderNo).Count(),
                        totalMoney = x.PayMoney,
                        receiveName = x.Name,
                        payWay = x.PayWay,
                        orderState = x.OrderState,
                        goodList = ordergoodbll.GetList(y => y.OrderNo == x.OrderNo).Select(y => new
                        {
                            goodId=y.GoodID,
                            goodName = goodbll.GetList(z => z.ID == y.GoodID).FirstOrDefault().Name,
                            goodImage = goodbll.GetList(z => z.ID == y.GoodID).FirstOrDefault().CoverImage,
                            goodPrice = goodbll.GetList(z => z.ID == y.GoodID).FirstOrDefault().PresentPrice,
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
        [AllowAnonymous]
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
                    goodList = ordergoodbll.GetList(y => y.OrderNo == orderNo).Select(y => new
                    {
                        goodId=x.ID,
                        goodName = goodbll.GetList(z => z.ID == y.GoodID).FirstOrDefault().Name,
                        goodImage = goodbll.GetList(z => z.ID == y.GoodID).FirstOrDefault().CoverImage,
                        goodPrice = goodbll.GetList(z => z.ID == y.GoodID).FirstOrDefault().PresentPrice,
                        goodNumber = y.GoodNumber,
                    }),  
                });
                mr.data = orderDetail;
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
        /// <summary>
        /// 删除已关闭订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> DeleteOrder(PayOrder model)
        {
            int userId = 0;
            userId = GetTicket();
            if (userId != 0)
            {
                try
                {
                    var ordermodel = orderbll.GetList(x => x.IsDelete == 0 && x.OrderNo == model.orderNo).FirstOrDefault();
                    if (ordermodel.OrderState == 6)
                    {
                        ordermodel.IsDelete = 1;
                        My_Order m = orderbll.GetUpdateModel<My_Order>(ordermodel, "ID");
                        orderbll.Update(m);
                        mr.message = "您已成功删除订单";
                    }
                    else
                    {
                        mr.status = 1;
                        mr.message = "该订单不能删除";
                    }
                }
                catch (Exception e)
                {
                    mr.status = 1;
                    mr.message = "服务器错误";
                    Log.Error(e.Message);
                }
            }
            return mr;
        }
        
        public class config
        {
            public config()
            {
                //
                // TODO: 在此处添加构造函数逻辑
                //
            }
            // 应用ID,您的APPID
            public static string app_id = "2019033163690973";

            // 支付宝网关
            public static string gatewayUrl = "https://openapi.alipaydev.com/gateway.do ";
            //
            // 商户私钥，您的原始格式RSA私钥
            public static string private_key = "MIIEpAIBAAKCAQEAokUan80Z5Cdixf0IH3fAfC5SAk6PwNl5qRXFYoZvjGFQRNdINeit7hjntpbnHDZiPdvQ8aCdVFi5zr6j6Yb7dmo5nRmwU58gxG4B/hBsQvfLuvssb5DkVFFR0qCtWFaxKbn3ZiaGMvYdAE3LibhihpsSZfWdRn9N5yll2ZQYNK7MXYAq4/syeJyw0yixjFYmgtee56BvVB7FdYUKyPLmVHdnarmoVMx0TehDQIklpY6HvK/FRsO353jmywodz3fVI0mbplTtkPSHhKheyLmqrh/eUsh6t9LHML7gSJQJ8GxhIpTuZkSRyzu2AjN/MrHqoAAlTqWfuYXkAHAG4p6b7wIDAQABAoIBAFVbmHe7AgWcGj8frSgwbBZmn0kLXl8Jhw/EngIHyaHEht7Ph/KjpOp8O1c25fdXPDJh8PVJkbkOux7f9YUgNiLCGfBj1PcH28q4O2AT738Cri57dZJyW0lnRW1QjB7N63+RjWw0k583C2ZfhdO6JEm+RN1RaBvvV2Fjy2m6l0kOW5AFEdH0uhzpFbv8HqXJ0WBTr4Dfyu98LZdCV/9928dJXmNBXJeGUHXOLhYnuYLk7uE1QO5lFu44yFys3YoPXnKL/Orv/m/Mca0zsJkZUz9U4Dm7u4scpXvhyYOgg6C1lgXKzfOmrLpUdL8VtsLIolYPE8AGVOyvY85uSxRp09ECgYEAzOuI403XLB/2tx0BNTkU8LJp5KC14XDKNAcPcPY7OpZ68BziD5IleRPkXRIbp1VBWRSPYkVjoU4iYvNEJiFaDPkm9oWLcufSM/1eGn/vGf2a5JLGhHR+vdXwLX8DIoUMfim7FtEQODjq+3K+dkmHAbHwlOwj+ZIM3wbmpmPkR4MCgYEAyrf1KDJzmZ6AjxzH9FA4tNNRHArFkyLUCGnIFAohZ/M8cyuvWXw3639prdxnHBCKHumuZfp0SqyluuM3BIFU3QRg/yrhJIWd8QTw2H/Jt1BpptAuemSYRqiuQHKrCKYLfk+2e2EwXeaa72Bu24y/4TOs57R81HvygNcBaZ+lwiUCgYA3UBSBFo/QL0iPINsknKXUWrROrDqsAZ/y7zzxxuUx6VCB46n5ff9zcTUhEjQ4tMCQ5QXXBtffwJFzmCp7CGGgQtiLjnyXpY+NzQRLruDBaT0YGa5QLonPgCH2heQ4uyUmIOmJPSFdq69x9AUJNMumX3uLFzqWsR+cS+aSX0BNNQKBgQC9jpq9xb927FHGgOEwwTrlS5xSnnf1h+HBJDklE/v82eOyxiynfpJDsda97pS0F3swQM8FKNdJZHtscD7oBY+3Q/r3X787iX4Q+8/Cgmu68IR6qbxsUlhZ6i1WsmLgKXQh3qQCZvT1OUezgvbmcYyTJuENoSYBAw3WEDaP7+rtyQKBgQDJMS64/AHVvD8oi9DXVFXHQ3m4oYeYGFF4qW0UIBQ3ol6y+XgSlfybWzpScCF1QHu/Bp0NWdhJhvDTQx5XNDladNfo0tShptCwsLQL2WLFl2EGUuGfLSiWRFcENXFYHAqhWfzokEyMbaUj17MTgjQRIcm43JLgdArUhp7V+IxHdg==";


            // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
            public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnpFMTlDXODnOPbgTXWGBxzNXc7DrW2U26I5xNOsWAFq3jvJKjxMUx+4zUIPmBQdhLlF0OIt17OE6yYuz5BKROh0UdXf0ENR0MA398pI2GS8rq5dyLJuJBmUFmApJI5y/utC3bI+CY1/LlguPW+CSRyMr3QhWIfB9iK2AzVSclW9TflP7dsRhpHN3P/OrkWEKN3ebivtEV8jLJezFDi8dBM9wU4q5QFmeunFf4nFjx7ToAOEwuoWywupYtdz4bfxKd4RXVDKcQnGpjoOGMRw5T+JIt0pQoEv5d2y3Ri5ycsDG5mqEc4F6ApwUrEFlsHpqArmCejbmvt1ceqVM8s3j2QIDAQAB";


            // 签名方式
            public static string sign_type = "RSA2";

            // 编码格式
            public static string charset = "UTF-8";
        }
    }
}
