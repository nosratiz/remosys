using FluentValidation;
using Remosys.Api.Core.Application.Departments.Command.Create;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.Departments
{
    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty().NotNull().WithMessage(ResponseMessage.NameIsRequired);
        }
    }
}