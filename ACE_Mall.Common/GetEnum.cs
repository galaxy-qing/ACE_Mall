using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Mall.Common
{
    public class GetEnum
    {
        #region 订单类型
        /// <summary>
        /// 订单支付类别
        /// </summary>
        public enum order_state
        {
            /// <summary>
            /// 全部
            /// </summary>
            全部 = 0,
            /// <summary>
            /// 待支付
            /// </summary>
            待支付 = 1,
            /// <summary>
            /// 待发货
            /// </summary>
            待发货 = 2,
            /// <summary>
            /// 待收货
            /// </summary>
            待收货 = 3,
            /// <summary>
            /// 待评价
            /// </summary>
            待评价 = 4,
            /// <summary>
            /// 待评价
            /// </summary>
            已完成 = 5,
            /// <summary>
            /// 已取消
            /// </summary>
            已取消 = 6,
        }
        #endregion
    }
}
