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
    
    public partial class Mall_Good_Evaluation
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> GoodID { get; set; }
        public Nullable<int> Star { get; set; }
        public string Evaluation { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int IsDelete { get; set; }
    }
}
