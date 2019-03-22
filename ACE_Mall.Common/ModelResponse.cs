using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Mall.Common
{
    public class ModelResponse<T>
    {
        public ModelResponse()
        {
            status = 200;
            message = "";
            total = 0;
        }
        public int status { get; set; }
        public string message { get; set; }
        public int total { get; set; }
        public T rows { get; set; }
        public T data { get; set; }
    }
    public class ModelRequest<T>
    {
        public T body { get; set; }
    }
}
