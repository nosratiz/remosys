using FluentValidation;
using Remosys.Api.Core.Application.Employees.Command.Update;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.Employees
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(dto => dto.Id).NotEmpty().NotNull();

            RuleFor(dto => dto.Code).NotEmpty().NotNull().WithMessage(ResponseMessage.CodeIsRequired);

            RuleFor(dto => dto.UserId).NotEmpty().NotNull();

            RuleFor(dto => dto).Must(dto =>
                    (!dto.EndContract.HasValue && !dto.StartContract.HasValue) || dto.EndContract > dto.StartContract)
                .WithMessage(ResponseMessage.InvalidDateContract);
        }
    }
}