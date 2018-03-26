using CommonTool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXOrdrPlatform.Models;

namespace WXOrdrPlatform.Controllers
{
    public class HomeController : Base_Home
    {
        #region 微信支付

        public ActionResult GetOpenId()
        {
            string strCode = Request.QueryString["code"];
            string callBackUrl = Request.QueryString["callBackUrl"];
            string strOpenID = CommonTool.WXOperate.GetOpenID(strCode);
            Session["openid"] = strOpenID;
            BLL.User user = new BLL.User();
            DataTable dt_user = user.GetUserBaseInfo(strOpenID);
            if (dt_user.Rows.Count <= 0)
            {
                string id = Guid.NewGuid().ToString();
                DBHelper.DataModal mode = new DBHelper.DataModal();
                mode.TableName = "dbo.wxop_user";
                mode.Type = DBHelper.ExecuteType.Insert;
                mode.ListFieldItem.Add(new DBHelper.FieldItem("id", id));
                mode.ListFieldItem.Add(new DBHelper.FieldItem("openid", strOpenID));

                mode.Execute();
            }

            return Redirect(callBackUrl);
        }

        public string WxPay(string SendData)
        {
            string strReturn = CommonTool.WXOperate.WxPayFor(SendData);
            return strReturn;
        }

        public string WxConfig(string SendData)
        {
            string strReturn = CommonTool.WXOperate.WxConfig(SendData);
            return strReturn;
        }
        #endregion

        #region 预定服务
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductList()
        {
            return View();
        }

        public ActionResult ProductDetail(string id)
        {
            ViewData["id"] = id;
            return View();
        }

        public ActionResult Reservation(string id)
        {
            ViewData["openid"] = Session["openid"].ToString();
            ViewData["productId"] = id;
            BLL.User user = new BLL.User();
            DataTable dt = user.GetUserBaseInfo(Session["openid"].ToString());
            ViewData["userId"] = dt.Rows[0]["id"].ToString();

            return View();
        }

        public ActionResult PayFor(string id)
        {
            ViewData["orderId"] = id;
            ViewData["openid"] = Session["openid"];
            return View();
        }

        public ActionResult Room3D()
        {
            return View();
        }
        #endregion

        #region 案例展示
        public ActionResult CaseShow(string id)
        {
            ViewData["id"] = id;

            return View();
        }

        public ActionResult CaseDetail(string id)
        {
            ViewData["caseId"] = id;

            return View();
        }

        public ActionResult NewsDetail(string id)
        {
            ViewData["newsId"] = id;

            return View();
        }

        #endregion

        #region 个人中心
        public ActionResult We()
        {
            string openid = CommonTool.WXParam.OpenID;
            CommonTool.WriteLog.Write("We page openid === " + openid);
            WxUser user = CommonTool.WXOperate.GetWxUser(openid);
            ViewData["user_photo_url"] = user.headimgurl;
            ViewData["user_nickname"] = user.nickname;
            ViewData["province"] = user.province;
            ViewData["city"] = user.city;

            BLL.User _user = new BLL.User();
            DataTable dt = _user.GetUserBaseInfo(Session["openid"].ToString());
            ViewData["userId"] = dt.Rows[0]["id"].ToString();

            return View();
        }

        public ActionResult PersonalInfo()
        {           
            ViewData["openid"] = Session["openid"].ToString();

            return View();
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult MyAddress()
        {
            ViewData["openid"] = Session["openid"].ToString();
            BLL.User user = new BLL.User();
            DataTable dt = user.GetUserBaseInfo(Session["openid"].ToString());
            ViewData["userid"] = dt.Rows[0]["id"].ToString();

            return View();
        }

        public ActionResult AddAddress(string id)
        {
            ViewData["edit"] = "0";
            if (!string.IsNullOrEmpty(id))
            {
                ViewData["edit"] = "1";
                ViewData["address_id"] = id;
            }
            ViewData["openid"] = Session["openid"].ToString();
            BLL.User user = new BLL.User();
            DataTable dt = user.GetUserBaseInfo(Session["openid"].ToString());
            ViewData["userid"] = dt.Rows[0]["id"].ToString();

            return View();
        }

        public ActionResult ReservationOrder(string id)
        {
            BLL.User _user = new BLL.User();
            DataTable dt = _user.GetUserBaseInfo(Session["openid"].ToString());
            ViewData["userId"] = dt.Rows[0]["id"].ToString();
            ViewData["id"] = id;

            return View();
        }

        public ActionResult OrderDetail(string id)
        {
            ViewData["id"] = id;

            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        #endregion

        #region 接口
        //获取用户所有地址
        public JsonResult getUserAddress(string openid)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            DataTable dt = user.GetUserAddress(openid);
            List<address> addressList = new List<address>();
            if (dt.Rows.Count >= 0)
            {         
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    address ares = new address();
                    ares.id = dt.Rows[i]["id"].ToString();
                    ares.userId = dt.Rows[i]["userId"].ToString();
                    ares.receiver = dt.Rows[i]["receiver"].ToString();
                    ares.telephone = dt.Rows[i]["telephone"].ToString();
                    ares.province = dt.Rows[i]["province"].ToString();
                    ares.city = dt.Rows[i]["city"].ToString();
                    ares.county = dt.Rows[i]["county"].ToString();
                    ares.area = dt.Rows[i]["area"].ToString();
                    ares.isDefault = Convert.ToInt32(dt.Rows[i]["isDefault"]);
                    addressList.Add(ares);
                }
                    res.Data = new
                    {
                        success = true,
                        backData = addressList
                    };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "地址查询失败"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //选择默认地址
        public JsonResult ChooseDefault(string user_id, string address_id)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            bool flag = user.ChooseDefault(user_id, address_id);
            if (flag)
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
                    backMsg = "修改失败"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //保存新的地址
        public JsonResult SaveAddress(address ares)
        {
            var res = new JsonResult();
            string userId = ares.userId;
            string receiver = ares.receiver;
            string telephone = ares.telephone;
            string province = ares.province;
            string city = ares.city;
            string county = ares.county;
            string area = ares.area;
            BLL.User user = new BLL.User();
            bool flag;
            if (string.IsNullOrEmpty(ares.id))
            {
                flag = user.addNewAddress(userId, receiver, telephone, province, city, county, area);
            }
            else
            {
                flag = user.modifyAddress(ares.id, userId, receiver, telephone, province, city, county, area);
            }
            
            if (flag)
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
                    backMsg = "新增失败"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //获取地址信息
        public JsonResult getAddressInfo(string id)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            try
            {
                DataTable dt = user.getAddressInfo(id);
                address addr = new address();
                if(dt.Rows.Count > 0){
                    addr.id = dt.Rows[0]["id"].ToString();
                    addr.receiver = dt.Rows[0]["receiver"].ToString();
                    addr.telephone = dt.Rows[0]["telephone"].ToString();
                    addr.province = dt.Rows[0]["province"].ToString();
                    addr.city = dt.Rows[0]["city"].ToString();
                    addr.county = dt.Rows[0]["county"].ToString();
                    addr.area = dt.Rows[0]["area"].ToString();
                    addr.isDefault = Convert.ToInt32(dt.Rows[0]["isDefault"]);
                }

                res.Data = new
                {
                    success = true,
                    backData = dt.Rows.Count > 0 ? addr : null
                };
            }
            catch(Exception e)
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "查询失败"
                };
            }
            
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //删除地址
        public JsonResult delAddress(string id)
        {
            var res = new JsonResult();

            BLL.User user = new BLL.User();
            bool flag = user.delAddress(id);
            if (flag)
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
                    backMsg = "删除失败"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //获取默认地址
        public JsonResult getDefaultAddress()
        {
            var res = new JsonResult();

            BLL.User user = new BLL.User();
            DataTable dt = user.getUserDefaultAddress(Session["openid"].ToString());
            if (dt.Rows.Count == 1)
            {
                res.Data = new
                {
                    success = true,
                    backData = new
                    {
                        id = dt.Rows[0]["id"],
                        userId = dt.Rows[0]["userId"],
                        receiver = dt.Rows[0]["receiver"],
                        telephone = dt.Rows[0]["telephone"],
                        province = dt.Rows[0]["province"],
                        city = dt.Rows[0]["city"],
                        county = dt.Rows[0]["county"],
                        area = dt.Rows[0]["area"]
                    }
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "请设置默认地址"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //获取用户所有订单
        public JsonResult getUserOrderList(string userId)
        {
            var res = new JsonResult();
            BLL.Order _order = new BLL.Order();
            DataTable dt = _order.GetUserOrderList(userId);
            List<order> orderList = new List<order>();
            if (dt.Rows.Count >= 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    order o = new order();
                    o.id = dt.Rows[i]["id"].ToString();
                    o.orderNo = dt.Rows[i]["orderNo"].ToString();
                    o.productId = dt.Rows[i]["productId"].ToString();
                    o.productName = dt.Rows[i]["productName"].ToString();
                    o.coverAttaches = dt.Rows[i]["coverAttaches"].ToString();
                    o.price = dt.Rows[i]["price"].ToString();
                    o.userId = dt.Rows[i]["userId"].ToString();
                    o.userName = dt.Rows[i]["userName"].ToString();
                    o.telephone = dt.Rows[i]["telephone"].ToString();
                    o.state = Convert.ToInt32(dt.Rows[i]["state"]);
                    orderList.Add(o);
                }
                res.Data = new
                {
                    success = true,
                    backData = orderList
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "订单查询失败"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        #endregion
    }
}
