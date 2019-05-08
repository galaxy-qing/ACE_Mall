using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// config 的摘要说明
/// </summary>
namespace ACE_Mall.Common
{
    public class config
    {
        public config()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        // 应用ID,您的APPID
        public static string app_id = "2019033163690973";

        // 支付宝网关
        public static string gatewayUrl = "https://openapi.alipay.com/gateway.do";

        // 商户私钥，您的原始格式RSA私钥
        public static string private_key = "jxFDsvzDvsuOGf2bbR4Cjw==";

        // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAr1ZOPB45X5ZutEkb4ETVJ0tlcMZ5gmB93e84Ij4G+HUrYLOp3/RnM1ChqU43e2q4wBMN+/XVpUrTe7mNAY59U7/4CTPKvNSqbMzUt9pXm+V+tyAAYQmZUD+jP6AAkBidkpAc4vwqCX6WP4MtWePNspM566aodFYTlGtPULWypxOfqxsQYBAv3l/QDg78qBaInTCOwEUMtMin/qb/WaendDCFzWe6SvIrRtJ9tR8c9tTJGgxAxE8GuPqF+Yx1PNs58/63FxsMrFFgKbyoM72u4nu3U1oqTDOQMlWYagnCgzzIRRsKBZ9JHg96YphonPbjRIHkALLRmLOXvzOEpqi8cwIDAQAB";

        // 签名方式
        public static string sign_type = "RSA2";

        // 编码格式
        public static string charset = "UTF-8";
    }
}