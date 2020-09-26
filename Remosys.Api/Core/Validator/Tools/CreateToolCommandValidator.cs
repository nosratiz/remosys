using FluentValidation;
using Remosys.Api.Core.Application.Tools.Command.Create;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.Tools
{
    public class CreateToolCommandValidator : AbstractValidator<CreateToolCommand>
    {
        public CreateToolCommandValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty().NotNull().WithMessage(ResponseMessage.NameIsRequired);
        }
    }
}