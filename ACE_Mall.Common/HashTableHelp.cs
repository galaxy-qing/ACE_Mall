using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace ACE_Mall.Common
{
    public class HashTableHelp
    {
        public static Hashtable GetHash(int flag)
        {
            Hashtable ht = new Hashtable();
            if (flag > 0)
            {
                ht["result"] = true;
                ht["message"] = "操作执行成功";
            }
            else
            {
                ht["result"] = false;
                ht["message"] = "服务器内部错误";
            }
            return ht;
        }
        public static Hashtable GetHash(bool flag)
        {
            Hashtable ht = new Hashtable();
            if (flag == true)
            {
                ht["result"] = true;
                ht["message"] = "操作执行成功";
            }
            else
            {
                ht["result"] = false;
                ht["message"] = "服务器内部错误";
            }
            return ht;
        }
    }
}
