using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXOrdrPlatform.Models;

namespace WXOrdrPlatform.Controllers
{
    public class OrderController : Base
    {
        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderList()
        {
            return View();
        }

        public ActionResult OrderDetail(string id)
        {
            ViewData["orderId"] = id;

            return View();
        }

        #region 接口
        public JsonResult getOrderList(string orderNo)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                BLL.Order order = new BLL.Order();
                DataTable dt = order.GetOrderList(orderNo);
                res.Data = new
                {
                    success = true,
                    backData = CommonTool.JsonHelper.DataTableToJSON(dt)

                };
            }
            catch (Exception ex)
            {
                res.Data = new
                {
                    success = false
                };

            }

            return res;
        }

        public JsonResult getOrderInfo(string orderId)
        {
            var res = new JsonResult();
            BLL.Order order = new BLL.Order();
            DataTable dt = order.GetOrderBaseInfo(orderId);
            if (dt.Rows.Count == 1)
            {
                res.Data = new
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
                res.Data = new
                {
                    success = false,
                    backMsg = "订单不存在"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        public JsonResult submitOrder(order or)
        {
            var res = new JsonResult();
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
                res.Data = new
                {
                    success = true,
                    id = id
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单生成失败"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        public JsonResult payOrder(string orderId)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            if (string.IsNullOrEmpty(orderId))
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单号不能为空"
                };

                return res;
            }

            BLL.Order order = new BLL.Order();
            if (!order.checkOrderIsExist(orderId))
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单不存在"
                };

                return res;
            }
            int flag = order.PayOrder(orderId);
            if (flag > 0)
            {
                res.Data = new
                {
                    success = true
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单回写失败，请于商家联系"
                };
            }       

            return res;
        }

        public JsonResult completeOrder(string orderId)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            if (string.IsNullOrEmpty(orderId))
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单号不能为空"
                };

                return res;
            }

            BLL.Order order = new BLL.Order();
            if (!order.checkOrderIsExist(orderId))
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单不存在"
                };

                return res;
            }
            int flag = order.CompleteOrder(orderId);
            if (flag > 0)
            {
                res.Data = new
                {
                    success = true
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单完成失败，请重试"
                };
            }

            return res;
        }

        public JsonResult delOrder(string orderId)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            if (string.IsNullOrEmpty(orderId))
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单号不能为空"
                };

                return res;
            }

            BLL.Order order = new BLL.Order();
            if (!order.checkOrderIsExist(orderId))
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单不存在"
                };

                return res;
            }
            int flag = order.DelOrder(orderId);
            if (flag == 40003)
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "当前状态为非未支付状态，不可取消订单"
                };

                return res;
            }
            if (flag > 0)
            {
                res.Data = new
                {
                    success = true
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "取消订单失败，请重试"
                };
            }

            return res;
        }

        #endregion

    }
}
