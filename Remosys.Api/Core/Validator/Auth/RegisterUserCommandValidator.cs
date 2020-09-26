using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Remosys.Api.Core.Application.Auth.Command.RegisterCommand;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;

namespace Remosys.Api.Core.Validator.Auth
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IMongoRepository<User> _userRepository;
        public RegisterUserCommandValidator(IMongoRepository<User> userRepository)
        {
            _userRepository = userRepository;

            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.FirstName).NotNull().WithMessage(ResponseMessage.NameIsRequired)
                .NotEmpty().WithMessage(ResponseMessage.NameIsRequired);

            RuleFor(dto => dto.LastName).NotNull().WithMessage(ResponseMessage.LastNameIsRequired)
                .NotEmpty().WithMessage(ResponseMessage.LastNameIsRequired);

            RuleFor(dto => dto.Mobile)
                .NotEmpty().WithMessage(ResponseMessage.MobileIsRequired)
                .NotNull().WithMessage(ResponseMessage.MobileIsRequired)
                .Must(dto => dto.StartsWith("09"))
                .WithMessage(ResponseMessage.MobileFormatInvalid)
                .MinimumLength(11).WithMessage(ResponseMessage.MobileFormatInvalid)
                .MaximumLength(11).WithMessage(ResponseMessage.MobileFormatInvalid);

            RuleFor(dto => dto.NationalCode).NotNull().WithMessage(ResponseMessage.NationalCodeIsRequired)
                .NotEmpty().WithMessage(ResponseMessage.NationalCodeIsRequired)
                .MinimumLength(10).MaximumLength(10);

            RuleFor(dto => dto.Password)
                .NotNull().NotEmpty().WithMessage(ResponseMessage.PasswordIsRequired).MinimumLength(6);

            RuleFor(dto => dto).MustAsync(ValidMobileAddress).WithMessage(ResponseMessage.MobileAlreadyExist);
        }

        public async Task<bool> ValidMobileAddress(RegisterCommand registerCommand, CancellationToken cancellationToken)
            => !await _userRepository.ExistsAsync(x => x.IsDelete == false && x.Mobile == registerCommand.Mobile, cancellationToken);
        

    }
}