using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSwag.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Good.Admin.API.Controllers.Base_Manage
{
    [OpenApiTag("主页")]
    [Route("api/[controller]/[action]")]
    public class HomeController : BaseApiController
    {
        #region DI
        readonly IBase_UserBusiness _userBus;
        //readonly IPermissionBusiness _permissionBus;
        //readonly IBase_UserBusiness _userBus;
        readonly IOperator _operator;
        private readonly JwtOptions _jwtOptions;
        public HomeController(
            IBase_UserBusiness userBus,
            IOptions<JwtOptions> jwtOptions
            )
        {
            _userBus = userBus;
            _jwtOptions = jwtOptions.Value;
        }
        #endregion
        /// <summary>
        /// 用户登录(获取token)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<TokenDTO> Login(LoginInputDTO input)
        {
            TokenDTO result = new TokenDTO();
            var userId = await _userBus.LoginAsync(input, true);
            var claims = new[]
            {
                new Claim("userId",userId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.Now;

            var jwtToken = new JwtSecurityToken(
                string.Empty,
                string.Empty,
                claims,
                expires: now.AddHours(_jwtOptions.AccessExpireHours),
                signingCredentials: credentials);

            result.token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            result.expires_in = 24 * 60 * 60;
            return result;
        }

        /// <summary>
        /// 刷新token接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]
        [ApiPermission("Base_User.RefreshToken")]
        public async Task<TokenDTO> RefreshToken(TokenDTO tokenDTO)
        {
            TokenDTO result = new TokenDTO();

            if (string.IsNullOrEmpty(tokenDTO.token))
            {
                throw new BusException("请登录!");
            }

            var now = DateTime.Now;
            var tokenHandler = new JwtSecurityTokenHandler();
            var readedToken = tokenHandler.ReadToken(tokenDTO.token);
            var securityToken = (JwtSecurityToken)readedToken;
            if (securityToken != null && JwtHelper.TokenSafeVerify(tokenDTO.token))
            {
                //JwtHelper.BuildToken(_operator.UserId);
                result.token = JwtHelper.BuildToken("Admin", now);
                result.expires_in = 24 * 60 * 60;
                return result;
            }
            else
            {
                throw new BusException("认证失败");
            }
        }
    }
}
