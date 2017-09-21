using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TianCheng.BaseService.PlugIn.Swagger;

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
        /// <param name="configuration"></param>
        static public void SwaggerRegister(this IServiceCollection services, IConfigurationRoot configuration)
        {
            //拷贝引用程序集目录下的xml文件
            CopyXmlFile();
            //读取配置信息
            //添加options
            //services.AddOptions();
            //services.Configure<SwaggerDocInfo>(configuration.GetSection("SwaggerDoc"));
            //SwaggerDocInfo doc = configuration.GetValue<SwaggerDocInfo>("SwaggerDoc");
            //if(doc == null)
            //{
            //    doc = SwaggerDocInfo.Default;
            //}
            SwaggerDocInfo doc = null;
            try
            {
                var jsonServices = JObject.Parse(File.ReadAllText("appSettings.json"))["SwaggerDoc"];
                doc = JsonConvert.DeserializeObject<SwaggerDocInfo>(jsonServices.ToString());
                if (doc == null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                doc = SwaggerDocInfo.Default;
            }

            #region Swagger
            // Inject an implementation of ISwaggerProvider with defaulted settings applied.  
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<AuthorizationParameterFilter>();    //设置Token参数的显示
                options.OperationFilter<SwaggerFileUploadFilter>();         //设置文件上传的参数显示

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

                //查找运行目录的所有xml文件，并加载到当前页面
                //var basePath = System.IO.Path.Combine(
                //    PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath,
                //    doc.XmlPath);
                var basePath = PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath;
                List<string> fileList = new List<string>();


                foreach (var file in GetXmlFile(basePath))
                {
                    options.IncludeXmlComments(file);
                }

                //隐藏满足条件的接口
                options.DocumentFilter<HiddenApiFilter>();
                //增加默认的注释信息
                options.DocumentFilter<AppendComment>();
                //设置接口的排序
                //options.OrderActionGroupsBy(new SwaggerOrderBy());
                options.OrderActionsBy(SwaggerOrderBy.Order);
            });

            #endregion
        }
        /// <summary>
        /// 获取说明的xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static private List<string> GetXmlFile(string path)
        {
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
            if (!System.IO.Directory.Exists(xmlPath))
            {
                System.IO.Directory.CreateDirectory(xmlPath);
            }

            foreach (CompilationLibrary library in DependencyContext.Default.CompileLibraries)
            {
                // 除了系统的库文件其它的程序集全部做查询处理
                if(!library.Serviceable)
                {
                    Assembly assembly = Assembly.Load(new AssemblyName(library.Name));
                    if (assembly != null && !String.IsNullOrWhiteSpace(assembly.Location))
                    {
                        string path = System.IO.Path.GetDirectoryName(assembly.Location);
                        foreach (string file in System.IO.Directory.GetFiles(path, "*.xml"))
                        {
                            // 将xml文件拷贝到运行目录
                            string desc = $"{xmlPath}\\{System.IO.Path.GetFileName(file)}";
                            System.IO.File.Copy(file, desc, true);
                        }
                    }
                }
            }
        }
    }
}
