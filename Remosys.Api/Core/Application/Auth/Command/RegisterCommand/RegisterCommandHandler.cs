using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Mongo;
using Remosys.Common.Result;
using Remosys.Common.Sms;

namespace Remosys.Api.Core.Application.Auth.Command.RegisterCommand
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResult>>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMongoRepository<Role> _roleRepository;
        private readonly IPayamakService _payamakService;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IMongoRepository<User> userRepository, IMapper mapper, IMongoRepository<Role> roleRepository, IPayamakService payamakService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _payamakService = payamakService;
        }

        public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            var role = await _roleRepository.GetAsync(x => x.Name == Role.User, cancellationToken);

            user.Roles = new List<Role> { role };

            await _userRepository.AddAsync(user);

            await _payamakService.SendMessage(request.Mobile, user.ActiveCode);

            return Result<RegisterResult>.SuccessFul(new RegisterResult { Mobile = user.Mobile });
        }
    }
}