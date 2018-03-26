using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WXOrdrPlatform.Models
{
    public class address
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string receiver { get; set; }
        public string telephone { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string area { get; set; }
        public int isDefault { get; set; }
    }
}