using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXOrdrPlatform.Models;

namespace WXOrdrPlatform.Controllers
{
    public class AdminManageController : Controller
    {
        #region 接口

        //单附件上传
        public JsonResult UpLoadImage(HttpPostedFileBase file)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try
            {
                string fileName = file.FileName;
                string filePath = string.Empty;
                string id = Guid.NewGuid().ToString();
                filePath = "/UpLoadFile/" + id + ".png";
                string path = Server.MapPath(filePath);
                file.SaveAs(path);

                res.Data = new
                {
                    success = true,
                    data = new
                    {
                        id = id,
                        link = filePath
                    }
                };
                return res;
            }
            catch (Exception ex)
            {
                res.Data = new
                {
                    success = false
                };
                return res;
            }
        }

        #endregion

    }
}
