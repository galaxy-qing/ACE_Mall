﻿using System;
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
                    Progress = x.OrderState == 6 ? "100%" : (double)x.OrderState / 5 * 100 + "%",
                    x.CompleteTime,
                    x.CourierName,
                    x.CourierNo,
                    x.DeliveryTime,
                    x.CreateTime,
                    x.IsDelete,
                }).ToList();
                if (!string.IsNullOrEmpty(request.state))//下拉框搜索orderState
                {
                    if (request.state == "1")
                    {
                        item = item.Where(x => x.OrderState<=2).ToList();

                    }
                    if (request.state == "2")
                    {
                        item = item.Where(x => x.OrderState > 2).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(request.key1))//关键字搜索orderNo
                {
                    item = item.Where(x => x.OrderNo.Contains(request.key1.Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(request.key2))//关键字搜索receiveName
                {
                    item = item.Where(x => x.Name.Contains(request.key2.Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(request.key3))//关键字搜索receivePhone
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
    }
}