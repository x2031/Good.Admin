using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Repository;
using Xunit;

namespace Good.Admin.Test.Business
{

    public class Base_UserBusinessTest : BaseTest
    {

        private readonly IBase_UserBusiness _business;
        private readonly IOperator _operator;
        private readonly IUnitOfWork _IUnitOfWork;

        public Base_UserBusinessTest()
        {

            _operator = GetService<IOperator>();
            _business = GetService<IBase_UserBusiness>();
        }
        /// <summary>
        /// 无md5加密登录
        /// </summary>
        [Fact]
        public async void UseLoginNoMd5()
        {
            LoginInputDTO inputDTO = new LoginInputDTO();
            inputDTO.userName = "admin";
            inputDTO.password = "123456";
            var res = await _business.LoginAsync(inputDTO);
            Assert.True(res == "Admin");
        }
        /// <summary>
        /// md5密码加密登录
        /// </summary>
        [Fact]
        public async void UseLoginMd5()
        {
            LoginInputDTO inputDTO = new LoginInputDTO();
            inputDTO.userName = "admin";
            inputDTO.password = "e10adc3949ba59abbe56e057f20f883e";
            var res = await _business.LoginAsync(inputDTO, true);
            Assert.True(res == "Admin");
        }
    }
}
