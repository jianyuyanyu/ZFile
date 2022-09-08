using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZT.Common.Utils;
using ZT.Domain.Core.Jwt.Model;

namespace ZT.Domain.Core.Jwt
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/9/7 14:14:52 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class JwtAuthService
    {
        public static string IssueJwt(JwtToken token)
        {
            var jwtModel = AppUtils.GetConfig(JwtModel.Name).Get<JwtModel>();
            var claims = new List<Claim>();

            var dateTime = DateTime.UtcNow;
            //var dateTime = DateTime.UtcNow;
            //var claims = new Claim[]
            //    {
            //        new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid),
            //        new Claim("UserName", tokenModel.UserName),//用户名
            //        new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(dateTime).ToUnixTimeSeconds()}"),
            //        new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(dateTime).ToUnixTimeSeconds()}") , 
            //        //这个就是过期时间，目前是过期100秒，可自定义，注意JWT有自己的缓冲过期时间
            //        new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(dateTime.AddMinutes(exp)).ToUnixTimeSeconds()}"),
            //        new Claim(JwtRegisteredClaimNames.Iss,jwtConfig.Issuer),
            //        new Claim(JwtRegisteredClaimNames.Aud,jwtConfig.Audience),
            //        new Claim(ClaimTypes.Role,tokenModel.Role),
            //   };
            ////秘钥
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JWTSecretKey));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var jwt = new JwtSecurityToken(
            //    issuer: jwtConfig.Issuer,
            //    audience: jwtConfig.Audience,
            //    claims: claims,
            //     expires: dateTime.AddMinutes(exp),
            //    signingCredentials: creds);
            //var jwtHandler = new JwtSecurityTokenHandler();
            //var encodedJwt = jwtHandler.WriteToken(jwt);

            //每次登陆动态刷新
            //JwtConst.ValidAudience = token.Id + DateTime.Now.ToString(CultureInfo.InvariantCulture);
            claims.AddRange(new[] {
            new Claim (nameof (JwtToken.Id), token.Id.ToString()),
            new Claim (nameof (JwtToken.FullName), token.FullName),
            new Claim (nameof (JwtToken.RoleArray), token.RoleArray),
            new Claim (nameof (JwtToken.Time), token.Time.ToString (CultureInfo.InvariantCulture)),
            new Claim (ClaimTypes.Role, token.Role)
        });
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModel.Security));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                issuer: jwtModel.Issuer,
                //audience: JwtConst.ValidAudience,
                audience: jwtModel.Audience,
                claims: claims,
                expires: dateTime.AddMinutes(jwtModel.WebExp),
                signingCredentials: cred);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static JwtToken SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object? userName;
            object? roleArray;
            object? time;
            object? id;
            object? role;
            try
            {
                jwtToken.Payload.TryGetValue("FullName", out userName);
                jwtToken.Payload.TryGetValue("RoleArray", out roleArray);
                jwtToken.Payload.TryGetValue("Time", out time);
                jwtToken.Payload.TryGetValue("Id", out id);
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return new JwtToken()
            {
                Id = Convert.ToInt64(id),
                FullName = userName?.ToString(),
                RoleArray = roleArray?.ToString(),
                Role = role?.ToString(),
                Time = Convert.ToDateTime(time)
            };
        }
    }
}
