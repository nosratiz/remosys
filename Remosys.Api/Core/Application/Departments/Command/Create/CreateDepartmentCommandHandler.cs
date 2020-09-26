using System.Collections.Generic;
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

namespace Remosys.Api.Core.Application.Departments.Command.Create
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result<DepartmentDto>>
    {
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IMongoRepository<Department> _departmentRepository;
        private readonly IMongoRepository<ToolsCategory> _toolCategoryRepository;
        private readonly IMongoRepository<Models.Organization> _organizationRepository;
        private readonly IMapper _mapper;

        public CreateDepartmentCommandHandler(IMongoRepository<Employee> employeeRepository,
            IMongoRepository<Department> departmentRepository, IMapper mapper,
            IMongoRepository<ToolsCategory> toolCategoryRepository, IMongoRepository<Models.Organization> organizationRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _toolCategoryRepository = toolCategoryRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<Result<DepartmentDto>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = _mapper.Map<Department>(request);

            #region Validation 

            if (request.ToolCategoryIds != null)
            {
                List<ToolsCategory> toolsCategories = new List<ToolsCategory>();

                foreach (var id in request.ToolCategoryIds)
                {
                    var category = await _toolCategoryRepository.GetAsync(x => x.IsDeleted == false && x.Id == id, cancellationToken);

                    if (category is null)
                        return Result<DepartmentDto>.Failed(
                            new BadRequestObjectResult(new ApiMessage($"دسته بندی با ایدی {id} یافت نشد")));

                    toolsCategories.Add(category);
                }

                department.ToolsCategories = toolsCategories;
            }

            if (request.EmployeeIds != null)
            {
                List<Employee> employees = new List<Employee>();
                foreach (var id in request.EmployeeIds)
                {
                    var employee = await _employeeRepository.GetAsync(x => x.IsDeleted == false && x.Id == id, cancellationToken);

                    if (employee is null)
                        return Result<DepartmentDto>.Failed(
                            new BadRequestObjectResult(new ApiMessage($"کارمندی با شماره ایدی {id} پیدا نشد")));

                    employees.Add(employee);
                }

                department.Employees = employees;
            }

            if (request.ManagerId.HasValue)
            {
                var manager =
                    await _employeeRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.ManagerId.Value, cancellationToken);

                if (manager is null)
                    return Result<DepartmentDto>.Failed(
                        new BadRequestObjectResult(new ApiMessage("مدیر با ایدی ارسالی پیدا نشد")));

                department.Manager = manager;
            }

            #endregion

            var organization = await _organizationRepository.GetAsync(request.OrganizationId, cancellationToken);

            if (organization is null)
                return Result<DepartmentDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.OrganizationNotFound)));

            

            await _departmentRepository.AddAsync(department);

            return Result<DepartmentDto>.SuccessFul(_mapper.Map<DepartmentDto>(department));
        }
    }
}