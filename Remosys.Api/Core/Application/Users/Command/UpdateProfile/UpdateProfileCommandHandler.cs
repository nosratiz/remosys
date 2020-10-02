using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Users.Command.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand,Result>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProfileCommandHandler(IMongoRepository<User> userRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(Guid.Parse(_currentUserService.UserId), cancellationToken);

            if (user is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));

            _mapper.Map(request, user);

            await _userRepository.UpdateAsync(user);

            return Result.SuccessFul();
        }
    }
}