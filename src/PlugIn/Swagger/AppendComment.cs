using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 为Swagger文档中增加通用注释
    /// </summary>
    public class AppendComment : IDocumentFilter
    {
        /// <summary>  
        /// 为Swagger文档中增加通用注释
        /// </summary>  
        /// <param name="swaggerDoc">swagger文档文件</param>  
        /// <param name="context">api接口集合</param>  
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            //默认隐藏Options接口
            foreach (var path in swaggerDoc.Paths)
            {
                var control = path.Value;

                SetResponse(control);

                SetViewDesc(control);
                
            }
        }

        #region 异常说明的返回

        private IDictionary<string, Response> _CommonResponse = null;

        /// <summary>
        /// 通用的返回值信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, Response> CommonResponse
        {
            get
            {
                if (_CommonResponse == null)
                {
                    Response r400 = new Response()
                    {
                        Description = "请求参数错误。{ code:\"请求参数错误码\",message:\"请求参数错误提示文本\" }"
                    };
                    Response r401 = new Response()
                    {
                        Description = "请求参数错误，token值错误，需要重新登陆。{ code:\"401\",message:\"提交的token值\" }"
                    };
                    Response r403 = new Response()
                    {
                        Description = "请求参数错误，没有权限调用本接口。{ code:\"403\",message:\"no power\" }"
                    };
                    Response r404 = new Response()
                    {
                        Description = "找不到要操作的数据，找不到要操作的数据。{ code:\"404\",message:\"要操作的数据ID\" }"
                    };
                    IDictionary<string, Response> result = new Dictionary<string, Response>();
                    result.Add("400", r400);
                    result.Add("401", r401);
                    result.Add("403", r403);
                    result.Add("404", r404);

                    _CommonResponse = result;
                }
                return _CommonResponse;
            }
        }
        /// <summary>
        /// 为一个接口设置默认的返回信息
        /// </summary>
        /// <param name="oper"></param>
        private void SetResponse(Operation oper)
        {
            if (oper == null)
            {
                return;
            }
            //为返回值信息添加常见的异常返回信息
            if (oper.Responses == null)
            {
                oper.Responses = CommonResponse;
                return;
            }
            foreach (string key in CommonResponse.Keys)
            {
                if (!oper.Responses.ContainsKey(key))
                {
                    oper.Responses.Add(key, CommonResponse[key]);
                }
            }
            //完善调用成功后的说明
            SetResponse200(oper);
        }
        /// <summary>
        /// 完善调用成功后的说明
        /// </summary>
        /// <param name="oper"></param>
        private void SetResponse200(Operation oper)
        {
            if (!oper.Responses.ContainsKey("200"))
            {
                oper.Responses.Add("200", new Response());
            }

            var respOk = oper.Responses["200"];
            //设置返回值里的数据结构
            respOk.Description = UpdateViewInfo(respOk.Description);

            if (respOk.Schema != null && respOk.Schema.Ref == "#/definitions/ResultView")
            {
                //如果没有成功说明，添加默认的成功说明
                if (String.IsNullOrEmpty(respOk.Description) || respOk.Description == "Success")
                {
                    respOk.Description = "操作成功 <br/> 返回结果: <br/>\r\n     { code:\"0\",message:\"数据ID\" }\r\n     ";
                }
            }
        }

        private void SetResponse(PathItem control)
        {
            if (control == null)
            {
                return;
            }
            SetResponse(control.Get);
            SetResponse(control.Post);
            SetResponse(control.Put);
            SetResponse(control.Patch);
            SetResponse(control.Options);
            SetResponse(control.Delete);
        }
        #endregion

        #region 对象说明添加
        /// <summary>
        /// 对象说明字典
        /// </summary>
        private IDictionary<string, string> _ViewDescDict = null;
        /// <summary>
        /// 对象说明字典
        /// </summary>
        private IDictionary<string, string> ViewDescDict
        {
            get
            {
                if (_ViewDescDict == null)
                {
                    GetModelDesc();
                }
                return _ViewDescDict;
            }
        }

        private const string modelAssemblyName = "TianCheng.Model";
        /// <summary>
        /// 获取对象的描述信息
        /// </summary>
        private void GetModelDesc()
        {
            _ViewDescDict = new Dictionary<string, string>();
            return;

            //AssemblyName an = new AssemblyName(modelAssemblyName);
            //Assembly assembly = Assembly.Load(an);
            //if (_ViewDescDict == null)
            //{
            //    _ViewDescDict = new Dictionary<string, string>();

            //}
            //foreach (var type in assembly.GetTypes())
            //{
            //    StringBuilder sb = new StringBuilder(100);
            //    sb.Append(String.Format("\r\n     {0}             {1}             {2}                                       {3}", "属性", "数据类型", "说明", "备注"));

            //    foreach (var propery in type.GetProperties())
            //    {
            //        string aname = propery.Name;
            //        string atype = propery.PropertyType.Name.ToLower();
            //        if (atype.Contains("data") || atype.Contains("null"))
            //        {
            //            atype = "string";
            //        }
            //        if (atype.Contains("int"))
            //        {
            //            atype = "int";
            //        }
            //        var descAttr = propery.GetCustomAttribute<DescriptionAttribute>();
            //        string adesc = String.Empty;
            //        string remark = String.Empty;
            //        if (descAttr != null)
            //        {
            //            adesc = descAttr.Desc;
            //            remark = descAttr.Remark;
            //        }
            //        sb.Append(String.Format("\r\n     {0}             {1}             {2}                                       {3}", aname, atype, adesc, remark));
            //    }

            //    string typeName = type.Name;
            //    _ViewDescDict.Add(typeName, sb.ToString());
            //}

        }

        /// <summary>
        /// 设置对象说明
        /// </summary>
        /// <param name="oper"></param>
        private void SetViewDesc(Operation oper)
        {
            if (oper == null || oper.Description == null)
            {
                return;
            }

            oper.Description = UpdateViewInfo(oper.Description);
        }
        /// <summary>
        /// 设置对象结构的描述信息
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string UpdateViewInfo(string desc)
        {
            if (String.IsNullOrEmpty(desc))
            {
                return desc;
            }

            //判断是否有需要需要设置对象的
            string key = "[LoadViewObject:";
            string objKey = string.Empty;
            if (desc.Contains(key))
            {
                int start = desc.IndexOf(key) + key.Length;
                int end = desc.IndexOf("]", start);
                if (end <= start)
                {
                    return desc;
                }
                objKey = desc.Substring(start, end - start);
                Console.Write(objKey);
            }
            //判断指定的对象是否有注释
            if (!ViewDescDict.ContainsKey(objKey.Trim()))
            {
                return desc;
            }
            //设置对象的注释信息
            string appendDesc = "对象结构如下\r\n\r\n    " + ViewDescDict[objKey.Trim()] + "\r\n\r\n    ";

            //更新对象的注释信息
            string val = key + objKey + "]";
            desc = desc.Replace(val, appendDesc);
            return desc;
        }

        private void SetViewDesc(PathItem control)
        {
            if (control == null)
            {
                return;
            }
            SetViewDesc(control.Get);
            SetViewDesc(control.Post);
            SetViewDesc(control.Put);
            SetViewDesc(control.Patch);
            SetViewDesc(control.Options);
            SetViewDesc(control.Delete);
        }
        #endregion
    }
}
