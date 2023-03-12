using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Xunit;

namespace Good.Admin.Test.Business
{

    /// <summary>
    /// 用户相关业务层测试
    /// </summary>
    public class Base_UserBusinessTest : IClassFixture<TestServerFixture>
    {

        private readonly IBase_UserBusiness _business;
        private readonly IOperator _operator;
        private readonly TestServerFixture _testServerFixture;

        public Base_UserBusinessTest(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            _operator = _testServerFixture.GetService<IOperator>();
            _business = _testServerFixture.GetService<IBase_UserBusiness>();
        }
        /// <summary>
        /// 无md5加密登录
        /// </summary>
        [Fact]
        public async void UseLoginNoMd5()
        {
            LoginInputDTO inputDTO = new LoginInputDTO();
            inputDTO.userName = "Admin";
            inputDTO.password = "123456";
            var res = await _business.LoginAsync(inputDTO, true);
            Assert.True(res == "Admin");
        }
        /// <summary>
        /// 无md5加密登录
        /// </summary>
        [Fact]
        public async void UseLoginNoMd52()
        {
            LoginInputDTO inputDTO = new LoginInputDTO();
            inputDTO.userName = "admin";
            inputDTO.password = "123456";
            var res = await _business.LoginAsync(inputDTO, true);
            Assert.True(res == "Admin");
        }
        /// <summary>
        /// md5密码加密登录
        /// </summary>
        [Fact]
        public async void UseLoginMd5()
        {
            LoginInputDTO inputDTO = new LoginInputDTO();
            inputDTO.userName = "Admin";
            inputDTO.password = "e10adc3949ba59abbe56e057f20f883e";
            var res = await _business.LoginAsync(inputDTO);
            Assert.True(res == "Admin");
        }
        /// <summary>
        /// md5密码加密登录
        /// </summary>
        [Fact]
        public async void UseLoginMd52()
        {
            LoginInputDTO inputDTO = new LoginInputDTO();
            inputDTO.userName = "admin";
            inputDTO.password = "e10adc3949ba59abbe56e057f20f883e";
            var res = await _business.LoginAsync(inputDTO);
            Assert.True(res == "Admin");
        }
    }
}
