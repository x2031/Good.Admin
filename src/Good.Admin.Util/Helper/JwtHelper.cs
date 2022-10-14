using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Good.Admin.Util
{
    public class JwtHelper
    {


        private static readonly string jwtSecret = Appsettings.app(new string[] { "Jwt", "secret" });
        private static readonly int expires = Appsettings.app(new string[] { "Jwt", "accessExpireHours" }).ToInt();

        /// <summary>
        /// 根据用户信息生成token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="jwtOption"></param>
        /// <returns></returns>
        public static string BuildToken(TokenModelJwt user, JwtOption jwtOption, bool isRefrash = false)
        {
            //添加Claims信息
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()), // iat (Issued At)：签发时间
                    new Claim(JwtRegisteredClaimNames.Jti, user.UserName), // jti (JWT ID)：编号
                    new Claim(ClaimTypes.Name, user.UserName)
            };

            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: jwtOption.Issuer,
                audience: jwtOption.Audience,
                claims: claims,
                notBefore: now,
                expires: isRefrash ? DateTime.Now.AddMinutes(jwtOption.RefreshExpires) : DateTime.Now.AddMinutes(jwtOption.Expires),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Secret)), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// 根据用户信息生成token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jwtOption"></param>
        /// <param name="isRefrash"></param>
        /// <returns></returns>
        public static string BuildToken(string userId, DateTime dateTime, bool isRefrash = false)
        {
            //添加Claims信息
            var claims = new[]
            {
                new Claim("userId",userId)
            };


            var token = new JwtSecurityToken(
                issuer: string.Empty,
                audience: string.Empty,
                claims: claims,
                notBefore: dateTime,
                expires: dateTime.AddHours(expires),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            TokenModelJwt tokenModelJwt = new TokenModelJwt();

            // token校验
            if (!string.IsNullOrWhiteSpace(jwtStr) && jwtHandler.CanReadToken(jwtStr))
            {

                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

                object role;

                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);

                tokenModelJwt = new TokenModelJwt
                {
                    UserName = (jwtToken.Id)
                };
            }
            return tokenModelJwt;
        }
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool TokenSafeVerify(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = jwtSecret;
            var keyByteArray = Encoding.UTF8.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = jwtHandler.ReadJwtToken(token);
            return jwt.RawSignature == Microsoft.IdentityModel.JsonWebTokens.JwtTokenUtilities.CreateEncodedSignature(jwt.RawHeader + "." + jwt.RawPayload, signingCredentials);
        }
    }


    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// Id
        /// </summary>
        public string UserName { get; set; }
    }
    public class JwtOption
    {
        /// <summary>
        /// 签发人（一般写接口请求地址）
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 受众（一般写接口请求地址）
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 超时时间 单位分钟
        /// </summary>
        public int Expires { get; set; }
        /// <summary>
        /// 刷新时间  单位分钟
        /// </summary>
        public int RefreshExpires { get; set; }
        /// <summary>
        /// 哈希签名的秘钥  签名 Signing 证书 Credentials
        /// </summary>
        public string Secret { get; set; }
    }
}
