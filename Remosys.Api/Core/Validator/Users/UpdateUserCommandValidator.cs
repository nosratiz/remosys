using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Remosys.Api.Core.Application.Users.Command.UpdateUser;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;

namespace Remosys.Api.Core.Validator.Users
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IMongoRepository<User> _userRepository;
        public UpdateUserCommandValidator(IMongoRepository<User> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(dto => dto.Id).NotEmpty();

            RuleFor(dto => dto.FirstName)
                .NotEmpty().WithMessage(ResponseMessage.NameIsRequired)
                .MinimumLength(2);

            RuleFor(dto => dto.LastName)
                .NotEmpty().WithMessage(ResponseMessage.LastNameIsRequired)
                .MinimumLength(2);

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage(ResponseMessage.EmailIsRequired)
                .EmailAddress().WithMessage(ResponseMessage.InvalidEmailFormat);


            RuleFor(dto => dto.RoleId)
                .NotEmpty().WithMessage(ResponseMessage.RoleIsRequired);

            RuleFor(dto => dto)
                .MustAsync(ValidMobile)
                .WithMessage(ResponseMessage.MobileAlreadyExist)
                .MustAsync(ValidEmailAddress)
                .WithMessage(ResponseMessage.EmailAlreadyExist);


        }


        private async Task<bool> ValidEmailAddress(UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
            => !await _userRepository.ExistsAsync(x => x.Email == updateUserCommand.Email && x.Id != updateUserCommand.Id && x.IsDelete == false, cancellationToken);


        private async Task<bool> ValidMobile(UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(updateUserCommand.Mobile))
                if (await _userRepository.ExistsAsync(x => x.IsDelete == false && x.Mobile == updateUserCommand.Mobile && x.Id != updateUserCommand.Id, cancellationToken))
                    return false;

            return true;
        }
    }
}