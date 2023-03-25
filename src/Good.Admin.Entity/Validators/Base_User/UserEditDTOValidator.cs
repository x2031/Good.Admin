using FluentValidation;

namespace Good.Admin.Entity.Validators.Base_User
{
    public class UserEditDTOValidator : AbstractValidator<UserEditDTO>
    {
        public UserEditDTOValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithName("用户名").WithMessage("{PropertyName}不能为空");
            RuleFor(x => x.RealName).NotNull().WithName("姓名").WithMessage("{PropertyName}不能为空");
            RuleFor(x => x.DepartmentId).NotNull().WithName("部门").WithMessage("{PropertyName}不能为空");
        }
    }
}

