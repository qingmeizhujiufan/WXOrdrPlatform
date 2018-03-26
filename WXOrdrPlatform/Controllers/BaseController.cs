using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WXOrdrPlatform.Controllers
{
    public class Base_Home : Controller
    {
        protected BLL.Common commonBll = null;
        protected string userOpenID = string.Empty;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            commonBll = new BLL.Common();
            this.userOpenID = commonBll.GetCurrentUserID();

            string actionName = filterContext.ActionDescriptor.ActionName.ToLower();

            string strAddHomeBaseActions = "GetOpenId,ScanWX";
            strAddHomeBaseActions = strAddHomeBaseActions.ToLower();
            string[] aryAction = strAddHomeBaseActions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (!aryAction.Contains(actionName))
            {
                //微信专用 获取openid
                if (CommonTool.Common.GetAppSetting("useBasePageGetOpenID") == "1")
                {
                    if (CommonTool.Common.GetAppSetting("isTest") == "1")
                    {
                        Session["openid"] = CommonTool.Common.GetAppSetting("openID");
                    }

                    if (Session["openid"] == null)
                    {
                        string askUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={2}?callBackUrl={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
                        string p0 = CommonTool.WXParam.APP_ID;
                        string p1 = Request.Url.ToString();
                        string p2 = CommonTool.Common.GetAppSetting("redirectUri");
                        askUrl = string.Format(askUrl, p0, p1, p2);
                        Response.Redirect(askUrl);
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class Base : Controller
    {
        protected BLL.Common commonBll = null;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            commonBll = new BLL.Common();

            //判断用户是否登陆
            string actionName = filterContext.ActionDescriptor.ActionName.ToLower();
            string strAddHomeBaseActions = "UserList,ProductList,ProductDetail,BrandAdmin,EditProduct,OrderList,OrderDetail,NewsList,AddNews,NewsDetailInfo,EditNews,CaseList,AddCase,CaseDetailInfo,EditCase";
            strAddHomeBaseActions = strAddHomeBaseActions.ToLower();
            string[] aryAction = strAddHomeBaseActions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (aryAction.Contains(actionName))
            {
                //判断门店是否登陆
                if (!commonBll.IsAdminLogin())
                {
                    string oldurl = Request.Url.ToString();
                    if (TempData.ContainsKey("oldurl"))
                    {
                        TempData["oldurl"] = oldurl;
                    }
                    else
                    {
                        TempData.Add("oldurl", oldurl);
                    }

                    filterContext.Result = RedirectToRoute(new { Controller = "AdminManage", Action = "Login" });
                    RedirectToAction("Login", "AdminManage");
                }
            }

            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}
