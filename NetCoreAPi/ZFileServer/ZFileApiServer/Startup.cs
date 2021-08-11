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

            //�����ͼ����������ı�������
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(JwtAuthorizeAttribute.JwtAuthenticationScheme)
               .AddJwtBearer(JwtAuthorizeAttribute.JwtAuthenticationScheme,o =>
               {
                   var jwtConfig = new JwtAuthConfigModel();
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,//�Ƿ���֤Issuer
                        ValidateAudience = true,//�Ƿ���֤Audience
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                        ValidateLifetime = true,//�Ƿ���֤��ʱ  ������exp��nbfʱ��Ч ͬʱ����ClockSkew 
                        ClockSkew = TimeSpan.Zero,//ע�����ǻ������ʱ�䣬�ܵ���Чʱ��������ʱ�����jwt�Ĺ���ʱ�䣬��������ã�Ĭ����5����
                        ValidAudience = jwtConfig.Audience,//Audience
                        ValidIssuer = jwtConfig.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
                        RequireExpirationTime = true,///��Ҫ��Token��Claims�б������Expires
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtAuth:SecurityKey"]))//�õ�SecurityKey
                    };
                   o.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                            // ������ڣ����<�Ƿ����>��ӵ�������ͷ��Ϣ��
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers.Add("Token-Expired", "true");
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

            #region ��Ȩ
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "�ҵ�APi", Version = "v1" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "ZFileApiServer.xml");
                var entityXmlPath = Path.Combine(basePath, "ZFileApiServer.xml");
                c.IncludeXmlComments(xmlPath, true);
                c.IncludeXmlComments(entityXmlPath);


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT-Test: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //���������������޸�
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
        /// �Զ�ע����񡪡���ȡ�����е�ʵ�����Ӧ�Ķ���ӿ�
        /// </summary>
        /// <param name="services">���񼯺�</param>  
        /// <param name="assemblyName">��������</param>
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
