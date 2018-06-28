using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WXOrdrPlatform.Models
{
    public class product
    {
        public string id { set; get; }
        public string name { set; get; }
        public string unit { set; get; }
        public double price { set; get; }
        public string type { set; get; }
        public string detail { set; get; }
        public string structuralSection { set; get; }
        public string hardware { set; get; }
        public string sealant { set; get; }
        public string status { set; get; }
        public string attaches { set; get; }
        public string coverAttaches { set; get; }
        public string create_time { set; get; }
    }
}