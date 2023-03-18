using FluentValidation;
using Good.Admin.Util;

namespace Good.Admin.Entity
{
    public class Base_UsersInputDTO_PageValidator : AbstractValidator<PageInput<Base_UsersInputDTO>>
    {
        public Base_UsersInputDTO_PageValidator()
        {
            RuleFor(x => x.Search.Id).MaximumLength(50).WithName("用户id").WithMessage("{PropertyName}长度不能超过{MaxLength}");
        }
    }
}
