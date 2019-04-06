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

namespace ACE_Behind_Mall.WebApi.Controllers
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// 我的信息
    /// </summary>
    public class UserController : ApiController
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        UserBLL userbll = new UserBLL();
        ShopCartBLL shopcartbll = new ShopCartBLL();
        GoodBLL goodbll = new GoodBLL();
        SpecificationBLL specificationbll = new SpecificationBLL();
        [HttpPost]
        public HttpResponseMessage Get(UserLoginBindingModel model)
        {
            var userList = userbll.GetList(x => (x.Account == model.account || x.Email == model.account) & x.Password == model.password & x.IsDelete == 0);
            // HttpCookie cookie = new HttpCookie("userid");
            // cookie.Value = userList.FirstOrDefault().ID.ToString();
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            var resp = new HttpResponseMessage();
            var cookie = new CookieHeaderValue("userid", userList.FirstOrDefault().ID.ToString());
            cookie.Domain = "192.168.0.183";
            cookie.Domain = Request.RequestUri.Host;
            cookie.Domain = ".shit.com";
            cookie.Path = "/";
            resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return resp;

        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"> account:账号/邮箱<br/>password:密码</param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]

        public ModelResponse<dynamic> MyLogin(UserLoginBindingModel model)
        {
            try
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
                var userList = userbll.GetList(x => (x.Account == model.account || x.Email == model.account) & x.Password == model.password & x.IsDelete == 0);
                // HttpCookie cookie = new HttpCookie("userid", userList.FirstOrDefault().ID.ToString());
                //System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                //HttpCookie cookie = new HttpCookie("userid");
                //cookie.Value = userList.FirstOrDefault().ID.ToString();
                //cookie.Domain = "www.shit.com";
                ////cookie.Domain = ".192.168.0.143";
                //cookie.Domain = Request.RequestUri.Host;
                //cookie.Path = "/";
                //System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                if (userList.Count > 0)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, model.account, DateTime.Now,
                           DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", model.account, model.password),
                           FormsAuthentication.FormsCookiePath);
                    var item = userbll.GetList(x => x.IsDelete == 0 & x.Account == model.account).Select(x => new
                    {
                        id = x.ID,
                        email = x.Email,
                        image = "http://192.168.0.143:60391" + x.Image,
                        account = x.Account,
                        receiveName = x.ReceiveName,
                        receivePhone = x.ReceivePhone,
                        receiveAddress = x.ReceiveAddress,
                        addTime = x.CreateTime.ToShortDateString(),
                        Ticket = FormsAuthentication.Encrypt(ticket)
                    });
                    mr.message = "登录成功";
                    mr.data = item;
                    mr.total = item.Count();
                }
                else
                {
                    mr.status = 1;
                    mr.message = "账户或密码错误";
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
        /// 用户注册
        /// </summary>
        /// <param name="model">email:邮箱<br/>account:账号<br/>
        /// password:密码<br/>receiveName:收件人姓名<br/>
        /// receiveAddress:收件人地址<br/>receivePhone:收件人</param>
        /// <returns></returns>
        [HttpPost]
        public ModelResponse<dynamic> MyRegister(UserRegisterModel model)
        {
            var userCount = userbll.GetList(x => x.IsDelete == 0 & x.Account == model.account).Count();
            if (userCount > 0)
            {
                mr.status = 1;
                mr.message = "该账号已存在";
            }
            else
            {
                My_Data mymodel = new My_Data();
                mymodel.Email = model.email;
                mymodel.Account = model.account;
                mymodel.Password = model.password;
                mymodel.Image = "/images/users/userphoto.jpg";
                mymodel.ReceiveName = model.receiveName;
                mymodel.ReceivePhone = model.receivePhone;
                mymodel.ReceiveAddress = model.receiveAddress;
                mymodel.CreateTime = DateTime.Now;
                mymodel.IsDelete = 0;
                try
                {
                    bool flag = userbll.Add(mymodel);
                    if (flag == true)
                    {
                        mr.message = "注册成功";
                        mr.total = 1;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
            return mr;
        }
        /// <summary>
        /// 获取用户购物车
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ModelResponse<dynamic> GetMyShopCart(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    mr.status = 2;//未登录
                    mr.message = "请先登录";
                }
                if (userId != 0)
                {
                    var model = shopcartbll.GetList(x => x.IsDelete == 0 & x.UserID == userId).Select(x => new
                    {
                        goodId=x.GoodID,
                        goodStock= goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Stock,
                        goodImage = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().CoverImage,
                        goodName = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Name,
                        goodPrice = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice,
                        number = x.Number,
                        subTotal = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice * x.Number,
                    });
                    mr.data = model;
                    mr.total = model.Count();
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
        /// 加入购物车
        /// </summary>
        /// <param name="model">userId:用户ID<br/>goodId:商品ID<br/>number:商品数量</param>
        /// <returns></returns>
        [HttpPost]
        public ModelResponse<dynamic> AddMyShopCart(MyShopCartModel model)
        {
            if (model.userId == 0)
            {
                mr.status = 2;//未登录
                mr.message = "请先登录";
            }
            var usermodel = userbll.GetList(x => x.ID == model.userId).Count();
            if (model.userId != 0&&usermodel>0)
            {
                try
                {
                    int good_stock = goodbll.GetList(x => x.IsDelete == 0 & x.ID == model.goodId).FirstOrDefault().Stock;
                    if (model.number > good_stock)//库存不足
                    {
                        mr.status = 1;
                        mr.message = "商品加购件数(含已加购件数)已超过库存";
                    }
                    else//库存充足
                    {
                        var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == model.userId && x.GoodID == model.goodId).FirstOrDefault();
                        bool flag = false;
                        if (shopcartmodel == null)
                        {
                            My_Shopcart mymodel = new My_Shopcart();
                            mymodel.UserID = model.userId;
                            mymodel.GoodID = model.goodId;
                            mymodel.Number = model.number;
                            mymodel.CreateTime = DateTime.Now;
                            mymodel.IsDelete = 0;
                            flag = shopcartbll.Add(mymodel);
                        }
                        else
                        {
                            var totalNumber = model.number + shopcartmodel.Number;
                            if (good_stock < totalNumber)
                            {
                                mr.status = 1;
                                mr.message = "商品加购件数(含已加购件数)已超过库存";
                                flag = false;
                            }
                            else
                            {
                                shopcartmodel.Number += model.number;
                                My_Shopcart m = shopcartbll.GetUpdateModel<My_Shopcart>(shopcartmodel, "ID");
                                flag = shopcartbll.Update(m);
                            }            
                        }
                        if (flag == true)
                        {
                            var goodmodel = goodbll.GetList(x => x.IsDelete == 0 && x.ID == model.goodId).FirstOrDefault();
                            //goodmodel.SaleNumber += model.number;
                            //goodmodel.Stock -= model.number;
                            Mall_Good m = goodbll.GetUpdateModel<Mall_Good>(goodmodel, "ID");
                            goodbll.Update(m);
                            mr.message = "添加成功";
                            mr.total = 1;
                        }
                    }
                }
                catch (Exception e)
                {
                    mr.status = 1;
                    Log.Error(e.Message);
                }
            }
            return mr;
        }
        /// <summary>
        /// 修改购物车数量
        /// </summary>
        /// <param name="model">userId:用户ID<br/>goodId:商品ID<br/>number:商品数量</param>
        /// <returns></returns>
        [HttpPost]
        public ModelResponse<dynamic> UpdateShopCartNumber(MyShopCartModel model)
        {
            try
            {
                int good_stock = goodbll.GetList(x => x.IsDelete == 0 & x.ID == model.goodId).FirstOrDefault().Stock;
                if (model.number > good_stock)//库存不足
                {
                    mr.status = 1;
                    mr.message = "商品加购件数(含已加购件数)已超过库存";
                }
                else
                {
                    var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == model.userId && x.GoodID == model.goodId).FirstOrDefault();
                    shopcartmodel.Number = model.number;
                    My_Shopcart m = shopcartbll.GetUpdateModel<My_Shopcart>(shopcartmodel, "ID");
                    bool flag = shopcartbll.Update(m);
                    if (flag == true)
                    {

                        var myshopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 & x.UserID == model.userId).Select(x => new
                        {
                            goodId = x.GoodID,
                            goodStock = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Stock,
                            goodImage = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().CoverImage,
                            goodName = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Name,
                            goodPrice = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice,
                            number = x.Number,
                            subTotal = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice * x.Number,
                        });
                        mr.data = myshopcartmodel;
                        mr.message = "修改成功";
                        mr.total = 1;
                        
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
        /// 删除购物车商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        public ModelResponse<dynamic> DeleteShopCartGood(DeleteShopCartGoodModel model)
        {
            try
            {
                var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == model.userId && x.GoodID == model.goodId).FirstOrDefault();
                shopcartmodel.IsDelete = 1;
                My_Shopcart m = shopcartbll.GetUpdateModel<My_Shopcart>(shopcartmodel, "ID");
                bool flag = shopcartbll.Update(m);
                if (flag == true)
                {
                    mr.message = "删除成功";
                    mr.total = 1;
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
