using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WXOrdrPlatform.Models
{
    public class order
    {
        public string id { set; get; }
        public string orderNo { set; get; }
        public string productId { set; get; }
        public string productName { set; get; }
        public string coverAttaches { set; get; }
        public string price { set; get; }
        public string userId { set; get; }
        public string userName { set; get; }
        public string telephone { set; get; }
        public string addressId { set; get; }
        public string installDate { set; get; }
        public float installSize { set; get; }
        public int installNum { set; get; }
        public float payMoney { set; get; }
        public int state { set; get; } // 0: 待支付   1: 已支付   2: 已完成   -1: 已取消
        public string create_time { set; get; }
    }
}