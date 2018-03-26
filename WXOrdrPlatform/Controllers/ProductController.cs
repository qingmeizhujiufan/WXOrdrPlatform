using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXOrdrPlatform.Models;

namespace WXOrdrPlatform.Controllers
{
    public class ProductController : Base
    {
        //
        // GET: /Product/

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

        public ActionResult AddProduct()
        {
            return View();
        }

        public ActionResult EditProduct(string id)
        {
            ViewData["id"] = id;
            return View();
        }

        public ActionResult BrandAdmin()
        {
            return View();
        }

        #region 接口
        public JsonResult getProductList(string conditionValue)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                BLL.Product product = new BLL.Product();
                DataTable dt = product.GetProductList(conditionValue);
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

        public JsonResult getProductInfo(string id)
        {
            var res = new JsonResult();
            BLL.Product product = new BLL.Product();
            DataTable dt = product.GetProductBaseInfo(id);
            if (dt.Rows.Count == 1)
            {
                res.Data = new
                {
                    success = true,
                    backData = new
                    {
                        id = dt.Rows[0]["id"].ToString(),
                        name = dt.Rows[0]["name"].ToString(),
                        unit = dt.Rows[0]["unit"].ToString(),
                        price = dt.Rows[0]["price"].ToString(),
                        type = dt.Rows[0]["type"].ToString(),
                        detail = dt.Rows[0]["detail"].ToString(),
                        status = dt.Rows[0]["status"].ToString(),
                        attaches = dt.Rows[0]["attaches"].ToString(),
                        coverAttaches = dt.Rows[0]["coverAttaches"].ToString(),
                        structuralSection = dt.Rows[0]["structuralSection"].ToString(),
                        hardware = dt.Rows[0]["hardware"].ToString(),
                        sealant = dt.Rows[0]["sealant"].ToString()
                    }
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "产品不存在"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        public JsonResult saveProduct(product p)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            string id = p.id;
            string name = p.name;
            string unit = p.unit;
            double price = p.price;
            string type = p.type;
            string detail = p.detail;
            string attaches = p.attaches;
            string coverAttaches = p.coverAttaches;
            string structuralSection = p.structuralSection;
            string hardware = p.hardware;
            string sealant = p.sealant;
            try
            {
                BLL.Product product = new BLL.Product();
                bool flag = false;
                if (string.IsNullOrEmpty(id))
                {
                    flag = product.AddProduct(name, unit, price, type, detail, attaches, coverAttaches, structuralSection, hardware, sealant);
                }
                else
                {
                    flag = product.EditProduct(name, unit, price, type, detail, attaches, coverAttaches, structuralSection, hardware, sealant, id);
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
                        backMsg = string.IsNullOrEmpty(id) ? "新增产品失败" : "更新产品信息失败"

                    };
                }

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

        public JsonResult getBrandList(int type)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                BLL.Product product = new BLL.Product();
                DataTable dt = product.GetBrandList(type);
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

        public JsonResult saveBrand(brand b)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            string name = b.name;
            int type = b.type;
            try
            {
                BLL.Product product = new BLL.Product();
                bool flag = false;
                flag = product.AddBrand(type, name);

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
                        success = true,
                        backMsg = "新增品牌失败"

                    };
                }

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

        public JsonResult delBrand(string id)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try
            {
                BLL.Product product = new BLL.Product();
                bool flag = false;
                flag = product.DelBrand(id);

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
                        success = true,
                        backMsg = "删除品牌失败"

                    };
                }

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

        #endregion

    }
}
