using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// config 的摘要说明
/// </summary>
public class config
{
    public config()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    // 应用ID,您的APPID
    public static string app_id = "2016092700604341";

    // 支付宝网关
    public static string gatewayUrl = "https://openapi.alipaydev.com/gateway.do";

    // 商户私钥，您的原始格式RSA私钥
    public static string private_key = "MIIEpAIBAAKCAQEAtaWn9+3hgjIJLisH6W6fXk0PLuzFOffFdTZCfNFzwRMPu0ngXN7bNaXSOP55nPizsDvnjOTQPgGlEQj5q2G/6hfvHcSukLWR4Gp8wTgHzZAa8Us7w+sr49XK2PyursU8vwC3zc2JOLytH9mVGg8ky5b5uZYJpiZAM93NEFn0qKQObDrPuKi6S4roee7DrD4TglU20y2QbDJV9QUO/h2khz40N1ivkb/Ssz2/6gegCO1B+qbDqtfn/yMpqIpGLh3ex5TiRUi0o7bE65aI8cy403CXB088T47d/ivemj719+PMDoz4j4DyrZuytJAcKyalejFy/dTf7xfzE3nBrCubLwIDAQABAoIBAErjR/OcPAucK1LAZNulQsjzBh53ePxkHSEsxI6HH6zq+eJiG8DOFBvzCE99ApBnrFUs7VKWBxB5Rx9VzvEMNL4DLg7cxodchA68eECEpBronWL7fGFSdF7TBnUc/7uID3pDhoOviDI+/zURVDpRIf9ZeA0+QS4huPhpKiDipU5c2lN80VQb4Mx/CPzILHgVlUA9Haw7cVQdaMS4g9EDapvSe840awtqI0623tLzWlVVRFO1kf0OcYaqrZXnNPrUwz7c4wbqP88s//+Vlj4rt5yXq31iIpNOAtnpx8Hf5GLf3oY1ixSSentK2/tI9FCYMseDsfMYvrYls1jtSmazBckCgYEA6FmYe6Efrf0HBPIOA/u9+mgtMBCqohGFqPkXrocz6B0Es1XgyJGpjNR1t41YItFaGBKeBMF28Sq238Ga+TwN3c6JUgS++ltpqsjAassG3h82fIKNnOBdywFFfinWOrI4xIB8JL/dFqdXPTjB8XYHsEnasOq43G5vWHxEh+3ow7sCgYEAyCLhzVA7DWIewm/eAhiSPTjLX39kCbF3Xxv/LzMvUpamsdl6nodxRYyrw6Ipor/zG9/ZUVRYZySnXwjur5GLhragHsFTl1hCMechSuc+n1KJ+bnKWXfYmzoZ6dFUjqN08/XVUDbc5bgpzb1502ks6R7hvWCKXmBkmTOOJMGv3R0CgYEAln7Z7lHZpCd88W4bN+dKETSC+FwDOcBhs6XL+gamz1RkZAhe9WqryIUgzkgl6z9wvKDqaygoc2L4WowbZg3I6X9KoK5kSOMG2VD4mPNyOlM7RoCFWzRUbBYhves5UIF5lYWnY2JGwNKZBhWKQWzy4/OiS+9s15JMWqL6/JRTrNsCgYBRC2cbCFFRgxlnrXtep+qYZiZdHq45CqeHWUQGLhkMvbr1LFduWPSysFtiFon7wPVGpWhQefJumjY40totOKgivlOrAKjEEdaEdM9TkL8YL4l2GhlqD5Ekkuupdr5iIKkcncFrATyEvgYXrZHm3aF1Ka9KTAzcWPaD34/BLLn1tQKBgQCkr4B6z5vwbA2NVSL6uQp3fM2m6EJb5YFPIOnL6Zx/25g9pxdISDD2SK+f4UhCXPJchrr2Cq4lRnOQ7u0Bnw4ZFGdSb0hD/DTuhaTFxNgQQWbOcQbOyIkEQuZTSfMkcoFvpWEcqNAhEN6PSVklu9tAWfwEi/kM3EXc5nhq+4MiwQ==";


    // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
    public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAu3wgGzFzl6gNeCdastRmI5nL/9pno2ENN13quRjpviAYzmG6CnIIIC0JhnlkwR3bU7Yj4FX6az3TD5P8UPp+MSxW8XC9vaPX2zUKxs35ZaBSuRePusdv3rqQ6HbtroLt5jVZ5fI1qqyn9HqoI/oFaOzi+DXmFntlKszMrwKBIbUE90ooHdAmQA2Pv1e1mJRi/4q0svU0mrNigQUgcu/PHsOt9PWmUKyl2EBGCLx2HHGLgKPY8ttfUYqzEFZGW7ZjP45982rwSIZH64WYul86ClVutrOmlZkHo6cDkugGURx5SEqzTvngvMRsPpzvk20OWOSZ7CQuUmeDtqmWz4Y0IQIDAQAB";


    // 签名方式
    public static string sign_type = "RSA2";

    // 编码格式
    public static string charset = "UTF-8";
}