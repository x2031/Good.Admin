using FluentValidation;

namespace Good.Admin.Entity
{
    public class Base_UsersInputDTOValidator : AbstractValidator<Base_UsersInputDTO>
    {
        public Base_UsersInputDTOValidator()
        {
            RuleFor(x => x.Id).MaximumLength(50).WithName("用户id").WithMessage("{PropertyName}长度不能超过{MaxLength}");
        }
    }


}
