using FluentValidation;
using Good.Admin.Util;

namespace Good.Admin.Entity
{
    public class Base_RoleActionDTO_PageValidator : AbstractValidator<PageInput<RoleActionDTO>>
    {
        public Base_RoleActionDTO_PageValidator()
        {
            RuleFor(x => x.Search.RoleId).NotNull().WithName("角色Id").WithMessage("{PropertyName}不能为空");
        }
    }
}
