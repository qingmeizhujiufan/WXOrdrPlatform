using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WXOrdrPlatform.Models
{
    public class news
    {
        public string id { set; get; }
        public string news_type { set; get; }
        public string news_title { set; get; }
        public string news_cover { set; get; }
        public string news_author { set; get; }
        public string news_brief { set; get; }
        public string news_content { set; get; }
        public string create_time { set; get; }
    }
}