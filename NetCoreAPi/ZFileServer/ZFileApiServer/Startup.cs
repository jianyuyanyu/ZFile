using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using ZFile.Extensions.Authorize;
using ZFile.Extensions.JWT;

namespace ZFileApiServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            this.Configuration = builder.Build();
            BaseConfigModel.SetBaseConfig(Configuration, env.ContentRootPath, env.WebRootPath);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddAssembly(services, "ZFile.Service");

            //解决视图输出内容中文编码问题
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(JwtAuthorizeAttribute.JwtAuthenticationScheme)
               .AddJwtBearer(JwtAuthorizeAttribute.JwtAuthenticationScheme,o =>
               {
                   var jwtConfig = new JwtAuthConfigModel();
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidateLifetime = true,//是否验证超时  当设置exp和nbf时有效 同时启用ClockSkew 
                        ClockSkew = TimeSpan.Zero,//注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                        ValidAudience = jwtConfig.Audience,//Audience
                        ValidIssuer = jwtConfig.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                        RequireExpirationTime = true,///否要求Token的Claims中必须包含Expires
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtAuth:SecurityKey"]))//拿到SecurityKey
                    };
                   o.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                            // 如果过期，则把<是否过期>添加到，返回头信息中
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers.Add("Token-Expired", "true");
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

            #region 授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("App", policy => policy.RequireRole("App").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("AdminOrApp", policy => policy.RequireRole("Admin,App").Build());
            });
            #endregion
          
            services.AddSwaggerGen(c =>
            {
                c.IgnoreObsoleteActions();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "我的APi", Version = "v1" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "ZFileApiServer.xml");
                var entityXmlPath = Path.Combine(basePath, "ZFileApiServer.xml");
                c.IncludeXmlComments(xmlPath, true);
                c.IncludeXmlComments(entityXmlPath);


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT-Test: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //这两个参数均有修改
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] {  }
                    }
                  });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("Any", policy =>
                {
                    policy.WithOrigins("http://localhost:8081", "http://localhost:8082", "http://localhost:8083")
                      .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
           
            }

            app.UseHttpsRedirection();


            app.UseRouting();


            app.UseAuthentication();

            app.UseAuthorization();


            app.UseSwagger();
            app.UseCors("Any");
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZFileApiServer v1"));


       

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>  
        /// 自动注册服务——获取程序集中的实现类对应的多个接口
        /// </summary>
        /// <param name="services">服务集合</param>  
        /// <param name="assemblyName">程序集名称</param>
        public void AddAssembly(IServiceCollection services, string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType).ToList();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    if (interfaceType.Length == 1)
                    {
                        services.AddTransient(interfaceType[0], item);
                    }
                    if (interfaceType.Length > 1)
                    {
                        services.AddTransient(interfaceType[1], item);
                    }
                }
            }
        }
    }
}
