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
                // �]�w���W�� (���n) (�w�]��: v1)
                config.DocumentName = "v1";

                // �]�w���� API ������T
                config.Version = "SwaggerDocument Version";

                // �]�w�����D (����� Swagger/ReDoc UI ���ɭԷ|��ܦb�e���W)
                config.Title = "SwaggerDocument Title";

                // �]�w���²�n����
                config.Description = "SwaggerDocument Description";

                // JWT ���ұ��v���]�w
                // �аȥ��N config.AddSecurity() �P new AspNetCoreOperationSecurityScopeProcessor() ���]�w���@�˪��w���W��(JWT Token)
                // �Ҧ��M�פ��u�n���M��[Authorize] �ݩ�(Attribute)�� API action ���|�۰ʮM�� JWT Token ���w���]�w
                // �b�u�W�o�e API �n�D�ɡA�N�|�۰ʰe�X Bearer Token
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
                // �o�̪� Path �Ψӳ]�w OpenAPI ��󪺸��� (���}���|) (�@�w�n�H / �׽u�}�Y)
                config.Path = "/swagger/v1/swagger.json";

                // �o�̪� DocumentName ������ services.AddOpenApiDocument() ���ɭԳ]�w�� DocumentName �@�P�I
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
