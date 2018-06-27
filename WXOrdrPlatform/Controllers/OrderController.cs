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
    public class OrderController : ApiController
    {
        #region 获取所有商品
        /// <summary>  
        /// 获取所有商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getOrderList()
        {
            object data;
            try
            {
                BLL.Order order = new BLL.Order();
                DataTable dt = order.GetOrderList();
                List<order> list = new List<order>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    order o = new order();
                    o.id = dt.Rows[i]["id"].ToString();
                    o.orderNo = dt.Rows[i]["orderNo"].ToString();
                    o.productId = dt.Rows[i]["productId"].ToString();
                    o.productName = dt.Rows[i]["productName"].ToString();
                    o.price = dt.Rows[i]["price"].ToString();
                    o.userName = dt.Rows[i]["userName"].ToString();
                    o.telephone = dt.Rows[i]["telephone"].ToString();
                    o.addressId = dt.Rows[i]["addressId"].ToString();
                    o.installDate = dt.Rows[i]["installDate"].ToString();
                    o.installSize = Convert.ToSingle(dt.Rows[i]["installSize"].ToString());
                    o.installNum = Convert.ToInt32(dt.Rows[i]["installNum"].ToString());
                    o.payMoney = Convert.ToSingle(dt.Rows[i]["payMoney"].ToString());
                    o.state = Convert.ToInt32(dt.Rows[i]["state"].ToString());
                    o.create_time = dt.Rows[i]["create_time"].ToString();

                    list.Add(o);
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

        #region 获取所有商品
        /// <summary>  
        /// 获取所有商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getOrderInfo(string orderId)
        {
            object data;
            BLL.Order order = new BLL.Order();
            DataTable dt = order.GetOrderBaseInfo(orderId);
            if (dt.Rows.Count == 1)
            {
                data = new
                {
                    success = true,
                    backData = new
                    {
                        id = dt.Rows[0]["id"].ToString(),
                        orderNo = dt.Rows[0]["orderNo"].ToString(),
                        productId = dt.Rows[0]["productId"].ToString(),
                        productName = dt.Rows[0]["productName"].ToString(),
                        coverAttaches = dt.Rows[0]["coverAttaches"].ToString(),
                        price = dt.Rows[0]["price"].ToString(),
                        userId = dt.Rows[0]["userId"].ToString(),
                        userName = dt.Rows[0]["userName"].ToString(),
                        telephone = dt.Rows[0]["telephone"].ToString(),
                        province = dt.Rows[0]["province"].ToString(),
                        city = dt.Rows[0]["city"].ToString(),
                        county = dt.Rows[0]["county"].ToString(),
                        area = dt.Rows[0]["area"].ToString(),
                        installDate = dt.Rows[0]["installDate"].ToString(),
                        installSize = dt.Rows[0]["installSize"].ToString(),
                        installNum = Convert.ToUInt32(dt.Rows[0]["installNum"]),
                        payMoney = dt.Rows[0]["payMoney"].ToString(),
                        state = dt.Rows[0]["state"].ToString(),
                        create_time = dt.Rows[0]["create_time"].ToString(),
                    }
                };
            }
            else
            {
                data = new
                {
                    success = false,
                    backMsg = "订单不存在"
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
        public HttpResponseMessage submitOrder(order or)
        {
            object data;
            string productId = or.productId;
            string userId = or.userId;
            string userName = or.userName;
            string telephone = or.telephone;
            string addressId = or.addressId;
            string installDate = or.installDate;
            float installSize = or.installSize;
            int installNum = or.installNum;
            float payMoney = or.payMoney;

            BLL.Order order = new BLL.Order();
            string id = order.SubmitOrder(productId, userId, userName, telephone, addressId, installDate, installSize, installNum, payMoney);
            if (id != "")
            {
                data = new
                {
                    success = true,
                    id = id
                };
            }
            else
            {
                data = new
                {
                    success = false,
                    backMsg = "订单生成失败"
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
        public HttpResponseMessage payOrder(string orderId)
        {
            object data;

            if (string.IsNullOrEmpty(orderId))
            {
                data = new
                {
                    success = false,
                    backMsg = "订单号不能为空"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(data);
                return new HttpResponseMessage
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }

            BLL.Order order = new BLL.Order();
            if (!order.checkOrderIsExist(orderId))
            {
                data = new
                {
                    success = false,
                    backMsg = "订单不存在"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(data);
                return new HttpResponseMessage
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }
            int flag = order.PayOrder(orderId);
            if (flag > 0)
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
                    backMsg = "订单回写失败，请于商家联系"
                };
            }

            JavaScriptSerializer _serializer = new JavaScriptSerializer();
            string _json = _serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(_json, System.Text.Encoding.UTF8, "application/json")
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
        public HttpResponseMessage completeOrder(string orderId)
        {
            object data;

            if (string.IsNullOrEmpty(orderId))
            {
                data = new
                {
                    success = false,
                    backMsg = "订单号不能为空"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(data);
                return new HttpResponseMessage
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }

            BLL.Order order = new BLL.Order();
            if (!order.checkOrderIsExist(orderId))
            {
                data = new
                {
                    success = false,
                    backMsg = "订单不存在"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(data);
                return new HttpResponseMessage
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }
            int flag = order.CompleteOrder(orderId);
            if (flag > 0)
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
                    backMsg = "订单完成失败，请重试"
                };
            }

            JavaScriptSerializer _serializer = new JavaScriptSerializer();
            string _json = _serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(_json, System.Text.Encoding.UTF8, "application/json")
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
        public HttpResponseMessage delOrder(string orderId)
        {
            object data;

            if (string.IsNullOrEmpty(orderId))
            {
                data = new
                {
                    success = false,
                    backMsg = "订单号不能为空"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(data);
                return new HttpResponseMessage
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }

            BLL.Order order = new BLL.Order();
            if (!order.checkOrderIsExist(orderId))
            {
                data = new
                {
                    success = false,
                    backMsg = "订单不存在"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(data);
                return new HttpResponseMessage
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }
            int flag = order.DelOrder(orderId);
            if (flag == 40003)
            {
                data = new
                {
                    success = false,
                    backMsg = "当前状态为非未支付状态，不可取消订单"
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(data);
                return new HttpResponseMessage
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }
            if (flag > 0)
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
                    backMsg = "取消订单失败，请重试"
                };
            }

            JavaScriptSerializer _serializer = new JavaScriptSerializer();
            string _json = _serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(_json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

    }
}