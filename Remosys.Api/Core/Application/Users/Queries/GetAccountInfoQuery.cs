using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Employees.Dto;
using Remosys.Api.Core.Application.Users.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Users.Queries
{
    public class GetAccountInfoQuery : IRequest<Result<UserDto>>
    {
    }

    public class GetAccountInfoQueryHandler : IRequestHandler<GetAccountInfoQuery, Result<UserDto>>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetAccountInfoQueryHandler(IMongoRepository<User> userRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(Guid.Parse(_currentUserService.UserId), cancellationToken);

            if (user is null)
                return Result<UserDto>.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));

            return Result<UserDto>.SuccessFul(_mapper.Map<UserDto>(user));
        }
    }
}
