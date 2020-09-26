using FluentValidation;
using Remosys.Api.Core.Application.Tools.Command.Update;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.Tools
{
    public class UpdateToolCommandValidator : AbstractValidator<UpdateToolCommand>
    {
        public UpdateToolCommandValidator()
        {
            RuleFor(dto => dto.Id).NotEmpty().NotNull();
            RuleFor(dto => dto.Name).NotEmpty().NotNull().WithMessage(ResponseMessage.NameIsRequired);

        }
    }
}