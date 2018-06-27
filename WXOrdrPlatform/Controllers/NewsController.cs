using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WXOrdrPlatform.Models;
using WXOrdrPlatform.Core;


namespace WXOrdrPlatform.Controllers
{
    public class NewsController : ApiController
    {
        #region 获取所有商品
        /// <summary>  
        /// 获取所有商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getNewsList()
        {
            object data;

            try
            {
                string strSql = @"select
                                    id, 
                                    news_type,
                                    news_title,
                                    news_cover,
                                    news_brief,
                                    CONVERT(varchar(19) , create_time, 120 ) as create_time
                                from dbo.wxop_news 
                                order by create_time desc";
                strSql = string.Format(strSql);
                DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);

                List<news> list = new List<news>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    news _news = new news();
                    _news.id = dt.Rows[i]["id"].ToString();
                    _news.news_type = dt.Rows[i]["news_type"].ToString();
                    _news.news_title = dt.Rows[i]["news_title"].ToString();
                    _news.news_cover = dt.Rows[i]["news_cover"].ToString();
                    _news.news_brief = dt.Rows[i]["news_brief"].ToString();
                    _news.create_time = dt.Rows[i]["create_time"].ToString();

                    list.Add(_news);
                }

                data = new
                {
                    success = true,
                    backData = new
                    {
                        content = list
                    }
                };
            }
            catch (Exception ex)
            {
                data = new
                {
                    success = false
                };

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 获取所有商品
        /// <summary>  
        /// 获取所有商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getNewsDetail(string newsId)
        {
            Object data;
            try
            {
                BLL.News news = new BLL.News();
                DataTable dt = news.GetNewsDetail(newsId);

                news _news = new news();
                _news.id = dt.Rows[0]["id"].ToString();
                _news.news_type = dt.Rows[0]["news_type"].ToString();
                _news.news_title = dt.Rows[0]["news_title"].ToString();
                _news.news_cover = dt.Rows[0]["news_cover"].ToString();
                _news.news_brief = dt.Rows[0]["news_brief"].ToString();
                _news.news_content = dt.Rows[0]["news_content"].ToString();
                _news.create_time = dt.Rows[0]["create_time"].ToString();

                data = new
                {
                    success = true,
                    backData = _news

                };
            }
            catch (Exception ex)
            {
                data = new
                {
                    success = false
                };

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 保存商品
        /// <summary>  
        /// 保存商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage saveAPNews(news n)
        {
            Object data;

            string id = n.id;
            string news_type = n.news_type;
            string news_cover = n.news_cover;
            string news_title = n.news_title;
            string news_brief = n.news_brief;
            string news_content = n.news_content;

            try
            {
                BLL.News news = new BLL.News();
                bool flag = false;
                if (string.IsNullOrEmpty(id))
                {
                    flag = news.AddNews(news_type, news_cover, news_title, news_brief, news_content);
                }
                else
                {
                    flag = news.EditNews(news_type, news_cover, news_title, news_brief, news_content, id);
                }

                if (flag)
                {
                    data = new
                    {
                        success = true

                    };
                }
                else
                {
                    data = new
                    {
                        success = false,
                        backMsg = string.IsNullOrEmpty(id) ? "新增新闻失败" : "更新新闻信息失败"

                    };
                }                            
            }
            catch (Exception ex)
            {
                data = new
                {
                    success = false
                };

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 保存商品
        /// <summary>  
        /// 保存商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delNews(string newsId)
        {
            Object data;

            BLL.News news = new BLL.News();
            bool flag = news.DelNews(newsId);
            if (flag)
            {
                data = new
                {
                    success = true
                };
            }
            else
            {
                data = new
                {
                    success = false,
                    backMsg = "删除失败，请重试！"
                };
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 获取所有商品
        /// <summary>  
        /// 获取所有商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getCaseList()
        {
            Object data;

            try
            {
                BLL.Case Case = new BLL.Case();
                DataTable dt = Case.GgetCaseList();

                List<@case> list = new List<@case>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    @case _case = new @case();
                    _case.id = dt.Rows[i]["id"].ToString();
                    _case.cover_img = dt.Rows[i]["cover_img"].ToString();
                    _case.title = dt.Rows[i]["title"].ToString();
                    _case.detail_img = dt.Rows[i]["detail_img"].ToString();
                    _case.create_time = dt.Rows[i]["create_time"].ToString();

                    list.Add(_case);
                }

                data = new
                {
                    success = true,
                    backData = new
                    {
                        content = list
                    }
                };
            }
            catch (Exception ex)
            {
                data = new
                {
                    success = false
                };

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 获取所有商品
        /// <summary>  
        /// 获取所有商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getCaseDetail(string caseId)
        {
            Object data;
            try
            {
                BLL.Case ca = new BLL.Case();
                DataTable dt = ca.GetCaseDetail(caseId);

                @case _case = new @case();
                _case.id = dt.Rows[0]["id"].ToString();
                _case.cover_img = dt.Rows[0]["cover_img"].ToString();
                _case.title = dt.Rows[0]["title"].ToString();
                _case.detail_img = dt.Rows[0]["detail_img"].ToString();
                _case.create_time = dt.Rows[0]["create_time"].ToString();

                data = new
                {
                    success = true,
                    backData = _case

                };
            }
            catch (Exception ex)
            {
                data = new
                {
                    success = false
                };

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 保存商品
        /// <summary>  
        /// 保存商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage saveAPCase(@case n)
        {
            Object data;

            string id = n.id;
            string cover_img = n.cover_img;
            string title = n.title;
            string detail_img = n.detail_img;

            try
            {
                BLL.Case @case = new BLL.Case();
                bool flag = false;
                if (string.IsNullOrEmpty(id))
                {
                    flag = @case.AddCase(cover_img, title, detail_img);
                }
                else
                {
                    flag = @case.EditCase(cover_img, title, detail_img, id);
                }

                if (flag)
                {
                    data = new
                    {
                        success = true

                    };
                }
                else
                {
                    data = new
                    {
                        success = false,
                        backMsg = string.IsNullOrEmpty(id) ? "新增案例失败" : "更新案例信息失败"

                    };
                }
            }
            catch (Exception ex)
            {
                data = new
                {
                    success = false
                };

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 保存商品
        /// <summary>  
        /// 保存商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delCase(string caseId)
        {
            Object data;

            BLL.Case @case = new BLL.Case();
            bool flag = @case.DelCase(caseId);
            if (flag)
            {
                data = new
                {
                    success = true
                };
            }
            else
            {
                data = new
                {
                    success = false,
                    backMsg = "删除失败，请重试！"
                };
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

    }
}
