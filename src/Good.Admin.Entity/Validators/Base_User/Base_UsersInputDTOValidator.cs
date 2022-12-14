using FluentValidation;
using Good.Admin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.Entity
{
    public class Base_UsersInputDTOValidator : AbstractValidator<Base_UsersInputDTO>
    {
        public Base_UsersInputDTOValidator()
        {
            RuleFor(x => x.userId).MaximumLength(10).WithName("用户id").WithMessage("{PropertyName}长度不能超过{MaxLength}");
            RuleFor(x => x.keyword).MaximumLength(10).WithName("搜索字符串").WithMessage("{PropertyName}长度不能超过{MaxLength}");
        }
    }


}
