using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProgrammingLanguages.Commands.UpdateProgrammingLanguage
{
    public class UpdateProgrammingLanguageCommandValidator : AbstractValidator<UpdateProgrammingLanguageCommand>
    {
        public UpdateProgrammingLanguageCommandValidator()
        {
            RuleFor(pl => pl.Name).NotNull().NotEmpty();
            RuleFor(pl => pl.Name).MinimumLength(1).MaximumLength(50);
            RuleFor(pl => pl.Description).MinimumLength(1).MaximumLength(5000);
        }
    }
}
