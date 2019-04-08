using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Mall.Common
{
    public class PageReq
    {
        public int page { get; set; }
        public int limit { get; set; }
        public string key { get; set; }
        public string key1 { get; set; }
        public string key2 { get; set; }
        public string key3 { get; set; }
        public string state { get; set; }

        public PageReq()
        {
            key = "";
            key1 = "";
            key2 = "";
            key3 = "";
            state = "";
            page = 1;
            limit = 10;
        }
    }
}
