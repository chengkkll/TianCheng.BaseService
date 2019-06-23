using Microsoft.Extensions.DependencyModel;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TianCheng.BaseService.PlugIn.Swagger;
using TianCheng.Model;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    static public class SwaggerRegisterConfigureServicesExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        static public void SwaggerRegister(this IServiceCollection services)
        {
            //读取配置信息
            SwaggerDocInfo doc = SwaggerDocInfo.Load();

            #region Swagger
            // Inject an implementation of ISwaggerProvider with defaulted settings applied.  
            services.AddSwaggerGen(options =>
            {
                // 设置接口文档的基本信息
                options.SwaggerDoc(doc.Version, new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = doc.Version,
                    Title = doc.Title,
                    Description = doc.Description,
                    TermsOfService = doc.TermsOfService,
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = doc.ContactName,
                        Email = doc.ContactEmail,
                        Url = doc.ContactUrl
                    }
                });

                // 启用便签功能，实现接口按指定分类进行分组
                options.EnableAnnotations();

                // 遍历所有的注释文件，并添加到接口文档中
                CopyXmlFile();      // 拷贝引用程序集目录下的xml文件
                foreach (var file in GetXmlFile())
                {
                    options.IncludeXmlComments(file);
                }

                // 增加默认的注释信息
                options.DocumentFilter<AppendComment>();

                // 设置接口的排序
                options.OrderActionsBy(SwaggerOrderBy.Order);

                // 设置登录验证参数的效果
                options.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = $"Token验证。格式：Bearer token \r\n\t 注意：Bearer与token之间需要一个空格连接",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>(); // 官方登录验证的处理
                options.OperationFilter<AuthorizationParameterFilter>();        // 设置接口备注，显示权限名称
                //options.OperationFilter<SwaggerDecryptFilter>();                // 设置参数加密上传的效果

            });
            #endregion
        }

        #region 注释文件的查找和添加
        /// <summary>
        /// 获取说明的xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static private List<string> GetXmlFile(string path = "")
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                var host = ServiceLoader.GetService<AspNetCore.Hosting.IHostingEnvironment>();
                path = host.ContentRootPath;
            }
            List<string> fileList = new List<string>();
            foreach (var file in new System.IO.DirectoryInfo(path).GetFiles())
            {
                if (file.Extension.Equals(".xml"))
                {
                    fileList.Add(file.FullName);
                }
            }

            foreach (var dir in new System.IO.DirectoryInfo(path).GetDirectories())
            {
                fileList.AddRange(GetXmlFile(dir.FullName));
            }
            return fileList;
        }

        /// <summary>
        /// 拷贝引用程序集目录下的xml文件
        /// </summary>
        static private void CopyXmlFile()
        {
            string xmlPath = $"{AppContext.BaseDirectory}\\LibraryComments";
            if (!Directory.Exists(xmlPath))
            {
                Directory.CreateDirectory(xmlPath);
            }

            foreach (CompilationLibrary library in DependencyContext.Default.CompileLibraries)
            {
                // 除了系统的库文件其它的程序集全部做查询处理
                if (!library.Name.Contains("System.") && !library.Name.Contains("Microsoft.") &&
                    !library.Name.Equals("Libuv") && !library.Name.Contains("NETStandard.") &&
                    !library.Name.Contains("MongoDB.") && !library.Name.Contains("Serilog") && !library.Name.Contains("AutoMapper") &&
                    !library.Name.Contains("Swashbuckle") && !library.Name.Contains("Newtonsoft"))
                {
                    try
                    {
                        Assembly assembly = Assembly.Load(new AssemblyName(library.Name));
                        if (assembly != null && !String.IsNullOrWhiteSpace(assembly.Location))
                        {
                            string path = Path.GetDirectoryName(assembly.Location);
                            foreach (string file in Directory.GetFiles(path, "*.xml"))
                            {
                                // 将xml文件拷贝到运行目录
                                string desc = $"{xmlPath}\\{System.IO.Path.GetFileName(file)}";
                                File.Copy(file, desc, true);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        #endregion
    }
}
