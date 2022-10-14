using FluentValidation;
using Good.Admin.Util;

namespace Good.Admin.Entity
{
    public class Base_RoleActionInputDTO_PageValidator : AbstractValidator<PageInput<Base_RoleActionInputDTO>>
    {
        public Base_RoleActionInputDTO_PageValidator()
        {
            RuleFor(x => x.Search.RoleId).NotNull().WithName("角色Id").WithMessage("{PropertyName}不能为空");
        }
    }
}
