﻿using ACE_Behind_Mall.WebApi.App_Start;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using ACE_Mall.Model;
using NLog.Fluent;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Security;
using static ACE_Behind_Mall.WebApi.Models.UserBindingModels;

namespace ACE_Behind_Mall.WebApi.Controllers
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// 我的信息
    /// </summary>
    public class UserController : BasicController
    {
        private UserBLL userbll = new UserBLL();
        private ShopCartBLL shopcartbll = new ShopCartBLL();
        private GoodBLL goodbll = new GoodBLL();
        private OrderBLL orderbll = new OrderBLL();
        private OrderGoodBLL ordergoodbll = new OrderGoodBLL();
        private SpecificationBLL specificationbll = new SpecificationBLL();
        private EvaluationBLL evaluationbll = new EvaluationBLL();
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
                        image = x.Image,
                        account = x.Account,
                        receiveName = x.ReceiveName,
                        receivePhone = x.ReceivePhone,
                        receiveAddress = x.ReceiveAddress,
                        addTime = x.CreateTime.ToShortDateString(),
                        Ticket = FormsAuthentication.Encrypt(ticket)
                    });
                    mr.message = "登录成功";
                    mr.data = item;
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
        /// 个人中心信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> GetMyData()
        {
            // GetUserId(userTicket);
             int userId=GetTicket();
             var usermodel = userbll.GetList(x => x.IsDelete == 0 && x.ID == userId).Select(x => new
            {
                account = x.Account,
                cretateTime = Convert.ToDateTime(x.CreateTime).ToString("yyyy-MM-dd HH:MM:SS"),
                email = x.Email,
                id = x.ID,
                image = x.Image,
                password = x.Password,
                receiveAddress = x.ReceiveAddress,
                receiveName = x.ReceiveName,
                x.ReceivePhone,
            });
            if (usermodel.Count()==0)
            {
                mr.status = 2;
                mr.message = "请前往登录";
            }
            else
            {
                mr.data = usermodel;
            }
            return mr;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> UpdateMyPhoto(Files model)
        {
            if (model.file.Count() < 1)
            {
                mr.status = 1;
                mr.message = "请上传图片";
            }
            string result = FileHelper.SaveImage(model.file);
            if (result == "err")
            {
                mr.status = 1;
                mr.message = "服务器错误";
            }
            mr.data = new { img = result };
            return mr;
        }
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> UpdateMyData(MyDataInfo model)
        {
            int userId = GetTicket();
            try
            {
                var usermodel = userbll.GetList(x => x.IsDelete == 0 && x.ID == userId).FirstOrDefault();
                usermodel.Email = model.email;
                usermodel.Image = model.image;
                usermodel.Password = model.password;
                usermodel.ReceiveAddress = model.receiveAddress;
                usermodel.ReceiveName = model.receiveName;
                usermodel.ReceivePhone = model.receivePhone;
                My_Data m = userbll.GetUpdateModel<My_Data>(usermodel, "ID");
                bool flag = userbll.Update(m);
                mr.message = "修改成功";
            }
            catch (Exception e)
            {
                mr.status = 1;
                Log.Error(e.Message);
            }
            return mr;
        }
        /// <summary>
        /// 获取用户购物车
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> GetMyShopCart()
        {
            int userId = 0;
            userId = GetTicket();
            try
            {
                if (userId == 0)
                {
                    mr.status = 2;//未登录
                    mr.message = "请先登录";
                }
                if (userId != 0)
                {
                    getModel(userId, 1);
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
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> AddMyShopCart(MyShopCartModel model)
        {
            int userId = GetTicket();
            if (userId == 0)
            {
                mr.status = 2;//未登录
                mr.message = "请先登录";
            }
            var usermodel = userbll.GetList(x => x.ID == userId).Count();
            if (userId != 0 && usermodel > 0)
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
                        var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.GoodID == model.goodId).FirstOrDefault();
                        bool flag = false;
                        if (shopcartmodel == null)
                        {
                            My_Shopcart mymodel = new My_Shopcart();
                            mymodel.UserID = userId;
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
                            mr.message = "添加成功";
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
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> UpdateShopCartNumber(MyShopCartModel model)
        {
            int userId = GetTicket();
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
                    var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.GoodID == model.goodId).FirstOrDefault();
                    shopcartmodel.Number = model.number;
                    My_Shopcart m = shopcartbll.GetUpdateModel<My_Shopcart>(shopcartmodel, "ID");
                    bool flag = shopcartbll.Update(m);
                    if (flag == true)
                    {
                        getModel(userId, 1);
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
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> DeleteShopCartGood(DeleteShopCartGoodModel model)
        {
            int userId = GetTicket();
            try
            {
                var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.GoodID == model.goodId).FirstOrDefault();
                shopcartmodel.IsDelete = 1;
                My_Shopcart m = shopcartbll.GetUpdateModel<My_Shopcart>(shopcartmodel, "ID");
                bool flag = shopcartbll.Update(m);
                if (flag == true)
                {
                    getModel(userId, 1);
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
        /// 获取商品选中状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> SetGoodsChecked(GetGoodsChecked model)
        {
            int userId = GetTicket();
            try
            {
                var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.GoodID == model.goodId).FirstOrDefault();
                shopcartmodel.IsChecked = model.isChecked;
                My_Shopcart m = shopcartbll.GetUpdateModel<My_Shopcart>(shopcartmodel, "ID");
                bool flag = shopcartbll.Update(m);
                if (flag == true)
                {
                    getModel(userId, 1);
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
        /// 获取商品是否全选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ModelResponse<dynamic> SetGoodsAllChecked(GetGoodsAllChecked model)
        {
            int userId = GetTicket();
            try
            {
                var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId);
                foreach (var item in shopcartmodel)
                {
                    item.IsChecked = model.isChecked;
                    My_Shopcart m = shopcartbll.GetUpdateModel<My_Shopcart>(item, "ID");
                    bool flag = shopcartbll.Update(m);
                    if (flag == true)
                    {
                        getModel(userId, 1);
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
        /// 去结算
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [RequestAuthorize]
        public ModelResponse<dynamic> GetShopCartShow()
        {
            int userId = GetTicket();
            try
            {
                var shopcartmodel = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId);
                foreach (var item in shopcartmodel)
                {
                    //getModel(userId, 2);
                    var model = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.IsChecked == true);
                    var usermodel = userbll.GetList(y => y.ID == userId).FirstOrDefault();
                    var mymodel = model.Select(x => new
                    {
                        receiveName = usermodel.ReceiveName == null ? "" : usermodel.ReceiveName,
                        receivePhone = usermodel.ReceivePhone == null ? "" : usermodel.ReceivePhone,
                        receiveAddress = usermodel.ReceiveAddress == null ? "" : usermodel.ReceiveAddress,
                        goodId = x.GoodID == 0 ? 0 : x.GoodID,
                        goodStock = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Stock == 0 ? 0 : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Stock,
                        goodImage = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().CoverImage == null ? "" : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().CoverImage,
                        goodName = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Name == null ? "" : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Name,
                        goodPrice = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice == null ? 0 : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice,
                        number = x.Number == 0 ? 0 : x.Number,
                        isChecked = x.IsChecked,
                        subTotal = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice * x.Number == 0 ? 0 : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice * x.Number,
                    });
                    int totalNumber = 0;
                    decimal totalPrice = 0;
                    int checkNumber = mymodel.Where(x => x.isChecked == true).Count();
                    foreach (var item1 in mymodel.Where(x => x.isChecked == true))
                    {
                        totalPrice += Convert.ToDecimal(item1.subTotal);
                        totalNumber += Convert.ToInt32(item1.number);
                    }
                    mr.data = new { totalNumber = totalNumber, checkNumber = checkNumber, totalPrice = totalPrice, mymodel };
                    mr.message = "数据加载成功";
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
        /// 获取我的评价
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [RequestAuthorize]
        public ModelResponse<dynamic> GetMyEvaluation()
        {

            int userId = 0;
            userId = GetTicket();
            try
            {
                if (userId == 0)
                {
                    mr.status = 2;//未登录
                    mr.message = "请先登录";
                }
                if (userId != 0)
                {
                    var model = evaluationbll.GetList(x => x.IsDelete == 0 && x.UserID == userId).Select(x => new
                    {
                        goodName=goodbll.GetList(y=>y.IsDelete==0&&y.ID==x.GoodID).FirstOrDefault().Name,
                        goodImage = goodbll.GetList(y => y.IsDelete == 0 && y.ID == x.GoodID).FirstOrDefault().CoverImage,
                        star =x.Star,
                        evaluation=x.Evaluation,
                        createtime=x.CreateTime,
                    });
                    mr.message = "数据加载成功";
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
        public ModelResponse<dynamic> levenshtein(String str1, String str2)
        {
            //计算两个字符串的长度。 
            int len1 = str1.Length;
            int len2 = str2.Length;
            //建立上面说的数组，比字符长度大一个空间 
            int[,] dif = new int[len1 + 1, len2 + 1];
            //赋初值，步骤B。 
            for (int a = 0; a <= len1; a++)
            {
                dif[a, 0] = a;
            }
            for (int a = 0; a <= len2; a++)
            {
                dif[0, a] = a;
            }
            //计算两个字符是否一样，计算左上的值 
            int temp;
            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    //取三个值中最小的 
                    dif[i, j] = min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1,
                    dif[i - 1, j] + 1);
                }
            }

            //计算相似度 
            float similarity = 1 - (float)dif[len1, len2] / Math.Max(str1.Length, str2.Length);
            mr.message= similarity.ToString();
            return mr;
        }

        //得到最小值 
        private static int min(params int[] arr)
        {
            int min = int.MaxValue;
            foreach (int i in arr)
            {
                if (min > i)
                {
                    min = i;
                }
            }
            return min;
        }
        /// <summary>
        /// 获取购物车
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        public void getModel(int userId, int type)
        {
            var model = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId);
            if (type == 2)
            {
                model = shopcartbll.GetList(x => x.IsDelete == 0 && x.UserID == userId && x.IsChecked == true);
            }
            var usermodel = userbll.GetList(y => y.ID == userId).FirstOrDefault();
            var mymodel = model.Select(x => new
            {
                //receiveName = usermodel.ReceiveName==null?"": usermodel.ReceiveName,
                //receivePhone = usermodel.ReceivePhone == null ? "" : usermodel.ReceivePhone,
                // receiveAddress = usermodel.ReceiveAddress == null ? "" : usermodel.ReceiveAddress,
                goodId = x.GoodID == 0 ? 0 : x.GoodID,
                goodStock = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Stock == 0 ? 0 : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Stock,
                goodImage = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().CoverImage == null ? "" : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().CoverImage,
                goodName = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Name == null ? "" : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().Name,
                goodPrice = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice == null ? 0 : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice,
                number = x.Number == 0 ? 0 : x.Number,
                isChecked = x.IsChecked,
                subTotal = goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice * x.Number == 0 ? 0 : goodbll.GetList(y => y.IsDelete == 0 & y.ID == x.GoodID).FirstOrDefault().PresentPrice * x.Number,
            });
            decimal totalPrice = 0;
            int totalNumber = mymodel.Count();
            int checkNumber = mymodel.Where(x => x.isChecked == true).Count();
            foreach (var item1 in mymodel.Where(x => x.isChecked == true))
            {
                totalPrice += Convert.ToDecimal(item1.subTotal);
            }
            bool isAllChecked = shopcartbll.GetList(y => y.IsDelete == 0 && y.UserID == userId && y.IsChecked == false).Count == 0 ? true : false;
            mr.data = new { isAllChecked = isAllChecked, totalNumber = totalNumber, checkNumber = checkNumber, totalPrice = totalPrice, mymodel };
            mr.message = "数据加载成功";
        }
    }
}
