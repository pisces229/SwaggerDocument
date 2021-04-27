using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors.Security;
using SwaggerDocument.OperationProcessor;
using SwaggerDocument.OperationProcessor.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SwaggerDocument
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(webHostEnvironment.ContentRootPath);
            configurationBuilder.AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true);
            _configuration = configurationBuilder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => _configuration);
            //services.AddMvc().AddJsonOptions(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //});
            services.AddControllers().AddJsonOptions(options =>
            {
                //options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            // Add OpenAPI v3 document
            //services.AddOpenApiDocument();
            // add OpenAPI v3 document
            services.AddOpenApiDocument(config =>
            {
                // 設定文件名稱 (重要) (預設值: v1)
                config.DocumentName = "v1";

                // 設定文件或 API 版本資訊
                config.Version = "SwaggerDocument Version";

                // 設定文件標題 (當顯示 Swagger/ReDoc UI 的時候會顯示在畫面上)
                config.Title = "SwaggerDocument Title";

                // 設定文件簡要說明
                config.Description = "SwaggerDocument Description";

                // JWT 驗證授權的設定
                // 請務必將 config.AddSecurity() 與 new AspNetCoreOperationSecurityScopeProcessor() 都設定為一樣的安全名稱(JWT Token)
                // 所有專案內只要有套用[Authorize] 屬性(Attribute)的 API action 都會自動套用 JWT Token 的安全設定
                // 在線上發送 API 要求時，就會自動送出 Bearer Token
                //var openApiSecurityScheme = new OpenApiSecurityScheme()
                //{
                //    Type = OpenApiSecuritySchemeType.ApiKey,
                //    Name = "Authorization",
                //    In = OpenApiSecurityApiKeyLocation.Header,
                //    Description = "Copy this into the value field: Bearer {token}"
                //};
                //config.AddSecurity("JWT Token", Enumerable.Empty<string>(), openApiSecurityScheme);
                //config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT Token"));

                config.AddOperationFilter(context =>
                {
                    if ("PostJsonResponseValue".Equals(context.MethodInfo.GetBaseDefinition().Name))
                    {
                        context.OperationDescription.Operation.Parameters.Add(
                            new OpenApiParameter()
                            {
                                Name = "Token",
                                Kind = OpenApiParameterKind.Header,
                                Type = JsonObjectType.String,
                                IsRequired = true,
                                Description = "Token Description",
                                Default = "",
                            });
                    }
                    //if (context.MethodInfo.CustomAttributes
                    //    .Where(w => w.AttributeType == typeof(OperationHeaderToken))
                    //    .Any())
                    //{
                    //    context.OperationDescription.Operation.Parameters.Add(
                    //        new OpenApiParameter()
                    //        {
                    //            Name = "Token",
                    //            Kind = OpenApiParameterKind.Header,
                    //            Type = JsonObjectType.String,
                    //            IsRequired = true,
                    //            Description = "Token Description",
                    //            Default = "",
                    //        });
                    //}
                    return true;
                });
                //config.OperationProcessors.Add(new AddRequiredHeaderParameter());
            });
            // Add Swagger v2 document
            // services.AddSwaggerDocument();
        }
        public void Configure(IApplicationBuilder app)
        {
            // serve OpenAPI/Swagger documents
            app.UseOpenApi(config =>
            {
                // 這裡的 Path 用來設定 OpenAPI 文件的路由 (網址路徑) (一定要以 / 斜線開頭)
                config.Path = "/swagger/v1/swagger.json";

                // 這裡的 DocumentName 必須跟 services.AddOpenApiDocument() 的時候設定的 DocumentName 一致！
                config.DocumentName = "v1";

                config.PostProcess = (document, http) =>
                {
                    document.Info.TermsOfService = "https://go.microsoft.com/fwlink/?LinkID=206977";

                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "Test",
                        Email = "test@mail.com",
                        Url = "https://localhost:44316/"
                    };
                    document.Info.License = new OpenApiLicense
                    {
                        Name = "The MIT License",
                        Url = "https://localhost:44316/licenses/MIT"
                    };
                };
            });
            // serve Swagger UI
            app.UseSwaggerUi3();
            // serve ReDoc UI
            app.UseReDoc(config =>
            {
                config.Path = "/redoc";
            });
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
