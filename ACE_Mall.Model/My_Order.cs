//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACE_Mall.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class My_Order
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string OrderNo { get; set; }
        public int OrderState { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Nullable<decimal> PayMoney { get; set; }
        public Nullable<int> PayWay { get; set; }
        public string Note { get; set; }
        public string CourierName { get; set; }
        public string CourierNo { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
        public Nullable<System.DateTime> DeliveryTime { get; set; }
        public Nullable<System.DateTime> CompleteTime { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public int IsDelete { get; set; }
    }
}
