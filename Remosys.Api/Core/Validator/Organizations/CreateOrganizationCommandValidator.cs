using FluentValidation;
using Remosys.Api.Core.Application.Organization.Command.Create;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.Organizations
{
    public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
    {
        public CreateOrganizationCommandValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage(ResponseMessage.NameIsRequired)
                .NotNull().WithMessage(ResponseMessage.NameIsRequired);
        }
    }
}