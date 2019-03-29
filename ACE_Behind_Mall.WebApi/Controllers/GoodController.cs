using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ACE_Mall.BLL;
using ACE_Mall.Common;
using NLog.Fluent;

namespace ACE_Behind_Mall.WebApi.Controllers
{
    /// <summary>
    /// 商品
    /// </summary>
    public class GoodController : ApiController
    {
        protected ModelResponse<dynamic> mr = new ACE_Mall.Common.ModelResponse<dynamic>();
        GoodBLL goodsbll = new GoodBLL();
        /// <summary>
        /// 商品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetGoods()
        {
            try
            {
                var item = goodsbll.GetList(x => x.IsDelete == 0);
                if (item.Count > 0)
                {
                    mr.status = 0;
                    mr.total = item.Count;
                    mr.data = item;
                }
            }
            catch (Exception e)
            {
                mr.status = 500;
                mr.data = goodsbll.GetList(x => x.IsDelete == 0);
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
    }
}
