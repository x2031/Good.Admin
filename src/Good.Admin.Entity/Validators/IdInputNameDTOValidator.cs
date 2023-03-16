﻿using FluentValidation;

namespace Good.Admin.Entity.Validators
{
    public class NameInputDTOValidator : AbstractValidator<NameInputDTO>
    {
        public NameInputDTOValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithName("name").WithMessage("{PropertyName}不能为空!");
        }
    }
}
