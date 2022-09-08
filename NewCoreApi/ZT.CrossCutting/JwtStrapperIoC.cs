using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZT.Common.Utils;
using ZT.Domain.Core.Jwt.Model;

namespace ZT.CrossCutting
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/7 16:59:37 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public static class JwtStrapperIoC
    {
        public static void AddJwtConfiguration(this IServiceCollection services)
        {
            services.Configure<JwtModel>(AppUtils.Configuration.GetSection("JwtAuth"));
            var token = AppUtils.Configuration.GetSection("JwtAuth").Get<JwtModel>();

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Security)),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,

                    //ValidateIssuer = true,//是否验证Issuer
                    //ValidateAudience = true,//是否验证Audience
                    //ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    //ValidateLifetime = true,//是否验证超时  当设置exp和nbf时有效 同时启用ClockSkew 
                   // ClockSkew = TimeSpan.FromSeconds(30),//注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                    //ValidAudience = jwtConfig.Audience,//Audience
                    //ValidIssuer = jwtConfig.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    //RequireExpirationTime = true,///否要求Token的Claims中必须包含Expires
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtAuth:SecurityKey"]))//拿到SecurityKey
                    /*AudienceValidator = (m, n, z) => 
                        m != null && m.FirstOrDefault()!.Equals(JwtConst.ValidAudience),*/
                };
                x.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context => {
                        var values = context.Request.Headers["accessToken"];
                        context.Token = values.FirstOrDefault();
                        return Task.CompletedTask;
                    },
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

            services.AddAuthorization(options => {
                options.AddPolicy("App", policy => policy.RequireRole("App").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            });
        }
    }
}
