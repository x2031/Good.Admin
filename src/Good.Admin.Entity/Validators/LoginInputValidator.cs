using FluentValidation;

namespace Good.Admin.Entity
{
    /// <summary>
    /// 参数校验
    /// </summary>
    public class LoginInputValidator : AbstractValidator<LoginInputDTO>
    {
        public LoginInputValidator()
        {
            RuleFor(x => x.userName).NotEmpty().WithName("用户名").WithMessage("请输入{PropertyName}!");
            RuleFor(x => x.password).NotEmpty().WithName("密码").WithMessage("请输入{PropertyName}!");
        }
    }
}
