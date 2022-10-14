using FluentValidation;

namespace Good.Admin.Entity
{
    public class IdInputDTOValidator : AbstractValidator<IdInputDTO>
    {
        public IdInputDTOValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithName("ID").WithMessage("{PropertyName}不能为空!");
        }
    }
}
