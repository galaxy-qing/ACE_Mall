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
        public string state { get; set; }

        public PageReq()
        {
            key = "";
            state = "";
            page = 1;
            limit = 10;
        }
    }
}
