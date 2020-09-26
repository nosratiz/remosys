using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Remosys.Api.Core.Application.Users.Command.CreateUser;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;

namespace Remosys.Api.Core.Validator.Users
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IMongoRepository<User> _userRepository;
        public CreateUserCommandValidator(IMongoRepository<User> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(dto => dto.FirstName)
                .NotEmpty().WithMessage(ResponseMessage.NameIsRequired)
                .MinimumLength(2);

            RuleFor(dto => dto.LastName)
                .NotEmpty().WithMessage(ResponseMessage.LastNameIsRequired)
                .MinimumLength(2);

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage(ResponseMessage.PasswordIsRequired)
                .MinimumLength(6).WithMessage(ResponseMessage.MinPasswordLength);

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage(ResponseMessage.EmailIsRequired)
                .EmailAddress().WithMessage(ResponseMessage.InvalidEmailFormat);


            RuleFor(dto => dto).MustAsync(ValidMobile)
                .WithMessage(ResponseMessage.MobileAlreadyExist)
                .MustAsync(ValidEmailAddress).WithMessage(ResponseMessage.EmailAlreadyExist);
        }


        private async Task<bool> ValidEmailAddress(CreateUserCommand createUserCommand, CancellationToken cancellationToken)
        => !await _userRepository.ExistsAsync(x => x.Email == createUserCommand.Email && x.IsDelete == false, cancellationToken);


        private async Task<bool> ValidMobile(CreateUserCommand createUserCommand, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(createUserCommand.Mobile))
                if (await _userRepository.ExistsAsync(x => x.IsDelete == false && x.Mobile == createUserCommand.Mobile, cancellationToken))
                    return false;

            return true;
        }

    }
}