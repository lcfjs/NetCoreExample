using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Example.WebCore.Filter;
using Example.WebCore.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Example.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected Dictionary<string, string> SwaggerDocs = new Dictionary<string, string> {
             { "Default","Ĭ�Ͻӿ�" },
             { "Mobile","�ֻ�APP�ӿ�" },
             { "Applet","С����ӿ�" }
        };

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region JWT
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //    {
            //        ValidateIssuer = true,//�Ƿ���֤Issuer
            //        ValidIssuer = Common.JwtTools.JwtSetting.Issuer,
            //        ValidateAudience = true,//�Ƿ���֤Audience
            //        ValidAudience = Common.JwtTools.JwtSetting.Audience,
            //        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
            //        //ClockSkew = TimeSpan.FromSeconds(60),
            //        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(Common.JwtTools.JwtSetting.SecretKey))
            //    };
            //});
            #endregion

            #region Swagger
#if DEBUG
            services.AddSwaggerGen(options =>
            {
                foreach (var item in SwaggerDocs)
                {
                    options.SwaggerDoc(item.Key, new OpenApiInfo { Title = item.Key, Description = item.Value, Version = "1.0" });
                }

                //��ֹĬ������ʾ��������
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    System.Reflection.MethodInfo methodInfo;
                    if (!apiDesc.TryGetMethodInfo(out methodInfo))
                    {
                        return false;
                    }

                    var versions = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(x => x.GroupName);
                    if (docName.ToLower() == "default" && versions.FirstOrDefault() == null)
                    {
                        return true;
                    }
                    return versions.Any(x => x.ToString() == docName);
                });

                //����Bearer
                var security = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference() { Id = "Bearer", Type = ReferenceType.SecurityScheme },
                        },
                        new List<string>()
                    }
                };
                options.AddSecurityRequirement(security);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Ȩ����֤(���ݽ�������ͷ�н��д���) �����ṹ: \"Bearer {token}\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });

                //ע��
                //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var basePath = AppContext.BaseDirectory;
                options.IncludeXmlComments(Path.Combine(basePath, "Example.API.xml"));
            });
#endif
            #endregion

            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<AuthorizationFilter>();
                options.Filters.Add<ActionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var item in SwaggerDocs)
                {
                    options.SwaggerEndpoint($"/swagger/{item.Key}/swagger.json", $"{item.Key}");
                }
            });

            app.UseMiddleware<RequestMiddleware>();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
