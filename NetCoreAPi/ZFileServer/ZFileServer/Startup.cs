using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZFile.Core;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using ZFile.Extensions.Authorize;
using ZFile.Extensions.JWT;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ZFile.Service.DtoModel;

namespace ZFileServer
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
            
            //�Զ�ע��
            AddAssembly(services, "ZFile.Service");
            //services.AddTransient<DbContext>();
            //�����ͼ����������ı�������
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = new PathString("/Home/Index");
                })
                .AddJwtBearer(JwtAuthorizeAttribute.JwtAuthenticationScheme, o =>
                {
                    var jwtConfig = new JwtAuthConfigModel();
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//�Ƿ���֤Issuer
                        ValidateAudience = true,//�Ƿ���֤Audience
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                        ValidateLifetime = true,//�Ƿ���֤��ʱ  ������exp��nbfʱ��Ч ͬʱ����ClockSkew 
                        ClockSkew = TimeSpan.FromSeconds(30),//ע�����ǻ������ʱ�䣬�ܵ���Чʱ��������ʱ�����jwt�Ĺ���ʱ�䣬��������ã�Ĭ����5����
                        ValidAudience = jwtConfig.Audience,//Audience
                        ValidIssuer = jwtConfig.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
                        RequireExpirationTime = true,
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("App", policy => policy.RequireRole("App").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("AdminOrApp", policy => policy.RequireRole("Admin,App").Build());
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
          //  InitializeAsync(app);
        }

        public static void InitializeAsync(IApplicationBuilder app)
        {
            var db = app.ApplicationServices.GetRequiredService<DbContext>();
            db.InitDb();
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
