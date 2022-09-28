using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProgrammingLanguages.Commands.CreateProgrammingLanguage
{
    public class CreateProgrammingLanguageCommandValidator : AbstractValidator<CreateProgrammingLanguageCommand>
    {
        public CreateProgrammingLanguageCommandValidator()
        {
            RuleFor(pl => pl.Name).NotEmpty().NotNull();
            RuleFor(pl => pl.Name).MinimumLength(1).MaximumLength(50);
            RuleFor(pl => pl.Description).MinimumLength(1).MaximumLength(5000);            
        }
    }
}
