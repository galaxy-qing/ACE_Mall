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
    
    public partial class Adm_Log
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Account { get; set; }
        public string OpType { get; set; }
        public string OpContent { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
