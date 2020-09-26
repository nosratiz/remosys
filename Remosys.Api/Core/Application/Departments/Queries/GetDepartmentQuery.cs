using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Departments.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Departments.Queries
{
    public class GetDepartmentQuery : IRequest<Result<DepartmentDto>>
    {
        public Guid Id { get; set; }
    }


    public class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, Result<DepartmentDto>>
    {
        private readonly IMongoRepository<Department> _departmentRepository;
        private readonly IMapper _mapper;

        public GetDepartmentQueryHandler(IMongoRepository<Department> departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<Result<DepartmentDto>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetAsync(request.Id, cancellationToken);

            if (department is null)
                return Result<DepartmentDto>.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.DepartmentNotFound)));

            return Result<DepartmentDto>.SuccessFul(_mapper.Map<DepartmentDto>(department));
        }
    }
}