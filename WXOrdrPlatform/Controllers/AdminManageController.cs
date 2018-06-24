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
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Top(string section)
        {
            string lihtml = "";
            Dictionary<string, string> dicHtml = new Dictionary<string, string>();
            dicHtml.Add("0", "<li {0}><a  href=\"/Home/Index\"><i class=\"icon-home\"></i>首页概况</a></li>");

            string strTemp = string.Empty;
            foreach (string key in dicHtml.Keys)
            {
                strTemp = string.Empty;
                if (key == section)
                {
                    strTemp = string.Format(dicHtml[key], "class=\"active\"");
                }
                else
                {
                    strTemp = string.Format(dicHtml[key], "");
                }
                lihtml += strTemp;
            }
            ViewData["lihtml"] = lihtml;

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(FormCollection form)
        {
            string member_name = Request["username"];
            string member_passwd = Request["pwd"];

            //验证用户是否存在
            if (member_name == "admin" && member_passwd == "123456")
            {
                Session.Add("admin_id", member_name);
                if (TempData["oldurl"] == null)
                {
                    return RedirectToAction("UserList", "User");
                }
                else
                {
                    string strRediretUrl = TempData["oldurl"].ToString();
                    return Redirect(strRediretUrl);
                }
            }
            else
            {
                ViewData["msg"] = "登录失败,用户名或密码错误";
                return View(ViewData);
            }

        }

        public ActionResult LoginOut()
        {
            //清除session
            Session.Clear();
            return RedirectToAction("Login", "AdminManage");
        }     

        public ActionResult Aside(string menu_id)
        {
            ViewData["menu_id"] = menu_id;
            return View(ViewData);
        }

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

        //多附件上传
        public JsonResult MultiUpLoadImage(List<file> fs)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;      

            if (fs.Count == 0)
            {
                res.Data = new
                {
                    success = false
                };
                return res;
            }

            try
            {
                List<file> list = new List<file>();
                for (int i = 0; i < fs.Count; i++)
                {
                    string fileName = fs[i].fileName;
                    string fileContent = fs[i].fileContent;
                    string fileSize = fs[i].fileSize;
                    string filePath = string.Empty;
                    byte[] arr = Convert.FromBase64String(fileContent);
                    MemoryStream ms = new MemoryStream(arr);
                    Bitmap bmp = new Bitmap(ms);
                    string id = Guid.NewGuid().ToString();
                    filePath = "/UpLoadFile/" + id + ".png";
                    string path = Server.MapPath(filePath);
                    bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Close();

                    file f = new file();
                    f.id = id;
                    f.fileName = fileName;
                    f.fileSize = fileSize;
                    f.filePath = filePath;

                    list.Add(f);
                }             

                res.Data = new
                {
                    success = true,
                    backData = list
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
