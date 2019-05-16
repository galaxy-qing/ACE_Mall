using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Mall.Common.App_Code
{
    public class AliPayHelper
    {
        public static string AliPayInfo(string out_trade_no, string total_amount, string subject)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //Log.Error("用户个人佣金或余额提现开始", "");
            dic.Add("out_trade_no", out_trade_no);//商户转账唯一订单号
            dic.Add("total_amount", total_amount);//收款方账户类型
            dic.Add("subject", "");//收款方账户
            //dic.Add("store_id", "");//转账金额
            //dic.Add("timeout_express", "90m");//付款方姓名
            //dic.Add("payee_real_name", payee_real_name);//收款方姓名
            //dic.Add("remark", remark);//转账备注
            return JsonConvert.SerializeObject(dic);
        }

    }
}
