using FluentValidation;
using Remosys.Api.Core.Application.Departments.Command.Update;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.Departments
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(dto => dto.Id).NotEmpty().NotNull();
            RuleFor(dto => dto.Name).NotEmpty().NotNull().WithMessage(ResponseMessage.NameIsRequired);

        }
    }
}