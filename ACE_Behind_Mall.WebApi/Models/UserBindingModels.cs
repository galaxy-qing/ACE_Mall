﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACE_Behind_Mall.WebApi.Models
{
    /// <summary>
    /// 用户model
    /// </summary>
    public class UserBindingModels
    {
        /// <summary>
        /// 用户登录model
        /// </summary>
        public class UserLoginBindingModel
        {
            [Required]
            [Display(Name = "账号/邮箱")]
            public string account { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string password { get; set; }
        }
        /// <summary>
        /// 用户注册model
        /// </summary>
        public class UserRegisterModel
        {
            [Required]
            [Display(Name = "邮箱")]
            public string email { get; set; }
            [Required]
            [Display(Name = "账号")]
            public string account { get; set; }
            [Required]
            [Display(Name = "密码")]
            public string password { get; set; }
            [Required]
            [Display(Name = "收件人姓名")]
            public string receiveName { get; set; }
            [Required]
            [Display(Name = "收件人地址")]
            public string receiveAddress { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "收件人手机")]
            public string receivePhone { get; set; }
        }
        /// <summary>
        /// 加入购物车model
        /// </summary>
        public class MyShopCartModel
        {
            [Required]
            [Display(Name = "用户ID")]
            public int userId { get; set; }
            [Required]
            [Display(Name = "商品ID")]
            public int goodId { get; set; }
            [Required]
            [Display(Name = "数量")]
            public int number { get; set; }
        }
        /// <summary>
        /// 删除购物车商品
        /// </summary>
        public class DeleteShopCartGoodModel
        {
            [Required]
            [Display(Name = "用户ID")]
            public int userId { get; set; }
            [Required]
            [Display(Name = "商品ID")]
            public int goodId { get; set; }
        }
        /// <summary>
        /// 是否选中购物车商品
        /// </summary>
        public class GetGoodsChecked
        {

            [Required]
            [Display(Name = "用户ID")]
            public int userId { get; set; }
            [Required]
            [Display(Name = "商品ID")]
            public int goodId { get; set; }
            [Required]
            [Display(Name = "是否选中")]
            public bool isChecked { get; set; }
        }
        /// <summary>
        /// 是否全选购物车商品
        /// </summary>
        public class GetGoodsAllChecked
        {
            [Required]
            [Display(Name = "用户ID")]
            public int userId { get; set; }
            [Required]
            [Display(Name = "是否选中")]
            public bool isChecked { get; set; }
        }
        public class PostOrderShow
        {
            [Required]
            [Display(Name = "用户ID")]
            public int userId { get; set; }
        }
        public class SubmitOrder
        {
            [Required]
            [Display(Name = "用户ID")]
            public int userId { get; set; }
            [Required]
            [Display(Name = "用户留言")]
            public string note { get; set; }
            [Required]
            [Display(Name = "支付金额")]
            public decimal payMoney { get; set; }
        }
        public class PayOrder
        {
            [Required]
            [Display(Name = "订单号")]
            public string orderNo { get; set; }
        }
    }
}