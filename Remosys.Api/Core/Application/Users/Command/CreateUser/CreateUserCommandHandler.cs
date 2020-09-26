using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeviceDetectorNET;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Users.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;
using Remosys.Common.Sms;

namespace Remosys.Api.Core.Application.Users.Command.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMongoRepository<Role> _roleRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPayamakService _payamakService;
        private readonly IMapper _mapper;


        public CreateUserCommandHandler(IMongoRepository<User> userRepository, IMapper mapper,
            IMongoRepository<Role> roleRepository, ICurrentUserService currentUserService,
            IPayamakService payamakService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _currentUserService = currentUserService;
            _payamakService = payamakService;
        }

        public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.GetAsync(u => u.Id == Guid.Parse(_currentUserService.UserId),
                cancellationToken);

            if (currentUser.Organizations is null)
                return Result<UserDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.NeedOrganization)));

            var user = _mapper.Map<User>(request);
        
            var role = await _roleRepository.GetAsync(x => x.Name == Role.User, cancellationToken);
            user.Roles = new List<Role> { role };
            
            user.Organizations = new List<Models.Organization> { currentUser.Organizations.FirstOrDefault() };

            await _userRepository.AddAsync(user);

            await _payamakService.SendInvitation(request.Mobile, user.Organizations.FirstOrDefault()?.Name, "");

            return Result<UserDto>.SuccessFul(_mapper.Map<UserDto>(user));
        }
    }
}