# TianCheng.BaseService

目标架构为：.NET Standard 2.0

业务处理通用的方法。
包括：Jwt的登录控制、其它接口的调用封装、文件上传处理、Swagger、通用业务的封装

## 如何在WebApi中使用

1. 在`Program`的`BuildWebHost`方法中增加一行：`.ConfigureAppConfiguration(TianCheng.Model.ServiceLoader.Appsettings)`。

  修改后的样子 Program.cs

  ```csharp
      public class Program
      {
          public static void Main(string[] args)
          {
              BuildWebHost(args).Run();
          }

          public static IWebHost BuildWebHost(string[] args) =>
              WebHost.CreateDefaultBuilder(args)
                  .UseStartup<Startup>()
                  .ConfigureAppConfiguration(TianCheng.Model.ServiceLoader.Appsettings)  // 此处新增一行，用于按环境变量组合appsettings.json文件
                  .Build();
      }
  ```

2. 在`Startup`的`ConfigureServices`中增加一行：`services.TianChengBaseServicesInit(Configuration);`。
3. 在`Startup`的`Configure`中增加一行：`app.TianChengBaseServicesInit(Configuration);`。

  修改后的样子 Startup.cs

  ```csharp
      public class Startup
      {
          public Startup(IConfiguration configuration)
          {
              Configuration = configuration;
          }

          public IConfiguration Configuration { get; }

          public void ConfigureServices(IServiceCollection services)
          {
              services.TianChengBaseServicesInit(Configuration);
          }

          public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
          {
              app.TianChengBaseServicesInit(Configuration);
          }
      }
  ```

4. 修改appsettings.json配置文件。这里主要设置了Swagger的显示信息以及Token生成时的一些配置。

  修改后的appsettings.json配置文件

  ```json
  {
    "AllowedHosts": "*",
    "SwaggerDoc": {
      "Version": "v1",
      "Title": "演示系统接口文档",
      "Description": "RESTful API for 演示",
      "TermsOfService": "https://github.com/chengkkll/TianCheng.BaseService",
      "ContactName": "cheng_kkll",
      "ContactEmail": "cheng_kkll@163.com",
      "ContactUrl": "",
      "EndpointUrl": "/swagger/v1/swagger.json",
      "EndpointDesc": "演示系统 API V1"
    },
    "Token": {
      "Issuer": "TianCheng.WebApi.Demo",
      "Audience": "TianCheng",
      "SecretKey": "TianChengSecretKey",
      "Scheme": "Bearer"
    }
  }
  ```

5. 修改appsettings.Development.json配置文件。这里主要是设置日志的记录方式和数据库的访问方式，一般开发模式和生产模式的日志记录与数据库连接方式不同。所以可以分开配置。

  修改后的appsettings.Development.json配置文件
  
  ```json
  {
  "Serilog": {
    "WriteTo": [
      {
        "Name": "RollingFile",
        "Args": {
          "pathFormat": "Logs/common-{Date}.txt",
          "restrictedToMinimumLevel": "Information"
        }
      },
      "Debug",
      {
        "Name": "Console",
        "Args": {
          "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}",
          "restrictedToMinimumLevel": "Information"
        }
      },
      {
        "Name": "MongoDB",
        "Args": {
          "databaseUrl": "mongodb://localhost:27017/samples",
          "collectionName": "system_log",
          "restrictedToMinimumLevel": "Information"
        }
      }
    ]
  },
  "DBConnection": [
    {
      "Name": "default",
      "ServerAddress": "mongodb://localhost:27017",
      "Database": "samples",
      "Type": "MongoDB"
    }
  ]
}
  ```

6. 完成，运行查看。

修改后直接运行，由于自动配置了swagger所以，可以在网站地址及端口号后直接填写swagger地址进行接口查看，地址类似：`http://localhost:800/swagger`,端口号根据你的项目设置填写。

## 内部工具的使用方式

### 登录处理

实现`IAuthService`接口，并在生成`Claim`列表时增设置实现对象的名称。

示例：

  ```csharp
   public class AuthService : IAuthService
   {
     ...
     public string Login(string account, string password)
     {
       ...
       List<Claim> claims = new List<Claim>
       {
            new Claim("id",employee.Id.ToString()),
            new Claim("name", employee.Name ?? ""),
            new Claim("type","AuthService")
       };
      ...
     }
     ...
   }
  ```

  完整的实现代码可以参考[TianCheng.SystemCommon](https://github.com/chengkkll/TianCheng.SystemCommon)的登陆处理。

### 调用其它Api的方法

经常会有调用第三方或者项目外的接口。为了方便调用，封装了一下API方法的调用。调用类名称为`LoadApi`,根据http的请求方式不同分为：Get、Post、Put、Pathc、Delete、Options。如果接口需要登陆验证可传递token参数，如不需要忽略。调用方法时，指定类型后，会将返回的数据返回指定的数据类型，否则返回字符串。

调用示例：

  ```csharp

  // 通过高德API获取城市信息
  AmapDistricts amap = LoadApi.Get<AmapDistricts>(AmapDistrictApiUrl);

  ```

### 文件上传

调用`UploadFileHandle`的上传方法即可。

调用示例：

  ```csharp
    /// <summary>
    ///  上传文件测试
    /// </summary>
    /// <param name="file"></param>
    [SwaggerOperation(Tags = new[] { "系统管理-测试接口" })]
    [HttpPost("UploadFile")]
    public UploadFileInfo Upfile(IFormFile file)
    {
        string webFile = $"~/wwwroot/UploadFiles/{Guid.NewGuid().ToString()}";
        return UploadFileHandle.UploadImage(webFile, file);
    }
  ```

### 文件下载

调用`DownloadFileHandle`的下载方法即可。

### 通用业务方法的使用

通用的业务方法都是基类，通过继承的方式来实现相关的通用共功能。封装通用的功能，使得在Service中更关注业务逻辑，只需要在Servic中书写特有的业务逻辑即可， 方便代码的阅读和管理。

#### MongoBusinessService&lt;T&gt; 说明

##### 定义说明

 T 表示操作的实体对象；ObjectId 类型为MongoDB的ID类型

以下方法只要继承MongoBusinessService就有拥有，包含方法说明：

|方法定义   |  说明 |
|:-----------------------|:-------------|
| List&lt;T&gt; _SearchByIds(IEnumerable&lt;string&gt; ids)  |  根据ID列表获取对象集合 |
| virtual T _SearchById(string id)  | 根据id查询对象信息  如果无查询结果会抛出异常 |
| T _SearchById(ObjectId id)  | 根据id查询对象信息  如果无查询结果会抛出异常 |
| IQueryable&lt;T&gt; SearchQueryable()  | 获取查询条件的链式表达式 |
| int Count()  | 获取所有记录的总数 |
| virtual ResultView Create(T info, TokenLogonInfo logonInfo)  | 创建一个对象信息 |
| virtual ResultView Update(T info, TokenLogonInfo logonInfo)  | 更新一个对象信息 |
| ResultView Delete(string id, TokenLogonInfo logonInfo)  | 逻辑删除 |
| ResultView DeleteById(ObjectId id, TokenLogonInfo logonInfo)  | 逻辑删除 |
| ResultView Delete(T info, TokenLogonInfo logonInfo)  | 逻辑删除 |
| ResultView UnDelete(string id, TokenLogonInfo logonInfo)　| 取消逻辑删除|
| virtual ResultView UnDeleteById(ObjectId id, TokenLogonInfo logonInfo)　| 取消逻辑删除|
| virtual ResultView UnDelete(T info, TokenLogonInfo logonInfo)　|取消逻辑删除 |
| ResultView Remove(string id, TokenLogonInfo logonInfo)　|　物理删除　|
| ResultView RemoveById(ObjectId id, TokenLogonInfo logonInfo)　|　物理删除　|
| ResultView Remove(T info, TokenLogonInfo logonInfo)　|　物理删除　|

根据业务逻辑，派生类可重写的方法

|方法定义   |  说明 |
|:-----------------------|:-------------|
| protected virtual void Creating(T info, TokenLogonInfo logonInfo) |  更新的前置操作 |
| protected virtual void Created(T info, TokenLogonInfo logonInfo) | 更新的后置操作 |
| protected virtual void Updating(T info, T old, TokenLogonInfo logonInfo)| 更新的前置操作 |
| protected virtual void Updated(T info, T old, TokenLogonInfo logonInfo)| 更新的后置操作|
| protected virtual void Saving(T info, TokenLogonInfo logonInfo)| 保存的前置操作 |
| protected virtual void Saved(T info, TokenLogonInfo logonInfo)| 保存的后置操作|
| protected virtual void SavingCheck(T info, TokenLogonInfo logonInfo)| 保存前的数据校验|
| protected virtual bool IsExecuteCreate(T info, TokenLogonInfo logonInfo)| 是否可以执行新增操作|
| protected virtual bool IsExecuteUpdate(T info, T old, TokenLogonInfo logonInfo)| 是否可以执行保存操作 |

根据业务逻辑，其它类可通过事件的形式，扩展对应事件的后续处理。这些事件如下

|方法定义   |  说明 |
|:-----------------------|:-------------|
| public Action&lt;T, TokenLogonInfo&gt; OnCreatingCheck| 新增前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存 |
| public Action&lt;T, TokenLogonInfo&gt; OnCreating| 新增前置事件|
| public Action&lt;T, TokenLogonInfo&gt; OnCreated| 新增后置事件 |
| public Action&lt;T, T, TokenLogonInfo&gt; OnUpdateCheck| 更新前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存|
| public Action&lt;T, T, TokenLogonInfo&gt; OnUpdating| 更新前置事件 |
| public Action&lt;T, T, TokenLogonInfo&gt; OnUpdated| 更新后置事件  |
| public Action&lt;T, TokenLogonInfo&gt; OnSaveCheck| 保存前的数据验证事件 如果验证失败通过抛出异常形式终止数据保存|
| public Action&lt;T, TokenLogonInfo&gt; OnSaving| 保存前置事件 |
| public Action&lt;T, TokenLogonInfo&gt; OnSaved| 保存后置事件 |
| public Action&lt;T, TokenLogonInfo&gt; OnDeleting | 逻辑删除的前置事件处理 |
| public Action&lt;T, TokenLogonInfo&gt; OnDeleted| 逻辑删除后的事件处理 |
| Action&lt;T, TokenLogonInfo&gt; OnUnDeleting| 取消逻辑删除的前置事件处理|
| Action&lt;T, TokenLogonInfo&gt; OnUnDeleted| 取消逻辑删除后的事件处理|
| Action&lt;T, TokenLogonInfo&gt; OnRemoving| 物理删除的前置事件处理|
| Action&lt;T, TokenLogonInfo&gt; OnRemoved| 物理删除后的事件处理|
| Action&lt;T, TokenLogonInfo&gt; OnDeleteCheck| 逻辑删除的 删除前检测|
| Action&lt;T, TokenLogonInfo&gt; OnRemoveCheck| 物理删除的 删除前检测|
| Action&lt;T, TokenLogonInfo&gt; OnDeleteRemoveCheck| 逻辑与物理删除的 删除前检测|

使用示例

  ```csharp
    // 在部门名称更新后，想修改对应员工信息中的名称
    DepartmentService.OnUpdated += EmployeeService.OnDepartmentUpdated;
    // 部门下有员工信息，不允许删除。
    DepartmentService.OnRemoveCheck += EmployeeService.OnRemoveCheck;
  ```

然后在EmployeeService的对应方法中根据业务规则书写代码。
这种扩展处理，可以在不同的类库中，不限次数的扩展，方便模块间的组合且不用修改原模块的代码。

##### 业务流程说明

  业务流程一般分为3大块，数据校验、前置处理、后置处理。
  
  数据校验，通常用来处理数据及请求是否符合要求，是否可以执行具体的业务。

  前置处理，通常是收集整理业务需要的数据。

  后置处理，通常是业务操作成功后的一些后续扩展处理。

新增业务的调用流程

|调用顺序   |  名称 | 类型 |
|:-----------------------|:-------------|:-------------|
|  1 | SavingCheck     | 可派生类中重写 |
|  2 | IsExecuteCreate | 可派生类中重写 |
|  3 | OnCreateCheck | 可扩展事件 |
|  4 | OnSaveCheck     | 可扩展事件 |
|  5 | Creating        | 可派生类中重写|
|  6 | Saving          | 可派生类中重写|
|  7 | OnCreating      | 可扩展事件|
|  8 | OnSaving        | 可扩展事件|
|  9 | 此处做数据库处理 | 不可改变|
| 10 | Created         | 可派生类中重写|
| 11 | Saved           | 可派生类中重写|
| 12 | OnCreated       | 可扩展事件|
| 13 | OnSaved         | 可扩展事件|

更新业务的调用流程

|调用顺序   |  名称 | 类型 |
|:-----------------------|:-------------|:-------------|
|  1 | SavingCheck     | 可派生类中重写 |
|  2 | IsExecuteUpdate | 可派生类中重写 |
|  3 | OnUpdateCheck   | 可扩展事件 |
|  4 | OnSaveCheck     | 可扩展事件 |
|  5 | Updating        | 可派生类中重写 |
|  6 | Saving          | 可派生类中重写|
|  7 | OnUpdating      | 可扩展事件|
|  8 | OnSaving        | 可扩展事件|
|  9 | 此处做数据库处理 | 不可改变|
| 10 | Updated         | 可派生类中重写|
| 11 | Saved           | 可派生类中重写|
| 12 | OnUpdated       | 可扩展事件|
| 13 | OnSaved         | 可扩展事件|

逻辑删除业务的调用流程

|调用顺序   |  名称 | 类型 |
|:-----------------------|:-------------|:-------------|
|  1 | DeleteRemoveCheck   | 可派生类中重写 |
|  2 | OnDeleteRemoveCheck | 可扩展事件 |
|  3 | DeleteCheck         | 可派生类中重写 |
|  4 | OnDeleteCheck       | 可扩展事件 |
|  5 | DeleteRemoving      | 可派生类中重写 |
|  6 | Deleting            | 可派生类中重写|
|  7 | OnDeleting          | 可扩展事件|
|  8 | 此处做数据库处理      | 不可改变|
|  9 | Deleted             | 可派生类中重写|
| 10 | DeleteRemoved       | 可派生类中重写|
| 11 | OnDeleted           | 可扩展事件|

物理删除业务的调用流程

|调用顺序   |  名称 | 类型 |
|:-----------------------|:-------------|:-------------|
|  1 | DeleteRemoveCheck   | 可派生类中重写 |
|  2 | OnDeleteRemoveCheck | 可扩展事件 |
|  3 | RemoveCheck         | 可派生类中重写 |
|  4 | OnRemoveCheck       | 可扩展事件 |
|  5 | DeleteRemoving      | 可派生类中重写 |
|  6 | Removing            | 可派生类中重写|
|  7 | OnRemoving          | 可扩展事件|
|  8 | 此处做数据库处理      | 不可改变|
|  9 | Removed             | 可派生类中重写|
| 10 | DeleteRemoved       | 可派生类中重写|
| 11 | OnRemoved           | 可扩展事件|

取消逻辑删除业务的调用流程

|调用顺序   |  名称 | 类型 |
|:-----------------------|:-------------|:-------------|
|  1 | UnDeleting            | 可派生类中重写|
|  2 | OnUnDeleting          | 可扩展事件|
|  3 | 此处做数据库处理       | 不可改变|
|  4 | UnDeleted             | 可派生类中重写|
|  5 | OnUnDeleted           | 可扩展事件|

#### MongoBusinessService&lt;T, Q&gt; 说明

MongoBusinessService&lt;T, Q&gt; 中包含MongoBusinessService&lt;T&gt;的所有功能，针对其新功能做以下说明。

T 表示操作的实体对象; Q 表示查询条件，需继承于`QueryInfo`

|方法定义   |  说明 |
|:-----------------------|:-------------|
| PagedResult&lt;T&gt; _FilterPage(Q input)  |  分页查询 获取实体对象 |
| List&lt;T&gt; _FilterStep(Q input)  |  分步查询 获取实体对象，用于页面下拉获取新数据的场景 |
| List&lt;SelectView&gt; Select(Q input)  | 获取一个显示于下拉列表中的对象集合 |
| List&lt;SelectView&gt; SelectTop(Q input)  | 获取满足条件的前几条数据    input.Pagination.PageMaxRecords为指定的返回记录的最大条数 |
| int Count(Q input)  | 获取满足条件的记录总数 |

根据业务逻辑，派生类可重写的方法
|方法定义   |  说明 |
|:-----------------------|:-------------|
| public virtual IQueryable&lt;T&gt; _Filter(Q input) |  按条件查询数据 |

_Filter 方法算是核心的查询方法，上述的所有方法都会调用本方法做查询。也就是说，只需要重写_Filter方法，就会将规则应用于整个对象的操作。

重写 _Filter 时中主要写查询条件与排序的处理。以下为示例

  ```csharp
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override IQueryable<DepartmentInfo> _Filter(DepartmentQuery filter)
        {
            var query = _Dal.Queryable();

            #region 查询条件
            //不显示删除的数据
            query = query.Where(e => e.IsDelete == false);

            // 按名称的模糊查询
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(e => !string.IsNullOrEmpty(e.Name) && e.Name.Contains(filter.Name));
            }
            // 按上级部门ID查询
            if (!string.IsNullOrWhiteSpace(filter.ParentId))
            {
                query = query.Where(e => e.ParentsIds.Contains(filter.ParentId));
            }
            #endregion

            #region 设置排序规则
            //设置排序方式
            switch (filter.Sort.Property)
            {
                case "name": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name); break; }
                case "code": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.Code) : query.OrderByDescending(e => e.Code); break; }
                case "parent": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.ParentName) : query.OrderByDescending(e => e.ParentName); break; }
                case "date": { query = filter.Sort.IsAsc ? query.OrderBy(e => e.UpdateDate) : query.OrderByDescending(e => e.UpdateDate); break; }
                default: { query = query.OrderByDescending(e => e.UpdateDate); break; }
            }
            #endregion

            //返回查询结果
            return query;
        }
  ```

#### MongoBusinessService&lt;T, V, Q&gt; 说明

MongoBusinessService&lt;T, V, Q&gt; 中包含MongoBusinessService&lt;T, Q&gt;的所有功能，针对其新功能做以下说明。

1. T 表示操作的实体对象
2. V 表示最终呈现给终端的数据结构，通常用于接口的输出和输入，
3. Q 表示查询条件，需继承于`QueryInfo`

新增的V是用于呈现给终端的数据结构，所以，新增的功能也都是针对V来操作的。

|方法定义   |  说明 |
|:-----------------------|:-------------|
| virtual ResultView Create(V view, TokenLogonInfo logonInfo)  | 创建一个对象信息 |
| virtual ResultView Update(V view, TokenLogonInfo logonInfo)  | 更新一个对象信息 |
| virtual V SearchById(string id)  | 根据id查询对象信息 |
| List&lt;V&gt; Filter(Q input)       | 无分页查询 获取默认的View对象 |
| List&lt;OV&gt; Filter&lt;OV&gt;(Q input)  | 无分页查询 获取指定的View对象 |
| List&lt;V&gt; FilterStep(Q input)              | 分步查询数据，返回默认的View对象 |
| List&lt;VO&gt; FilterStep&lt;VO&gt;(Q input)         | 分步查询数据，返回指定的View对象 |
| PagedResult&lt;V&gt; FilterPage(Q input)       | 分页查询 返回默认的View对象 |
| PagedResult&lt;OV&gt; FilterPage&lt;OV&gt;(Q input)  | 分页查询 转换成指定的View对象 |

### 其它

关于继承业务更多实现代码可以参考[TianCheng.SystemCommon](https://github.com/chengkkll/TianCheng.SystemCommon)的处理。
