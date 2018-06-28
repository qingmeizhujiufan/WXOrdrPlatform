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
    public class ProductController : ApiController
    {
        #region 获取所有商品
        /// <summary>  
        /// 获取所有商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getProductList()
        {
            Object data;
            DataTable dt = new BLL.Product().GetProductList();
            if (dt.Rows.Count >= 0)
            {
                List<product> list = new List<product>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    product product = new product();
                    product.id = dt.Rows[i]["id"].ToString();
                    product.name = dt.Rows[i]["name"].ToString();
                    product.unit = dt.Rows[i]["unit"].ToString();
                    product.price = Convert.ToDouble(dt.Rows[i]["price"].ToString());
                    product.type = dt.Rows[i]["type"].ToString();
                    product.structuralSection = dt.Rows[i]["structuralSection"].ToString();
                    product.hardware = dt.Rows[i]["hardware"].ToString();
                    product.sealant = dt.Rows[i]["sealant"].ToString();
                    product.attaches = dt.Rows[i]["attaches"].ToString();
                    product.coverAttaches = dt.Rows[i]["coverAttaches"].ToString();
                    product.status = dt.Rows[i]["status"].ToString();
                    product.create_time = dt.Rows[i]["create_time"].ToString();

                    list.Add(product);
                }


                data = new
                {
                    success = true,
                    backData = list
                };
            }
            else
            {
                data = new
                {
                    success = false,
                    backMsg = "数据异常"
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

        #region 获取商品信息
        /// <summary>  
        /// 获取商品信息 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getProductInfo(string id)
        {
            DataTable dt = new BLL.Product().GetProductBaseInfo(id);
            Object data;
            if (dt.Rows.Count == 1)
            {
                product p = new product();
                p.id = dt.Rows[0]["id"].ToString();
                p.name = dt.Rows[0]["name"].ToString();
                p.unit = dt.Rows[0]["unit"].ToString();
                p.price = Convert.ToDouble(dt.Rows[0]["price"].ToString());
                p.type = dt.Rows[0]["type"].ToString();
                p.detail = dt.Rows[0]["detail"].ToString();
                p.status = dt.Rows[0]["status"].ToString();
                p.attaches = dt.Rows[0]["attaches"].ToString();
                p.coverAttaches = dt.Rows[0]["coverAttaches"].ToString();
                p.structuralSection = dt.Rows[0]["structuralSection"].ToString();
                p.hardware = dt.Rows[0]["hardware"].ToString();
                p.sealant = dt.Rows[0]["sealant"].ToString();

                data = new
                {
                    success = true,
                    backData = p
                };
            }
            else
            {
                data = new
                {
                    success = false,
                    backMsg = "产品不存在"
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
        public HttpResponseMessage saveProduct(dynamic p)
        {
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
            Object data;
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
                        backMsg = string.IsNullOrEmpty(id) ? "新增产品失败" : "更新产品信息失败"

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

        #region 删除商品
        /// <summary>  
        /// 删除商品 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delProduct(dynamic d)
        {
            string id = d.id;
            object data = new object();

            BLL.Product product = new BLL.Product();
            bool flag = false;
            flag = product.DelProduct(id);

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
                    backMsg = "删除商品失败"

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

        #region 获取所有品牌
        /// <summary>  
        /// 获取所有品牌 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getBrandList(int type)
        {
            Object data;
            try
            {
                BLL.Product product = new BLL.Product();
                DataTable dt = product.GetBrandList(type);
                List<brand> list = new List<brand>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    brand b = new brand();
                    b.id = dt.Rows[i]["id"].ToString();
                    b.name = dt.Rows[i]["name"].ToString();
                    b.type = Convert.ToInt32(dt.Rows[i]["type"].ToString());
                    list.Add(b);
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

        #region 保存品牌
        /// <summary>  
        /// 保存品牌 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage saveBrand(brand b)
        {
            Object data;
            string name = b.name;
            int type = b.type;
            try
            {
                BLL.Product product = new BLL.Product();
                bool flag = false;
                flag = product.AddBrand(type, name);

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
                        success = true,
                        backMsg = "新增品牌失败"

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

        #region 删除品牌
        /// <summary>  
        /// 删除品牌 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delBrand(dynamic d)
        {
            string id = d.id;
            object data = new object();

            BLL.Product product = new BLL.Product();
            bool flag = false;
            flag = product.DelBrand(id);

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
                    backMsg = "删除品牌失败"

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
