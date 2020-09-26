using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Users.Command.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMongoRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IMongoRepository<User> userRepository, IMongoRepository<Role> roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            #region Validation

            var role = await _roleRepository.GetAsync(request.RoleId, cancellationToken);

            if (role is null)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.RoleNotFound)));

            var user = await _userRepository.GetAsync(request.Id, cancellationToken);

            if (user is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));

            #endregion

            _mapper.Map(request, user);

            user.Roles = new List<Role> { role };

            await _userRepository.UpdateAsync(user);

            return Result.SuccessFul();
        }
    }
}