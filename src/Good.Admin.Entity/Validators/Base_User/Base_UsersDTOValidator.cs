using FluentValidation;

namespace Good.Admin.Entity
{
    public class Base_UsersDTOValidator : AbstractValidator<UsersDTO>
    {
        public Base_UsersDTOValidator()
        {
            RuleFor(x => x.Id).MaximumLength(50).WithName("用户id").WithMessage("{PropertyName}长度不能超过{MaxLength}");
        }
    }
}
