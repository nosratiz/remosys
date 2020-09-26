using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Employees.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.Employees.Queries
{
    public class GetEmployeePagedListQuery : PagingOptions, IRequest<PagedResult<EmployeeDto>>
    {
    }

    public class GetEmployeePagedListQueryHandler : IRequestHandler<GetEmployeePagedListQuery, PagedResult<EmployeeDto>>
    {
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetEmployeePagedListQueryHandler(IMongoRepository<Employee> employeeRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<EmployeeDto>> Handle(GetEmployeePagedListQuery request,
            CancellationToken cancellationToken)
        {
            var employee = _employeeRepository.GetAll().Where(x => x.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(request.Query))
                employee = employee.Where(x => x.Code.Contains(request.Query)
                                               || x.User.FirstName.Contains(request.Query)
                                               || x.User.LastName.Contains(request.Query)
                                               || x.User.Email.Contains(request.Query));

            var employeeList = _employeeRepository.BrowseAsync(employee, request);

            return _mapper.Map<PagedResult<EmployeeDto>>(employeeList);
        }
    }
}