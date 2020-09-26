using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Departments.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.Departments.Queries
{
    public class GetDepartmentPagedListQuery : PagingOptions, IRequest<PagedResult<DepartmentDto>>
    {

    }


    public class GetDepartmentPagedListQueryHandler : IRequestHandler<GetDepartmentPagedListQuery, PagedResult<DepartmentDto>>
    {
        private readonly IMongoRepository<Department> _departmentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetDepartmentPagedListQueryHandler(IMongoRepository<Department> departmentRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<DepartmentDto>> Handle(GetDepartmentPagedListQuery request, CancellationToken cancellationToken)
        {
            var department = _departmentRepository.GetAll().Where(x => x.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(request.Query))
                department = department.Where(x => x.Name.Contains(request.Query));

            if (_currentUserService.RoleName != Role.Admin)
                department = department.Where(x => x.Manager.Id == Guid.Parse(_currentUserService.UserId) 
                                                   || x.Organization.Manager.Id == Guid.Parse(_currentUserService.UserId));
            

            var departmentList = await _departmentRepository.BrowseAsync(department, request);

            return _mapper.Map<PagedResult<DepartmentDto>>(departmentList);

        }
    }
}