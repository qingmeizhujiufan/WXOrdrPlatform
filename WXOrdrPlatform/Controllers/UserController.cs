using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WXOrdrPlatform.Controllers
{
    public class UserController : Base
    {
        //
        // GET: /User/

        public ActionResult UserList()
        {
            return View();
        }

        #region 接口
        public JsonResult getUserList()
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                BLL.User user = new BLL.User();
                DataTable dt = user.GetUserList();
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
        public JsonResult getUserInfo(string openid)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            DataTable dt = user.GetUserBaseInfo(openid);
            if (dt.Rows.Count == 1)
            {
                res.Data = new
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
                res.Data = new
                {
                    success = false,
                    backMsg = "用户不存在"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        public JsonResult updateUserInfo(string openID, string fieldName, string fieldValue)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            bool flag = user.UpdateUserInfo(openID, fieldName, fieldValue);
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
                    backMsg = "修改失败，请重试"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        #endregion

    }
}
