using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Users.Command.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteUserCommandHandler(IMongoRepository<User> userRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.Id, cancellationToken);

            if (user is null)
                return Result.Failed(
                    new NotFoundObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));

            if (user.Roles.Any(x => x.Name == Role.Admin))
                return Result.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.DeleteUserAdminNotAllowed)));

            var currentUser = await _userRepository.GetAsync(Guid.Parse(_currentUserService.UserId), cancellationToken);

            var organization = currentUser.Organizations.Select(x => x.Id).Intersect(user.Organizations.Select(x => x.Id));

            if (_currentUserService.RoleName == Role.Manager && organization.Count() > 0)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.DeleteNotAllowed)));
            

            user.IsDelete = true;

            await _userRepository.UpdateAsync(user);


            return Result.SuccessFul();
        }
    }
}