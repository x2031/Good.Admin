using FluentValidation;
using Good.Admin.Common;

namespace Good.Admin.Entity
{
    public class Base_UsersDTO_PageValidator : AbstractValidator<PageInput<UsersDTO>>
    {
        public Base_UsersDTO_PageValidator()
        {
            RuleFor(x => x.Search.Id).MaximumLength(50).WithName("用户id").WithMessage("{PropertyName}长度不能超过{MaxLength}");
        }
    }
}
