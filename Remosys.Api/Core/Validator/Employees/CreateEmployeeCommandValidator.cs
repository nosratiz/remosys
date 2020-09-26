using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Remosys.Api.Core.Application.Employees.Command.Create;
using Remosys.Api.Core.Application.Users.Command.CreateUser;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;

namespace Remosys.Api.Core.Validator.Employees
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        private readonly IMongoRepository<User> _userRepository;
        public CreateEmployeeCommandValidator(IMongoRepository<User> userRepository)
        {
            _userRepository = userRepository;

            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Code).NotEmpty().NotNull().WithMessage(ResponseMessage.CodeIsRequired);

            RuleFor(dto => dto.Email)
                .EmailAddress().WithMessage(ResponseMessage.InvalidEmailFormat);

            RuleFor(dto => dto.Mobile).NotEmpty()
                .WithMessage(ResponseMessage.MobileIsRequired)
                .NotNull().WithMessage(ResponseMessage.MobileIsRequired)
                .Must(dto => dto.StartsWith("09")).WithMessage(ResponseMessage.MobileFormatInvalid)
                .MinimumLength(11).WithMessage(ResponseMessage.MobileFormatInvalid).MaximumLength(11)
                .WithMessage(ResponseMessage.MobileFormatInvalid);

            RuleFor(dto => dto).MustAsync(ValidMobile)
                .WithMessage(ResponseMessage.MobileAlreadyExist)
                .MustAsync(ValidEmailAddress).WithMessage(ResponseMessage.EmailAlreadyExist);

            RuleFor(dto => dto).Must(dto =>
                (!dto.EndContract.HasValue && !dto.StartContract.HasValue) || dto.EndContract > dto.StartContract)
                .WithMessage(ResponseMessage.InvalidDateContract);
        }

        private async Task<bool> ValidEmailAddress(CreateEmployeeCommand createUserCommand, CancellationToken cancellationToken)
            => !await _userRepository.ExistsAsync(x => x.Email == createUserCommand.Email && x.IsDelete == false, cancellationToken);


        private async Task<bool> ValidMobile(CreateEmployeeCommand createUserCommand, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(createUserCommand.Mobile))
                if (await _userRepository.ExistsAsync(x => x.IsDelete == false && x.Mobile == createUserCommand.Mobile, cancellationToken))
                    return false;

            return true;
        }
    }
}