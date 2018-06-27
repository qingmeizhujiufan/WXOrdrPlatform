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
    public class UserController : ApiController
    {

        #region 获取所有用户
        /// <summary>  
        /// 获取所有用户 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getUserList()
        {
            object data;
            try
            {
                BLL.User user = new BLL.User();
                DataTable dt = user.GetUserList();
                List<user> list = new List<user>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    user u = new user();
                    u.id = dt.Rows[i]["id"].ToString();
                    u.name = dt.Rows[i]["name"].ToString();
                    u.sex = dt.Rows[i]["sex"].ToString();
                    u.birth = dt.Rows[i]["birth"].ToString();
                    u.telephone = dt.Rows[i]["telephone"].ToString();
                    u.village = dt.Rows[i]["village"].ToString();

                    list.Add(u);
                }

                data = new
                {
                    success = true,
                    backData = list

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

        #region 获取所有用户
        /// <summary>  
        /// 获取所有用户 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getUserInfo(string openid)
        {
            object data;
            BLL.User user = new BLL.User();
            DataTable dt = user.GetUserBaseInfo(openid);
            if (dt.Rows.Count == 1)
            {
                data = new
                {
                    success = true,
                    backData = new
                    {
                        id = dt.Rows[0]["id"].ToString(),
                        openid = dt.Rows[0]["openid"].ToString(),
                        name = dt.Rows[0]["name"].ToString(),
                        sex = dt.Rows[0]["sex"].ToString(),
                        birth = dt.Rows[0]["birth"].ToString(),
                        telephone = dt.Rows[0]["telephone"].ToString(),
                        village = dt.Rows[0]["village"].ToString()
                    }
                };
            }
            else
            {
                data = new
                {
                    success = false,
                    backMsg = "用户不存在"
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

        #region 获取所有用户
        /// <summary>  
        /// 获取所有用户 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage updateUserInfo(string openID, string fieldName, string fieldValue)
        {
            object data;
            BLL.User user = new BLL.User();
            bool flag = user.UpdateUserInfo(openID, fieldName, fieldValue);
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
                    backMsg = "修改失败，请重试"
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
