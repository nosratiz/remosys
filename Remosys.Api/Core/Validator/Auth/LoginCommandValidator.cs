using FluentValidation;
using Remosys.Api.Core.Application.Auth.Command;
using Remosys.Api.Core.Application.Auth.Command.LoginCommand;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.Auth
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {

            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Mobile)
                .NotEmpty().WithMessage(ResponseMessage.MobileIsRequired)
                .Must(dto => dto.StartsWith("09"))
                .WithMessage(ResponseMessage.MobileFormatInvalid);

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage(ResponseMessage.PasswordIsRequired);
        }
    }
}