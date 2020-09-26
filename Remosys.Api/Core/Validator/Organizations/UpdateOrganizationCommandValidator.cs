using FluentValidation;
using Remosys.Api.Core.Application.Organization.Command.Update;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.Organizations
{
    public class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
    {
        public UpdateOrganizationCommandValidator()
        {

            RuleFor(dto => dto.Id).NotEmpty().NotNull();
            RuleFor(dto => dto.Name).NotEmpty().NotNull().WithMessage(ResponseMessage.NameIsRequired);

        }
    }
}