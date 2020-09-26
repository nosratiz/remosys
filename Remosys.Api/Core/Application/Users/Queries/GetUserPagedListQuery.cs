using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Users.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.Users.Queries
{
    public class GetUserPagedListQuery : PagingOptions, IRequest<PagedResult<UserDto>>
    {

    }


    public class GetUserPagedListQueryHandler : IRequestHandler<GetUserPagedListQuery, PagedResult<UserDto>>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public GetUserPagedListQueryHandler(IMongoRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserDto>> Handle(GetUserPagedListQuery request, CancellationToken cancellationToken)
        {
            var users = _userRepository.GetAll(x => x.IsDelete == false);

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                users = users.Where(x => x.FirstName.Contains(request.Query) ||
                                         x.LastName.Contains(request.Query) ||
                                         x.Email.Contains(request.Query) ||
                                         x.Mobile == request.Query);
            }

            var userList = await _userRepository.BrowseAsync(users, request);

            
            return _mapper.Map<PagedResult<UserDto>>(userList);
        }
    }
}