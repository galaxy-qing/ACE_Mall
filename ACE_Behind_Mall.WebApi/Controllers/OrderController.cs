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

                DefaultAopClient client = new DefaultAopClient(config.gatewayUrl, config.app_id, config.private_key, "json", "1.0", config.sign_type, config.alipay_public_key, config.charset, false);

                // 外部订单号，商户网站订单系统中唯一的订单号
                string out_trade_no = myorder.OrderNo;

                // 订单名称
                string subject = "ACE货物购买";

                // 付款金额
                string total_amout = model.payMoney.ToString();

                // 商品描述
                string body = model.note;

                // 组装业务参数model
                AlipayTradePagePayModel paymodel = new AlipayTradePagePayModel();
                paymodel.Body = body;
                paymodel.Subject = subject;
                paymodel.TotalAmount = total_amout;
                paymodel.OutTradeNo = out_trade_no;
                paymodel.ProductCode = "FAST_INSTANT_TRADE_PAY";

                AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
                // 设置同步回调地址
                request.SetReturnUrl("http://192.168.0.198:8088/dist/view/order.html?type=2");
                // 设置异步通知接收地址
                request.SetNotifyUrl("http://192.168.0.144:60391/Notify_url.aspx");
                // 将业务model载入到request
                request.SetBizModel(paymodel);

                AlipayTradePagePayResponse response = null;
                try
                {
                    response = client.pageExecute(request, null, "post");
                    mr.data = response.Body;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
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
                    if (ordermodel.OrderState >=5)
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
            public static string app_id = "2016092700604341";

            // 支付宝网关
            public static string gatewayUrl = "https://openapi.alipaydev.com/gateway.do";

            // 商户私钥，您的原始格式RSA私钥
            public static string private_key = "MIIEpAIBAAKCAQEAtaWn9+3hgjIJLisH6W6fXk0PLuzFOffFdTZCfNFzwRMPu0ngXN7bNaXSOP55nPizsDvnjOTQPgGlEQj5q2G/6hfvHcSukLWR4Gp8wTgHzZAa8Us7w+sr49XK2PyursU8vwC3zc2JOLytH9mVGg8ky5b5uZYJpiZAM93NEFn0qKQObDrPuKi6S4roee7DrD4TglU20y2QbDJV9QUO/h2khz40N1ivkb/Ssz2/6gegCO1B+qbDqtfn/yMpqIpGLh3ex5TiRUi0o7bE65aI8cy403CXB088T47d/ivemj719+PMDoz4j4DyrZuytJAcKyalejFy/dTf7xfzE3nBrCubLwIDAQABAoIBAErjR/OcPAucK1LAZNulQsjzBh53ePxkHSEsxI6HH6zq+eJiG8DOFBvzCE99ApBnrFUs7VKWBxB5Rx9VzvEMNL4DLg7cxodchA68eECEpBronWL7fGFSdF7TBnUc/7uID3pDhoOviDI+/zURVDpRIf9ZeA0+QS4huPhpKiDipU5c2lN80VQb4Mx/CPzILHgVlUA9Haw7cVQdaMS4g9EDapvSe840awtqI0623tLzWlVVRFO1kf0OcYaqrZXnNPrUwz7c4wbqP88s//+Vlj4rt5yXq31iIpNOAtnpx8Hf5GLf3oY1ixSSentK2/tI9FCYMseDsfMYvrYls1jtSmazBckCgYEA6FmYe6Efrf0HBPIOA/u9+mgtMBCqohGFqPkXrocz6B0Es1XgyJGpjNR1t41YItFaGBKeBMF28Sq238Ga+TwN3c6JUgS++ltpqsjAassG3h82fIKNnOBdywFFfinWOrI4xIB8JL/dFqdXPTjB8XYHsEnasOq43G5vWHxEh+3ow7sCgYEAyCLhzVA7DWIewm/eAhiSPTjLX39kCbF3Xxv/LzMvUpamsdl6nodxRYyrw6Ipor/zG9/ZUVRYZySnXwjur5GLhragHsFTl1hCMechSuc+n1KJ+bnKWXfYmzoZ6dFUjqN08/XVUDbc5bgpzb1502ks6R7hvWCKXmBkmTOOJMGv3R0CgYEAln7Z7lHZpCd88W4bN+dKETSC+FwDOcBhs6XL+gamz1RkZAhe9WqryIUgzkgl6z9wvKDqaygoc2L4WowbZg3I6X9KoK5kSOMG2VD4mPNyOlM7RoCFWzRUbBYhves5UIF5lYWnY2JGwNKZBhWKQWzy4/OiS+9s15JMWqL6/JRTrNsCgYBRC2cbCFFRgxlnrXtep+qYZiZdHq45CqeHWUQGLhkMvbr1LFduWPSysFtiFon7wPVGpWhQefJumjY40totOKgivlOrAKjEEdaEdM9TkL8YL4l2GhlqD5Ekkuupdr5iIKkcncFrATyEvgYXrZHm3aF1Ka9KTAzcWPaD34/BLLn1tQKBgQCkr4B6z5vwbA2NVSL6uQp3fM2m6EJb5YFPIOnL6Zx/25g9pxdISDD2SK+f4UhCXPJchrr2Cq4lRnOQ7u0Bnw4ZFGdSb0hD/DTuhaTFxNgQQWbOcQbOyIkEQuZTSfMkcoFvpWEcqNAhEN6PSVklu9tAWfwEi/kM3EXc5nhq+4MiwQ==";


            // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
            public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAu3wgGzFzl6gNeCdastRmI5nL/9pno2ENN13quRjpviAYzmG6CnIIIC0JhnlkwR3bU7Yj4FX6az3TD5P8UPp+MSxW8XC9vaPX2zUKxs35ZaBSuRePusdv3rqQ6HbtroLt5jVZ5fI1qqyn9HqoI/oFaOzi+DXmFntlKszMrwKBIbUE90ooHdAmQA2Pv1e1mJRi/4q0svU0mrNigQUgcu/PHsOt9PWmUKyl2EBGCLx2HHGLgKPY8ttfUYqzEFZGW7ZjP45982rwSIZH64WYul86ClVutrOmlZkHo6cDkugGURx5SEqzTvngvMRsPpzvk20OWOSZ7CQuUmeDtqmWz4Y0IQIDAQAB";


            // 签名方式
            public static string sign_type = "RSA2";

            // 编码格式
            public static string charset = "UTF-8";
        }
    }
}
